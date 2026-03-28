namespace QuanLyQuanCaPhe.DTO;

public class DashboardRecentBillDTO
{
    public int ID { get; set; }
    public DateTime NgayLap { get; set; }
    public int TrangThai { get; set; }
    public string TenBan { get; set; } = string.Empty;
    public string TenNhanVien { get; set; } = string.Empty;
    public long TongTien { get; set; }
}
