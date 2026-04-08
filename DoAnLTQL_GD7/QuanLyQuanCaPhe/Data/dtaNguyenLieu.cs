namespace QuanLyQuanCaPhe.Data;

public class dtaNguyenLieu
{
    public int ID { get; set; }
    public string TenNguyenLieu { get; set; } = string.Empty;
    public string DonViTinh { get; set; } = string.Empty;
    public decimal SoLuongTon { get; set; }
    public decimal MucCanhBao { get; set; }
    public decimal GiaNhapGanNhat { get; set; }
    public string TrangThaiTextLegacy { get; set; } = string.Empty;
    public int TrangThai { get; set; }

    public ICollection<dtaPhieuNhapKho> PhieuNhapKho { get; set; } = new List<dtaPhieuNhapKho>();
    public ICollection<dtaCongThucMon> CongThucMon { get; set; } = new List<dtaCongThucMon>();
    public ICollection<dtaPhieuXuatKho> PhieuXuatKho { get; set; } = new List<dtaPhieuXuatKho>();
}
