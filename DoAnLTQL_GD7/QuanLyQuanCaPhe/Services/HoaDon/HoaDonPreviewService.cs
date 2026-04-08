using System.Text;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.HoaDon
{
    public class HoaDonPreviewService
    {
        public string TaoNoiDungXemTruoc(HoaDonDTO hoaDon)
        {
            var noiDung = new StringBuilder();
            noiDung.AppendLine("CA PHE NAPOLE");
            noiDung.AppendLine("188 Le Loi, Quan 1, TP.HCM");
            noiDung.AppendLine(new string('-', 34));
            noiDung.AppendLine($"Hóa đơn: {hoaDon.MaHoaDonHienThi}");
            noiDung.AppendLine($"Ngày lập: {hoaDon.NgayLapHienThi}");
            noiDung.AppendLine($"Bàn/Khách: {hoaDon.BanKhachHienThi}");
            noiDung.AppendLine($"Nhân viên: {hoaDon.TenNhanVien}");
            noiDung.AppendLine(new string('-', 34));

            foreach (var item in hoaDon.ChiTiet)
            {
                noiDung.AppendLine($"{item.TenMon} x{item.SoLuong}  {item.ThanhTienHienThi}");
            }

            noiDung.AppendLine(new string('-', 34));
            noiDung.AppendLine($"Tổng tiền: {hoaDon.TongTienHienThi}");
            noiDung.AppendLine($"Trạng thái: {hoaDon.TrangThaiText}");

            return noiDung.ToString();
        }
    }
}
