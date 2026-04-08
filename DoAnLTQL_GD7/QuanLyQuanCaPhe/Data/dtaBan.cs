namespace QuanLyQuanCaPhe.Data;

public class dtaBan
{
    public int ID { get; set; }
    public string TenBan { get; set; } = string.Empty;
    public int TrangThai { get; set; }

    public ICollection<dtaHoadon> HoaDon { get; set; } = new List<dtaHoadon>();
}