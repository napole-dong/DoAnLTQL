using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class DashboardBUS
{
    private readonly DashboardDAL _dashboardDAL = new();

    public DashboardDataDTO LayDuLieuDashboard(int soHoaDonGanDay = 10)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var sevenDaysAgo = today.AddDays(-6);

        return new DashboardDataDTO
        {
            TongQuan = _dashboardDAL.GetTongQuan(today, tomorrow),
            DoanhThu7Ngay = _dashboardDAL.GetDoanhThuTheoNgay(sevenDaysAgo, tomorrow),
            TopMonBanChay = _dashboardDAL.GetTopMonBanChay(sevenDaysAgo, tomorrow, 5),
            HoaDonGanDay = _dashboardDAL.GetHoaDonGanDay(soHoaDonGanDay)
        };
    }
}
