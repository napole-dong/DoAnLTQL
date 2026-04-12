using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.Mon;
using QuanLyQuanCaPhe.Services.UI;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiMon : Form
    {
        private readonly bool _isEmbedded;
        private readonly IMonService _monBUS;
        private readonly ILoaiMonService _loaiMonBUS;
        private readonly IPermissionService _permissionBUS;
        private readonly MonInputValidator _monInputValidator;
        private bool _isSaving;
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;

        public frmQuanLiMon(
            IMonService? monBUS = null,
            ILoaiMonService? loaiMonBUS = null,
            IPermissionService? permissionBUS = null,
            MonInputValidator? monInputValidator = null,
            bool isEmbedded = false)
        {
            _monBUS = AppServiceProvider.Resolve(monBUS, () => new MonBUS());
            _loaiMonBUS = AppServiceProvider.Resolve(loaiMonBUS, () => new LoaiMonBUS());
            _permissionBUS = AppServiceProvider.Resolve(permissionBUS, () => new PermissionBUS());
            _monInputValidator = monInputValidator ?? new MonInputValidator();
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

            dgvDanhSachMon.AutoGenerateColumns = false;
            colMaMon.DataPropertyName = nameof(MonDTO.ID);
            colTenMon.DataPropertyName = nameof(MonDTO.TenMon);
            colLoaiMon.DataPropertyName = nameof(MonDTO.TenLoaiMon);
            colDonGia.DataPropertyName = nameof(MonDTO.DonGiaHienThi);
            colTrangThai.DataPropertyName = nameof(MonDTO.TrangThaiHienThi);
            colMoTaMon.DataPropertyName = nameof(MonDTO.MoTa);

            dgvDanhSachLoaiMon.AutoGenerateColumns = false;
            colMaLoaiMon.DataPropertyName = nameof(LoaiMonDTO.ID);
            colTenLoaiMon.DataPropertyName = nameof(LoaiMonDTO.TenLoai);
            colSoLuongMon.DataPropertyName = nameof(LoaiMonDTO.SoMon);
            colMoTaLoaiMon.DataPropertyName = nameof(LoaiMonDTO.MoTa);

            Load += FrmQuanLiMon_Load;
            SizeChanged += FrmQuanLiMon_SizeChanged;
            panelDanhSachHeader.SizeChanged += (_, _) => CapNhatBoCucDanhSachHeader();

            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            tabDanhSach.SelectedIndexChanged += tabDanhSach_SelectedIndexChanged;

            dgvDanhSachMon.SelectionChanged += dgvDanhSachMon_SelectionChanged;
            dgvDanhSachMon.CellClick += dgvDanhSachMon_CellClick;
            dgvDanhSachLoaiMon.SelectionChanged += dgvDanhSachLoaiMon_SelectionChanged;
            dgvDanhSachLoaiMon.CellClick += dgvDanhSachLoaiMon_CellClick;

            btnThemLoaiMon.Click += btnThemLoaiMon_Click;
            btnCapNhatLoaiMon.Click += btnCapNhatLoaiMon_Click;
            btnXoaLoaiMon.Click += btnXoaLoaiMon_Click;

            btnXoaMon.Text = "Ngừng bán";

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            _btnQuanLyKhoSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NguyenLieu, () => new frmQuanLiKho(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            _btnKhachHangSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            _btnAuditLogSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmAuditLog(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            btnNhanVien.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NhanVien, () => new frmNhanVien(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            btnHoaDon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.HoaDon, () => new frmHoaDon(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            btnThongKe.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmThongKe(isEmbedded: true), onCurrentFormReactivated: RefreshAllData, skipNavigation: _isEmbedded);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void FrmQuanLiMon_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Quan ly mon"))
            {
                return;
            }

            try
            {
                _permissionBUS.EnsurePermission(PermissionFeatures.Menu, PermissionActions.View, "Bạn không có quyền truy cập chức năng Quản lý món.");
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
            ApDungPhanQuyenUI();
            ApplyRoundedUi();
            LoadLoaiMonComboBox();
            RefreshAllData();
        }

        private void HienThiNguoiDungDangNhap()
        {
        }

        private void ApDungPhanQuyenUI()
        {
            var coQuyenXemMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
            var coQuyenThemMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Create);
            var coQuyenCapNhatMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Update);
            var coQuyenXoaMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.Delete);

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
                btnQuanLyMon,
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

            btnThemMon.Visible = coQuyenThemMenu;
            btnCapNhatMon.Visible = coQuyenCapNhatMenu;
            btnXoaMon.Visible = coQuyenXoaMenu;

            btnThemLoaiMon.Visible = coQuyenThemMenu;
            btnCapNhatLoaiMon.Visible = coQuyenCapNhatMenu;
            btnXoaLoaiMon.Visible = coQuyenXoaMenu;

            var coQuyenSuaGia = _permissionBUS.CanEditMenuPrice();
            lblDonGia.Visible = coQuyenSuaGia;
            txtDonGia.Visible = coQuyenSuaGia;

            if (!coQuyenSuaGia)
            {
                txtDonGia.Text = "0";
            }
        }

        private void FrmQuanLiMon_SizeChanged(object? sender, EventArgs e)
        {
            ApplyRoundedUi();
            CapNhatBoCucDanhSachHeader();
        }

        private void ApplyRoundedUi()
        {
            ApplyRoundRegion(panelThongTin, 14);
            ApplyRoundRegion(panelDanhSach, 14);

            ApplyRoundRegion(cardTongMon, 14);
            ApplyRoundRegion(cardDangBan, 14);
            ApplyRoundRegion(cardTongLoai, 14);
            ApplyRoundRegion(cardNgungBan, 14);

            ApplyRoundRegion(btnThemMon, 8);
            ApplyRoundRegion(btnCapNhatMon, 8);
            ApplyRoundRegion(btnXoaMon, 8);
            ApplyRoundRegion(btnThemLoaiMon, 8);
            ApplyRoundRegion(btnCapNhatLoaiMon, 8);
            ApplyRoundRegion(btnXoaLoaiMon, 8);
        }

        private void LoadLoaiMonComboBox()
        {
            var dsLoaiMon = _monBUS.LayDanhSachLoaiMon();
            cboLoaiMon.DataSource = dsLoaiMon;
            cboLoaiMon.DisplayMember = nameof(LoaiMonDTO.TenLoai);
            cboLoaiMon.ValueMember = nameof(LoaiMonDTO.ID);
            cboLoaiMon.SelectedIndex = dsLoaiMon.Count > 0 ? 0 : -1;

            if (cboTrangThai.Items.Count > 0)
            {
                cboTrangThai.SelectedIndex = 0;
            }
        }

        private void CapNhatBoCucDanhSachHeader()
        {
            const int marginRight = 12;
            const int spacing = 6;
            const int actionRowY = 43;

            if (panelDanhSachHeader.Width <= 0)
            {
                return;
            }

            var right = panelDanhSachHeader.ClientSize.Width - marginRight;

            btnLamMoi.Location = new Point(Math.Max(0, right - btnLamMoi.Width), actionRowY);

            lblTimKiem.Location = new Point(14, actionRowY + 3);
            txtTimKiem.Location = new Point(lblTimKiem.Right + 8, actionRowY + 1);

            var widthTimKiem = btnLamMoi.Left - spacing - txtTimKiem.Left;
            txtTimKiem.Width = Math.Max(0, widthTimKiem);
        }

        private void RefreshAllData()
        {
            var maMonDangChon = dgvDanhSachMon.CurrentRow?.DataBoundItem is MonDTO monDangChon
                ? monDangChon.ID
                : (int?)null;
            var maLoaiDangChon = dgvDanhSachLoaiMon.CurrentRow?.DataBoundItem is LoaiMonDTO loaiDangChon
                ? loaiDangChon.ID
                : (int?)null;

            var tuKhoa = txtTimKiem.Text.Trim();
            var dsMon = _monBUS.LayDanhSachMon(tuKhoa, null);

            var dsLoai = _loaiMonBUS.LayDanhSachLoai(tuKhoa, null);

            dgvDanhSachMon.DataSource = dsMon;
            dgvDanhSachLoaiMon.DataSource = dsLoai;

            lblTongMonValue.Text = dsMon.Count.ToString();
            lblDangBanValue.Text = dsMon.Count(x => x.TrangThai == 1).ToString();
            lblTongLoaiValue.Text = dsLoai.Count.ToString();
            lblNgungBanValue.Text = dsMon.Count(x => x.TrangThai == 0).ToString();

            if (!maMonDangChon.HasValue || !SelectMonRow(maMonDangChon.Value))
            {
                SelectFirstMonRow();
            }

            if (!maLoaiDangChon.HasValue || !SelectLoaiRow(maLoaiDangChon.Value))
            {
                SelectFirstLoaiRow();
            }
        }

        private void txtTimKiem_TextChanged(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            RefreshAllData();
        }

        private void tabDanhSach_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabDanhSach.SelectedTab == tabMon)
            {
                ResetFormLoai();
                return;
            }

            ResetFormMon();
        }

        private void btnThemMon_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyQuanLiMon(true);

            if (!TryGetValidatedMon(out var monDTO))
            {
                _isSaving = false;
                DatTrangThaiDangXuLyQuanLiMon(false);
                return;
            }

            try
            {
                var result = _monBUS.ThemMon(monDTO);
                if (!result.ThanhCong || result.MonMoi == null)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RefreshAllData();
                SelectMonRow(result.MonMoi.ID);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyQuanLiMon(false);
            }
        }

        private void btnCapNhatMon_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!int.TryParse(txtMaMon.Text, out var maMon))
            {
                MessageBox.Show("Vui lòng chọn món cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryGetValidatedMon(out var monDTO))
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyQuanLiMon(true);

            try
            {
                monDTO.ID = maMon;
                var result = _monBUS.CapNhatMon(monDTO);
                if (!result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RefreshAllData();
                SelectMonRow(maMon);
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyQuanLiMon(false);
            }
        }

        private void btnXoaMon_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!int.TryParse(txtMaMon.Text, out var maMon))
            {
                MessageBox.Show("Vui lòng chọn món cần ngừng bán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDanhSachMon.CurrentRow?.DataBoundItem is not MonDTO mon)
            {
                MessageBox.Show("Không tìm thấy món để ngừng bán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (mon.IsDeleted)
            {
                MessageBox.Show("Món đã ngừng bán trước đó.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Ngừng bán món '{mon.TenMon}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyQuanLiMon(true);

            try
            {
                var result = _monBUS.XoaMon(maMon);
                if (!result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RefreshAllData();
                ResetFormMon();
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyQuanLiMon(false);
            }
        }

        private void btnThemLoaiMon_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyQuanLiMon(true);

            try
            {
                var result = _loaiMonBUS.ThemLoai(txtTenLoai.Text, txtMoTaLoai.Text);
                if (!result.ThanhCong || result.LoaiMoi == null)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RefreshAllData();
                SelectLoaiRow(result.LoaiMoi.ID);
                LoadLoaiMonComboBox();
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyQuanLiMon(false);
            }
        }

        private void btnCapNhatLoaiMon_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!int.TryParse(txtMaLoai.Text, out var maLoai))
            {
                MessageBox.Show("Vui lòng chọn loại món cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyQuanLiMon(true);

            try
            {
                var result = _loaiMonBUS.CapNhatLoai(maLoai, txtTenLoai.Text, txtMoTaLoai.Text);
                if (!result.ThanhCong)
                {
                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RefreshAllData();
                SelectLoaiRow(maLoai);
                LoadLoaiMonComboBox();
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyQuanLiMon(false);
            }
        }

        private void btnXoaLoaiMon_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (!int.TryParse(txtMaLoai.Text, out var maLoai))
            {
                MessageBox.Show("Vui lòng chọn loại món cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDanhSachLoaiMon.CurrentRow?.DataBoundItem is not LoaiMonDTO loai)
            {
                MessageBox.Show("Không tìm thấy loại món để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa loại món '{loai.TenLoai}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyQuanLiMon(true);

            try
            {
                var result = _loaiMonBUS.XoaLoai(maLoai);
                if (!result.ThanhCong)
                {
                    if (result.ThongBao.Contains("đang được sử dụng", StringComparison.OrdinalIgnoreCase))
                    {
                        var xacNhanChuyen = MessageBox.Show(
                            "Loại món này đang có món bên trong. Bạn có muốn chuyển các món sang loại khác rồi xóa không?",
                            "Ràng buộc dữ liệu",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (xacNhanChuyen == DialogResult.Yes)
                        {
                            var loaiDichId = ChonLoaiMonDich(maLoai);
                            if (!loaiDichId.HasValue)
                            {
                                return;
                            }

                            var chuyenResult = _loaiMonBUS.ChuyenMonSangLoaiKhac(maLoai, loaiDichId.Value);
                            if (!chuyenResult.ThanhCong)
                            {
                                MessageBox.Show(chuyenResult.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            var xoaSauKhiChuyen = _loaiMonBUS.XoaLoai(maLoai);
                            if (!xoaSauKhiChuyen.ThanhCong)
                            {
                                MessageBox.Show(xoaSauKhiChuyen.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            RefreshAllData();
                            LoadLoaiMonComboBox();
                            SelectLoaiRow(loaiDichId.Value);
                            MessageBox.Show("Đã chuyển món sang loại mới và xóa loại món cũ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RefreshAllData();
                ResetFormLoai();
                LoadLoaiMonComboBox();
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyQuanLiMon(false);
            }
        }

        private int? ChonLoaiMonDich(int loaiNguonId)
        {
            var dsLoaiDich = _loaiMonBUS
                .LayDanhSachLoai(null, null)
                .Where(x => x.ID != loaiNguonId)
                .ToList();

            if (dsLoaiDich.Count == 0)
            {
                MessageBox.Show("Không có loại món khác để chuyển dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            using var dialog = new Form
            {
                Text = "Chọn loại món đích",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                ClientSize = new Size(360, 140)
            };

            var lbl = new Label
            {
                Text = "Chuyển toàn bộ món sang loại:",
                AutoSize = true,
                Location = new Point(16, 16)
            };

            var cbo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(16, 42),
                Width = 328,
                DataSource = dsLoaiDich,
                DisplayMember = nameof(LoaiMonDTO.TenLoai),
                ValueMember = nameof(LoaiMonDTO.ID)
            };

            var btnDongY = new Button
            {
                Text = "Đồng ý",
                DialogResult = DialogResult.OK,
                Location = new Point(188, 88),
                Width = 75
            };

            var btnHuy = new Button
            {
                Text = "Hủy",
                DialogResult = DialogResult.Cancel,
                Location = new Point(269, 88),
                Width = 75
            };

            dialog.Controls.Add(lbl);
            dialog.Controls.Add(cbo);
            dialog.Controls.Add(btnDongY);
            dialog.Controls.Add(btnHuy);
            dialog.AcceptButton = btnDongY;
            dialog.CancelButton = btnHuy;

            return dialog.ShowDialog(this) == DialogResult.OK && cbo.SelectedValue is int loaiDichId
                ? loaiDichId
                : null;
        }

        private void DatTrangThaiDangXuLyQuanLiMon(bool dangXuLy)
        {
            UiLoadingStateHelper.Apply(
                this,
                dangXuLy,
                btnThemMon,
                btnCapNhatMon,
                btnXoaMon,
                btnThemLoaiMon,
                btnCapNhatLoaiMon,
                btnXoaLoaiMon,
                btnLamMoi,
                txtTimKiem,
                dgvDanhSachMon,
                dgvDanhSachLoaiMon,
                cboLoaiMon,
                cboTrangThai,
                txtTenMon,
                txtDonGia,
                txtTenLoai,
                txtMoTaLoai);

            if (!dangXuLy)
            {
                ApDungPhanQuyenUI();
            }
        }

        private bool TryGetValidatedMon(out MonDTO monDTO)
        {
            var validation = _monInputValidator.Validate(
                txtTenMon.Text,
                cboLoaiMon.SelectedValue,
                cboTrangThai.SelectedItem,
                txtDonGia.Text,
                string.Empty,
                string.Empty);

            if (validation.HopLe && validation.Mon != null)
            {
                monDTO = validation.Mon;
                return true;
            }

            monDTO = new MonDTO();
            MessageBox.Show(validation.ThongBao ?? "Dữ liệu không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void dgvDanhSachMon_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachMon.CurrentRow?.DataBoundItem is not MonDTO mon)
            {
                return;
            }

            HienThiThongTinMon(mon);
        }

        private void dgvDanhSachMon_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dgvDanhSachMon.Rows[e.RowIndex].DataBoundItem is MonDTO mon)
            {
                HienThiThongTinMon(mon);
            }
        }

        private void dgvDanhSachLoaiMon_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachLoaiMon.CurrentRow?.DataBoundItem is not LoaiMonDTO loaiMon)
            {
                return;
            }

            HienThiThongTinLoaiMon(loaiMon);
        }

        private void dgvDanhSachLoaiMon_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dgvDanhSachLoaiMon.Rows[e.RowIndex].DataBoundItem is LoaiMonDTO loaiMon)
            {
                HienThiThongTinLoaiMon(loaiMon);
            }
        }

        private bool SelectMonRow(int maMon)
        {
            dgvDanhSachMon.ClearSelection();

            foreach (DataGridViewRow row in dgvDanhSachMon.Rows)
            {
                if (row.DataBoundItem is not MonDTO mon || mon.ID != maMon)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachMon.CurrentCell = row.Cells[0];
                HienThiThongTinMon(mon);
                return true;
            }

            return false;
        }

        private bool SelectLoaiRow(int maLoai)
        {
            dgvDanhSachLoaiMon.ClearSelection();

            foreach (DataGridViewRow row in dgvDanhSachLoaiMon.Rows)
            {
                if (row.DataBoundItem is not LoaiMonDTO loaiMon || loaiMon.ID != maLoai)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachLoaiMon.CurrentCell = row.Cells[0];
                HienThiThongTinLoaiMon(loaiMon);
                return true;
            }

            return false;
        }

        private void SelectFirstMonRow()
        {
            if (dgvDanhSachMon.Rows.Count == 0)
            {
                ResetFormMon();
                return;
            }

            var row = dgvDanhSachMon.Rows[0];
            dgvDanhSachMon.ClearSelection();
            row.Selected = true;
            dgvDanhSachMon.CurrentCell = row.Cells[0];

            if (row.DataBoundItem is MonDTO mon)
            {
                HienThiThongTinMon(mon);
            }
        }

        private void SelectFirstLoaiRow()
        {
            if (dgvDanhSachLoaiMon.Rows.Count == 0)
            {
                ResetFormLoai();
                return;
            }

            var row = dgvDanhSachLoaiMon.Rows[0];
            dgvDanhSachLoaiMon.ClearSelection();
            row.Selected = true;
            dgvDanhSachLoaiMon.CurrentCell = row.Cells[0];

            if (row.DataBoundItem is LoaiMonDTO loaiMon)
            {
                HienThiThongTinLoaiMon(loaiMon);
            }
        }

        private void HienThiThongTinMon(MonDTO mon)
        {
            txtMaMon.Text = mon.ID.ToString();
            txtTenMon.Text = mon.TenMon;
            txtDonGia.Text = mon.DonGia.ToString();
            cboLoaiMon.SelectedValue = mon.LoaiMonID;

            var trangThaiIndex = cboTrangThai.FindStringExact(mon.TrangThaiHienThi);
            cboTrangThai.SelectedIndex = trangThaiIndex >= 0 ? trangThaiIndex : 0;
        }

        private void HienThiThongTinLoaiMon(LoaiMonDTO loaiMon)
        {
            txtMaLoai.Text = loaiMon.ID.ToString();
            txtTenLoai.Text = loaiMon.TenLoai;
            txtMoTaLoai.Text = loaiMon.MoTa;
        }

        private void ResetFormMon()
        {
            txtMaMon.Text = _monBUS.LayMaMonTiepTheo().ToString();
            txtTenMon.Clear();
            txtDonGia.Text = "0";

            if (cboLoaiMon.Items.Count > 0)
            {
                cboLoaiMon.SelectedIndex = 0;
            }

            if (cboTrangThai.Items.Count > 0)
            {
                cboTrangThai.SelectedIndex = 0;
            }
        }

        private void ResetFormLoai()
        {
            txtMaLoai.Text = _loaiMonBUS.LayMaLoaiTiepTheo().ToString();
            txtTenLoai.Clear();
            txtMoTaLoai.Clear();
        }

        private static void ApplyRoundRegion(Control? control, int radius)
        {
            if (control == null || control.Width <= 0 || control.Height <= 0)
            {
                return;
            }

            using var path = new GraphicsPath();
            var rect = new Rectangle(0, 0, control.Width, control.Height);
            var diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }
    }
}
