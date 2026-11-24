using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.Backend.Services;

namespace LR2_QuizMaster.Tests;

internal static class Program
{
    private static int passed;
    private static async Task<int> Main()
    {
        Run("JSON question bank and localized labels", Bank);
        Run("year questions contain four years and answer sets stay attached", BankAnswerSets);
        Run("correct/wrong scoring and lives", CorrectWrongAndLives);
        Run("timeout uses injected clock once", Timeout);
        Run("last life ends session", LastLife);
        Run("last question completes session", LastQuestion);
        Run("streak and best streak", Streaks);
        Run("duplicate and stale answers are ignored", DuplicateAndStale);
        Run("shuffle preserves bank and answer ids", Shuffle);
        Run("restart clears all state", Restart);
        await RunAsync("leaderboard saves, filters, deduplicates and handles corruption", Leaderboard);
        Console.WriteLine($"Passed {passed} backend checks.");
        return await RegressionChecks.RunAll();
    }

    private static void Bank()
    {
        var bank = QuestionBank.Load(Path.Combine(AppContext.BaseDirectory, "Data", "questions.json"));
        Equal(150, bank.Questions.Count);
        foreach (var category in Enum.GetValues<QuizCategory>())
            foreach (var difficulty in Enum.GetValues<Difficulty>())
                Equal(10, bank.Find(category, difficulty).Count);
        Equal(QuizCategory.GeneralKnowledge, QuizCatalog.ParseCategory("  Загальні знання "));
        Equal(Difficulty.Hard, QuizCatalog.ParseDifficulty("Складна"));
    }

    private static void BankAnswerSets()
    {
        var bank = QuestionBank.Load(Path.Combine(AppContext.BaseDirectory, "Data", "questions.json"));
        var years = bank.Questions.Where(q => q.Text.Contains("У якому році"));
        True(years.Any());
        foreach (var q in years)
            True(q.Answers.All(a => System.Text.RegularExpressions.Regex.IsMatch(a.Text, @"^\d{4}$")));
        foreach (var q in bank.Questions)
            Equal(4, q.Answers.Select(a => a.Text).Distinct(StringComparer.OrdinalIgnoreCase).Count());
        foreach (var category in Enum.GetValues<QuizCategory>())
        foreach (var difficulty in Enum.GetValues<Difficulty>())
        foreach (bool shuffle in new[] { false, true })
        {
            var session = new QuizSession(bank);
            session.Start(new QuizRequest("Перевірка", category, difficulty,
                new QuizOptions { ShuffleQuestions = shuffle, ShuffleAnswers = shuffle, RandomSeed = 42 }));
            var seen = new HashSet<string>();
            while (session.GetState() is { Status: SessionStatus.QuestionActive, CurrentQuestion: { } ticket } state)
            {
                True(seen.Add(ticket.QuestionId));
                var source = bank.Questions.Single(q => q.Id == ticket.QuestionId);
                Equal(source.Text, ticket.Text);
                SequenceEqual(source.Answers.OrderBy(a => a.Id), ticket.Answers.OrderBy(a => a.Id));
                session.SubmitAnswer(state.SessionId, ticket.QuestionToken, source.CorrectAnswerId);
                if (session.GetState().Status == SessionStatus.AwaitingNext)
                    True(session.NextQuestion(state.SessionId, ticket.QuestionToken));
            }
            Equal(10, seen.Count);
            Equal(10, session.GetState().Result!.CorrectAnswers);
        }
    }

    private static void CorrectWrongAndLives()
    {
        var (session, _, _) = Start(count: 3);
        var state = session.GetState();
        var ticket = state.CurrentQuestion!;
        var correct = ticket.Answers.Single(a => a.Id == Questions[0].CorrectAnswerId);
        var result = session.SubmitAnswer(state.SessionId, ticket.QuestionToken, correct.Id);
        Equal(AnswerDisposition.Correct, result.Disposition);
        Equal(100, result.Score);
        True(session.NextQuestion(state.SessionId, ticket.QuestionToken));
        state = session.GetState();
        ticket = state.CurrentQuestion!;
        var wrong = ticket.Answers.First(a => a.Id != Questions[1].CorrectAnswerId);
        result = session.SubmitAnswer(state.SessionId, ticket.QuestionToken, wrong.Id);
        Equal(AnswerDisposition.Incorrect, result.Disposition);
        Equal(2, result.Lives);
        Equal(0, result.CurrentStreak);
        Equal(100, result.Score);
    }

