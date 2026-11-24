#nullable enable

using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

partial class ResultsForm
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblBrand = null!;
    private Label lblDemo = null!;
    private Label lblHeading = null!;
    private Label lblPlayer = null!;
    private QuizPanel pnlSummary = null!;
    private Label lblScoreCaption = null!;
    private Label lblTotalScore = null!;
    private Label lblCorrect = null!;
    private Label lblAccuracy = null!;
    private Label lblBestStreak = null!;
    private Label lblSummaryNote = null!;
    private QuizButton btnPlayAgain = null!;
    private QuizButton btnLeaderboard = null!;
    private QuizButton btnMainMenu = null!;

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
        lblHeading = new Label();
        lblPlayer = new Label();
        pnlSummary = new QuizPanel();
        lblScoreCaption = new Label();
        lblTotalScore = new Label();
        lblCorrect = new Label();
        lblAccuracy = new Label();
        lblBestStreak = new Label();
        lblSummaryNote = new Label();
        btnPlayAgain = new QuizButton();
        btnLeaderboard = new QuizButton();
        btnMainMenu = new QuizButton();
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

        lblHeading.TabIndex = 2;
        lblHeading.Name = "lblHeading";
        lblHeading.Location = new Point(32, 105);
        lblHeading.Size = new Size(976, 57);
        lblHeading.Text = "Гру завершено!";
        lblHeading.Font = new Font("Segoe UI", 32F, FontStyle.Bold, GraphicsUnit.Point);
        lblHeading.ForeColor = Color.FromArgb(244, 247, 252);
        lblHeading.BackColor = Color.FromArgb(16, 24, 39);

        lblPlayer.TabIndex = 3;
        lblPlayer.Name = "lblPlayer";
        lblPlayer.Location = new Point(32, 170);
        lblPlayer.Size = new Size(976, 35);
        lblPlayer.Text = "Гравець · Програмування · Середня";
        lblPlayer.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
        lblPlayer.ForeColor = Color.FromArgb(170, 182, 204);
        lblPlayer.BackColor = Color.FromArgb(16, 24, 39);

        pnlSummary.TabIndex = 4;
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Location = new Point(32, 239);
        pnlSummary.Size = new Size(976, 268);
        pnlSummary.BackColor = Color.FromArgb(27, 38, 59);

        lblScoreCaption.TabIndex = 5;
        lblScoreCaption.Name = "lblScoreCaption";
        lblScoreCaption.Location = new Point(28, 26);
        lblScoreCaption.Size = new Size(800, 30);
        lblScoreCaption.Text = "ТВІЙ РЕЗУЛЬТАТ";
        lblScoreCaption.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        lblScoreCaption.ForeColor = Color.FromArgb(170, 182, 204);
        lblScoreCaption.BackColor = Color.FromArgb(16, 24, 39);
        lblScoreCaption.BackColor = Color.FromArgb(27, 38, 59);

        lblTotalScore.TabIndex = 6;
        lblTotalScore.Name = "lblTotalScore";
        lblTotalScore.Location = new Point(25, 71);
        lblTotalScore.Size = new Size(880, 78);
        lblTotalScore.Text = "— балів";
        lblTotalScore.Font = new Font("Segoe UI", 46F, FontStyle.Bold, GraphicsUnit.Point);
        lblTotalScore.ForeColor = Color.FromArgb(244, 247, 252);
        lblTotalScore.BackColor = Color.FromArgb(16, 24, 39);
        lblTotalScore.BackColor = Color.FromArgb(27, 38, 59);

        lblCorrect.TabIndex = 7;
        lblCorrect.Name = "lblCorrect";
        lblCorrect.Location = new Point(28, 190);
        lblCorrect.Size = new Size(280, 32);
        lblCorrect.Text = "Правильно: —";
        lblCorrect.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
        lblCorrect.ForeColor = Color.FromArgb(244, 247, 252);
        lblCorrect.BackColor = Color.FromArgb(16, 24, 39);
        lblCorrect.BackColor = Color.FromArgb(27, 38, 59);

        lblAccuracy.TabIndex = 8;
        lblAccuracy.Name = "lblAccuracy";
        lblAccuracy.Location = new Point(341, 190);
        lblAccuracy.Size = new Size(280, 32);
        lblAccuracy.Text = "Точність: —";
        lblAccuracy.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
        lblAccuracy.ForeColor = Color.FromArgb(244, 247, 252);
        lblAccuracy.BackColor = Color.FromArgb(16, 24, 39);
        lblAccuracy.BackColor = Color.FromArgb(27, 38, 59);

        lblBestStreak.TabIndex = 9;
        lblBestStreak.Name = "lblBestStreak";
        lblBestStreak.Location = new Point(648, 190);
        lblBestStreak.Size = new Size(310, 32);
        lblBestStreak.Text = "Найкраща серія: —";
        lblBestStreak.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
        lblBestStreak.ForeColor = Color.FromArgb(244, 247, 252);
        lblBestStreak.BackColor = Color.FromArgb(16, 24, 39);
        lblBestStreak.BackColor = Color.FromArgb(27, 38, 59);

        lblSummaryNote.TabIndex = 10;
        lblSummaryNote.Name = "lblSummaryNote";
        lblSummaryNote.Location = new Point(32, 527);
        lblSummaryNote.Size = new Size(976, 32);
        lblSummaryNote.Text = "";
        lblSummaryNote.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        lblSummaryNote.ForeColor = Color.FromArgb(170, 182, 204);
        lblSummaryNote.BackColor = Color.FromArgb(16, 24, 39);

        btnPlayAgain.TabIndex = 11;
        btnPlayAgain.Name = "btnPlayAgain";
        btnPlayAgain.Location = new Point(32, 610);
        btnPlayAgain.Size = new Size(312, 58);
        btnPlayAgain.Text = "Зіграти ще";
        btnPlayAgain.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnPlayAgain.BackColor = Color.FromArgb(109, 94, 245);
        btnPlayAgain.ForeColor = Color.FromArgb(244, 247, 252);
        btnPlayAgain.FlatStyle = FlatStyle.Flat;
        btnPlayAgain.FlatAppearance.BorderSize = 0;
        btnPlayAgain.UseVisualStyleBackColor = false;
        btnPlayAgain.Cursor = Cursors.Hand;

        btnLeaderboard.TabIndex = 12;
        btnLeaderboard.Name = "btnLeaderboard";
        btnLeaderboard.Location = new Point(364, 610);
        btnLeaderboard.Size = new Size(312, 58);
        btnLeaderboard.Text = "Рекорди";
        btnLeaderboard.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnLeaderboard.BackColor = Color.FromArgb(27, 38, 59);
        btnLeaderboard.ForeColor = Color.FromArgb(244, 247, 252);
        btnLeaderboard.FlatStyle = FlatStyle.Flat;
        btnLeaderboard.FlatAppearance.BorderSize = 0;
        btnLeaderboard.UseVisualStyleBackColor = false;
        btnLeaderboard.Cursor = Cursors.Hand;

        btnMainMenu.TabIndex = 13;
        btnMainMenu.Name = "btnMainMenu";
        btnMainMenu.Location = new Point(696, 610);
        btnMainMenu.Size = new Size(312, 58);
        btnMainMenu.Text = "Головне меню";
        btnMainMenu.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnMainMenu.BackColor = Color.FromArgb(27, 38, 59);
        btnMainMenu.ForeColor = Color.FromArgb(244, 247, 252);
        btnMainMenu.FlatStyle = FlatStyle.Flat;
        btnMainMenu.FlatAppearance.BorderSize = 0;
        btnMainMenu.UseVisualStyleBackColor = false;
        btnMainMenu.Cursor = Cursors.Hand;


        Controls.Add(lblBrand);
        Controls.Add(lblDemo);
        Controls.Add(lblHeading);
        Controls.Add(lblPlayer);
        Controls.Add(pnlSummary);
        pnlSummary.Controls.Add(lblScoreCaption);
        pnlSummary.Controls.Add(lblTotalScore);
        pnlSummary.Controls.Add(lblCorrect);
        pnlSummary.Controls.Add(lblAccuracy);
        pnlSummary.Controls.Add(lblBestStreak);
        Controls.Add(lblSummaryNote);
        Controls.Add(btnPlayAgain);
        Controls.Add(btnLeaderboard);
        Controls.Add(btnMainMenu);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(16, 24, 39);
        ForeColor = Color.FromArgb(244, 247, 252);
        Font = new Font("Segoe UI", 11F);
        ClientSize = new Size(1040, 700);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        StartPosition = FormStartPosition.CenterScreen;
        Name = "ResultsForm";
        Text = "Quiz Master";
        ResumeLayout(false);
        PerformLayout();
    }
}
