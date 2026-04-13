namespace QuanLyQuanCaPhe.Data;

public class dtaPhieuNhapKho
{
    public int ID { get; set; }
    public DateTime NgayNhap { get; set; }
    public string? GhiChu { get; set; }
    public int NhanVienID { get; set; }

    public dtaNhanVien NhanVien { get; set; } = null!;
    public ICollection<dtaChiTietPhieuNhap> ChiTietPhieuNhap { get; set; } = new List<dtaChiTietPhieuNhap>();
}
