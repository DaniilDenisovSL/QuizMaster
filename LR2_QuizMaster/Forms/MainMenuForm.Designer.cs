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
        pnlNameField = new QuizPanel();
        txtPlayerName = new TextBox();
        btnStart = new QuizButton();
        btnLeaderboard = new QuizButton();
        btnExit = new QuizButton();
        lblFooter = new Label();
        lblHeaderRule = new Label();
        pnlStart.SuspendLayout();
        pnlNameField.SuspendLayout();
        SuspendLayout();
        // 
        // lblBrand
        // 
        lblBrand.BackColor = Color.FromArgb(16, 24, 39);
        lblBrand.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblBrand.ForeColor = Color.FromArgb(244, 247, 252);
        lblBrand.Location = new Point(32, 22);
        lblBrand.Name = "lblBrand";
        lblBrand.Size = new Size(250, 30);
        lblBrand.TabIndex = 0;
        lblBrand.Text = "QUIZ MASTER";
        // 
        // lblDemo
        // 
        lblDemo.BackColor = Color.FromArgb(16, 24, 39);
        lblDemo.Font = new Font("Segoe UI", 9F);
        lblDemo.ForeColor = Color.FromArgb(170, 182, 204);
        lblDemo.Location = new Point(680, 24);
        lblDemo.Name = "lblDemo";
        lblDemo.Size = new Size(328, 25);
        lblDemo.TabIndex = 1;
        lblDemo.Visible = false;
        // 
        // lblEyebrow
        // 
        lblEyebrow.BackColor = Color.FromArgb(16, 24, 39);
        lblEyebrow.Font = new Font("Segoe UI", 10F);
        lblEyebrow.ForeColor = Color.FromArgb(170, 182, 204);
        lblEyebrow.Location = new Point(52, 142);
        lblEyebrow.Name = "lblEyebrow";
        lblEyebrow.Size = new Size(480, 26);
        lblEyebrow.TabIndex = 2;
        lblEyebrow.Text = "ВІКТОРИНА ДЛЯ ДОПИТЛИВИХ";
        // 
        // lblTitle
        // 
        lblTitle.BackColor = Color.FromArgb(16, 24, 39);
        lblTitle.Font = new Font("Segoe UI", 37F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(244, 247, 252);
        lblTitle.Location = new Point(48, 181);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(520, 128);
        lblTitle.TabIndex = 3;
        lblTitle.Text = "Перевір себе";
        // 
        // lblIntro
        // 
        lblIntro.BackColor = Color.FromArgb(16, 24, 39);
        lblIntro.Font = new Font("Segoe UI", 14F);
        lblIntro.ForeColor = Color.FromArgb(170, 182, 204);
        lblIntro.Location = new Point(52, 328);
        lblIntro.Name = "lblIntro";
        lblIntro.Size = new Size(475, 65);
        lblIntro.TabIndex = 4;
        lblIntro.Text = "Обирай категорію, відповідай на питання\r\nта встановлюй власний рекорд.";
        // 
        // lblFeatures
        // 
        lblFeatures.BackColor = Color.FromArgb(16, 24, 39);
        lblFeatures.Font = new Font("Segoe UI", 12F);
        lblFeatures.ForeColor = Color.FromArgb(244, 247, 252);
        lblFeatures.Location = new Point(52, 438);
        lblFeatures.Name = "lblFeatures";
        lblFeatures.Size = new Size(480, 30);
        lblFeatures.TabIndex = 5;
        lblFeatures.Text = "5 категорій  /  3 рівні складності";
        // 
        // lblFeatures2
        // 
        lblFeatures2.BackColor = Color.FromArgb(16, 24, 39);
        lblFeatures2.Font = new Font("Segoe UI", 11F);
        lblFeatures2.ForeColor = Color.FromArgb(170, 182, 204);
        lblFeatures2.Location = new Point(52, 477);
        lblFeatures2.Name = "lblFeatures2";
        lblFeatures2.Size = new Size(480, 30);
        lblFeatures2.TabIndex = 6;
        lblFeatures2.Text = "Бали  •  Життя  •  Серія відповідей";
        // 
        // pnlStart
        // 
        pnlStart.BackColor = Color.FromArgb(27, 38, 59);
        pnlStart.Controls.Add(lblWelcome);
        pnlStart.Controls.Add(lblPlayerName);
        pnlStart.Controls.Add(pnlNameField);
        pnlStart.Controls.Add(btnStart);
        pnlStart.Controls.Add(btnLeaderboard);
        pnlStart.Controls.Add(btnExit);
        pnlStart.Location = new Point(600, 132);
        pnlStart.Name = "pnlStart";
        pnlStart.Size = new Size(388, 434);
        pnlStart.TabIndex = 7;
        // 
        // lblWelcome
        // 
        lblWelcome.BackColor = Color.FromArgb(27, 38, 59);
        lblWelcome.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
        lblWelcome.ForeColor = Color.FromArgb(244, 247, 252);
        lblWelcome.Location = new Point(28, 30);
        lblWelcome.Name = "lblWelcome";
        lblWelcome.Size = new Size(330, 45);
        lblWelcome.TabIndex = 8;
        lblWelcome.Text = "Почнімо гру";
        // 
        // lblPlayerName
        // 
        lblPlayerName.BackColor = Color.FromArgb(27, 38, 59);
        lblPlayerName.Font = new Font("Segoe UI", 11F);
        lblPlayerName.ForeColor = Color.FromArgb(170, 182, 204);
        lblPlayerName.Location = new Point(28, 96);
        lblPlayerName.Name = "lblPlayerName";
        lblPlayerName.Size = new Size(328, 26);
        lblPlayerName.TabIndex = 9;
        lblPlayerName.Text = "Ім’я гравця";
        // 
        // pnlNameField
        // 
        pnlNameField.BackColor = Color.FromArgb(36, 49, 73);
        pnlNameField.Controls.Add(txtPlayerName);
        pnlNameField.CornerRadius = 9;
        pnlNameField.Location = new Point(28, 133);
        pnlNameField.Name = "pnlNameField";
        pnlNameField.Size = new Size(332, 44);
        pnlNameField.TabIndex = 10;
        // 
        // txtPlayerName
        // 
        txtPlayerName.BackColor = Color.FromArgb(36, 49, 73);
        txtPlayerName.BorderStyle = BorderStyle.None;
        txtPlayerName.Font = new Font("Segoe UI", 13F);
        txtPlayerName.ForeColor = Color.FromArgb(244, 247, 252);
        txtPlayerName.Location = new Point(12, 8);
        txtPlayerName.MaxLength = 20;
        txtPlayerName.Name = "txtPlayerName";
        txtPlayerName.PlaceholderText = "Введи своє ім’я";
        txtPlayerName.Size = new Size(308, 35);
        txtPlayerName.TabIndex = 10;
        // 
        // btnStart
        // 
        btnStart.BackColor = Color.FromArgb(109, 94, 245);
        btnStart.Cursor = Cursors.Hand;
        btnStart.FlatAppearance.BorderSize = 0;
        btnStart.FlatStyle = FlatStyle.Flat;
        btnStart.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnStart.ForeColor = Color.FromArgb(244, 247, 252);
        btnStart.Location = new Point(28, 199);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(332, 54);
        btnStart.TabIndex = 11;
        btnStart.Text = "Почати гру  →";
        btnStart.UseVisualStyleBackColor = false;
        // 
        // btnLeaderboard
        // 
        btnLeaderboard.BackColor = Color.FromArgb(27, 38, 59);
        btnLeaderboard.Cursor = Cursors.Hand;
        btnLeaderboard.FlatAppearance.BorderSize = 0;
        btnLeaderboard.FlatStyle = FlatStyle.Flat;
        btnLeaderboard.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnLeaderboard.ForeColor = Color.FromArgb(244, 247, 252);
        btnLeaderboard.Location = new Point(28, 267);
        btnLeaderboard.Name = "btnLeaderboard";
        btnLeaderboard.Size = new Size(332, 50);
        btnLeaderboard.TabIndex = 12;
        btnLeaderboard.Text = "Таблиця рекордів";
        btnLeaderboard.UseVisualStyleBackColor = false;
        // 
        // btnExit
        // 
        btnExit.BackColor = Color.FromArgb(27, 38, 59);
        btnExit.Cursor = Cursors.Hand;
        btnExit.FlatAppearance.BorderSize = 0;
        btnExit.FlatStyle = FlatStyle.Flat;
        btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnExit.ForeColor = Color.FromArgb(244, 247, 252);
        btnExit.Location = new Point(28, 333);
        btnExit.Name = "btnExit";
        btnExit.Size = new Size(332, 50);
        btnExit.TabIndex = 13;
        btnExit.Text = "Вийти";
        btnExit.UseVisualStyleBackColor = false;
        // 
        // lblFooter
        // 
        lblFooter.BackColor = Color.FromArgb(16, 24, 39);
        lblFooter.Font = new Font("Segoe UI", 10F);
        lblFooter.ForeColor = Color.FromArgb(170, 182, 204);
        lblFooter.Location = new Point(32, 649);
        lblFooter.Name = "lblFooter";
        lblFooter.Size = new Size(650, 24);
        lblFooter.TabIndex = 14;
        lblFooter.Text = "Quiz Master  /  Командний проєкт";
        // 
        // lblHeaderRule
        // 
        lblHeaderRule.BackColor = Color.FromArgb(36, 49, 73);
        lblHeaderRule.Location = new Point(32, 63);
        lblHeaderRule.Name = "lblHeaderRule";
        lblHeaderRule.Size = new Size(976, 1);
        lblHeaderRule.TabIndex = 0;
        // 
        // MainMenuForm
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(16, 24, 39);
        ClientSize = new Size(1040, 700);
        Controls.Add(lblHeaderRule);
        Controls.Add(lblBrand);
        Controls.Add(lblDemo);
        Controls.Add(lblEyebrow);
        Controls.Add(lblTitle);
        Controls.Add(lblIntro);
        Controls.Add(lblFeatures);
        Controls.Add(lblFeatures2);
        Controls.Add(pnlStart);
        Controls.Add(lblFooter);
        Font = new Font("Segoe UI", 11F);
        ForeColor = Color.FromArgb(244, 247, 252);
        Name = "MainMenuForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Quiz Master";
        pnlStart.ResumeLayout(false);
        pnlNameField.ResumeLayout(false);
        pnlNameField.PerformLayout();
        ResumeLayout(false);
    }
}
