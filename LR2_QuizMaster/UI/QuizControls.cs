using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace LR2_QuizMaster.UI;

internal static class QuizDrawing
{
    internal static GraphicsPath Round(RectangleF r, float radius)
    {
        var p = new GraphicsPath();
        if (r.Width <= 0 || r.Height <= 0) return p;
        float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    internal static Color Lighten(Color c, int amount) => Color.FromArgb(
        Math.Clamp(c.R + amount, 0, 255), Math.Clamp(c.G + amount, 0, 255), Math.Clamp(c.B + amount, 0, 255));

    internal static void PaintParentSurface(Graphics graphics, Control control)
    {
        Color background = Color.FromArgb(16, 24, 39);
        for (Control? parent = control.Parent; parent != null; parent = parent.Parent)
        {
            if (parent.BackColor.A != 255) continue;
            background = parent.BackColor;
            break;
        }
        using var brush = new SolidBrush(background);
        graphics.FillRectangle(brush, control.ClientRectangle);
    }
}

public class QuizButton : Button
{
    private bool hover;
    private bool pressed;
    private float hoverAmount;
    private QuizIcon iconKind;
    private readonly System.Windows.Forms.Timer hoverTimer = new() { Interval = 15 };

    [DefaultValue(QuizIcon.None)]
    public QuizIcon IconKind { get => iconKind; set { iconKind = value; Invalidate(); } }

    public QuizButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        UseVisualStyleBackColor = false;
        Cursor = Cursors.Hand;
        hoverTimer.Tick += (_, _) =>
        {
            float target = hover && Enabled ? 1f : 0f;
            hoverAmount += Math.Clamp(target - hoverAmount, -.15f, .15f);
            if (Math.Abs(target - hoverAmount) < .01f) { hoverAmount = target; hoverTimer.Stop(); }
            Invalidate();
        };
    }

    protected override void OnMouseEnter(EventArgs e) { hover = true; hoverTimer.Start(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { hover = pressed = false; hoverTimer.Start(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { pressed = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { pressed = false; Invalidate(); base.OnMouseUp(e); }
    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        QuizDrawing.PaintParentSurface(e.Graphics, this);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        // Flat Button paints as Opaque, so Windows can skip OnPaintBackground.
        // Cover the whole buffer on every frame, including outside the rounded path.
        QuizDrawing.PaintParentSurface(g, this);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float dpi = DeviceDpi / 96f;
        if (Width <= 3 * dpi || Height <= 3 * dpi) return;
        float pad = dpi;
        Color fill = !Enabled ? Color.FromArgb(35, 45, 64) : QuizDrawing.Lighten(BackColor, pressed ? -7 : (int)(hoverAmount * 18));
        bool accent = BackColor.B > 150 && BackColor.R > 65;
        using var body = QuizDrawing.Round(new RectangleF(pad, pad, Width - pad * 3, Height - pad * 3), 12 * dpi);
        using var brush = new SolidBrush(fill);
        g.FillPath(brush, body);
        using var border = new Pen(accent ? Color.FromArgb(141, 127, 255) : Color.FromArgb(48, 62, 86), dpi);
        g.DrawPath(border, body);
        var textBounds = new Rectangle((int)(14 * dpi), 0, Width - (int)(28 * dpi), Height - (int)(2 * dpi));
        if (IconKind != QuizIcon.None)
        {
            float iconSize = 24 * dpi;
            QuizIcons.Draw(g, IconKind, new RectangleF(20 * dpi, (Height - iconSize) / 2, iconSize, iconSize),
                Enabled ? ForeColor : Color.FromArgb(147, 160, 182));
            textBounds.X = (int)(60 * dpi);
            textBounds.Width = Width - (int)(76 * dpi);
        }
        var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;
        if (!UseMnemonic) flags |= TextFormatFlags.NoPrefix;
        TextRenderer.DrawText(g, Text, Font, textBounds, Enabled ? ForeColor : Color.FromArgb(147, 160, 182), flags);
        if (Focused && ShowFocusCues)
        {
            using var pen = new Pen(Color.FromArgb(180, 169, 255), dpi) { DashStyle = DashStyle.Dot };
            using var focus = QuizDrawing.Round(new RectangleF(5 * dpi, 5 * dpi, Width - 11 * dpi, Height - 11 * dpi), 8 * dpi);
            g.DrawPath(pen, focus);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) { hoverTimer.Stop(); hoverTimer.Dispose(); }
        base.Dispose(disposing);
    }
}

public class QuizPanel : Panel
{
    private int cornerRadius = 18;
    [DefaultValue(18)]
    public int CornerRadius { get => cornerRadius; set { cornerRadius = Math.Max(1, value); Invalidate(); } }

    public QuizPanel()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        QuizDrawing.PaintParentSurface(e.Graphics, this);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        QuizDrawing.PaintParentSurface(g, this);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float dpi = DeviceDpi / 96f;
        if (Width <= 3 * dpi || Height <= 3 * dpi) return;
        using var path = QuizDrawing.Round(new RectangleF(dpi, dpi, Width - dpi * 3, Height - dpi * 3), CornerRadius * dpi);
        using var fill = new SolidBrush(BackColor);
        g.FillPath(fill, path);
        using var line = new Pen(Color.FromArgb(45, 58, 81), dpi);
        g.DrawPath(line, path);
        base.OnPaint(e);
    }
}

public class QuizProgressBar : Control
{
    private int maximum = 20;
    private int value = 18;
    [DefaultValue(20)]
    public int Maximum { get => maximum; set { maximum = Math.Max(1, value); this.value = Math.Min(this.value, maximum); Invalidate(); } }
    [DefaultValue(18)]
    public int Value { get => value; set { this.value = Math.Clamp(value, 0, maximum); Invalidate(); } }

    public QuizProgressBar()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
        TabStop = false;
        AccessibleRole = AccessibleRole.ProgressBar;
        AccessibleName = "Залишок часу";
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        QuizDrawing.PaintParentSurface(e.Graphics, this);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        QuizDrawing.PaintParentSurface(g, this);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var track = QuizDrawing.Round(new RectangleF(0, 0, Width, Height), Height / 2f);
        using var muted = new SolidBrush(Color.FromArgb(39, 52, 73));
        g.FillPath(muted, track);
        if (Value == 0) return;
        using var fill = new SolidBrush(Color.FromArgb(39, 198, 152));
        using var progress = QuizDrawing.Round(new RectangleF(0, 0, Width * Value / (float)Maximum, Height), Height / 2f);
        g.FillPath(fill, progress);
    }
}

public class QuizComboBox : ComboBox
{
    public QuizComboBox() { DrawMode = DrawMode.OwnerDrawFixed; }
    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        bool selected = (e.State & DrawItemState.Selected) != 0;
        using var fill = new SolidBrush(selected ? Color.FromArgb(63, 57, 110) : BackColor);
        e.Graphics.FillRectangle(fill, e.Bounds);
        string text = e.Index >= 0 ? GetItemText(Items[e.Index]) ?? string.Empty : Text;
        var r = e.Bounds; r.X += 8; r.Width -= 8;
        TextRenderer.DrawText(e.Graphics, text, Font, r, ForeColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        base.OnDrawItem(e);
    }
}
