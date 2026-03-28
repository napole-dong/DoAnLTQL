using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.Mon
{
    public class MonInputValidator
    {
        public MonValidationResult Validate(string tenMon, object? loaiMonValue, string donGiaText, string moTa, string duongDanAnh)
        {
            if (string.IsNullOrWhiteSpace(tenMon))
            {
                return MonValidationResult.ThatBai("Tên món không được để trống.", MonInputField.TenMon);
            }

            if (loaiMonValue is not int loaiId)
            {
                return MonValidationResult.ThatBai("Vui lòng chọn loại món.", MonInputField.LoaiMon);
            }

            if (!int.TryParse(donGiaText.Trim(), out var donGia) || donGia < 0)
            {
                return MonValidationResult.ThatBai("Đơn giá không hợp lệ.", MonInputField.DonGia);
            }

            var mon = new MonDTO
            {
                TenMon = tenMon.Trim(),
                LoaiMonID = loaiId,
                DonGia = donGia,
                MoTa = moTa.Trim(),
                HinhAnh = string.IsNullOrWhiteSpace(duongDanAnh) ? null : duongDanAnh.Trim()
            };

            return MonValidationResult.ThanhCong(mon);
        }
    }
}
