namespace QuanLyQuanCaPhe.DTO;

public class NguyenLieuDTO
{
    public int MaNguyenLieu { get; set; }
    public string TenNguyenLieu { get; set; } = string.Empty;
    public string DonViTinh { get; set; } = string.Empty;
    public decimal SoLuongTon { get; set; }
    public decimal MucCanhBao { get; set; }
    public decimal GiaNhapGanNhat { get; set; }
    public int TrangThai { get; set; } = 1;
    public string TrangThaiHienThi => TrangThai switch
    {
        0 => "Ngừng dùng",
        2 => SoLuongTon <= 0 ? "Hết hàng" : "Sắp hết",
        _ => SoLuongTon <= 0 ? "Hết hàng" : "Đang sử dụng"
    };
}
