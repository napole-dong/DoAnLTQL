namespace QuanLyQuanCaPhe.DTO;

public class BanHangTrangThaiPhieuDTO
{
    public int BanID { get; set; }
    public string TenBan { get; set; } = string.Empty;
    public int TrangThaiBan { get; set; }
    public int? HoaDonID { get; set; }
    public int? TrangThaiHoaDon { get; set; }
    public int? KhachHangID { get; set; }
    public string TenKhachHang { get; set; } = string.Empty;
    public int SoMonChoGoi { get; set; }
    public int TongMon { get; set; }
    public decimal TongTien { get; set; }
    public List<BanHangOrderItemDTO> ChiTietHienThi { get; set; } = new();
}