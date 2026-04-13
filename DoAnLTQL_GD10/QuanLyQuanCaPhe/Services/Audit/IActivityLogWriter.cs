namespace QuanLyQuanCaPhe.Services.Audit;

public interface IActivityLogWriter
{
    void Log(
        int? userId,
        string action,
        string entity,
        string? entityId,
        string description,
        object? oldValue = null,
        object? newValue = null,
        string? performedBy = null);

    Task LogAsync(
        int? userId,
        string action,
        string entity,
        string? entityId,
        string description,
        object? oldValue = null,
        object? newValue = null,
        string? performedBy = null,
        CancellationToken cancellationToken = default);
}
