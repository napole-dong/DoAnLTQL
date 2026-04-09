using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class KhachHangBUS
{
    private readonly KhachHangDAL _khachHangDAL = new();
    private readonly PermissionBUS _permissionBUS = new();

    public List<KhachHangDTO> LayDanhSachKhach(string? textTimKhach)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.View))
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
            return (false, "Bạn không có quyền xóa khách hàng.");
        }

        if (khachId <= 0)
        {
            return (false, "Vui lòng chọn khách hàng cần xóa.");
        }

        if (_khachHangDAL.KhachDaPhatSinhHoaDon(khachId))
        {
            return (false, "Khách hàng đã phát sinh hóa đơn, không thể xóa.");
        }

        var daXoa = _khachHangDAL.XoaKhach(khachId);
        return daXoa
            ? (true, "Xóa khách hàng thành công.")
            : (false, "Không tìm thấy khách hàng để xóa.");
    }

    public (int SoThemMoi, int SoCapNhat, int SoBoQua) NhapKhachTuCsv(string[] lines)
    {
        var coQuyenThemMoi = _permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.Create);
        var coQuyenCapNhat = _permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.Update);

        if (!coQuyenThemMoi && !coQuyenCapNhat)
        {
            return (0, 0, lines.Length);
        }

        if (lines.Length == 0)
        {
            return (0, 0, 1);
        }

        var dsKhach = new List<KhachHangDTO>();
        var soBoQua = 0;
        var startIndex = lines[0].Contains("HoVaTen", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

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

            string hoVaTen;
            string dienThoai;
            string diaChi;

            if (cot.Count >= 4)
            {
                hoVaTen = BusInputHelper.NormalizeText(cot[1]);
                dienThoai = BusInputHelper.NormalizeText(cot[2]);
                diaChi = BusInputHelper.NormalizeText(cot[3]);
            }
            else
            {
                hoVaTen = BusInputHelper.NormalizeText(cot.ElementAtOrDefault(0));
                dienThoai = BusInputHelper.NormalizeText(cot.ElementAtOrDefault(1));
                diaChi = BusInputHelper.NormalizeText(cot.ElementAtOrDefault(2));
            }

            if (string.IsNullOrWhiteSpace(hoVaTen))
            {
                soBoQua++;
                continue;
            }

            if (!string.IsNullOrWhiteSpace(dienThoai) && !BusInputHelper.IsValidPhoneNumber(dienThoai))
            {
                soBoQua++;
                continue;
            }

            dsKhach.Add(new KhachHangDTO
            {
                HoVaTen = hoVaTen,
                DienThoai = string.IsNullOrWhiteSpace(dienThoai) ? null : dienThoai,
                DiaChi = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi
            });
        }

        var result = _khachHangDAL.NhapDanhSachKhach(dsKhach, coQuyenThemMoi, coQuyenCapNhat);
        return (result.SoThemMoi, result.SoCapNhat, result.SoBoQua + soBoQua);
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
