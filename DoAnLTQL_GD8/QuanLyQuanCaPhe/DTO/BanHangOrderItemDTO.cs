namespace QuanLyQuanCaPhe.DTO;

public class BanHangOrderItemDTO
{
    public int MonID { get; set; }
    public string TenMon { get; set; } = string.Empty;
    public short SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public decimal ThanhTien => SoLuong * DonGia;
    public string DonGiaHienThi => $"{DonGia:N0}đ";
    public string ThanhTienHienThi => $"{ThanhTien:N0}đ";
}
