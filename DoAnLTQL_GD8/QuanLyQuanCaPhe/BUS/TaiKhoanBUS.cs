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

        var daXoa = _taiKhoanDAL.DeleteUser(userId);
        return daXoa
            ? (true, "Xóa tài khoản thành công.")
            : (false, "Không tìm thấy tài khoản cần xóa.");
    }
}
