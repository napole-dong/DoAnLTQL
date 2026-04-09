namespace QuanLyQuanCaPhe.Services.Auth;

public enum Feature
{
    User,
    Employee,
    Menu,
    Inventory,
    Invoice,
    Report,
    Sell
}

public static class PermissionFeatures
{
    public const string NhanVien = "NhanVien";
    public const string BanHang = "BanHang";
    public const string Menu = "Menu";
    public const string NguyenLieu = "NguyenLieu";
    public const string HoaDon = "HoaDon";
    public const string ThongKe = "ThongKe";
    public const string TaiKhoan = "TaiKhoan";

    public static readonly string[] TatCa =
    {
        NhanVien,
        BanHang,
        Menu,
        NguyenLieu,
        HoaDon,
        ThongKe,
        TaiKhoan
    };

    private static readonly HashSet<string> TatCaDaChuanHoa = new(
        TatCa.Select(NormalizeFeature),
        StringComparer.OrdinalIgnoreCase);

    public static string ToFeatureKey(Feature feature)
    {
        return feature switch
        {
            Feature.User => TaiKhoan,
            Feature.Employee => NhanVien,
            Feature.Menu => Menu,
            Feature.Inventory => NguyenLieu,
            Feature.Invoice => HoaDon,
            Feature.Report => ThongKe,
            Feature.Sell => BanHang,
            _ => string.Empty
        };
    }

    public static string NormalizeFeature(string? feature)
    {
        if (string.IsNullOrWhiteSpace(feature))
        {
            return string.Empty;
        }

        return feature.Trim();
    }

    public static bool IsKnownFeature(string? feature)
    {
        var featureKey = NormalizeFeature(feature);
        if (featureKey.Length == 0)
        {
            return false;
        }

        return TatCaDaChuanHoa.Contains(featureKey);
    }

    public static bool TryParseFeature(string? feature, out Feature parsedFeature)
    {
        var featureKey = NormalizeFeature(feature).ToUpperInvariant();
        parsedFeature = featureKey switch
        {
            "TAIKHOAN" => Feature.User,
            "NHANVIEN" => Feature.Employee,
            "MENU" => Feature.Menu,
            "NGUYENLIEU" => Feature.Inventory,
            "HOADON" => Feature.Invoice,
            "THONGKE" => Feature.Report,
            "BANHANG" => Feature.Sell,
            _ => Feature.Menu
        };

        return IsKnownFeature(featureKey);
    }
}
