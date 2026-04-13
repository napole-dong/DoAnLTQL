using System.Globalization;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Presenters;

public enum CongThucInputField
{
    None,
    Mon,
    NguyenLieu,
    DinhLuong
}

public sealed class CongThucValidationResult
{
    public bool HopLe { get; private init; }
    public string? ThongBao { get; private init; }
    public CongThucInputField TruongLoi { get; private init; }
    public int? MonId { get; private init; }
    public int? NguyenLieuId { get; private init; }
    public decimal? SoLuong { get; private init; }

    public static CongThucValidationResult ThanhCong(int? monId = null, int? nguyenLieuId = null, decimal? soLuong = null)
    {
        return new CongThucValidationResult
        {
            HopLe = true,
            TruongLoi = CongThucInputField.None,
            MonId = monId,
            NguyenLieuId = nguyenLieuId,
            SoLuong = soLuong
        };
    }

    public static CongThucValidationResult ThatBai(string thongBao, CongThucInputField truongLoi)
    {
        return new CongThucValidationResult
        {
            HopLe = false,
            ThongBao = thongBao,
            TruongLoi = truongLoi
        };
    }
}

public sealed record CongThucOverviewStats(
    int TongCongThuc,
    int MonCoCongThuc,
    int NguyenLieuThamGia,
    int CongThucCanhBao);

public sealed class CongThucPresenter
{
    public CongThucValidationResult ValidateInput(object? monSelectedValue, object? nguyenLieuSelectedValue, string? soLuongText)
    {
        if (monSelectedValue is not int monId || monId <= 0)
        {
            return CongThucValidationResult.ThatBai("Vui lòng chọn món.", CongThucInputField.Mon);
        }

        if (nguyenLieuSelectedValue is not int nguyenLieuId || nguyenLieuId <= 0)
        {
            return CongThucValidationResult.ThatBai("Vui lòng chọn nguyên liệu.", CongThucInputField.NguyenLieu);
        }

        var soLuongResult = ValidateSoLuong(soLuongText);
        if (!soLuongResult.HopLe || !soLuongResult.SoLuong.HasValue)
        {
            return soLuongResult;
        }

        return CongThucValidationResult.ThanhCong(monId, nguyenLieuId, soLuongResult.SoLuong.Value);
    }

    public CongThucValidationResult ValidateSoLuong(string? soLuongText)
    {
        if (!TryParseSoLuong(soLuongText, out var soLuong) || soLuong <= 0)
        {
            return CongThucValidationResult.ThatBai("Định lượng không hợp lệ.", CongThucInputField.DinhLuong);
        }

        return CongThucValidationResult.ThanhCong(soLuong: soLuong);
    }

    public CongThucOverviewStats BuildOverviewStats(IReadOnlyCollection<CongThucMonDTO> dsCongThuc)
    {
        return new CongThucOverviewStats(
            TongCongThuc: dsCongThuc.Count,
            MonCoCongThuc: dsCongThuc.Select(x => x.MonID).Distinct().Count(),
            NguyenLieuThamGia: dsCongThuc.Select(x => x.NguyenLieuID).Distinct().Count(),
            CongThucCanhBao: dsCongThuc.Count(x => x.SoLuongTon < x.SoLuong));
    }

    public string BuildTrangThaiTon(NguyenLieuDTO nguyenLieu, string? soLuongText)
    {
        if (!TryParseSoLuong(soLuongText, out var soLuongDinhLuong) || soLuongDinhLuong <= 0)
        {
            return nguyenLieu.TrangThaiHienThi;
        }

        return BuildTrangThaiTon(nguyenLieu.SoLuongTon, soLuongDinhLuong);
    }

    private static bool TryParseSoLuong(string? input, out decimal soLuong)
    {
        var normalized = input?.Trim() ?? string.Empty;
        if (normalized.Length == 0)
        {
            soLuong = 0;
            return false;
        }

        return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.CurrentCulture, out soLuong)
               || decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out soLuong);
    }

    private static string BuildTrangThaiTon(decimal soLuongTon, decimal soLuongDinhLuong)
    {
        if (soLuongTon <= 0)
        {
            return "Hết hàng";
        }

        if (soLuongTon < soLuongDinhLuong)
        {
            return "Thiếu";
        }

        return "Đủ";
    }
}