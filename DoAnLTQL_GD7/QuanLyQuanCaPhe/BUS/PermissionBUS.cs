using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class PermissionBUS
{
    private readonly PermissionDAL _permissionDAL = new();

    public bool CheckPermission(string feature, string action)
    {
        var roleId = LayRoleIdDangNhap();
        if (roleId <= 0)
        {
            return false;
        }

        return _permissionDAL.CheckPermission(roleId, feature, action);
    }

    public List<string> LayDanhSachVaiTroCoTheGan()
    {
        var roleId = LayRoleIdDangNhap();
        if (roleId <= 0)
        {
            return new List<string>();
        }

        return _permissionDAL.LayDanhSachVaiTroCoTheGan(roleId);
    }

    public bool CoTheGanVaiTro(string tenVaiTro)
    {
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
}
