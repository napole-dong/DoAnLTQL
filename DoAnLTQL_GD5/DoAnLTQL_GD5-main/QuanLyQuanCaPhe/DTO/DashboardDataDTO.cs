namespace QuanLyQuanCaPhe.DTO;

public class DashboardDataDTO
{
    public DashboardTongQuanDTO TongQuan { get; set; } = new();
    public List<DashboardRevenueDTO> DoanhThu7Ngay { get; set; } = new();
    public List<DashboardTopMonDTO> TopMonBanChay { get; set; } = new();
    public List<DashboardRecentBillDTO> HoaDonGanDay { get; set; } = new();
}
