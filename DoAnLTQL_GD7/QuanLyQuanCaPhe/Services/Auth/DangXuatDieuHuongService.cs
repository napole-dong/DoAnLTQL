using System.Linq;
using System.Windows.Forms;

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