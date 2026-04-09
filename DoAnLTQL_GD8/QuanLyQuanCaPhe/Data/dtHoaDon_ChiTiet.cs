namespace QuanLyQuanCaPhe.Data;

public class dtHoaDon_ChiTiet
{
    public int ID { get; set; }
    public int HoaDonID { get; set; }
    public int MonID { get; set; }
    public short SoLuongBan { get; set; }
    public decimal DonGiaBan { get; set; }
    public string? GhiChu { get; set; }

    public dtaHoadon HoaDon { get; set; } = null!;
    public dtaMon Mon { get; set; } = null!;
}