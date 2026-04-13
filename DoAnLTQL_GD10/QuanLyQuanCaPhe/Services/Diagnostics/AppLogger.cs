using QuanLyQuanCaPhe.Services.Auth;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace QuanLyQuanCaPhe.Services.Diagnostics;

public static class AppLogger
{
    private static readonly object SyncLock = new();
    private static string _logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
    private static bool _initialized;
    private static Serilog.ILogger _logger = Log.Logger;

    public static void Initialize(string? logDirectory = null, string? minimumLevel = null)
    {
        lock (SyncLock)
        {
            if (!string.IsNullOrWhiteSpace(logDirectory))
            {
                _logDirectory = logDirectory.Trim();
            }

            Directory.CreateDirectory(_logDirectory);

            var minLevel = ResolveMinimumLevel(minimumLevel);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(minLevel)
                .Enrich.WithProperty("Application", "QuanLyQuanCaPhe")
                .WriteTo.File(
                    formatter: new CompactJsonFormatter(),
                    path: Path.Combine(_logDirectory, "application-.json"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    shared: true)
                .CreateLogger();

            _logger = Log.Logger;
            _initialized = true;
        }

        Info("Logger initialized (Serilog JSON rolling file).", nameof(AppLogger));
    }

    public static void Info(string message, string? source = null)
    {
        try
        {
            EnsureInitialized();
            CreateContextLogger(source)
                .Information("{Message}", message);
        }
        catch
        {
            // Never throw from logging to avoid cascading failures.
        }
    }

    public static void Warning(string message, string? source = null, string? errorCode = null)
    {
        try
        {
            EnsureInitialized();
            CreateContextLogger(source, errorCode)
                .Warning("{Message}", message);
        }
        catch
        {
            // Never throw from logging to avoid cascading failures.
        }
    }

    public static void Error(Exception exception, string message, string? source = null, string? errorCode = null)
    {
        try
        {
            EnsureInitialized();
            CreateContextLogger(source, errorCode)
                .Error(exception, "{Message}", message);
        }
        catch
        {
            // Never throw from logging to avoid cascading failures.
        }
    }

    public static void Audit(string action, string message, object? auditData = null, string? source = null)
    {
        try
        {
            EnsureInitialized();

            var logger = CreateContextLogger(source)
                .ForContext("LogCategory", "Audit")
                .ForContext("AuditAction", action);

            if (auditData == null)
            {
                logger.Information("{Message}", message);
                return;
            }

            logger.Information("{Message} {@AuditData}", message, auditData);
        }
        catch
        {
            // Never throw from logging to avoid cascading failures.
        }
    }

    public static void Shutdown()
    {
        lock (SyncLock)
        {
            if (!_initialized)
            {
                return;
            }

            Log.CloseAndFlush();
            _initialized = false;
        }
    }

    private static void EnsureInitialized()
    {
        if (_initialized)
        {
            return;
        }

        lock (SyncLock)
        {
            if (_initialized)
            {
                return;
            }

            Directory.CreateDirectory(_logDirectory);

            var minLevel = ResolveMinimumLevel(null);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(minLevel)
                .Enrich.WithProperty("Application", "QuanLyQuanCaPhe")
                .WriteTo.File(
                    formatter: new CompactJsonFormatter(),
                    path: Path.Combine(_logDirectory, "application-.json"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    shared: true)
                .CreateLogger();

            _logger = Log.Logger;
            _initialized = true;
        }
    }

    private static Serilog.ILogger CreateContextLogger(string? source, string? errorCode = null)
    {
        var logger = _logger;

        var correlationId = CorrelationContext.CurrentId;
        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            logger = logger.ForContext("CorrelationId", correlationId);
        }

        if (!string.IsNullOrWhiteSpace(source))
        {
            logger = logger.ForContext("Component", source);
        }

        if (!string.IsNullOrWhiteSpace(errorCode))
        {
            logger = logger.ForContext("ErrorCode", errorCode);
        }

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        if (nguoiDung != null)
        {
            logger = logger
                .ForContext("UserName", nguoiDung.TenDangNhap)
                .ForContext("DisplayName", nguoiDung.HoVaTen)
                .ForContext("UserRole", nguoiDung.QuyenHan);
        }

        return logger;
    }

    private static LogEventLevel ResolveMinimumLevel(string? explicitLevel)
    {
        var levelText = explicitLevel;
        if (string.IsNullOrWhiteSpace(levelText))
        {
            levelText = Environment.GetEnvironmentVariable("CAFE_LOG_LEVEL");
        }

        if (Enum.TryParse<LogEventLevel>(levelText, ignoreCase: true, out var level))
        {
            return level;
        }

        return LogEventLevel.Information;
    }
}
