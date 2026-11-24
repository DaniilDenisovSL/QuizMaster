using LR2_QuizMaster.Backend.Domain;

using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class GameSetupForm : Form
{
    public string SelectedCategory { get; private set; } = "Програмування";
    public string SelectedDifficulty => rdoEasy.Checked ? "Легка" : rdoHard.Checked ? "Складна" : "Середня";
    public bool ShuffleQuestions => chkShuffleQuestions.Checked;
    public bool ShuffleAnswers => chkShuffleAnswers.Checked;

    public GameSetupForm()
    {
        InitializeComponent();
        WindowPresentation.Attach(this);
        var rules = new QuizOptions();
        lblRules.Text = $"{rules.QuestionCount} питань · {rules.TimePerQuestion.TotalSeconds:0} с · {rules.StartingLives} життя";
        var buttons = new[] { btnGames, btnProgramming, btnSport, btnMovies, btnGeneral };
        foreach (var button in buttons)
        {
            button.Click += (_, _) =>
            {
                SelectedCategory = button.Text;
                lblSelection.Text = $"Обрано: {SelectedCategory}";
                foreach (var item in buttons) item.BackColor = Color.FromArgb(27, 38, 59);
                button.BackColor = Color.FromArgb(109, 94, 245);
            };
        }
        btnBack.Click += (_, _) => { DialogResult = DialogResult.Cancel; };
        btnStartQuiz.Click += (_, _) => { DialogResult = DialogResult.OK; };
        CancelButton = btnBack;
        AcceptButton = btnStartQuiz;
    }
}
