namespace QuanLyQuanCaPhe.Data;

public class dtaPhieuNhapKho
{
    public int ID { get; set; }
    public int NguyenLieuID { get; set; }
    public decimal SoLuongNhap { get; set; }
    public decimal GiaNhap { get; set; }
    public DateTime NgayNhap { get; set; }
    public string? GhiChu { get; set; }

    public dtaNguyenLieu NguyenLieu { get; set; } = null!;
}
