using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class LoaiMonBUS : ILoaiMonService
{
    private readonly ILoaiMonRepository _loaiMonDAL;
    private readonly IPermissionService _permissionBUS;

    public LoaiMonBUS(ILoaiMonRepository? loaiMonDAL = null, IPermissionService? permissionBUS = null)
    {
        _loaiMonDAL = loaiMonDAL ?? new LoaiMonDAL();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public List<LoaiMonDTO> LayDanhSachLoai(string? textSearch, string? textTimLoai)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new List<LoaiMonDTO>();
        }

        var tuKhoa = BusInputHelper.MergeKeywords(textSearch, textTimLoai);
        return _loaiMonDAL.GetDanhSachLoai(tuKhoa);
    }

    public int LayMaLoaiTiepTheo()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create))
        {
            return 0;
        }

        return _loaiMonDAL.GetNextLoaiMonId();
    }

    public (bool ThanhCong, string ThongBao, LoaiMonDTO? LoaiMoi) ThemLoai(string tenLoai, string? moTa)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create))
        {
            return (false, "Bạn không có quyền thêm loại món.", null);
        }

        var tenLoaiChuan = BusInputHelper.NormalizeText(tenLoai);
        var moTaChuan = BusInputHelper.NormalizeNullableText(moTa);

        if (tenLoaiChuan.Length == 0)
        {
            return (false, "Tên loại món không được để trống.", null);
        }

        if (_loaiMonDAL.TenLoaiDaTonTai(tenLoaiChuan))
        {
            return (false, "Tên loại món đã tồn tại.", null);
        }

        var loai = _loaiMonDAL.ThemLoai(tenLoaiChuan, moTaChuan);
        return (true, "Thêm loại món thành công.", loai);
    }

    public (bool ThanhCong, string ThongBao) CapNhatLoai(int id, string tenLoai, string? moTa)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền cập nhật loại món.");
        }

        var tenLoaiChuan = BusInputHelper.NormalizeText(tenLoai);
        var moTaChuan = BusInputHelper.NormalizeNullableText(moTa);

        if (id <= 0)
        {
            return (false, "Vui lòng chọn loại món cần cập nhật.");
        }

        if (tenLoaiChuan.Length == 0)
        {
            return (false, "Tên loại món không được để trống.");
        }

        if (_loaiMonDAL.TenLoaiDaTonTai(tenLoaiChuan, id))
        {
            return (false, "Tên loại món đã tồn tại.");
        }

        var daCapNhat = _loaiMonDAL.CapNhatLoai(id, tenLoaiChuan, moTaChuan);
        return daCapNhat
            ? (true, "Cập nhật loại món thành công.")
            : (false, "Không tìm thấy loại món để cập nhật.");
    }

    public (bool ThanhCong, string ThongBao) XoaLoai(int id)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Delete))
        {
            return (false, "Bạn không có quyền xóa loại món.");
        }

        if (id <= 0)
        {
            return (false, "Vui lòng chọn loại món cần xóa.");
        }

        var ketQua = _loaiMonDAL.XoaLoai(id);
        return (ketQua.ThanhCong, ketQua.ThongBao);
    }

    public (bool ThanhCong, string ThongBao) ChuyenMonSangLoaiKhac(int loaiNguonId, int loaiDichId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền chuyển món giữa các loại.");
        }

        if (loaiNguonId <= 0 || loaiDichId <= 0)
        {
            return (false, "Loại món chuyển không hợp lệ.");
        }

        if (loaiNguonId == loaiDichId)
        {
            return (false, "Loại món đích phải khác loại món nguồn.");
        }

        if (!_loaiMonDAL.LoaiTonTai(loaiNguonId) || !_loaiMonDAL.LoaiTonTai(loaiDichId))
        {
            return (false, "Loại món nguồn hoặc đích không tồn tại.");
        }

        var daChuyen = _loaiMonDAL.ChuyenMonSangLoaiKhac(loaiNguonId, loaiDichId);
        return daChuyen
            ? (true, "Đã chuyển món sang loại mới.")
            : (false, "Không thể chuyển món sang loại mới.");
    }

}
