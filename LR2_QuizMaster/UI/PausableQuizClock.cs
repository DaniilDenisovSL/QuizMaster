namespace LR2_QuizMaster.UI;

/// <summary>Freezes the UI-owned quiz clock while the exit confirmation is open.</summary>
internal sealed class PausableQuizClock(TimeProvider source) : TimeProvider
{
    private TimeSpan pausedTime;
    private DateTimeOffset? pausedAt;

    public override DateTimeOffset GetUtcNow() => (pausedAt ?? source.GetUtcNow()) - pausedTime;

    public IDisposable Pause()
    {
        if (pausedAt != null) throw new InvalidOperationException("The quiz clock is already paused.");
        pausedAt = source.GetUtcNow();
        return new PauseScope(this);
    }

    private void Resume()
    {
        if (pausedAt is not { } start) return;
        pausedTime += source.GetUtcNow() - start;
        pausedAt = null;
    }

    private sealed class PauseScope(PausableQuizClock clock) : IDisposable
    {
        private bool disposed;
        public void Dispose() { if (!disposed) { disposed = true; clock.Resume(); } }
    }
}
