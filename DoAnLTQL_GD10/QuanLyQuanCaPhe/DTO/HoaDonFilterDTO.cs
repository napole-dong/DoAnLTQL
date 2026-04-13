namespace QuanLyQuanCaPhe.DTO;

public class HoaDonFilterDTO
{
    public string? TuKhoa { get; set; }
    public DateTime TuNgay { get; set; }
    public DateTime DenNgay { get; set; }
    public int? TrangThai { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
