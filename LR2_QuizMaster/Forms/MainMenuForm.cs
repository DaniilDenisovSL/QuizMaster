using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.Backend.Services;
using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class MainMenuForm : Form
{
    private readonly LeaderboardStore leaderboardStore;
    private QuestionBank? questionBank;

    public MainMenuForm() : this(null, null) { }

    public MainMenuForm(QuestionBank? bank, LeaderboardStore? store)
    {
        leaderboardStore = store ?? new LeaderboardStore();
        InitializeComponent();
        WindowPresentation.Attach(this);
        AcceptButton = btnStart;
        btnStart.Click += StartGame;
        btnLeaderboard.Click += (_, _) =>
        {
            using var form = new LeaderboardForm(leaderboardStore);
            form.ShowDialog(this);
        };
        btnExit.Click += (_, _) => Close();
        try { questionBank = bank ?? QuestionBank.Load(QuestionBank.DefaultFilePath); }
        catch (Exception ex) when (ex is IOException or InvalidDataException or UnauthorizedAccessException or ArgumentException)
        {
            btnStart.Enabled = false;
            Shown += (_, _) => MessageBox.Show(this, ex.Message, "Не вдалося завантажити питання",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void StartGame(object? sender, EventArgs e)
    {
        if (questionBank == null) return;
        if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
        {
            MessageBox.Show(this, "Введи ім’я гравця.", "Quiz Master", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtPlayerName.Focus();
            return;
        }
        while (true)
        {
            using var setup = new GameSetupForm();
            if (setup.ShowDialog(this) != DialogResult.OK) return;
            var options = new QuizOptions { ShuffleQuestions = setup.ShuffleQuestions, ShuffleAnswers = setup.ShuffleAnswers };
            var category = QuizCatalog.ParseCategory(setup.SelectedCategory);
            var difficulty = QuizCatalog.ParseDifficulty(setup.SelectedDifficulty);
            try { questionBank.EnsureCanStart(category, difficulty, options.QuestionCount); }
            catch (InsufficientQuestionsException ex)
            {
                MessageBox.Show(this, ex.Message, "Недостатньо питань", MessageBoxButtons.OK, MessageBoxIcon.Information);
                continue;
            }
            var request = new QuizRequest(txtPlayerName.Text.Trim(), category, difficulty, options);
            DialogResult outcome;
            Hide();
            try
            {
                using var quiz = new QuizForm(request, questionBank, leaderboardStore);
                outcome = quiz.ShowDialog();
            }
            finally { Show(); Activate(); }
            if (outcome != DialogResult.Retry) return;
        }
    }
}
