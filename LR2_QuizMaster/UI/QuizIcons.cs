using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace LR2_QuizMaster.UI;

public enum QuizIcon { None, Logo, Games, Code, Sport, Movies, Knowledge, Trophy, Clock, Star, Heart, Flame, Completed, Defeat }

public static class QuizIcons
{
    private static readonly Lazy<Icon> windowIcon = new(() =>
    {
        using var data = new MemoryStream(CreateIconFile());
        using var icon = new Icon(data);
        return (Icon)icon.Clone();
    });

    public static Icon WindowIcon => windowIcon.Value;

    public static byte[] CreateIconFile()
    {
        using var bitmap = new Bitmap(32, 32);
        using (var g = Graphics.FromImage(bitmap)) Draw(g, QuizIcon.Logo, new RectangleF(0, 0, 32, 32), Color.White);
        using var png = new MemoryStream();
        bitmap.Save(png, ImageFormat.Png);
        byte[] bytes = png.ToArray();
        using var result = new MemoryStream();
        using var writer = new BinaryWriter(result);
        writer.Write((ushort)0); writer.Write((ushort)1); writer.Write((ushort)1);
        writer.Write((byte)32); writer.Write((byte)32); writer.Write((byte)0); writer.Write((byte)0);
        writer.Write((ushort)1); writer.Write((ushort)32);
        writer.Write(bytes.Length); writer.Write(22); writer.Write(bytes);
        return result.ToArray();
    }

