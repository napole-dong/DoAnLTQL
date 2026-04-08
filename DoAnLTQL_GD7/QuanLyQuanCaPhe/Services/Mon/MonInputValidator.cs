using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.Mon
{
    public class MonInputValidator
    {
        public MonValidationResult Validate(string tenMon, object? loaiMonValue, object? trangThaiValue, string donGiaText, string moTa, string duongDanAnh)
        {
            if (string.IsNullOrWhiteSpace(tenMon))
            {
                return MonValidationResult.ThatBai("Tên món không được để trống.", MonInputField.TenMon);
            }

            if (loaiMonValue is not int loaiId)
            {
                return MonValidationResult.ThatBai("Vui lòng chọn loại món.", MonInputField.LoaiMon);
            }

            if (!decimal.TryParse(donGiaText.Trim(), out var donGia) || donGia < 0)
            {
                return MonValidationResult.ThatBai("Đơn giá không hợp lệ.", MonInputField.DonGia);
            }

            if (!TryParseTrangThai(trangThaiValue, out var trangThai))
            {
                return MonValidationResult.ThatBai("Vui lòng chọn trạng thái món.", MonInputField.None);
            }

            var mon = new MonDTO
            {
                TenMon = tenMon.Trim(),
                LoaiMonID = loaiId,
                DonGia = donGia,
                TrangThai = trangThai,
                MoTa = moTa.Trim(),
                HinhAnh = string.IsNullOrWhiteSpace(duongDanAnh) ? null : duongDanAnh.Trim()
            };

            return MonValidationResult.ThanhCong(mon);
        }

    private static bool TryParseTrangThai(object? trangThaiValue, out int trangThai)
    {
        var raw = trangThaiValue?.ToString()?.Trim();
        if (string.IsNullOrWhiteSpace(raw))
        {
            trangThai = 1;
            return false;
        }

        if (int.TryParse(raw, out var parsed) && parsed is >= 0 and <= 2)
        {
            trangThai = parsed;
            return true;
        }

        if (raw.Equals("Đang kinh doanh", StringComparison.OrdinalIgnoreCase))
        {
            trangThai = 1;
            return true;
        }

        if (raw.Equals("Ngừng bán", StringComparison.OrdinalIgnoreCase)
            || raw.Equals("Ngừng kinh doanh", StringComparison.OrdinalIgnoreCase))
        {
            trangThai = 0;
            return true;
        }

        if (raw.Equals("Tạm ngừng", StringComparison.OrdinalIgnoreCase))
        {
            trangThai = 2;
            return true;
        }

        trangThai = 1;
        return false;
    }
    }
}
