using System.ComponentModel.DataAnnotations;

namespace QuanLyQuanCaPhe.Data;

public class dtaHoadon
{
    public int ID { get; set; }
    public int NhanVienID { get; set; }
    public int? KhachHangID { get; set; }
    public string CustomerName { get; set; } = "Khách lẻ";
    public int BanID { get; set; }
    public DateTime NgayLap { get; set; }
    public int TrangThai { get; set; }
    public decimal TongTien { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public string? GhiChuHoaDon { get; set; }

    public dtaKhachHang? KhachHang { get; set; }
    public dtaNhanVien NhanVien { get; set; } = null!;
    public dtaBan Ban { get; set; } = null!;
    public ICollection<dtHoaDon_ChiTiet> HoaDon_ChiTiet { get; set; } = new List<dtHoaDon_ChiTiet>();
}