namespace QuanLyQuanCaPhe.DTO;

public class MonDTO
{
    public int ID { get; set; }
    public int LoaiMonID { get; set; }
    public string TenMon { get; set; } = string.Empty;
    public string TenLoaiMon { get; set; } = string.Empty;
    public int DonGia { get; set; }
    public string DonGiaHienThi => $"{DonGia:N0}đ";
    public string MoTa { get; set; } = string.Empty;
    public string? HinhAnh { get; set; }
}
