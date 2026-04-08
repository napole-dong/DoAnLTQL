using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.Auth;

public static class NguoiDungHienTaiService
{
    private static readonly object SyncLock = new();
    private static ThongTinDangNhapDTO? _nguoiDungDangNhap;

    public static void DatNguoiDungDangNhap(ThongTinDangNhapDTO thongTinDangNhap)
    {
        lock (SyncLock)
        {
            _nguoiDungDangNhap = thongTinDangNhap;
        }
    }

    public static ThongTinDangNhapDTO? LayNguoiDungDangNhap()
    {
        lock (SyncLock)
        {
            return _nguoiDungDangNhap;
        }
    }

    public static void XoaNguoiDungDangNhap()
    {
        lock (SyncLock)
        {
            _nguoiDungDangNhap = null;
        }
    }
}
