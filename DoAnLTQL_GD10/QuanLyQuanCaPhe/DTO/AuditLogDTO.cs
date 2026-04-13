namespace QuanLyQuanCaPhe.DTO;

public class AuditLogDTO
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string PerformedBy { get; set; } = string.Empty;

    public string MucDo { get; set; } = "Info";
    public string ChiTietHienThi { get; set; } = string.Empty;
    public string DiaChiIp { get; set; } = "-";

    public string ThoiGianHienThi => CreatedAt.ToString("dd/MM/yyyy HH:mm:ss");
    public string NguoiThucHienHienThi => string.IsNullOrWhiteSpace(PerformedBy) ? "system" : PerformedBy;
    public string HanhDongHienThi => Action;
    public string BangDuLieuHienThi => EntityName;
    public string DoiTuongHienThi => string.IsNullOrWhiteSpace(EntityId) ? "-" : EntityId;
}
