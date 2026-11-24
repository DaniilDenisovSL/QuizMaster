using System.Text.Json;
using System.Text.Json.Serialization;
using LR2_QuizMaster.Backend.Domain;

namespace LR2_QuizMaster.Backend.Services;

public sealed record LeaderboardLoadResult(IReadOnlyList<QuizResult> Records, string? Warning = null);

/// <summary>Local JSON persistence. Interrupted sessions are deliberately excluded.</summary>
public sealed class LeaderboardStore
{
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, SemaphoreSlim> FileGates = new(StringComparer.OrdinalIgnoreCase);
    private readonly string filePath;
    private readonly SemaphoreSlim gate;
    private readonly JsonSerializerOptions jsonOptions = CreateJsonOptions();

    public LeaderboardStore(string? filePath = null)
    {
        this.filePath = Path.GetFullPath(filePath ?? DefaultFilePath);
        gate = FileGates.GetOrAdd(this.filePath, _ => new SemaphoreSlim(1, 1));
    }

    public static string DefaultFilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QuizMaster", "leaderboard.json");

    public async Task<LeaderboardLoadResult> LoadAsync(CancellationToken cancellationToken = default)
    {
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try { return await LoadUnsafeAsync(cancellationToken).ConfigureAwait(false); }
        finally { gate.Release(); }
    }

    public async Task<IReadOnlyList<QuizResult>> GetSortedAsync(QuizCategory? category = null, CancellationToken cancellationToken = default)
    {
        var loaded = await LoadAsync(cancellationToken).ConfigureAwait(false);
        return loaded.Records.Where(x => category is null || x.Category == category)
            .OrderByDescending(x => x.Score).ThenByDescending(x => x.Accuracy).ThenByDescending(x => x.FinishedAtUtc).ToArray();
    }

    /// <returns>true only when a new non-interrupted session was written.</returns>
    public async Task<bool> SaveAsync(QuizResult result, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(result);
        if (result.EndReason == GameEndReason.Interrupted) return false;
        if (!IsValidRecord(result))
            throw new ArgumentException("Результат містить некоректні або неузгоджені значення.", nameof(result));

        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var loaded = await LoadUnsafeAsync(cancellationToken).ConfigureAwait(false);
            if (loaded.Records.Any(x => x.SessionId == result.SessionId)) return false;

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            if (loaded.Warning is not null && File.Exists(filePath))
            {
                var backupPath = filePath + ".corrupt-" + DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmssfff") + ".bak";
                File.Copy(filePath, backupPath, overwrite: false);
            }

            var records = loaded.Records.ToList();
            records.Add(result);
            var tempPath = filePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                await using (var stream = new FileStream(tempPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, useAsync: true))
                    await JsonSerializer.SerializeAsync(stream, records, jsonOptions, cancellationToken).ConfigureAwait(false);
                File.Move(tempPath, filePath, overwrite: true);
            }
            finally
            {
                if (File.Exists(tempPath)) File.Delete(tempPath);
            }
            return true;
        }
        finally { gate.Release(); }
    }

    private async Task<LeaderboardLoadResult> LoadUnsafeAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath)) return new LeaderboardLoadResult(Array.Empty<QuizResult>());
        try
        {
            var text = await File.ReadAllTextAsync(filePath, cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(text)) return new LeaderboardLoadResult(Array.Empty<QuizResult>());
            var records = JsonSerializer.Deserialize<List<QuizResult>>(text, jsonOptions);
            if (records is null) return new LeaderboardLoadResult(Array.Empty<QuizResult>(), "Файл рекордів містить null; показано порожній список.");
            var valid = records.Where(IsValidRecord).DistinctBy(r => r.SessionId).ToArray();
            return new LeaderboardLoadResult(valid, valid.Length == records.Count ? null :
                "Некоректні, повторні або перервані результати пропущено. Перед збереженням буде створено резервну копію початкового файлу.");
        }
        catch (JsonException ex)
        {
            return new LeaderboardLoadResult(Array.Empty<QuizResult>(), $"Файл рекордів пошкоджений і не завантажений: {ex.Message}");
        }
    }

    private static bool IsValidRecord(QuizResult? value)
    {
        if (value is null || value.SessionId == Guid.Empty || string.IsNullOrWhiteSpace(value.PlayerName) ||
            !Enum.IsDefined(value.Category) || !Enum.IsDefined(value.Difficulty) || !Enum.IsDefined(value.EndReason) ||
            value.EndReason == GameEndReason.Interrupted || value.FinishedAtUtc == default || value.Score < 0 ||
            value.TotalQuestions <= 0 || value.QuestionsPlayed <= 0 || value.QuestionsPlayed > value.TotalQuestions ||
            value.CorrectAnswers < 0 || value.CorrectAnswers > value.QuestionsPlayed ||
            value.BestStreak < 0 || value.BestStreak > value.CorrectAnswers ||
            !double.IsFinite(value.Accuracy) || value.Accuracy < 0 || value.Accuracy > 1 ||
            (value.EndReason == GameEndReason.Completed && value.QuestionsPlayed != value.TotalQuestions)) return false;
        return Math.Abs(value.Accuracy - (double)value.CorrectAnswers / value.QuestionsPlayed) < 1e-10;
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var result = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true };
        result.Converters.Add(new JsonStringEnumConverter());
        return result;
    }
}
