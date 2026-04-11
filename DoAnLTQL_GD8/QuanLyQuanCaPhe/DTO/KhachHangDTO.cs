namespace QuanLyQuanCaPhe.DTO;

public class KhachHangDTO
{
    public int ID { get; set; }
    public string HoVaTen { get; set; } = string.Empty;
    public string? DienThoai { get; set; }
    public string? DiaChi { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
