using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class TaiKhoanBUS
{
    private readonly TaiKhoanDAL _taiKhoanDAL = new();
    private readonly PermissionBUS _permissionBUS = new();

    public void DeleteUser(int userId)
    {
        if (!_permissionBUS.IsAdmin())
        {
            throw new Exception("Không có quyền");
        }

        if (userId <= 0)
        {
            throw new Exception("Mã tài khoản không hợp lệ.");
        }

        var daXoa = _taiKhoanDAL.DeleteUser(userId);
        if (!daXoa)
        {
            throw new Exception("Không tìm thấy tài khoản cần xóa.");
        }
    }
}