    private static void Timeout()
    {
        var (session, clock, _) = Start(count: 2);
        clock.Advance(TimeSpan.FromSeconds(20));
        var state = session.GetState();
        Equal(SessionStatus.AwaitingNext, state.Status);
        Equal(AnswerDisposition.TimedOut, state.LastAnswer!.Disposition);
        Equal(TimeSpan.Zero, state.RemainingTime);
        Equal(2, state.Lives);
        var again = session.GetState();
        Equal(2, again.Lives);
        Equal(1, again.LastAnswer?.Disposition == AnswerDisposition.TimedOut ? 1 : 0);
    }

    private static void LastLife()
    {
        var (session, _, _) = Start(count: 4, lives: 1);
        var state = session.GetState();
        var ticket = state.CurrentQuestion!;
        var wrong = ticket.Answers.First(x => x.Id != Questions[0].CorrectAnswerId);
        session.SubmitAnswer(state.SessionId, ticket.QuestionToken, wrong.Id);
        state = session.GetState();
        Equal(SessionStatus.Finished, state.Status);
        Equal(GameEndReason.LivesExhausted, state.Result!.EndReason);
        Equal(1, state.Result.QuestionsPlayed);
        Equal(0d, state.Result.Accuracy);
    }

    private static void LastQuestion()
    {
        var (session, _, _) = Start(count: 1);
        var state = session.GetState();
        var ticket = state.CurrentQuestion!;
        session.SubmitAnswer(state.SessionId, ticket.QuestionToken, Questions[0].CorrectAnswerId);
        state = session.GetState();
        Equal(SessionStatus.Finished, state.Status);
        Equal(GameEndReason.Completed, state.Result!.EndReason);
        Equal(1, state.Result.CorrectAnswers);
        Equal(1d, state.Result.Accuracy);
    }

    private static void Streaks()
    {
        var (session, _, _) = Start(count: 4);
        for (var i = 0; i < 4; i++)
        {
            var state = session.GetState();
            var ticket = state.CurrentQuestion!;
            var answerId = i < 3 ? Questions[i].CorrectAnswerId : ticket.Answers.First(a => a.Id != Questions[i].CorrectAnswerId).Id;
            var outcome = session.SubmitAnswer(state.SessionId, ticket.QuestionToken, answerId);
            if (i < 3) True(session.NextQuestion(state.SessionId, ticket.QuestionToken));
            else
            {
                Equal(3, outcome.BestStreak);
                Equal(0, outcome.CurrentStreak);
            }
        }
        Equal(3, session.GetState().Result!.BestStreak);
    }

    private static void DuplicateAndStale()
    {
        var (session, _, _) = Start(count: 3);
        var first = session.GetState();
        var ticket = first.CurrentQuestion!;
        var accepted = session.SubmitAnswer(first.SessionId, ticket.QuestionToken, Questions[0].CorrectAnswerId);
        var duplicate = session.SubmitAnswer(first.SessionId, ticket.QuestionToken, Questions[0].CorrectAnswerId);
        Equal(AnswerDisposition.AlreadyAnswered, duplicate.Disposition);
        Equal(100, duplicate.Score);
        True(session.NextQuestion(first.SessionId, ticket.QuestionToken));
        var second = session.GetState();
        var staleQuestion = session.SubmitAnswer(second.SessionId, ticket.QuestionToken, Questions[1].CorrectAnswerId);
        Equal(AnswerDisposition.StaleQuestion, staleQuestion.Disposition);
        var oldSession = first.SessionId;
        session.Start(Request(3));
        var newState = session.GetState();
        var staleSession = session.SubmitAnswer(oldSession, newState.CurrentQuestion!.QuestionToken, Questions[0].CorrectAnswerId);
        Equal(AnswerDisposition.StaleSession, staleSession.Disposition);
        Equal(0, session.GetState().Score);
        Equal(3, session.GetState().Lives);
    }

