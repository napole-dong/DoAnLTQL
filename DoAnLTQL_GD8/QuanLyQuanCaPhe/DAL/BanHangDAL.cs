using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class BanHangDAL
{
    private readonly OrderService _orderService = new();
    private readonly HoaDonDAL _hoaDonDAL = new();

    public BanHangPhieuDTO GetPhieuTheoBan(int banId)
    {
        using var context = new CaPheDbContext();

        var banRow = QueryBanRow(context, banId);
        if (banRow == null)
        {
            return new BanHangPhieuDTO();
        }

        var hoaDonDangHoatDong = QueryHoaDonDangHoatDongSnapshot(context, banId);
        var chiTietRows = hoaDonDangHoatDong != null
            ? QueryHoaDonChiTietRows(context, hoaDonDangHoatDong.ID)
            : new List<BanHangOrderItemReadModel>();

        return MapBanHangPhieuDto(banRow, hoaDonDangHoatDong, chiTietRows);
    }

    public BanActionResultDTO GoiMon(int banId, IEnumerable<BanHangThemMonDTO> dsMonThem)
    {
        if (banId <= 0)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Vui lòng chọn bàn trước khi gọi món."
            };
        }

        using var context = new CaPheDbContext();

        var banTonTai = context.Ban
            .AsNoTracking()
            .Any(x => x.ID == banId);
        if (!banTonTai)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Không tìm thấy bàn để gọi món."
            };
        }

        var hoaDonDangHoatDong = QueryHoaDonDangHoatDongSnapshot(context, banId);
        if (hoaDonDangHoatDong == null)
        {
            var ketQuaTaoHoaDon = _hoaDonDAL.ThemHoaDon(new HoaDonSaveRequestDTO
            {
                BanID = banId,
                NgayLap = DateTime.Now,
                TrangThai = (int)HoaDonTrangThai.Draft
            });

            if (!ketQuaTaoHoaDon.ThanhCong)
            {
                return new BanActionResultDTO
                {
                    ThanhCong = false,
                    ThongBao = ketQuaTaoHoaDon.ThongBao
                };
            }

            hoaDonDangHoatDong = new HoaDonDangHoatDongSnapshotReadModel
            {
                ID = ketQuaTaoHoaDon.HoaDonId,
                RowVersion = null
            };
        }

        if (hoaDonDangHoatDong.TrangThaiHoaDon == (int)HoaDonTrangThai.Paid)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Bàn đã thanh toán, vui lòng dọn bàn trước khi gọi món mới."
            };
        }

        return _orderService.AddItemsToOrder(
            hoaDonDangHoatDong.ID,
            dsMonThem,
            successMessage: "Gọi món thành công.",
            expectedRowVersion: hoaDonDangHoatDong.RowVersion);
    }

    public BanActionResultDTO ThanhToan(int banId)
    {
        if (banId <= 0)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Vui lòng chọn bàn trước khi thanh toán."
            };
        }

        using var context = new CaPheDbContext();

        var banTonTai = context.Ban
            .AsNoTracking()
            .Any(x => x.ID == banId);
        if (!banTonTai)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Không tìm thấy bàn cần thanh toán."
            };
        }

        var hoaDonDangHoatDong = QueryHoaDonDangHoatDongSnapshot(context, banId);
        if (hoaDonDangHoatDong == null)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Bàn này chưa có hóa đơn mở."
            };
        }

        if (hoaDonDangHoatDong.TrangThaiHoaDon == (int)HoaDonTrangThai.Paid)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Hóa đơn của bàn này đã thanh toán. Vui lòng dọn bàn để tiếp tục phục vụ."
            };
        }

        return _orderService.Checkout(hoaDonDangHoatDong.ID, hoaDonDangHoatDong.RowVersion);
    }

    private static BanReadModel? QueryBanRow(CaPheDbContext context, int banId)
    {
        return context.Ban
            .AsNoTracking()
            .Where(x => x.ID == banId)
            .Select(x => new BanReadModel
            {
                ID = x.ID,
                TenBan = x.TenBan,
                TrangThaiBan = x.TrangThai
            })
            .FirstOrDefault();
    }

    private static HoaDonDangHoatDongSnapshotReadModel? QueryHoaDonDangHoatDongSnapshot(CaPheDbContext context, int banId)
    {
        return context.HoaDon
            .AsNoTracking()
            .Where(x => x.BanID == banId
                && x.TrangThai != (int)HoaDonTrangThai.Closed
                && x.TrangThai != (int)HoaDonTrangThai.Cancelled)
            .OrderByDescending(x => x.NgayLap)
            .ThenByDescending(x => x.ID)
            .Select(x => new HoaDonDangHoatDongSnapshotReadModel
            {
                ID = x.ID,
                TrangThaiHoaDon = x.TrangThai,
                RowVersion = x.RowVersion,
                KhachHangID = x.KhachHangID,
                TenKhachHang = x.CustomerName
            })
            .FirstOrDefault();
    }

    private static List<BanHangOrderItemReadModel> QueryHoaDonChiTietRows(CaPheDbContext context, int hoaDonId)
    {
        return context.HoaDon_ChiTiet
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => x.HoaDonID == hoaDonId)
            .GroupBy(x => new { x.MonID, x.Mon.TenMon, x.DonGiaBan })
            .Select(g => new BanHangOrderItemReadModel
            {
                MonID = g.Key.MonID,
                TenMon = g.Key.TenMon,
                DonGia = g.Key.DonGiaBan,
                TongSoLuong = g.Sum(x => (int)x.SoLuongBan)
            })
            .OrderBy(x => x.TenMon)
            .ToList();
    }

    private static BanHangPhieuDTO MapBanHangPhieuDto(
        BanReadModel banRow,
        HoaDonDangHoatDongSnapshotReadModel? hoaDonDangHoatDong,
        IEnumerable<BanHangOrderItemReadModel> chiTietRows)
    {
        return new BanHangPhieuDTO
        {
            BanID = banRow.ID,
            TenBan = banRow.TenBan,
            TrangThaiBan = banRow.TrangThaiBan,
            HoaDonID = hoaDonDangHoatDong?.ID,
            TrangThaiHoaDon = hoaDonDangHoatDong?.TrangThaiHoaDon,
            HoaDonRowVersion = hoaDonDangHoatDong?.RowVersion,
            KhachHangID = hoaDonDangHoatDong?.KhachHangID,
            TenKhachHang = string.IsNullOrWhiteSpace(hoaDonDangHoatDong?.TenKhachHang)
                ? "Khách lẻ"
                : hoaDonDangHoatDong?.TenKhachHang ?? "Khách lẻ",
            ChiTiet = chiTietRows.Select(MapBanHangOrderItemDto).ToList()
        };
    }

    private static BanHangOrderItemDTO MapBanHangOrderItemDto(BanHangOrderItemReadModel chiTietRow)
    {
        return new BanHangOrderItemDTO
        {
            MonID = chiTietRow.MonID,
            TenMon = chiTietRow.TenMon,
            DonGia = chiTietRow.DonGia,
            SoLuong = (short)Math.Clamp(chiTietRow.TongSoLuong, 1, short.MaxValue)
        };
    }

    private sealed class BanReadModel
    {
        public int ID { get; init; }
        public string TenBan { get; init; } = string.Empty;
        public int TrangThaiBan { get; init; }
    }

    private sealed class BanHangOrderItemReadModel
    {
        public int MonID { get; init; }
        public string TenMon { get; init; } = string.Empty;
        public decimal DonGia { get; init; }
        public int TongSoLuong { get; init; }
    }

    private sealed class HoaDonDangHoatDongSnapshotReadModel
    {
        public int ID { get; init; }
        public int TrangThaiHoaDon { get; init; }
        public byte[]? RowVersion { get; init; }
        public int? KhachHangID { get; init; }
        public string TenKhachHang { get; init; } = string.Empty;
    }
}
