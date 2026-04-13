using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.Auth;

public sealed class Session
{
    public Session(int userId, string username, RoleEnum role)
    {
        UserId = userId;
        Username = string.IsNullOrWhiteSpace(username) ? string.Empty : username.Trim();
        Role = role;
    }

    public Session(int userId, string username, Role role)
        : this(userId, username, (RoleEnum)role)
    {
    }

    public int UserId { get; }
    public string Username { get; }
    public RoleEnum Role { get; }

    public static Session? FromLoginInfo(ThongTinDangNhapDTO? loginInfo)
    {
        if (loginInfo == null || loginInfo.UserId <= 0)
        {
            return null;
        }

        return new Session(
            loginInfo.UserId,
            loginInfo.TenDangNhap,
            RoleMapper.ParseRoleEnum(loginInfo.QuyenHan));
    }
}