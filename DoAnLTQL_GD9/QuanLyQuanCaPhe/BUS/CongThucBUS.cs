using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class CongThucBUS : ICongThucService
{
    private readonly ICongThucRepository _congThucDAL;
    private readonly IMonService _monBUS;
    private readonly INguyenLieuService _nguyenLieuBUS;
    private readonly IPermissionService _permissionBUS;

    public CongThucBUS(
        ICongThucRepository? congThucDAL = null,
        IMonService? monBUS = null,
        INguyenLieuService? nguyenLieuBUS = null,
        IPermissionService? permissionBUS = null)
    {
        _congThucDAL = congThucDAL ?? new CongThucDAL();
        _monBUS = monBUS ?? new MonBUS();
        _nguyenLieuBUS = nguyenLieuBUS ?? new NguyenLieuBUS();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public List<CongThucMonDTO> LayDanhSachCongThuc(string? tuKhoa, int? monId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new List<CongThucMonDTO>();
        }

        var keyword = BusInputHelper.NormalizeNullableText(tuKhoa);
        return _congThucDAL.GetDanhSachCongThuc(keyword, monId);
    }

    public List<MonDTO> LayDanhSachMonChoCongThuc()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new List<MonDTO>();
        }

        return _monBUS
            .LayDanhSachMon(null, null)
            .Where(x => x.TrangThai != 0)
            .OrderBy(x => x.TenMon)
            .ToList();
    }

    public List<NguyenLieuDTO> LayDanhSachNguyenLieuChoCongThuc()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.View))
        {
            return new List<NguyenLieuDTO>();
        }

        return _nguyenLieuBUS
            .LayDanhSachNguyenLieu(null)
            .Where(x => x.TrangThai != 0)
            .OrderBy(x => x.TenNguyenLieu)
            .ToList();
    }

    public (bool ThanhCong, string ThongBao) ThemCongThuc(int monId, int nguyenLieuId, decimal soLuong)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create))
        {
            return (false, "Bạn không có quyền thêm công thức.");
        }

        var validation = KiemTraDuLieu(monId, nguyenLieuId, soLuong);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi);
        }

        return _congThucDAL.ThemCongThuc(monId, nguyenLieuId, soLuong);
    }

    public (bool ThanhCong, string ThongBao) CapNhatCongThuc(int monId, int nguyenLieuId, decimal soLuong)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền cập nhật công thức.");
        }

        var validation = KiemTraDuLieu(monId, nguyenLieuId, soLuong);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi);
        }

        return _congThucDAL.CapNhatCongThuc(monId, nguyenLieuId, soLuong);
    }

    public (bool ThanhCong, string ThongBao) XoaCongThuc(int monId, int nguyenLieuId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Delete))
        {
            return (false, "Bạn không có quyền xóa công thức.");
        }

        if (monId <= 0 || nguyenLieuId <= 0)
        {
            return (false, "Vui lòng chọn công thức hợp lệ để xóa.");
        }

        return _congThucDAL.XoaCongThuc(monId, nguyenLieuId);
    }

    private static (bool HopLe, string ThongBaoLoi) KiemTraDuLieu(int monId, int nguyenLieuId, decimal soLuong)
    {
        if (monId <= 0)
        {
            return (false, "Vui lòng chọn món.");
        }

        if (nguyenLieuId <= 0)
        {
            return (false, "Vui lòng chọn nguyên liệu.");
        }

        if (soLuong <= 0)
        {
            return (false, "Số lượng nguyên liệu phải lớn hơn 0.");
        }

        if (soLuong > 999999)
        {
            return (false, "Số lượng nguyên liệu quá lớn.");
        }

        return (true, string.Empty);
    }
}
