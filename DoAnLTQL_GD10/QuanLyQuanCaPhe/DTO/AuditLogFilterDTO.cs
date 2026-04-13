namespace QuanLyQuanCaPhe.DTO;

public class AuditLogFilterDTO
{
    public DateTime TuNgay { get; set; }
    public DateTime DenNgay { get; set; }
    public string? MucDo { get; set; }
    public string? HanhDong { get; set; }
    public string? BangDuLieu { get; set; }
    public string? NguoiDung { get; set; }
    public string? TuKhoa { get; set; }
    public int SoLuongToiDa { get; set; } = 1000;
}