    private static void Shuffle()
    {
        var unrelatedEasy = new Question("other-easy", "Стороння легка тема", QuizCategory.Games, Difficulty.Easy,
            Enumerable.Range(1, 4).Select(i => new Answer($"other-easy-a{i}", $"Стороння відповідь {i}")).ToArray(), "other-easy-a1");
        var unrelatedDifficulty = new Question("other-hard", "Стороння складна тема", QuizCategory.Programming, Difficulty.Hard,
            Enumerable.Range(1, 4).Select(i => new Answer($"other-hard-a{i}", $"Складна відповідь {i}")).ToArray(), "other-hard-a1");
        var bank = new QuestionBank(Questions.Concat([unrelatedEasy, unrelatedDifficulty]));
        var originalQuestionIds = bank.Questions.Select(x => x.Id).ToArray();
        var originalAnswers = bank.Questions.ToDictionary(q => q.Id, q => q.Answers.ToArray());
        var eligibleIds = bank.Find(QuizCategory.Programming, Difficulty.Easy).Select(q => q.Id).ToHashSet(StringComparer.Ordinal);
        var shuffledClock = new ManualClock(DateTimeOffset.Parse("2026-01-01T00:00:00Z"));
        var shuffled = new QuizSession(bank, shuffledClock);
        shuffled.Start(new QuizRequest("Гравець", QuizCategory.Programming, Difficulty.Easy,
            new QuizOptions { QuestionCount = 10, ShuffleQuestions = true, ShuffleAnswers = true, RandomSeed = 44 }));
        var shuffledQuestionOrder = new List<string>();
        var answerOrderChanged = false;
        var seen = new HashSet<string>(StringComparer.Ordinal);
        while (shuffled.GetState() is { Status: SessionStatus.QuestionActive, CurrentQuestion: { } ticket } state)
        {
            True(seen.Add(ticket.QuestionId));
            True(eligibleIds.Contains(ticket.QuestionId));
            shuffledQuestionOrder.Add(ticket.QuestionId);
            var source = bank.Questions.Single(q => q.Id == ticket.QuestionId);
            Equal(source.Text, ticket.Text);
            SequenceEqual(source.Answers.OrderBy(a => a.Id), ticket.Answers.OrderBy(a => a.Id));
            answerOrderChanged |= !source.Answers.SequenceEqual(ticket.Answers);

            var repeatedRead = shuffled.GetState().CurrentQuestion!;
            Equal(ticket.QuestionId, repeatedRead.QuestionId);
            SequenceEqual(ticket.Answers, repeatedRead.Answers);
            shuffledClock.Advance(TimeSpan.FromSeconds(1));
            var timerRefresh = shuffled.GetState();
            Equal(ticket.QuestionId, timerRefresh.CurrentQuestion!.QuestionId);
            SequenceEqual(ticket.Answers, timerRefresh.CurrentQuestion.Answers);
            True(timerRefresh.RemainingTime < state.RemainingTime);
            var outcome = shuffled.SubmitAnswer(state.SessionId, ticket.QuestionToken, source.CorrectAnswerId);
            Equal(AnswerDisposition.Correct, outcome.Disposition);
            Equal(source.CorrectAnswerId, outcome.CorrectAnswerId);
            SequenceEqual(ticket.Answers, shuffled.GetState().CurrentQuestion!.Answers);
            if (shuffled.GetState().Status == SessionStatus.AwaitingNext)
                True(shuffled.NextQuestion(state.SessionId, ticket.QuestionToken));
        }
        Equal(10, seen.Count);
        True(!shuffledQuestionOrder.SequenceEqual(bank.Find(QuizCategory.Programming, Difficulty.Easy).Take(10).Select(q => q.Id)));
        True(answerOrderChanged);
        Equal(10, shuffled.GetState().Result!.CorrectAnswers);
        SequenceEqual(originalQuestionIds, bank.Questions.Select(x => x.Id));
        foreach (var question in bank.Questions)
            SequenceEqual(originalAnswers[question.Id], question.Answers);

        var fixedOrder = new QuizSession(bank, new ManualClock(DateTimeOffset.Parse("2026-01-01T00:00:00Z")));
        fixedOrder.Start(new QuizRequest("Гравець", QuizCategory.Programming, Difficulty.Easy,
            new QuizOptions { QuestionCount = 10, ShuffleQuestions = false, ShuffleAnswers = false, RandomSeed = 44 }));
        var fixedIndex = 0;
        while (fixedOrder.GetState() is { Status: SessionStatus.QuestionActive, CurrentQuestion: { } fixedTicket } fixedState)
        {
            var source = bank.Find(QuizCategory.Programming, Difficulty.Easy)[fixedIndex++];
            Equal(source.Id, fixedTicket.QuestionId);
            SequenceEqual(source.Answers, fixedTicket.Answers);
            Equal(AnswerDisposition.Correct,
                fixedOrder.SubmitAnswer(fixedState.SessionId, fixedTicket.QuestionToken, source.CorrectAnswerId).Disposition);
            if (fixedOrder.GetState().Status == SessionStatus.AwaitingNext)
                True(fixedOrder.NextQuestion(fixedState.SessionId, fixedTicket.QuestionToken));
        }
        Equal(10, fixedIndex);
    }

