#nullable enable

using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

partial class GameSetupForm
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblBrand = null!;
    private Label lblDemo = null!;
    private QuizButton btnBack = null!;
    private Label lblTitle = null!;
    private Label lblCategory = null!;
    private QuizButton btnGames = null!;
    private QuizButton btnProgramming = null!;
    private QuizButton btnSport = null!;
    private QuizButton btnMovies = null!;
    private QuizButton btnGeneral = null!;
    private Label lblSelection = null!;
    private Label lblDifficulty = null!;
    private QuizButton btnEasy = null!;
    private QuizButton btnMedium = null!;
    private QuizButton btnHard = null!;
    private Label lblDifficultyNote = null!;
    private QuizButton btnShuffle = null!;
    private Label lblRules = null!;
    private QuizButton btnStartQuiz = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private Label lblHeaderRule = null!;

    private void InitializeComponent()
    {
        lblBrand = new Label();
        lblDemo = new Label();
        btnBack = new QuizButton();
        lblTitle = new Label();
        lblCategory = new Label();
        btnGames = new QuizButton();
        btnProgramming = new QuizButton();
        btnSport = new QuizButton();
        btnMovies = new QuizButton();
        btnGeneral = new QuizButton();
        lblSelection = new Label();
        lblDifficulty = new Label();
        btnEasy = new QuizButton();
        btnMedium = new QuizButton();
        btnHard = new QuizButton();
        lblDifficultyNote = new Label();
        btnShuffle = new QuizButton();
        lblRules = new Label();
        btnStartQuiz = new QuizButton();
        lblHeaderRule = new Label();
        SuspendLayout();
        lblHeaderRule.Name = "lblHeaderRule";
        lblHeaderRule.Location = new Point(32, 63);
        lblHeaderRule.Size = new Size(976, 1);
        lblHeaderRule.BackColor = Color.FromArgb(36, 49, 73);
        Controls.Add(lblHeaderRule);
        lblBrand.TabIndex = 0;
        lblBrand.Name = "lblBrand";
        lblBrand.Location = new Point(32, 22);
        lblBrand.Size = new Size(250, 30);
        lblBrand.Text = "QUIZ MASTER";
        lblBrand.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
        lblBrand.ForeColor = Color.FromArgb(244, 247, 252);
        lblBrand.BackColor = Color.FromArgb(16, 24, 39);

        lblDemo.TabIndex = 1;
        lblDemo.Name = "lblDemo";
        lblDemo.Location = new Point(680, 24);
        lblDemo.Size = new Size(328, 25);
        lblDemo.Text = "";
        lblDemo.Visible = false;
        lblDemo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblDemo.ForeColor = Color.FromArgb(170, 182, 204);
        lblDemo.BackColor = Color.FromArgb(16, 24, 39);

        btnBack.TabIndex = 2;
        btnBack.Name = "btnBack";
        btnBack.Location = new Point(32, 77);
        btnBack.Size = new Size(130, 40);
        btnBack.Text = "←  Назад";
        btnBack.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnBack.BackColor = Color.FromArgb(27, 38, 59);
        btnBack.ForeColor = Color.FromArgb(244, 247, 252);
        btnBack.FlatStyle = FlatStyle.Flat;
        btnBack.FlatAppearance.BorderSize = 0;
        btnBack.UseVisualStyleBackColor = false;
        btnBack.Cursor = Cursors.Hand;

        lblTitle.TabIndex = 3;
        lblTitle.Name = "lblTitle";
        lblTitle.Location = new Point(190, 77);
        lblTitle.Size = new Size(650, 46);
        lblTitle.Text = "Налаштуй свою гру";
        lblTitle.Font = new Font("Segoe UI", 26F, FontStyle.Bold, GraphicsUnit.Point);
        lblTitle.ForeColor = Color.FromArgb(244, 247, 252);
        lblTitle.BackColor = Color.FromArgb(16, 24, 39);

        lblCategory.TabIndex = 4;
        lblCategory.Name = "lblCategory";
        lblCategory.Location = new Point(32, 150);
        lblCategory.Size = new Size(700, 30);
        lblCategory.Text = "01  Обери категорію";
        lblCategory.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
        lblCategory.ForeColor = Color.FromArgb(244, 247, 252);
        lblCategory.BackColor = Color.FromArgb(16, 24, 39);

        btnGames.TabIndex = 5;
        btnGames.Name = "btnGames";
        btnGames.Location = new Point(32, 195);
        btnGames.Size = new Size(312, 76);
        btnGames.Text = "Ігри";
        btnGames.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnGames.BackColor = Color.FromArgb(27, 38, 59);
        btnGames.ForeColor = Color.FromArgb(244, 247, 252);
        btnGames.FlatStyle = FlatStyle.Flat;
        btnGames.FlatAppearance.BorderSize = 0;
        btnGames.UseVisualStyleBackColor = false;
        btnGames.Cursor = Cursors.Hand;

        btnProgramming.TabIndex = 6;
        btnProgramming.Name = "btnProgramming";
        btnProgramming.Location = new Point(364, 195);
        btnProgramming.Size = new Size(312, 76);
        btnProgramming.Text = "Програмування";
        btnProgramming.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnProgramming.BackColor = Color.FromArgb(109, 94, 245);
        btnProgramming.ForeColor = Color.FromArgb(244, 247, 252);
        btnProgramming.FlatStyle = FlatStyle.Flat;
        btnProgramming.FlatAppearance.BorderSize = 0;
        btnProgramming.UseVisualStyleBackColor = false;
        btnProgramming.Cursor = Cursors.Hand;

        btnSport.TabIndex = 7;
        btnSport.Name = "btnSport";
        btnSport.Location = new Point(696, 195);
        btnSport.Size = new Size(312, 76);
        btnSport.Text = "Спорт";
        btnSport.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnSport.BackColor = Color.FromArgb(27, 38, 59);
        btnSport.ForeColor = Color.FromArgb(244, 247, 252);
        btnSport.FlatStyle = FlatStyle.Flat;
        btnSport.FlatAppearance.BorderSize = 0;
        btnSport.UseVisualStyleBackColor = false;
        btnSport.Cursor = Cursors.Hand;

        btnMovies.TabIndex = 8;
        btnMovies.Name = "btnMovies";
        btnMovies.Location = new Point(32, 287);
        btnMovies.Size = new Size(312, 76);
        btnMovies.Text = "Фільми";
        btnMovies.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnMovies.BackColor = Color.FromArgb(27, 38, 59);
        btnMovies.ForeColor = Color.FromArgb(244, 247, 252);
        btnMovies.FlatStyle = FlatStyle.Flat;
        btnMovies.FlatAppearance.BorderSize = 0;
        btnMovies.UseVisualStyleBackColor = false;
        btnMovies.Cursor = Cursors.Hand;

        btnGeneral.TabIndex = 9;
        btnGeneral.Name = "btnGeneral";
        btnGeneral.Location = new Point(364, 287);
        btnGeneral.Size = new Size(312, 76);
        btnGeneral.Text = "Загальні знання";
        btnGeneral.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnGeneral.BackColor = Color.FromArgb(27, 38, 59);
        btnGeneral.ForeColor = Color.FromArgb(244, 247, 252);
        btnGeneral.FlatStyle = FlatStyle.Flat;
        btnGeneral.FlatAppearance.BorderSize = 0;
        btnGeneral.UseVisualStyleBackColor = false;
        btnGeneral.Cursor = Cursors.Hand;

        lblSelection.TabIndex = 10;
        lblSelection.Name = "lblSelection";
        lblSelection.Location = new Point(32, 376);
        lblSelection.Size = new Size(700, 25);
        lblSelection.Text = "Обрано: Програмування";
        lblSelection.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        lblSelection.ForeColor = Color.FromArgb(170, 182, 204);
        lblSelection.BackColor = Color.FromArgb(16, 24, 39);

        lblDifficulty.TabIndex = 11;
        lblDifficulty.Name = "lblDifficulty";
        lblDifficulty.Location = new Point(32, 420);
        lblDifficulty.Size = new Size(500, 30);
        lblDifficulty.Text = "02  Обери складність";
        lblDifficulty.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
        lblDifficulty.ForeColor = Color.FromArgb(244, 247, 252);
        lblDifficulty.BackColor = Color.FromArgb(16, 24, 39);

        btnEasy.TabIndex = 12;
        btnEasy.Name = "btnEasy";
        btnEasy.Location = new Point(32, 466);
        btnEasy.Size = new Size(312, 86);
        btnEasy.Text = "Легка";
        btnEasy.Description = "Знайомі теми · для розігріву";
        btnEasy.Selected = false;
        btnEasy.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
        btnEasy.ForeColor = Color.FromArgb(244, 247, 252);
        btnEasy.BackColor = Color.FromArgb(27, 38, 59);
        btnEasy.Cursor = Cursors.Hand;

        btnMedium.TabIndex = 13;
        btnMedium.Name = "btnMedium";
        btnMedium.Location = new Point(364, 466);
        btnMedium.Size = new Size(312, 86);
        btnMedium.Text = "Середня";
        btnMedium.Description = "Більше деталей · перевір себе";
        btnMedium.Selected = true;
        btnMedium.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
        btnMedium.ForeColor = Color.FromArgb(244, 247, 252);
        btnMedium.BackColor = Color.FromArgb(109, 94, 245);
        btnMedium.Cursor = Cursors.Hand;

        btnHard.TabIndex = 14;
        btnHard.Name = "btnHard";
        btnHard.Location = new Point(696, 466);
        btnHard.Size = new Size(312, 86);
        btnHard.Text = "Складна";
        btnHard.Description = "Тонкощі теми · прийми виклик";
        btnHard.Selected = false;
        btnHard.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
        btnHard.ForeColor = Color.FromArgb(244, 247, 252);
        btnHard.BackColor = Color.FromArgb(27, 38, 59);
        btnHard.Cursor = Cursors.Hand;

        lblDifficultyNote.Name = "lblDifficultyNote";
        lblDifficultyNote.Location = new Point(32, 567);
        lblDifficultyNote.Size = new Size(976, 28);
        lblDifficultyNote.Text = "Складність змінює набір питань. Наведи на рівень, щоб дізнатися більше.";
        lblDifficultyNote.Font = new Font("Segoe UI", 10F);
        lblDifficultyNote.ForeColor = Color.FromArgb(170, 182, 204);
        lblDifficultyNote.BackColor = Color.FromArgb(16, 24, 39);

        btnShuffle.TabIndex = 15;
        btnShuffle.Name = "btnShuffle";
        btnShuffle.Location = new Point(32, 609);
        btnShuffle.Size = new Size(650, 43);
        btnShuffle.Text = "Перемішування питань і відповідей: вимкнено";
        btnShuffle.AccessibleDescription = "Питання та варіанти відповідей у межах кожного питання.";
        btnShuffle.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
        btnShuffle.BackColor = Color.FromArgb(27, 38, 59);
        btnShuffle.ForeColor = Color.FromArgb(244, 247, 252);
        btnShuffle.FlatStyle = FlatStyle.Flat;
        btnShuffle.FlatAppearance.BorderSize = 0;
        btnShuffle.UseVisualStyleBackColor = false;
        btnShuffle.Cursor = Cursors.Hand;

        lblRules.TabIndex = 17;
        lblRules.Name = "lblRules";
        lblRules.Location = new Point(32, 658);
        lblRules.Size = new Size(650, 27);
        lblRules.Text = "Параметри гри";
        lblRules.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        lblRules.ForeColor = Color.FromArgb(170, 182, 204);
        lblRules.BackColor = Color.FromArgb(16, 24, 39);

        btnStartQuiz.TabIndex = 18;
        btnStartQuiz.Name = "btnStartQuiz";
        btnStartQuiz.Location = new Point(728, 609);
        btnStartQuiz.Size = new Size(280, 58);
        btnStartQuiz.Text = "Розпочати  →";
        btnStartQuiz.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnStartQuiz.BackColor = Color.FromArgb(109, 94, 245);
        btnStartQuiz.ForeColor = Color.FromArgb(244, 247, 252);
        btnStartQuiz.FlatStyle = FlatStyle.Flat;
        btnStartQuiz.FlatAppearance.BorderSize = 0;
        btnStartQuiz.UseVisualStyleBackColor = false;
        btnStartQuiz.Cursor = Cursors.Hand;


        Controls.Add(lblBrand);
        Controls.Add(lblDemo);
        Controls.Add(btnBack);
        Controls.Add(lblTitle);
        Controls.Add(lblCategory);
        Controls.Add(btnGames);
        Controls.Add(btnProgramming);
        Controls.Add(btnSport);
        Controls.Add(btnMovies);
        Controls.Add(btnGeneral);
        Controls.Add(lblSelection);
        Controls.Add(lblDifficulty);
        Controls.Add(btnEasy);
        Controls.Add(btnMedium);
        Controls.Add(btnHard);
        Controls.Add(lblDifficultyNote);
        Controls.Add(btnShuffle);
        Controls.Add(lblRules);
        Controls.Add(btnStartQuiz);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(16, 24, 39);
        ForeColor = Color.FromArgb(244, 247, 252);
        Font = new Font("Segoe UI", 11F);
        ClientSize = new Size(1040, 700);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        StartPosition = FormStartPosition.CenterScreen;
        Name = "GameSetupForm";
        Text = "Quiz Master";
        ResumeLayout(false);
        PerformLayout();
    }
}
