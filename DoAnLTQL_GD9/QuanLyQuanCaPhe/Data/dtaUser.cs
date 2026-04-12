namespace QuanLyQuanCaPhe.Data;

public class dtaUser
{
    public int ID { get; set; }
    public int NhanVienID { get; set; }
    public int VaiTroID { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public string MatKhau { get; set; } = string.Empty;
    public bool HoatDong { get; set; }

    public dtaNhanVien NhanVien { get; set; } = null!;
    public dtaVaiTro VaiTro { get; set; } = null!;
}
