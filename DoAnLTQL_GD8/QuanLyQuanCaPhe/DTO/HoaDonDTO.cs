namespace QuanLyQuanCaPhe.DTO;

public class HoaDonDTO
{
    public int ID { get; set; }
    public DateTime NgayLap { get; set; }
    public int BanID { get; set; }
    public string TenBan { get; set; } = string.Empty;
    public int KhachHangID { get; set; }
    public string TenKhachHang { get; set; } = string.Empty;
    public int NhanVienID { get; set; }
    public string TenNhanVien { get; set; } = string.Empty;
    public int TrangThai { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public string TrangThaiText { get; set; } = string.Empty;
    public decimal TongTien { get; set; }
    public List<HoaDonChiTietDTO> ChiTiet { get; set; } = new();

    public string MaHoaDonHienThi => $"HD{ID:D5}";
    public string NgayLapHienThi => NgayLap.ToString("dd/MM/yyyy HH:mm");
    public string TongTienHienThi => $"{TongTien:N0}đ";

    public string BanKhachHienThi
    {
        get
        {
            if (string.IsNullOrWhiteSpace(TenKhachHang))
            {
                return TenBan;
            }

            return $"{TenBan} / {TenKhachHang}";
        }
    }
}
