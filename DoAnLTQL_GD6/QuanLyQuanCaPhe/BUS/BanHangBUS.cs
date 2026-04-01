using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class BanHangBUS
{
    private readonly BanHangDAL _banHangDAL = new();

    public BanHangPhieuDTO LayPhieuTheoBan(int banId)
    {
        if (banId <= 0)
        {
            return new BanHangPhieuDTO();
        }

        return _banHangDAL.GetPhieuTheoBan(banId);
    }

    public BanActionResultDTO GoiMon(int banId, IEnumerable<BanHangThemMonDTO> dsMonThem)
    {
        if (banId <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Vui lòng chọn bàn trước khi gọi món." };
        }

        var dsMonTongHop = dsMonThem
            .Where(x => x.MonID > 0 && x.SoLuong > 0)
            .GroupBy(x => x.MonID)
            .Select(g => new BanHangThemMonDTO
            {
                MonID = g.Key,
                SoLuong = (short)Math.Clamp(g.Sum(x => (int)x.SoLuong), 1, short.MaxValue)
            })
            .ToList();

        if (dsMonTongHop.Count == 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Chưa có món hợp lệ để gọi." };
        }

        return _banHangDAL.GoiMon(banId, dsMonTongHop);
    }

    public BanActionResultDTO ThanhToan(int banId)
    {
        if (banId <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Vui lòng chọn bàn trước khi thanh toán." };
        }

        return _banHangDAL.ThanhToan(banId);
    }
}