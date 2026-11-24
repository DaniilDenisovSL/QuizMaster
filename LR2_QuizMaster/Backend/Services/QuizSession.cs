using LR2_QuizMaster.Backend.Domain;

namespace LR2_QuizMaster.Backend.Services;

/// <summary>
/// Thread-safe single-game state machine. Time is read from TimeProvider; no background timer,
/// UI control, event subscription, or mutable reference to the source bank is retained.
/// </summary>
public sealed class QuizSession
{
    private readonly object gate = new();
    private readonly QuestionBank bank;
    private readonly TimeProvider timeProvider;
    private QuizRequest? request;
    private QuizOptions? options;
    private List<Round> rounds = [];
    private int index = -1, score, lives, currentStreak, bestStreak, correctAnswers, questionsPlayed;
    private DateTimeOffset deadline;
    private SessionStatus status = SessionStatus.NotStarted;
    private AnswerOutcome? lastAnswer;
    private QuizResult? result;
    private Guid sessionId;

    public QuizSession(QuestionBank bank, TimeProvider? timeProvider = null)
    {
        this.bank = bank ?? throw new ArgumentNullException(nameof(bank));
        this.timeProvider = timeProvider ?? TimeProvider.System;
    }

    public Guid Start(QuizRequest newRequest)
    {
        ArgumentNullException.ThrowIfNull(newRequest);
        var player = newRequest.PlayerName?.Trim();
        if (string.IsNullOrWhiteSpace(player)) throw new ArgumentException("Вкажіть ім’я гравця.", nameof(newRequest));
        var chosenOptions = newRequest.Options ?? new QuizOptions();
        chosenOptions.Validate();
        bank.EnsureCanStart(newRequest.Category, newRequest.Difficulty, chosenOptions.QuestionCount);

        lock (gate)
        {
            if (chosenOptions.TimePerQuestion > DateTimeOffset.MaxValue - timeProvider.GetUtcNow())
                throw new ArgumentOutOfRangeException(nameof(newRequest), "Час на питання перевищує допустимий діапазон дат.");
            // Replacing a running game is an explicit restart. New ids invalidate late UI callbacks.
            sessionId = Guid.NewGuid();
            request = newRequest with { PlayerName = player, Options = chosenOptions };
            options = chosenOptions;
            var random = chosenOptions.RandomSeed is int seed ? new Random(seed) : new Random();
            var eligible = bank.Find(newRequest.Category, newRequest.Difficulty).ToList();
            if (chosenOptions.ShuffleQuestions) Shuffle(eligible, random);
            rounds = eligible.Take(chosenOptions.QuestionCount)
                .Select(q => new Round(q, chosenOptions.ShuffleAnswers ? ShuffledAnswers(q, random) : q.Answers.ToArray()))
                .ToList();
            index = 0;
            score = correctAnswers = questionsPlayed = currentStreak = bestStreak = 0;
            lives = chosenOptions.StartingLives;
            lastAnswer = null;
            result = null;
            status = SessionStatus.QuestionActive;
            BeginQuestion();
            return sessionId;
        }
    }

    public QuizState GetState()
    {
        lock (gate)
        {
            if (status == SessionStatus.QuestionActive && timeProvider.GetUtcNow() >= deadline)
                CommitMiss(AnswerDisposition.TimedOut, null);
            var remaining = status == SessionStatus.QuestionActive ? deadline - timeProvider.GetUtcNow() : TimeSpan.Zero;
            if (remaining < TimeSpan.Zero) remaining = TimeSpan.Zero;
            var current = index >= 0 && index < rounds.Count ? rounds[index] : null;
            return new QuizState(
                sessionId, status,
                current is null ? null : new QuestionTicket(sessionId, current.Token, current.Question.Id, current.Question.Text, Array.AsReadOnly(current.Answers)),
                current is null ? 0 : index + 1, rounds.Count, remaining,
                score, lives, currentStreak, bestStreak, lastAnswer, result);
        }
    }

