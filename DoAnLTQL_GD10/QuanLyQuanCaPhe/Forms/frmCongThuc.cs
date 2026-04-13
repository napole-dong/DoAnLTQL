using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Presenters;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.Permission;
using QuanLyQuanCaPhe.Services.UI;
using System.Globalization;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmCongThuc : Form
    {
        private readonly bool _isEmbedded;
        private readonly IPermissionService _permissionBUS;
        private readonly ICongThucService _congThucBUS;
        private readonly PermissionService _formPermissionService = PermissionService.Shared;
        private readonly CongThucPresenter _congThucPresenter;
        private readonly SearchDebounceHelper _timKiemDebounce;
        private FormPermission _formPermission = FormPermission.Deny(nameof(frmCongThuc), UserRole.Staff);
        private bool _dangNapDuLieu;
        private bool _dangXuLyCongThuc;
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;

        private sealed class MonLocItem
        {
            public int MonId { get; init; }
            public string TenMon { get; init; } = string.Empty;
        }

        public frmCongThuc() : this(isEmbedded: false)
        {
        }

        public frmCongThuc(
            IPermissionService? permissionBUS = null,
            ICongThucService? congThucBUS = null,
            CongThucPresenter? congThucPresenter = null,
            bool isEmbedded = false)
        {
            _permissionBUS = AppServiceProvider.Resolve(permissionBUS, () => new PermissionBUS());
            _congThucBUS = AppServiceProvider.Resolve(congThucBUS, () => new CongThucBUS());
            _congThucPresenter = congThucPresenter ?? new CongThucPresenter();
            _isEmbedded = isEmbedded;
            InitializeComponent();
            (_btnKhachHangSidebar, _btnQuanLyKhoSidebar, _btnAuditLogSidebar) = SidebarUiHelper.EnsureUnifiedSidebarMenu(
                flowSidebarMenu,
                btnBanHang,
                btnQuanLyBan,
                btnQuanLyMon,
                btnCongThuc,
                btnHoaDon,
                btnNhanVien,
                btnThongKe);

            _timKiemDebounce = new SearchDebounceHelper(300, () => TaiDanhSachCongThuc(giuDongDangChon: true));

            CauHinhBangDuLieu();
            CauHinhSuKienNghiepVu();
            CauHinhSuKienDieuHuong();

            Load += frmCongThuc_Load;
        }

        private void CauHinhBangDuLieu()
        {
            dgvDanhSachCongThuc.AutoGenerateColumns = false;
            colMonID.DataPropertyName = nameof(CongThucMonDTO.MonID);
            colTenMon.DataPropertyName = nameof(CongThucMonDTO.TenMon);
            colNguyenLieuID.DataPropertyName = nameof(CongThucMonDTO.NguyenLieuID);
            colTenNguyenLieu.DataPropertyName = nameof(CongThucMonDTO.TenNguyenLieu);
            colSoLuong.DataPropertyName = nameof(CongThucMonDTO.SoLuongHienThi);
            colDonViTinh.DataPropertyName = nameof(CongThucMonDTO.DonViTinh);
            colSoLuongTon.DataPropertyName = nameof(CongThucMonDTO.SoLuongTonHienThi);
            colTrangThaiTon.DataPropertyName = nameof(CongThucMonDTO.TrangThaiTonHienThi);
        }

        private void CauHinhSuKienNghiepVu()
        {
            txtTimKiem.TextChanged += (_, _) => _timKiemDebounce.Signal();
            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            cboLocMon.SelectedIndexChanged += cboLocMon_SelectedIndexChanged;

            cboMon.SelectedIndexChanged += cboMon_SelectedIndexChanged;
            cboNguyenLieu.SelectedIndexChanged += cboNguyenLieu_SelectedIndexChanged;
            txtDinhLuong.TextChanged += (_, _) => CapNhatTrangThaiTonTheoDinhLuong();

            dgvDanhSachCongThuc.SelectionChanged += dgvDanhSachCongThuc_SelectionChanged;

            btnThemCongThuc.Click += btnThemCongThuc_Click;
            btnCapNhatCongThuc.Click += btnCapNhatCongThuc_Click;
            btnXoaCongThuc.Click += btnXoaCongThuc_Click;
            btnLamMoiThongTin.Click += btnLamMoiThongTin_Click;
            btnLamMoiDanhSach.Click += btnLamMoiDanhSach_Click;
        }

        private void CauHinhSuKienDieuHuong()
        {
            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), skipNavigation: _isEmbedded);
            _btnQuanLyKhoSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NguyenLieu, () => new frmQuanLiKho(isEmbedded: true), skipNavigation: _isEmbedded);
            _btnKhachHangSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), skipNavigation: _isEmbedded);
            _btnAuditLogSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmAuditLog(isEmbedded: true), skipNavigation: _isEmbedded);
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

            var currentRole = PermissionExtensions.GetCurrentUserRole();
            _formPermission = _formPermissionService.GetPermission(nameof(frmCongThuc), currentRole);
            if (!this.EnsureCanView(_formPermission, "Ban khong co quyen truy cap chuc nang cong thuc."))
            {
                Close();
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
                EmbeddedFormLayoutHelper.UseContentOnlyLayout(panelMain, panelSidebar, panelTopbar);
            }

            ApDungPhanQuyenDieuHuong();
            this.ApplyPermission(_formPermission);
            TaiThongKeMacDinh();
            TaiDanhMucMonVaNguyenLieu();
            TaiDanhSachCongThuc(giuDongDangChon: false);
        }

        private void ApDungPhanQuyenDieuHuong()
        {
            var coQuyenMenu = _formPermission.CanView
                             && _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
            var coQuyenThemCongThuc = _formPermission.CanAdd
                                      && _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create);
            var coQuyenCapNhatCongThuc = _formPermission.CanEdit
                                         && _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update);
            var coQuyenXoaCongThuc = _formPermission.CanDelete
                                     && _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Delete);

            SidebarUiHelper.ApplySidebarVisibility(
                _permissionBUS,
                btnBanHang,
                btnQuanLyBan,
                btnQuanLyMon,
                btnCongThuc,
                btnHoaDon,
                btnNhanVien,
                btnThongKe,
                btnKhachHang: _btnKhachHangSidebar,
                btnQuanLyKho: _btnQuanLyKhoSidebar,
                btnAuditLog: _btnAuditLogSidebar);

            SidebarUiHelper.HighlightSidebarSelection(
                btnCongThuc,
                btnBanHang,
                btnQuanLyBan,
                btnQuanLyMon,
                btnCongThuc,
                _btnQuanLyKhoSidebar,
                btnHoaDon,
                _btnKhachHangSidebar,
                btnNhanVien,
                btnThongKe,
                _btnAuditLogSidebar);

            btnThemCongThuc.Visible = coQuyenThemCongThuc;
            btnCapNhatCongThuc.Visible = coQuyenCapNhatCongThuc;
            btnXoaCongThuc.Visible = coQuyenXoaCongThuc;
            btnLamMoiThongTin.Visible = coQuyenMenu;
            btnLamMoiDanhSach.Visible = coQuyenMenu;

            var choPhepNhapThongTin = coQuyenThemCongThuc || coQuyenCapNhatCongThuc;
            cboMon.Enabled = choPhepNhapThongTin;
            cboNguyenLieu.Enabled = choPhepNhapThongTin;
            txtDinhLuong.ReadOnly = !choPhepNhapThongTin;

            txtTimKiem.Enabled = coQuyenMenu;
            cboLocMon.Enabled = coQuyenMenu;
            dgvDanhSachCongThuc.Enabled = coQuyenMenu;
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

        private void TaiDanhMucMonVaNguyenLieu()
        {
            _dangNapDuLieu = true;

            try
            {
                var dsMon = _congThucBUS.LayDanhSachMonChoCongThuc();
                cboMon.DataSource = dsMon;
                cboMon.DisplayMember = nameof(MonDTO.TenMon);
                cboMon.ValueMember = nameof(MonDTO.ID);
                cboMon.SelectedIndex = dsMon.Count > 0 ? 0 : -1;

                var dsLocMon = new List<MonLocItem>
                {
                    new() { MonId = 0, TenMon = "Tất cả món" }
                };

                dsLocMon.AddRange(dsMon.Select(x => new MonLocItem
                {
                    MonId = x.ID,
                    TenMon = x.TenMon
                }));

                cboLocMon.DataSource = dsLocMon;
                cboLocMon.DisplayMember = nameof(MonLocItem.TenMon);
                cboLocMon.ValueMember = nameof(MonLocItem.MonId);
                cboLocMon.SelectedIndex = 0;

                var dsNguyenLieu = _congThucBUS.LayDanhSachNguyenLieuChoCongThuc();
                cboNguyenLieu.DataSource = dsNguyenLieu;
                cboNguyenLieu.DisplayMember = nameof(NguyenLieuDTO.TenNguyenLieu);
                cboNguyenLieu.ValueMember = nameof(NguyenLieuDTO.MaNguyenLieu);
                cboNguyenLieu.SelectedIndex = dsNguyenLieu.Count > 0 ? 0 : -1;
            }
            finally
            {
                _dangNapDuLieu = false;
            }

            DatLaiThongTinNhap();
        }

        private void TaiDanhSachCongThuc(bool giuDongDangChon)
        {
            var monDangChon = 0;
            var nguyenLieuDangChon = 0;

            if (giuDongDangChon && dgvDanhSachCongThuc.CurrentRow?.DataBoundItem is CongThucMonDTO itemDangChon)
            {
                monDangChon = itemDangChon.MonID;
                nguyenLieuDangChon = itemDangChon.NguyenLieuID;
            }

            var dsCongThuc = _congThucBUS.LayDanhSachCongThuc(txtTimKiem.Text, LayMonLocDangChon());
            DataGridViewSelectionHelper.RebindData(dgvDanhSachCongThuc, dsCongThuc);
            CapNhatThongKe(dsCongThuc);

            if (giuDongDangChon && monDangChon > 0 && nguyenLieuDangChon > 0)
            {
                if (TrySelectCongThucRow(monDangChon, nguyenLieuDangChon))
                {
                    return;
                }
            }

            if (dgvDanhSachCongThuc.Rows.Count > 0)
            {
                DataGridViewSelectionHelper.TrySelectNearestRow(dgvDanhSachCongThuc, 0);
                return;
            }

            DatLaiThongTinNhap();
        }

        private int? LayMonLocDangChon()
        {
            return cboLocMon.SelectedValue is int monId && monId > 0
                ? monId
                : null;
        }

        private void CapNhatThongKe(IReadOnlyCollection<CongThucMonDTO> dsCongThuc)
        {
            var thongKe = _congThucPresenter.BuildOverviewStats(dsCongThuc);
            lblTongCongThucValue.Text = thongKe.TongCongThuc.ToString(CultureInfo.InvariantCulture);
            lblMonCoCongThucValue.Text = thongKe.MonCoCongThuc.ToString(CultureInfo.InvariantCulture);
            lblNguyenLieuThamGiaValue.Text = thongKe.NguyenLieuThamGia.ToString(CultureInfo.InvariantCulture);
            lblCongThucCanhBaoValue.Text = thongKe.CongThucCanhBao.ToString(CultureInfo.InvariantCulture);
        }

        private void DatLaiThongTinNhap()
        {
            _dangNapDuLieu = true;

            try
            {
                if (cboMon.Items.Count > 0 && cboMon.SelectedIndex < 0)
                {
                    cboMon.SelectedIndex = 0;
                }

                if (cboNguyenLieu.Items.Count > 0 && cboNguyenLieu.SelectedIndex < 0)
                {
                    cboNguyenLieu.SelectedIndex = 0;
                }

                txtDinhLuong.Text = string.Empty;
                HienThiThongTinMonDangChon();
                HienThiThongTinNguyenLieuDangChon();
            }
            finally
            {
                _dangNapDuLieu = false;
            }

            CapNhatTrangThaiTonTheoDinhLuong();
        }

        private void HienThiThongTinMonDangChon()
        {
            if (cboMon.SelectedItem is not MonDTO mon)
            {
                txtMaMon.Text = string.Empty;
                return;
            }

            txtMaMon.Text = mon.ID.ToString(CultureInfo.InvariantCulture);
        }

        private void HienThiThongTinNguyenLieuDangChon()
        {
            if (cboNguyenLieu.SelectedItem is not NguyenLieuDTO nguyenLieu)
            {
                txtMaNguyenLieu.Text = string.Empty;
                txtDonViTinh.Text = string.Empty;
                txtSoLuongTon.Text = string.Empty;
                txtTrangThaiTon.Text = string.Empty;
                return;
            }

            txtMaNguyenLieu.Text = nguyenLieu.MaNguyenLieu.ToString(CultureInfo.InvariantCulture);
            txtDonViTinh.Text = nguyenLieu.DonViTinh;
            txtSoLuongTon.Text = nguyenLieu.SoLuongTon.ToString("N3", CultureInfo.CurrentCulture);

            CapNhatTrangThaiTonTheoDinhLuong();
        }

        private void CapNhatTrangThaiTonTheoDinhLuong()
        {
            if (cboNguyenLieu.SelectedItem is not NguyenLieuDTO nguyenLieu)
            {
                txtTrangThaiTon.Text = string.Empty;
                return;
            }

            txtTrangThaiTon.Text = _congThucPresenter.BuildTrangThaiTon(nguyenLieu, txtDinhLuong.Text);
        }

        private void txtTimKiem_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            _timKiemDebounce.Flush();
        }

        private void cboLocMon_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            TaiDanhSachCongThuc(giuDongDangChon: false);
        }

        private void cboMon_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            HienThiThongTinMonDangChon();
        }

        private void cboNguyenLieu_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            HienThiThongTinNguyenLieuDangChon();
        }

        private void dgvDanhSachCongThuc_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            if (dgvDanhSachCongThuc.CurrentRow?.DataBoundItem is not CongThucMonDTO congThucDangChon)
            {
                return;
            }

            _dangNapDuLieu = true;

            try
            {
                cboMon.SelectedValue = congThucDangChon.MonID;
                cboNguyenLieu.SelectedValue = congThucDangChon.NguyenLieuID;

                txtMaMon.Text = congThucDangChon.MonID.ToString(CultureInfo.InvariantCulture);
                txtMaNguyenLieu.Text = congThucDangChon.NguyenLieuID.ToString(CultureInfo.InvariantCulture);
                txtDinhLuong.Text = congThucDangChon.SoLuong.ToString("0.###", CultureInfo.CurrentCulture);
                txtDonViTinh.Text = congThucDangChon.DonViTinh;
                txtSoLuongTon.Text = congThucDangChon.SoLuongTon.ToString("N3", CultureInfo.CurrentCulture);
                txtTrangThaiTon.Text = congThucDangChon.TrangThaiTonHienThi;
            }
            finally
            {
                _dangNapDuLieu = false;
            }
        }

        private void btnThemCongThuc_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyCongThuc)
            {
                return;
            }

            if (!_formPermission.CanAdd)
            {
                MessageBox.Show("Bạn không có quyền thêm công thức.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _dangXuLyCongThuc = true;
            DatTrangThaiDangXuLyCongThuc(true);

            try
            {
            var validation = _congThucPresenter.ValidateInput(cboMon.SelectedValue, cboNguyenLieu.SelectedValue, txtDinhLuong.Text);
            if (!validation.HopLe)
            {
                HienThiLoiValidate(validation);
                return;
            }

            if (!validation.MonId.HasValue || !validation.NguyenLieuId.HasValue || !validation.SoLuong.HasValue)
            {
                MessageBox.Show("Dữ liệu công thức không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var monId = validation.MonId.Value;
            var nguyenLieuId = validation.NguyenLieuId.Value;
            var soLuong = validation.SoLuong.Value;

            var result = _congThucBUS.ThemCongThuc(monId, nguyenLieuId, soLuong);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSachCongThuc(giuDongDangChon: false);
            TrySelectCongThucRow(monId, nguyenLieuId);

            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                DatTrangThaiDangXuLyCongThuc(false);
                _dangXuLyCongThuc = false;
            }
        }

        private void btnCapNhatCongThuc_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyCongThuc)
            {
                return;
            }

            if (!_formPermission.CanEdit)
            {
                MessageBox.Show("Bạn không có quyền cập nhật công thức.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _dangXuLyCongThuc = true;
            DatTrangThaiDangXuLyCongThuc(true);

            try
            {
            if (!DataGridViewSelectionHelper.TryGetCurrentOrSelectedItem<CongThucMonDTO>(dgvDanhSachCongThuc, out var congThucDangChon, out _)
                || congThucDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn công thức cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var validation = _congThucPresenter.ValidateSoLuong(txtDinhLuong.Text);
            if (!validation.HopLe)
            {
                HienThiLoiValidate(validation);
                return;
            }

            if (!validation.SoLuong.HasValue)
            {
                MessageBox.Show("Dữ liệu công thức không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var soLuongMoi = validation.SoLuong.Value;

            var result = _congThucBUS.CapNhatCongThuc(congThucDangChon.MonID, congThucDangChon.NguyenLieuID, soLuongMoi);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSachCongThuc(giuDongDangChon: false);
            TrySelectCongThucRow(congThucDangChon.MonID, congThucDangChon.NguyenLieuID);

            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                DatTrangThaiDangXuLyCongThuc(false);
                _dangXuLyCongThuc = false;
            }
        }

        private void btnXoaCongThuc_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyCongThuc)
            {
                return;
            }

            if (!_formPermission.CanDelete)
            {
                MessageBox.Show("Bạn không có quyền xóa công thức.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _dangXuLyCongThuc = true;
            DatTrangThaiDangXuLyCongThuc(true);

            try
            {
            if (!DataGridViewSelectionHelper.TryGetCurrentOrSelectedItem<CongThucMonDTO>(dgvDanhSachCongThuc, out var congThucDangChon, out _)
                || congThucDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn công thức cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Xóa nguyên liệu '{congThucDangChon.TenNguyenLieu}' khỏi công thức món '{congThucDangChon.TenMon}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            var result = _congThucBUS.XoaCongThuc(congThucDangChon.MonID, congThucDangChon.NguyenLieuID);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSachCongThuc(giuDongDangChon: false);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                DatTrangThaiDangXuLyCongThuc(false);
                _dangXuLyCongThuc = false;
            }
        }

        private void btnLamMoiThongTin_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyCongThuc)
            {
                return;
            }

            _dangXuLyCongThuc = true;
            DatTrangThaiDangXuLyCongThuc(true);

            try
            {
            DataGridViewSelectionHelper.ClearSelection(dgvDanhSachCongThuc);
            DatLaiThongTinNhap();
            }
            finally
            {
                DatTrangThaiDangXuLyCongThuc(false);
                _dangXuLyCongThuc = false;
            }
        }

        private void btnLamMoiDanhSach_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyCongThuc)
            {
                return;
            }

            _dangXuLyCongThuc = true;
            DatTrangThaiDangXuLyCongThuc(true);

            try
            {
            _dangNapDuLieu = true;

            try
            {
                txtTimKiem.Text = string.Empty;

                if (cboLocMon.Items.Count > 0)
                {
                    cboLocMon.SelectedIndex = 0;
                }
            }
            finally
            {
                _dangNapDuLieu = false;
            }

            TaiDanhSachCongThuc(giuDongDangChon: false);
            }
            finally
            {
                DatTrangThaiDangXuLyCongThuc(false);
                _dangXuLyCongThuc = false;
            }
        }

        private void DatTrangThaiDangXuLyCongThuc(bool dangXuLy)
        {
            UiLoadingStateHelper.Apply(
                this,
                dangXuLy,
                btnThemCongThuc,
                btnCapNhatCongThuc,
                btnXoaCongThuc,
                btnLamMoiThongTin,
                btnLamMoiDanhSach,
                txtTimKiem,
                cboLocMon,
                cboMon,
                cboNguyenLieu,
                txtDinhLuong,
                dgvDanhSachCongThuc);

            if (!dangXuLy)
            {
                ApDungPhanQuyenDieuHuong();
            }
        }

        private void HienThiLoiValidate(CongThucValidationResult validation)
        {
            if (validation.HopLe || string.IsNullOrWhiteSpace(validation.ThongBao))
            {
                return;
            }

            MessageBox.Show(validation.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            switch (validation.TruongLoi)
            {
                case CongThucInputField.Mon:
                    cboMon.Focus();
                    break;
                case CongThucInputField.NguyenLieu:
                    cboNguyenLieu.Focus();
                    break;
                case CongThucInputField.DinhLuong:
                    txtDinhLuong.Focus();
                    break;
            }
        }

        private bool TrySelectCongThucRow(int monId, int nguyenLieuId)
        {
            return DataGridViewSelectionHelper.TrySelectRow<CongThucMonDTO>(
                dgvDanhSachCongThuc,
                x => x.MonID == monId && x.NguyenLieuID == nguyenLieuId);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timKiemDebounce.Dispose();
            base.OnFormClosed(e);
        }
    }
}
