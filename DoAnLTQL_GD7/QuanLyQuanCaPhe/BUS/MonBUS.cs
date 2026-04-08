using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class MonBUS
{
    private readonly MonDAL _monDAL = new();
    private readonly PermissionBUS _permissionBUS = new();

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

        var tuKhoa = ($"{textSearch} {textTimMon}").Trim();
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

        var validation = KiemTraMon(monDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi);
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
            return (false, "Bạn không có quyền xóa món.");
        }

        if (monId <= 0)
        {
            return (false, "Vui lòng chọn món cần xóa.");
        }

        if (_monDAL.MonDaPhatSinhHoaDon(monId))
        {
            return (false, "Món đã phát sinh hóa đơn, không thể xóa.");
        }

        var daXoa = _monDAL.XoaMon(monId);
        return daXoa
            ? (true, "Xóa món thành công.")
            : (false, "Không tìm thấy món để xóa.");
    }

    public MonImportResultDTO NhapMonTuCsv(string[] lines)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create)
            && !_permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update))
        {
            return new MonImportResultDTO { SoBoQua = lines.Length };
        }

        if (lines.Length == 0)
        {
            return new MonImportResultDTO { SoBoQua = 1 };
        }

        var dsLoai = LayDanhSachLoaiMon();
        var dsMonNhap = new List<MonDTO>();
        var soBoQua = 0;

        var startIndex = lines[0].Contains("TenMon", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        for (var i = startIndex; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            var cot = SplitCsvLine(lines[i]);
            if (cot.Count < 3)
            {
                soBoQua++;
                continue;
            }

            string tenMon;
            string loaiMonText;
            string donGiaText;
            string trangThai;
            string moTa;
            string hinhAnh;

            if (cot.Count >= 8)
            {
                tenMon = cot[1].Trim();
                loaiMonText = cot[2].Trim();
                donGiaText = cot[4].Trim();
                trangThai = cot[5].Trim();
                moTa = cot[6].Trim();
                hinhAnh = cot[7].Trim();
            }
            else if (cot.Count >= 7)
            {
                tenMon = cot[1].Trim();
                loaiMonText = cot[2].Trim();
                donGiaText = cot[4].Trim();
                trangThai = string.Empty;
                moTa = cot[5].Trim();
                hinhAnh = cot[6].Trim();
            }
            else
            {
                tenMon = cot[0].Trim();
                loaiMonText = cot[1].Trim();
                donGiaText = cot[2].Trim();
                trangThai = string.Empty;
                moTa = cot.Count > 3 ? cot[3].Trim() : string.Empty;
                hinhAnh = cot.Count > 4 ? cot[4].Trim() : string.Empty;
            }

            if (string.IsNullOrWhiteSpace(tenMon) || !decimal.TryParse(donGiaText, out var donGia) || donGia < 0)
            {
                soBoQua++;
                continue;
            }

            var loaiMonId = 0;
            if (int.TryParse(loaiMonText, out var loaiId))
            {
                loaiMonId = loaiId;
            }
            else
            {
                loaiMonId = dsLoai.FirstOrDefault(x =>
                    string.Equals(x.TenLoai, loaiMonText, StringComparison.OrdinalIgnoreCase))?.ID ?? 0;
            }

            if (loaiMonId == 0)
            {
                soBoQua++;
                continue;
            }

            dsMonNhap.Add(new MonDTO
            {
                TenMon = tenMon,
                LoaiMonID = loaiMonId,
                DonGia = donGia,
                TrangThai = ParseTrangThaiText(trangThai, donGia),
                MoTa = moTa,
                HinhAnh = hinhAnh
            });
        }

        var result = _monDAL.NhapDanhSachMon(dsMonNhap);
        result.SoBoQua += soBoQua;
        return result;
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

        return (true, string.Empty);
    }

    private static int ParseTrangThaiText(string? trangThai, decimal donGia)
    {
        if (string.IsNullOrWhiteSpace(trangThai))
        {
            return donGia <= 0 ? 0 : 1;
        }

        if (int.TryParse(trangThai, out var trangThaiInt) && trangThaiInt is >= 0 and <= 2)
        {
            return trangThaiInt;
        }

        if (trangThai.Equals("Đang kinh doanh", StringComparison.OrdinalIgnoreCase))
        {
            return 1;
        }

        if (trangThai.Equals("Ngừng bán", StringComparison.OrdinalIgnoreCase)
            || trangThai.Equals("Ngừng kinh doanh", StringComparison.OrdinalIgnoreCase))
        {
            return 0;
        }

        if (trangThai.Equals("Tạm ngừng", StringComparison.OrdinalIgnoreCase))
        {
            return 2;
        }

        return donGia <= 0 ? 0 : 1;
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
