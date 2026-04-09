using System.Threading;

namespace QuanLyQuanCaPhe.Services.Diagnostics;

public static class CorrelationContext
{
    private static readonly AsyncLocal<string?> CurrentIdStore = new();

    public static string? CurrentId => CurrentIdStore.Value;

    public static string EnsureCorrelationId()
    {
        if (string.IsNullOrWhiteSpace(CurrentIdStore.Value))
        {
            CurrentIdStore.Value = Guid.NewGuid().ToString("N");
        }

        return CurrentIdStore.Value;
    }

    public static IDisposable BeginScope(string? correlationId = null)
    {
        var previous = CurrentIdStore.Value;
        CurrentIdStore.Value = string.IsNullOrWhiteSpace(correlationId)
            ? Guid.NewGuid().ToString("N")
            : correlationId.Trim();

        return new CorrelationScope(previous);
    }

    public static void Clear()
    {
        CurrentIdStore.Value = null;
    }

    private sealed class CorrelationScope(string? previousId) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            CurrentIdStore.Value = previousId;
            _disposed = true;
        }
    }
}
