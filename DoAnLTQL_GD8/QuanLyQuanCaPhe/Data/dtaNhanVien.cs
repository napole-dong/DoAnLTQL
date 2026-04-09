namespace QuanLyQuanCaPhe.Data;

public class dtaNhanVien
{
    public int ID { get; set; }
    public string HoVaTen { get; set; } = string.Empty;
    public string? DienThoai { get; set; }
    public string? DiaChi { get; set; }

    public dtaUser? User { get; set; }
    public ICollection<dtaHoadon> HoaDon { get; set; } = new List<dtaHoadon>();
}