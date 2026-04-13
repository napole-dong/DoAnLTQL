using System.Linq;
using System.Windows.Forms;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.DependencyInjection;

namespace QuanLyQuanCaPhe.Services.Auth;

public static class DangXuatDieuHuongService
{
    private static readonly object SyncLock = new();
    private static bool _daYeuCauDangNhapLai;

    public static bool DaYeuCauDangNhapLai
    {
        get
        {
            lock (SyncLock)
            {
                return _daYeuCauDangNhapLai;
            }
        }
    }

    public static void DatLaiYeuCauDangNhapLai()
    {
        lock (SyncLock)
        {
            _daYeuCauDangNhapLai = false;
        }
    }

    public static void DangXuatVaQuayVeDangNhap()
    {
        lock (SyncLock)
        {
            _daYeuCauDangNhapLai = true;
        }

        var currentUser = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        var activityLogWriter = AppServiceProvider.TryGetService<IActivityLogWriter>();
        if (activityLogWriter != null)
        {
            activityLogWriter.Log(
                currentUser?.UserId,
                AuditActions.Logout,
                "Auth",
                currentUser?.UserId.ToString(),
                string.IsNullOrWhiteSpace(currentUser?.TenDangNhap)
                    ? "Người dùng đăng xuất."
                    : $"Người dùng {currentUser.TenDangNhap} đã đăng xuất.",
                oldValue: new
                {
                    currentUser?.UserId,
                    currentUser?.TenDangNhap,
                    currentUser?.RoleId,
                    currentUser?.QuyenHan
                },
                newValue: null,
                performedBy: currentUser?.TenDangNhap);
        }

        NguoiDungHienTaiService.XoaNguoiDungDangNhap();

        var rootForm = Application.OpenForms
            .Cast<Form>()
            .FirstOrDefault(form => form.Owner == null);

        if (rootForm != null && !rootForm.IsDisposed)
        {
            rootForm.Close();
            return;
        }

        Application.Exit();
    }
}