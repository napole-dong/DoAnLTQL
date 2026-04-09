using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class NhanVienBUS
{
    private readonly NhanVienDAL _nhanVienDAL = new();
    private readonly PermissionBUS _permissionBUS = new();

    public List<NhanVienDTO> LayDanhSachNhanVien(string? tuKhoa)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View))
        {
            return new List<NhanVienDTO>();
        }

        return _nhanVienDAL.GetDanhSachNhanVien(BusInputHelper.NormalizeNullableText(tuKhoa));
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
        if (!_permissionBUS.CheckPermission(PermissionFeatures.TaiKhoan, PermissionActions.View))
        {
            return new List<string>();
        }

        return _permissionBUS.LayDanhSachVaiTroCoTheGan();
    }

    public (bool ThanhCong, string ThongBao, NhanVienDTO? NhanVienMoi) ThemNhanVien(NhanVienDTO nhanVienDTO)
    {
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

        if (_nhanVienDAL.TenDangNhapDaTonTai(nhanVienDTO.TenDangNhap))
        {
            return (false, "Tên đăng nhập đã tồn tại.", null);
        }

        var nhanVienMoi = _nhanVienDAL.ThemNhanVien(nhanVienDTO);
        return (true, "Thêm nhân viên thành công.", nhanVienMoi);
    }

    public (bool ThanhCong, string ThongBao) CapNhatNhanVien(NhanVienDTO nhanVienDTO)
    {
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

        if (_nhanVienDAL.TenDangNhapDaTonTai(nhanVienDTO.TenDangNhap, nhanVienDTO.ID))
        {
            return (false, "Tên đăng nhập đã tồn tại.");
        }

        var daCapNhat = _nhanVienDAL.CapNhatNhanVien(nhanVienDTO);
        return daCapNhat
            ? (true, "Cập nhật nhân viên thành công.")
            : (false, "Không tìm thấy nhân viên để cập nhật.");
    }

    public (bool ThanhCong, string ThongBao) XoaNhanVien(int nhanVienId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Delete))
        {
            return (false, "Bạn không có quyền xóa nhân viên.");
        }

        if (nhanVienId <= 0)
        {
            return (false, "Vui lòng chọn nhân viên cần xóa.");
        }

        if (_nhanVienDAL.NhanVienDaPhatSinhHoaDon(nhanVienId))
        {
            return (false, "Nhân viên đã phát sinh hóa đơn, không thể xóa.");
        }

        var daXoa = _nhanVienDAL.XoaNhanVien(nhanVienId);
        return daXoa
            ? (true, "Xóa nhân viên thành công.")
            : (false, "Không tìm thấy nhân viên để xóa.");
    }

    public (int SoThemMoi, int SoCapNhat, int SoBoQua) NhapNhanVienTuCsv(string[] lines)
    {
        var coQuyenThemMoi = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Create);
        var coQuyenCapNhat = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Update);
        var coQuyenPhanQuyen = _permissionBUS.CheckPermission(PermissionFeatures.TaiKhoan, PermissionActions.PhanQuyen);

        if ((!coQuyenThemMoi && !coQuyenCapNhat) || !coQuyenPhanQuyen)
        {
            return (0, 0, lines.Length);
        }

        if (lines.Length == 0)
        {
            return (0, 0, 1);
        }

        var dsNhap = new List<NhanVienDTO>();
        var soBoQua = 0;
        var dsVaiTroCoTheGan = new HashSet<string>(
            _permissionBUS.LayDanhSachVaiTroCoTheGan(),
            StringComparer.OrdinalIgnoreCase);

        var startIndex = lines[0].Contains("HoVaTen", StringComparison.OrdinalIgnoreCase)
            || lines[0].Contains("TenDangNhap", StringComparison.OrdinalIgnoreCase)
            ? 1
            : 0;

        for (var i = startIndex; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            if (!BusInputHelper.TrySplitCsvLine(lines[i], out var cot, out _))
            {
                soBoQua++;
                continue;
            }

            var hoVaTen = string.Empty;
            var dienThoai = string.Empty;
            var diaChi = string.Empty;
            var tenDangNhap = string.Empty;
            var matKhau = string.Empty;
            var quyenHan = string.Empty;

            if (cot.Count >= 7)
            {
                hoVaTen = BusInputHelper.NormalizeText(cot[1]);
                dienThoai = BusInputHelper.NormalizeText(cot[2]);
                diaChi = BusInputHelper.NormalizeText(cot[3]);
                tenDangNhap = BusInputHelper.NormalizeText(cot[4]);
                matKhau = BusInputHelper.NormalizeText(cot[5]);
                quyenHan = BusInputHelper.NormalizeText(cot[6]);
            }
            else if (cot.Count == 6)
            {
                hoVaTen = BusInputHelper.NormalizeText(cot[0]);
                dienThoai = BusInputHelper.NormalizeText(cot[1]);
                diaChi = BusInputHelper.NormalizeText(cot[2]);
                tenDangNhap = BusInputHelper.NormalizeText(cot[3]);
                matKhau = BusInputHelper.NormalizeText(cot[4]);
                quyenHan = BusInputHelper.NormalizeText(cot[5]);
            }
            else if (cot.Count == 5)
            {
                hoVaTen = BusInputHelper.NormalizeText(cot[0]);
                dienThoai = BusInputHelper.NormalizeText(cot[1]);
                diaChi = BusInputHelper.NormalizeText(cot[2]);
                tenDangNhap = BusInputHelper.NormalizeText(cot[3]);
                quyenHan = BusInputHelper.NormalizeText(cot[4]);
            }
            else
            {
                soBoQua++;
                continue;
            }

            var nhanVien = new NhanVienDTO
            {
                HoVaTen = hoVaTen,
                DienThoai = string.IsNullOrWhiteSpace(dienThoai) ? null : dienThoai,
                DiaChi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi,
                TenDangNhap = tenDangNhap,
                MatKhau = string.IsNullOrWhiteSpace(matKhau) ? null : matKhau,
                QuyenHan = quyenHan
            };

            var validation = KiemTraThongTin(nhanVien, false);
            if (!validation.HopLe)
            {
                soBoQua++;
                continue;
            }

            if (!dsVaiTroCoTheGan.Contains(nhanVien.QuyenHan))
            {
                soBoQua++;
                continue;
            }

            dsNhap.Add(nhanVien);
        }

        var result = _nhanVienDAL.NhapDanhSachNhanVien(dsNhap, coQuyenThemMoi, coQuyenCapNhat);
        return (result.SoThemMoi, result.SoCapNhat, result.SoBoQua + soBoQua);
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
