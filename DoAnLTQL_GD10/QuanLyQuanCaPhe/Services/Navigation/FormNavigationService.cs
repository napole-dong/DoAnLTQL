using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Forms;
using QuanLyQuanCaPhe.Services.Auth;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyQuanCaPhe.Services.Navigation;

public static class FormNavigationService
{
    public static void Navigate<TForm>(
        Form currentForm,
        IPermissionService permissionBUS,
        string feature,
        Func<TForm> formFactory,
        Action? onCurrentFormReactivated = null,
        bool skipNavigation = false,
        string deniedMessage = "Ban khong co quyen truy cap chuc nang nay.")
        where TForm : Form
    {
        if (skipNavigation || currentForm.IsDisposed || currentForm.Disposing)
        {
            return;
        }

        try
        {
            permissionBUS.EnsurePermission(feature, PermissionActions.View, deniedMessage);
        }
        catch (UnauthorizedAccessException ex)
        {
            MessageBox.Show(ex.Message, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var mainForm = Application.OpenForms
            .Cast<Form>()
            .OfType<frmBanHang>()
            .FirstOrDefault(form => !form.IsDisposed);

        if (mainForm == null)
        {
            if (currentForm is frmBanHang currentMainForm)
            {
                mainForm = currentMainForm;
            }
            else
            {
                MessageBox.Show("Khong tim thay man hinh chinh de dieu huong.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        if (typeof(TForm) == typeof(frmBanHang))
        {
            mainForm.HienThiManHinhBanHang();
        }
        else
        {
            mainForm.OpenChildForm(formFactory());
        }

        if (!ReferenceEquals(currentForm, mainForm) && !currentForm.IsDisposed && !currentForm.Disposing)
        {
            currentForm.Close();
        }

        onCurrentFormReactivated?.Invoke();
    }
}
