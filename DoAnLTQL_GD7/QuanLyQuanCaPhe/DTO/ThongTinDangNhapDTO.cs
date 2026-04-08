namespace QuanLyQuanCaPhe.DTO;

public class ThongTinDangNhapDTO
{
    public int UserId { get; set; }
    public int NhanVienId { get; set; }
    public int RoleId { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public string HoVaTen { get; set; } = string.Empty;
    public string QuyenHan { get; set; } = string.Empty;
}
