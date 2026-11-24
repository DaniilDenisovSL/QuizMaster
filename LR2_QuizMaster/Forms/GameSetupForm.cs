using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class GameSetupForm : Form
{
    public string SelectedCategory { get; private set; } = "Програмування";
    public string SelectedDifficulty { get; private set; } = "Середня";
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool ShuffleEnabled => btnShuffle.Selected;

    public GameSetupForm()
    {
        InitializeComponent();
        DoubleBuffered = true;
        WindowPresentation.Attach(this);
        components ??= new System.ComponentModel.Container();
        var tips = new ToolTip(components) { InitialDelay = 400, ReshowDelay = 100, AutoPopDelay = 7000, ShowAlways = true };
        var rules = new QuizOptions();
        lblRules.Text = $"{rules.QuestionCount} питань · {rules.TimePerQuestion.TotalSeconds:0} с · {rules.StartingLives} життя";
        var buttons = new[] { btnGames, btnProgramming, btnSport, btnMovies, btnGeneral };
        string[] categoryTips = ["Персонажі, консолі та історія відеоігор.", "Мови, алгоритми й основи розробки.", "Правила, змагання та спортивні факти.", "Герої, режисери й відомі кінострічки.", "Наука, географія та світ навколо нас."];
        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            tips.SetToolTip(button, categoryTips[i]);
            button.AccessibleDescription = categoryTips[i];
            button.Click += (_, _) =>
            {
                SelectedCategory = button.Text;
                lblSelection.Text = $"Обрано: {SelectedCategory}";
                foreach (var item in buttons) item.BackColor = Color.FromArgb(27, 38, 59);
                button.BackColor = Color.FromArgb(109, 94, 245);
            };
        }
        var levels = new[] { btnEasy, btnMedium, btnHard };
        string[] levelTips = ["Базові факти та знайомі теми. Добре для першої спроби.", "Більше деталей і запитань на уважність. Для тих, хто вже знайомий із темою.", "Рідкісні факти й тонкощі теми. Для впевнених знавців."];
        for (int i = 0; i < levels.Length; i++)
        {
            var button = levels[i];
            tips.SetToolTip(button, levelTips[i]);
            button.AccessibleDescription = levelTips[i];
            button.Click += (_, _) =>
            {
                SelectedDifficulty = button.Text;
                foreach (var level in levels)
                {
                    level.Selected = level == button;
                    level.BackColor = level.Selected ? Color.FromArgb(109, 94, 245) : Color.FromArgb(27, 38, 59);
                }
            };
        }
        bool shuffleHovered = false;
        void UpdateShuffleAppearance()
        {
            btnShuffle.Text = ShuffleEnabled
                ? "Перемішування питань і відповідей: увімкнено"
                : "Перемішування питань і відповідей: вимкнено";
            btnShuffle.BackColor = ShuffleEnabled
                ? (shuffleHovered ? Color.FromArgb(124, 111, 250) : Color.FromArgb(109, 94, 245))
                : (shuffleHovered ? Color.FromArgb(47, 61, 95) : Color.FromArgb(27, 38, 59));
            btnShuffle.AccessibleDescription = ShuffleEnabled
                ? "Перемішування увімкнено. Натисни, щоб вимкнути."
                : "Перемішування вимкнено. Натисни, щоб увімкнути.";
        }
        btnShuffle.Click += (_, _) => { btnShuffle.Selected = !btnShuffle.Selected; UpdateShuffleAppearance(); };
        btnShuffle.MouseEnter += (_, _) => { shuffleHovered = true; UpdateShuffleAppearance(); };
        btnShuffle.MouseLeave += (_, _) => { shuffleHovered = false; UpdateShuffleAppearance(); };
        tips.SetToolTip(btnShuffle, "Увімкнути або вимкнути перемішування питань і варіантів відповідей у межах кожного питання.");
        UpdateShuffleAppearance();
        tips.SetToolTip(btnStartQuiz, "Почати тест з обраною категорією та складністю.");
        tips.SetToolTip(btnBack, "Повернутися до головного меню.");
        btnBack.Click += (_, _) => DialogResult = DialogResult.Cancel;
        btnStartQuiz.Click += (_, _) => DialogResult = DialogResult.OK;
        CancelButton = btnBack;
        AcceptButton = btnStartQuiz;
    }
}
