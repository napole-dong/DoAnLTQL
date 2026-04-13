using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class BanBUS : IBanService
{
    private readonly IBanRepository _banDAL;
    private readonly IPermissionService _permissionBUS;

    public BanBUS(IBanRepository? banDAL = null, IPermissionService? permissionBUS = null)
    {
        _banDAL = banDAL ?? new BanDAL();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public BanThongKeDTO LayThongKe()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new BanThongKeDTO();
        }

        return _banDAL.GetThongKe();
    }

    public List<BanDTO> LayDanhSachBan(string? khuVuc, string? trangThai, string? tuKhoa)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new List<BanDTO>();
        }

        return _banDAL.GetDanhSachBan(khuVuc, trangThai, BusInputHelper.NormalizeNullableText(tuKhoa));
    }

    public List<BanDTO> LaySoDoBan()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new List<BanDTO>();
        }

        return _banDAL.GetSoDoBan();
    }

    public int LayHoacTaoBanMangDi()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View))
        {
            return 0;
        }

        return _banDAL.LayHoacTaoBanMangDi();
    }

    public BanActionResultDTO ThemBan(string tenBan)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền thêm bàn.");
        }

        var tenBanChuan = BusInputHelper.NormalizeText(tenBan);
        if (tenBanChuan.Length == 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Tên bàn không được để trống.");
        }

        if (_banDAL.TenBanDaTonTai(tenBanChuan))
        {
            return BusMessageCatalog.CreateActionResult(false, "Tên bàn đã tồn tại.");
        }

        _banDAL.ThemBan(tenBanChuan);
        return BusMessageCatalog.CreateActionResult(true, "Thêm bàn thành công.");
    }

    public BanDTO? LayBanTheoId(int banId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return null;
        }

        return _banDAL.GetBanById(banId);
    }

    public BanActionResultDTO XoaBan(int banId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Delete))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền xóa bàn.");
        }

        var ban = _banDAL.GetBanById(banId);
        if (ban == null)
        {
            return BusMessageCatalog.CreateActionResult(false, "Không tìm thấy bàn cần xóa.");
        }

        if (ban.TrangThai != 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Bàn đang được sử dụng, không thể xóa.");
        }

        if (_banDAL.BanDaPhatSinhHoaDon(banId))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bàn đã phát sinh hóa đơn, không thể xóa.");
        }

        var daXoa = _banDAL.XoaBan(banId);
        return daXoa
            ? BusMessageCatalog.CreateActionResult(true, "Xóa bàn thành công.")
            : BusMessageCatalog.CreateActionResult(false, "Không thể xóa bàn.");
    }

    public BanActionResultDTO DonBan(int banId)
    {
        if (banId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Bàn cần dọn không hợp lệ.");
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền dọn bàn.");
        }

        return BusMessageCatalog.NormalizeActionResult(_banDAL.DonBan(banId));
    }

    public List<BanDTO> LayDanhSachBanDich(int banNguonId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new List<BanDTO>();
        }

        return _banDAL.GetDanhSachBanDich(banNguonId);
    }

    public BanActionResultDTO ChuyenHoacGopBan(BanChuyenGopRequestDTO request)
    {
        if (request.BanNguonId <= 0 || request.BanDichId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Thông tin bàn không hợp lệ.");
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền thao tác bàn.");
        }

        if (request.BanNguonId == request.BanDichId)
        {
            return BusMessageCatalog.CreateActionResult(false, "Bàn nguồn và bàn đích không được trùng nhau.");
        }

        if (request.LaChuyenBan && !_permissionBUS.CanTransferTable())
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền chuyển bàn.");
        }

        if (!request.LaChuyenBan && !_permissionBUS.CanMergeTable())
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền gộp bàn.");
        }

        return BusMessageCatalog.NormalizeActionResult(_banDAL.ChuyenHoacGopBan(request));
    }
}
