namespace QuanLyQuanCaPhe.Services.Auth;

public static class RoleConstants
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Staff = "Staff";

    public static bool CoQuyenXacNhanThuTien(string? roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return false;
        }

        return string.Equals(roleName, Admin, StringComparison.OrdinalIgnoreCase)
            || string.Equals(roleName, Manager, StringComparison.OrdinalIgnoreCase)
            || string.Equals(roleName, Staff, StringComparison.OrdinalIgnoreCase);
    }
}