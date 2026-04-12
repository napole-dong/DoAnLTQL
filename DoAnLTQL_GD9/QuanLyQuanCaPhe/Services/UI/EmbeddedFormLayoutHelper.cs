namespace QuanLyQuanCaPhe.Services.UI;

internal static class EmbeddedFormLayoutHelper
{
    public static void UseContentOnlyLayout(Panel panelMain, Panel panelSidebar, Panel panelTopbar)
    {
        ArgumentNullException.ThrowIfNull(panelMain);
        ArgumentNullException.ThrowIfNull(panelSidebar);
        ArgumentNullException.ThrowIfNull(panelTopbar);

        panelSidebar.Visible = false;
        panelSidebar.Enabled = false;
        panelSidebar.Dock = DockStyle.None;
        panelSidebar.Width = 0;

        panelTopbar.Visible = false;
        panelTopbar.Enabled = false;
        panelTopbar.Dock = DockStyle.None;
        panelTopbar.Height = 0;

        panelMain.Dock = DockStyle.Fill;
        panelMain.Location = Point.Empty;
        panelMain.Margin = Padding.Empty;
        panelMain.BringToFront();

        panelMain.Parent?.PerformLayout();
    }
}