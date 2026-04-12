using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Presenters;

public sealed record BanHangOrderViewModel(
    string OrderMeta,
    string TableInfo,
    decimal TongTien,
    bool CoTheLuuBan,
    bool CoTheThanhToan);

public sealed class BanHangPresenter
{
    public bool ShouldFallbackToUnfilteredData(string tuKhoa, int soLuongKetQua)
    {
        return !string.IsNullOrWhiteSpace(tuKhoa) && soLuongKetQua == 0;
    }

    public BanHangOrderViewModel BuildOrderViewModel(BanHangTrangThaiPhieuDTO trangThaiPhieu, bool cheDoMangDi)
    {
        var tenBan = cheDoMangDi ? "Mang đi" : trangThaiPhieu.TenBan;
        var trangThaiBan = BanHangBUS.ChuyenTrangThaiBan(trangThaiPhieu.TrangThaiBan, trangThaiPhieu.TrangThaiHoaDon);
        var hoaDonDaThanhToan = trangThaiPhieu.TrangThaiHoaDon == (int)HoaDonTrangThai.Paid;
        var tenKhachHang = string.IsNullOrWhiteSpace(trangThaiPhieu.TenKhachHang)
            ? "Khách lẻ"
            : trangThaiPhieu.TenKhachHang;

        var orderMeta = cheDoMangDi
            ? BuildMangDiMeta(tenBan, tenKhachHang, trangThaiPhieu.SoMonChoGoi)
            : BuildTaiQuanMeta(tenBan, trangThaiBan, tenKhachHang, trangThaiPhieu.SoMonChoGoi);

        var tableInfo = cheDoMangDi
            ? BuildMangDiTableInfo(trangThaiPhieu.TongMon)
            : BuildTaiQuanTableInfo(tenBan, trangThaiPhieu.TongMon, hoaDonDaThanhToan);

        return new BanHangOrderViewModel(
            orderMeta,
            tableInfo,
            trangThaiPhieu.TongTien,
            CoTheLuuBan: !cheDoMangDi && !hoaDonDaThanhToan,
            CoTheThanhToan: !hoaDonDaThanhToan);
    }

    private static string BuildMangDiMeta(string tenBan, string tenKhachHang, int soMonChoGoi)
    {
        return soMonChoGoi > 0
            ? $"{tenBan} • {soMonChoGoi} món chờ gọi • {tenKhachHang}"
            : $"{tenBan} • {tenKhachHang}";
    }

    private static string BuildTaiQuanMeta(string tenBan, string trangThaiBan, string tenKhachHang, int soMonChoGoi)
    {
        return soMonChoGoi > 0
            ? $"{tenBan} • {trangThaiBan} • {soMonChoGoi} món chờ gọi • {tenKhachHang}"
            : $"{tenBan} • {trangThaiBan} • {tenKhachHang}";
    }

    private static string BuildMangDiTableInfo(int tongMon)
    {
        return tongMon > 0
            ? $"Đơn mang đi đang có {tongMon} món"
            : "Chọn món để tạo đơn mang đi";
    }

    private static string BuildTaiQuanTableInfo(string tenBan, int tongMon, bool hoaDonDaThanhToan)
    {
        if (hoaDonDaThanhToan)
        {
            return $"{tenBan} đã thanh toán. Chuột phải vào bàn để dọn bàn.";
        }

        return tongMon > 0
            ? $"{tenBan} đang có {tongMon} món phục vụ"
            : "Chọn bàn để xem món đang phục vụ";
    }
}