    private static void Restart()
    {
        var (session, _, _) = Start(count: 3);
        var first = session.GetState();
        var ticket = first.CurrentQuestion!;
        var wrong = ticket.Answers.First(x => x.Id != Questions[0].CorrectAnswerId);
        session.SubmitAnswer(first.SessionId, ticket.QuestionToken, wrong.Id);
        var oldSessionId = first.SessionId;
        var newSessionId = session.Start(Request(3));
        True(oldSessionId != newSessionId);
        var restarted = session.GetState();
        Equal(0, restarted.Score);
        Equal(3, restarted.Lives);
        Equal(0, restarted.CurrentStreak);
        Equal(0, restarted.BestStreak);
        Equal(0, restarted.LastAnswer is null ? 0 : 1);
    }

    private static async Task Leaderboard()
    {
        var folder = Path.Combine(AppContext.BaseDirectory, "QuizMaster-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        try
        {
            var path = Path.Combine(folder, "leaderboard.json");
            var store = new LeaderboardStore(path);
            Equal(0, (await store.LoadAsync()).Records.Count);

            var record = new QuizResult(Guid.NewGuid(), "Олена", QuizCategory.Games, Difficulty.Easy,
                300, 3, 4, 4, 0.75, 3, GameEndReason.Completed, DateTimeOffset.UtcNow);
            True(await store.SaveAsync(record));
            True(!await store.SaveAsync(record));
            var interrupted = record with { SessionId = Guid.NewGuid(), EndReason = GameEndReason.Interrupted };
            True(!await store.SaveAsync(interrupted));
            Equal(1, (await store.GetSortedAsync(QuizCategory.Games)).Count);
            Equal(0, (await store.GetSortedAsync(QuizCategory.Movies)).Count);

            await File.WriteAllTextAsync(path, "{ definitely not JSON");
            var corrupt = await store.LoadAsync();
            True(corrupt.Warning is not null);
            Equal(0, corrupt.Records.Count);
            var replacement = record with { SessionId = Guid.NewGuid(), Score = 50 };
            True(await store.SaveAsync(replacement));
            True(Directory.GetFiles(folder, "*.bak").Length == 1);
            Equal(1, (await store.LoadAsync()).Records.Count);
        }
        finally { Directory.Delete(folder, recursive: true); }
    }

    private static (QuizSession Session, ManualClock Clock, QuestionBank Bank) Start(int count, int lives = 3)
    {
        var bank = MakeBank();
        var clock = new ManualClock(DateTimeOffset.Parse("2026-01-01T00:00:00Z"));
        var session = new QuizSession(bank, clock);
        session.Start(Request(count, lives));
        return (session, clock, bank);
    }

    private static QuizRequest Request(int count, int lives = 3) => new("Тест", QuizCategory.Programming, Difficulty.Easy,
        new QuizOptions { QuestionCount = count, StartingLives = lives, TimePerQuestion = TimeSpan.FromSeconds(20),
            PointsPerCorrectAnswer = 100, ShuffleQuestions = false, ShuffleAnswers = false });

    private static QuestionBank MakeBank() => new(Questions);

    private static readonly Question[] Questions = Enumerable.Range(1, 12).Select(i =>
    {
        var id = $"q{i:000}";
        var answers = Enumerable.Range(1, 4).Select(j => new Answer($"a{i:000}-{j}", $"Відповідь {i}.{j}")).ToArray();
        return new Question(id, $"Питання {i}", QuizCategory.Programming, Difficulty.Easy, answers, answers[0].Id);
    }).ToArray();

    private static void Run(string name, Action test)
    {
        test();
        passed++;
        Console.WriteLine($"PASS {name}");
    }

    private static async Task RunAsync(string name, Func<Task> test)
    {
        await test();
        passed++;
        Console.WriteLine($"PASS {name}");
    }

    private static void True(bool value)
    {
        if (!value) throw new InvalidOperationException("Expected true.");
    }

    private static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new InvalidOperationException($"Expected <{expected}> but got <{actual}>.");
    }

    private static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
        if (!expected.SequenceEqual(actual))
            throw new InvalidOperationException("Sequences differ.");
    }

    private sealed class ManualClock(DateTimeOffset now) : TimeProvider
    {
        private DateTimeOffset current = now;
        public override DateTimeOffset GetUtcNow() => current;
        public void Advance(TimeSpan amount) => current += amount;
    }
}



