namespace QuanLyQuanCaPhe.Data;

public class dtaCongThucMon
{
    public int MonID { get; set; }
    public int NguyenLieuID { get; set; }
    public decimal SoLuong { get; set; }

    public dtaMon Mon { get; set; } = null!;
    public dtaNguyenLieu NguyenLieu { get; set; } = null!;
}
