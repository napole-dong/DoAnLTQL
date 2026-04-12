namespace QuanLyQuanCaPhe.Services.UI;

public sealed class SearchDebounceHelper : IDisposable
{
    private readonly System.Windows.Forms.Timer _timer;
    private readonly Action _callback;
    private bool _disposed;

    public SearchDebounceHelper(int intervalMilliseconds, Action callback)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));

        _timer = new System.Windows.Forms.Timer
        {
            Interval = Math.Max(50, intervalMilliseconds)
        };

        _timer.Tick += Timer_Tick;
    }

    public void Signal()
    {
        if (_disposed)
        {
            return;
        }

        _timer.Stop();
        _timer.Start();
    }

    public void Flush()
    {
        if (_disposed)
        {
            return;
        }

        _timer.Stop();
        _callback();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _timer.Stop();
        _timer.Tick -= Timer_Tick;
        _timer.Dispose();
        _disposed = true;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _timer.Stop();
        _callback();
    }
}
