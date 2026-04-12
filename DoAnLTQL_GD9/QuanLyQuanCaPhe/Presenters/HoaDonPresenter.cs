using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Presenters;

public enum HoaDonInputField
{
    None,
    BanKhach,
    HoaDon,
    TienKhachDua,
    Permission
}

public sealed class HoaDonValidationResult
{
    public bool HopLe { get; private init; }
    public string? ThongBao { get; private init; }
    public HoaDonInputField TruongLoi { get; private init; }
    public int? BanId { get; private init; }

    public static HoaDonValidationResult ThanhCong(int? banId = null)
    {
        return new HoaDonValidationResult
        {
            HopLe = true,
            TruongLoi = HoaDonInputField.None,
            BanId = banId
        };
    }

    public static HoaDonValidationResult ThatBai(string thongBao, HoaDonInputField truongLoi)
    {
        return new HoaDonValidationResult
        {
            HopLe = false,
            ThongBao = thongBao,
            TruongLoi = truongLoi
        };
    }
}

public sealed class HoaDonPresenter
{
    private readonly string _thongBaoXungDot;

    public HoaDonPresenter(string thongBaoXungDot)
    {
        _thongBaoXungDot = thongBaoXungDot;
    }

    public HoaDonValidationResult ValidateSaveRequest(object? banKhachSelectedValue, bool laCapNhat, byte[]? rowVersion)
    {
        if (banKhachSelectedValue is not int banId || banId <= 0)
        {
            return HoaDonValidationResult.ThatBai("Vui lòng chọn bàn hợp lệ.", HoaDonInputField.BanKhach);
        }

        if (laCapNhat && (rowVersion == null || rowVersion.Length == 0))
        {
            return HoaDonValidationResult.ThatBai(_thongBaoXungDot, HoaDonInputField.HoaDon);
        }

        return HoaDonValidationResult.ThanhCong(banId);
    }

    public HoaDonValidationResult ValidateCheckout(
        bool coQuyenThuTien,
        HoaDonDTO? hoaDon,
        decimal tongTien,
        decimal tienKhachDua)
    {
        if (!coQuyenThuTien)
        {
            return HoaDonValidationResult.ThatBai("Bạn không có quyền xác nhận thu tiền hóa đơn.", HoaDonInputField.Permission);
        }

        if (hoaDon == null)
        {
            return HoaDonValidationResult.ThatBai("Vui lòng chọn hóa đơn cần thu tiền.", HoaDonInputField.HoaDon);
        }

        if (hoaDon.TrangThai != (int)HoaDonTrangThai.ChuaThanhToan)
        {
            return HoaDonValidationResult.ThatBai("Hóa đơn này không còn ở trạng thái chờ thanh toán.", HoaDonInputField.HoaDon);
        }

        if (tongTien <= 0)
        {
            return HoaDonValidationResult.ThatBai("Hóa đơn chưa có món, không thể xác nhận thu tiền.", HoaDonInputField.HoaDon);
        }

        if (tienKhachDua < tongTien)
        {
            return HoaDonValidationResult.ThatBai("Tiền khách đưa chưa đủ để thanh toán hóa đơn.", HoaDonInputField.TienKhachDua);
        }

        return HoaDonValidationResult.ThanhCong();
    }

    public bool IsConcurrencyConflict(string? thongBao)
    {
        return string.Equals(thongBao?.Trim(), _thongBaoXungDot, StringComparison.Ordinal);
    }
}
