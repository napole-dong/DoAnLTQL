namespace QuanLyQuanCaPhe.Data;

public class dtaPermission
{
    public int VaiTroID { get; set; }
    public string Feature { get; set; } = string.Empty;
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }

    public dtaVaiTro VaiTro { get; set; } = null!;
}
