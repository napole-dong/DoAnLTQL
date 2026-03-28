namespace QuanLyQuanCaPhe.DTO;

public class NhanVienDTO
{
    public int ID { get; set; }
    public string HoVaTen { get; set; } = string.Empty;
    public string? DienThoai { get; set; }
    public string? DiaChi { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public string? MatKhau { get; set; }
    public string QuyenHan { get; set; } = string.Empty;
}
