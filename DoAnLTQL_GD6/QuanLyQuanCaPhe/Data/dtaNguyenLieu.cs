namespace QuanLyQuanCaPhe.Data;

public class dtaNguyenLieu
{
    public int ID { get; set; }
    public string TenNguyenLieu { get; set; } = string.Empty;
    public string DonViTinh { get; set; } = string.Empty;
    public decimal SoLuongTon { get; set; }
    public decimal MucCanhBao { get; set; }
    public decimal GiaNhapGanNhat { get; set; }
    public string TrangThai { get; set; } = string.Empty;
}
