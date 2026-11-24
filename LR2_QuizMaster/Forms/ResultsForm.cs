using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.Backend.Services;
using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class ResultsForm : Form
{
    private LeaderboardStore leaderboardStore = new();

    public ResultsForm()
    {
        InitializeComponent();
        DoubleBuffered = true;
        WindowPresentation.Attach(this);
        btnPlayAgain.Click += (_, _) => { DialogResult = DialogResult.Retry; };
        btnMainMenu.Click += (_, _) => { DialogResult = DialogResult.OK; };
        btnLeaderboard.Click += (_, _) =>
        {
            using var form = new LeaderboardForm(leaderboardStore);
            WindowNavigation.ShowDialog(this, form);
        };
    }

    public ResultsForm(QuizResult result, LeaderboardStore leaderboardStore, string? saveWarning = null) : this()
    {
        ArgumentNullException.ThrowIfNull(result);
        this.leaderboardStore = leaderboardStore ?? throw new ArgumentNullException(nameof(leaderboardStore));
        bool lost = result.EndReason == GameEndReason.LivesExhausted;
        lblHeading.Text = lost ? "Цього разу програно" : "Гру завершено!";
        lblHeading.Tag = lost ? GameResultVisual.Lost : GameResultVisual.Completed;
        lblHeading.ForeColor = lost ? Color.FromArgb(239, 153, 166) : Color.FromArgb(244, 247, 252);
        btnPlayAgain.Text = lost ? "Спробувати ще раз" : "Зіграти ще раз";
        lblPlayer.Text = $"{result.PlayerName} · {QuizCatalog.CategoryName(result.Category)} · {QuizCatalog.DifficultyName(result.Difficulty)}";
        lblTotalScore.Text = $"{result.Score} балів";
        lblCorrect.Text = $"Правильно: {result.CorrectAnswers}/{result.QuestionsPlayed}";
        lblAccuracy.Text = $"Точність: {result.Accuracy:P0}";
        lblBestStreak.Text = $"Найкраща серія: {result.BestStreak}";
        string reason = result.EndReason == GameEndReason.LivesExhausted ? "Життя вичерпано. Спробуй знову — усе вийде." : "Усі питання пройдено.";
        lblSummaryNote.Text = $"{reason} Пройдено {result.QuestionsPlayed} із {result.TotalQuestions} питань. " +
            (saveWarning == null ? "Результат збережено." : "Рекорд не вдалося зберегти.");
        if (saveWarning != null)
        {
            components ??= new System.ComponentModel.Container();
            var tooltip = new ToolTip(components);
            tooltip.SetToolTip(lblSummaryNote, saveWarning);
            lblSummaryNote.ForeColor = Color.FromArgb(239, 153, 166);
        }
    }
}
