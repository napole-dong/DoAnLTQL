namespace QuanLyQuanCaPhe.DTO;

public class MonDTO
{
    public int ID { get; set; }
    public int LoaiMonID { get; set; }
    public string TenMon { get; set; } = string.Empty;
    public string TenLoaiMon { get; set; } = string.Empty;
    public decimal DonGia { get; set; }
    public int TrangThai { get; set; } = 1;
    public string DonGiaHienThi => $"{DonGia:N0}đ";
    public string TrangThaiHienThi => TrangThai switch
    {
        1 => "Đang kinh doanh",
        0 => "Ngừng bán",
        2 => "Tạm ngừng",
        _ => "Không xác định"
    };
    public string MoTa { get; set; } = string.Empty;
    public string? HinhAnh { get; set; }
}
