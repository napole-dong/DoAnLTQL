using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class KhachHangBUS : IKhachHangService
{
    private readonly IKhachHangRepository _khachHangDAL;
    private readonly IPermissionService _permissionBUS;

    public KhachHangBUS(IKhachHangRepository? khachHangDAL = null, IPermissionService? permissionBUS = null)
    {
        _khachHangDAL = khachHangDAL ?? new KhachHangDAL();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public List<KhachHangDTO> LayDanhSachKhach(string? textTimKhach)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.View))
        {
            return new List<KhachHangDTO>();
        }

        return _khachHangDAL.GetDanhSachKhach(BusInputHelper.NormalizeNullableText(textTimKhach));
    }

    public List<KhachHangDTO> LayDanhSachKhachChoBanHang(string? textTimKhach)
    {
        var coQuyenXemKhachHang = _permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.View);
        var coQuyenBanHang = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View);

        if (!coQuyenXemKhachHang && !coQuyenBanHang)
        {
            return new List<KhachHangDTO>();
        }

        return _khachHangDAL.GetDanhSachKhach(BusInputHelper.NormalizeNullableText(textTimKhach));
    }

    public int LayMaKhachTiepTheo()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.Create))
        {
            return 0;
        }

        return _khachHangDAL.GetNextKhachHangId();
    }

    public (bool ThanhCong, string ThongBao, KhachHangDTO? KhachMoi) ThemKhach(KhachHangDTO khachDTO)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.Create))
        {
            return (false, "Bạn không có quyền thêm khách hàng.", null);
        }

        ChuanHoaKhachHang(khachDTO);

        var validation = KiemTraThongTin(khachDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi, null);
        }

        if (!string.IsNullOrWhiteSpace(khachDTO.DienThoai)
            && _khachHangDAL.DienThoaiDaTonTai(khachDTO.DienThoai))
        {
            return (false, "Số điện thoại đã tồn tại.", null);
        }

        var khachMoi = _khachHangDAL.ThemKhach(khachDTO);
        return (true, "Thêm khách hàng thành công.", khachMoi);
    }

    public (bool ThanhCong, string ThongBao, KhachHangDTO? KhachMoi) ThemKhachNhanhChoBanHang(KhachHangDTO khachDTO)
    {
        var coQuyenThemKhach = _permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.Create)
            || _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update);

        if (!coQuyenThemKhach)
        {
            return (false, "Bạn không có quyền thêm khách hàng mới khi bán hàng.", null);
        }

        ChuanHoaKhachHang(khachDTO);

        var validation = KiemTraThongTin(khachDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi, null);
        }

        if (!string.IsNullOrWhiteSpace(khachDTO.DienThoai)
            && _khachHangDAL.DienThoaiDaTonTai(khachDTO.DienThoai))
        {
            return (false, "Số điện thoại đã tồn tại.", null);
        }

        var khachMoi = _khachHangDAL.ThemKhach(khachDTO);
        return (true, "Đã thêm khách hàng mới.", khachMoi);
    }

    public (bool ThanhCong, string ThongBao) CapNhatKhach(KhachHangDTO khachDTO)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền cập nhật khách hàng.");
        }

        if (khachDTO.ID <= 0)
        {
            return (false, "Vui lòng chọn khách hàng cần cập nhật.");
        }

        ChuanHoaKhachHang(khachDTO);

        var validation = KiemTraThongTin(khachDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi);
        }

        if (!string.IsNullOrWhiteSpace(khachDTO.DienThoai)
            && _khachHangDAL.DienThoaiDaTonTai(khachDTO.DienThoai, khachDTO.ID))
        {
            return (false, "Số điện thoại đã tồn tại.");
        }

        var daCapNhat = _khachHangDAL.CapNhatKhach(khachDTO);
        return daCapNhat
            ? (true, "Cập nhật khách hàng thành công.")
            : (false, "Không tìm thấy khách hàng để cập nhật.");
    }

    public (bool ThanhCong, string ThongBao) XoaKhach(int khachId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.Delete))
        {
            return (false, "Bạn không có quyền ngừng hoạt động khách hàng.");
        }

        if (khachId <= 0)
        {
            return (false, "Vui lòng chọn khách hàng cần ngừng hoạt động.");
        }

        var daXoa = _khachHangDAL.XoaKhach(khachId);
        return daXoa
            ? (true, "Đã ngừng hoạt động khách hàng thành công.")
            : (false, "Khách hàng không tồn tại hoặc đã ngừng hoạt động trước đó.");
    }

    public (bool ThanhCong, string ThongBao) KhoiPhucKhach(int khachId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền khôi phục khách hàng.");
        }

        if (khachId <= 0)
        {
            return (false, "Mã khách hàng không hợp lệ.");
        }

        var daKhoiPhuc = _khachHangDAL.RestoreKhach(khachId);
        return daKhoiPhuc
            ? (true, "Khôi phục khách hàng thành công.")
            : (false, "Khách hàng chưa bị ngừng hoạt động hoặc không tồn tại.");
    }

    public (bool ThanhCong, string ThongBao) HardDeleteKhach(int khachId)
    {
        if (!_permissionBUS.IsAdmin())
        {
            return (false, "Chỉ Admin mới được hard delete khách hàng.");
        }

        if (khachId <= 0)
        {
            return (false, "Mã khách hàng không hợp lệ.");
        }

        try
        {
            var daXoa = _khachHangDAL.HardDeleteKhach(khachId);
            return daXoa
                ? (true, "Hard delete khách hàng thành công.")
                : (false, "Không tìm thấy khách hàng để hard delete.");
        }
        catch (InvalidOperationException ex)
        {
            return (false, ex.Message);
        }
    }

    private static (bool HopLe, string ThongBaoLoi) KiemTraThongTin(KhachHangDTO khachDTO)
    {
        if (string.IsNullOrWhiteSpace(khachDTO.HoVaTen))
        {
            return (false, "Họ và tên không được để trống.");
        }

        if (!string.IsNullOrWhiteSpace(khachDTO.DienThoai) && !BusInputHelper.IsValidPhoneNumber(khachDTO.DienThoai))
        {
            return (false, "Số điện thoại không hợp lệ.");
        }

        return (true, string.Empty);
    }

    private static void ChuanHoaKhachHang(KhachHangDTO khachDTO)
    {
        khachDTO.HoVaTen = BusInputHelper.NormalizeText(khachDTO.HoVaTen);
        khachDTO.DienThoai = BusInputHelper.NormalizeNullableText(khachDTO.DienThoai);
        khachDTO.DiaChi = BusInputHelper.NormalizeNullableText(khachDTO.DiaChi);
    }
}
