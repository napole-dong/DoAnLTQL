using System.ComponentModel;
using System.Linq;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.UI;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiKho : Form
    {
        private readonly bool _isEmbedded;
        private readonly INguyenLieuService _nguyenLieuBUS;
        private readonly IPermissionService _permissionBUS;
        private bool _isSaving;
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;
        private readonly BindingList<NhapKhoDongTamViewModel> _phieuNhapTam = new();
        private Panel? _panelNhapKhoNhieuDong;
        private ComboBox? _cboNhapNguyenLieu;
        private TextBox? _txtNhapSoLuong;
        private TextBox? _txtDonGiaNhap;
        private TextBox? _txtGhiChuNhapKho;
        private DataGridView? _dgvChiTietNhapKhoTam;
        private Button? _btnThemDongNhap;
        private Button? _btnXoaDongNhap;
        private Button? _btnLuuPhieuNhap;
        private Label? _lblTongTienPhieuNhap;

        private sealed class NhapKhoDongTamViewModel
        {
            public int NguyenLieuID { get; init; }
            public string TenNguyenLieu { get; init; } = string.Empty;
            public decimal SoLuong { get; init; }
            public decimal DonGiaNhap { get; init; }
            public decimal ThanhTien => SoLuong * DonGiaNhap;
        }

        private sealed class NguyenLieuNhapOption
        {
            public int NguyenLieuID { get; init; }
            public string HienThi { get; init; } = string.Empty;
        }

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

            KhoiTaoKhuVucNhapKhoNhieuDong();
            DatLaiPhieuNhapTam(xoaChiTiet: false);
        }

        private void frmQuanLiKho_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Quan ly kho"))
            {
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
            if (_permissionBUS.IsAdmin())
            {
                btnThemNguyenLieu.Visible = true;
                btnCapNhatNguyenLieu.Visible = true;
                btnXoaNguyenLieu.Visible = true;
                btnThemNguyenLieu.Enabled = true;
                btnCapNhatNguyenLieu.Enabled = true;
                btnXoaNguyenLieu.Enabled = true;
                txtTimNguyenLieu.Enabled = true;
                dgvDanhSachKho.Enabled = true;
                if (_panelNhapKhoNhieuDong != null)
                {
                    _panelNhapKhoNhieuDong.Visible = true;
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

                return;
            }

            var coQuyenKhoXem = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.View);
            var coQuyenKhoThem = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Create);
            var coQuyenKhoCapNhat = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Update);
            var coQuyenKhoXoa = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Delete);
            var coQuyenKhoNhap = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.NhapKho);

            btnThemNguyenLieu.Visible = coQuyenKhoThem;
            btnCapNhatNguyenLieu.Visible = coQuyenKhoCapNhat;
            btnXoaNguyenLieu.Visible = coQuyenKhoXoa;

            btnThemNguyenLieu.Enabled = btnThemNguyenLieu.Visible;
            btnCapNhatNguyenLieu.Enabled = btnCapNhatNguyenLieu.Visible;
            btnXoaNguyenLieu.Enabled = btnXoaNguyenLieu.Visible;
            if (_panelNhapKhoNhieuDong != null)
            {
                _panelNhapKhoNhieuDong.Visible = coQuyenKhoNhap;
            }

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
            var dsNguonNhapKho = _nguyenLieuBUS.LayDanhSachNguyenLieu(null);
            CapNhatNguonNguyenLieuNhapKho(dsNguonNhapKho);
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
                DatLaiPhieuNhapTam(xoaChiTiet: true);
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

            if (_panelNhapKhoNhieuDong?.Visible == true)
            {
                if (_cboNhapNguyenLieu != null)
                {
                    controlsCanToggle.Add(_cboNhapNguyenLieu);
                }

                if (_txtNhapSoLuong != null)
                {
                    controlsCanToggle.Add(_txtNhapSoLuong);
                }

                if (_txtDonGiaNhap != null)
                {
                    controlsCanToggle.Add(_txtDonGiaNhap);
                }

                if (_txtGhiChuNhapKho != null)
                {
                    controlsCanToggle.Add(_txtGhiChuNhapKho);
                }

                if (_dgvChiTietNhapKhoTam != null)
                {
                    controlsCanToggle.Add(_dgvChiTietNhapKhoTam);
                }

                if (_btnThemDongNhap != null)
                {
                    controlsCanToggle.Add(_btnThemDongNhap);
                }

                if (_btnXoaDongNhap != null)
                {
                    controlsCanToggle.Add(_btnXoaDongNhap);
                }

                if (_btnLuuPhieuNhap != null)
                {
                    controlsCanToggle.Add(_btnLuuPhieuNhap);
                }
            }

            UiLoadingStateHelper.Apply(
                this,
                dangXuLy,
                controlsCanToggle.ToArray());

            if (!dangXuLy)
            {
                ApDungPhanQuyenLenUI();
            }
        }

        private void KhoiTaoKhuVucNhapKhoNhieuDong()
        {
            _panelNhapKhoNhieuDong = new Panel
            {
                BackColor = Color.FromArgb(252, 249, 245),
                Dock = DockStyle.Top,
                Height = 250,
                Padding = new Padding(10, 10, 10, 10)
            };

            var lblTieuDe = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(62, 45, 36),
                Location = new Point(12, 10),
                Text = "Phiếu nhập kho nhiều dòng"
            };

            var lblNguyenLieu = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(88, 72, 62),
                Location = new Point(12, 42),
                Text = "Nguyên liệu"
            };

            _cboNhapNguyenLieu = new ComboBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(12, 62),
                Size = new Size(255, 28)
            };

            var lblSoLuongNhap = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(88, 72, 62),
                Location = new Point(275, 42),
                Text = "Số lượng"
            };

            _txtNhapSoLuong = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(275, 62),
                Size = new Size(105, 27)
            };

            var lblDonGiaNhap = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(88, 72, 62),
                Location = new Point(388, 42),
                Text = "Đơn giá"
            };

            _txtDonGiaNhap = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(388, 62),
                Size = new Size(130, 27)
            };

            _btnThemDongNhap = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(25, 135, 84),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(526, 60),
                Size = new Size(92, 31),
                Text = "Thêm dòng",
                UseVisualStyleBackColor = false
            };
            _btnThemDongNhap.FlatAppearance.BorderSize = 0;
            _btnThemDongNhap.Click += btnThemDongNhap_Click;

            _btnXoaDongNhap = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(220, 53, 69),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(626, 60),
                Size = new Size(92, 31),
                Text = "Xóa dòng",
                UseVisualStyleBackColor = false
            };
            _btnXoaDongNhap.FlatAppearance.BorderSize = 0;
            _btnXoaDongNhap.Click += btnXoaDongNhap_Click;

            _dgvChiTietNhapKhoTam = new DataGridView
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(12, 98),
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Size = new Size(706, 96)
            };

            _dgvChiTietNhapKhoTam.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NhapKhoDongTamViewModel.NguyenLieuID),
                    HeaderText = "Mã NL",
                    FillWeight = 18,
                    ReadOnly = true
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NhapKhoDongTamViewModel.TenNguyenLieu),
                    HeaderText = "Tên nguyên liệu",
                    FillWeight = 37,
                    ReadOnly = true
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NhapKhoDongTamViewModel.SoLuong),
                    HeaderText = "Số lượng",
                    FillWeight = 15,
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NhapKhoDongTamViewModel.DonGiaNhap),
                    HeaderText = "Đơn giá",
                    FillWeight = 15,
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(NhapKhoDongTamViewModel.ThanhTien),
                    HeaderText = "Thành tiền",
                    FillWeight = 15,
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
                });
            _dgvChiTietNhapKhoTam.DataSource = _phieuNhapTam;

            var lblGhiChu = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(88, 72, 62),
                Location = new Point(12, 203),
                Text = "Ghi chú"
            };

            _txtGhiChuNhapKho = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(67, 200),
                Size = new Size(260, 27)
            };

            _lblTongTienPhieuNhap = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                ForeColor = Color.FromArgb(88, 72, 62),
                Location = new Point(334, 203),
                Text = "Tổng: 0"
            };

            _btnLuuPhieuNhap = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(13, 110, 253),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(594, 198),
                Size = new Size(124, 33),
                Text = "Lưu phiếu nhập",
                UseVisualStyleBackColor = false
            };
            _btnLuuPhieuNhap.FlatAppearance.BorderSize = 0;
            _btnLuuPhieuNhap.Click += btnLuuPhieuNhap_Click;

            _panelNhapKhoNhieuDong.Controls.Add(lblTieuDe);
            _panelNhapKhoNhieuDong.Controls.Add(lblNguyenLieu);
            _panelNhapKhoNhieuDong.Controls.Add(_cboNhapNguyenLieu);
            _panelNhapKhoNhieuDong.Controls.Add(lblSoLuongNhap);
            _panelNhapKhoNhieuDong.Controls.Add(_txtNhapSoLuong);
            _panelNhapKhoNhieuDong.Controls.Add(lblDonGiaNhap);
            _panelNhapKhoNhieuDong.Controls.Add(_txtDonGiaNhap);
            _panelNhapKhoNhieuDong.Controls.Add(_btnThemDongNhap);
            _panelNhapKhoNhieuDong.Controls.Add(_btnXoaDongNhap);
            _panelNhapKhoNhieuDong.Controls.Add(_dgvChiTietNhapKhoTam);
            _panelNhapKhoNhieuDong.Controls.Add(lblGhiChu);
            _panelNhapKhoNhieuDong.Controls.Add(_txtGhiChuNhapKho);
            _panelNhapKhoNhieuDong.Controls.Add(_lblTongTienPhieuNhap);
            _panelNhapKhoNhieuDong.Controls.Add(_btnLuuPhieuNhap);

            panelDanhSachKho.Controls.Add(_panelNhapKhoNhieuDong);
            panelDanhSachKho.Controls.SetChildIndex(panelDanhSachHeader, 0);
            panelDanhSachKho.Controls.SetChildIndex(_panelNhapKhoNhieuDong, 1);
            panelDanhSachKho.Controls.SetChildIndex(dgvDanhSachKho, 2);
        }

        private void CapNhatNguonNguyenLieuNhapKho(IEnumerable<NguyenLieuDTO> dsNguyenLieu)
        {
            if (_cboNhapNguyenLieu == null)
            {
                return;
            }

            var luaChonHienTai = _cboNhapNguyenLieu.SelectedItem as NguyenLieuNhapOption;
            var nguonNhap = dsNguyenLieu
                .Where(x => x.TrangThai != 0)
                .OrderBy(x => x.TenNguyenLieu)
                .Select(x => new NguyenLieuNhapOption
                {
                    NguyenLieuID = x.MaNguyenLieu,
                    HienThi = $"{x.TenNguyenLieu} ({x.DonViTinh})"
                })
                .ToList();

            _cboNhapNguyenLieu.DataSource = null;
            _cboNhapNguyenLieu.DisplayMember = nameof(NguyenLieuNhapOption.HienThi);
            _cboNhapNguyenLieu.ValueMember = nameof(NguyenLieuNhapOption.NguyenLieuID);
            _cboNhapNguyenLieu.DataSource = nguonNhap;

            if (nguonNhap.Count == 0)
            {
                return;
            }

            if (luaChonHienTai != null)
            {
                var match = nguonNhap.FirstOrDefault(x => x.NguyenLieuID == luaChonHienTai.NguyenLieuID);
                if (match != null)
                {
                    _cboNhapNguyenLieu.SelectedItem = match;
                    return;
                }
            }

            _cboNhapNguyenLieu.SelectedIndex = 0;
        }

        private void btnThemDongNhap_Click(object? sender, EventArgs e)
        {
            if (_isSaving || _cboNhapNguyenLieu == null || _txtNhapSoLuong == null || _txtDonGiaNhap == null)
            {
                return;
            }

            if (_cboNhapNguyenLieu.SelectedItem is not NguyenLieuNhapOption nguyenLieuChon)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu để thêm vào phiếu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(_txtNhapSoLuong.Text.Trim(), out var soLuongNhap) || soLuongNhap <= 0)
            {
                MessageBox.Show("Số lượng nhập phải lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtNhapSoLuong.Focus();
                return;
            }

            if (!decimal.TryParse(_txtDonGiaNhap.Text.Trim(), out var donGiaNhap) || donGiaNhap < 0)
            {
                MessageBox.Show("Đơn giá nhập không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtDonGiaNhap.Focus();
                return;
            }

            if (_phieuNhapTam.Any(x => x.NguyenLieuID == nguyenLieuChon.NguyenLieuID))
            {
                MessageBox.Show("Nguyên liệu này đã có trong phiếu nhập. Vui lòng xóa dòng cũ nếu muốn nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _phieuNhapTam.Add(new NhapKhoDongTamViewModel
            {
                NguyenLieuID = nguyenLieuChon.NguyenLieuID,
                TenNguyenLieu = nguyenLieuChon.HienThi,
                SoLuong = soLuongNhap,
                DonGiaNhap = donGiaNhap
            });

            CapNhatTongTienPhieuNhap();
            _txtNhapSoLuong.Text = "0";
            _txtDonGiaNhap.Text = "0";
            _txtNhapSoLuong.Focus();
        }

        private void btnXoaDongNhap_Click(object? sender, EventArgs e)
        {
            if (_isSaving || _dgvChiTietNhapKhoTam?.CurrentRow?.DataBoundItem is not NhapKhoDongTamViewModel dongNhap)
            {
                return;
            }

            _phieuNhapTam.Remove(dongNhap);
            CapNhatTongTienPhieuNhap();
        }

        private void btnLuuPhieuNhap_Click(object? sender, EventArgs e)
        {
            if (_isSaving)
            {
                return;
            }

            if (_phieuNhapTam.Count == 0)
            {
                MessageBox.Show("Phiếu nhập kho chưa có dòng chi tiết nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _isSaving = true;
            DatTrangThaiDangXuLyKho(true);

            try
            {
                var dsChiTiet = _phieuNhapTam
                    .Select(x => new NhapKhoChiTietDTO
                    {
                        NguyenLieuID = x.NguyenLieuID,
                        SoLuong = x.SoLuong,
                        DonGiaNhap = x.DonGiaNhap
                    })
                    .ToList();

                var ketQua = _nguyenLieuBUS.NhapKhoNhieuNguyenLieu(dsChiTiet, _txtGhiChuNhapKho?.Text);
                if (!ketQua.ThanhCong)
                {
                    MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DatLaiPhieuNhapTam(xoaChiTiet: true);
                LoadDanhSachKho();
                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                _isSaving = false;
                DatTrangThaiDangXuLyKho(false);
            }
        }

        private void DatLaiPhieuNhapTam(bool xoaChiTiet)
        {
            if (xoaChiTiet)
            {
                _phieuNhapTam.Clear();
            }

            if (_txtNhapSoLuong != null)
            {
                _txtNhapSoLuong.Text = "0";
            }

            if (_txtDonGiaNhap != null)
            {
                _txtDonGiaNhap.Text = "0";
            }

            if (_txtGhiChuNhapKho != null)
            {
                _txtGhiChuNhapKho.Clear();
            }

            CapNhatTongTienPhieuNhap();
        }

        private void CapNhatTongTienPhieuNhap()
        {
            if (_lblTongTienPhieuNhap == null)
            {
                return;
            }

            var tongTien = _phieuNhapTam.Sum(x => x.ThanhTien);
            _lblTongTienPhieuNhap.Text = $"Tổng: {tongTien:N0}";
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
