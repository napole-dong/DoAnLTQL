using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class BanHangDAL
{
    public BanHangPhieuDTO GetPhieuTheoBan(int banId)
    {
        using var context = new CaPheDbContext();

        var ban = context.Ban
            .AsNoTracking()
            .FirstOrDefault(x => x.ID == banId);
        if (ban == null)
        {
            return new BanHangPhieuDTO();
        }

        var hoaDonMo = context.HoaDon
            .AsNoTracking()
            .Include(x => x.HoaDon_ChiTiet)
            .ThenInclude(x => x.Mon)
            .FirstOrDefault(x => x.BanID == banId && x.TrangThai == 0);

        var chiTiet = hoaDonMo?.HoaDon_ChiTiet
            .GroupBy(x => new { x.MonID, x.Mon.TenMon, x.DonGiaBan })
            .Select(g => new BanHangOrderItemDTO
            {
                MonID = g.Key.MonID,
                TenMon = g.Key.TenMon,
                DonGia = g.Key.DonGiaBan,
                SoLuong = (short)Math.Clamp(g.Sum(x => (int)x.SoLuongBan), 1, short.MaxValue)
            })
            .OrderBy(x => x.TenMon)
            .ToList() ?? new List<BanHangOrderItemDTO>();

        return new BanHangPhieuDTO
        {
            BanID = ban.ID,
            TenBan = ban.TenBan,
            TrangThaiBan = ban.TrangThai,
            HoaDonID = hoaDonMo?.ID,
            ChiTiet = chiTiet
        };
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
        if (dsMonHopLe.Count == 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Danh sách món gọi không hợp lệ." };
        }

        var dsMonDb = context.Mon
            .Where(x => dsMonHopLe.Select(m => m.MonID).Contains(x.ID))
            .ToDictionary(x => x.ID);

        if (dsMonDb.Count != dsMonHopLe.Select(x => x.MonID).Distinct().Count())
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Có món không tồn tại trong hệ thống." };
        }

        if (dsMonDb.Values.Any(x => !x.TrangThai.Equals("Đang kinh doanh", StringComparison.OrdinalIgnoreCase)))
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Có món đang ngừng bán, vui lòng tải lại danh sách món." };
        }

        var hoaDonMo = context.HoaDon
            .Include(x => x.HoaDon_ChiTiet)
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

        foreach (var monThem in dsMonHopLe)
        {
            var mon = dsMonDb[monThem.MonID];

            var chiTiet = hoaDonMo.HoaDon_ChiTiet
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

        if (hoaDonMo.HoaDon_ChiTiet.Count == 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Hóa đơn chưa có món, không thể thanh toán." };
        }

        hoaDonMo.TrangThai = 1;
        ban.TrangThai = 0;
        context.SaveChanges();

        return new BanActionResultDTO { ThanhCong = true, ThongBao = $"Thanh toán hóa đơn HD{hoaDonMo.ID:D5} thành công." };
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
