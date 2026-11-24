using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class ResultsForm : Form
{
    public ResultsForm()
    {
        InitializeComponent();
        WindowPresentation.Attach(this);
        btnPlayAgain.Click += (_, _) => { DialogResult = DialogResult.Retry; };
        btnMainMenu.Click += (_, _) => { DialogResult = DialogResult.OK; };
        btnLeaderboard.Click += (_, _) => { using var form = new LeaderboardForm(); form.ShowDialog(this); };
    }

    public ResultsForm(string playerName, string category, string difficulty) : this()
    {
        lblPlayer.Text = $"{playerName} · {category} · {difficulty}";
        // TODO Backend: замінити приклад 820 / 8 з 10 / 80% / серія 5 реальним результатом.
    }
}
