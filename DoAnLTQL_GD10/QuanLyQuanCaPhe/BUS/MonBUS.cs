using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class MonBUS : IMonService
{
    private readonly IMonRepository _monDAL;
    private readonly IPermissionService _permissionBUS;

    public MonBUS(IMonRepository? monDAL = null, IPermissionService? permissionBUS = null)
    {
        _monDAL = monDAL ?? new MonDAL();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public List<LoaiMonDTO> LayDanhSachLoaiMon()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new List<LoaiMonDTO>();
        }

        return _monDAL.GetLoaiMon();
    }

    public List<MonDTO> LayDanhSachMon(string? textSearch, string? textTimMon)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new List<MonDTO>();
        }

        var tuKhoa = BusInputHelper.MergeKeywords(textSearch, textTimMon);
        return _monDAL.GetDanhSachMon(tuKhoa);
    }

    public int LayMaMonTiepTheo()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create))
        {
            return 0;
        }

        return _monDAL.GetNextMonId();
    }

    public (bool ThanhCong, string ThongBao, MonDTO? MonMoi) ThemMon(MonDTO monDTO)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create))
        {
            return (false, "Bạn không có quyền thêm món.", null);
        }

        var validation = KiemTraMon(monDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi, null);
        }

        if (_monDAL.TenMonDaTonTai(monDTO.TenMon))
        {
            return (false, "Tên món đã tồn tại.", null);
        }

        var monMoi = _monDAL.ThemMon(monDTO);
        return (true, "Thêm món thành công.", monMoi);
    }

    public (bool ThanhCong, string ThongBao) CapNhatMon(MonDTO monDTO)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền cập nhật món.");
        }

        if (monDTO.ID <= 0)
        {
            return (false, "Vui lòng chọn món cần cập nhật.");
        }

        var monHienTai = _monDAL.GetMonById(monDTO.ID);
        if (monHienTai == null)
        {
            return (false, "Không tìm thấy món để cập nhật.");
        }

        var validation = KiemTraMon(monDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi);
        }

        var tenMonDaDoi = !string.Equals(
            BusInputHelper.NormalizeText(monHienTai.TenMon),
            monDTO.TenMon,
            StringComparison.OrdinalIgnoreCase);

        if (tenMonDaDoi && _monDAL.TenMonDaTonTai(monDTO.TenMon, monDTO.ID))
        {
            return (false, "Tên món đã tồn tại.");
        }

        if (monHienTai.DonGia != monDTO.DonGia && !_permissionBUS.CanEditMenuPrice())
        {
            return (false, "Bạn không có quyền sửa giá món.");
        }

        var daCapNhat = _monDAL.CapNhatMon(monDTO);
        return daCapNhat
            ? (true, "Cập nhật món thành công.")
            : (false, "Không tìm thấy món để cập nhật.");
    }

    public (bool ThanhCong, string ThongBao) XoaMon(int monId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Delete))
        {
            return (false, "Bạn không có quyền ngừng bán món.");
        }

        if (monId <= 0)
        {
            return (false, "Vui lòng chọn món cần ngừng bán.");
        }

        var daXoa = _monDAL.XoaMon(monId);
        return daXoa
            ? (true, "Đã ngừng bán món thành công.")
            : (false, "Món không tồn tại hoặc đã ngừng bán trước đó.");
    }

    public (bool ThanhCong, string ThongBao) DeleteMenu(int monId)
    {
        if (!_permissionBUS.IsAdmin() && !_permissionBUS.IsManager())
        {
            return (false, "Bạn không có quyền ngừng bán món.");
        }

        if (monId <= 0)
        {
            return (false, "Mã món không hợp lệ.");
        }

        return _monDAL.XoaMon(monId)
            ? (true, "Đã ngừng bán món thành công.")
            : (false, "Món không tồn tại hoặc đã ngừng bán trước đó.");
    }

    public (bool ThanhCong, string ThongBao) KhoiPhucMon(int monId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền khôi phục món.");
        }

        if (monId <= 0)
        {
            return (false, "Mã món không hợp lệ.");
        }

        var daKhoiPhuc = _monDAL.RestoreMon(monId);
        return daKhoiPhuc
            ? (true, "Khôi phục món thành công.")
            : (false, "Món chưa bị ngừng bán hoặc không tồn tại.");
    }

    public (bool ThanhCong, string ThongBao) HardDeleteMon(int monId)
    {
        if (!_permissionBUS.IsAdmin())
        {
            return (false, "Chỉ Admin mới được hard delete món.");
        }

        if (monId <= 0)
        {
            return (false, "Mã món không hợp lệ.");
        }

        try
        {
            var daXoa = _monDAL.HardDeleteMon(monId);
            return daXoa
                ? (true, "Hard delete món thành công.")
                : (false, "Không tìm thấy món để hard delete.");
        }
        catch (InvalidOperationException ex)
        {
            return (false, ex.Message);
        }
    }

    private static (bool HopLe, string ThongBaoLoi) KiemTraMon(MonDTO monDTO)
    {
        if (string.IsNullOrWhiteSpace(monDTO.TenMon))
        {
            return (false, "Tên món không được để trống.");
        }

        if (monDTO.LoaiMonID <= 0)
        {
            return (false, "Vui lòng chọn loại món.");
        }

        if (monDTO.TrangThai is < 0 or > 2)
        {
            return (false, "Vui lòng chọn trạng thái món.");
        }

        if (monDTO.DonGia < 0)
        {
            return (false, "Đơn giá không hợp lệ.");
        }

        if (monDTO.TrangThai == 1 && monDTO.DonGia <= 0)
        {
            return (false, "Món đang kinh doanh phải có đơn giá lớn hơn 0.");
        }

        if (monDTO.TrangThai == 0)
        {
            monDTO.DonGia = 0;
        }

        monDTO.TenMon = BusInputHelper.NormalizeText(monDTO.TenMon);
        monDTO.MoTa = BusInputHelper.NormalizeText(monDTO.MoTa);
        monDTO.HinhAnh = BusInputHelper.NormalizeNullableText(monDTO.HinhAnh);

        return (true, string.Empty);
    }

}
