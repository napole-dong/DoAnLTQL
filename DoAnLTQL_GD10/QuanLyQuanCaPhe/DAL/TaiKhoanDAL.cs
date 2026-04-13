using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;

namespace QuanLyQuanCaPhe.DAL;

public class TaiKhoanDAL : ITaiKhoanRepository
{
    private readonly IActivityLogWriter _activityLogWriter;

    public TaiKhoanDAL(IActivityLogWriter? activityLogWriter = null)
    {
        _activityLogWriter = AppServiceProvider.Resolve(activityLogWriter, () => new ActivityLogService());
    }

    public sealed record DeleteUserResult(bool ThanhCong, string ThongBao);

    public DeleteUserResult DeleteUser(int userId)
    {
        if (userId <= 0)
        {
            return new DeleteUserResult(false, "Mã tài khoản không hợp lệ.");
        }

        var currentSession = NguoiDungHienTaiService.LaySession();
        if (currentSession == null || currentSession.UserId <= 0)
        {
            return new DeleteUserResult(false, "Phiên đăng nhập không hợp lệ. Vui lòng đăng nhập lại.");
        }

        using var context = new CaPheDbContext();
        var user = context.User
            .Include(x => x.VaiTro)
            .FirstOrDefault(x => x.ID == userId);

        if (user == null)
        {
            return new DeleteUserResult(false, "Không tìm thấy tài khoản cần xóa.");
        }

        if (user.ID == currentSession.UserId)
        {
            return new DeleteUserResult(false, "Không thể xóa chính tài khoản đang đăng nhập.");
        }

        if (LaTaiKhoanAdmin(user))
        {
            return new DeleteUserResult(false, "Không được phép xóa hoặc khóa tài khoản Admin.");
        }

        if (!user.HoatDong)
        {
            return new DeleteUserResult(false, "Tài khoản này đã bị khóa trước đó.");
        }

        var oldSnapshot = new
        {
            user.ID,
            user.TenDangNhap,
            user.VaiTroID,
            VaiTro = user.VaiTro?.TenVaiTro,
            HoatDong = user.HoatDong
        };

        // Soft delete: giữ lịch sử và tránh làm đứt dữ liệu liên quan.
        user.HoatDong = false;
        context.SaveChanges();

        var actor = currentSession.Username;
        _activityLogWriter.Log(
            userId: currentSession.UserId,
            action: AuditActions.DeleteUser,
            entity: "User",
            entityId: user.ID.ToString(),
            description: $"Tài khoản {user.TenDangNhap} đã bị khóa bởi {actor}.",
            oldValue: oldSnapshot,
            newValue: new
            {
                user.ID,
                user.TenDangNhap,
                user.VaiTroID,
                VaiTro = user.VaiTro?.TenVaiTro,
                HoatDong = user.HoatDong
            },
            performedBy: actor);

        return new DeleteUserResult(true, "Khóa tài khoản thành công.");
    }

    private static bool LaTaiKhoanAdmin(dtaUser user)
    {
        var role = RoleMapper.ParseRoleEnum(user.VaiTro?.TenVaiTro, RoleEnum.Staff);
        return role == RoleEnum.Admin;
    }
}
