namespace QuanLyQuanCaPhe.Data;

public class dtaPhieuXuatKho
{
    public int ID { get; set; }
    public DateTime NgayXuat { get; set; }
    public string LyDo { get; set; } = string.Empty;
    public int NhanVienID { get; set; }

    public dtaNhanVien NhanVien { get; set; } = null!;
    public ICollection<dtaChiTietPhieuXuat> ChiTietPhieuXuat { get; set; } = new List<dtaChiTietPhieuXuat>();
}
