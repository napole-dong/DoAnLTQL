using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class NhanVienBUS : INhanVienService
{
    private readonly INhanVienRepository _nhanVienDAL;
    private readonly IPermissionService _permissionBUS;

    public NhanVienBUS(INhanVienRepository? nhanVienDAL = null, IPermissionService? permissionBUS = null)
    {
        _nhanVienDAL = nhanVienDAL ?? new NhanVienDAL();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public async Task<List<NhanVienDTO>> LayDanhSachNhanVienAsync(string? tuKhoa)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View))
        {
            return new List<NhanVienDTO>();
        }

        return await _nhanVienDAL.GetDanhSachNhanVienAsync(BusInputHelper.NormalizeNullableText(tuKhoa));
    }

    public int LayMaNhanVienTiepTheo()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Create))
        {
            return 0;
        }

        return _nhanVienDAL.GetNextNhanVienId();
    }

    public List<string> LayDanhSachVaiTroCoTheGan()
    {
        if (_permissionBUS.IsManager())
        {
            return new List<string>();
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.TaiKhoan, PermissionActions.View))
        {
            return new List<string>();
        }

        return _permissionBUS.LayDanhSachVaiTroCoTheGan();
    }

    public async Task<(bool ThanhCong, string ThongBao, NhanVienDTO? NhanVienMoi)> ThemNhanVienAsync(NhanVienDTO nhanVienDTO)
    {
        if (_permissionBUS.IsManager())
        {
            return (false, "Manager chỉ được xem thông tin nhân viên.", null);
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Create))
        {
            return (false, "Bạn không có quyền thêm nhân viên.", null);
        }

        ChuanHoaNhanVien(nhanVienDTO);

        var validation = KiemTraThongTin(nhanVienDTO, true);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi, null);
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.TaiKhoan, PermissionActions.PhanQuyen))
        {
            return (false, "Bạn không có quyền gán vai trò tài khoản.", null);
        }

        if (!_permissionBUS.CoTheGanVaiTro(nhanVienDTO.QuyenHan))
        {
            return (false, "Bạn không có quyền gán vai trò này.", null);
        }

        if (await _nhanVienDAL.TenDangNhapDaTonTaiAsync(nhanVienDTO.TenDangNhap))
        {
            return (false, "Tên đăng nhập đã tồn tại.", null);
        }

        var nhanVienMoi = await _nhanVienDAL.ThemNhanVienAsync(nhanVienDTO);
        return (true, "Thêm nhân viên thành công.", nhanVienMoi);
    }

    public async Task<(bool ThanhCong, string ThongBao)> CapNhatNhanVienAsync(NhanVienDTO nhanVienDTO)
    {
        if (_permissionBUS.IsManager())
        {
            return (false, "Manager chỉ được xem thông tin nhân viên.");
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền cập nhật nhân viên.");
        }

        if (nhanVienDTO.ID <= 0)
        {
            return (false, "Vui lòng chọn nhân viên cần cập nhật.");
        }

        ChuanHoaNhanVien(nhanVienDTO);

        var validation = KiemTraThongTin(nhanVienDTO, false);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi);
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.TaiKhoan, PermissionActions.PhanQuyen))
        {
            return (false, "Bạn không có quyền gán vai trò tài khoản.");
        }

        if (!_permissionBUS.CoTheGanVaiTro(nhanVienDTO.QuyenHan))
        {
            return (false, "Bạn không có quyền gán vai trò này.");
        }

        if (await _nhanVienDAL.TenDangNhapDaTonTaiAsync(nhanVienDTO.TenDangNhap, nhanVienDTO.ID))
        {
            return (false, "Tên đăng nhập đã tồn tại.");
        }

        var daCapNhat = await _nhanVienDAL.CapNhatNhanVienAsync(nhanVienDTO);
        return daCapNhat
            ? (true, "Cập nhật nhân viên thành công.")
            : (false, "Không tìm thấy nhân viên để cập nhật.");
    }

    public (bool ThanhCong, string ThongBao) XoaNhanVien(int nhanVienId, bool softDelete = true)
    {
        if (_permissionBUS.IsManager())
        {
            return softDelete
                ? (false, "Manager không được phép ngừng hoạt động nhân viên.")
                : (false, "Manager không được phép xóa nhân viên.");
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Delete))
        {
            return softDelete
                ? (false, "Bạn không có quyền ngừng hoạt động nhân viên.")
                : (false, "Bạn không có quyền xóa nhân viên.");
        }

        if (nhanVienId <= 0)
        {
            return softDelete
                ? (false, "Vui lòng chọn nhân viên cần ngừng hoạt động.")
                : (false, "Vui lòng chọn nhân viên cần xóa.");
        }

        var result = _nhanVienDAL.DeleteNhanVien(nhanVienId, softDelete);
        return MapDeleteResult(result);
    }

    public (bool ThanhCong, string ThongBao) KhoiPhucNhanVien(int nhanVienId)
    {
        if (_permissionBUS.IsManager())
        {
            return (false, "Manager không được phép khôi phục nhân viên.");
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền khôi phục nhân viên.");
        }

        if (nhanVienId <= 0)
        {
            return (false, "Mã nhân viên không hợp lệ.");
        }

        var daKhoiPhuc = _nhanVienDAL.RestoreNhanVien(nhanVienId);
        return daKhoiPhuc
            ? (true, "Khôi phục nhân viên thành công.")
            : (false, "Nhân viên chưa bị ngừng hoạt động hoặc không tồn tại.");
    }

    public (bool ThanhCong, string ThongBao) HardDeleteNhanVien(int nhanVienId)
    {
        if (_permissionBUS.IsManager())
        {
            return (false, "Manager không được phép hard delete nhân viên.");
        }

        if (!_permissionBUS.IsAdmin())
        {
            return (false, "Chỉ Admin mới được hard delete nhân viên.");
        }

        if (nhanVienId <= 0)
        {
            return (false, "Mã nhân viên không hợp lệ.");
        }

        try
        {
            var result = _nhanVienDAL.DeleteNhanVien(nhanVienId, softDelete: false);
            return MapDeleteResult(result);
        }
        catch (InvalidOperationException ex)
        {
            return (false, ex.Message);
        }
    }

    private static (bool ThanhCong, string ThongBao) MapDeleteResult(
        NhanVienDAL.DeleteNhanVienResult result)
    {
        return result.Outcome switch
        {
            NhanVienDAL.DeleteNhanVienOutcome.SuccessHardDelete
                => (true, "Xóa nhân viên thành công. Tài khoản liên kết đã được xóa tự động."),
            NhanVienDAL.DeleteNhanVienOutcome.SuccessSoftDelete
                => (true, "Đã ngừng hoạt động nhân viên và khóa tài khoản liên kết."),
            NhanVienDAL.DeleteNhanVienOutcome.ForbiddenSelfDelete
                => (false, "Không thể xóa chính tài khoản đang đăng nhập."),
            NhanVienDAL.DeleteNhanVienOutcome.ForbiddenAdminAccount
                => (false, "Không được phép xóa hoặc khóa tài khoản Admin."),
            NhanVienDAL.DeleteNhanVienOutcome.NotFound
                => (false, "Nhân viên không tồn tại."),
            NhanVienDAL.DeleteNhanVienOutcome.AlreadyDeleted
                => (false, "Nhân viên đã ngừng hoạt động trước đó."),
            NhanVienDAL.DeleteNhanVienOutcome.HasInvoices
                => (false, "Không thể xóa cứng nhân viên đã phát sinh hóa đơn. Hãy chọn ngừng hoạt động (soft delete)."),
            NhanVienDAL.DeleteNhanVienOutcome.InvalidInput
                => (false, "Mã nhân viên không hợp lệ."),
            _ => (false, "Không thể xử lý yêu cầu xóa nhân viên.")
        };
    }

    private static (bool HopLe, string ThongBaoLoi) KiemTraThongTin(NhanVienDTO nhanVienDTO, bool batBuocMatKhau)
    {
        if (string.IsNullOrWhiteSpace(nhanVienDTO.HoVaTen))
        {
            return (false, "Họ và tên không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(nhanVienDTO.TenDangNhap))
        {
            return (false, "Tên đăng nhập không được để trống.");
        }

        if (batBuocMatKhau && string.IsNullOrWhiteSpace(nhanVienDTO.MatKhau))
        {
            return (false, "Mật khẩu không được để trống.");
        }

        if (!string.IsNullOrWhiteSpace(nhanVienDTO.MatKhau)
            && !MatKhauService.DatYeuCauDoManh(nhanVienDTO.MatKhau, out var thongBaoLoiMatKhau))
        {
            return (false, thongBaoLoiMatKhau);
        }

        if (string.IsNullOrWhiteSpace(nhanVienDTO.QuyenHan))
        {
            return (false, "Vui lòng chọn quyền hạn.");
        }

        if (!string.IsNullOrWhiteSpace(nhanVienDTO.DienThoai)
            && !BusInputHelper.IsValidPhoneNumber(nhanVienDTO.DienThoai))
        {
            return (false, "Số điện thoại không hợp lệ.");
        }

        return (true, string.Empty);
    }

    private static void ChuanHoaNhanVien(NhanVienDTO nhanVienDTO)
    {
        nhanVienDTO.HoVaTen = BusInputHelper.NormalizeText(nhanVienDTO.HoVaTen);
        nhanVienDTO.TenDangNhap = BusInputHelper.NormalizeText(nhanVienDTO.TenDangNhap);
        nhanVienDTO.QuyenHan = BusInputHelper.NormalizeText(nhanVienDTO.QuyenHan);
        nhanVienDTO.DienThoai = BusInputHelper.NormalizeNullableText(nhanVienDTO.DienThoai);
        nhanVienDTO.DiaChi = BusInputHelper.NormalizeNullableText(nhanVienDTO.DiaChi);
        nhanVienDTO.MatKhau = BusInputHelper.NormalizeNullableText(nhanVienDTO.MatKhau);
    }
}
