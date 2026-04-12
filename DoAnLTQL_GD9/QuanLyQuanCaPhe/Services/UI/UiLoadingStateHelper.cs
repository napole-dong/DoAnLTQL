namespace QuanLyQuanCaPhe.Services.UI;

public static class UiLoadingStateHelper
{
    public static void Apply(Control owner, bool isLoading, params Control[] controlsToToggle)
    {
        owner.UseWaitCursor = isLoading;

        foreach (var control in controlsToToggle)
        {
            control.Enabled = !isLoading;
        }
    }
}
