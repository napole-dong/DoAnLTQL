namespace QuanLyQuanCaPhe.Data;

public class dtaMon
{
    public int ID { get; set; }
    public int LoaiMonID { get; set; }
    public string TenMon { get; set; } = string.Empty;
    public decimal DonGia { get; set; }
    public string? HinhAnh { get; set; }
    public string? MoTa { get; set; }
    public string TrangThaiTextLegacy { get; set; } = "Đang kinh doanh";
    public int TrangThai { get; set; }

    public dtaLoaiMon LoaiMon { get; set; } = null!;
    public ICollection<dtHoaDon_ChiTiet> HoaDon_ChiTiet { get; set; } = new List<dtHoaDon_ChiTiet>();
    public ICollection<dtaCongThucMon> CongThucMon { get; set; } = new List<dtaCongThucMon>();
}