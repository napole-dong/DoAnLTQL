namespace QuanLyQuanCaPhe.Data;

public class dtaLoaiMon : ISoftDelete
{
    public int ID { get; set; }
    public string TenLoai { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public ICollection<dtaMon> Mon { get; set; } = new List<dtaMon>();
}