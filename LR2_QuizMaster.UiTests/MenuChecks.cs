using System.Drawing.Imaging;
using LR2_QuizMaster.Forms;
using LR2_QuizMaster.UI;
using LR2_QuizMaster.Backend.Services;

internal static class MenuChecks
{
    private static readonly List<string> failures = [];
    private static int checks;
    private static string output = "";

    [STAThread]
    internal static int Run(string[] args)
    {
        Application.SetHighDpiMode(args.Contains("--dpi-aware") ? HighDpiMode.PerMonitorV2 : HighDpiMode.DpiUnaware);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        Application.EnableVisualStyles();
        output = args[0]; Directory.CreateDirectory(output);
        using var host = new Form { Opacity = 0, ShowInTaskbar = false };
        host.Shown += async (_, _) =>
        {
            try { await Exercise(); }
            catch (Exception ex) { failures.Add(ex.ToString()); }
            finally { host.Close(); }
        };
        Application.Run(host);
        foreach (string failure in failures) Console.WriteLine("FAIL " + failure);
        Console.WriteLine($"Menu checks: {checks}, failures: {failures.Count}.");
        return failures.Count == 0 ? 0 : 1;
    }

    private static async Task Exercise()
    {
        var store = new LeaderboardStore(Path.Combine(output, "isolated-records.json"));
        using var menu = new MainMenuForm(QuestionBank.Load(QuestionBank.DefaultFilePath), store) { ShowInTaskbar = false };
        menu.Show(); await Task.Delay(250);
        Console.WriteLine($"DPI={menu.DeviceDpi}; client={menu.ClientSize}");
        Validate(menu, "normal");
        Find<TextBox>(menu,"txtPlayerName").Focus();
        Check(Find<TextBox>(menu,"txtPlayerName").Focused,"player name accepts focus");
        int count = Application.OpenForms.Count;
        menu.Size = menu.MinimumSize; await Task.Delay(60); Validate(menu,"minimum");
        menu.WindowState = FormWindowState.Maximized; await Task.Delay(60); Validate(menu,"maximized");
        WindowPresentation.ToggleFullScreen(menu); await Task.Delay(60); Validate(menu,"fullscreen");
        Check(WindowPresentation.IsFullScreen(menu),"fullscreen entered");
        // Keyboard routing is covered by GameChecks; geometry checks do not require foreground focus.
        WindowPresentation.ToggleFullScreen(menu);
        await Task.Delay(60); Validate(menu,"restored");
        Check(menu.WindowState == FormWindowState.Maximized && !WindowPresentation.IsFullScreen(menu),"maximized state restored");
        Check(Application.OpenForms.Count == count,"no duplicate menu");
        menu.WindowState = FormWindowState.Normal;
        bool visited = false;
        using var closer = new System.Windows.Forms.Timer { Interval = 300 };
        closer.Tick += (_,_) =>
        {
            var records = Application.OpenForms.OfType<LeaderboardForm>().FirstOrDefault(f=>f.Visible);
            if (records == null) return;
            visited = true; closer.Stop(); Find<Button>(records,"btnBack").PerformClick();
        };
        closer.Start(); Find<Button>(menu,"btnLeaderboard").PerformClick(); closer.Stop();
        Check(visited && menu.Visible && menu.Opacity == 1,"records button and return work");
        Find<Button>(menu,"btnExit").PerformClick();
        Check(menu.IsDisposed || !menu.Visible,"exit button closes menu");
    }

    private static void Validate(MainMenuForm menu,string mode)
    {
        var canvas=Find<Panel>(menu,"pnlResponsiveCanvas");
        Check(menu.ClientRectangle.Contains(canvas.Bounds),mode+": canvas inside window");
        var title=Find<Label>(menu,"lblTitle"); var panel=Find<Panel>(menu,"pnlStart");
        Check(title.Right < panel.Left,mode+": title does not overlap start panel");
        foreach(Control control in Descendants(canvas).Where(c=>c.Visible))
        {
            Check(control.Parent!.ClientRectangle.Contains(control.Bounds),mode+": "+control.Name+" inside parent");
            if(control is Label label && !string.IsNullOrWhiteSpace(label.Text))
            {
                var required=TextRenderer.MeasureText(label.Text,label.Font,new Size(label.Width,int.MaxValue),TextFormatFlags.WordBreak|TextFormatFlags.NoPadding);
                Check(required.Height<=label.Height,mode+": "+label.Name+" text fits (needs "+required.Height+", has "+label.Height+")");
            }
        }
        Capture(menu,mode+".png");
    }
    private static IEnumerable<Control> Descendants(Control parent) => parent.Controls.Cast<Control>().SelectMany(c=>new[]{c}.Concat(Descendants(c)));
    private static T Find<T>(Control parent,string name) where T:Control => (T)parent.Controls.Find(name,true).Single();
    private static void Check(bool condition,string name) { checks++; if(!condition) failures.Add(name); }
    private static void Capture(Form form,string name) { using var bitmap=new Bitmap(form.Width,form.Height); form.DrawToBitmap(bitmap,new Rectangle(Point.Empty,bitmap.Size)); bitmap.Save(Path.Combine(output,name),ImageFormat.Png); }
}
