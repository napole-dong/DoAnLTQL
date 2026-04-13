namespace QuanLyQuanCaPhe.Data;

public class dtaChiTietPhieuXuat
{
    public int PhieuXuatID { get; set; }
    public int NguyenLieuID { get; set; }
    public decimal SoLuong { get; set; }

    public dtaPhieuXuatKho PhieuXuat { get; set; } = null!;
    public dtaNguyenLieu NguyenLieu { get; set; } = null!;
}
