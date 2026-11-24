using System.Drawing.Imaging;
using LR2_QuizMaster.Forms;
using LR2_QuizMaster.UI;
using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.Backend.Services;

internal static class LayoutChecks
{
    private static string output = "";
    private static int assertions;
    [STAThread]
    internal static int Run(string[] args)
    {
        Application.SetHighDpiMode(args.Contains("--dpi-aware") ? HighDpiMode.PerMonitorV2 : HighDpiMode.DpiUnaware);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        Application.EnableVisualStyles();
        output = args[0]; Directory.CreateDirectory(output);
        Exception? failure = null;
        using var host = new Form { Opacity = 0, ShowInTaskbar = false };
        host.Shown += async (_, _) =>
        {
            try { await Run(); }
            catch(Exception ex) { failure=ex; Console.WriteLine(ex); }
            finally { host.Close(); }
        };
        Application.Run(host);
        Console.WriteLine(failure==null ? $"PASS {assertions} checks; leaderboard size/header/columns locked, empty and populated filters, fullscreen parent return." : "FAIL");
        return failure == null ? 0 : 1;
    }

    private static async Task Run()
    {
        var store = new LeaderboardStore(Path.Combine(output, Guid.NewGuid().ToString("N"), "records.json"));
        for(int i=0;i<12;i++)
            await store.SaveAsync(new QuizResult(Guid.NewGuid(), $"Гравець {i+1}", QuizCategory.Movies, Difficulty.Easy,
                1000-i*10, 8, 10, 10, .8, 4, GameEndReason.Completed, DateTimeOffset.UtcNow));
        using(var form = new LeaderboardForm(store) { ShowInTaskbar = false })
        {
            form.Show(); await Loaded(form); await Task.Delay(230);
            Console.WriteLine($"DPI={form.DeviceDpi}, window={form.Size}, client={form.ClientSize}");
            CheckTable(form);
            var grid=Find<DataGridView>(form,"dgvLeaderboard");
            var empty=Find<Label>(form,"lblEmptyState");
            Assert(grid.Rows.Count==12 && !empty.Visible,"actual records populated");
            var size=form.Size; form.Size = new Size(size.Width+500,size.Height+400);
            Assert(form.Size==size,"oversized resize is constrained");
            form.Size = new Size(500,400); Assert(form.Size==size,"undersized resize is constrained");
            int count=Application.OpenForms.Count;
            WindowPresentation.ToggleFullScreen(form);
            Assert(!WindowPresentation.IsFullScreen(form) && form.Size==size,"fullscreen cannot bypass lock");
            Assert(Application.OpenForms.Count==count,"same form retained");
            Capture(form,"01-records.png");
            Find<ComboBox>(form,"cmbCategory").SelectedIndex=3;
            await Loaded(form); CheckTable(form);
            Assert(grid.Rows.Count==0 && empty.Visible,"empty category displayed");
            Assert(empty.Text.Contains("категорії"),"empty message belongs to filter");
            Assert(empty.TextAlign==ContentAlignment.MiddleCenter,"empty message centered");
            Assert(empty.Parent==grid && empty.Left==0 && empty.Width==grid.ClientSize.Width,"empty panel fits table width");
            Assert(empty.Top==grid.ColumnHeadersHeight,"empty panel starts below header");
            Assert(empty.Bottom==grid.ClientSize.Height,"empty panel fills table body");
            Assert(empty.BackColor==grid.BackgroundColor,"no contrasting horizontal strip");
            Capture(form,"02-empty-category.png");
            Find<ComboBox>(form,"cmbCategory").SelectedIndex=4;
            await Loaded(form);
            Assert(grid.Rows.Count==12 && !empty.Visible,"return to nonempty category works");
            form.Close();
        }
        using(var form = new LeaderboardForm(new LeaderboardStore(Path.Combine(output,"empty-records.json"))) { ShowInTaskbar=false })
        {
            form.Show(); await Loaded(form);
            Assert(Find<Label>(form,"lblEmptyState").Visible,"entirely empty store works");
            CheckTable(form); form.Close();
        }
        using var parent=new GameSetupForm { ShowInTaskbar=false };
        parent.Show(); await Task.Delay(230);
        WindowPresentation.ToggleFullScreen(parent);
        Assert(WindowPresentation.IsFullScreen(parent),"other forms retain fullscreen");
        using var child=new LeaderboardForm(store);
        using var closer=new System.Windows.Forms.Timer { Interval=300 };
        Exception? error=null;
        closer.Tick+=(_,_)=>
        {
            closer.Stop();
            try
            {
                CheckTable(child);
                Assert(!WindowPresentation.IsFullScreen(child),"fixed modal ignores fullscreen inheritance");
                Assert(parent.Opacity==0 && Form.ActiveForm==child,"owner covered during modal; opacity="+parent.Opacity);
                Assert(WindowPresentation.IsFullScreen(parent),"opening leaderboard does not reset session mode");
                int count=Application.OpenForms.Count;
                var key=Message.Create(child.Handle,0x100,(IntPtr)(int)Keys.F11,IntPtr.Zero);
                Assert(Application.FilterMessage(ref key),"fixed form consumes F11");
                Assert(count==Application.OpenForms.Count && child.WindowState==FormWindowState.Normal,"F11 does not open another form");
            }
            catch(Exception ex) { error=ex; }
            child.Close();
        };
        child.Shown+=(_,_)=>closer.Start();
        WindowNavigation.ShowDialog(parent,child);
        if(error!=null) throw error;
        Assert(parent.Visible && parent.Opacity==1 && WindowPresentation.IsFullScreen(parent),"return restores fullscreen parent");
        WindowPresentation.ToggleFullScreen(parent); parent.Close();
    }

