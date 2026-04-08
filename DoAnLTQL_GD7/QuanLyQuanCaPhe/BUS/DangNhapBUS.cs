using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class DangNhapBUS
{
    private readonly DangNhapDAL _dangNhapDAL = new();

    public (bool ThanhCong, string ThongBao, string TruongLoi, ThongTinDangNhapDTO? ThongTinDangNhap) DangNhap(string tenDangNhap, string matKhau)
    {
        var tenDangNhapChuan = tenDangNhap.Trim();
        var matKhauChuan = matKhau.Trim();

        if (string.IsNullOrWhiteSpace(tenDangNhapChuan))
        {
            return (false, "Tên đăng nhập không được để trống.", "TenDangNhap", null);
        }

        if (string.IsNullOrWhiteSpace(matKhauChuan))
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
