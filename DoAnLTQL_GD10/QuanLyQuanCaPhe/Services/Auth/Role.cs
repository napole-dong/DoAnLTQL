using System.Globalization;
using System.Text;

namespace QuanLyQuanCaPhe.Services.Auth;

public enum RoleEnum
{
    Staff = 0,
    Manager = 1,
    Admin = 2
}

public enum Role
{
    Staff = (int)RoleEnum.Staff,
    Manager = (int)RoleEnum.Manager,
    Admin = (int)RoleEnum.Admin
}

public static class RoleMapper
{
    private static readonly IReadOnlyDictionary<string, RoleEnum> NameMap = new Dictionary<string, RoleEnum>(StringComparer.Ordinal)
    {
        ["ADMIN"] = RoleEnum.Admin,
        ["MANAGER"] = RoleEnum.Manager,
        ["QUANLY"] = RoleEnum.Manager,
        ["STAFF"] = RoleEnum.Staff,
        ["NHANVIEN"] = RoleEnum.Staff
    };

    private static readonly IReadOnlyDictionary<RoleEnum, string> DefaultNameMap = new Dictionary<RoleEnum, string>
    {
        [RoleEnum.Admin] = "Admin",
        [RoleEnum.Manager] = "Manager",
        [RoleEnum.Staff] = "Staff"
    };

    public static RoleEnum ParseRoleEnum(string? roleName, RoleEnum fallback = RoleEnum.Staff)
    {
        return TryParseRoleEnum(roleName, out var role) ? role : fallback;
    }

    public static bool TryParseRoleEnum(string? roleName, out RoleEnum role)
    {
        var key = NormalizeRoleKey(roleName);
        if (key.Length == 0)
        {
            role = RoleEnum.Staff;
            return false;
        }

        return NameMap.TryGetValue(key, out role);
    }

    public static string ToRoleName(RoleEnum role)
    {
        return DefaultNameMap.TryGetValue(role, out var roleName)
            ? roleName
            : DefaultNameMap[RoleEnum.Staff];
    }

    public static Role Parse(string? roleName, Role fallback = Role.Staff)
    {
        var fallbackEnum = (RoleEnum)fallback;
        return (Role)ParseRoleEnum(roleName, fallbackEnum);
    }

    public static bool TryParse(string? roleName, out Role role)
    {
        var success = TryParseRoleEnum(roleName, out var roleEnum);
        role = (Role)roleEnum;
        return success;
    }

    public static string ToRoleName(Role role)
    {
        return ToRoleName((RoleEnum)role);
    }

    private static string NormalizeRoleKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var normalized = value.Trim().ToUpperInvariant().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (char.IsLetterOrDigit(c))
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }
}