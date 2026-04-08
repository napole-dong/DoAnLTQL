using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class BanBUS
{
    private readonly BanDAL _banDAL = new();
    private readonly PermissionBUS _permissionBUS = new();

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
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền chuyển hoặc gộp bàn.");
        }

        if (request.BanNguonId <= 0 || request.BanDichId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Thông tin bàn không hợp lệ.");
        }

        if (request.BanNguonId == request.BanDichId)
        {
            return BusMessageCatalog.CreateActionResult(false, "Bàn nguồn và bàn đích không được trùng nhau.");
        }

        return BusMessageCatalog.NormalizeActionResult(_banDAL.ChuyenHoacGopBan(request));
    }
}
