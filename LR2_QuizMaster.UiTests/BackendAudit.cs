using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.Backend.Services;

internal static class BackendAudit
{
    internal static async Task<int> Run(string output)
    {
        int failures = 0;
        var source = JsonNode.Parse(File.ReadAllText(QuestionBank.DefaultFilePath))!.AsArray()
            .First(q => q!["id"]!.GetValue<string>() == "programming-medium-001")!;
        foreach (string field in new[] { "category", "difficulty" })
        {
            var q = source.DeepClone().AsObject(); q.Remove(field);
            string file = Path.Combine(output, "missing-" + field + ".json");
            File.WriteAllText(file, new JsonArray(q).ToJsonString());
            try
            {
                var bank = QuestionBank.Load(file);
                failures++;
                Console.WriteLine($"FAIL DATA-01: missing {field} silently becomes {bank.Questions[0].Category}/{bank.Questions[0].Difficulty}");
            }
            catch (InvalidDataException) { Console.WriteLine("PASS required " + field); }
        }
        string nullFile = Path.Combine(output, "null-record.json");
        File.WriteAllText(nullFile, "[null]");
        try
        {
            var loaded = await new LeaderboardStore(nullFile).LoadAsync();
            if (loaded.Records.Count == 0) Console.WriteLine("PASS null leaderboard record safely handled");
            else { failures++; Console.WriteLine("FAIL DATA-02: null record retained"); }
        }
        catch (Exception ex) { failures++; Console.WriteLine("FAIL DATA-02: [null] throws " + ex.GetType().Name); }
        string enumFile = Path.Combine(output, "unknown-category.json");
        var invalid = new QuizResult(Guid.NewGuid(), "Probe", (QuizCategory)99, Difficulty.Easy,
            100, 1, 1, 10, .1, 1, GameEndReason.Completed, DateTimeOffset.UtcNow);
        var options = new JsonSerializerOptions(); options.Converters.Add(new JsonStringEnumConverter());
        File.WriteAllText(enumFile, JsonSerializer.Serialize(new[] { invalid }, options));
        try
        {
            var loaded = await new LeaderboardStore(enumFile).LoadAsync();
            if (loaded.Records.Count == 0) Console.WriteLine("PASS unknown category filtered out");
            else
            {
                foreach (var record in loaded.Records) QuizCatalog.CategoryName(record.Category);
                failures++; Console.WriteLine("FAIL DATA-02: unknown category retained");
            }
        }
        catch (Exception ex) { failures++; Console.WriteLine("FAIL DATA-02: unknown category causes " + ex.GetType().Name); }
        Console.WriteLine($"Data audit: {failures} failing cases.");
        return failures == 0 ? 0 : 1;
    }
}
