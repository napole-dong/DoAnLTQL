namespace QuanLyQuanCaPhe.DTO;

public class BanDTO
{
    public int ID { get; set; }
    public string TenBan { get; set; } = string.Empty;
    public int TrangThai { get; set; }
    public string KhuVuc { get; set; } = string.Empty;
    public string SucChua { get; set; } = "4 người";
    public string TinhTrang { get; set; } = string.Empty;
}
