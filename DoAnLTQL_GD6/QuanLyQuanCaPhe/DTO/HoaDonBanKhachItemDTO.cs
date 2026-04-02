namespace QuanLyQuanCaPhe.DTO;

public class HoaDonBanKhachItemDTO
{
    public int BanID { get; set; }
    public string TenBan { get; set; } = string.Empty;
    public int TrangThaiBan { get; set; }

    public string TenHienThi
    {
        get
        {
            var trangThai = TrangThaiBan == 1 ? "Đang phục vụ" : "Trống";
            return $"{TenBan} - {trangThai}";
        }
    }
}
