using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.Backend.Services;
using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class LeaderboardForm : Form
{
    private LeaderboardStore leaderboardStore = new();
    private readonly CancellationTokenSource lifetime = new();
    private int refreshVersion;

    public LeaderboardForm()
    {
        InitializeComponent();
        DoubleBuffered = true;
        WindowPresentation.Attach(this, allowResize: false);
        dgvLeaderboard.SizeChanged += (_, _) => LayoutEmptyState();
        dgvLeaderboard.ColumnHeadersHeightChanged += (_, _) => LayoutEmptyState();
        LayoutEmptyState();
        btnBack.Click += (_, _) => Close();
        CancelButton = btnBack;
        cmbCategory.SelectedIndexChanged += async (_, _) => { if (Visible) await RefreshRowsAsync(); };
        Shown += async (_, _) => await RefreshRowsAsync();
        FormClosed += (_, _) => { lifetime.Cancel(); lifetime.Dispose(); };
    }

    public LeaderboardForm(LeaderboardStore leaderboardStore) : this() =>
        this.leaderboardStore = leaderboardStore ?? throw new ArgumentNullException(nameof(leaderboardStore));

    private void LayoutEmptyState()
    {
        int top = dgvLeaderboard.ColumnHeadersHeight;
        lblEmptyState.SetBounds(0, top, dgvLeaderboard.ClientSize.Width,
            Math.Max(0, dgvLeaderboard.ClientSize.Height - top));
    }

    private async Task RefreshRowsAsync()
    {
        if (IsDisposed || Disposing) return;
        int version = ++refreshVersion;
        var selected = cmbCategory.SelectedItem?.ToString();
        QuizCategory? category = QuizCatalog.TryParseCategory(selected, out var parsed) ? parsed : null;
        lblDemoData.Text = "Завантаження рекордів…";
        lblEmptyState.Visible = false;
        try
        {
            var loaded = await leaderboardStore.LoadAsync(lifetime.Token);
            if (IsDisposed || Disposing || version != refreshVersion) return;
            var records = loaded.Records.Where(r => category == null || r.Category == category)
                .OrderByDescending(r => r.Score).ThenByDescending(r => r.Accuracy).ThenByDescending(r => r.FinishedAtUtc).ToArray();
            dgvLeaderboard.Rows.Clear();
            for (int i = 0; i < records.Length; i++)
            {
                var record = records[i];
                dgvLeaderboard.Rows.Add(i + 1, record.PlayerName, QuizCatalog.CategoryName(record.Category),
                    QuizCatalog.DifficultyName(record.Difficulty), record.Score);
            }
            dgvLeaderboard.ClearSelection();
            lblEmptyState.Text = category == null ? "Рекордів ще немає. Зіграй першу гру!" : "У цій категорії ще немає рекордів.";
            lblEmptyState.Visible = records.Length == 0;
            if (lblEmptyState.Visible) lblEmptyState.BringToFront();
            lblDemoData.Text = loaded.Warning == null ? $"Збережених результатів: {records.Length}" :
                "Файл рекордів пошкоджений. Після нової гри буде збережено резервну копію.";
            lblDemoData.ForeColor = loaded.Warning == null ? Color.FromArgb(170, 182, 204) : Color.FromArgb(239, 153, 166);
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            if (IsDisposed || Disposing || version != refreshVersion) return;
            dgvLeaderboard.Rows.Clear();
            lblDemoData.Text = "Не вдалося завантажити рекорди. Перевір доступ до папки даних.";
            lblDemoData.ForeColor = Color.FromArgb(239, 153, 166);
            lblEmptyState.Text = "Рекорди тимчасово недоступні.";
            lblEmptyState.Visible = true;
            lblEmptyState.BringToFront();
        }
    }
}
