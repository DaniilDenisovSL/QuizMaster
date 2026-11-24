#nullable enable

using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

partial class MainMenuForm
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblBrand = null!;
    private Label lblDemo = null!;
    private Label lblEyebrow = null!;
    private Label lblTitle = null!;
    private Label lblIntro = null!;
    private Label lblFeatures = null!;
    private Label lblFeatures2 = null!;
    private QuizPanel pnlStart = null!;
    private Label lblWelcome = null!;
    private Label lblPlayerName = null!;
    private TextBox txtPlayerName = null!;
    private QuizButton btnStart = null!;
    private QuizButton btnLeaderboard = null!;
    private QuizButton btnExit = null!;
    private Label lblFooter = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private Label lblHeaderRule = null!;

    private QuizPanel pnlNameField = null!;

    private void InitializeComponent()
    {
        lblBrand = new Label();
        lblDemo = new Label();
        lblEyebrow = new Label();
        lblTitle = new Label();
        lblIntro = new Label();
        lblFeatures = new Label();
        lblFeatures2 = new Label();
        pnlStart = new QuizPanel();
        lblWelcome = new Label();
        lblPlayerName = new Label();
        txtPlayerName = new TextBox();
        btnStart = new QuizButton();
        btnLeaderboard = new QuizButton();
        btnExit = new QuizButton();
        lblFooter = new Label();
        lblHeaderRule = new Label();
        pnlNameField = new QuizPanel();
        SuspendLayout();
        pnlNameField.Name = "pnlNameField";
        pnlNameField.Location = new Point(28, 133);
        pnlNameField.Size = new Size(332, 44);
        pnlNameField.CornerRadius = 9;
        pnlNameField.BackColor = Color.FromArgb(36, 49, 73);
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

        lblEyebrow.TabIndex = 2;
        lblEyebrow.Name = "lblEyebrow";
        lblEyebrow.Location = new Point(52, 142);
        lblEyebrow.Size = new Size(480, 26);
        lblEyebrow.Text = "ВІКТОРИНА ДЛЯ ДОПИТЛИВИХ";
        lblEyebrow.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        lblEyebrow.ForeColor = Color.FromArgb(170, 182, 204);
        lblEyebrow.BackColor = Color.FromArgb(16, 24, 39);

        lblTitle.TabIndex = 3;
        lblTitle.Name = "lblTitle";
        lblTitle.Location = new Point(48, 181);
        lblTitle.Size = new Size(520, 128);
        lblTitle.Text = "Твої знання.\r\nТвоя перемога.";
        lblTitle.Font = new Font("Segoe UI", 37F, FontStyle.Bold, GraphicsUnit.Point);
        lblTitle.ForeColor = Color.FromArgb(244, 247, 252);
        lblTitle.BackColor = Color.FromArgb(16, 24, 39);

        lblIntro.TabIndex = 4;
        lblIntro.Name = "lblIntro";
        lblIntro.Location = new Point(52, 328);
        lblIntro.Size = new Size(475, 65);
        lblIntro.Text = "Обирай категорію, відповідай на питання\r\nта встановлюй власний рекорд.";
        lblIntro.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
        lblIntro.ForeColor = Color.FromArgb(170, 182, 204);
        lblIntro.BackColor = Color.FromArgb(16, 24, 39);

        lblFeatures.TabIndex = 5;
        lblFeatures.Name = "lblFeatures";
        lblFeatures.Location = new Point(52, 438);
        lblFeatures.Size = new Size(480, 30);
        lblFeatures.Text = "5 категорій  /  3 рівні складності";
        lblFeatures.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        lblFeatures.ForeColor = Color.FromArgb(244, 247, 252);
        lblFeatures.BackColor = Color.FromArgb(16, 24, 39);

        lblFeatures2.TabIndex = 6;
        lblFeatures2.Name = "lblFeatures2";
        lblFeatures2.Location = new Point(52, 477);
        lblFeatures2.Size = new Size(480, 30);
        lblFeatures2.Text = "Бали  •  Життя  •  Серія відповідей";
        lblFeatures2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        lblFeatures2.ForeColor = Color.FromArgb(170, 182, 204);
        lblFeatures2.BackColor = Color.FromArgb(16, 24, 39);

        pnlStart.TabIndex = 7;
        pnlStart.Name = "pnlStart";
        pnlStart.Location = new Point(600, 132);
        pnlStart.Size = new Size(388, 434);
        pnlStart.BackColor = Color.FromArgb(27, 38, 59);

        lblWelcome.TabIndex = 8;
        lblWelcome.Name = "lblWelcome";
        lblWelcome.Location = new Point(28, 30);
        lblWelcome.Size = new Size(330, 45);
        lblWelcome.Text = "Почнімо гру";
        lblWelcome.Font = new Font("Segoe UI", 23F, FontStyle.Bold, GraphicsUnit.Point);
        lblWelcome.ForeColor = Color.FromArgb(244, 247, 252);
        lblWelcome.BackColor = Color.FromArgb(16, 24, 39);
        lblWelcome.BackColor = Color.FromArgb(27, 38, 59);

        lblPlayerName.TabIndex = 9;
        lblPlayerName.Name = "lblPlayerName";
        lblPlayerName.Location = new Point(28, 96);
        lblPlayerName.Size = new Size(328, 26);
        lblPlayerName.Text = "Ім’я гравця";
        lblPlayerName.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        lblPlayerName.ForeColor = Color.FromArgb(170, 182, 204);
        lblPlayerName.BackColor = Color.FromArgb(16, 24, 39);
        lblPlayerName.BackColor = Color.FromArgb(27, 38, 59);

        txtPlayerName.TabIndex = 10;
        txtPlayerName.Name = "txtPlayerName";
        txtPlayerName.Location = new Point(12, 8);
        txtPlayerName.Size = new Size(308, 30);
        txtPlayerName.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
        txtPlayerName.BackColor = Color.FromArgb(36, 49, 73);
        txtPlayerName.ForeColor = Color.FromArgb(244, 247, 252);
        txtPlayerName.BorderStyle = BorderStyle.None;
        txtPlayerName.MaxLength = 20;
        txtPlayerName.PlaceholderText = "Введи своє ім’я";

        btnStart.TabIndex = 11;
        btnStart.Name = "btnStart";
        btnStart.Location = new Point(28, 199);
        btnStart.Size = new Size(332, 54);
        btnStart.Text = "Почати гру  →";
        btnStart.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnStart.BackColor = Color.FromArgb(109, 94, 245);
        btnStart.ForeColor = Color.FromArgb(244, 247, 252);
        btnStart.FlatStyle = FlatStyle.Flat;
        btnStart.FlatAppearance.BorderSize = 0;
        btnStart.UseVisualStyleBackColor = false;
        btnStart.Cursor = Cursors.Hand;

        btnLeaderboard.TabIndex = 12;
        btnLeaderboard.Name = "btnLeaderboard";
        btnLeaderboard.Location = new Point(28, 267);
        btnLeaderboard.Size = new Size(332, 50);
        btnLeaderboard.Text = "Таблиця рекордів";
        btnLeaderboard.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnLeaderboard.BackColor = Color.FromArgb(27, 38, 59);
        btnLeaderboard.ForeColor = Color.FromArgb(244, 247, 252);
        btnLeaderboard.FlatStyle = FlatStyle.Flat;
        btnLeaderboard.FlatAppearance.BorderSize = 0;
        btnLeaderboard.UseVisualStyleBackColor = false;
        btnLeaderboard.Cursor = Cursors.Hand;

        btnExit.TabIndex = 13;
        btnExit.Name = "btnExit";
        btnExit.Location = new Point(28, 333);
        btnExit.Size = new Size(332, 50);
        btnExit.Text = "Вийти";
        btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        btnExit.BackColor = Color.FromArgb(27, 38, 59);
        btnExit.ForeColor = Color.FromArgb(244, 247, 252);
        btnExit.FlatStyle = FlatStyle.Flat;
        btnExit.FlatAppearance.BorderSize = 0;
        btnExit.UseVisualStyleBackColor = false;
        btnExit.Cursor = Cursors.Hand;

        lblFooter.TabIndex = 14;
        lblFooter.Name = "lblFooter";
        lblFooter.Location = new Point(32, 649);
        lblFooter.Size = new Size(650, 24);
        lblFooter.Text = "Quiz Master  /  Командний проєкт";
        lblFooter.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        lblFooter.ForeColor = Color.FromArgb(170, 182, 204);
        lblFooter.BackColor = Color.FromArgb(16, 24, 39);


        Controls.Add(lblBrand);
        Controls.Add(lblDemo);
        Controls.Add(lblEyebrow);
        Controls.Add(lblTitle);
        Controls.Add(lblIntro);
        Controls.Add(lblFeatures);
        Controls.Add(lblFeatures2);
        Controls.Add(pnlStart);
        pnlStart.Controls.Add(lblWelcome);
        pnlStart.Controls.Add(lblPlayerName);
        pnlNameField.Controls.Add(txtPlayerName);
        pnlStart.Controls.Add(pnlNameField);
        pnlStart.Controls.Add(btnStart);
        pnlStart.Controls.Add(btnLeaderboard);
        pnlStart.Controls.Add(btnExit);
        Controls.Add(lblFooter);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(16, 24, 39);
        ForeColor = Color.FromArgb(244, 247, 252);
        Font = new Font("Segoe UI", 11F);
        ClientSize = new Size(1040, 700);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        StartPosition = FormStartPosition.CenterScreen;
        Name = "MainMenuForm";
        Text = "Quiz Master";
        ResumeLayout(false);
        PerformLayout();
    }
}
