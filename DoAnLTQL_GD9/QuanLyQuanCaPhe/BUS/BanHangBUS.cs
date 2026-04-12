using System.Globalization;
using System.Text;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class BanHangBUS : IBanHangService
{
    private readonly IBanHangRepository _banHangDAL;
    private readonly IHoaDonRepository _hoaDonDAL;
    private readonly IOrderService _orderService;
    private readonly IPermissionService _permissionBUS;
    private readonly Dictionary<int, List<BanHangOrderItemDTO>> _gioTamTheoBan = new();

    public BanHangBUS(
        IBanHangRepository? banHangDAL = null,
        IHoaDonRepository? hoaDonDAL = null,
        IOrderService? orderService = null,
        IPermissionService? permissionBUS = null)
    {
        _banHangDAL = banHangDAL ?? new BanHangDAL();
        _hoaDonDAL = hoaDonDAL ?? new HoaDonDAL();
        _orderService = orderService ?? new OrderService();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

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
            HoaDonID = phieuDb.HoaDonID,
            TrangThaiHoaDon = phieuDb.TrangThaiHoaDon,
            KhachHangID = phieuDb.KhachHangID,
            TenKhachHang = string.IsNullOrWhiteSpace(phieuDb.TenKhachHang) ? "Khách lẻ" : phieuDb.TenKhachHang,
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

        var phieu = _banHangDAL.GetPhieuTheoBan(banId);
        if (phieu.BanID <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Không tìm thấy bàn đã chọn.");
        }

        if (LaBanDangChoDon(phieu))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bàn đã thanh toán và đang chờ dọn. Vui lòng dọn bàn trước khi nhận khách mới.");
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

    public BanActionResultDTO XoaMonKhoiBan(int banId, int monId, decimal donGia, short soLuong)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền xóa món khỏi order.");
        }

        if (banId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn hợp lệ.");
        }

        if (monId <= 0 || soLuong <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Món cần xóa không hợp lệ.");
        }

        var gioTam = LayGioTamTheoBan(banId);
        var soLuongTrongGioTam = gioTam
            .Where(x => x.MonID == monId && x.DonGia == donGia)
            .Sum(x => (int)x.SoLuong);

        var soLuongCanXoaKhoiHoaDon = Math.Max(0, soLuong - soLuongTrongGioTam);
        var soLuongCanXoaKhoiGioTam = Math.Min(soLuong, soLuongTrongGioTam);

        if (soLuongCanXoaKhoiHoaDon > 0)
        {
            var phieu = _banHangDAL.GetPhieuTheoBan(banId);
            if (!phieu.HoaDonID.HasValue || phieu.HoaDonID.Value <= 0)
            {
                return BusMessageCatalog.CreateActionResult(false, "Không tìm thấy hóa đơn mở để xóa món.");
            }

            var ketQuaXoaMonKhoiHoaDon = _orderService.RemoveItemFromOrder(
                phieu.HoaDonID.Value,
                monId,
                (short)Math.Clamp(soLuongCanXoaKhoiHoaDon, 1, short.MaxValue),
                phieu.HoaDonRowVersion);

            if (!ketQuaXoaMonKhoiHoaDon.ThanhCong)
            {
                return BusMessageCatalog.NormalizeActionResult(ketQuaXoaMonKhoiHoaDon);
            }
        }

        if (soLuongCanXoaKhoiGioTam > 0)
        {
            XoaMonKhoiGioTam(gioTam, monId, donGia, soLuongCanXoaKhoiGioTam);
            if (gioTam.Count == 0)
            {
                _gioTamTheoBan.Remove(banId);
            }
        }

        return BusMessageCatalog.CreateActionResult(true, "Đã xóa món khỏi order.");
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

    public BanActionResultDTO LuuMonChoGoiVoiKhachHang(int banId, int? khachHangId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền lưu bàn.");
        }

        return DongBoMonTamTheoBan(
            banId,
            yeuCauCoMonChoGoi: true,
            khachHangId: khachHangId,
            dongBoKhachHang: true);
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

    public BanActionResultDTO ThanhToanHoaDonVoiKhachHang(int banId, int? khachHangId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền thanh toán.");
        }

        if (banId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn trước khi thanh toán.");
        }

        var resultDongBo = DongBoMonTamTheoBan(
            banId,
            yeuCauCoMonChoGoi: false,
            khachHangId: khachHangId,
            dongBoKhachHang: true);
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

    public static string ChuyenTrangThaiBan(int trangThai, int? trangThaiHoaDon = null)
    {
        if (trangThaiHoaDon == (int)HoaDonTrangThai.Draft)
        {
            return "Có khách";
        }

        if (trangThaiHoaDon == (int)HoaDonTrangThai.Paid)
        {
            return "Đã thanh toán";
        }

        return trangThai switch
        {
            1 => "Có khách",
            2 => "Chờ dọn / Đã thanh toán",
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

        return GoiMonNoPermission(banId, dsMonThem);
    }

    private BanActionResultDTO GoiMonNoPermission(
        int banId,
        IEnumerable<BanHangThemMonDTO> dsMonThem,
        int? khachHangId = null,
        bool dongBoKhachHang = false)
    {
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

        var ketQuaDamBaoHoaDon = DamBaoHoaDonMoTheoBan(
            banId,
            khachHangId: khachHangId,
            dongBoKhachHang: dongBoKhachHang);
        if (!ketQuaDamBaoHoaDon.Result.ThanhCong)
        {
            return ketQuaDamBaoHoaDon.Result;
        }

        return BusMessageCatalog.NormalizeActionResult(
            _orderService.AddItemsToOrder(
                ketQuaDamBaoHoaDon.HoaDonId,
                dsMonTongHop,
                successMessage: "Gọi món thành công.",
                expectedRowVersion: ketQuaDamBaoHoaDon.RowVersion));
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

        var phieu = _banHangDAL.GetPhieuTheoBan(banId);

        if (LaBanDangChoDon(phieu))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bàn đang chờ dọn, không thể nhận thêm thao tác thanh toán.");
        }

        if (phieu.TrangThaiHoaDon == (int)HoaDonTrangThai.Paid)
        {
            return BusMessageCatalog.CreateActionResult(false, "Hóa đơn của bàn này đã thanh toán. Vui lòng dọn bàn trước khi nhận khách mới.");
        }

        if (!phieu.HoaDonID.HasValue)
        {
            return BusMessageCatalog.CreateActionResult(false, "Bàn này chưa có hóa đơn mở.");
        }

        return BusMessageCatalog.NormalizeActionResult(_orderService.Checkout(phieu.HoaDonID.Value, phieu.HoaDonRowVersion));
    }

    private (BanActionResultDTO Result, int HoaDonId, byte[]? RowVersion) DamBaoHoaDonMoTheoBan(
        int banId,
        int? khachHangId = null,
        bool dongBoKhachHang = false)
    {
        var phieu = _banHangDAL.GetPhieuTheoBan(banId);

        if (phieu.BanID <= 0)
        {
            return (BusMessageCatalog.CreateActionResult(false, "Không tìm thấy bàn đã chọn."), 0, null);
        }

        if (LaBanDangChoDon(phieu))
        {
            return (BusMessageCatalog.CreateActionResult(false, "Bàn đã thanh toán và đang chờ dọn. Vui lòng dọn bàn trước khi mở hóa đơn mới."), 0, null);
        }

        if (phieu.HoaDonID.HasValue && phieu.HoaDonID.Value > 0)
        {
            if (phieu.TrangThaiHoaDon != (int)HoaDonTrangThai.Draft)
            {
                return (BusMessageCatalog.CreateActionResult(false, "Bàn đã thanh toán và đang chờ dọn. Vui lòng dọn bàn trước khi mở hóa đơn mới."), 0, null);
            }

            if (dongBoKhachHang)
            {
                var ketQuaCapNhatKhach = _hoaDonDAL.CapNhatKhachHangChoHoaDonMo(phieu.HoaDonID.Value, khachHangId);
                if (!ketQuaCapNhatKhach.ThanhCong)
                {
                    return (BusMessageCatalog.CreateActionResult(false, ketQuaCapNhatKhach.ThongBao), 0, null);
                }

                var phieuSauCapNhat = _banHangDAL.GetPhieuTheoBan(banId);
                if (!phieuSauCapNhat.HoaDonID.HasValue)
                {
                    return (BusMessageCatalog.CreateActionResult(false, "Không tìm thấy hóa đơn mở sau khi cập nhật khách hàng."), 0, null);
                }

                return (BusMessageCatalog.CreateActionResult(true, string.Empty), phieuSauCapNhat.HoaDonID.Value, phieuSauCapNhat.HoaDonRowVersion);
            }

            return (BusMessageCatalog.CreateActionResult(true, string.Empty), phieu.HoaDonID.Value, phieu.HoaDonRowVersion);
        }

        var ketQuaTaoHoaDon = _hoaDonDAL.ThemHoaDon(new HoaDonSaveRequestDTO
        {
            BanID = banId,
            NgayLap = DateTime.Now,
            TrangThai = 0,
            KhachHangID = dongBoKhachHang ? khachHangId : null
        });

        if (!ketQuaTaoHoaDon.ThanhCong)
        {
            return (BusMessageCatalog.CreateActionResult(false, ketQuaTaoHoaDon.ThongBao), 0, null);
        }

        return (BusMessageCatalog.CreateActionResult(true, string.Empty), ketQuaTaoHoaDon.HoaDonId, null);
    }

    private BanActionResultDTO DongBoMonTamTheoBan(
        int banId,
        bool yeuCauCoMonChoGoi,
        int? khachHangId = null,
        bool dongBoKhachHang = false)
    {
        if (banId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn hợp lệ.");
        }

        var phieu = _banHangDAL.GetPhieuTheoBan(banId);
        if (phieu.BanID <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Không tìm thấy bàn đã chọn.");
        }

        if (LaBanDangChoDon(phieu))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bàn đang chờ dọn, vui lòng dọn bàn trước khi nhận khách mới.");
        }

        var gioTam = LayGioTamTheoBan(banId);
        if (gioTam.Count == 0)
        {
            if (yeuCauCoMonChoGoi)
            {
                return BusMessageCatalog.CreateActionResult(false, "Không có món mới để lưu bàn.");
            }

            return dongBoKhachHang
                ? DongBoKhachHangHoaDonMoTheoBan(banId, khachHangId)
                : BusMessageCatalog.CreateActionResult(true, string.Empty);
        }

        var result = GoiMonNoPermission(
            banId,
            gioTam.Select(x => new BanHangThemMonDTO
            {
                MonID = x.MonID,
                SoLuong = x.SoLuong
            }),
            khachHangId: khachHangId,
            dongBoKhachHang: dongBoKhachHang);

        if (result.ThanhCong)
        {
            _gioTamTheoBan.Remove(banId);
        }

        return result;
    }

    private BanActionResultDTO DongBoKhachHangHoaDonMoTheoBan(int banId, int? khachHangId)
    {
        var phieu = _banHangDAL.GetPhieuTheoBan(banId);
        if (!phieu.HoaDonID.HasValue || phieu.HoaDonID.Value <= 0)
        {
            return BusMessageCatalog.CreateActionResult(true, string.Empty);
        }

        var ketQuaCapNhatKhach = _hoaDonDAL.CapNhatKhachHangChoHoaDonMo(phieu.HoaDonID.Value, khachHangId);
        return ketQuaCapNhatKhach.ThanhCong
            ? BusMessageCatalog.CreateActionResult(true, string.Empty)
            : BusMessageCatalog.CreateActionResult(false, ketQuaCapNhatKhach.ThongBao);
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

    private static void XoaMonKhoiGioTam(List<BanHangOrderItemDTO> gioTam, int monId, decimal donGia, int soLuongCanXoa)
    {
        if (soLuongCanXoa <= 0)
        {
            return;
        }

        var dsDongMon = gioTam
            .Where(x => x.MonID == monId && x.DonGia == donGia)
            .OrderByDescending(x => x.SoLuong)
            .ToList();

        foreach (var dongMon in dsDongMon)
        {
            if (soLuongCanXoa <= 0)
            {
                break;
            }

            if (dongMon.SoLuong <= soLuongCanXoa)
            {
                soLuongCanXoa -= dongMon.SoLuong;
                gioTam.Remove(dongMon);
                continue;
            }

            dongMon.SoLuong = (short)Math.Clamp(dongMon.SoLuong - soLuongCanXoa, 1, short.MaxValue);
            soLuongCanXoa = 0;
        }
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

    private static bool LaBanDangChoDon(BanHangPhieuDTO phieu)
    {
        return phieu.BanID > 0
            && ((phieu.HoaDonID.HasValue
                    && phieu.TrangThaiHoaDon == (int)HoaDonTrangThai.Paid)
                || (phieu.TrangThaiBan == 2 && !phieu.HoaDonID.HasValue));
    }
}