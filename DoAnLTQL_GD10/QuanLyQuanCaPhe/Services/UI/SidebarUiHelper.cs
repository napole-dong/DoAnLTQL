using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Permission;

namespace QuanLyQuanCaPhe.Services.UI;

internal static class SidebarUiHelper
{
    private const string BrandTitleText = "☕ Cà phê Pro";
    private const string BrandSubtitleText = "Coffee Management";
    private const string SalesButtonText = "🧾  Bán hàng";
    private const string TableManagementButtonText = "🪑  Quản lý bàn";
    private const string MenuManagementButtonText = "☕  Quản lý món";
    private const string RecipeButtonText = "🧪  Công thức";
    private const string InventoryButtonText = "📦  Quản lý kho";
    private const string InvoiceButtonText = "🧾  Hóa đơn";
    private const string CustomerButtonText = "👤  Khách hàng";
    private const string EmployeeButtonText = "🧑‍💼  Nhân viên";
    private const string ReportButtonText = "📈  Thống kê";
    private const string AuditLogButtonText = "🛡  Audit log";
    private static readonly Color ActiveBackColor = Color.FromArgb(108, 74, 54);
    private static readonly Color ActiveForeColor = Color.White;
    private static readonly Color InactiveBackColor = Color.FromArgb(52, 36, 29);
    private static readonly Color InactiveForeColor = Color.FromArgb(220, 214, 207);
    private static readonly Color HoverBackColor = Color.FromArgb(76, 53, 41);
    private static readonly Color PressedBackColor = Color.FromArgb(92, 64, 49);
    private static readonly Font ActiveFont = new("Segoe UI Semibold", 10.5F, FontStyle.Bold);
    private static readonly Font InactiveFont = new("Segoe UI", 10F, FontStyle.Regular);
    private static readonly Size SidebarButtonSize = new(230, 48);
    private static readonly Padding SidebarButtonPadding = new(20, 0, 0, 0);
    private static readonly Padding SidebarButtonMargin = new(0);

    public static (Button btnKhachHang, Button btnQuanLyKho, Button btnAuditLog) EnsureUnifiedSidebarMenu(
        FlowLayoutPanel flowSidebarMenu,
        params Button[] menuButtons)
    {
        ArgumentNullException.ThrowIfNull(flowSidebarMenu);

        var btnBanHang = FindSidebarButton(flowSidebarMenu, "btnBanHang", menuButtons)
            ?? throw new InvalidOperationException("Sidebar is missing btnBanHang.");
        var btnQuanLyBan = FindSidebarButton(flowSidebarMenu, "btnQuanLyBan", menuButtons)
            ?? throw new InvalidOperationException("Sidebar is missing btnQuanLyBan.");
        var btnQuanLyMon = FindSidebarButton(flowSidebarMenu, "btnQuanLyMon", menuButtons)
            ?? throw new InvalidOperationException("Sidebar is missing btnQuanLyMon.");
        var btnCongThuc = FindSidebarButton(flowSidebarMenu, "btnCongThuc", menuButtons)
            ?? throw new InvalidOperationException("Sidebar is missing btnCongThuc.");
        var btnHoaDon = FindSidebarButton(flowSidebarMenu, "btnHoaDon", menuButtons)
            ?? throw new InvalidOperationException("Sidebar is missing btnHoaDon.");
        var btnNhanVien = FindSidebarButton(flowSidebarMenu, "btnNhanVien", menuButtons)
            ?? throw new InvalidOperationException("Sidebar is missing btnNhanVien.");
        var btnThongKe = FindSidebarButton(flowSidebarMenu, "btnThongKe", menuButtons)
            ?? throw new InvalidOperationException("Sidebar is missing btnThongKe.");

        var btnQuanLyKho = FindSidebarButton(flowSidebarMenu, "btnQuanLyKho", menuButtons)
                   ?? CreateSidebarButton("btnQuanLyKho", InventoryButtonText);
        var btnKhachHang = FindSidebarButton(flowSidebarMenu, "btnKhachHang", menuButtons)
                          ?? CreateSidebarButton("btnKhachHang", CustomerButtonText);
        var btnAuditLog = FindSidebarButton(flowSidebarMenu, "btnAuditLog", menuButtons)
                         ?? CreateSidebarButton("btnAuditLog", AuditLogButtonText);

        btnBanHang.Text = SalesButtonText;
        btnQuanLyBan.Text = TableManagementButtonText;
        btnQuanLyMon.Text = MenuManagementButtonText;
        btnCongThuc.Text = RecipeButtonText;
        btnHoaDon.Text = InvoiceButtonText;
        btnNhanVien.Text = EmployeeButtonText;
        btnThongKe.Text = ReportButtonText;
        btnQuanLyKho.Text = InventoryButtonText;
        btnKhachHang.Text = CustomerButtonText;
        btnAuditLog.Text = AuditLogButtonText;

        ApplyInactiveSidebarStyle(btnBanHang);
        ApplyInactiveSidebarStyle(btnQuanLyBan);
        ApplyInactiveSidebarStyle(btnQuanLyMon);
        ApplyInactiveSidebarStyle(btnCongThuc);
        ApplyInactiveSidebarStyle(btnHoaDon);
        ApplyInactiveSidebarStyle(btnNhanVien);
        ApplyInactiveSidebarStyle(btnThongKe);
        ApplyInactiveSidebarStyle(btnQuanLyKho);
        ApplyInactiveSidebarStyle(btnKhachHang);
        ApplyInactiveSidebarStyle(btnAuditLog);

        ApplyUnifiedSidebarBranding(flowSidebarMenu);

        var orderedButtons = new List<Button>
        {
            btnBanHang,
            btnQuanLyBan,
            btnQuanLyMon,
            btnCongThuc,
            btnQuanLyKho,
            btnHoaDon,
            btnKhachHang,
            btnNhanVien,
            btnThongKe,
            btnAuditLog
        };

        ReorderSidebar(flowSidebarMenu, orderedButtons);
        return (btnKhachHang, btnQuanLyKho, btnAuditLog);
    }

