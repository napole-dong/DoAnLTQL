using QuanLyQuanCaPhe.Forms;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace QuanLyQuanCaPhe.Services.Navigation;

public static class ChildFormRuntimePolicy
{
    private const string EnforceEmbeddedChildFormsKey = "EnforceEmbeddedChildForms";

    public static bool TryBlockStandalone(Form form, bool isEmbedded, string childFormName)
    {
        if (isEmbedded || form is frmBanHang || !ShouldEnforceEmbeddedMode())
        {
            return false;
        }

        MessageBox.Show(
            $"Man hinh '{childFormName}' chi duoc mo ben trong man hinh Ban hang.",
            "Thong bao",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        form.BeginInvoke(new Action(form.Close));
        return true;
    }

    private static bool ShouldEnforceEmbeddedMode()
    {
        var value = ConfigurationManager.AppSettings[EnforceEmbeddedChildFormsKey];
        return bool.TryParse(value, out var enabled) && enabled;
    }
}
