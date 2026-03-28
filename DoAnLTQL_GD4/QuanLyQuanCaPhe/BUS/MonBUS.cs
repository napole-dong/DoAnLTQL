using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class MonBUS
{
    private readonly MonDAL _monDAL = new();

    public List<LoaiMonDTO> LayDanhSachLoaiMon()
    {
        return _monDAL.GetLoaiMon();
    }

    public List<MonDTO> LayDanhSachMon(string? textSearch, string? textTimMon)
    {
        var tuKhoa = ($"{textSearch} {textTimMon}").Trim();
        return _monDAL.GetDanhSachMon(tuKhoa);
    }

    public int LayMaMonTiepTheo()
    {
        return _monDAL.GetNextMonId();
    }

    public (bool ThanhCong, string ThongBao, MonDTO? MonMoi) ThemMon(MonDTO monDTO)
    {
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
            string moTa;
            string hinhAnh;

            if (cot.Count >= 7)
            {
                tenMon = cot[1].Trim();
                loaiMonText = cot[2].Trim();
                donGiaText = cot[4].Trim();
                moTa = cot[5].Trim();
                hinhAnh = cot[6].Trim();
            }
            else
            {
                tenMon = cot[0].Trim();
                loaiMonText = cot[1].Trim();
                donGiaText = cot[2].Trim();
                moTa = cot.Count > 3 ? cot[3].Trim() : string.Empty;
                hinhAnh = cot.Count > 4 ? cot[4].Trim() : string.Empty;
            }

            if (string.IsNullOrWhiteSpace(tenMon) || !int.TryParse(donGiaText, out var donGia) || donGia < 0)
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

        if (monDTO.DonGia < 0)
        {
            return (false, "Đơn giá không hợp lệ.");
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
