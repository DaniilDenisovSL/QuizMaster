using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LR2_QuizMaster.UI;

internal enum GameResultVisual { Completed, Lost }

/// <summary>Presentation only: window placement, transitions, icons and responsive layout.</summary>
public static class WindowPresentation
{
    private static readonly ConditionalWeakTable<Form, Presenter> presenters = new();

    public static void Attach(Form form, bool allowResize = true)
    {
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || form.Site?.DesignMode == true) return;
        if (!presenters.TryGetValue(form, out _)) presenters.Add(form, new Presenter(form, allowResize));
    }

    public static void ToggleFullScreen(Form form)
    {
        if (presenters.TryGetValue(form, out var presenter)) presenter.ToggleFullScreen();
    }

    public static bool IsFullScreen(Form form) => presenters.TryGetValue(form, out var presenter) && presenter.FullScreen;

    internal static void PrepareReturn(Form form)
    {
        if (presenters.TryGetValue(form, out var presenter)) presenter.PrepareReturn();
    }

    private enum DisplayMode { Normal, Maximized, FullScreen }
    private static DisplayMode sessionMode;
    private static FormWindowState windowedState = FormWindowState.Normal;
    private static string? monitorName;

    private sealed class Presenter : IMessageFilter
    {
        private readonly Form form;
        private readonly bool allowResize;
        private readonly System.Windows.Forms.Timer fadeTimer = new() { Interval = 15 };
        private readonly List<LayoutItem> items = new();
        private readonly Dictionary<string, Font> fonts = new();
        private Panel? canvas;
        private Label? hint;
        private SizeF logicalCanvas;
        private Rectangle windowedBounds;
        private bool initialized;
        private bool hasShown;
        private bool applying;
        private bool animationAllowed;
        private bool disposed;
        private long fadeStarted;
        private float fontScale = -1;
        public bool FullScreen { get; private set; }

        public Presenter(Form form, bool allowResize)
        {
            this.form = form;
            this.allowResize = allowResize;
            form.KeyPreview = true;
            form.Load += OnLoad;
            form.Shown += OnShown;
            form.Activated += OnActivated;
            form.VisibleChanged += OnVisibleChanged;
            form.Resize += OnResize;
            form.LocationChanged += OnLocationChanged;
            form.DpiChanged += OnDpiChanged;
            form.Disposed += OnDisposed;
            fadeTimer.Tick += OnFadeTick;
            Application.AddMessageFilter(this);
        }

        private void OnLoad(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || form.Site?.DesignMode == true) return;
            animationAllowed = form.Opacity >= .99;
            if (animationAllowed) form.Opacity = 0;
            form.Icon = QuizIcons.WindowIcon;
            form.FormBorderStyle = allowResize ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;
            form.MaximizeBox = allowResize;
            form.SuspendLayout();
            AddIcons();
            hint = new Label
            {
                Name = "lblWindowHint", Text = "F11 · повний екран", Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(170, 182, 204), BackColor = form.BackColor,
                Location = ScalePoint(340, 24), Size = ScaleSize(320, 25), TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand, AccessibleName = "Повний екран — F11"
            };
            hint.Visible = allowResize;
            hint.Click += (_, _) => ToggleFullScreen();
            form.Controls.Add(hint);
            float dpi = Dpi;
            logicalCanvas = new SizeF(1040, 700);
            canvas = new BufferedCanvas
            {
                Name = "pnlResponsiveCanvas", BackColor = form.BackColor,
                Size = new Size((int)(1040 * dpi), (int)(700 * dpi)), TabStop = false
            };
            var controls = form.Controls.Cast<Control>().ToArray();
            foreach (var control in controls) canvas.Controls.Add(control);
            form.Controls.Add(canvas);
            foreach (Control control in canvas.Controls) Capture(control, dpi);
            initialized = true;
            ApplyMode();
            form.ResumeLayout(false);
            LayoutContent();
        }

        private float Dpi => form.DeviceDpi / 96f;
        private Point ScalePoint(int x, int y) => new((int)Math.Round(x * Dpi), (int)Math.Round(y * Dpi));
        private Size ScaleSize(int w, int h) => new((int)Math.Round(w * Dpi), (int)Math.Round(h * Dpi));

        private Screen TargetScreen()
        {
            if (monitorName != null)
            {
                var match = Screen.AllScreens.FirstOrDefault(s => s.DeviceName == monitorName);
                if (match != null) return match;
            }
            if (form.Owner is { IsDisposed: false } owner) return Screen.FromControl(owner);
            return Screen.FromPoint(Cursor.Position);
        }

        private void UpdateMinimum(Screen screen)
        {
            var frame = form.Size - form.ClientSize;
            form.MinimumSize = new Size(
                Math.Min((int)(920 * Dpi) + frame.Width, screen.WorkingArea.Width),
                Math.Min((int)(620 * Dpi) + frame.Height, screen.WorkingArea.Height));
        }

        private Rectangle CenteredBounds(Screen screen, Size desired)
        {
            var work = screen.WorkingArea;
            int margin = Math.Min((int)(16 * Dpi), Math.Max(0, (work.Height - form.MinimumSize.Height) / 2));
            int width = Math.Min(Math.Max(desired.Width, form.MinimumSize.Width), work.Width - margin * 2);
            int height = Math.Min(Math.Max(desired.Height, form.MinimumSize.Height), work.Height - margin * 2);
            return new Rectangle(work.X + (work.Width - width) / 2, work.Y + (work.Height - height) / 2, width, height);
        }

        private void ApplyMode()
        {
            if (applying || disposed) return;
            var screen = TargetScreen();
            monitorName = screen.DeviceName;
            applying = true;
            try
            {
                FullScreen = allowResize && sessionMode == DisplayMode.FullScreen;
                form.WindowState = FormWindowState.Normal;
                form.FormBorderStyle = !allowResize ? FormBorderStyle.FixedSingle : FullScreen ? FormBorderStyle.None : FormBorderStyle.Sizable;
                if (!allowResize)
                {
                    form.MinimumSize = Size.Empty;
                    form.MaximumSize = Size.Empty;
                }
                UpdateMinimum(screen);
                var frame = form.Size - form.ClientSize;
                var normal = CenteredBounds(screen, ScaleSize(1040, 700) + frame);
                form.Bounds = normal;
                if (!allowResize)
                {
                    form.MinimumSize = normal.Size;
                    form.MaximumSize = normal.Size;
                }
                else if (sessionMode == DisplayMode.FullScreen)
                {
                    windowedBounds = normal;
                    EnterFullScreen(screen);
                }
                else if (sessionMode == DisplayMode.Maximized) form.WindowState = FormWindowState.Maximized;
                UpdateHint();
            }
            finally { applying = false; }
        }

        public void ToggleFullScreen()
        {
            if (!initialized || disposed || applying || !allowResize) return;
            applying = true;
            try
            {
                if (!FullScreen)
                {
                    windowedState = form.WindowState == FormWindowState.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
                    windowedBounds = form.WindowState == FormWindowState.Normal ? form.Bounds : form.RestoreBounds;
                    sessionMode = DisplayMode.FullScreen;
                    EnterFullScreen(Screen.FromControl(form));
                }
                else
                {
                    var screen = Screen.FromControl(form);
                    FullScreen = false;
                    sessionMode = windowedState == FormWindowState.Maximized ? DisplayMode.Maximized : DisplayMode.Normal;
                    form.WindowState = FormWindowState.Normal;
                    form.FormBorderStyle = FormBorderStyle.Sizable;
                    UpdateMinimum(screen);
                    form.Bounds = CenteredBounds(screen, windowedBounds.Size);
                    form.WindowState = windowedState;
                }
                UpdateHint();
                LayoutContent();
            }
            finally { applying = false; }
        }

        private void EnterFullScreen(Screen screen)
        {
            FullScreen = true;
            form.WindowState = FormWindowState.Normal;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Bounds = screen.Bounds;
            monitorName = screen.DeviceName;
        }

        private void UpdateHint()
        {
            if (hint != null) hint.Text = FullScreen ? "Esc · повернутись до вікна" : "F11 · повний екран";
        }

        private void OnShown(object? sender, EventArgs e) { hasShown = true; FadeIn(); }
        private void OnActivated(object? sender, EventArgs e)
        {
            if (!allowResize) return;
            if (!initialized || disposed || applying || !form.Visible || !form.Enabled || form.OwnedForms.Any(f => f.Visible)) return;
            var current = FullScreen ? DisplayMode.FullScreen : form.WindowState == FormWindowState.Maximized ? DisplayMode.Maximized : DisplayMode.Normal;
            if (current != sessionMode) { ApplyMode(); LayoutContent(); }
        }
        private void OnVisibleChanged(object? sender, EventArgs e)
        {
            if (applying) return;
            if (!form.Visible) { fadeTimer.Stop(); return; }
            if (!initialized || !hasShown || applying) return;
            ApplyMode();
            LayoutContent();
            FadeIn();
        }
        private void FadeIn()
        {
            if (!animationAllowed || disposed || !form.Visible) return;
            form.Opacity = 0;
            fadeStarted = Stopwatch.GetTimestamp();
            fadeTimer.Start();
        }
        private void OnFadeTick(object? sender, EventArgs e)
        {
            if (disposed || !form.Visible) { fadeTimer.Stop(); return; }
            double t = Math.Clamp(Stopwatch.GetElapsedTime(fadeStarted).TotalMilliseconds / 180, 0, 1);
            form.Opacity = 1 - Math.Pow(1 - t, 3);
            if (t >= 1) { form.Opacity = 1; fadeTimer.Stop(); }
        }
        private void OnResize(object? sender, EventArgs e)
        {
            if (!initialized || disposed || form.WindowState == FormWindowState.Minimized) return;
            if (allowResize && !applying && form.Visible && form.Enabled && Form.ActiveForm == form && !FullScreen)
            {
                sessionMode = form.WindowState == FormWindowState.Maximized ? DisplayMode.Maximized : DisplayMode.Normal;
                windowedState = form.WindowState;
            }
            LayoutContent();
        }
        private void OnLocationChanged(object? sender, EventArgs e)
        {
            if (initialized && form.Visible && !applying && form.WindowState != FormWindowState.Minimized)
                monitorName = Screen.FromControl(form).DeviceName;
        }
        private void OnDpiChanged(object? sender, DpiChangedEventArgs e)
        {
            if (!initialized || disposed || !form.IsHandleCreated) return;
            form.BeginInvoke((Action)(() =>
            {
                if (disposed) return;
                if (allowResize) UpdateMinimum(Screen.FromControl(form)); else ApplyMode();
                LayoutContent();
            }));
        }

        private void Capture(Control control, float dpi)
        {
            var bounds = control.Bounds;
            var item = new LayoutItem(control, new RectangleF(bounds.X / dpi, bounds.Y / dpi, bounds.Width / dpi, bounds.Height / dpi),
                control.Font.FontFamily.Name, control.Font.SizeInPoints, control.Font.Style);
            if (control is DataGridView grid)
            {
                // These explicit DataGridView metrics remain designer pixels after DPI autoscaling.
                // Unlike control bounds, they must be scaled from 96 DPI exactly once here.
                item.HeaderHeight = grid.ColumnHeadersHeight;
                item.RowHeight = grid.RowTemplate.Height;
                item.HeaderFontSize = grid.ColumnHeadersDefaultCellStyle.Font?.SizeInPoints ?? 10;
            }
            items.Add(item);
            if (control is DataGridView or ComboBox) return;
            foreach (Control child in control.Controls) Capture(child, dpi);
        }

        private Font GetFont(string family, float size, FontStyle style, float scale)
        {
            float points = Math.Max(7f, (float)Math.Round(size * scale, 2));
            string key = $"{family}|{points:F2}|{(int)style}";
            if (!fonts.TryGetValue(key, out var font))
            {
                font = new Font(family, points, style, GraphicsUnit.Point);
                fonts.Add(key, font);
            }
            return font;
        }

        private void LayoutContent()
        {
            if (canvas == null || !initialized || form.ClientSize.Width <= 0 || form.ClientSize.Height <= 0) return;
            float dpi = Dpi;
            float scale = Math.Min(1.6f, Math.Min(form.ClientSize.Width / (logicalCanvas.Width * dpi), form.ClientSize.Height / (logicalCanvas.Height * dpi)));
            if (scale <= 0) return;
            bool changeFonts = Math.Abs(scale - fontScale) > .005f;
            Font[] retired = changeFonts ? fonts.Values.ToArray() : Array.Empty<Font>();
            if (changeFonts) fonts.Clear();
            canvas.SuspendLayout();
            foreach (var item in items)
            {
                var r = item.Bounds;
                if (changeFonts) item.Control.Font = GetFont(item.Family, item.FontSize, item.Style, scale);
                item.Control.Bounds = new Rectangle((int)Math.Round(r.X * dpi * scale), (int)Math.Round(r.Y * dpi * scale),
                    Math.Max(1, (int)Math.Round(r.Width * dpi * scale)), Math.Max(1, (int)Math.Round(r.Height * dpi * scale)));
                if (item.Control is DataGridView grid)
                {
                    grid.ColumnHeadersHeight = Math.Max(24, (int)(item.HeaderHeight * dpi * scale));
                    grid.RowTemplate.Height = Math.Max(24, (int)(item.RowHeight * dpi * scale));
                    foreach (DataGridViewRow row in grid.Rows) row.Height = grid.RowTemplate.Height;
                    if (changeFonts) grid.ColumnHeadersDefaultCellStyle.Font = GetFont(item.Family, item.HeaderFontSize, FontStyle.Regular, scale);
                }
            }
            canvas.Size = new Size((int)Math.Round(logicalCanvas.Width * dpi * scale), (int)Math.Round(logicalCanvas.Height * dpi * scale));
            canvas.Location = new Point((form.ClientSize.Width - canvas.Width) / 2, (form.ClientSize.Height - canvas.Height) / 2);
            canvas.ResumeLayout(false);
            if (changeFonts) fontScale = scale;
            foreach (var font in retired) font.Dispose();
        }

        private void AddIcons()
        {
            Control? Find(string name) => form.Controls.Find(name, true).FirstOrDefault();
            var logo = new QuizIconView { Name = "icoAppLogo", IconKind = QuizIcon.Logo, ForeColor = Color.White, Location = ScalePoint(32, 23), Size = ScaleSize(24, 24) };
            form.Controls.Add(logo);
            if (Find("lblBrand") is Control brand) brand.Left = (int)(68 * Dpi);
            var categories = new Dictionary<string, QuizIcon>
            {
                ["btnGames"] = QuizIcon.Games, ["btnProgramming"] = QuizIcon.Code, ["btnSport"] = QuizIcon.Sport,
                ["btnMovies"] = QuizIcon.Movies, ["btnGeneral"] = QuizIcon.Knowledge, ["btnLeaderboard"] = QuizIcon.Trophy
            };
            foreach (var entry in categories) if (Find(entry.Key) is QuizButton button) button.IconKind = entry.Value;
            AddInline(Find("lblTimer"), QuizIcon.Clock, Color.FromArgb(39, 198, 152));
            AddInline(Find("lblScore"), QuizIcon.Star, Color.FromArgb(180, 169, 255));
            AddInline(Find("lblLives"), QuizIcon.Heart, Color.FromArgb(39, 198, 152));
            AddInline(Find("lblStreak"), QuizIcon.Flame, Color.FromArgb(180, 169, 255));
            if (Find("lblHeading") is Control heading)
            {
                form.Controls.Add(new QuizIconView { Name = "icoCompleted", IconKind = heading.Tag is GameResultVisual.Lost ? QuizIcon.Defeat : QuizIcon.Completed, ForeColor = heading.Tag is GameResultVisual.Lost ? Color.FromArgb(239, 153, 166) : Color.FromArgb(39, 198, 152), Location = heading.Location, Size = ScaleSize(46, 46) });
                heading.Left += (int)(64 * Dpi);
                heading.Width -= (int)(64 * Dpi);
            }
        }

        private void AddInline(Control? label, QuizIcon icon, Color color)
        {
            if (label?.Parent == null) return;
            int side = (int)(19 * Dpi), gap = (int)(28 * Dpi);
            label.Parent.Controls.Add(new QuizIconView
            {
                Name = "ico" + label.Name, IconKind = icon, ForeColor = color,
                Location = new Point(label.Left, label.Top + (label.Height - side) / 2), Size = new Size(side, side)
            });
            label.Left += gap; label.Width -= gap;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (disposed || !initialized || !form.Visible || m.Msg is not (0x100 or 0x104)) return false;
            var target = Control.FromChildHandle(m.HWnd);
            // A disabled owner must never handle the active modal window's shortcut.
            if (!form.Enabled || target?.FindForm() != form || Form.ActiveForm != form) return false;
            var key = (Keys)(int)m.WParam;
            if (key != Keys.F11 && !(key == Keys.Escape && FullScreen)) return false;
            if ((m.LParam.ToInt64() & (1L << 30)) != 0) return true;
            ToggleFullScreen();
            return true;
        }

        public void PrepareReturn()
        {
            if (!initialized || disposed) return;
            ApplyMode();
            LayoutContent();
        }

        private void OnDisposed(object? sender, EventArgs e)
        {
            disposed = true;
            fadeTimer.Stop(); fadeTimer.Dispose();
            Application.RemoveMessageFilter(this);
            foreach (var font in fonts.Values) font.Dispose();
            fonts.Clear();
        }

        private sealed class BufferedCanvas : Panel
        {
            public BufferedCanvas() { DoubleBuffered = true; ResizeRedraw = true; }
        }

        private sealed class LayoutItem(Control control, RectangleF bounds, string family, float fontSize, FontStyle style)
        {
            public Control Control { get; } = control;
            public RectangleF Bounds { get; } = bounds;
            public string Family { get; } = family;
            public float FontSize { get; } = fontSize;
            public FontStyle Style { get; } = style;
            public float HeaderHeight { get; set; }
            public float RowHeight { get; set; }
            public float HeaderFontSize { get; set; }
        }
    }
}
