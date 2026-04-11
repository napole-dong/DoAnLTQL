using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class ThongKeBUS
{
    private readonly ThongKeDAL _thongKeDAL = new();
    private readonly PermissionBUS _permissionBUS = new();

    public List<ThongKeHoaDonDTO> LayDanhSachHoaDonDaThanhToan(DateTime tuNgay, DateTime denNgay, string? tuKhoa)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View))
        {
            return new List<ThongKeHoaDonDTO>();
        }

        var (tuNgayDaChuanHoa, denNgayDaChuanHoa) = ChuanHoaKhoangNgay(tuNgay, denNgay);
        return _thongKeDAL.LayDanhSachHoaDonDaThanhToan(
            tuNgayDaChuanHoa,
            denNgayDaChuanHoa,
            BusInputHelper.NormalizeNullableText(tuKhoa));
    }

    public List<ThongKeTopMonDTO> LayTopMonBanChay(DateTime tuNgay, DateTime denNgay, string? tuKhoa, int soLuongTop = 10)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View))
        {
            return new List<ThongKeTopMonDTO>();
        }

        var (tuNgayDaChuanHoa, denNgayDaChuanHoa) = ChuanHoaKhoangNgay(tuNgay, denNgay);
        var soLuongTopHopLe = Math.Clamp(soLuongTop, 1, 50);

        return _thongKeDAL.LayTopMonBanChay(
            tuNgayDaChuanHoa,
            denNgayDaChuanHoa,
            BusInputHelper.NormalizeNullableText(tuKhoa),
            soLuongTopHopLe);
    }

    private static (DateTime TuNgay, DateTime DenNgay) ChuanHoaKhoangNgay(DateTime tuNgay, DateTime denNgay)
    {
        var tuNgayDate = tuNgay.Date;
        var denNgayDate = denNgay.Date;

        if (tuNgayDate > denNgayDate)
        {
            return (denNgayDate, tuNgayDate);
        }

        return (tuNgayDate, denNgayDate);
    }
}
