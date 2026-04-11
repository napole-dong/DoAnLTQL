using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class TaiKhoanBUS
{
    private readonly TaiKhoanDAL _taiKhoanDAL = new();
    private readonly PermissionBUS _permissionBUS = new();

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
