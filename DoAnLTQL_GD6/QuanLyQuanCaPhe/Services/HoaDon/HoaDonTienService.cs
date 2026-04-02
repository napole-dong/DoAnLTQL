namespace QuanLyQuanCaPhe.Services.HoaDon
{
    public class HoaDonTienService
    {
        public int ChuyenTextTienThanhSo(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            var digits = new string(input.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out var soTien) ? soTien : 0;
        }

        public string DinhDangTien(int soTien)
        {
            return $"{soTien:N0}đ";
        }
    }
}
