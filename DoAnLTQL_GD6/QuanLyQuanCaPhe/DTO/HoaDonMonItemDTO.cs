namespace QuanLyQuanCaPhe.DTO;

public class HoaDonMonItemDTO
{
    public int MonID { get; set; }
    public string TenMon { get; set; } = string.Empty;
    public int DonGia { get; set; }
    public string TenHienThi => $"{TenMon} ({DonGia:N0}đ)";
}
