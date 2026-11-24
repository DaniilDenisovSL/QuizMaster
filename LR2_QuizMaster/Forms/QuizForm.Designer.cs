#nullable enable

using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

partial class QuizForm
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblBrand = null!;
    private Label lblDemo = null!;
    private QuizButton btnExitQuiz = null!;
    private Label lblCategory = null!;
    private QuizPanel pnlStats = null!;
    private Label lblProgress = null!;
    private Label lblScore = null!;
    private Label lblLives = null!;
    private Label lblStreak = null!;
    private Label lblTimer = null!;
    private Label lblTimerNote = null!;
    private QuizProgressBar prgTime = null!;
    private Label lblQuestion = null!;
    private QuizButton btnAnswerA = null!;
    private QuizButton btnAnswerB = null!;
    private QuizButton btnAnswerC = null!;
    private QuizButton btnAnswerD = null!;
    private Label lblFeedback = null!;
    private QuizButton btnNextQuestion = null!;

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
        btnExitQuiz = new QuizButton();
        lblCategory = new Label();
        pnlStats = new QuizPanel();
        lblProgress = new Label();
        lblScore = new Label();
        lblLives = new Label();
        lblStreak = new Label();
        lblTimer = new Label();
        lblTimerNote = new Label();
        prgTime = new QuizProgressBar();
        lblQuestion = new Label();
        btnAnswerA = new QuizButton();
        btnAnswerB = new QuizButton();
        btnAnswerC = new QuizButton();
        btnAnswerD = new QuizButton();
        lblFeedback = new Label();
        btnNextQuestion = new QuizButton();
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

        btnExitQuiz.TabIndex = 2;
        btnExitQuiz.Name = "btnExitQuiz";
        btnExitQuiz.Location = new Point(32, 78);
        btnExitQuiz.Size = new Size(160, 40);
        btnExitQuiz.Text = "←  До меню";
        btnExitQuiz.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnExitQuiz.BackColor = Color.FromArgb(27, 38, 59);
        btnExitQuiz.ForeColor = Color.FromArgb(244, 247, 252);
        btnExitQuiz.FlatStyle = FlatStyle.Flat;
        btnExitQuiz.FlatAppearance.BorderSize = 0;
        btnExitQuiz.UseVisualStyleBackColor = false;
        btnExitQuiz.Cursor = Cursors.Hand;

        lblCategory.TabIndex = 3;
        lblCategory.Name = "lblCategory";
        lblCategory.Location = new Point(216, 85);
        lblCategory.Size = new Size(700, 30);
        lblCategory.Text = "Програмування  /  Середня";
        lblCategory.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
        lblCategory.ForeColor = Color.FromArgb(170, 182, 204);
        lblCategory.BackColor = Color.FromArgb(16, 24, 39);

        pnlStats.TabIndex = 4;
        pnlStats.Name = "pnlStats";
        pnlStats.Location = new Point(32, 143);
        pnlStats.Size = new Size(976, 82);
        pnlStats.BackColor = Color.FromArgb(27, 38, 59);

        lblProgress.TabIndex = 5;
        lblProgress.Name = "lblProgress";
        lblProgress.Location = new Point(20, 23);
        lblProgress.Size = new Size(250, 33);
        lblProgress.Text = "Питання 1/10";
        lblProgress.Font = new Font("Segoe UI", 17F, FontStyle.Bold, GraphicsUnit.Point);
        lblProgress.ForeColor = Color.FromArgb(244, 247, 252);
        lblProgress.BackColor = Color.FromArgb(16, 24, 39);
        lblProgress.BackColor = Color.FromArgb(27, 38, 59);

        lblScore.TabIndex = 6;
        lblScore.Name = "lblScore";
        lblScore.Location = new Point(310, 27);
        lblScore.Size = new Size(180, 27);
        lblScore.Text = "Бали: 0";
        lblScore.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        lblScore.ForeColor = Color.FromArgb(244, 247, 252);
        lblScore.BackColor = Color.FromArgb(16, 24, 39);
        lblScore.BackColor = Color.FromArgb(27, 38, 59);

        lblLives.TabIndex = 7;
        lblLives.Name = "lblLives";
        lblLives.Location = new Point(535, 27);
        lblLives.Size = new Size(190, 27);
        lblLives.Text = "Життя: 3/3";
        lblLives.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        lblLives.ForeColor = Color.FromArgb(244, 247, 252);
        lblLives.BackColor = Color.FromArgb(16, 24, 39);
        lblLives.BackColor = Color.FromArgb(27, 38, 59);

        lblStreak.TabIndex = 8;
        lblStreak.Name = "lblStreak";
        lblStreak.Location = new Point(768, 27);
        lblStreak.Size = new Size(185, 27);
        lblStreak.Text = "Серія: 0";
        lblStreak.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        lblStreak.ForeColor = Color.FromArgb(244, 247, 252);
        lblStreak.BackColor = Color.FromArgb(16, 24, 39);
        lblStreak.BackColor = Color.FromArgb(27, 38, 59);

        lblTimer.TabIndex = 9;
        lblTimer.Name = "lblTimer";
        lblTimer.Location = new Point(32, 244);
        lblTimer.Size = new Size(400, 26);
        lblTimer.Text = "Час: 18 с";
        lblTimer.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        lblTimer.ForeColor = Color.FromArgb(244, 247, 252);
        lblTimer.BackColor = Color.FromArgb(16, 24, 39);

        lblTimerNote.TabIndex = 10;
        lblTimerNote.Name = "lblTimerNote";
        lblTimerNote.Location = new Point(694, 247);
        lblTimerNote.Size = new Size(314, 24);
        lblTimerNote.Text = "Демонстраційне питання";
        lblTimerNote.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        lblTimerNote.ForeColor = Color.FromArgb(170, 182, 204);
        lblTimerNote.BackColor = Color.FromArgb(16, 24, 39);

        prgTime.TabIndex = 11;
        prgTime.Name = "prgTime";
        prgTime.Location = new Point(32, 284);
        prgTime.Size = new Size(976, 10);
        prgTime.ForeColor = Color.FromArgb(39, 198, 152);
        prgTime.Maximum = 20;
        prgTime.Value = 18;


        lblQuestion.TabIndex = 12;
        lblQuestion.Name = "lblQuestion";
        lblQuestion.Location = new Point(32, 325);
        lblQuestion.Size = new Size(976, 102);
        lblQuestion.Text = "Яка мова програмування\r\nвикористовується у цьому проєкті?";
        lblQuestion.Font = new Font("Segoe UI", 26F, FontStyle.Bold, GraphicsUnit.Point);
        lblQuestion.ForeColor = Color.FromArgb(244, 247, 252);
        lblQuestion.BackColor = Color.FromArgb(16, 24, 39);

        btnAnswerA.TabIndex = 13;
        btnAnswerA.Name = "btnAnswerA";
        btnAnswerA.Location = new Point(32, 456);
        btnAnswerA.Size = new Size(476, 67);
        btnAnswerA.Text = "A    Python";
        btnAnswerA.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnAnswerA.BackColor = Color.FromArgb(27, 38, 59);
        btnAnswerA.ForeColor = Color.FromArgb(244, 247, 252);
        btnAnswerA.FlatStyle = FlatStyle.Flat;
        btnAnswerA.FlatAppearance.BorderSize = 0;
        btnAnswerA.UseVisualStyleBackColor = false;
        btnAnswerA.Cursor = Cursors.Hand;

        btnAnswerB.TabIndex = 14;
        btnAnswerB.Name = "btnAnswerB";
        btnAnswerB.Location = new Point(532, 456);
        btnAnswerB.Size = new Size(476, 67);
        btnAnswerB.Text = "B    C#";
        btnAnswerB.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnAnswerB.BackColor = Color.FromArgb(27, 38, 59);
        btnAnswerB.ForeColor = Color.FromArgb(244, 247, 252);
        btnAnswerB.FlatStyle = FlatStyle.Flat;
        btnAnswerB.FlatAppearance.BorderSize = 0;
        btnAnswerB.UseVisualStyleBackColor = false;
        btnAnswerB.Cursor = Cursors.Hand;

        btnAnswerC.TabIndex = 15;
        btnAnswerC.Name = "btnAnswerC";
        btnAnswerC.Location = new Point(32, 539);
        btnAnswerC.Size = new Size(476, 67);
        btnAnswerC.Text = "C    Java";
        btnAnswerC.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnAnswerC.BackColor = Color.FromArgb(27, 38, 59);
        btnAnswerC.ForeColor = Color.FromArgb(244, 247, 252);
        btnAnswerC.FlatStyle = FlatStyle.Flat;
        btnAnswerC.FlatAppearance.BorderSize = 0;
        btnAnswerC.UseVisualStyleBackColor = false;
        btnAnswerC.Cursor = Cursors.Hand;

        btnAnswerD.TabIndex = 16;
        btnAnswerD.Name = "btnAnswerD";
        btnAnswerD.Location = new Point(532, 539);
        btnAnswerD.Size = new Size(476, 67);
        btnAnswerD.Text = "D    C++";
        btnAnswerD.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnAnswerD.BackColor = Color.FromArgb(27, 38, 59);
        btnAnswerD.ForeColor = Color.FromArgb(244, 247, 252);
        btnAnswerD.FlatStyle = FlatStyle.Flat;
        btnAnswerD.FlatAppearance.BorderSize = 0;
        btnAnswerD.UseVisualStyleBackColor = false;
        btnAnswerD.Cursor = Cursors.Hand;

        lblFeedback.TabIndex = 17;
        lblFeedback.Name = "lblFeedback";
        lblFeedback.Location = new Point(32, 636);
        lblFeedback.Size = new Size(600, 28);
        lblFeedback.Text = "Обери один варіант відповіді";
        lblFeedback.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        lblFeedback.ForeColor = Color.FromArgb(170, 182, 204);
        lblFeedback.BackColor = Color.FromArgb(16, 24, 39);

        btnNextQuestion.TabIndex = 18;
        btnNextQuestion.Name = "btnNextQuestion";
        btnNextQuestion.Location = new Point(768, 624);
        btnNextQuestion.Size = new Size(240, 47);
        btnNextQuestion.Text = "Далі  →";
        btnNextQuestion.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnNextQuestion.BackColor = Color.FromArgb(109, 94, 245);
        btnNextQuestion.ForeColor = Color.FromArgb(244, 247, 252);
        btnNextQuestion.FlatStyle = FlatStyle.Flat;
        btnNextQuestion.FlatAppearance.BorderSize = 0;
        btnNextQuestion.UseVisualStyleBackColor = false;
        btnNextQuestion.Cursor = Cursors.Hand;

        btnNextQuestion.Enabled = false;
        btnNextQuestion.BackColor = Color.FromArgb(36, 49, 73);

        Controls.Add(lblBrand);
        Controls.Add(lblDemo);
        Controls.Add(btnExitQuiz);
        Controls.Add(lblCategory);
        Controls.Add(pnlStats);
        pnlStats.Controls.Add(lblProgress);
        pnlStats.Controls.Add(lblScore);
        pnlStats.Controls.Add(lblLives);
        pnlStats.Controls.Add(lblStreak);
        Controls.Add(lblTimer);
        Controls.Add(lblTimerNote);
        Controls.Add(prgTime);
        Controls.Add(lblQuestion);
        Controls.Add(btnAnswerA);
        Controls.Add(btnAnswerB);
        Controls.Add(btnAnswerC);
        Controls.Add(btnAnswerD);
        Controls.Add(lblFeedback);
        Controls.Add(btnNextQuestion);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(16, 24, 39);
        ForeColor = Color.FromArgb(244, 247, 252);
        Font = new Font("Segoe UI", 11F);
        ClientSize = new Size(1040, 700);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        StartPosition = FormStartPosition.CenterScreen;
        Name = "QuizForm";
        Text = "Quiz Master";
        ResumeLayout(false);
        PerformLayout();
    }
}
