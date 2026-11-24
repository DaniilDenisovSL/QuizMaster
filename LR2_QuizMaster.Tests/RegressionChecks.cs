using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.Backend.Services;

internal static class RegressionChecks
{
    private static readonly JsonSerializerOptions Json = new() { Converters = { new JsonStringEnumConverter() } };
    private static string folder = "";
    internal static async Task<int> RunAll()
    {
        folder = Path.Combine(AppContext.BaseDirectory, "qa-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        var tests = new (string Name, Func<Task> Test)[]
        {
            ("N01 required category and difficulty", () => Sync(RequiredMetadata)),
            ("N02 null question JSON", () => Sync(() => InvalidBank("[null]"))),
            ("N03 null answer JSON", () => Sync(NullAnswer)),
            ("N04 question bank owns answer collections", () => Sync(ImmutableBank)),
            ("N05 null leaderboard entries", NullRecords),
            ("N06 invalid records, warning and recovery backup", InvalidRecords),
            ("N07 duplicate and interrupted records on load", DuplicateRecords),
            ("N08 invalid result rejected on save", RejectInvalidSave),
            ("N09 score overflow rejected before starting", () => Sync(ScoreOverflow)),
            ("N10 excessive deadline preserves current session", () => Sync(ExcessiveDeadline)),
            ("N11 invalid request preserves current session", () => Sync(InvalidRequest)),
            ("N12 invalid answers and premature next", () => Sync(InvalidAnswers)),
            ("N13 exact deadline and late clicks", () => Sync(Deadline)),
            ("N14 concurrent duplicate answer", ConcurrentAnswers),
            ("P01 concurrent saves through separate stores", ConcurrentSaves),
            ("N15 missing, malformed and empty bank", () => Sync(MalformedBanks)),
            ("N16 cancelled save leaves existing file intact", CancelledSave),
            ("P02 trimmed name, zero points and result accuracy", () => Sync(NameAndScore)),
            ("P03 leaderboard score, accuracy and date ordering", SortedRecords),
            ("N17 invalid configuration and catalog values", () => Sync(InvalidConfiguration)),
        };
        int failed = 0;
        try
        {
            foreach (var (name, test) in tests)
            {
                try { await test(); Console.WriteLine("PASS " + name); }
                catch (Exception ex) { failed++; Console.WriteLine("FAIL " + name + ": " + ex.Message); }
            }
        }
        finally { Directory.Delete(folder, recursive: true); }
        Console.WriteLine($"Regression scenarios: {tests.Length}, passed: {tests.Length - failed}, failed: {failed}.");
        return failed == 0 ? 0 : 1;
    }

    private static Task Sync(Action test) { test(); return Task.CompletedTask; }
    private static string PathFor(string name) => Path.Combine(folder, name + ".json");
    private static Question Sample(string id = "q1") => new(id, "Question", QuizCategory.Games, Difficulty.Easy,
        Enumerable.Range(1, 4).Select(i => new Answer(id + "a" + i, "Answer " + i)).ToArray(), id + "a1");
    private static QuizRequest Request(QuizOptions? options = null) => new(" Player ", QuizCategory.Games, Difficulty.Easy,
        options ?? new QuizOptions { QuestionCount = 2, ShuffleAnswers = false, ShuffleQuestions = false });
    private static QuizResult Record() => new(Guid.NewGuid(), "Player", QuizCategory.Games, Difficulty.Easy,
        100, 1, 2, 2, .5, 1, GameEndReason.Completed, DateTimeOffset.UtcNow);
    private static (QuizSession Session, Clock Clock) Session()
    {
        var clock = new Clock();
        var session = new QuizSession(new QuestionBank([Sample(), Sample("q2")]), clock);
        session.Start(Request());
        return (session, clock);
    }
    private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); } catch (T) { return; }
        throw new Exception("Expected " + typeof(T).Name);
    }
    private static async Task ThrowsAsync<T>(Func<Task> action) where T : Exception
    {
        try { await action(); } catch (T) { return; }
        throw new Exception("Expected " + typeof(T).Name);
    }
    private static JsonObject QuestionJson() => JsonSerializer.SerializeToNode(Sample(), Json)!.AsObject();
    private static void InvalidBank(string json)
    {
        var path = PathFor("bank-" + Guid.NewGuid()); File.WriteAllText(path, json);
        Throws<InvalidDataException>(() => QuestionBank.Load(path));
    }
    private static void RequiredMetadata()
    {
        foreach (var field in new[] { "Category", "Difficulty" })
        {
            var question = QuestionJson(); question.Remove(field);
            InvalidBank(new JsonArray(question).ToJsonString());
            question = QuestionJson(); question[field] = null;
            InvalidBank(new JsonArray(question).ToJsonString());
        }
    }
    private static void NullAnswer()
    {
        var question = QuestionJson(); question["Answers"]!.AsArray()[0] = null;
        InvalidBank(new JsonArray(question).ToJsonString());
    }
    private static void ImmutableBank()
    {
        var question = Sample(); var answers = (Answer[])question.Answers;
        var bank = new QuestionBank([question]);
        answers[0] = new Answer("tampered", "Tampered");
        Check(bank.Questions[0].Answers[0].Id == "q1a1", "caller changed validated bank");
        var session = new QuizSession(bank); session.Start(Request(new QuizOptions { QuestionCount = 1 }));
        Check(session.GetState().CurrentQuestion!.Answers.Any(a => a.Id == "q1a1"), "correct answer removed");
        Throws<NotSupportedException>(() => ((IList<Answer>)bank.Questions[0].Answers)[0] = answers[0]);
    }
    private static async Task NullRecords()
    {
        var path = PathFor("null-record"); File.WriteAllText(path, "[null]");
        var loaded = await new LeaderboardStore(path).LoadAsync();
        Check(loaded.Records.Count == 0 && loaded.Warning != null, "null record needs warning");
    }
    private static IEnumerable<QuizResult> BadRecords()
    {
        var r = Record();
        return [r with { Category = (QuizCategory)99 }, r with { Difficulty = (Difficulty)99 },
            r with { EndReason = (GameEndReason)99 }, r with { Score = -1 }, r with { QuestionsPlayed = 3 },
            r with { CorrectAnswers = 3 }, r with { Accuracy = 1.5 }, r with { Accuracy = .1 },
            r with { BestStreak = 2 }, r with { BestStreak = -1 }, r with { SessionId = Guid.Empty },
            r with { PlayerName = " " }, r with { FinishedAtUtc = default }, r with { QuestionsPlayed = 1, Accuracy = 1 }];
    }
    private static async Task InvalidRecords()
    {
        var valid = Record(); var path = PathFor("invalid-records");
        File.WriteAllText(path, JsonSerializer.Serialize(BadRecords().Prepend(valid), Json));
        var store = new LeaderboardStore(path); var loaded = await store.LoadAsync();
        Check(loaded.Records.Count == 1 && loaded.Records[0] == valid && loaded.Warning != null, "invalid records retained or warning missing");
        var oldBytes = File.ReadAllBytes(path); Check(await store.SaveAsync(Record()), "new record not saved");
        var backup = Directory.GetFiles(folder, "invalid-records.json.*.bak").Single();
        Check(oldBytes.SequenceEqual(File.ReadAllBytes(backup)), "backup did not preserve damaged data");
        Check((await store.LoadAsync()).Records.Count == 2, "valid records lost on recovery");
    }
    private static async Task DuplicateRecords()
    {
        var r = Record(); var path = PathFor("duplicates");
        File.WriteAllText(path, JsonSerializer.Serialize(new[] { r, r, r with { SessionId = Guid.NewGuid(), EndReason = GameEndReason.Interrupted } }, Json));
        var loaded = await new LeaderboardStore(path).LoadAsync();
        Check(loaded.Records.Count == 1 && loaded.Warning != null, "duplicate or interrupted result retained");
    }
    private static async Task RejectInvalidSave()
    {
        var path = PathFor("reject-save"); var store = new LeaderboardStore(path);
        Check(await store.SaveAsync(Record()), "valid record rejected"); var original = File.ReadAllBytes(path);
        foreach (var r in BadRecords().Append(Record() with { Accuracy = double.NaN }))
            await ThrowsAsync<ArgumentException>(() => store.SaveAsync(r));
        Check(original.SequenceEqual(File.ReadAllBytes(path)), "invalid save changed file");
    }
    private static void ScoreOverflow()
    {
        var (session, _) = Session(); var before = session.GetState();
        Throws<ArgumentOutOfRangeException>(() => session.Start(Request(new QuizOptions { QuestionCount = 2, PointsPerCorrectAnswer = int.MaxValue })));
        Check(session.GetState().SessionId == before.SessionId, "invalid restart changed session");
    }
    private static void ExcessiveDeadline()
    {
        var (session, _) = Session(); var before = session.GetState();
        Throws<ArgumentOutOfRangeException>(() => session.Start(Request(new QuizOptions { QuestionCount = 2, TimePerQuestion = TimeSpan.MaxValue })));
        Check(session.GetState().SessionId == before.SessionId, "deadline failure replaced session");
    }
    private static void InvalidRequest()
    {
        var (session, _) = Session(); var before = session.GetState();
        Throws<ArgumentException>(() => session.Start(Request() with { PlayerName = "  " }));
        Throws<InsufficientQuestionsException>(() => session.Start(Request(new QuizOptions { QuestionCount = 3 })));
        Check(session.GetState().SessionId == before.SessionId && session.GetState().Score == 0, "failed start corrupted game");
    }
    private static void InvalidAnswers()
    {
        var (session, _) = Session(); var state = session.GetState(); var t = state.CurrentQuestion!;
        Check(!session.NextQuestion(state.SessionId, t.QuestionToken), "next before answer accepted");
        foreach (var id in new[] { "unknown", "", null! })
            Check(session.SubmitAnswer(state.SessionId, t.QuestionToken, id).Disposition == AnswerDisposition.InvalidAnswer, "invalid answer accepted");
        Check(session.GetState().Lives == 3 && session.GetState().Score == 0, "invalid answer changed state");
        Check(session.SubmitAnswer(Guid.NewGuid(), t.QuestionToken, "q1a1").Disposition == AnswerDisposition.StaleSession, "foreign session accepted");
        Check(session.SubmitAnswer(state.SessionId, Guid.NewGuid(), "q1a1").Disposition == AnswerDisposition.StaleQuestion, "foreign token accepted");
    }
    private static void Deadline()
    {
        var (session, clock) = Session(); var state = session.GetState(); var t = state.CurrentQuestion!;
        clock.Advance(TimeSpan.FromSeconds(20));
        Check(session.SubmitAnswer(state.SessionId, t.QuestionToken, "q1a1").Disposition == AnswerDisposition.TimedOut, "answer at deadline counted");
        session.SubmitAnswer(state.SessionId, t.QuestionToken, "q1a1");
        Check(session.GetState().Lives == 2 && session.GetState().Score == 0, "late clicks changed score/lives");
    }
    private static async Task ConcurrentAnswers()
    {
        var (session, _) = Session(); var state = session.GetState(); var t = state.CurrentQuestion!;
        var outcomes = await Task.WhenAll(Enumerable.Range(0, 20).Select(_ => Task.Run(() => session.SubmitAnswer(state.SessionId, t.QuestionToken, "q1a1"))));
        Check(outcomes.Count(o => o.WasAccepted) == 1 && session.GetState().Score == 100, "duplicate concurrent answer counted");
    }
    private static async Task ConcurrentSaves()
    {
        var path = PathFor("concurrent"); var r = Record();
        var outcomes = await Task.WhenAll(Enumerable.Range(0, 20).Select(_ => new LeaderboardStore(path).SaveAsync(r)));
        Check(outcomes.Count(v => v) == 1, "same session saved twice");
        await Task.WhenAll(Enumerable.Range(0, 12).Select(_ => new LeaderboardStore(path).SaveAsync(Record())));
        Check((await new LeaderboardStore(path).LoadAsync()).Records.Count == 13, "concurrent unique records lost");
    }
    private static void MalformedBanks()
    {
        Throws<FileNotFoundException>(() => QuestionBank.Load(PathFor("missing")));
        foreach (var json in new[] { "", "{broken", "null", "[]" }) InvalidBank(json);
        var q = Sample(); Throws<InvalidDataException>(() => new QuestionBank([q, q]));
        Throws<InvalidDataException>(() => new QuestionBank([q with { CorrectAnswerId = "missing" }]));
        Throws<InvalidDataException>(() => new QuestionBank([q with { Answers = q.Answers.Take(3).ToArray() }]));
        Throws<InvalidDataException>(() => new QuestionBank([q with { Answers = [q.Answers[0], q.Answers[0], q.Answers[2], q.Answers[3]] }]));
        Throws<InvalidDataException>(() => new QuestionBank([q with { Category = (QuizCategory)99 }]));
    }
    private static async Task CancelledSave()
    {
        var path = PathFor("cancelled"); var store = new LeaderboardStore(path);
        await store.SaveAsync(Record()); var original = File.ReadAllBytes(path);
        using var cts = new CancellationTokenSource(); cts.Cancel();
        await ThrowsAsync<OperationCanceledException>(() => store.SaveAsync(Record(), cts.Token));
        Check(original.SequenceEqual(File.ReadAllBytes(path)), "cancelled save changed file");
    }
    private static void NameAndScore()
    {
        var session = new QuizSession(new QuestionBank([Sample()]));
        session.Start(Request(new QuizOptions { QuestionCount = 1, PointsPerCorrectAnswer = 0 }));
        var state = session.GetState(); session.SubmitAnswer(state.SessionId, state.CurrentQuestion!.QuestionToken, "q1a1");
        var result = session.GetState().Result!;
        Check(result.PlayerName == "Player" && result.Score == 0 && result.Accuracy == 1 && result.CorrectAnswers == 1, "trimmed name/zero score result incorrect");
    }
    private static async Task SortedRecords()
    {
        var store = new LeaderboardStore(PathFor("sorting")); var a = Record() with { Score = 200 };
        var b = Record() with { Score = 100, CorrectAnswers = 2, Accuracy = 1 };
        var c = Record() with { Score = 100, FinishedAtUtc = DateTimeOffset.UtcNow.AddDays(-1) };
        var d = Record() with { Score = 100, FinishedAtUtc = DateTimeOffset.UtcNow };
        foreach (var r in new[] { c, d, b, a }) await store.SaveAsync(r);
        Check((await store.GetSortedAsync()).Select(r => r.SessionId).SequenceEqual(new[] { a.SessionId, b.SessionId, d.SessionId, c.SessionId }), "ranking order incorrect");
        Check((await store.GetSortedAsync(QuizCategory.Movies)).Count == 0, "category filter incorrect");
    }
    private static void InvalidConfiguration()
    {
        foreach (var options in new[] { new QuizOptions { QuestionCount = 0 }, new QuizOptions { StartingLives = 0 }, new QuizOptions { TimePerQuestion = TimeSpan.Zero }, new QuizOptions { PointsPerCorrectAnswer = -1 } })
            Throws<ArgumentOutOfRangeException>(options.Validate);
        Throws<ArgumentException>(() => QuizCatalog.ParseCategory("unknown"));
        Check(!QuizCatalog.TryParseDifficulty(null, out _), "null difficulty parsed");
        var bank = new QuestionBank([Sample()]);
        Throws<ArgumentOutOfRangeException>(() => bank.EnsureCanStart((QuizCategory)99, Difficulty.Easy, 1));
        Throws<ArgumentOutOfRangeException>(() => bank.EnsureCanStart(QuizCategory.Games, (Difficulty)99, 1));
        Throws<ArgumentOutOfRangeException>(() => bank.EnsureCanStart(QuizCategory.Games, Difficulty.Easy, 0));
    }
    private sealed class Clock : TimeProvider
    {
        private DateTimeOffset now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        public override DateTimeOffset GetUtcNow() => now;
        public void Advance(TimeSpan delta) => now += delta;
    }
}
