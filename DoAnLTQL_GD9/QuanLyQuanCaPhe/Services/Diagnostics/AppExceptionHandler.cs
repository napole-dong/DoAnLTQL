using System.Windows.Forms;

namespace QuanLyQuanCaPhe.Services.Diagnostics;

public static class AppExceptionHandler
{
    public static string Handle(Exception exception, string source, bool showDialog)
    {
        var correlationId = CorrelationContext.EnsureCorrelationId();
        var mappedError = AppExceptionMapper.Map(exception);
        AppLogger.Error(
            exception,
            $"Unhandled exception captured at {source}.",
            source,
            mappedError.Code);

        if (showDialog)
        {
            MessageBox.Show(
                BuildUserMessage(mappedError.UserMessage, mappedError.Code, correlationId),
                "Loi he thong",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        return correlationId;
    }

    public static string CreateUserMessage(string baseMessage, Exception? exception = null, string? forceErrorCode = null)
    {
        var correlationId = CorrelationContext.EnsureCorrelationId();

        if (!string.IsNullOrWhiteSpace(forceErrorCode))
        {
            return BuildInlineUserMessage(baseMessage, forceErrorCode, correlationId);
        }

        var mappedError = AppExceptionMapper.Map(exception, baseMessage);
        return BuildInlineUserMessage(mappedError.UserMessage, mappedError.Code, correlationId);
    }

    private static string BuildUserMessage(string userMessage, string errorCode, string correlationId)
    {
        return $"{userMessage}\r\n"
             + $"Ma loi: {errorCode}-{correlationId}\r\n"
             + "Vui long gui ma nay cho quan tri vien de tra cuu log.";
    }

    private static string BuildInlineUserMessage(string userMessage, string errorCode, string correlationId)
    {
        return $"{userMessage} (Ma loi: {errorCode}-{correlationId})";
    }
}
