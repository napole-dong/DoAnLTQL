using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Navigation;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmCongThuc : Form
    {
        private readonly bool _isEmbedded;
        private readonly PermissionBUS _permissionBUS = new();

        public frmCongThuc() : this(false)
        {
        }

        public frmCongThuc(bool isEmbedded)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();
            CauHinhSuKienDieuHuong();
            Load += frmCongThuc_Load;
        }

        private void CauHinhSuKienDieuHuong()
        {
            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), skipNavigation: _isEmbedded);
            btnQuanLyKho.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NguyenLieu, () => new frmQuanLiKho(isEmbedded: true), skipNavigation: _isEmbedded);
            btnNhanVien.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NhanVien, () => new frmNhanVien(isEmbedded: true), skipNavigation: _isEmbedded);
            btnHoaDon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.HoaDon, () => new frmHoaDon(isEmbedded: true), skipNavigation: _isEmbedded);
            btnThongKe.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmThongKe(isEmbedded: true), skipNavigation: _isEmbedded);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void frmCongThuc_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Cong thuc"))
            {
                return;
            }

            try
            {
                _permissionBUS.EnsurePermission(PermissionFeatures.Menu, PermissionActions.View, "Ban khong co quyen truy cap chuc nang cong thuc.");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            if (_isEmbedded)
            {
                panelSidebar.Visible = false;
                panelTopbar.Visible = false;
                panelMain.Dock = DockStyle.Fill;
            }

            ApDungPhanQuyenDieuHuong();
            TaiThongKeMacDinh();
        }

        private void ApDungPhanQuyenDieuHuong()
        {
            var coQuyenMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);

            btnBanHang.Visible = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View);
            btnQuanLyBan.Visible = coQuyenMenu;
            btnQuanLyMon.Visible = coQuyenMenu;
            btnCongThuc.Visible = coQuyenMenu;
            btnQuanLyKho.Visible = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.View);
            btnNhanVien.Visible = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View);
            btnHoaDon.Visible = _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);
            btnThongKe.Visible = _permissionBUS.CanViewReport();
        }

        private void TaiThongKeMacDinh()
        {
            lblTongCongThucValue.Text = "0";
            lblMonCoCongThucValue.Text = "0";
            lblNguyenLieuThamGiaValue.Text = "0";
            lblCongThucCanhBaoValue.Text = "0";

            txtDonViTinh.Text = string.Empty;
            txtSoLuongTon.Text = string.Empty;
            txtTrangThaiTon.Text = string.Empty;
        }
    }
}
