using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class LoaiMonBUS
{
    private readonly LoaiMonDAL _loaiMonDAL = new();

    public List<LoaiMonDTO> LayDanhSachLoai(string? textSearch, string? textTimLoai)
    {
        var tuKhoa = ($"{textSearch} {textTimLoai}").Trim();
        return _loaiMonDAL.GetDanhSachLoai(tuKhoa);
    }

    public int LayMaLoaiTiepTheo()
    {
        return _loaiMonDAL.GetNextLoaiMonId();
    }

    public (bool ThanhCong, string ThongBao, LoaiMonDTO? LoaiMoi) ThemLoai(string tenLoai)
    {
        tenLoai = tenLoai.Trim();
        if (string.IsNullOrWhiteSpace(tenLoai))
        {
            return (false, "Tên loại món không được để trống.", null);
        }

        if (_loaiMonDAL.TenLoaiDaTonTai(tenLoai))
        {
            return (false, "Tên loại món đã tồn tại.", null);
        }

        var loai = _loaiMonDAL.ThemLoai(tenLoai);
        return (true, "Thêm loại món thành công.", loai);
    }

    public (bool ThanhCong, string ThongBao) CapNhatLoai(int id, string tenLoai)
    {
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

        var daCapNhat = _loaiMonDAL.CapNhatLoai(id, tenLoai);
        return daCapNhat
            ? (true, "Cập nhật loại món thành công.")
            : (false, "Không tìm thấy loại món để cập nhật.");
    }

    public (bool ThanhCong, string ThongBao) XoaLoai(int id)
    {
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

    public LoaiMonImportResultDTO NhapLoaiMonTuCsv(string[] lines)
    {
        if (lines.Length == 0)
        {
            return new LoaiMonImportResultDTO { SoBoQua = 1 };
        }

        var dsTenLoai = new List<string>();
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
            if (string.IsNullOrWhiteSpace(tenLoai))
            {
                soBoQua++;
                continue;
            }

            dsTenLoai.Add(tenLoai);
        }

        var result = _loaiMonDAL.NhapLoaiMon(dsTenLoai);
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
