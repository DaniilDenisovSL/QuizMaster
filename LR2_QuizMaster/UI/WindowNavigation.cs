using System.Diagnostics;

namespace LR2_QuizMaster.UI;

/// <summary>Keeps the outgoing window covered until the incoming window is ready.</summary>
public static class WindowNavigation
{
    public static DialogResult ShowDialog(Form source, Form destination, bool returnToSource = true)
    {
        destination.ShowInTaskbar = false;
        using var timer = new System.Windows.Forms.Timer { Interval = 15 };
        bool returning = false, allowClose = false;
        long started = 0;
        DialogResult result = DialogResult.Cancel;
        double sourceOpacity = source.Opacity;
        double destinationOpacity = 1;

        void Shown(object? sender, EventArgs e)
        {
            started = Stopwatch.GetTimestamp();
            timer.Start();
        }
        void Closing(object? sender, FormClosingEventArgs e)
        {
            if (e.Cancel || allowClose) return;
            if (returning) { e.Cancel = true; destination.DialogResult = DialogResult.None; return; }
            // Quiz -> results is a forward step: its hidden quiz should not reappear on return.
            if (!returnToSource || source.IsDisposed || e.CloseReason is CloseReason.WindowsShutDown or CloseReason.TaskManagerClosing) return;
            result = destination.DialogResult == DialogResult.None ? DialogResult.Cancel : destination.DialogResult;
            e.Cancel = true;
            destination.DialogResult = DialogResult.None;
            returning = true;
            destination.Enabled = false;
            destinationOpacity = destination.Opacity;
            WindowPresentation.PrepareReturn(source);
            source.Opacity = sourceOpacity;
            started = Stopwatch.GetTimestamp();
            timer.Start();
        }
        void Tick(object? sender, EventArgs e)
        {
            if (destination.IsDisposed) { timer.Stop(); return; }
            double elapsed = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
            if (!returning)
            {
                if (elapsed < 210 || destination.Opacity < .99) return;
                if (!source.IsDisposed) source.Opacity = 0;
                timer.Stop();
                return;
            }
            double t = Math.Clamp(elapsed / 140, 0, 1);
            destination.Opacity = destinationOpacity * (1 - t * t * (3 - 2 * t));
            if (t < 1) return;
            timer.Stop();
            allowClose = true;
            destination.DialogResult = result;
            destination.Close();
        }

        destination.Shown += Shown;
        destination.FormClosing += Closing;
        timer.Tick += Tick;
        try { return destination.ShowDialog(source); }
        finally
        {
            timer.Stop();
            destination.Shown -= Shown;
            destination.FormClosing -= Closing;
            if (returnToSource && !source.IsDisposed)
            {
                WindowPresentation.PrepareReturn(source);
                source.Opacity = sourceOpacity;
            }
        }
    }
}
