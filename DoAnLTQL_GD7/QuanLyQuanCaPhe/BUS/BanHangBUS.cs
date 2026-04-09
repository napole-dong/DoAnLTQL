using System.Globalization;
using System.Text;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class BanHangBUS
{
    private readonly BanHangDAL _banHangDAL = new();
    private readonly PermissionBUS _permissionBUS = new();
    private readonly Dictionary<int, List<BanHangOrderItemDTO>> _gioTamTheoBan = new();

    public BanHangPhieuDTO LayPhieuTheoBan(int banId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View))
        {
            return new BanHangPhieuDTO();
        }

        if (banId <= 0)
        {
            return new BanHangPhieuDTO();
        }

        return _banHangDAL.GetPhieuTheoBan(banId);
    }

    public BanHangTrangThaiPhieuDTO LayTrangThaiPhieuTheoBan(int banId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View))
        {
            return new BanHangTrangThaiPhieuDTO();
        }

        if (banId <= 0)
        {
            return new BanHangTrangThaiPhieuDTO();
        }

        var phieuDb = _banHangDAL.GetPhieuTheoBan(banId);
        var dsTam = _gioTamTheoBan.TryGetValue(banId, out var gioTam)
            ? gioTam
            : new List<BanHangOrderItemDTO>();

        var dsHienThi = TongHopChiTiet(phieuDb.ChiTiet.Concat(dsTam));

        return new BanHangTrangThaiPhieuDTO
        {
            BanID = phieuDb.BanID > 0 ? phieuDb.BanID : banId,
            TenBan = string.IsNullOrWhiteSpace(phieuDb.TenBan) ? $"Bàn {banId:D2}" : phieuDb.TenBan,
            TrangThaiBan = phieuDb.TrangThaiBan,
            SoMonChoGoi = dsTam.Sum(x => x.SoLuong),
            TongMon = dsHienThi.Sum(x => x.SoLuong),
            TongTien = dsHienThi.Sum(x => x.ThanhTien),
            ChiTietHienThi = dsHienThi
        };
    }

    public BanActionResultDTO ThemMonVaoGioTam(int banId, MonDTO mon)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Create))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền thêm món.");
        }

        if (banId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn trước khi thêm món.");
        }

        if (mon.ID <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Món không hợp lệ.");
        }

        var gioTam = LayGioTamTheoBan(banId);
        var dongMon = gioTam.FirstOrDefault(x => x.MonID == mon.ID && x.DonGia == mon.DonGia);

        if (dongMon == null)
        {
            gioTam.Add(new BanHangOrderItemDTO
            {
                MonID = mon.ID,
                TenMon = mon.TenMon,
                SoLuong = 1,
                DonGia = mon.DonGia
            });

            return BusMessageCatalog.CreateActionResult(true, "Đã thêm món vào giỏ tạm.");
        }

        dongMon.SoLuong = (short)Math.Clamp(dongMon.SoLuong + 1, 1, short.MaxValue);
        return BusMessageCatalog.CreateActionResult(true, "Đã cập nhật số lượng món trong giỏ tạm.");
    }

    public bool CoMonChoGoiTrongGioTam(int banId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View))
        {
            return false;
        }

        return _gioTamTheoBan.TryGetValue(banId, out var gioTam) && gioTam.Any();
    }

    public BanActionResultDTO LuuMonChoGoi(int banId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền lưu bàn.");
        }

        return DongBoMonTamTheoBan(banId, yeuCauCoMonChoGoi: true);
    }

    public BanActionResultDTO ThanhToanHoaDon(int banId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền thanh toán.");
        }

        if (banId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn trước khi thanh toán.");
        }

        var resultDongBo = DongBoMonTamTheoBan(banId, yeuCauCoMonChoGoi: false);
        if (!resultDongBo.ThanhCong)
        {
            return resultDongBo;
        }

        return ThanhToan(banId);
    }

    public List<MonDTO> LocMonPhuHopBanHang(IEnumerable<MonDTO> dsMon, string? boLocLoaiMon)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View))
        {
            return new List<MonDTO>();
        }

        return dsMon
            .Where(x => x.TrangThai == 1)
            .Where(x => PhuHopBoLocLoaiMon(x.TenLoaiMon, boLocLoaiMon))
            .OrderBy(x => x.TenLoaiMon)
            .ThenBy(x => x.TenMon)
            .ToList();
    }

    public static string ChuyenTrangThaiBan(int trangThai)
    {
        return trangThai switch
        {
            1 => "Đang phục vụ",
            2 => "Đặt trước",
            _ => "Trống"
        };
    }

    public BanActionResultDTO GoiMon(int banId, IEnumerable<BanHangThemMonDTO> dsMonThem)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Create))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền gọi món.");
        }

        if (banId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn trước khi gọi món.");
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
            return BusMessageCatalog.CreateActionResult(false, "Chưa có món hợp lệ để gọi.");
        }

        return BusMessageCatalog.NormalizeActionResult(_banHangDAL.GoiMon(banId, dsMonTongHop));
    }

    public BanActionResultDTO ThanhToan(int banId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền thanh toán.");
        }

        if (banId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn trước khi thanh toán.");
        }

        return BusMessageCatalog.NormalizeActionResult(_banHangDAL.ThanhToan(banId));
    }

    private BanActionResultDTO DongBoMonTamTheoBan(int banId, bool yeuCauCoMonChoGoi)
    {
        if (banId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn hợp lệ.");
        }

        var gioTam = LayGioTamTheoBan(banId);
        if (gioTam.Count == 0)
        {
            return yeuCauCoMonChoGoi
                ? BusMessageCatalog.CreateActionResult(false, "Không có món mới để lưu bàn.")
                : BusMessageCatalog.CreateActionResult(true, string.Empty);
        }

        var result = GoiMon(
            banId,
            gioTam.Select(x => new BanHangThemMonDTO
            {
                MonID = x.MonID,
                SoLuong = x.SoLuong
            }));

        if (result.ThanhCong)
        {
            _gioTamTheoBan.Remove(banId);
        }

        return result;
    }

    private List<BanHangOrderItemDTO> LayGioTamTheoBan(int banId)
    {
        if (_gioTamTheoBan.TryGetValue(banId, out var gioTam))
        {
            return gioTam;
        }

        gioTam = new List<BanHangOrderItemDTO>();
        _gioTamTheoBan[banId] = gioTam;
        return gioTam;
    }

    private static List<BanHangOrderItemDTO> TongHopChiTiet(IEnumerable<BanHangOrderItemDTO> dsChiTiet)
    {
        return dsChiTiet
            .GroupBy(x => new { x.MonID, x.TenMon, x.DonGia })
            .Select(g => new BanHangOrderItemDTO
            {
                MonID = g.Key.MonID,
                TenMon = g.Key.TenMon,
                DonGia = g.Key.DonGia,
                SoLuong = (short)Math.Clamp(g.Sum(x => (int)x.SoLuong), 1, short.MaxValue)
            })
            .OrderBy(x => x.TenMon)
            .ToList();
    }

    private static bool PhuHopBoLocLoaiMon(string tenLoaiMon, string? boLocLoaiMon)
    {
        if (string.IsNullOrWhiteSpace(boLocLoaiMon))
        {
            return true;
        }

        var tenLoai = ChuanHoaKhongDau(tenLoaiMon);
        var boLoc = ChuanHoaKhongDau(boLocLoaiMon);
        return boLoc.Length == 0 || tenLoai == boLoc;
    }

    private static string ChuanHoaKhongDau(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .ToLowerInvariant();
    }
}