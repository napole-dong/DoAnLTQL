namespace QuanLyQuanCaPhe.Services.Auth;

public static class MatKhauService
{
    private const int WorkFactor = 12;
    private const int MinPasswordLength = 3;

    public static string BamMatKhau(string matKhau)
    {
        return BCrypt.Net.BCrypt.HashPassword(matKhau, workFactor: WorkFactor);
    }

    public static bool KiemTraMatKhau(string matKhauNhap, string matKhauDaLuu)
    {
        if (string.IsNullOrWhiteSpace(matKhauNhap) || string.IsNullOrWhiteSpace(matKhauDaLuu))
        {
            return false;
        }

        if (LaBcryptHash(matKhauDaLuu))
        {
            return BCrypt.Net.BCrypt.Verify(matKhauNhap, matKhauDaLuu);
        }

        // Ho tro du lieu mat khau cu dang luu plain text.
        return string.Equals(matKhauNhap, matKhauDaLuu, StringComparison.Ordinal);
    }

    public static bool CanNangCapHash(string matKhauDaLuu)
    {
        return !LaBcryptHash(matKhauDaLuu);
    }

    public static string BamMatKhauNeuCan(string matKhau)
    {
        return LaBcryptHash(matKhau) ? matKhau : BamMatKhau(matKhau);
    }

    public static bool DatYeuCauDoManh(string matKhau, out string thongBaoLoi)
    {
        thongBaoLoi = string.Empty;

        if (string.IsNullOrWhiteSpace(matKhau))
        {
            thongBaoLoi = "Mat khau khong duoc de trong.";
            return false;
        }
        return true;
    }

    private static bool LaBcryptHash(string value)
    {
        return value.StartsWith("$2a$", StringComparison.Ordinal)
            || value.StartsWith("$2b$", StringComparison.Ordinal)
            || value.StartsWith("$2y$", StringComparison.Ordinal);
    }
}
