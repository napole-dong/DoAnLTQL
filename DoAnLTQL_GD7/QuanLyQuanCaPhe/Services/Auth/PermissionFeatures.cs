namespace QuanLyQuanCaPhe.Services.Auth;

public static class PermissionFeatures
{
    public const string NhanVien = "NhanVien";
    public const string BanHang = "BanHang";
    public const string Menu = "Menu";
    public const string ThongKe = "ThongKe";

    public static readonly string[] TatCa =
    {
        NhanVien,
        BanHang,
        Menu,
        ThongKe
    };
}
