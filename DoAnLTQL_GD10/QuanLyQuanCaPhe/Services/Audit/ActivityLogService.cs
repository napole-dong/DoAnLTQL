using System.Text.Json;
using System.Text.Json.Serialization;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.Services.Audit;

public class ActivityLogService : IActivityLogWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public void Log(
        int? userId,
        string action,
        string entity,
        string? entityId,
        string description,
        object? oldValue = null,
        object? newValue = null,
        string? performedBy = null)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await LogAsync(userId, action, entity, entityId, description, oldValue, newValue, performedBy, timeoutCts.Token)
                    .ConfigureAwait(false);
            }
            catch
            {
                // Never allow logging failures to interrupt business flows.
            }
        });
    }

    public async Task LogAsync(
        int? userId,
        string action,
        string entity,
        string? entityId,
        string description,
        object? oldValue = null,
        object? newValue = null,
        string? performedBy = null,
        CancellationToken cancellationToken = default)
    {
        var actionKey = ChuanHoa(action, "UNKNOWN", 50);
        var entityName = ChuanHoa(entity, "Unknown", 100);
        var entityIdValue = ChuanHoa(entityId, "-", 50);
        var descriptionText = ChuanHoa(description, "No description.", 1000);

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        var actor = ChuanHoa(
            performedBy,
            string.IsNullOrWhiteSpace(nguoiDung?.TenDangNhap) ? "system" : nguoiDung!.TenDangNhap,
            150);

        var oldJson = SerializeSafely(oldValue);
        var newJson = SerializeSafely(new
        {
            Description = descriptionText,
            UserId = userId,
            Data = newValue
        });

        try
        {
            await using var context = new CaPheDbContext();
            context.AuditLog.Add(new dtaAuditLog
            {
                Action = actionKey,
                EntityName = entityName,
                EntityId = entityIdValue,
                OldValue = oldJson,
                NewValue = newJson,
                PerformedBy = actor,
                CreatedAt = DateTime.Now
            });

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            AppLogger.Warning(
                $"Write audit log failed. Action={actionKey}, Entity={entityName}, EntityId={entityIdValue}, Error={ex.Message}",
                nameof(ActivityLogService));
        }
    }

    private static string ChuanHoa(string? value, string fallback, int maxLength)
    {
        var text = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        if (text.Length <= maxLength)
        {
            return text;
        }

        return text[..maxLength];
    }

    private static string? SerializeSafely(object? payload)
    {
        if (payload == null)
        {
            return null;
        }

        try
        {
            return JsonSerializer.Serialize(payload, JsonOptions);
        }
        catch
        {
            return null;
        }
    }
}
