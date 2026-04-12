namespace QuanLyQuanCaPhe.Data;

public class dtaVaiTro
{
    public int ID { get; set; }
    public string TenVaiTro { get; set; } = string.Empty;
    public string? MoTa { get; set; }

    public ICollection<dtaUser> Users { get; set; } = new List<dtaUser>();
    public ICollection<dtaPermission> Permissions { get; set; } = new List<dtaPermission>();
}
