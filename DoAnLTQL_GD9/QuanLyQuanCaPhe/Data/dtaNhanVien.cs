namespace QuanLyQuanCaPhe.Data;

public class dtaNhanVien : ISoftDelete
{
    public int ID { get; set; }
    public string HoVaTen { get; set; } = string.Empty;
    public string? DienThoai { get; set; }
    public string? DiaChi { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public dtaUser? User { get; set; }
    public ICollection<dtaHoadon> HoaDon { get; set; } = new List<dtaHoadon>();
}