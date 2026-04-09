namespace QuanLyQuanCaPhe.DTO;

public class BanHangPhieuDTO
{
    public int BanID { get; set; }
    public string TenBan { get; set; } = string.Empty;
    public int TrangThaiBan { get; set; }
    public int? HoaDonID { get; set; }
    public List<BanHangOrderItemDTO> ChiTiet { get; set; } = new();
    public decimal TamTinh => ChiTiet.Sum(x => x.ThanhTien);
}