    private static void CheckTable(LeaderboardForm form)
    {
        var grid=Find<DataGridView>(form,"dgvLeaderboard");
        Assert(form.FormBorderStyle==FormBorderStyle.FixedSingle && !form.MaximizeBox,"window resize/maximize locked");
        Assert(form.MinimumSize==form.MaximumSize && form.Size==form.MinimumSize,"size constraints agree");
        Assert(!Find<Label>(form,"lblWindowHint").Visible,"no misleading F11 hint");
        Assert(!grid.AllowUserToResizeColumns && !grid.AllowUserToResizeRows,"table drag resizing locked");
        Assert(grid.ColumnHeadersHeightSizeMode==DataGridViewColumnHeadersHeightSizeMode.DisableResizing,"header drag locked");
        Assert(grid.Columns.Cast<DataGridViewColumn>().All(c=>c.Resizable==DataGridViewTriState.False),"each column locked");
        Assert(grid.ColumnHeadersHeight < grid.Height*.2 && grid.ColumnHeadersHeight>=24,"header has compact height");
        Assert(grid.DisplayedRowCount(false)>=4 || grid.Rows.Count==0,"rows fit below header");
        Console.WriteLine($"header={grid.ColumnHeadersHeight}, row={grid.RowTemplate.Height}, table={grid.Size}");
    }
    private static async Task Loaded(Form form)
    {
        for(int i=0;i<200;i++)
        {
            if(Find<Label>(form,"lblDemoData").Text.StartsWith("Збережених результатів:")) return;
            await Task.Delay(20);
        }
        throw new Exception("leaderboard did not load");
    }
    private static T Find<T>(Control form,string name) where T:Control => (T)form.Controls.Find(name,true).Single();
    private static void Assert(bool condition,string message) { if(!condition) throw new Exception(message); assertions++; }
    private static void Capture(Form form,string name) { using var b=new Bitmap(form.Width,form.Height); form.DrawToBitmap(b,new Rectangle(Point.Empty,b.Size)); b.Save(Path.Combine(output,name),ImageFormat.Png); }
}
