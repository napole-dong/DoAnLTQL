using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.DependencyInjection;

namespace QuanLyQuanCaPhe.BUS;

public class DangNhapBUS : IDangNhapService
{
    private readonly IDangNhapRepository _dangNhapDAL;
    private readonly IActivityLogWriter _activityLogWriter;

    public DangNhapBUS(
        IDangNhapRepository? dangNhapDAL = null,
        IActivityLogWriter? activityLogWriter = null)
    {
        _dangNhapDAL = dangNhapDAL ?? new DangNhapDAL();
        _activityLogWriter = AppServiceProvider.Resolve(activityLogWriter, () => new ActivityLogService());
    }

    public (bool ThanhCong, string ThongBao, string TruongLoi, ThongTinDangNhapDTO? ThongTinDangNhap) DangNhap(string tenDangNhap, string matKhau)
    {
        var tenDangNhapChuan = BusInputHelper.NormalizeText(tenDangNhap);
        var matKhauChuan = BusInputHelper.NormalizeText(matKhau);

        if (tenDangNhapChuan.Length == 0)
        {
            GhiDangNhapThatBai(tenDangNhap, "Tên đăng nhập trống.");
            return (false, "Tên đăng nhập không được để trống.", "TenDangNhap", null);
        }

        if (matKhauChuan.Length == 0)
        {
            GhiDangNhapThatBai(tenDangNhapChuan, "Mật khẩu trống.");
            return (false, "Mật khẩu không được để trống.", "MatKhau", null);
        }

        var thongTinDangNhap = _dangNhapDAL.XacThucDangNhap(tenDangNhapChuan, matKhauChuan);
        if (thongTinDangNhap != null)
        {
            _activityLogWriter.Log(
                thongTinDangNhap.UserId,
                AuditActions.LoginSuccess,
                "Auth",
                thongTinDangNhap.UserId.ToString(),
                $"Người dùng {thongTinDangNhap.TenDangNhap} đăng nhập thành công.",
                oldValue: null,
                newValue: new
                {
                    Username = thongTinDangNhap.TenDangNhap,
                    thongTinDangNhap.RoleId,
                    thongTinDangNhap.QuyenHan
                },
                performedBy: thongTinDangNhap.TenDangNhap);

            return (true, "Đăng nhập thành công.", string.Empty, thongTinDangNhap);
        }

        if (!_dangNhapDAL.TonTaiTenDangNhap(tenDangNhapChuan))
        {
            GhiDangNhapThatBai(tenDangNhapChuan, "Tên đăng nhập không tồn tại.");
            return (false, "Tên đăng nhập không tồn tại.", "TenDangNhap", null);
        }

        if (!_dangNhapDAL.LaTaiKhoanHoatDong(tenDangNhapChuan))
        {
            GhiDangNhapThatBai(tenDangNhapChuan, "Tài khoản bị khóa.");
            return (false, "Tài khoản đang bị khóa. Vui lòng liên hệ quản trị viên.", "TenDangNhap", null);
        }

        GhiDangNhapThatBai(tenDangNhapChuan, "Sai mật khẩu.");
        return (false, "Mật khẩu không chính xác.", "MatKhau", null);
    }

    private void GhiDangNhapThatBai(string? tenDangNhap, string lyDo)
    {
        var username = BusInputHelper.NormalizeText(tenDangNhap);
        if (username.Length == 0)
        {
            username = "unknown";
        }

        _activityLogWriter.Log(
            userId: null,
            action: AuditActions.LoginFailed,
            entity: "Auth",
            entityId: username,
            description: $"Đăng nhập thất bại cho tài khoản {username}. Lý do: {lyDo}",
            oldValue: null,
            newValue: new
            {
                Username = username,
                Reason = lyDo
            },
            performedBy: username);
    }
}
