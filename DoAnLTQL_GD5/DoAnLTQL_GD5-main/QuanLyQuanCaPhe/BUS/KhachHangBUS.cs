using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class KhachHangBUS
{
    private readonly KhachHangDAL _khachHangDAL = new();

    public List<KhachHangDTO> LayDanhSachKhach(string? textTimKhach)
    {
        return _khachHangDAL.GetDanhSachKhach(textTimKhach?.Trim());
    }

    public int LayMaKhachTiepTheo()
    {
        return _khachHangDAL.GetNextKhachHangId();
    }

    public (bool ThanhCong, string ThongBao, KhachHangDTO? KhachMoi) ThemKhach(KhachHangDTO khachDTO)
    {
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
        if (khachDTO.ID <= 0)
        {
            return (false, "Vui lòng chọn khách hàng cần cập nhật.");
        }

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

            var cot = SplitCsvLine(lines[i]);

            string hoVaTen;
            string dienThoai;
            string diaChi;

            if (cot.Count >= 4)
            {
                hoVaTen = cot[1].Trim();
                dienThoai = cot[2].Trim();
                diaChi = cot[3].Trim();
            }
            else
            {
                hoVaTen = cot.ElementAtOrDefault(0)?.Trim() ?? string.Empty;
                dienThoai = cot.ElementAtOrDefault(1)?.Trim() ?? string.Empty;
                diaChi = cot.ElementAtOrDefault(2)?.Trim() ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(hoVaTen))
            {
                soBoQua++;
                continue;
            }

            if (!string.IsNullOrWhiteSpace(dienThoai) && !LaSoDienThoaiHopLe(dienThoai))
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

        var result = _khachHangDAL.NhapDanhSachKhach(dsKhach);
        return (result.SoThemMoi, result.SoCapNhat, result.SoBoQua + soBoQua);
    }

    private static (bool HopLe, string ThongBaoLoi) KiemTraThongTin(KhachHangDTO khachDTO)
    {
        if (string.IsNullOrWhiteSpace(khachDTO.HoVaTen))
        {
            return (false, "Họ và tên không được để trống.");
        }

        if (!string.IsNullOrWhiteSpace(khachDTO.DienThoai) && !LaSoDienThoaiHopLe(khachDTO.DienThoai))
        {
            return (false, "Số điện thoại không hợp lệ.");
        }

        return (true, string.Empty);
    }

    private static bool LaSoDienThoaiHopLe(string dienThoai)
    {
        return dienThoai.All(char.IsDigit) && dienThoai.Length is >= 9 and <= 11;
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
//hello word