using System.ComponentModel;
using System.Linq;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.Permission;
using QuanLyQuanCaPhe.Services.UI;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiKho : Form
    {
        private readonly bool _isEmbedded;
        private readonly INguyenLieuService _nguyenLieuBUS;
        private readonly IPermissionService _permissionBUS;
        private readonly PermissionService _formPermissionService = PermissionService.Shared;
        private FormPermission _formPermission = FormPermission.Deny(nameof(frmQuanLiKho), UserRole.Staff);
        private bool _isSaving;
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;

        public frmQuanLiKho(
            INguyenLieuService? nguyenLieuBUS = null,
            IPermissionService? permissionBUS = null,
            bool isEmbedded = false)
        {
            _nguyenLieuBUS = AppServiceProvider.Resolve(nguyenLieuBUS, () => new NguyenLieuBUS());
            _permissionBUS = AppServiceProvider.Resolve(permissionBUS, () => new PermissionBUS());
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

            dgvDanhSachKho.AutoGenerateColumns = false;
            colMaNguyenLieu.DataPropertyName = nameof(NguyenLieuDTO.MaNguyenLieu);
            colTenNguyenLieu.DataPropertyName = nameof(NguyenLieuDTO.TenNguyenLieu);
            colDonViTinhKho.DataPropertyName = nameof(NguyenLieuDTO.DonViTinh);
            colSoLuongTonKho.DataPropertyName = nameof(NguyenLieuDTO.SoLuongTon);
            colMucCanhBaoKho.DataPropertyName = nameof(NguyenLieuDTO.MucCanhBao);
            colGiaNhapGanNhatKho.DataPropertyName = nameof(NguyenLieuDTO.GiaNhapGanNhat);
            colTrangThaiKho.DataPropertyName = nameof(NguyenLieuDTO.TrangThaiHienThi);

            Load += frmQuanLiKho_Load;
            txtTimNguyenLieu.TextChanged += txtTimNguyenLieu_TextChanged;
            dgvDanhSachKho.SelectionChanged += dgvDanhSachKho_SelectionChanged;
            btnLamMoi.Click += btnLamMoi_Click;

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            _btnQuanLyKhoSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NguyenLieu, () => new frmQuanLiKho(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            _btnKhachHangSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            _btnAuditLogSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmAuditLog(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            btnNhanVien.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NhanVien, () => new frmNhanVien(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            btnHoaDon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.HoaDon, () => new frmHoaDon(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            btnThongKe.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmThongKe(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKho, skipNavigation: _isEmbedded);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void frmQuanLiKho_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Quan ly kho"))
            {
                return;
            }

            var currentRole = PermissionExtensions.GetCurrentUserRole();
            _formPermission = _formPermissionService.GetPermission(nameof(frmQuanLiKho), currentRole);
            if (!this.EnsureCanView(_formPermission, "Ban khong co quyen truy cap chuc nang quan ly kho."))
            {
                Close();
                return;
            }

            try
            {
                _permissionBUS.EnsurePermission(PermissionFeatures.NguyenLieu, PermissionActions.View, "Ban khong co quyen truy cap chuc nang quan ly kho.");
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

            HienThiNguoiDungDangNhap();
            ApDungPhanQuyenLenUI();
            this.ApplyPermission(_formPermission);

            if (cboDonViTinh.Items.Count > 0)
            {
                cboDonViTinh.SelectedIndex = 0;
            }

            if (cboTrangThai.Items.Count > 0)
            {
                cboTrangThai.SelectedIndex = 0;
            }

            LoadDanhSachKho();
        }

        private void HienThiNguoiDungDangNhap()
        {
        }

        private void ApDungPhanQuyenLenUI()
        {
            var coQuyenKhoXem = _formPermission.CanView
                                && _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.View);
            var coQuyenKhoThem = _formPermission.CanAdd
                                 && _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Create);
            var coQuyenKhoCapNhat = _formPermission.CanEdit
                                    && _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Update);
            var coQuyenKhoXoa = _formPermission.CanDelete
                                && _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Delete);

            btnThemNguyenLieu.Visible = coQuyenKhoThem;
            btnCapNhatNguyenLieu.Visible = coQuyenKhoCapNhat;
            btnXoaNguyenLieu.Visible = coQuyenKhoXoa;

            btnThemNguyenLieu.Enabled = btnThemNguyenLieu.Visible;
            btnCapNhatNguyenLieu.Enabled = btnCapNhatNguyenLieu.Visible;
            btnXoaNguyenLieu.Enabled = btnXoaNguyenLieu.Visible;

            txtTimNguyenLieu.Enabled = coQuyenKhoXem;
            dgvDanhSachKho.Enabled = coQuyenKhoXem;

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
                _btnQuanLyKhoSidebar,
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
        }

        private void LoadDanhSachKho()
        {
            LoadDanhSachKho(false, -1);
        }

        private void LoadDanhSachKho(bool khoiPhucLuaChon, int viTriUuTien)
        {
            var dsNguyenLieu = _nguyenLieuBUS.LayDanhSachNguyenLieu(txtTimNguyenLieu.Text);
            DataGridViewSelectionHelper.RebindData(dgvDanhSachKho, dsNguyenLieu);

            if (khoiPhucLuaChon)
            {
                if (!DataGridViewSelectionHelper.TrySelectNearestRow(dgvDanhSachKho, viTriUuTien))
                {
                    ResetForm();
                }
            }
            else
            {
                ResetForm();
            }

            lblTongNguyenLieuValue.Text = dsNguyenLieu.Count.ToString();
            lblSapHetValue.Text = dsNguyenLieu.Count(x => x.TrangThai == 2 && x.SoLuongTon > 0).ToString();
            lblHetHangValue.Text = dsNguyenLieu.Count(x => x.SoLuongTon <= 0).ToString();
            lblDonViTinhValue.Text = dsNguyenLieu
                .Select(x => x.DonViTinh)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count()
                .ToString();
        }

        private void btnThemNguyenLieu_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!_formPermission.CanAdd)
            {
                MessageBox.Show("Bạn không có quyền thêm nguyên liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyKho(true);

            if (!ValidateInput(out var nguyenLieuDTO))
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKho(false);
                return;
            }

            try
            {
                var result = _nguyenLieuBUS.ThemNguyenLieu(nguyenLieuDTO);
                if (!result.ThanhCong || result.NguyenLieuMoi == null)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadDanhSachKho();
                SelectRow(result.NguyenLieuMoi.MaNguyenLieu);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKho(false);
            }
        }

        private void btnCapNhatNguyenLieu_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!_formPermission.CanEdit)
            {
                MessageBox.Show("Bạn không có quyền cập nhật nguyên liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtMaNguyenLieu.Text, out var maNguyenLieu))
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out var nguyenLieuDTO))
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyKho(true);

            try
            {
                nguyenLieuDTO.MaNguyenLieu = maNguyenLieu;
                var result = _nguyenLieuBUS.CapNhatNguyenLieu(nguyenLieuDTO);
                if (!result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadDanhSachKho();
                SelectRow(maNguyenLieu);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKho(false);
            }
        }

        private void btnXoaNguyenLieu_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!_formPermission.CanDelete)
            {
                MessageBox.Show("Bạn không có quyền xóa nguyên liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DataGridViewSelectionHelper.TryGetSelectedItem<NguyenLieuDTO>(dgvDanhSachKho, out var nguyenLieu, out var viTriDangChon)
                || nguyenLieu == null)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa nguyên liệu '{nguyenLieu.TenNguyenLieu}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyKho(true);

            try
            {
                var result = _nguyenLieuBUS.XoaNguyenLieu(nguyenLieu.MaNguyenLieu);
                if (!result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadDanhSachKho(khoiPhucLuaChon: false, viTriUuTien: viTriDangChon);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKho(false);
            }
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyKho(true);

            try
            {
                txtTimNguyenLieu.Clear();
                LoadDanhSachKho();
                ResetForm();
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKho(false);
            }
        }

        private void txtTimNguyenLieu_TextChanged(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            LoadDanhSachKho();
        }

        private void DatTrangThaiDangXuLyKho(bool dangXuLy)
        {
            var controlsCanToggle = new List<Control>
            {
                btnThemNguyenLieu,
                btnCapNhatNguyenLieu,
                btnXoaNguyenLieu,
                btnLamMoi,
                txtTimNguyenLieu,
                dgvDanhSachKho,
                txtTenNguyenLieu,
                txtSoLuongTon,
                txtMucCanhBao,
                txtGiaNhapGanNhat,
                cboDonViTinh,
                cboTrangThai
            };

            UiLoadingStateHelper.Apply(
                this,
                dangXuLy,
                controlsCanToggle.ToArray());

            if (!dangXuLy)
            {
                ApDungPhanQuyenLenUI();
            }
        }

        private void dgvDanhSachKho_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachKho.CurrentRow?.DataBoundItem is not NguyenLieuDTO nguyenLieu)
            {
                return;
            }

            txtMaNguyenLieu.Text = nguyenLieu.MaNguyenLieu.ToString();
            txtTenNguyenLieu.Text = nguyenLieu.TenNguyenLieu;
            txtSoLuongTon.Text = nguyenLieu.SoLuongTon.ToString();
            txtMucCanhBao.Text = nguyenLieu.MucCanhBao.ToString();
            txtGiaNhapGanNhat.Text = nguyenLieu.GiaNhapGanNhat.ToString();

            var donViTinhIndex = cboDonViTinh.FindStringExact(nguyenLieu.DonViTinh);
            cboDonViTinh.SelectedIndex = donViTinhIndex >= 0 ? donViTinhIndex : 0;

            var trangThaiIndex = cboTrangThai.FindStringExact(nguyenLieu.TrangThaiHienThi);
            cboTrangThai.SelectedIndex = trangThaiIndex >= 0 ? trangThaiIndex : 0;
        }

        private void ResetForm()
        {
            txtMaNguyenLieu.Text = _nguyenLieuBUS.LayMaNguyenLieuTiepTheo().ToString();
            txtTenNguyenLieu.Clear();
            txtSoLuongTon.Text = "0";
            txtMucCanhBao.Text = "0";
            txtGiaNhapGanNhat.Text = "0";

            if (cboDonViTinh.Items.Count > 0)
            {
                cboDonViTinh.SelectedIndex = 0;
            }

            if (cboTrangThai.Items.Count > 0)
            {
                cboTrangThai.SelectedIndex = 0;
            }
        }

        private bool ValidateInput(out NguyenLieuDTO nguyenLieuDTO)
        {
            nguyenLieuDTO = new NguyenLieuDTO();

            if (string.IsNullOrWhiteSpace(txtTenNguyenLieu.Text))
            {
                MessageBox.Show("Tên nguyên liệu không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNguyenLieu.Focus();
                return false;
            }

            if (cboDonViTinh.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDonViTinh.Focus();
                return false;
            }

            if (!decimal.TryParse(txtSoLuongTon.Text.Trim(), out var soLuongTon) || soLuongTon < 0)
            {
                MessageBox.Show("Số lượng tồn không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongTon.Focus();
                return false;
            }

            if (!decimal.TryParse(txtMucCanhBao.Text.Trim(), out var mucCanhBao) || mucCanhBao < 0)
            {
                MessageBox.Show("Mức cảnh báo không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMucCanhBao.Focus();
                return false;
            }

            if (!decimal.TryParse(txtGiaNhapGanNhat.Text.Trim(), out var giaNhapGanNhat) || giaNhapGanNhat < 0)
            {
                MessageBox.Show("Giá nhập gần nhất không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhapGanNhat.Focus();
                return false;
            }

            var trangThai = cboTrangThai.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                MessageBox.Show("Vui lòng chọn trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTrangThai.Focus();
                return false;
            }

            nguyenLieuDTO = new NguyenLieuDTO
            {
                TenNguyenLieu = txtTenNguyenLieu.Text.Trim(),
                DonViTinh = cboDonViTinh.SelectedItem?.ToString() ?? string.Empty,
                SoLuongTon = soLuongTon,
                MucCanhBao = mucCanhBao,
                GiaNhapGanNhat = giaNhapGanNhat,
                TrangThai = ChuyenTrangThaiTextSangInt(trangThai)
            };

            return true;
        }

        private static int ChuyenTrangThaiTextSangInt(string trangThai)
        {
            if (trangThai.Equals("Ngừng dùng", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            if (trangThai.Equals("Sắp hết", StringComparison.OrdinalIgnoreCase)
                || trangThai.Equals("Hết hàng", StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }

            return 1;
        }

        private void SelectRow(int maNguyenLieu)
        {
            DataGridViewSelectionHelper.ClearSelection(dgvDanhSachKho);

            foreach (DataGridViewRow row in dgvDanhSachKho.Rows)
            {
                if (row.DataBoundItem is not NguyenLieuDTO nguyenLieu || nguyenLieu.MaNguyenLieu != maNguyenLieu)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachKho.CurrentCell = row.Cells[0];
                return;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }
    }
}
