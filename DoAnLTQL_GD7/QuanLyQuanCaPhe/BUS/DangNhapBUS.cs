using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class DangNhapBUS
{
    private readonly DangNhapDAL _dangNhapDAL = new();

    public (bool ThanhCong, string ThongBao, string TruongLoi, ThongTinDangNhapDTO? ThongTinDangNhap) DangNhap(string tenDangNhap, string matKhau)
    {
        var tenDangNhapChuan = BusInputHelper.NormalizeText(tenDangNhap);
        var matKhauChuan = BusInputHelper.NormalizeText(matKhau);

        if (tenDangNhapChuan.Length == 0)
        {
            return (false, "Tên đăng nhập không được để trống.", "TenDangNhap", null);
        }

        if (matKhauChuan.Length == 0)
        {
            return (false, "Mật khẩu không được để trống.", "MatKhau", null);
        }

        var thongTinDangNhap = _dangNhapDAL.XacThucDangNhap(tenDangNhapChuan, matKhauChuan);
        if (thongTinDangNhap != null)
        {
            return (true, "Đăng nhập thành công.", string.Empty, thongTinDangNhap);
        }

        if (!_dangNhapDAL.TonTaiTenDangNhap(tenDangNhapChuan))
        {
            return (false, "Tên đăng nhập không tồn tại.", "TenDangNhap", null);
        }

        if (!_dangNhapDAL.LaTaiKhoanHoatDong(tenDangNhapChuan))
        {
            return (false, "Tài khoản đang bị khóa. Vui lòng liên hệ quản trị viên.", "TenDangNhap", null);
        }

        return (false, "Mật khẩu không chính xác.", "MatKhau", null);
    }
}
