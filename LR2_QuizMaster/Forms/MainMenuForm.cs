using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class MainMenuForm : Form
{
    public MainMenuForm()
    {
        InitializeComponent();
        WindowPresentation.Attach(this);
        AcceptButton = btnStart;
        btnStart.Click += StartGame;
        btnLeaderboard.Click += (_, _) => { using var form = new LeaderboardForm(); form.ShowDialog(this); };
        btnExit.Click += (_, _) => Close();
    }

    private void StartGame(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
        {
            MessageBox.Show(this, "Введи ім’я гравця.", "Quiz Master", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtPlayerName.Focus();
            return;
        }
        DialogResult outcome;
        do
        {
            using var setup = new GameSetupForm();
            if (setup.ShowDialog(this) != DialogResult.OK) return;
            Hide();
            try
            {
                using var quiz = new QuizForm(txtPlayerName.Text.Trim(), setup.SelectedCategory, setup.SelectedDifficulty, setup.ShuffleQuestions, setup.ShuffleAnswers);
                outcome = quiz.ShowDialog();
            }
            finally { Show(); Activate(); }
        } while (outcome == DialogResult.Retry);
    }
}
