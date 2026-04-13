using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Presenters;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.Permission;
using QuanLyQuanCaPhe.Services.UI;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmNhanVien : Form
    {
        private readonly bool _isEmbedded;
        private readonly INhanVienService _nhanVienBUS;
        private readonly IPermissionService _permissionBUS;
        private readonly PermissionService _formPermissionService = PermissionService.Shared;
        private readonly NhanVienPresenter _nhanVienPresenter;
        private readonly SearchDebounceHelper _timKiemDebounce;
        private FormPermission _formPermission = FormPermission.Deny(nameof(frmNhanVien), UserRole.Staff);
        private bool _dangXuLyTacVuNhanVien;
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;

        public frmNhanVien(
            INhanVienService? nhanVienBUS = null,
            IPermissionService? permissionBUS = null,
            NhanVienPresenter? nhanVienPresenter = null,
            bool isEmbedded = false)
        {
            _nhanVienBUS = AppServiceProvider.Resolve(nhanVienBUS, () => new NhanVienBUS());
            _permissionBUS = AppServiceProvider.Resolve(permissionBUS, () => new PermissionBUS());
            _nhanVienPresenter = nhanVienPresenter ?? new NhanVienPresenter();
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
            _timKiemDebounce = new SearchDebounceHelper(300, () => _ = LoadDanhSachNhanVienAsync(false, -1));

            dgvDanhSachNhanVien.AutoGenerateColumns = false;
            colIDNhanVien.DataPropertyName = nameof(NhanVienDTO.ID);
            colHoVaTenNhanVien.DataPropertyName = nameof(NhanVienDTO.HoVaTen);
            colDienThoaiNhanVien.DataPropertyName = nameof(NhanVienDTO.DienThoai);
            colDiaChiNhanVien.DataPropertyName = nameof(NhanVienDTO.DiaChi);
            colTenDangNhapNhanVien.DataPropertyName = nameof(NhanVienDTO.TenDangNhap);
            colQuyenHanNhanVien.DataPropertyName = nameof(NhanVienDTO.QuyenHan);

            Load += frmNhanVien_Load;
            txtTimNhanVien.TextChanged += txtTimNhanVien_TextChanged;
            txtTimNhanVien.KeyDown += txtTimNhanVien_KeyDown;
            dgvDanhSachNhanVien.SelectionChanged += dgvDanhSachNhanVien_SelectionChanged;
            btnLamMoi.Click += btnLamMoi_Click;

            btnXoaNhanVien.Text = "Ngừng hoạt động";

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            _btnQuanLyKhoSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NguyenLieu, () => new frmQuanLiKho(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            _btnKhachHangSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            _btnAuditLogSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmAuditLog(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnThongKe.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmThongKe(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnHoaDon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.HoaDon, () => new frmHoaDon(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private async void frmNhanVien_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Nhan vien"))
            {
                return;
            }

            var currentRole = PermissionExtensions.GetCurrentUserRole();
            _formPermission = _formPermissionService.GetPermission(nameof(frmNhanVien), currentRole);
            if (!this.EnsureCanView(_formPermission, "Bạn không có quyền truy cập chức năng Nhân viên."))
            {
                Close();
                return;
            }

            if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View))
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng Nhân viên.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            if (_isEmbedded)
            {
                EmbeddedFormLayoutHelper.UseContentOnlyLayout(panelMain, panelSidebar, panelTopbar);
            }

            HienThiNguoiDungDangNhap();
            TaiDanhSachVaiTroTheoQuyen();
            ApDungPhanQuyenLenUI();
            this.ApplyPermission(_formPermission);

            await LoadDanhSachNhanVienAsync(false, -1);
        }

        private void HienThiNguoiDungDangNhap()
        {
        }

        private void TaiDanhSachVaiTroTheoQuyen()
        {
            var dsVaiTro = _nhanVienBUS.LayDanhSachVaiTroCoTheGan();

            cboQuyenHan.Items.Clear();
            foreach (var vaiTro in dsVaiTro)
            {
                cboQuyenHan.Items.Add(vaiTro);
            }

            if (cboQuyenHan.Items.Count > 0)
            {
                cboQuyenHan.SelectedIndex = 0;
            }
        }

        private void ApDungPhanQuyenLenUI()
        {
            var coQuyenXemNhanVien = _formPermission.CanView
                                     && _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View);
            var coQuyenThemNhanVien = _formPermission.CanAdd
                                      && _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Create);
            var coQuyenCapNhatNhanVien = _formPermission.CanEdit
                                         && _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Update);
            var coQuyenXoaNhanVien = _formPermission.CanDelete
                                     && _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Delete);

            btnThemNhanVien.Visible = coQuyenThemNhanVien;
            btnCapNhatNhanVien.Visible = coQuyenCapNhatNhanVien;
            btnXoaNhanVien.Visible = coQuyenXoaNhanVien;
            btnNhapNhanVien.Visible = false;
            btnXuatNhanVien.Visible = false;
            btnTimNhanVien.Visible = coQuyenXemNhanVien;

            btnThemNhanVien.Enabled = btnThemNhanVien.Visible;
            btnCapNhatNhanVien.Enabled = btnCapNhatNhanVien.Visible;
            btnXoaNhanVien.Enabled = btnXoaNhanVien.Visible;
            btnNhapNhanVien.Enabled = false;
            btnXuatNhanVien.Enabled = false;
            btnTimNhanVien.Enabled = btnTimNhanVien.Visible;

            txtTimNhanVien.Enabled = coQuyenXemNhanVien;

            var coTheSuaRoleVaMatKhau = !_formPermission.HasLimitedEdit;
            cboQuyenHan.Enabled = coTheSuaRoleVaMatKhau && (coQuyenThemNhanVien || coQuyenCapNhatNhanVien);
            txtMatKhau.Enabled = coTheSuaRoleVaMatKhau && (coQuyenThemNhanVien || coQuyenCapNhatNhanVien);
            if (_formPermission.HasLimitedEdit)
            {
                txtMatKhau.Clear();
            }

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
                btnNhanVien,
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

        private void LoadDanhSachNhanVien()
        {
            _ = LoadDanhSachNhanVienAsync(false, -1);
        }

        private void LoadDanhSachNhanVien(bool khoiPhucLuaChon, int viTriUuTien)
        {
            _ = LoadDanhSachNhanVienAsync(khoiPhucLuaChon, viTriUuTien);
        }

        private async Task LoadDanhSachNhanVienAsync(bool khoiPhucLuaChon, int viTriUuTien)
        {
            try
            {
                var dsNhanVien = await _nhanVienBUS.LayDanhSachNhanVienAsync(txtTimNhanVien.Text);
                DataGridViewSelectionHelper.RebindData(dgvDanhSachNhanVien, dsNhanVien);

                if (khoiPhucLuaChon)
                {
                    if (!DataGridViewSelectionHelper.TrySelectNearestRow(dgvDanhSachNhanVien, viTriUuTien))
                    {
                        ResetForm();
                    }
                }
                else
                {
                    ResetForm();
                }

                var thongKeTongQuan = _nhanVienPresenter.BuildOverviewStats(dsNhanVien);
                lblTongNhanVienValue.Text = thongKeTongQuan.TongNhanVien.ToString();
                lblQuanLyValue.Text = thongKeTongQuan.SoQuanLy.ToString();
                lblThuNganValue.Text = thongKeTongQuan.SoNhanVienKhac.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThemNhanVien_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyTacVuNhanVien)
            {
                return;
            }

            if (!_formPermission.CanAdd)
            {
                MessageBox.Show("Bạn không có quyền thêm nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out var nhanVienDTO, false))
            {
                return;
            }

            _dangXuLyTacVuNhanVien = true;
            DatTrangThaiDangXuLyNhanVien(true);

            try
            {
                var result = await _nhanVienBUS.ThemNhanVienAsync(nhanVienDTO);
                if (!result.ThanhCong || result.NhanVienMoi == null)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await LoadDanhSachNhanVienAsync(false, -1);
                SelectRow(result.NhanVienMoi.ID);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DatTrangThaiDangXuLyNhanVien(false);
                _dangXuLyTacVuNhanVien = false;
            }
        }

        private void DatTrangThaiDangXuLyNhanVien(bool dangXuLy)
        {
            UiLoadingStateHelper.Apply(
                this,
                dangXuLy,
                btnThemNhanVien,
                btnCapNhatNhanVien,
                btnXoaNhanVien,
                btnTimNhanVien,
                btnLamMoi,
                txtTimNhanVien,
                dgvDanhSachNhanVien);

            if (!dangXuLy)
            {
                ApDungPhanQuyenLenUI();
            }
        }

        private async void btnCapNhatNhanVien_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyTacVuNhanVien)
            {
                return;
            }

            if (!_formPermission.CanEdit)
            {
                MessageBox.Show("Bạn không có quyền cập nhật nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtMaNhanVien.Text, out var nhanVienId))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out var nhanVienDTO, true))
            {
                return;
            }

            if (_formPermission.HasLimitedEdit
                && DataGridViewSelectionHelper.TryGetCurrentOrSelectedItem<NhanVienDTO>(dgvDanhSachNhanVien, out var nhanVienDangChon, out _)
                && nhanVienDangChon != null)
            {
                nhanVienDTO.QuyenHan = nhanVienDangChon.QuyenHan;
                nhanVienDTO.MatKhau = null;
            }

            nhanVienDTO.ID = nhanVienId;
            _dangXuLyTacVuNhanVien = true;
            DatTrangThaiDangXuLyNhanVien(true);

            try
            {
                var result = await _nhanVienBUS.CapNhatNhanVienAsync(nhanVienDTO);
                if (!result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await LoadDanhSachNhanVienAsync(false, -1);
                SelectRow(nhanVienId);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DatTrangThaiDangXuLyNhanVien(false);
                _dangXuLyTacVuNhanVien = false;
            }
        }

        private async void btnXoaNhanVien_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyTacVuNhanVien)
            {
                return;
            }

            if (!_formPermission.CanDelete)
            {
                MessageBox.Show("Bạn không có quyền ngừng hoạt động nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DataGridViewSelectionHelper.TryGetSelectedItem<NhanVienDTO>(dgvDanhSachNhanVien, out var nhanVien, out var viTriDangChon)
                || nhanVien == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần ngừng hoạt động.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nhanVien.IsDeleted)
            {
                MessageBox.Show("Nhân viên đã ngừng hoạt động trước đó.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Ngừng hoạt động nhân viên '{nhanVien.HoVaTen}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            _dangXuLyTacVuNhanVien = true;
            DatTrangThaiDangXuLyNhanVien(true);

            try
            {
                var result = await Task.Run(() => _nhanVienBUS.XoaNhanVien(nhanVien.ID, softDelete: true));
                if (!result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await LoadDanhSachNhanVienAsync(khoiPhucLuaChon: false, viTriUuTien: viTriDangChon);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DatTrangThaiDangXuLyNhanVien(false);
                _dangXuLyTacVuNhanVien = false;
            }
        }

        private async void btnLamMoi_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyTacVuNhanVien)
            {
                return;
            }

            _dangXuLyTacVuNhanVien = true;
            DatTrangThaiDangXuLyNhanVien(true);

            try
            {
                await LoadDanhSachNhanVienAsync(false, -1);
            }
            finally
            {
                DatTrangThaiDangXuLyNhanVien(false);
                _dangXuLyTacVuNhanVien = false;
            }
        }

        private async void btnTimNhanVien_Click(object? sender, EventArgs e)
        {
            _timKiemDebounce.Flush();
        }

        private void txtTimNhanVien_TextChanged(object? sender, EventArgs e)
        {
            _timKiemDebounce.Signal();
        }

        private void txtTimNhanVien_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            _timKiemDebounce.Flush();
            e.SuppressKeyPress = true;
        }

        private void ResetForm()
        {
            txtHoVaTen.Clear();
            txtDienThoai.Clear();
            txtDiaChi.Clear();
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();

            if (cboQuyenHan.Items.Count > 0)
            {
                cboQuyenHan.SelectedIndex = 0;
            }

            var maTiepTheo = _nhanVienBUS.LayMaNhanVienTiepTheo();
            txtMaNhanVien.Text = maTiepTheo > 0 ? maTiepTheo.ToString() : string.Empty;
        }

        private bool ValidateInput(out NhanVienDTO nhanVienDTO, bool laCapNhat)
        {
            var ketQua = _nhanVienPresenter.ValidateInput(
                txtHoVaTen.Text,
                txtDienThoai.Text,
                txtDiaChi.Text,
                txtTenDangNhap.Text,
                txtMatKhau.Text,
                cboQuyenHan.SelectedItem,
                laCapNhat);

            if (!ketQua.HopLe || ketQua.NhanVien == null)
            {
                nhanVienDTO = new NhanVienDTO();
                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FocusInputControl(ketQua.TruongLoi);
                return false;
            }

            nhanVienDTO = ketQua.NhanVien;
            return true;
        }

        private void SelectRow(int id)
        {
            if (!DataGridViewSelectionHelper.TrySelectRow<NhanVienDTO>(dgvDanhSachNhanVien, x => x.ID == id))
            {
                DataGridViewSelectionHelper.ClearSelection(dgvDanhSachNhanVien);
            }
        }

        private void dgvDanhSachNhanVien_SelectionChanged(object? sender, EventArgs e)
        {
            if (!DataGridViewSelectionHelper.TryGetCurrentOrSelectedItem<NhanVienDTO>(dgvDanhSachNhanVien, out var nhanVien, out _)
                || nhanVien == null)
            {
                return;
            }

            txtMaNhanVien.Text = nhanVien.ID.ToString();
            txtHoVaTen.Text = nhanVien.HoVaTen;
            txtDienThoai.Text = nhanVien.DienThoai ?? string.Empty;
            txtDiaChi.Text = nhanVien.DiaChi ?? string.Empty;
            txtTenDangNhap.Text = nhanVien.TenDangNhap;
            txtMatKhau.Clear();

            var quyenHanIndex = cboQuyenHan.FindStringExact(nhanVien.QuyenHan);
            cboQuyenHan.SelectedIndex = quyenHanIndex >= 0 ? quyenHanIndex : 0;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timKiemDebounce.Dispose();
            base.OnFormClosed(e);
        }

        private void FocusInputControl(NhanVienInputField truongNhap)
        {
            switch (truongNhap)
            {
                case NhanVienInputField.HoVaTen:
                    txtHoVaTen.Focus();
                    break;
                case NhanVienInputField.DienThoai:
                    txtDienThoai.Focus();
                    break;
                case NhanVienInputField.TenDangNhap:
                    txtTenDangNhap.Focus();
                    break;
                case NhanVienInputField.MatKhau:
                    txtMatKhau.Focus();
                    break;
                case NhanVienInputField.QuyenHan:
                    cboQuyenHan.Focus();
                    break;
            }
        }
    }
}
