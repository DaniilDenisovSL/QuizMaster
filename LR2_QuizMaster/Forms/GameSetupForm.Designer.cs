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
    private RadioButton rdoEasy = null!;
    private RadioButton rdoMedium = null!;
    private RadioButton rdoHard = null!;
    private CheckBox chkShuffleQuestions = null!;
    private CheckBox chkShuffleAnswers = null!;
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
        rdoEasy = new RadioButton();
        rdoMedium = new RadioButton();
        rdoHard = new RadioButton();
        chkShuffleQuestions = new CheckBox();
        chkShuffleAnswers = new CheckBox();
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
        lblDemo.Text = "ДЕМОНСТРАЦІЙНИЙ РЕЖИМ";
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

        rdoEasy.TabIndex = 12;
        rdoEasy.Name = "rdoEasy";
        rdoEasy.Location = new Point(32, 464);
        rdoEasy.Size = new Size(155, 35);
        rdoEasy.Text = "Легка";
        rdoEasy.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        rdoEasy.ForeColor = Color.FromArgb(244, 247, 252);

        rdoMedium.TabIndex = 13;
        rdoMedium.Name = "rdoMedium";
        rdoMedium.Location = new Point(206, 464);
        rdoMedium.Size = new Size(165, 35);
        rdoMedium.Text = "Середня";
        rdoMedium.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        rdoMedium.ForeColor = Color.FromArgb(244, 247, 252);
        rdoMedium.Checked = true;

        rdoHard.TabIndex = 14;
        rdoHard.Name = "rdoHard";
        rdoHard.Location = new Point(390, 464);
        rdoHard.Size = new Size(170, 35);
        rdoHard.Text = "Складна";
        rdoHard.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        rdoHard.ForeColor = Color.FromArgb(244, 247, 252);

        chkShuffleQuestions.TabIndex = 15;
        chkShuffleQuestions.Name = "chkShuffleQuestions";
        chkShuffleQuestions.Location = new Point(32, 527);
        chkShuffleQuestions.Size = new Size(400, 32);
        chkShuffleQuestions.Text = "Перемішувати питання";
        chkShuffleQuestions.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        chkShuffleQuestions.ForeColor = Color.FromArgb(244, 247, 252);
        chkShuffleQuestions.Checked = true;

        chkShuffleAnswers.TabIndex = 16;
        chkShuffleAnswers.Name = "chkShuffleAnswers";
        chkShuffleAnswers.Location = new Point(32, 566);
        chkShuffleAnswers.Size = new Size(400, 32);
        chkShuffleAnswers.Text = "Перемішувати відповіді";
        chkShuffleAnswers.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        chkShuffleAnswers.ForeColor = Color.FromArgb(244, 247, 252);
        chkShuffleAnswers.Checked = true;

        lblRules.TabIndex = 17;
        lblRules.Name = "lblRules";
        lblRules.Location = new Point(32, 632);
        lblRules.Size = new Size(650, 27);
        lblRules.Text = "Демонстрація: 10 питань · 20 с · 3 життя";
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
        Controls.Add(rdoEasy);
        Controls.Add(rdoMedium);
        Controls.Add(rdoHard);
        Controls.Add(chkShuffleQuestions);
        Controls.Add(chkShuffleAnswers);
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