    public AnswerOutcome SubmitAnswer(Guid requestedSessionId, Guid questionToken, string answerId)
    {
        lock (gate)
        {
            if (status == SessionStatus.NotStarted)
                return Outcome(AnswerDisposition.SessionNotActive, null, false);
            if (requestedSessionId != sessionId)
                return Outcome(AnswerDisposition.StaleSession, null, false);
            var current = CurrentRound();
            if (current is null || questionToken != current.Token)
                return Outcome(AnswerDisposition.StaleQuestion, null, false);
            if (status == SessionStatus.AwaitingNext)
                return Outcome(AnswerDisposition.AlreadyAnswered, answerId, false);
            if (status is SessionStatus.Finished or SessionStatus.Interrupted)
                return Outcome(AnswerDisposition.SessionNotActive, answerId, false);
            if (!current.Answers.Any(a => string.Equals(a.Id, answerId, StringComparison.Ordinal)))
                return Outcome(AnswerDisposition.InvalidAnswer, answerId, false);

            if (timeProvider.GetUtcNow() >= deadline)
            {
                CommitMiss(AnswerDisposition.TimedOut, null);
                return lastAnswer!;
            }

            if (string.Equals(current.Question.CorrectAnswerId, answerId, StringComparison.Ordinal))
            {
                score += options!.PointsPerCorrectAnswer;
                correctAnswers++;
                currentStreak++;
                bestStreak = Math.Max(bestStreak, currentStreak);
                questionsPlayed++;
                lastAnswer = Outcome(AnswerDisposition.Correct, answerId, true);
                FinishIfNeeded(GameEndReason.Completed);
            }
            else
            {
                CommitMiss(AnswerDisposition.Incorrect, answerId);
            }
            return lastAnswer!;
        }
    }

    /// <summary>Advances only when the caller presents the still-current question ticket.</summary>
    public bool NextQuestion(Guid requestedSessionId, Guid questionToken)
    {
        lock (gate)
        {
            if (requestedSessionId != sessionId || status != SessionStatus.AwaitingNext) return false;
            var current = CurrentRound();
            if (current is null || current.Token != questionToken) return false;
            index++;
            if (index >= rounds.Count)
            {
                Finish(GameEndReason.Completed);
                return true;
            }
            lastAnswer = null;
            status = SessionStatus.QuestionActive;
            BeginQuestion();
            return true;
        }
    }

    /// <summary>Ends the current run. A second call returns the already-created result.</summary>
    public QuizResult Interrupt()
    {
        lock (gate)
        {
            if (result is not null) return result;
            if (status == SessionStatus.NotStarted) throw new InvalidOperationException("Гру ще не запущено.");
            Finish(GameEndReason.Interrupted);
            status = SessionStatus.Interrupted;
            return result!;
        }
    }

    private void CommitMiss(AnswerDisposition disposition, string? submittedAnswerId)
    {
        questionsPlayed++;
        lives--;
        currentStreak = 0;
        lastAnswer = Outcome(disposition, submittedAnswerId, true);
        if (lives <= 0) Finish(GameEndReason.LivesExhausted);
        else FinishIfNeeded(GameEndReason.Completed);
        if (status == SessionStatus.QuestionActive) status = SessionStatus.AwaitingNext;
    }

    private void FinishIfNeeded(GameEndReason reason)
    {
        if (questionsPlayed >= rounds.Count) Finish(reason);
        else status = SessionStatus.AwaitingNext;
    }

    private void Finish(GameEndReason reason)
    {
        if (request is null) return;
        var accuracy = questionsPlayed == 0 ? 0d : (double)correctAnswers / questionsPlayed;
        result = new QuizResult(sessionId, request.PlayerName, request.Category, request.Difficulty, score,
            correctAnswers, questionsPlayed, rounds.Count, accuracy, bestStreak, reason, timeProvider.GetUtcNow());
        if (reason == GameEndReason.Interrupted) status = SessionStatus.Interrupted;
        else status = SessionStatus.Finished;
    }

    private AnswerOutcome Outcome(AnswerDisposition disposition, string? submittedAnswerId, bool accepted) =>
        new(disposition, CurrentRound()?.Question.CorrectAnswerId, submittedAnswerId, score, lives, currentStreak, bestStreak, accepted);

    private Round? CurrentRound() => index >= 0 && index < rounds.Count ? rounds[index] : null;

    private void BeginQuestion()
    {
        var current = CurrentRound() ?? throw new InvalidOperationException("Поточне питання відсутнє.");
        current.Token = Guid.NewGuid();
        deadline = timeProvider.GetUtcNow() + options!.TimePerQuestion;
    }

    private static Answer[] ShuffledAnswers(Question question, Random random)
    {
        var answers = question.Answers.ToArray();
        Shuffle(answers, random);
        return answers;
    }

    private static void Shuffle<T>(IList<T> items, Random random)
    {
        for (var i = items.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (items[i], items[j]) = (items[j], items[i]);
        }
    }

    private sealed class Round(Question question, Answer[] answers)
    {
        public Question Question { get; } = question;
        public Answer[] Answers { get; } = answers;
        public Guid Token { get; set; }
    }
}

