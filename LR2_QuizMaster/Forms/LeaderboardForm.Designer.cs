#nullable enable

using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

partial class LeaderboardForm
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblBrand = null!;
    private Label lblDemo = null!;
    private QuizButton btnBack = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Label lblFilter = null!;
    private QuizComboBox cmbCategory = null!;
    private DataGridView dgvLeaderboard = null!;
    private Label lblEmptyState = null!;
    private Label lblDemoData = null!;
    private Label lblFooter = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private DataGridViewTextBoxColumn colRank = null!;
    private DataGridViewTextBoxColumn colPlayer = null!;
    private DataGridViewTextBoxColumn colCategory = null!;
    private DataGridViewTextBoxColumn colDifficulty = null!;
    private DataGridViewTextBoxColumn colScore = null!;

    private Label lblHeaderRule = null!;

    private void InitializeComponent()
    {
        lblBrand = new Label();
        lblDemo = new Label();
        btnBack = new QuizButton();
        lblTitle = new Label();
        lblSubtitle = new Label();
        lblFilter = new Label();
        cmbCategory = new QuizComboBox();
        dgvLeaderboard = new DataGridView();
        lblEmptyState = new Label();
        lblDemoData = new Label();
        lblFooter = new Label();
        colRank = new DataGridViewTextBoxColumn();
        colPlayer = new DataGridViewTextBoxColumn();
        colCategory = new DataGridViewTextBoxColumn();
        colDifficulty = new DataGridViewTextBoxColumn();
        colScore = new DataGridViewTextBoxColumn();
        DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
        DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
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
        btnBack.Location = new Point(32, 78);
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
        lblTitle.Size = new Size(770, 46);
        lblTitle.Text = "Таблиця рекордів";
        lblTitle.Font = new Font("Segoe UI", 26F, FontStyle.Bold, GraphicsUnit.Point);
        lblTitle.ForeColor = Color.FromArgb(244, 247, 252);
        lblTitle.BackColor = Color.FromArgb(16, 24, 39);

        lblSubtitle.TabIndex = 4;
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Location = new Point(32, 150);
        lblSubtitle.Size = new Size(976, 30);
        lblSubtitle.Text = "Твої найкращі спроби в одному місці";
        lblSubtitle.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
        lblSubtitle.ForeColor = Color.FromArgb(170, 182, 204);
        lblSubtitle.BackColor = Color.FromArgb(16, 24, 39);

        lblFilter.TabIndex = 5;
        lblFilter.Name = "lblFilter";
        lblFilter.Location = new Point(32, 213);
        lblFilter.Size = new Size(160, 30);
        lblFilter.Text = "Категорія";
        lblFilter.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        lblFilter.ForeColor = Color.FromArgb(170, 182, 204);
        lblFilter.BackColor = Color.FromArgb(16, 24, 39);

        cmbCategory.TabIndex = 6;
        cmbCategory.Name = "cmbCategory";
        cmbCategory.Location = new Point(190, 207);
        cmbCategory.Size = new Size(325, 39);
        cmbCategory.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        cmbCategory.BackColor = Color.FromArgb(36, 49, 73);
        cmbCategory.ForeColor = Color.FromArgb(244, 247, 252);
        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategory.FlatStyle = FlatStyle.Flat;
        cmbCategory.Items.AddRange(new object[] { "Усі категорії", "Ігри", "Програмування", "Спорт", "Фільми", "Загальні знання" });
        cmbCategory.SelectedIndex = 0;

        dgvLeaderboard.TabIndex = 7;
        dgvLeaderboard.Name = "dgvLeaderboard";
        dgvLeaderboard.Location = new Point(32, 272);
        dgvLeaderboard.Size = new Size(976, 308);
        dgvLeaderboard.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        dgvLeaderboard.ForeColor = Color.FromArgb(27, 38, 59);
        dgvLeaderboard.BackgroundColor = Color.FromArgb(27, 38, 59);
        dgvLeaderboard.BorderStyle = BorderStyle.None;
        dgvLeaderboard.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvLeaderboard.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvLeaderboard.GridColor = Color.FromArgb(51, 66, 92);
        dgvLeaderboard.ReadOnly = true;
        dgvLeaderboard.AllowUserToAddRows = false;
        dgvLeaderboard.AllowUserToDeleteRows = false;
        dgvLeaderboard.AllowUserToResizeRows = false;
        dgvLeaderboard.AllowUserToResizeColumns = false;
        dgvLeaderboard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvLeaderboard.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        dgvLeaderboard.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        dgvLeaderboard.RowHeadersVisible = false;
        dgvLeaderboard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLeaderboard.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLeaderboard.MultiSelect = false;
        dgvLeaderboard.EnableHeadersVisualStyles = false;
        dgvLeaderboard.ColumnHeadersHeight = 46;
        dgvLeaderboard.RowTemplate.Height = 49;
        headerStyle.BackColor = Color.FromArgb(36, 49, 73);
        headerStyle.ForeColor = Color.FromArgb(170, 182, 204);
        headerStyle.Font = new Font("Segoe UI", 10F);
        headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        headerStyle.WrapMode = DataGridViewTriState.False;
        headerStyle.Padding = new Padding(10, 0, 0, 0);
        headerStyle.SelectionBackColor = Color.FromArgb(36, 49, 73);
        headerStyle.SelectionForeColor = Color.FromArgb(244, 247, 252);
        dgvLeaderboard.ColumnHeadersDefaultCellStyle = headerStyle;
        cellStyle.BackColor = Color.FromArgb(27, 38, 59);
        cellStyle.ForeColor = Color.FromArgb(244, 247, 252);
        cellStyle.SelectionBackColor = Color.FromArgb(109, 94, 245);
        cellStyle.SelectionForeColor = Color.FromArgb(244, 247, 252);
        cellStyle.Padding = new Padding(10, 0, 0, 0);
        cellStyle.WrapMode = DataGridViewTriState.False;
        dgvLeaderboard.DefaultCellStyle = cellStyle;
        colRank.Name = "colRank";
        colRank.HeaderText = "Місце";
        colRank.FillWeight = 12F;
        colRank.Resizable = DataGridViewTriState.False;
        colPlayer.Name = "colPlayer";
        colPlayer.HeaderText = "Гравець";
        colPlayer.FillWeight = 25F;
        colPlayer.Resizable = DataGridViewTriState.False;
        colCategory.Name = "colCategory";
        colCategory.HeaderText = "Категорія";
        colCategory.FillWeight = 27F;
        colCategory.Resizable = DataGridViewTriState.False;
        colDifficulty.Name = "colDifficulty";
        colDifficulty.HeaderText = "Складність";
        colDifficulty.FillWeight = 22F;
        colDifficulty.Resizable = DataGridViewTriState.False;
        colScore.Name = "colScore";
        colScore.HeaderText = "Бали";
        colScore.FillWeight = 14F;
        colScore.Resizable = DataGridViewTriState.False;
        dgvLeaderboard.Columns.AddRange(new DataGridViewColumn[] { colRank, colPlayer, colCategory, colDifficulty, colScore });

        lblEmptyState.TabIndex = 8;
        lblEmptyState.Name = "lblEmptyState";
        lblEmptyState.Location = new Point(0, 46);
        lblEmptyState.Size = new Size(976, 262);
        lblEmptyState.Text = "Рекордів ще немає. Зіграй першу гру!";
        lblEmptyState.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
        lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
        lblEmptyState.ForeColor = Color.FromArgb(170, 182, 204);
        lblEmptyState.BackColor = Color.FromArgb(27, 38, 59);

        lblDemoData.TabIndex = 9;
        lblDemoData.Name = "lblDemoData";
        lblDemoData.Location = new Point(32, 604);
        lblDemoData.Size = new Size(976, 29);
        lblDemoData.Text = "";
        lblDemoData.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        lblDemoData.ForeColor = Color.FromArgb(170, 182, 204);
        lblDemoData.BackColor = Color.FromArgb(16, 24, 39);

        lblFooter.TabIndex = 10;
        lblFooter.Name = "lblFooter";
        lblFooter.Location = new Point(32, 645);
        lblFooter.Size = new Size(976, 26);
        lblFooter.Text = "Рекорди збережені на цьому комп’ютері.";
        lblFooter.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        lblFooter.ForeColor = Color.FromArgb(170, 182, 204);
        lblFooter.BackColor = Color.FromArgb(16, 24, 39);

        lblEmptyState.Visible = false;

        Controls.Add(lblBrand);
        Controls.Add(lblDemo);
        Controls.Add(btnBack);
        Controls.Add(lblTitle);
        Controls.Add(lblSubtitle);
        Controls.Add(lblFilter);
        Controls.Add(cmbCategory);
        Controls.Add(dgvLeaderboard);
        dgvLeaderboard.Controls.Add(lblEmptyState);
        Controls.Add(lblDemoData);
        Controls.Add(lblFooter);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(16, 24, 39);
        ForeColor = Color.FromArgb(244, 247, 252);
        Font = new Font("Segoe UI", 11F);
        ClientSize = new Size(1040, 700);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Name = "LeaderboardForm";
        Text = "Quiz Master";
        ResumeLayout(false);
        PerformLayout();
    }
}
