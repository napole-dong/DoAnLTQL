using System.Data;
using System.Reflection;

namespace QuanLyQuanCaPhe.Services.HoaDon
{
    public class HoaDonPrintService
    {
        public static bool ValidateEmbeddedResource(string? resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName)) return false;

            var asm = Assembly.GetExecutingAssembly();
            var names = asm.GetManifestResourceNames();
            // allow either exact match or ends-with (safer across projects)
            return names.Any(n => string.Equals(n, resourceName, StringComparison.OrdinalIgnoreCase)
                                  || n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase)
                                  || resourceName.EndsWith(n, StringComparison.OrdinalIgnoreCase));
        }

        public static bool ValidateReportData(object? data, string datasetName, string? reportResource)
        {
            if (data == null) return false;

            if (data is DataTable dt)
            {
                return dt.Rows.Count > 0;
            }

            if (data is System.Collections.IEnumerable e)
            {
                foreach (var _ in e) return true;
                return false;
            }

            return false;
        }

        public PrintValidationResult ValidatePrint(object hoaDon)
        {
            // Basic placeholder: in production this would check hoaDon.TrangThai etc.
            if (hoaDon == null)
            {
                return new PrintValidationResult(false, false, "HoaDon is null", string.Empty, string.Empty);
            }

            return new PrintValidationResult(true, false, string.Empty, string.Empty, string.Empty);
        }

        public void LogPrintAudit(int hoaDonId, string auditAction, string message)
        {
            // In production write to audit log (DB). Keep no-op for now.
        }
    }

    public record PrintValidationResult(bool Success, bool RequiresConfirmation, string Message, string AuditAction, string Watermark);
}
