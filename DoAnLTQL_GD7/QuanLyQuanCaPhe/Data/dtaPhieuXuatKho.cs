namespace QuanLyQuanCaPhe.Data;

public class dtaPhieuXuatKho
{
    public int ID { get; set; }
    public int NguyenLieuID { get; set; }
    public decimal SoLuongXuat { get; set; }
    public DateTime NgayXuat { get; set; }
    public string LyDo { get; set; } = string.Empty;

    public dtaNguyenLieu NguyenLieu { get; set; } = null!;
}
