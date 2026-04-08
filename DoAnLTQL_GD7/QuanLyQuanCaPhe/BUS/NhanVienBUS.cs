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

        return _nhanVienDAL.GetDanhSachNhanVien(tuKhoa?.Trim());
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
        return _permissionBUS.LayDanhSachVaiTroCoTheGan();
    }

    public (bool ThanhCong, string ThongBao, NhanVienDTO? NhanVienMoi) ThemNhanVien(NhanVienDTO nhanVienDTO)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Create))
        {
            return (false, "Bạn không có quyền thêm nhân viên.", null);
        }

        var validation = KiemTraThongTin(nhanVienDTO, false);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi, null);
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

        var validation = KiemTraThongTin(nhanVienDTO, true);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi);
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
        if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Create)
            && !_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Update))
        {
            return (0, 0, lines.Length);
        }

        if (lines.Length == 0)
        {
            return (0, 0, 1);
        }

        var dsNhap = new List<NhanVienDTO>();
        var soBoQua = 0;
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

            var cot = SplitCsvLine(lines[i]);

            var hoVaTen = string.Empty;
            var dienThoai = string.Empty;
            var diaChi = string.Empty;
            var tenDangNhap = string.Empty;
            var matKhau = string.Empty;
            var quyenHan = string.Empty;

            if (cot.Count >= 7)
            {
                hoVaTen = cot[1].Trim();
                dienThoai = cot[2].Trim();
                diaChi = cot[3].Trim();
                tenDangNhap = cot[4].Trim();
                matKhau = cot[5].Trim();
                quyenHan = cot[6].Trim();
            }
            else if (cot.Count == 6)
            {
                hoVaTen = cot[0].Trim();
                dienThoai = cot[1].Trim();
                diaChi = cot[2].Trim();
                tenDangNhap = cot[3].Trim();
                matKhau = cot[4].Trim();
                quyenHan = cot[5].Trim();
            }
            else if (cot.Count == 5)
            {
                hoVaTen = cot[0].Trim();
                dienThoai = cot[1].Trim();
                diaChi = cot[2].Trim();
                tenDangNhap = cot[3].Trim();
                quyenHan = cot[4].Trim();
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
                MatKhau = string.IsNullOrWhiteSpace(matKhau) ? "123456" : matKhau,
                QuyenHan = quyenHan
            };

            var validation = KiemTraThongTin(nhanVien, false);
            if (!validation.HopLe)
            {
                soBoQua++;
                continue;
            }

            dsNhap.Add(nhanVien);
        }

        var result = _nhanVienDAL.NhapDanhSachNhanVien(dsNhap);
        return (result.SoThemMoi, result.SoCapNhat, result.SoBoQua + soBoQua);
    }

    private static (bool HopLe, string ThongBaoLoi) KiemTraThongTin(NhanVienDTO nhanVienDTO, bool laCapNhat)
    {
        if (string.IsNullOrWhiteSpace(nhanVienDTO.HoVaTen))
        {
            return (false, "Họ và tên không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(nhanVienDTO.TenDangNhap))
        {
            return (false, "Tên đăng nhập không được để trống.");
        }

        if (!laCapNhat && string.IsNullOrWhiteSpace(nhanVienDTO.MatKhau))
        {
            return (false, "Mật khẩu không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(nhanVienDTO.QuyenHan))
        {
            return (false, "Vui lòng chọn quyền hạn.");
        }

        if (!string.IsNullOrWhiteSpace(nhanVienDTO.DienThoai)
            && (!nhanVienDTO.DienThoai.All(char.IsDigit) || nhanVienDTO.DienThoai.Length is < 9 or > 11))
        {
            return (false, "Số điện thoại không hợp lệ.");
        }

        return (true, string.Empty);
    }

    private static List<string> SplitCsvLine(string line)
    {
        var result = new List<string>();
        var current = new System.Text.StringBuilder();
        var inQuotes = false;

        foreach (var c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (c == ',' && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
                continue;
            }

            current.Append(c);
        }

        result.Add(current.ToString());
        return result;
    }
}
