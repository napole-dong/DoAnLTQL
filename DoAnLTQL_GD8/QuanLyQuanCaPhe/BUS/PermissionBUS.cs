using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class PermissionBUS
{
    private readonly PermissionDAL _permissionDAL = new();
    private static readonly AuthorizationPolicyOptions AuthorizationPolicy = new()
    {
        AllowManagerEditMenuPrice = true
    };

    public bool CheckPermission(ActionType action, Feature feature)
    {
        return CheckPermission(PermissionFeatures.ToFeatureKey(feature), PermissionActions.ToActionKey(action));
    }

    public bool DongBoDuLieuQuyenMacDinh()
    {
        return _permissionDAL.DongBoDuLieuQuyenMacDinh();
    }

    public bool HasPermission(ActionType action, Feature feature)
    {
        return CheckPermission(action, feature);
    }

    public bool CheckPermission(string feature, string action)
    {
        if (!PermissionFeatures.IsKnownFeature(feature) || !PermissionActions.IsKnownAction(action))
        {
            return false;
        }

        var permission = TaoPermission();
        if (permission == null)
        {
            return false;
        }

        return permission.HasPermission(feature, action);
    }

    public void EnsurePermission(ActionType action, Feature feature, string? message = null)
    {
        EnsurePermission(PermissionFeatures.ToFeatureKey(feature), PermissionActions.ToActionKey(action), message);
    }

    public void EnsurePermission(string feature, string action, string? message = null)
    {
        if (!PermissionFeatures.IsKnownFeature(feature) || !PermissionActions.IsKnownAction(action))
        {
            throw new UnauthorizedAccessException(message ?? "Yeu cau quyen khong hop le.");
        }

        var permission = TaoPermissionBatBuocDangNhap(message);

        permission.EnsurePermission(feature, action, message);
    }

    public bool IsAdmin()
    {
        return Permission.IsAdminSession(NguoiDungHienTaiService.LaySession());
    }

    public bool IsManager()
    {
        return Permission.IsManagerSession(NguoiDungHienTaiService.LaySession());
    }

    public bool IsStaff()
    {
        return Permission.IsStaffSession(NguoiDungHienTaiService.LaySession());
    }

    public UserSession? LayUserSession()
    {
        return NguoiDungHienTaiService.LayUserSession();
    }

    public bool CanManageEmployees()
    {
        var authorization = TaoAuthorizationService();
        return authorization != null && authorization.CanManageEmployees();
    }

    public bool CanEditMenuPrice()
    {
        var authorization = TaoAuthorizationService();
        return authorization != null && authorization.CanEditMenuPrice();
    }

    public bool CanDeleteInvoice()
    {
        var authorization = TaoAuthorizationService();
        return authorization != null && authorization.CanDeleteInvoice();
    }

    public bool CanTransferTable()
    {
        var authorization = TaoAuthorizationService();
        return authorization != null && authorization.CanTransferTable();
    }

    public bool CanMergeTable()
    {
        var authorization = TaoAuthorizationService();
        return authorization != null && authorization.CanMergeTable();
    }

    public bool CanManageUser()
    {
        var permission = TaoPermission();
        return permission != null && permission.CanManageUser() && CanManageEmployees();
    }

    public bool CanManageMenu()
    {
        var permission = TaoPermission();
        return permission != null && permission.CanManageMenu();
    }

    public bool CanManageInventory()
    {
        var permission = TaoPermission();
        return permission != null && permission.CanManageInventory();
    }

    public bool CanViewReport()
    {
        var permission = TaoPermission();
        return permission != null && permission.CanViewReport();
    }

    public bool CanSell()
    {
        var permission = TaoPermission();
        return permission != null && permission.CanSell();
    }

    public List<string> LayDanhSachVaiTroCoTheGan()
    {
        if (IsAdmin())
        {
            return _permissionDAL.LayDanhSachTenVaiTro();
        }

        var roleId = LayRoleIdDangNhap();
        if (roleId <= 0)
        {
            return new List<string>();
        }

        return _permissionDAL.LayDanhSachVaiTroCoTheGan(roleId);
    }

    public bool CoTheGanVaiTro(string tenVaiTro)
    {
        if (string.IsNullOrWhiteSpace(tenVaiTro))
        {
            return false;
        }

        if (IsAdmin())
        {
            return true;
        }

        var roleId = LayRoleIdDangNhap();
        if (roleId <= 0)
        {
            return false;
        }

        return _permissionDAL.CoTheGanVaiTro(roleId, tenVaiTro);
    }

    private static int LayRoleIdDangNhap()
    {
        return NguoiDungHienTaiService.LayNguoiDungDangNhap()?.RoleId ?? 0;
    }

    private Permission? TaoPermission()
    {
        var session = NguoiDungHienTaiService.LaySession();
        if (session == null)
        {
            return null;
        }

        var roleId = LayRoleIdDangNhap();
        return new Permission(
            session,
            (featureKey, actionKey) => roleId > 0 && _permissionDAL.CheckPermission(roleId, featureKey, actionKey));
    }

    private static AuthorizationService? TaoAuthorizationService()
    {
        return AuthorizationService.TryCreateForCurrentUser(AuthorizationPolicy);
    }

    private Permission TaoPermissionBatBuocDangNhap(string? message)
    {
        var permission = TaoPermission();
        if (permission != null)
        {
            return permission;
        }

        throw new UnauthorizedAccessException(message ?? "Ban chua dang nhap vao he thong.");
    }
}
