namespace QuanLyQuanCaPhe.Data;

public class dtaHoadon
{
    public int ID { get; set; }
    public int NhanVienID { get; set; }
    public int? KhachHangID { get; set; }
    public int BanID { get; set; }
    public DateTime NgayLap { get; set; }
    public int TrangThai { get; set; }
    public string? GhiChuHoaDon { get; set; }

    public dtaKhachHang? KhachHang { get; set; }
    public dtaNhanVien NhanVien { get; set; } = null!;
    public dtaBan Ban { get; set; } = null!;
    public ICollection<dtHoaDon_ChiTiet> HoaDon_ChiTiet { get; set; } = new List<dtHoaDon_ChiTiet>();
}