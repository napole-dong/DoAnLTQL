namespace QuanLyQuanCaPhe.Services.HoaDon
{
    public class HoaDonTienService
    {
        public decimal ChuyenTextTienThanhSo(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            var digits = new string(input.Where(char.IsDigit).ToArray());
            return decimal.TryParse(digits, out var soTien) ? soTien : 0;
        }

        public string DinhDangTien(decimal soTien)
        {
            return $"{soTien:N0}đ";
        }
    }
}
