using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.Auth;

public sealed class UserSession
{
    public UserSession(int userId, Role role)
    {
        if (userId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userId), "UserId phai lon hon 0.");
        }

        UserId = userId;
        Role = role;
    }

    public int UserId { get; }
    public Role Role { get; }

    public static UserSession? FromLoginInfo(ThongTinDangNhapDTO? loginInfo)
    {
        if (loginInfo == null || loginInfo.UserId <= 0)
        {
            return null;
        }

        var role = RoleMapper.Parse(loginInfo.QuyenHan, Role.Staff);
        return new UserSession(loginInfo.UserId, role);
    }
}