    private static void ApplyUnifiedSidebarBranding(FlowLayoutPanel flowSidebarMenu)
    {
        if (flowSidebarMenu.Parent is not Control sidebarPanel)
        {
            return;
        }

        var panelLogo = sidebarPanel.Controls
            .OfType<Panel>()
            .FirstOrDefault(panel => string.Equals(panel.Name, "panelLogo", StringComparison.OrdinalIgnoreCase));

        if (panelLogo == null)
        {
            return;
        }

        var lblLogo = panelLogo.Controls
            .OfType<Label>()
            .FirstOrDefault(label => string.Equals(label.Name, "lblLogo", StringComparison.OrdinalIgnoreCase));
        var lblLogoSub = panelLogo.Controls
            .OfType<Label>()
            .FirstOrDefault(label => string.Equals(label.Name, "lblLogoSub", StringComparison.OrdinalIgnoreCase));

        if (lblLogo != null)
        {
            lblLogo.Text = BrandTitleText;
            lblLogo.AutoSize = true;
            lblLogo.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(22, 21);
        }

        if (lblLogoSub != null)
        {
            lblLogoSub.Text = BrandSubtitleText;
            lblLogoSub.AutoSize = true;
            lblLogoSub.ForeColor = Color.FromArgb(196, 176, 157);
            lblLogoSub.Location = new Point(24, 52);
        }
    }

