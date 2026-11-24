namespace LR2_QuizMaster.Backend.Domain;

/// <summary>Stable internal category keys. UI text is mapped by <see cref="QuizCatalog"/>.</summary>
public enum QuizCategory { Games, Programming, Sports, Movies, GeneralKnowledge }
public enum Difficulty { Easy, Medium, Hard }
public enum SessionStatus { NotStarted, QuestionActive, AwaitingNext, Finished, Interrupted }
public enum GameEndReason { Completed, LivesExhausted, Interrupted }
public enum AnswerDisposition { Correct, Incorrect, TimedOut, AlreadyAnswered, InvalidAnswer, StaleSession, StaleQuestion, SessionNotActive }

public sealed record Answer(string Id, string Text);
public sealed record Question(string Id, string Text, QuizCategory Category, Difficulty Difficulty, IReadOnlyList<Answer> Answers, string CorrectAnswerId);

/// <summary>All game rules live here so a session does not scatter magic numbers.</summary>
public sealed record QuizOptions
{
    public int QuestionCount { get; init; } = 10;
    public TimeSpan TimePerQuestion { get; init; } = TimeSpan.FromSeconds(20);
    public int StartingLives { get; init; } = 3;
    public int PointsPerCorrectAnswer { get; init; } = 100;
    public bool ShuffleQuestions { get; init; } = true;
    public bool ShuffleAnswers { get; init; } = true;
    /// <summary>Optional reproducibility hook for tests and deterministic demos.</summary>
    public int? RandomSeed { get; init; }

    public void Validate()
    {
        if ((long)QuestionCount * PointsPerCorrectAnswer > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(PointsPerCorrectAnswer), "Максимальна сума балів перевищує допустиме значення Int32.");
        if (QuestionCount <= 0) throw new ArgumentOutOfRangeException(nameof(QuestionCount), "Кількість питань має бути більшою за нуль.");
        if (TimePerQuestion <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(TimePerQuestion), "Час на питання має бути більшим за нуль.");
        if (StartingLives <= 0) throw new ArgumentOutOfRangeException(nameof(StartingLives), "Кількість життів має бути більшою за нуль.");
        if (PointsPerCorrectAnswer < 0) throw new ArgumentOutOfRangeException(nameof(PointsPerCorrectAnswer), "Бали не можуть бути від’ємними.");
    }
}

public sealed record QuizRequest(string PlayerName, QuizCategory Category, Difficulty Difficulty, QuizOptions? Options = null);
public sealed record QuestionTicket(Guid SessionId, Guid QuestionToken, string QuestionId, string Text, IReadOnlyList<Answer> Answers);
public sealed record AnswerOutcome(AnswerDisposition Disposition, string? CorrectAnswerId, string? SubmittedAnswerId, int Score, int Lives, int CurrentStreak, int BestStreak, bool WasAccepted)
{
    public bool IsCorrect => Disposition == AnswerDisposition.Correct;
}

public sealed record QuizResult(Guid SessionId, string PlayerName, QuizCategory Category, Difficulty Difficulty, int Score, int CorrectAnswers, int QuestionsPlayed, int TotalQuestions, double Accuracy, int BestStreak, GameEndReason EndReason, DateTimeOffset FinishedAtUtc);
public sealed record QuizState(Guid SessionId, SessionStatus Status, QuestionTicket? CurrentQuestion, int QuestionNumber, int TotalQuestions, TimeSpan RemainingTime, int Score, int Lives, int CurrentStreak, int BestStreak, AnswerOutcome? LastAnswer, QuizResult? Result);

public sealed class InsufficientQuestionsException(QuizCategory category, Difficulty difficulty, int requested, int available)
    : InvalidOperationException($"Для категорії «{QuizCatalog.CategoryName(category)}» та складності «{QuizCatalog.DifficultyName(difficulty)}» потрібно {requested} питань, у банку є {available}.")
{
    public QuizCategory Category { get; } = category;
    public Difficulty Difficulty { get; } = difficulty;
    public int Requested { get; } = requested;
    public int Available { get; } = available;
}

public static class QuizCatalog
{
    public static string CategoryName(QuizCategory value) => value switch
    {
        QuizCategory.Games => "Ігри", QuizCategory.Programming => "Програмування", QuizCategory.Sports => "Спорт",
        QuizCategory.Movies => "Фільми", QuizCategory.GeneralKnowledge => "Загальні знання",
        _ => throw new ArgumentOutOfRangeException(nameof(value))
    };

    public static string DifficultyName(Difficulty value) => value switch
    {
        Difficulty.Easy => "Легка", Difficulty.Medium => "Середня", Difficulty.Hard => "Складна",
        _ => throw new ArgumentOutOfRangeException(nameof(value))
    };

    public static bool TryParseCategory(string? displayName, out QuizCategory category)
    {
        category = default;
        foreach (var candidate in Enum.GetValues<QuizCategory>())
            if (string.Equals(CategoryName(candidate), displayName?.Trim(), StringComparison.CurrentCultureIgnoreCase))
            { category = candidate; return true; }
        return false;
    }

    public static QuizCategory ParseCategory(string displayName) => TryParseCategory(displayName, out var value)
        ? value : throw new ArgumentException($"Невідома категорія: «{displayName}».", nameof(displayName));

    public static bool TryParseDifficulty(string? displayName, out Difficulty difficulty)
    {
        difficulty = default;
        foreach (var candidate in Enum.GetValues<Difficulty>())
            if (string.Equals(DifficultyName(candidate), displayName?.Trim(), StringComparison.CurrentCultureIgnoreCase))
            { difficulty = candidate; return true; }
        return false;
    }

    public static Difficulty ParseDifficulty(string displayName) => TryParseDifficulty(displayName, out var value)
        ? value : throw new ArgumentException($"Невідома складність: «{displayName}».", nameof(displayName));
}

