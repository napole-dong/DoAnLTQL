using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.Services.Reporting
{
    public class HoaDonService
    {
        public dtaHoadon? GetHoaDonWithDetails(int hoaDonId)
        {
            try
            {
                using var db = new CaPheDbContext();
                var hoaDon = db.HoaDon
                    .AsNoTracking()
                    .Include(h => h.KhachHang)
                    .Include(h => h.HoaDon_ChiTiet)
                        .ThenInclude(ct => ct.Mon)
                    .FirstOrDefault(h => h.ID == hoaDonId);

                return hoaDon;
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, $"GetHoaDonWithDetails failed. HoaDonId={hoaDonId}", nameof(HoaDonService));
                return null;
            }
        }
    }
}
