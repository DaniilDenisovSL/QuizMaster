using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class QuizForm : Form
{
    private string playerName = "Гравець";
    private string category = "Програмування";
    private string difficulty = "Середня";
    public bool ShuffleQuestions { get; private set; } = true;
    public bool ShuffleAnswers { get; private set; } = true;

    public QuizForm()
    {
        InitializeComponent();
        WindowPresentation.Attach(this);
        var answers = new[] { btnAnswerA, btnAnswerB, btnAnswerC, btnAnswerD };
        foreach (var answer in answers)
        {
            answer.Click += (_, _) =>
            {
                foreach (var item in answers) item.BackColor = Color.FromArgb(27, 38, 59);
                answer.BackColor = Color.FromArgb(109, 94, 245);
                lblFeedback.Text = $"Обрано: {answer.Text.Trim()}";
                btnNextQuestion.Enabled = true;
                btnNextQuestion.BackColor = Color.FromArgb(109, 94, 245);
            };
        }
        btnExitQuiz.Click += (_, _) => { DialogResult = DialogResult.Cancel; };
        btnNextQuestion.Click += (_, _) =>
        {
            Hide();
            using var result = new ResultsForm(playerName, category, difficulty);
            DialogResult = result.ShowDialog() == DialogResult.Retry ? DialogResult.Retry : DialogResult.OK;
        };
        CancelButton = btnExitQuiz;
    }

    public QuizForm(string playerName, string category, string difficulty, bool shuffleQuestions, bool shuffleAnswers) : this()
    {
        this.playerName = playerName;
        this.category = category;
        this.difficulty = difficulty;
        ShuffleQuestions = shuffleQuestions;
        ShuffleAnswers = shuffleAnswers;
        lblCategory.Text = $"{category}  /  {difficulty}";
        // TODO Backend: завантажити питання обраної категорії та застосувати налаштування.
        // Поточне питання — лише приклад верстки; таймер і рахунок ще не працюють.
    }
}
