namespace QuanLyQuanCaPhe.Data;

public class dtaChiTietPhieuNhap
{
    public int PhieuNhapID { get; set; }
    public int NguyenLieuID { get; set; }
    public decimal SoLuong { get; set; }
    public decimal DonGiaNhap { get; set; }
    public decimal ThanhTien { get; set; }

    public dtaPhieuNhapKho PhieuNhap { get; set; } = null!;
    public dtaNguyenLieu NguyenLieu { get; set; } = null!;
}
