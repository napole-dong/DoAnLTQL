using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class PermissionBUS
{
    private readonly PermissionDAL _permissionDAL = new();

    public bool CheckPermission(string feature, string action)
    {
        var nguoiDungDangNhap = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        if (nguoiDungDangNhap == null)
        {
            return false;
        }

        return _permissionDAL.CheckPermission(nguoiDungDangNhap.RoleId, feature, action);
    }

    public List<string> LayDanhSachVaiTroCoTheGan()
    {
        var nguoiDungDangNhap = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        if (nguoiDungDangNhap == null)
        {
            return new List<string>();
        }

        return _permissionDAL.LayDanhSachVaiTroCoTheGan(nguoiDungDangNhap.RoleId);
    }

    public bool CoTheGanVaiTro(string tenVaiTro)
    {
        var nguoiDungDangNhap = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        if (nguoiDungDangNhap == null)
        {
            return false;
        }

        return _permissionDAL.CoTheGanVaiTro(nguoiDungDangNhap.RoleId, tenVaiTro);
    }
}
