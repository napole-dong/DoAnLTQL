using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.UI;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmKhachHang : Form
    {
        private readonly bool _isEmbedded;
        private readonly IKhachHangService _khachHangBUS;
        private readonly IPermissionService _permissionBUS;
        private bool _isSaving;
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;

        public frmKhachHang(
            IKhachHangService? khachHangBUS = null,
            IPermissionService? permissionBUS = null,
            bool isEmbedded = false)
        {
            _khachHangBUS = AppServiceProvider.Resolve(khachHangBUS, () => new KhachHangBUS());
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

            dgvDanhSachKhach.AutoGenerateColumns = false;
            colIDKhach.DataPropertyName = nameof(KhachHangDTO.ID);
            colHoVaTen.DataPropertyName = nameof(KhachHangDTO.HoVaTen);
            colDienThoai.DataPropertyName = nameof(KhachHangDTO.DienThoai);
            colDiaChi.DataPropertyName = nameof(KhachHangDTO.DiaChi);

            Load += frmKhachHang_Load;
            txtTimKhach.KeyDown += txtTimKhach_KeyDown;
            dgvDanhSachKhach.SelectionChanged += dgvDanhSachKhach_SelectionChanged;

            btnXoaKhach.Text = "Ngừng hoạt động";

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            _btnQuanLyKhoSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NguyenLieu, () => new frmQuanLiKho(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            _btnKhachHangSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            _btnAuditLogSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmAuditLog(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnNhanVien.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NhanVien, () => new frmNhanVien(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnHoaDon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.HoaDon, () => new frmHoaDon(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnThongKe.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmThongKe(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void frmKhachHang_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Khach hang"))
            {
                return;
            }

            try
            {
                _permissionBUS.EnsurePermission(PermissionFeatures.KhachHang, PermissionActions.View, "Bạn không có quyền truy cập chức năng Khách hàng.");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            if (_isEmbedded)
            {
                EmbeddedFormLayoutHelper.UseContentOnlyLayout(panelMain, panelSidebar, panelTopbar);
            }

            HienThiNguoiDungDangNhap();
            ApDungPhanQuyenSidebar();

            LoadDanhSachKhach();
        }

        private void HienThiNguoiDungDangNhap()
        {
        }

        private void ApDungPhanQuyenSidebar()
        {
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
                _btnKhachHangSidebar,
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

            btnNhapKhach.Visible = false;
            btnXuatKhach.Visible = false;
            btnNhapKhach.Enabled = false;
            btnXuatKhach.Enabled = false;
        }

        private void LoadDanhSachKhach()
        {
            LoadDanhSachKhach(false, -1);
        }

        private void LoadDanhSachKhach(bool khoiPhucLuaChon, int viTriUuTien)
        {
            var dsKhach = _khachHangBUS.LayDanhSachKhach(txtTimKhach.Text);
            DataGridViewSelectionHelper.RebindData(dgvDanhSachKhach, dsKhach);

            if (khoiPhucLuaChon)
            {
                if (!DataGridViewSelectionHelper.TrySelectNearestRow(dgvDanhSachKhach, viTriUuTien))
                {
                    ResetForm();
                }
            }
            else
            {
                ResetForm();
            }

            lblTongKhachValue.Text = dsKhach.Count.ToString();
            lblCoSoDienThoaiValue.Text = dsKhach.Count(x => !string.IsNullOrWhiteSpace(x.DienThoai)).ToString();
            lblCoDiaChiValue.Text = dsKhach.Count(x => !string.IsNullOrWhiteSpace(x.DiaChi)).ToString();
        }

        private void btnThemKhach_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyKhachHang(true);

            if (!ValidateInput(out var khachDTO))
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKhachHang(false);
                return;
            }

            try
            {
                var result = _khachHangBUS.ThemKhach(khachDTO);
                if (!result.ThanhCong || result.KhachMoi == null)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadDanhSachKhach();
                SelectRow(result.KhachMoi.ID);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKhachHang(false);
            }
        }

        private void btnCapNhatKhach_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!int.TryParse(txtMaKhach.Text, out var khachId))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out var khachDTO))
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyKhachHang(true);

            try
            {
                khachDTO.ID = khachId;
                var result = _khachHangBUS.CapNhatKhach(khachDTO);
                if (!result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadDanhSachKhach();
                SelectRow(khachId);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKhachHang(false);
            }
        }

        private void btnXoaKhach_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!DataGridViewSelectionHelper.TryGetSelectedItem<KhachHangDTO>(dgvDanhSachKhach, out var khach, out var viTriDangChon)
                || khach == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần ngừng hoạt động.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (khach.IsDeleted)
            {
                MessageBox.Show("Khách hàng đã ngừng hoạt động trước đó.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show($"Ngừng hoạt động khách hàng '{khach.HoVaTen}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyKhachHang(true);

            try
            {
                var result = _khachHangBUS.XoaKhach(khach.ID);
                if (!result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadDanhSachKhach(khoiPhucLuaChon: false, viTriUuTien: viTriDangChon);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKhachHang(false);
            }
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyKhachHang(true);

            try
            {
                LoadDanhSachKhach();
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKhachHang(false);
            }
        }

        private void btnTimKhach_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            LoadDanhSachKhach();
        }

        private void txtTimKhach_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (_isSaving)
            {
                e.SuppressKeyPress = true;
                return;
            }

            LoadDanhSachKhach();
            e.SuppressKeyPress = true;
        }

        private void DatTrangThaiDangXuLyKhachHang(bool dangXuLy)
        {
            UiLoadingStateHelper.Apply(
                this,
                dangXuLy,
                btnThemKhach,
                btnCapNhatKhach,
                btnXoaKhach,
                btnTimKhach,
                btnLamMoi,
                txtTimKhach,
                dgvDanhSachKhach);

            if (!dangXuLy)
            {
                ApDungPhanQuyenSidebar();
            }
        }

        private void ResetForm()
        {
            txtHoVaTen.Clear();
            txtDienThoai.Clear();
            txtDiaChi.Clear();
            txtMaKhach.Text = _khachHangBUS.LayMaKhachTiepTheo().ToString();
        }

        private bool ValidateInput(out KhachHangDTO khachDTO)
        {
            khachDTO = new KhachHangDTO();

            if (string.IsNullOrWhiteSpace(txtHoVaTen.Text))
            {
                MessageBox.Show("Họ và tên không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoVaTen.Focus();
                return false;
            }

            var soDienThoai = txtDienThoai.Text.Trim();
            if (!string.IsNullOrWhiteSpace(soDienThoai)
                && (!soDienThoai.All(char.IsDigit) || soDienThoai.Length is < 9 or > 11))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDienThoai.Focus();
                return false;
            }

            khachDTO = new KhachHangDTO
            {
                HoVaTen = txtHoVaTen.Text.Trim(),
                DienThoai = string.IsNullOrWhiteSpace(soDienThoai) ? null : soDienThoai,
                DiaChi = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim()
            };

            return true;
        }

        private void SelectRow(int id)
        {
            DataGridViewSelectionHelper.ClearSelection(dgvDanhSachKhach);

            foreach (DataGridViewRow row in dgvDanhSachKhach.Rows)
            {
                if (row.DataBoundItem is KhachHangDTO khach && khach.ID == id)
                {
                    row.Selected = true;
                    dgvDanhSachKhach.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }

        private void dgvDanhSachKhach_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachKhach.CurrentRow?.DataBoundItem is not KhachHangDTO khach)
            {
                return;
            }

            txtMaKhach.Text = khach.ID.ToString();
            txtHoVaTen.Text = khach.HoVaTen;
            txtDienThoai.Text = khach.DienThoai ?? string.Empty;
            txtDiaChi.Text = khach.DiaChi ?? string.Empty;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }
    }
}
