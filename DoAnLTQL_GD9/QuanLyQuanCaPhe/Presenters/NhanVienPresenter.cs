using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.UI;

namespace QuanLyQuanCaPhe.Presenters;

public enum NhanVienInputField
{
    None,
    HoVaTen,
    DienThoai,
    TenDangNhap,
    MatKhau,
    QuyenHan
}

public sealed class NhanVienValidationResult
{
    public bool HopLe { get; private init; }
    public string? ThongBao { get; private init; }
    public NhanVienInputField TruongLoi { get; private init; }
    public NhanVienDTO? NhanVien { get; private init; }

    public static NhanVienValidationResult ThanhCong(NhanVienDTO nhanVien)
    {
        return new NhanVienValidationResult
        {
            HopLe = true,
            TruongLoi = NhanVienInputField.None,
            NhanVien = nhanVien
        };
    }

    public static NhanVienValidationResult ThatBai(string thongBao, NhanVienInputField truongLoi)
    {
        return new NhanVienValidationResult
        {
            HopLe = false,
            ThongBao = thongBao,
            TruongLoi = truongLoi
        };
    }
}

public sealed record NhanVienOverviewStats(int TongNhanVien, int SoQuanLy, int SoNhanVienKhac);

public sealed class NhanVienPresenter
{
    public NhanVienValidationResult ValidateInput(
        string hoVaTen,
        string dienThoai,
        string diaChi,
        string tenDangNhap,
        string matKhau,
        object? quyenHan,
        bool laCapNhat)
    {
        if (!InputValidationHelper.TryGetRequiredText(hoVaTen, out var hoVaTenDaChuanHoa))
        {
            return NhanVienValidationResult.ThatBai("Họ và tên không được để trống.", NhanVienInputField.HoVaTen);
        }

        if (!InputValidationHelper.TryGetOptionalPhone(dienThoai, out var dienThoaiDaChuanHoa))
        {
            return NhanVienValidationResult.ThatBai("Số điện thoại không hợp lệ.", NhanVienInputField.DienThoai);
        }

        if (!InputValidationHelper.TryGetRequiredText(tenDangNhap, out var tenDangNhapDaChuanHoa))
        {
            return NhanVienValidationResult.ThatBai("Tên đăng nhập không được để trống.", NhanVienInputField.TenDangNhap);
        }

        string? matKhauDaChuanHoa = null;
        if (!laCapNhat)
        {
            if (!InputValidationHelper.TryGetRequiredText(matKhau, out var matKhauBatBuoc))
            {
                return NhanVienValidationResult.ThatBai("Mật khẩu không được để trống.", NhanVienInputField.MatKhau);
            }

            matKhauDaChuanHoa = matKhauBatBuoc;
        }
        else if (InputValidationHelper.TryGetRequiredText(matKhau, out var matKhauTuNhap))
        {
            matKhauDaChuanHoa = matKhauTuNhap;
        }

        var quyenHanDaChuanHoa = quyenHan?.ToString()?.Trim();
        if (string.IsNullOrWhiteSpace(quyenHanDaChuanHoa))
        {
            return NhanVienValidationResult.ThatBai("Vui lòng chọn quyền hạn.", NhanVienInputField.QuyenHan);
        }

        return NhanVienValidationResult.ThanhCong(new NhanVienDTO
        {
            HoVaTen = hoVaTenDaChuanHoa,
            DienThoai = dienThoaiDaChuanHoa,
            DiaChi = InputValidationHelper.NormalizeOptionalText(diaChi),
            TenDangNhap = tenDangNhapDaChuanHoa,
            MatKhau = matKhauDaChuanHoa,
            QuyenHan = quyenHanDaChuanHoa
        });
    }

    public NhanVienOverviewStats BuildOverviewStats(IReadOnlyCollection<NhanVienDTO> dsNhanVien)
    {
        var soQuanLy = dsNhanVien.Count(LaVaiTroQuanLy);
        return new NhanVienOverviewStats(dsNhanVien.Count, soQuanLy, dsNhanVien.Count - soQuanLy);
    }

    private static bool LaVaiTroQuanLy(NhanVienDTO nhanVien)
    {
        return RoleMapper.TryParseRoleEnum(nhanVien.QuyenHan, out var role)
            && role == RoleEnum.Manager;
    }
}