    public static void ApplySidebarVisibility(
        IPermissionService permissionBUS,
        Button btnBanHang,
        Button btnQuanLyBan,
        Button btnQuanLyMon,
        Button btnCongThuc,
        Button btnHoaDon,
        Button btnNhanVien,
        Button btnThongKe,
        Button? btnKhachHang = null,
        Button? btnQuanLyKho = null,
        Button? btnAuditLog = null)
    {
        ArgumentNullException.ThrowIfNull(permissionBUS);

        var currentRole = PermissionExtensions.GetCurrentUserRole();

        var isAdmin = permissionBUS.IsAdmin();
        var coQuyenBanHang = (isAdmin || permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View))
                             && PermissionService.Shared.CanAccessForm("frmBanHang", currentRole);
        var coQuyenMenu = isAdmin || permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
        var coQuyenQuanLyBan = coQuyenMenu && PermissionService.Shared.CanAccessForm("frmQuanLiBan", currentRole);
        var coQuyenQuanLyMon = coQuyenMenu && PermissionService.Shared.CanAccessForm("frmQuanLiMon", currentRole);
        var coQuyenCongThuc = coQuyenMenu && PermissionService.Shared.CanAccessForm("frmCongThuc", currentRole);
        var coQuyenHoaDon = (isAdmin || permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View))
                            && PermissionService.Shared.CanAccessForm("frmHoaDon", currentRole);
        var coQuyenKho = (isAdmin || permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.View))
                         && PermissionService.Shared.CanAccessForm("frmQuanLiKho", currentRole);
        var coQuyenKhachHang = (isAdmin || permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.View))
                               && PermissionService.Shared.CanAccessForm("frmKhachHang", currentRole);
        var coQuyenNhanVien = (isAdmin || permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View))
                              && PermissionService.Shared.CanAccessForm("frmNhanVien", currentRole);
        var coQuyenThongKe = (isAdmin || permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View))
                             && PermissionService.Shared.CanAccessForm("frmThongKe", currentRole);
        var coQuyenAuditLog = isAdmin && PermissionService.Shared.CanAccessForm("frmAuditLog", currentRole);

        SetSidebarButtonAvailability(btnBanHang, coQuyenBanHang);
        SetSidebarButtonAvailability(btnQuanLyBan, coQuyenQuanLyBan);
        SetSidebarButtonAvailability(btnQuanLyMon, coQuyenQuanLyMon);
        SetSidebarButtonAvailability(btnCongThuc, coQuyenCongThuc);
        SetSidebarButtonAvailability(btnHoaDon, coQuyenHoaDon);
        SetSidebarButtonAvailability(btnNhanVien, coQuyenNhanVien);
        SetSidebarButtonAvailability(btnThongKe, coQuyenThongKe);
        SetSidebarButtonAvailability(btnKhachHang, coQuyenKhachHang);
        SetSidebarButtonAvailability(btnQuanLyKho, coQuyenKho);
        SetSidebarButtonAvailability(btnAuditLog, coQuyenAuditLog);
    }

    public static void HighlightSidebarSelection(Button selectedButton, params Button[] menuButtons)
    {
        if (menuButtons.Length == 0)
        {
            return;
        }

        foreach (var menuButton in menuButtons.Where(button => button != null).Distinct())
        {
            if (ReferenceEquals(menuButton, selectedButton))
            {
                ApplyActiveSidebarStyle(menuButton);
            }
            else
            {
                ApplyInactiveSidebarStyle(menuButton);
            }
        }
    }

    private static void SetSidebarButtonAvailability(Button? button, bool isVisible)
    {
        if (button == null)
        {
            return;
        }

        button.Visible = isVisible;
        button.Enabled = isVisible;
    }

    private static Button? FindSidebarButton(FlowLayoutPanel flowSidebarMenu, string buttonName, IEnumerable<Button> fallbackButtons)
    {
        var existing = flowSidebarMenu.Controls
            .OfType<Button>()
            .FirstOrDefault(button => string.Equals(button.Name, buttonName, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
        {
            return existing;
        }

        return fallbackButtons.FirstOrDefault(button =>
            string.Equals(button.Name, buttonName, StringComparison.OrdinalIgnoreCase));
    }

    private static Button CreateSidebarButton(string name, string text)
    {
        var button = new Button
        {
            Name = name,
            Text = text,
            Cursor = Cursors.Hand
        };

        ApplyInactiveSidebarStyle(button);
        return button;
    }

    private static void ApplyActiveSidebarStyle(Button button)
    {
        ApplySharedSidebarStyle(button);
        button.BackColor = ActiveBackColor;
        button.Font = ActiveFont;
        button.ForeColor = ActiveForeColor;
        button.FlatAppearance.MouseOverBackColor = ActiveBackColor;
        button.FlatAppearance.MouseDownBackColor = ActiveBackColor;
        button.UseVisualStyleBackColor = false;
    }

    private static void ApplyInactiveSidebarStyle(Button button)
    {
        ApplySharedSidebarStyle(button);
        button.Font = InactiveFont;
        button.ForeColor = InactiveForeColor;
        button.BackColor = InactiveBackColor;
        button.FlatAppearance.MouseOverBackColor = HoverBackColor;
        button.FlatAppearance.MouseDownBackColor = PressedBackColor;
        button.UseVisualStyleBackColor = false;
    }

    private static void ApplySharedSidebarStyle(Button button)
    {
        button.AutoSize = false;
        button.AutoEllipsis = true;
        button.Cursor = Cursors.Hand;
        button.FlatAppearance.BorderSize = 0;
        button.FlatStyle = FlatStyle.Flat;
        button.TextAlign = ContentAlignment.MiddleLeft;
        button.Padding = SidebarButtonPadding;
        button.Margin = SidebarButtonMargin;
        button.Size = SidebarButtonSize;
    }

    private static void ReorderSidebar(FlowLayoutPanel flowSidebarMenu, IReadOnlyList<Button> orderedButtons)
    {
        var orderedSet = new HashSet<Button>(orderedButtons);
        var extraControls = flowSidebarMenu.Controls
            .OfType<Control>()
            .Where(control => control is not Button button || !orderedSet.Contains(button))
            .ToList();

        flowSidebarMenu.SuspendLayout();
        flowSidebarMenu.Controls.Clear();

        for (var index = 0; index < orderedButtons.Count; index++)
        {
            var button = orderedButtons[index];
            button.TabIndex = index;
            flowSidebarMenu.Controls.Add(button);
        }

        foreach (var extraControl in extraControls)
        {
            if (extraControl is Button extraButton)
            {
                ApplyInactiveSidebarStyle(extraButton);
                extraButton.TabIndex = flowSidebarMenu.Controls.Count;
            }

            flowSidebarMenu.Controls.Add(extraControl);
        }

        flowSidebarMenu.ResumeLayout();
    }
}