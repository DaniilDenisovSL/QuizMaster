using System.Text.Json;
using System.Text.Json.Serialization;
using LR2_QuizMaster.Backend.Domain;

namespace LR2_QuizMaster.Backend.Services;

public sealed class QuestionBank
{
    private readonly IReadOnlyList<Question> questions;
    public IReadOnlyList<Question> Questions => questions;

    public QuestionBank(IEnumerable<Question> questions)
    {
        var copy = questions?.ToArray() ?? throw new ArgumentNullException(nameof(questions));
        Validate(copy);
        this.questions = Array.AsReadOnly(copy.Select(q => q with
        {
            Answers = Array.AsReadOnly(q.Answers.ToArray())
        }).ToArray());
    }

    public IReadOnlyList<Question> Find(QuizCategory category, Difficulty difficulty) =>
        questions.Where(q => q.Category == category && q.Difficulty == difficulty).ToArray();

    public void EnsureCanStart(QuizCategory category, Difficulty difficulty, int count)
    {
        if (!Enum.IsDefined(category)) throw new ArgumentOutOfRangeException(nameof(category));
        if (!Enum.IsDefined(difficulty)) throw new ArgumentOutOfRangeException(nameof(difficulty));
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        var available = Find(category, difficulty).Count;
        if (available < count) throw new InsufficientQuestionsException(category, difficulty, count, available);
    }

    public static QuestionBank Load(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException($"Не знайдено банк питань: {path}", path);
        try
        {
            using var stream = File.OpenRead(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            var data = JsonSerializer.Deserialize<List<QuestionDto>>(stream, options)
                ?? throw new InvalidDataException("Файл банку питань порожній або містить null.");
            return new QuestionBank(data.Select(x =>
            {
                if (x is null) throw new InvalidDataException("Питання не може мати значення null.");
                if (x.Category is null || x.Difficulty is null)
                    throw new InvalidDataException($"Питання <{x.Id}> повинно містити категорію та складність.");
                return new Question(x.Id ?? "", x.Text ?? "", x.Category.Value, x.Difficulty.Value,
                    (x.Answers ?? []).Select(a => a is null
                        ? throw new InvalidDataException($"Питання <{x.Id}> містить відповідь зі значенням null.")
                        : new Answer(a.Id ?? "", a.Text ?? "")).ToArray(), x.CorrectAnswerId ?? "");
            }));
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Не вдалося прочитати JSON банку питань: {ex.Message}", ex);
        }
    }

    public static string DefaultFilePath => Path.Combine(AppContext.BaseDirectory, "Backend", "Data", "questions.json");

    private static void Validate(IReadOnlyList<Question> items)
    {
        if (items.Count == 0) throw new InvalidDataException("Банк питань не може бути порожнім.");
        var questionIds = new HashSet<string>(StringComparer.Ordinal);
        var answerIds = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < items.Count; i++)
        {
            var q = items[i] ?? throw new InvalidDataException($"Питання #{i + 1} дорівнює null.");
            var label = string.IsNullOrWhiteSpace(q.Id) ? $"#{i + 1}" : q.Id;
            if (string.IsNullOrWhiteSpace(q.Id) || !questionIds.Add(q.Id))
                throw new InvalidDataException($"Порожній або повторний ID питання «{label}».");
            if (string.IsNullOrWhiteSpace(q.Text)) throw new InvalidDataException($"Питання «{label}» не має тексту.");
            if (!Enum.IsDefined(q.Category) || !Enum.IsDefined(q.Difficulty))
                throw new InvalidDataException($"Питання «{label}» має невідому категорію або складність.");
            if (q.Answers is null || q.Answers.Count != 4)
                throw new InvalidDataException($"Питання «{label}» має містити рівно чотири відповіді.");
            var idsInQuestion = new HashSet<string>(StringComparer.Ordinal);
            foreach (var answer in q.Answers)
            {
                if (answer is null || string.IsNullOrWhiteSpace(answer.Id) || string.IsNullOrWhiteSpace(answer.Text))
                    throw new InvalidDataException($"Питання «{label}» містить порожню відповідь або ID.");
                if (!idsInQuestion.Add(answer.Id) || !answerIds.Add(answer.Id))
                    throw new InvalidDataException($"Повторний ID відповіді «{answer.Id}».");
            }
            if (!idsInQuestion.Contains(q.CorrectAnswerId))
                throw new InvalidDataException($"Правильна відповідь питання «{label}» відсутня серед варіантів.");
        }
    }

    private sealed class QuestionDto
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public QuizCategory? Category { get; set; }
        public Difficulty? Difficulty { get; set; }
        public List<AnswerDto>? Answers { get; set; }
        public string? CorrectAnswerId { get; set; }
    }

    private sealed class AnswerDto
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
    }
}



