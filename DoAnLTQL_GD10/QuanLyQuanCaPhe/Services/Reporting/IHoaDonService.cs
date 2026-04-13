using System.Threading.Tasks;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.Reporting
{
    // Reporting-specific interface to avoid name collision with BUS.IHoaDonService
    public interface IHoaDonReportingService
    {
        Task<HoaDonPrintDto?> GetHoaDonPrintDtoAsync(int hoaDonId);
    }
}
