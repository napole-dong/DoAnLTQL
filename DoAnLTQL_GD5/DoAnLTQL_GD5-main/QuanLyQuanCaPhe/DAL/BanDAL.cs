using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class BanDAL
{
    public BanThongKeDTO GetThongKe()
    {
        using var context = new CaPheDbContext();
        return new BanThongKeDTO
        {
            TongBan = context.Ban.AsNoTracking().Count(),
            BanDangPhucVu = context.Ban.AsNoTracking().Count(b => b.TrangThai == 1),
            BanTrong = context.Ban.AsNoTracking().Count(b => b.TrangThai == 0),
            BanDatTruoc = context.HoaDon
                .AsNoTracking()
                .Count(hd => hd.TrangThai == 0 && hd.GhiChuHoaDon != null && EF.Functions.Like(hd.GhiChuHoaDon, "%đặt trước%"))
        };
    }

    public List<BanDTO> GetDanhSachBan(string? khuVuc, string? trangThai, string? tuKhoa)
    {
        using var context = new CaPheDbContext();
        var dsBan = context.Ban
            .AsNoTracking()
            .OrderBy(b => b.ID)
            .ToList()
            .Select(b => new BanDTO
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai,
                KhuVuc = XacDinhKhuVuc(b.TenBan),
                TinhTrang = ChuyenTrangThaiBan(b.TrangThai)
            })
            .ToList();

        return dsBan
            .Where(b => string.IsNullOrWhiteSpace(khuVuc)
                || khuVuc == "Tất cả khu vực"
                || b.KhuVuc == khuVuc)
            .Where(b => string.IsNullOrWhiteSpace(trangThai)
                || trangThai == "Tất cả trạng thái"
                || (trangThai == "Trống" && b.TrangThai == 0)
                || (trangThai == "Đang phục vụ" && b.TrangThai == 1)
                || (trangThai == "Đặt trước" && b.TrangThai == 2))
            .Where(b => string.IsNullOrWhiteSpace(tuKhoa)
                || b.ID.ToString().Contains(tuKhoa)
                || b.TenBan.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                || b.KhuVuc.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                || b.TinhTrang.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                || (tuKhoa.Equals("trống", StringComparison.OrdinalIgnoreCase) && b.TrangThai == 0))
            .ToList();
    }

    public List<BanDTO> GetSoDoBan()
    {
        using var context = new CaPheDbContext();
        return context.Ban
            .AsNoTracking()
            .OrderBy(b => b.ID)
            .Select(b => new BanDTO
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai,
                TinhTrang = ChuyenTrangThaiBan(b.TrangThai),
                KhuVuc = XacDinhKhuVuc(b.TenBan)
            })
            .ToList();
    }

    public bool TenBanDaTonTai(string tenBan)
    {
        using var context = new CaPheDbContext();
        return context.Ban.Any(x => x.TenBan == tenBan);
    }

    public void ThemBan(string tenBan)
    {
        using var context = new CaPheDbContext();
        context.Ban.Add(new dtaBan
        {
            TenBan = tenBan,
            TrangThai = 0
        });
        context.SaveChanges();
    }

    public BanDTO? GetBanById(int banId)
    {
        using var context = new CaPheDbContext();
        return context.Ban
            .AsNoTracking()
            .Where(x => x.ID == banId)
            .Select(x => new BanDTO
            {
                ID = x.ID,
                TenBan = x.TenBan,
                TrangThai = x.TrangThai,
                KhuVuc = XacDinhKhuVuc(x.TenBan),
                TinhTrang = ChuyenTrangThaiBan(x.TrangThai)
            })
            .FirstOrDefault();
    }

    public bool BanDaPhatSinhHoaDon(int banId)
    {
        using var context = new CaPheDbContext();
        return context.HoaDon.Any(x => x.BanID == banId);
    }

    public bool XoaBan(int banId)
    {
        using var context = new CaPheDbContext();
        var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
        if (ban == null)
        {
            return false;
        }

        context.Ban.Remove(ban);
        context.SaveChanges();
        return true;
    }

    public List<BanDTO> GetDanhSachBanDich(int banNguonId)
    {
        using var context = new CaPheDbContext();
        return context.Ban
            .AsNoTracking()
            .Where(b => b.ID != banNguonId)
            .OrderBy(b => b.TenBan)
            .Select(b => new BanDTO
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai,
                KhuVuc = XacDinhKhuVuc(b.TenBan),
                TinhTrang = ChuyenTrangThaiBan(b.TrangThai)
            })
            .ToList();
    }

    public BanActionResultDTO ChuyenHoacGopBan(BanChuyenGopRequestDTO request)
    {
        using var context = new CaPheDbContext();

        var banNguon = context.Ban.FirstOrDefault(b => b.ID == request.BanNguonId);
        if (banNguon == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy bàn nguồn." };
        }

        var hoaDonNguon = context.HoaDon
            .Include(h => h.HoaDon_ChiTiet)
            .FirstOrDefault(h => h.BanID == banNguon.ID && h.TrangThai == 0);
        if (hoaDonNguon == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Bàn nguồn chưa có hóa đơn đang phục vụ để chuyển/gộp." };
        }

        var banDich = context.Ban.FirstOrDefault(b => b.ID == request.BanDichId);
        if (banDich == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy bàn đích." };
        }

        if (request.LaChuyenBan)
        {
            if (banDich.TrangThai != 0)
            {
                return new BanActionResultDTO { ThanhCong = false, ThongBao = "Chỉ có thể chuyển sang bàn trống." };
            }

            hoaDonNguon.BanID = banDich.ID;
            banNguon.TrangThai = 0;
            banDich.TrangThai = 1;
            context.SaveChanges();

            return new BanActionResultDTO { ThanhCong = true, ThongBao = "Chuyển bàn thành công." };
        }

        var hoaDonDich = context.HoaDon
            .Include(h => h.HoaDon_ChiTiet)
            .FirstOrDefault(h => h.BanID == banDich.ID && h.TrangThai == 0);

        if (hoaDonDich == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Bàn đích chưa có hóa đơn đang phục vụ để gộp." };
        }

        foreach (var ctNguon in hoaDonNguon.HoaDon_ChiTiet.ToList())
        {
            var ctDich = hoaDonDich.HoaDon_ChiTiet
                .FirstOrDefault(x => x.MonID == ctNguon.MonID && x.DonGiaBan == ctNguon.DonGiaBan && x.GhiChu == ctNguon.GhiChu);
            if (ctDich == null)
            {
                context.HoaDon_ChiTiet.Add(new dtHoaDon_ChiTiet
                {
                    HoaDonID = hoaDonDich.ID,
                    MonID = ctNguon.MonID,
                    SoLuongBan = ctNguon.SoLuongBan,
                    DonGiaBan = ctNguon.DonGiaBan,
                    GhiChu = ctNguon.GhiChu
                });
            }
            else
            {
                ctDich.SoLuongBan = (short)Math.Clamp(ctDich.SoLuongBan + ctNguon.SoLuongBan, 0, short.MaxValue);
            }
        }

        context.HoaDon_ChiTiet.RemoveRange(hoaDonNguon.HoaDon_ChiTiet);
        hoaDonNguon.TrangThai = 1;
        hoaDonNguon.GhiChuHoaDon = $"{hoaDonNguon.GhiChuHoaDon} [Đã gộp vào HD{hoaDonDich.ID:D5}]".Trim();

        banNguon.TrangThai = 0;
        banDich.TrangThai = 1;

        context.SaveChanges();
        return new BanActionResultDTO { ThanhCong = true, ThongBao = "Gộp bàn thành công." };
    }

    private static string ChuyenTrangThaiBan(int trangThai)
    {
        return trangThai switch
        {
            0 => "Sẵn sàng",
            1 => "Đang phục vụ",
            2 => "Đặt trước",
            _ => "Sẵn sàng"
        };
    }

    private static string XacDinhKhuVuc(string tenBan)
    {
        if (string.IsNullOrWhiteSpace(tenBan))
        {
            return "-";
        }

        var kyTu = char.ToUpperInvariant(tenBan.Trim().FirstOrDefault(char.IsLetter));
        return kyTu switch
        {
            'A' or 'B' => "Khu trong nhà",
            'C' => "Khu sân vườn",
            'D' => "Khu phòng riêng",
            _ => "Khu khác"
        };
    }
}
