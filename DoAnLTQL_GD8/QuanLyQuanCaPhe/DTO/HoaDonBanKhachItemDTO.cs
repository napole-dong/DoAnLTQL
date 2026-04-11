namespace QuanLyQuanCaPhe.DTO;

public class HoaDonBanKhachItemDTO
{
    public int BanID { get; set; }
    public string TenBan { get; set; } = string.Empty;
    public int TrangThaiBan { get; set; }
    public int? TrangThaiHoaDonDangHoatDong { get; set; }

    public string TenHienThi
    {
        get
        {
            var trangThai = TrangThaiHoaDonDangHoatDong switch
            {
                (int)HoaDonTrangThai.Draft => "Có khách",
                (int)HoaDonTrangThai.Paid => "Đã thanh toán",
                _ => TrangThaiBan switch
                {
                    1 => "Có khách",
                    2 => "Chờ dọn",
                    _ => "Trống"
                }
            };

            return $"{TenBan} - {trangThai}";
        }
    }
}
