namespace QuanLyQuanCaPhe.DTO;

public class CongThucMonDTO
{
    public int MonID { get; set; }
    public string TenMon { get; set; } = string.Empty;
    public int NguyenLieuID { get; set; }
    public string TenNguyenLieu { get; set; } = string.Empty;
    public decimal SoLuong { get; set; }
    public string DonViTinh { get; set; } = string.Empty;
    public decimal SoLuongTon { get; set; }

    public string SoLuongHienThi => SoLuong.ToString("N3");
    public string SoLuongTonHienThi => SoLuongTon.ToString("N3");
    public string TrangThaiTonHienThi => SoLuongTon <= 0
        ? "Hết hàng"
        : SoLuongTon < SoLuong
            ? "Thiếu"
            : "Đủ";
}