    public static void Draw(Graphics g, QuizIcon icon, RectangleF bounds, Color color)
    {
        if (icon == QuizIcon.None || bounds.Width <= 0 || bounds.Height <= 0) return;
        var saved = g.Save();
        g.TranslateTransform(bounds.X, bounds.Y);
        g.ScaleTransform(bounds.Width / 24f, bounds.Height / 24f);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var pen = new Pen(color, 1.65f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        using var fill = new SolidBrush(color);
        switch (icon)
        {
            case QuizIcon.Logo:
                using (var shape = QuizDrawing.Round(new RectangleF(0.5f, 0.5f, 23, 23), 5))
                using (var purple = new SolidBrush(Color.FromArgb(109, 94, 245))) g.FillPath(purple, shape);
                using (var white = new Pen(Color.White, 2.5f))
                { g.DrawEllipse(white, 6, 5, 11, 12); g.DrawLine(white, 13, 13, 18, 19); }
                break;
            case QuizIcon.Games:
                using (var shape = new GraphicsPath())
                {
                    shape.AddBezier(6, 7, 1, 7, 0, 20, 5, 19);
                    shape.AddLine(5, 19, 9, 15); shape.AddLine(9, 15, 15, 15); shape.AddLine(15, 15, 19, 19);
                    shape.AddBezier(19, 19, 24, 20, 23, 7, 18, 7); shape.CloseFigure(); g.DrawPath(pen, shape);
                }
                g.DrawLine(pen, 5, 11, 10, 11); g.DrawLine(pen, 7.5f, 8.5f, 7.5f, 13.5f);
                g.FillEllipse(fill, 16, 9, 2, 2); g.FillEllipse(fill, 18, 12, 2, 2);
                break;
            case QuizIcon.Code:
                g.DrawLines(pen, new[] { new PointF(7, 6), new PointF(2, 12), new PointF(7, 18) });
                g.DrawLines(pen, new[] { new PointF(17, 6), new PointF(22, 12), new PointF(17, 18) });
                g.DrawLine(pen, 14, 4, 10, 20);
                break;
            case QuizIcon.Sport:
                g.DrawEllipse(pen, 3, 3, 18, 18);
                g.DrawPolygon(pen, new[] { new PointF(12, 8), new PointF(16, 11), new PointF(14.5f, 16), new PointF(9.5f, 16), new PointF(8, 11) });
                g.DrawLine(pen, 12, 8, 12, 3); g.DrawLine(pen, 16, 11, 21, 9);
                g.DrawLine(pen, 14.5f, 16, 17, 20); g.DrawLine(pen, 9.5f, 16, 7, 20); g.DrawLine(pen, 8, 11, 3, 9);
                break;
            case QuizIcon.Movies:
                g.DrawRectangle(pen, 3, 9, 18, 12);
                g.DrawPolygon(pen, new[] { new PointF(3, 5), new PointF(20, 1.5f), new PointF(21, 6), new PointF(4, 9.5f) });
                g.DrawLine(pen, 8, 4, 11, 7.5f); g.DrawLine(pen, 14, 2.7f, 17, 6.5f);
                g.DrawLine(pen, 4, 14, 20, 14);
                break;
            case QuizIcon.Knowledge:
                g.DrawArc(pen, 5, 2, 14, 14, 155, 230);
                g.DrawLine(pen, 6, 12, 9, 17); g.DrawLine(pen, 18, 12, 15, 17);
                g.DrawLine(pen, 9, 17, 15, 17); g.DrawLine(pen, 9, 20, 15, 20); g.DrawLine(pen, 10.5f, 22, 13.5f, 22);
                break;
            case QuizIcon.Trophy:
                g.DrawLines(pen, new[] { new PointF(6, 3), new PointF(18, 3), new PointF(17, 11), new PointF(12, 15), new PointF(7, 11), new PointF(6, 3) });
                g.DrawArc(pen, 1, 4, 8, 8, 75, 190); g.DrawArc(pen, 15, 4, 8, 8, -85, 190);
                g.DrawLine(pen, 12, 15, 12, 21); g.DrawLine(pen, 7, 21, 17, 21);
                break;
            case QuizIcon.Clock:
                g.DrawEllipse(pen, 3, 3, 18, 18); g.DrawLine(pen, 12, 6, 12, 12); g.DrawLine(pen, 12, 12, 16, 14);
                break;
            case QuizIcon.Star:
                var points = Enumerable.Range(0, 10).Select(i =>
                { double angle = -Math.PI / 2 + i * Math.PI / 5; float radius = i % 2 == 0 ? 10 : 4.5f; return new PointF(12 + (float)Math.Cos(angle) * radius, 12 + (float)Math.Sin(angle) * radius); }).ToArray();
                g.DrawPolygon(pen, points);
                break;
            case QuizIcon.Heart:
                using (var heart = new GraphicsPath())
                {
                    heart.AddBezier(12, 21, 9, 18, 1, 12, 2, 7);
                    heart.AddBezier(2, 7, 3, 1, 10, 2, 12, 7);
                    heart.AddBezier(12, 7, 14, 2, 21, 1, 22, 7);
                    heart.AddBezier(22, 7, 23, 12, 15, 18, 12, 21); g.FillPath(fill, heart);
                }
                break;
            case QuizIcon.Flame:
                using (var flame = new GraphicsPath())
                {
                    flame.AddBezier(12, 2, 14, 8, 20, 9, 19, 16);
                    flame.AddBezier(19, 16, 18, 23, 5, 23, 5, 15);
                    flame.AddBezier(5, 15, 5, 11, 9, 10, 8, 7);
                    flame.AddBezier(8, 7, 12, 8, 11, 4, 12, 2); g.DrawPath(pen, flame);
                }
                break;
            case QuizIcon.Defeat:
                g.FillEllipse(fill, 1, 1, 22, 22);
                using (var white = new Pen(Color.White, 2.1f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                {
                    g.DrawLine(white, 8, 8, 16, 16);
                    g.DrawLine(white, 16, 8, 8, 16);
                }
                break;
            case QuizIcon.Completed:
                g.FillEllipse(fill, 1, 1, 22, 22);
                using (var white = new Pen(Color.White, 2.1f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                    g.DrawLines(white, new[] { new PointF(6.5f, 12), new PointF(10.3f, 15.7f), new PointF(17.5f, 8.3f) });
                break;
        }
        g.Restore(saved);
    }
}

public class QuizIconView : Control
{
    private QuizIcon iconKind;
    [DefaultValue(QuizIcon.None)]
    public QuizIcon IconKind { get => iconKind; set { iconKind = value; Invalidate(); } }
    public QuizIconView()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw, true);
        BackColor = Color.Transparent;
        TabStop = false;
        AccessibleRole = AccessibleRole.Graphic;
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        QuizIcons.Draw(e.Graphics, IconKind, new RectangleF(1, 1, Width - 2, Height - 2), ForeColor);
        base.OnPaint(e);
    }
}
