using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class BanHangDAL
{
    public BanHangPhieuDTO GetPhieuTheoBan(int banId)
    {
        using var context = new CaPheDbContext();

        var banRow = QueryBanRow(context, banId);
        if (banRow == null)
        {
            return new BanHangPhieuDTO();
        }

        var hoaDonMoId = QueryHoaDonMoId(context, banId);
        var chiTietRows = hoaDonMoId.HasValue
            ? QueryHoaDonChiTietRows(context, hoaDonMoId.Value)
            : new List<BanHangOrderItemReadModel>();

        return MapBanHangPhieuDto(banRow, hoaDonMoId, chiTietRows);
    }

    public BanActionResultDTO GoiMon(int banId, IEnumerable<BanHangThemMonDTO> dsMonThem)
    {
        using var context = new CaPheDbContext();

        var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
        if (ban == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy bàn để gọi món." };
        }

        var dsMonHopLe = dsMonThem
            .Where(x => x.MonID > 0 && x.SoLuong > 0)
            .ToList();
        if (!dsMonHopLe.Any())
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Danh sách món gọi không hợp lệ." };
        }

        var dsMonId = dsMonHopLe
            .Select(x => x.MonID)
            .Distinct()
            .ToList();

        var dsMonDb = context.Mon
            .Where(x => dsMonId.Contains(x.ID))
            .ToDictionary(x => x.ID);

        if (dsMonDb.Count != dsMonId.Count)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Có món không tồn tại trong hệ thống." };
        }

        if (dsMonDb.Values.Any(x => x.TrangThai != 1))
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Có món đang ngừng bán, vui lòng tải lại danh sách món." };
        }

        var hoaDonMo = context.HoaDon
            .AsNoTracking()
            .FirstOrDefault(x => x.BanID == banId && x.TrangThai == 0);

        if (hoaDonMo == null)
        {
            var nhanVienId = GetOrCreateNhanVienMacDinh(context);
            var khachHangId = GetOrCreateKhachLe(context);

            hoaDonMo = new dtaHoadon
            {
                BanID = banId,
                NhanVienID = nhanVienId,
                KhachHangID = khachHangId,
                NgayLap = DateTime.Now,
                TrangThai = 0,
                GhiChuHoaDon = string.Empty
            };

            context.HoaDon.Add(hoaDonMo);
            context.SaveChanges();
        }

        var chiTietHoaDon = context.HoaDon_ChiTiet
            .Where(x => x.HoaDonID == hoaDonMo.ID)
            .ToList();

        foreach (var monThem in dsMonHopLe)
        {
            var mon = dsMonDb[monThem.MonID];

            var chiTiet = chiTietHoaDon
                .FirstOrDefault(x => x.MonID == monThem.MonID && x.DonGiaBan == mon.DonGia && x.GhiChu == null);

            if (chiTiet == null)
            {
                context.HoaDon_ChiTiet.Add(new dtHoaDon_ChiTiet
                {
                    HoaDonID = hoaDonMo.ID,
                    MonID = monThem.MonID,
                    SoLuongBan = monThem.SoLuong,
                    DonGiaBan = mon.DonGia,
                    GhiChu = null
                });
            }
            else
            {
                chiTiet.SoLuongBan = (short)Math.Clamp(chiTiet.SoLuongBan + monThem.SoLuong, 1, short.MaxValue);
            }
        }

        ban.TrangThai = 1;
        context.SaveChanges();

        return new BanActionResultDTO { ThanhCong = true, ThongBao = "Gọi món thành công." };
    }

    public BanActionResultDTO ThanhToan(int banId)
    {
        using var context = new CaPheDbContext();

        var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
        if (ban == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy bàn cần thanh toán." };
        }

        var hoaDonMo = context.HoaDon
            .Include(x => x.HoaDon_ChiTiet)
            .FirstOrDefault(x => x.BanID == banId && x.TrangThai == 0);

        if (hoaDonMo == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Bàn này chưa có hóa đơn mở." };
        }

        if (!hoaDonMo.HoaDon_ChiTiet.Any())
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Hóa đơn chưa có món, không thể thanh toán." };
        }

        hoaDonMo.TrangThai = 1;
        ban.TrangThai = 0;
        context.SaveChanges();

        return new BanActionResultDTO { ThanhCong = true, ThongBao = $"Thanh toán hóa đơn HD{hoaDonMo.ID:D5} thành công." };
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

    private static int? QueryHoaDonMoId(CaPheDbContext context, int banId)
    {
        return context.HoaDon
            .AsNoTracking()
            .Where(x => x.BanID == banId && x.TrangThai == 0)
            .Select(x => (int?)x.ID)
            .FirstOrDefault();
    }

    private static List<BanHangOrderItemReadModel> QueryHoaDonChiTietRows(CaPheDbContext context, int hoaDonId)
    {
        return context.HoaDon_ChiTiet
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
        int? hoaDonMoId,
        IEnumerable<BanHangOrderItemReadModel> chiTietRows)
    {
        return new BanHangPhieuDTO
        {
            BanID = banRow.ID,
            TenBan = banRow.TenBan,
            TrangThaiBan = banRow.TrangThaiBan,
            HoaDonID = hoaDonMoId,
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

    private static int GetOrCreateNhanVienMacDinh(CaPheDbContext context)
    {
        var nhanVien = context.NhanVien.OrderBy(x => x.ID).FirstOrDefault();
        if (nhanVien != null)
        {
            return nhanVien.ID;
        }

        nhanVien = new dtaNhanVien
        {
            HoVaTen = "Nhân viên bán hàng",
            DienThoai = null,
            DiaChi = null
        };

        context.NhanVien.Add(nhanVien);
        context.SaveChanges();

        return nhanVien.ID;
    }

    private static int GetOrCreateKhachLe(CaPheDbContext context)
    {
        var khach = context.KhachHang.FirstOrDefault(x => x.HoVaTen == "Khách lẻ")
                    ?? context.KhachHang.OrderBy(x => x.ID).FirstOrDefault();

        if (khach != null)
        {
            return khach.ID;
        }

        khach = new dtaKhachHang
        {
            HoVaTen = "Khách lẻ",
            DienThoai = null,
            DiaChi = null
        };

        context.KhachHang.Add(khach);
        context.SaveChanges();

        return khach.ID;
    }
}
