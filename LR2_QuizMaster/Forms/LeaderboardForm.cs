using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class LeaderboardForm : Form
{
    private readonly object[][] demoRows =
    {
        new object[] { 1, "Олена", "Програмування", "Середня", 980 },
        new object[] { 2, "Максим", "Ігри", "Складна", 940 },
        new object[] { 3, "Софія", "Загальні знання", "Середня", 900 },
        new object[] { 4, "Андрій", "Спорт", "Легка", 870 },
        new object[] { 5, "Гравець", "Фільми", "Середня", 820 }
    };

    public LeaderboardForm()
    {
        InitializeComponent();
        WindowPresentation.Attach(this);
        btnBack.Click += (_, _) => Close();
        CancelButton = btnBack;
        cmbCategory.SelectedIndexChanged += (_, _) => RefreshRows();
        RefreshRows();
    }

    private void RefreshRows()
    {
        dgvLeaderboard.Rows.Clear();
        var selected = cmbCategory.SelectedItem?.ToString();
        var rank = 1;
        foreach (var row in demoRows)
        {
            if (selected != "Усі категорії" && row[2].ToString() != selected) continue;
            dgvLeaderboard.Rows.Add(rank++, row[1], row[2], row[3], row[4]);
        }
        lblEmptyState.Visible = dgvLeaderboard.Rows.Count == 0;
        dgvLeaderboard.ClearSelection();
        // TODO Backend: отримувати записи із сервісу рекордів замість demoRows.
    }
}
