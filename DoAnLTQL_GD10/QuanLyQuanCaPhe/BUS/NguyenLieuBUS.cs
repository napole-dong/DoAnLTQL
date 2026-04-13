using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class NguyenLieuBUS : INguyenLieuService
{
    private readonly INguyenLieuRepository _nguyenLieuDAL;
    private readonly IPermissionService _permissionBUS;

    public NguyenLieuBUS(INguyenLieuRepository? nguyenLieuDAL = null, IPermissionService? permissionBUS = null)
    {
        _nguyenLieuDAL = nguyenLieuDAL ?? new NguyenLieuDAL();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public List<NguyenLieuDTO> LayDanhSachNguyenLieu(string? tuKhoa)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.View))
        {
            return new List<NguyenLieuDTO>();
        }

        return _nguyenLieuDAL.GetDanhSachNguyenLieu(BusInputHelper.NormalizeNullableText(tuKhoa));
    }

    public List<NguyenLieuDTO> LayDanhSachNguyenLieuSapHet()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.View))
        {
            return new List<NguyenLieuDTO>();
        }

        return _nguyenLieuDAL.GetDanhSachNguyenLieuSapHet();
    }

    public int LayMaNguyenLieuTiepTheo()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Create))
        {
            return 0;
        }

        return _nguyenLieuDAL.GetNextNguyenLieuId();
    }

    public (bool ThanhCong, string ThongBao, NguyenLieuDTO? NguyenLieuMoi) ThemNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Create))
        {
            return (false, "Bạn không có quyền thêm nguyên liệu.", null);
        }

        ChuanHoaNguyenLieu(nguyenLieuDTO);

        var validation = KiemTraThongTin(nguyenLieuDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi, null);
        }

        nguyenLieuDTO.TrangThai = TinhTrangThai(nguyenLieuDTO.SoLuongTon, nguyenLieuDTO.MucCanhBao, nguyenLieuDTO.TrangThai);
        var nguyenLieuMoi = _nguyenLieuDAL.ThemNguyenLieu(nguyenLieuDTO);
        return (true, "Thêm nguyên liệu thành công.", nguyenLieuMoi);
    }

    public (bool ThanhCong, string ThongBao) CapNhatNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền cập nhật nguyên liệu.");
        }

        if (nguyenLieuDTO.MaNguyenLieu <= 0)
        {
            return (false, "Vui lòng chọn nguyên liệu cần cập nhật.");
        }

        ChuanHoaNguyenLieu(nguyenLieuDTO);

        var validation = KiemTraThongTin(nguyenLieuDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi);
        }

        nguyenLieuDTO.TrangThai = TinhTrangThai(nguyenLieuDTO.SoLuongTon, nguyenLieuDTO.MucCanhBao, nguyenLieuDTO.TrangThai);
        var daCapNhat = _nguyenLieuDAL.CapNhatNguyenLieu(nguyenLieuDTO);

        return daCapNhat
            ? (true, "Cập nhật nguyên liệu thành công.")
            : (false, "Không tìm thấy nguyên liệu để cập nhật.");
    }

    public (bool ThanhCong, string ThongBao) XoaNguyenLieu(int maNguyenLieu)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Delete))
        {
            return (false, "Bạn không có quyền xóa nguyên liệu.");
        }

        if (maNguyenLieu <= 0)
        {
            return (false, "Vui lòng chọn nguyên liệu cần xóa.");
        }

        var ketQua = _nguyenLieuDAL.XoaNguyenLieu(maNguyenLieu);
        return (ketQua.ThanhCong, ketQua.ThongBao);
    }

    public (bool ThanhCong, string ThongBao) NhapKhoNhieuNguyenLieu(IEnumerable<NhapKhoChiTietDTO> dsChiTiet, string? ghiChu)
    {
        try
        {
            _permissionBUS.EnsurePermission(PermissionFeatures.NguyenLieu, PermissionActions.NhapKho, "Bạn không có quyền nhập kho.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return (false, ex.Message);
        }

        if (dsChiTiet == null)
        {
            return (false, "Danh sách chi tiết nhập kho không hợp lệ.");
        }

        var dsChiTietHopLe = dsChiTiet
            .Where(x => x != null)
            .Select(x => new NhapKhoChiTietDTO
            {
                NguyenLieuID = x.NguyenLieuID,
                SoLuong = x.SoLuong,
                DonGiaNhap = x.DonGiaNhap
            })
            .ToList();

        if (dsChiTietHopLe.Count == 0)
        {
            return (false, "Phiếu nhập kho phải có ít nhất 1 nguyên liệu.");
        }

        if (dsChiTietHopLe.Any(x => x.NguyenLieuID <= 0))
        {
            return (false, "Có nguyên liệu không hợp lệ trong phiếu nhập kho.");
        }

        if (dsChiTietHopLe.Any(x => x.SoLuong <= 0))
        {
            return (false, "Số lượng nhập phải lớn hơn 0.");
        }

        if (dsChiTietHopLe.Any(x => x.DonGiaNhap < 0))
        {
            return (false, "Giá nhập không hợp lệ.");
        }

        var coNguyenLieuTrung = dsChiTietHopLe
            .GroupBy(x => x.NguyenLieuID)
            .Any(g => g.Count() > 1);
        if (coNguyenLieuTrung)
        {
            return (false, "Một nguyên liệu chỉ được xuất hiện 1 lần trong cùng phiếu nhập.");
        }

        var ghiChuDaChuanHoa = BusInputHelper.NormalizeNullableText(ghiChu);
        return _nguyenLieuDAL.NhapKhoNhieuNguyenLieu(dsChiTietHopLe, ghiChuDaChuanHoa);
    }

    public (bool ThanhCong, string ThongBao) NhapKho(int maNguyenLieu, decimal soLuongNhap, decimal giaNhap, string? ghiChu)
    {
        return NhapKhoNhieuNguyenLieu(
            new[]
            {
                new NhapKhoChiTietDTO
                {
                    NguyenLieuID = maNguyenLieu,
                    SoLuong = soLuongNhap,
                    DonGiaNhap = giaNhap
                }
            },
            ghiChu);
    }

    private static (bool HopLe, string ThongBaoLoi) KiemTraThongTin(NguyenLieuDTO nguyenLieuDTO)
    {
        if (string.IsNullOrWhiteSpace(nguyenLieuDTO.TenNguyenLieu))
        {
            return (false, "Tên nguyên liệu không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(nguyenLieuDTO.DonViTinh))
        {
            return (false, "Vui lòng chọn đơn vị tính.");
        }

        if (nguyenLieuDTO.SoLuongTon < 0)
        {
            return (false, "Số lượng tồn không hợp lệ.");
        }

        if (nguyenLieuDTO.MucCanhBao < 0)
        {
            return (false, "Mức cảnh báo không hợp lệ.");
        }

        if (nguyenLieuDTO.GiaNhapGanNhat < 0)
        {
            return (false, "Giá nhập gần nhất không hợp lệ.");
        }

        if (nguyenLieuDTO.TrangThai is < 0 or > 2)
        {
            return (false, "Vui lòng chọn trạng thái.");
        }

        return (true, string.Empty);
    }

    private static int TinhTrangThai(decimal soLuongTon, decimal mucCanhBao, int trangThaiHienTai)
    {
        if (trangThaiHienTai == 0)
        {
            return 0;
        }

        if (soLuongTon <= 0)
        {
            return 2;
        }

        if (soLuongTon <= mucCanhBao)
        {
            return 2;
        }

        return 1;
    }

    private static void ChuanHoaNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        nguyenLieuDTO.TenNguyenLieu = BusInputHelper.NormalizeText(nguyenLieuDTO.TenNguyenLieu);
        nguyenLieuDTO.DonViTinh = BusInputHelper.NormalizeText(nguyenLieuDTO.DonViTinh);
    }
}
