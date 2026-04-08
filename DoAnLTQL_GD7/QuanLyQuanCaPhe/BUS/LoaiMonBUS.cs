using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class LoaiMonBUS
{
    private readonly LoaiMonDAL _loaiMonDAL = new();
    private readonly PermissionBUS _permissionBUS = new();

    public List<LoaiMonDTO> LayDanhSachLoai(string? textSearch, string? textTimLoai)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View))
        {
            return new List<LoaiMonDTO>();
        }

        var tuKhoa = ($"{textSearch} {textTimLoai}").Trim();
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

        tenLoai = tenLoai.Trim();
        if (string.IsNullOrWhiteSpace(tenLoai))
        {
            return (false, "Tên loại món không được để trống.", null);
        }

        if (_loaiMonDAL.TenLoaiDaTonTai(tenLoai))
        {
            return (false, "Tên loại món đã tồn tại.", null);
        }

        var loai = _loaiMonDAL.ThemLoai(tenLoai, moTa);
        return (true, "Thêm loại món thành công.", loai);
    }

    public (bool ThanhCong, string ThongBao) CapNhatLoai(int id, string tenLoai, string? moTa)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update))
        {
            return (false, "Bạn không có quyền cập nhật loại món.");
        }

        tenLoai = tenLoai.Trim();
        if (id <= 0)
        {
            return (false, "Vui lòng chọn loại món cần cập nhật.");
        }

        if (string.IsNullOrWhiteSpace(tenLoai))
        {
            return (false, "Tên loại món không được để trống.");
        }

        if (_loaiMonDAL.TenLoaiDaTonTai(tenLoai, id))
        {
            return (false, "Tên loại món đã tồn tại.");
        }

        var daCapNhat = _loaiMonDAL.CapNhatLoai(id, tenLoai, moTa);
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

        if (_loaiMonDAL.LoaiDangSuDung(id))
        {
            return (false, "Loại món đang được sử dụng, không thể xóa.");
        }

        var daXoa = _loaiMonDAL.XoaLoai(id);
        return daXoa
            ? (true, "Xóa loại món thành công.")
            : (false, "Không tìm thấy loại món để xóa.");
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

    public LoaiMonImportResultDTO NhapLoaiMonTuCsv(string[] lines)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create)
            && !_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update))
        {
            return new LoaiMonImportResultDTO { SoBoQua = lines.Length };
        }

        if (lines.Length == 0)
        {
            return new LoaiMonImportResultDTO { SoBoQua = 1 };
        }

        var dsLoaiNhap = new List<LoaiMonDTO>();
        var soBoQua = 0;
        var startIndex = lines[0].Contains("TenLoai", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

        for (var i = startIndex; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            var cot = SplitCsvLine(lines[i]);
            var tenLoai = cot.Count >= 2 ? cot[1].Trim() : cot[0].Trim();
            var moTa = string.Empty;

            if (cot.Count >= 4)
            {
                moTa = cot[3].Trim();
            }
            else if (cot.Count == 3 && !int.TryParse(cot[2], out _))
            {
                moTa = cot[2].Trim();
            }

            if (string.IsNullOrWhiteSpace(tenLoai))
            {
                soBoQua++;
                continue;
            }

            dsLoaiNhap.Add(new LoaiMonDTO
            {
                TenLoai = tenLoai,
                MoTa = moTa
            });
        }

        var result = _loaiMonDAL.NhapLoaiMon(dsLoaiNhap);
        result.SoBoQua += soBoQua;
        return result;
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
