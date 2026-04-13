using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class TaiKhoanBUS : ITaiKhoanService
{
    private readonly ITaiKhoanRepository _taiKhoanDAL;
    private readonly IPermissionService _permissionBUS;

    public TaiKhoanBUS(ITaiKhoanRepository? taiKhoanDAL = null, IPermissionService? permissionBUS = null)
    {
        _taiKhoanDAL = taiKhoanDAL ?? new TaiKhoanDAL();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public (bool ThanhCong, string ThongBao) DeleteUser(int userId)
    {
        if (!_permissionBUS.IsAdmin())
        {
            return (false, "Bạn không có quyền xóa tài khoản.");
        }

        if (userId <= 0)
        {
            return (false, "Mã tài khoản không hợp lệ.");
        }

        var ketQua = _taiKhoanDAL.DeleteUser(userId);
        return (ketQua.ThanhCong, ketQua.ThongBao);
    }
}
