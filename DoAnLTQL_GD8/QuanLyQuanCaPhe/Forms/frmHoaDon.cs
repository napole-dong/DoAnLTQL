using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.HoaDon;
using QuanLyQuanCaPhe.Services.Navigation;
using System.ComponentModel;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmHoaDon : Form
    {
        private const string InvoiceConcurrencyMessage = "Dữ liệu đã bị thay đổi bởi nhân viên khác. Vui lòng tải lại!";
        private readonly bool _isEmbedded;
        private readonly HoaDonBUS _hoaDonBUS = new();
        private readonly OrderService _orderService = new();
        private readonly PermissionBUS _permissionBUS = new();
        private readonly HoaDonPreviewService _hoaDonPreviewService = new();
        private readonly HoaDonTienService _hoaDonTienService = new();
        private readonly HoaDonFormStateService _hoaDonFormStateService = new();

        private HoaDonManHinhState _manHinhState = HoaDonManHinhState.Xem;
        private bool _dangNapDuLieu;
        private bool _dangXuLyHuyHoaDon;
        private bool _dangXuLyThuTien;
        private bool _dangXuLyLuuHoaDon;
        private bool _dangXuLyChiTietHoaDon;
        private int? _hoaDonDangChonId;
        private byte[]? _hoaDonDangChonRowVersion;
        private decimal _tongTienDangChon;
        private BindingList<HoaDonChiTietDTO> _chiTietBindingList = new();
        private bool _dangXuLyThayDoiChiTiet;
        private short _soLuongCuTruocKhiSua;
        private int _monIdDangSua;
        private readonly ContextMenuStrip _menuChiTietHoaDon = new();

        private sealed class TrangThaiHoaDonOption
        {
            public int Value { get; set; }
            public string Text { get; set; } = string.Empty;
        }

        public frmHoaDon(bool isEmbedded = false)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();
            CauHinhBangDuLieu();
            CauHinhSuKien();
        }

        private void CauHinhBangDuLieu()
        {
            dgvDanhSachHoaDon.AutoGenerateColumns = false;
            colMaHoaDon.DataPropertyName = nameof(HoaDonDTO.MaHoaDonHienThi);
            colNgayLap.DataPropertyName = nameof(HoaDonDTO.NgayLapHienThi);
            colBanKhach.DataPropertyName = nameof(HoaDonDTO.BanKhachHienThi);
            colTongTien.DataPropertyName = nameof(HoaDonDTO.TongTienHienThi);
            colTrangThaiHoaDon.DataPropertyName = nameof(HoaDonDTO.TrangThaiText);
            colNhanVienLap.DataPropertyName = nameof(HoaDonDTO.TenNhanVien);

            dgvChiTietHoaDon.AutoGenerateColumns = false;
            colTenMon.DataPropertyName = nameof(HoaDonChiTietDTO.TenMon);
            colSoLuong.DataPropertyName = nameof(HoaDonChiTietDTO.SoLuong);
            colDonGia.DataPropertyName = nameof(HoaDonChiTietDTO.DonGiaHienThi);
            colThanhTien.DataPropertyName = nameof(HoaDonChiTietDTO.ThanhTienHienThi);
            colSoLuong.ReadOnly = false;
            dgvChiTietHoaDon.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvChiTietHoaDon.AllowUserToDeleteRows = false;

            KhoiTaoMenuChiTietHoaDon();

            cboBanKhach.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMon.DropDownStyle = ComboBoxStyle.DropDownList;

            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = short.MaxValue;
        }

        private void CauHinhSuKien()
        {
            Load += frmHoaDon_Load;
            dgvDanhSachHoaDon.SelectionChanged += dgvDanhSachHoaDon_SelectionChanged;
            dgvChiTietHoaDon.CellBeginEdit += dgvChiTietHoaDon_CellBeginEdit;
            dgvChiTietHoaDon.CellValidating += dgvChiTietHoaDon_CellValidating;
            dgvChiTietHoaDon.CellValueChanged += dgvChiTietHoaDon_CellValueChanged;
            dgvChiTietHoaDon.SelectionChanged += dgvChiTietHoaDon_SelectionChanged;
            dgvChiTietHoaDon.RowsRemoved += dgvChiTietHoaDon_RowsRemoved;
            dgvChiTietHoaDon.UserDeletedRow += dgvChiTietHoaDon_UserDeletedRow;
            dgvChiTietHoaDon.CellMouseDown += dgvChiTietHoaDon_CellMouseDown;
            dgvChiTietHoaDon.KeyDown += dgvChiTietHoaDon_KeyDown;
            txtTimKiemHoaDon.KeyDown += txtTimKiemHoaDon_KeyDown;
            btnLocXem.Click += (_, _) => TaiDanhSachHoaDon();
            btnLamMoi.Click += btnLamMoi_Click;

            btnThemMoi.Click += btnThemMoi_Click;
            btnSua.Click += btnSua_Click;
            btnLuu.Click += btnLuu_Click;
            btnBoQua.Click += btnBoQua_Click;
            btnXoaHuy.Click += btnXoaHuy_Click;

            btnThemMonVaoHoaDon.Click += btnThemMonVaoHoaDon_Click;
            btnXacNhanThuTien.Click += btnXacNhanThuTien_Click;
            btnInHoaDon.Click += btnInHoaDon_Click;

            txtTienKhachDua.TextChanged += txtTienKhachDua_TextChanged;

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnKhachHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnNhanVien.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NhanVien, () => new frmNhanVien(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnHoaDon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.HoaDon, () => new frmHoaDon(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnThongKe.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmThongKe(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void KhoiTaoMenuChiTietHoaDon()
        {
            _menuChiTietHoaDon.Items.Clear();

            var menuXoaMon = new ToolStripMenuItem("Xóa món đã chọn");
            menuXoaMon.Click += async (_, _) => await ThuXoaMonDangChonAsync();

            var menuDoiMon = new ToolStripMenuItem("Đổi món theo món đang chọn");
            menuDoiMon.Click += async (_, _) => await ThuDoiMonDangChonTheoMonDaChonAsync();

            _menuChiTietHoaDon.Items.Add(menuDoiMon);
            _menuChiTietHoaDon.Items.Add(menuXoaMon);

            dgvChiTietHoaDon.ContextMenuStrip = _menuChiTietHoaDon;
        }

        private void frmHoaDon_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Hoa don"))
            {
                return;
            }

            try
            {
                _permissionBUS.EnsurePermission(PermissionFeatures.HoaDon, PermissionActions.View, "Ban khong co quyen truy cap chuc nang hoa don.");
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

            HienThiNguoiDungDangNhap();
            ApDungPhanQuyenDieuHuong();
            KhoiTaoComboTrangThaiLoc();
            KhoiTaoBoLocMacDinh();
            KhoiTaoComboTrangThai();
            TaiDanhSachBanKhach();
            TaiDanhSachMon();
            TaiDanhSachHoaDon(giuHoaDonDangChon: false);
            ChuyenStateManHinh(HoaDonManHinhState.Xem);
        }

        private void ApDungPhanQuyenDieuHuong()
        {
            if (_permissionBUS.IsAdmin())
            {
                btnBanHang.Visible = true;
                btnQuanLyBan.Visible = true;
                btnQuanLyMon.Visible = true;
                btnCongThuc.Visible = true;
                btnKhachHang.Visible = true;
                btnNhanVien.Visible = true;
                btnHoaDon.Visible = true;
                btnThongKe.Visible = true;
                return;
            }

            var coQuyenMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
            var coQuyenKhachHang = _permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.View);
            btnBanHang.Visible = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View);
            btnQuanLyBan.Visible = coQuyenMenu;
            btnQuanLyMon.Visible = coQuyenMenu;
            btnCongThuc.Visible = coQuyenMenu;
            btnKhachHang.Visible = coQuyenKhachHang;
            btnNhanVien.Visible = _permissionBUS.CanManageEmployees();
            btnHoaDon.Visible = _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);
            btnThongKe.Visible = _permissionBUS.CanViewReport();
        }

        private void HienThiNguoiDungDangNhap()
        {
        }

        private void KhoiTaoBoLocMacDinh()
        {
            dtpTuNgay.Value = DateTime.Today.AddDays(-30);
            dtpDenNgay.Value = DateTime.Today;
            cboTrangThaiLoc.SelectedIndex = 0;
        }

        private void KhoiTaoComboTrangThaiLoc()
        {
            cboTrangThaiLoc.Items.Clear();
            cboTrangThaiLoc.Items.AddRange(new object[]
            {
                "Tất cả",
                "Draft",
                "Paid",
                "Closed",
                "Cancelled"
            });
        }

        private void KhoiTaoComboTrangThai()
        {
            var dsTrangThai = new List<TrangThaiHoaDonOption>
            {
                new() { Value = (int)HoaDonTrangThai.Draft, Text = HoaDonBUS.ChuyenTrangThaiHoaDon((int)HoaDonTrangThai.Draft) },
                new() { Value = (int)HoaDonTrangThai.Paid, Text = HoaDonBUS.ChuyenTrangThaiHoaDon((int)HoaDonTrangThai.Paid) },
                new() { Value = (int)HoaDonTrangThai.Closed, Text = HoaDonBUS.ChuyenTrangThaiHoaDon((int)HoaDonTrangThai.Closed) },
                new() { Value = (int)HoaDonTrangThai.Cancelled, Text = HoaDonBUS.ChuyenTrangThaiHoaDon((int)HoaDonTrangThai.Cancelled) }
            };

            cboTrangThai.DataSource = dsTrangThai;
            cboTrangThai.DisplayMember = nameof(TrangThaiHoaDonOption.Text);
            cboTrangThai.ValueMember = nameof(TrangThaiHoaDonOption.Value);
            cboTrangThai.SelectedValue = (int)HoaDonTrangThai.Draft;
            cboTrangThai.Enabled = false;
        }

        private void TaiDanhSachBanKhach()
        {
            var dsBanKhach = _hoaDonBUS.LayDanhSachBanKhach();

            cboBanKhach.DataSource = dsBanKhach;
            cboBanKhach.DisplayMember = nameof(HoaDonBanKhachItemDTO.TenHienThi);
            cboBanKhach.ValueMember = nameof(HoaDonBanKhachItemDTO.BanID);

            if (dsBanKhach.Count > 0)
            {
                cboBanKhach.SelectedIndex = 0;
            }
        }

        private void TaiDanhSachMon()
        {
            var dsMon = _hoaDonBUS.LayDanhSachMonDangKinhDoanh();

            cboMon.DataSource = dsMon;
            cboMon.DisplayMember = nameof(HoaDonMonItemDTO.TenHienThi);
            cboMon.ValueMember = nameof(HoaDonMonItemDTO.MonID);

            if (dsMon.Count > 0)
            {
                cboMon.SelectedIndex = 0;
            }
        }

        private void TaiDanhSachHoaDon(bool giuHoaDonDangChon = true)
        {
            var boLoc = new HoaDonFilterDTO
            {
                TuKhoa = txtTimKiemHoaDon.Text,
                TuNgay = dtpTuNgay.Value,
                DenNgay = dtpDenNgay.Value,
                TrangThai = HoaDonBUS.ChuyenTextSangTrangThaiLoc(cboTrangThaiLoc.SelectedItem?.ToString())
            };

            var dsHoaDon = _hoaDonBUS.LayDanhSachHoaDon(boLoc);

            _dangNapDuLieu = true;
            try
            {
                dgvDanhSachHoaDon.DataSource = null;
                dgvDanhSachHoaDon.DataSource = dsHoaDon;

                var mucTieuId = giuHoaDonDangChon ? _hoaDonDangChonId : null;

                var daChonHoaDonMucTieu = mucTieuId.HasValue && ChonDongHoaDon(mucTieuId.Value);

                if (!daChonHoaDonMucTieu && dgvDanhSachHoaDon.Rows.Count > 0)
                {
                    dgvDanhSachHoaDon.Rows[0].Selected = true;
                    dgvDanhSachHoaDon.CurrentCell = dgvDanhSachHoaDon.Rows[0].Cells[0];
                }
                else if (dgvDanhSachHoaDon.Rows.Count == 0)
                {
                    _hoaDonDangChonId = null;
                    HienThiTrangThaiKhongCoHoaDon();
                }
            }
            finally
            {
                _dangNapDuLieu = false;
            }

            TaiChiTietTheoDongDangChon();
        }

        private bool ChonDongHoaDon(int hoaDonId)
        {
            foreach (DataGridViewRow row in dgvDanhSachHoaDon.Rows)
            {
                if (row.DataBoundItem is not HoaDonDTO hoaDon || hoaDon.ID != hoaDonId)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachHoaDon.CurrentCell = row.Cells[0];
                return true;
            }

            return false;
        }

        private void dgvDanhSachHoaDon_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            TaiChiTietTheoDongDangChon();
        }

        private void dgvChiTietHoaDon_SelectionChanged(object? sender, EventArgs e)
        {
            DongBoSoLuongTuDongChiTietDangChon();
        }

        private void DongBoSoLuongTuDongChiTietDangChon()
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            if (dgvChiTietHoaDon.CurrentRow?.DataBoundItem is not HoaDonChiTietDTO chiTiet)
            {
                return;
            }

            var soLuongHopLe = Math.Clamp((decimal)chiTiet.SoLuong, nudSoLuong.Minimum, nudSoLuong.Maximum);
            if (nudSoLuong.Value != soLuongHopLe)
            {
                nudSoLuong.Value = soLuongHopLe;
            }
        }

        private void TaiChiTietTheoDongDangChon()
        {
            if (dgvDanhSachHoaDon.CurrentRow?.DataBoundItem is not HoaDonDTO hoaDonGrid)
            {
                _hoaDonDangChonId = null;
                HienThiTrangThaiKhongCoHoaDon();
                return;
            }

            _hoaDonDangChonId = hoaDonGrid.ID;
            var hoaDon = _hoaDonBUS.LayHoaDonTheoId(hoaDonGrid.ID);
            if (hoaDon == null)
            {
                HienThiTrangThaiKhongCoHoaDon();
                return;
            }

            _dangNapDuLieu = true;
            try
            {
                txtMaHoaDon.Text = hoaDon.MaHoaDonHienThi;
                dtpNgayTao.Value = hoaDon.NgayLap;

                if (cboBanKhach.Items.Count > 0)
                {
                    cboBanKhach.SelectedValue = hoaDon.BanID;
                }

                cboTrangThai.SelectedValue = hoaDon.TrangThai;

                _chiTietBindingList = new BindingList<HoaDonChiTietDTO>(hoaDon.ChiTiet);
                dgvChiTietHoaDon.DataSource = null;
                dgvChiTietHoaDon.DataSource = _chiTietBindingList;

                _hoaDonDangChonRowVersion = hoaDon.RowVersion?.ToArray();
                _tongTienDangChon = hoaDon.TongTien;
                CapNhatTongTien();

                txtTienKhachDua.Clear();
                lblTienThoiValue.Text = _hoaDonTienService.DinhDangTien(0);

                CapNhatDieuKienXuLyNut(hoaDon);
            }
            finally
            {
                _dangNapDuLieu = false;
            }

            DongBoSoLuongTuDongChiTietDangChon();
        }

        private void HienThiTrangThaiKhongCoHoaDon()
        {
            txtMaHoaDon.Text = string.Empty;
            dtpNgayTao.Value = DateTime.Now;
            cboTrangThai.SelectedValue = (int)HoaDonTrangThai.ChuaThanhToan;
            _hoaDonDangChonRowVersion = null;

            _chiTietBindingList = new BindingList<HoaDonChiTietDTO>();
            dgvChiTietHoaDon.DataSource = null;
            dgvChiTietHoaDon.DataSource = _chiTietBindingList;
            nudSoLuong.Value = nudSoLuong.Minimum;
            _tongTienDangChon = 0;
            lblTongTienValue.Text = _hoaDonTienService.DinhDangTien(0);
            txtTienKhachDua.Clear();
            lblTienThoiValue.Text = _hoaDonTienService.DinhDangTien(0);

            CapNhatDieuKienXuLyNut(null);
        }

        private void txtTimKiemHoaDon_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            TaiDanhSachHoaDon();
            e.SuppressKeyPress = true;
        }

        private void dgvChiTietHoaDon_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            if (_dangNapDuLieu || e.RowIndex < 0 || e.ColumnIndex != colSoLuong.Index)
            {
                return;
            }

            if (dgvChiTietHoaDon.Rows[e.RowIndex].DataBoundItem is not HoaDonChiTietDTO chiTiet)
            {
                return;
            }

            _soLuongCuTruocKhiSua = chiTiet.SoLuong;
            _monIdDangSua = chiTiet.MonID;
        }

        private void dgvChiTietHoaDon_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (_dangNapDuLieu || _dangXuLyThayDoiChiTiet || e.RowIndex < 0 || e.ColumnIndex != colSoLuong.Index)
            {
                return;
            }

            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null || hoaDon.TrangThai != (int)HoaDonTrangThai.ChuaThanhToan)
            {
                MessageBox.Show("Chỉ cập nhật số lượng cho hóa đơn chưa thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }

            var textSoLuong = (e.FormattedValue?.ToString() ?? string.Empty).Trim();
            if (!short.TryParse(textSoLuong, out var soLuongMoi) || soLuongMoi < 0)
            {
                MessageBox.Show("Số lượng món phải lớn hơn hoặc bằng 0 (nhập 0 để xóa món).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private async void dgvChiTietHoaDon_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (_dangNapDuLieu || _dangXuLyThayDoiChiTiet || e.RowIndex < 0 || e.ColumnIndex != colSoLuong.Index)
            {
                return;
            }

            if (dgvChiTietHoaDon.Rows[e.RowIndex].DataBoundItem is not HoaDonChiTietDTO chiTiet)
            {
                return;
            }

            await XuLyThayDoiSoLuongChiTietAsync(chiTiet);
        }

        private void dgvChiTietHoaDon_RowsRemoved(object? sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            CapNhatTongTien();
        }

        private void dgvChiTietHoaDon_UserDeletedRow(object? sender, DataGridViewRowEventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            CapNhatTongTien();
        }

        private void dgvChiTietHoaDon_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.Button != MouseButtons.Right)
            {
                return;
            }

            dgvChiTietHoaDon.ClearSelection();
            dgvChiTietHoaDon.Rows[e.RowIndex].Selected = true;
            dgvChiTietHoaDon.CurrentCell = dgvChiTietHoaDon.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private async void dgvChiTietHoaDon_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
            {
                return;
            }

            await ThuXoaMonDangChonAsync();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private async Task XuLyThayDoiSoLuongChiTietAsync(HoaDonChiTietDTO chiTiet)
        {
            if (_dangXuLyThayDoiChiTiet
                || _dangXuLyChiTietHoaDon
                || _dangXuLyLuuHoaDon
                || _dangXuLyThuTien
                || _dangXuLyHuyHoaDon)
            {
                return;
            }

            _dangXuLyChiTietHoaDon = true;
            CapNhatTrangThaiDangXuLyHoaDon();

            try
            {
                if (!TryLayHoaDonDangChonCoTheChinhSua(out var hoaDon))
                {
                    TaiDanhSachHoaDon();
                    return;
                }

                if (chiTiet.SoLuong < 0)
                {
                    MessageBox.Show("Số lượng món phải lớn hơn hoặc bằng 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    KhoiPhucSoLuongCu(chiTiet);
                    return;
                }

                if (chiTiet.SoLuong == 0)
                {
                    var xacNhan = MessageBox.Show(
                        $"Xóa món '{chiTiet.TenMon}' khỏi hóa đơn?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (xacNhan != DialogResult.Yes)
                    {
                        KhoiPhucSoLuongCu(chiTiet);
                        return;
                    }

                    var soLuongCanXoa = (short)Math.Clamp(_soLuongCuTruocKhiSua > 0 ? _soLuongCuTruocKhiSua : 1, 1, short.MaxValue);
                    var ketQuaXoa = await Task.Run(() => _orderService.RemoveItemFromOrder(
                        hoaDon.ID,
                        chiTiet.MonID,
                        soLuongCanXoa,
                        _hoaDonDangChonRowVersion));

                    if (!ketQuaXoa.ThanhCong)
                    {
                        if (LaThongBaoXungDotDuLieu(ketQuaXoa.ThongBao))
                        {
                            TaiDanhSachHoaDon();
                        }
                        else
                        {
                            KhoiPhucSoLuongCu(chiTiet);
                        }

                        MessageBox.Show(ketQuaXoa.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _dangXuLyThayDoiChiTiet = true;
                    try
                    {
                        _chiTietBindingList.Remove(chiTiet);
                        _chiTietBindingList.ResetBindings();
                    }
                    finally
                    {
                        _dangXuLyThayDoiChiTiet = false;
                    }

                    CapNhatTongTien();
                    TaiDanhSachHoaDon();
                    return;
                }

                if (_monIdDangSua == chiTiet.MonID && _soLuongCuTruocKhiSua > 0 && chiTiet.SoLuong == _soLuongCuTruocKhiSua)
                {
                    CapNhatTongTien();
                    return;
                }

                var ketQuaCapNhat = await Task.Run(() => _hoaDonBUS.CapNhatSoLuongMonTrongHoaDon(
                    hoaDon.ID,
                    chiTiet.MonID,
                    chiTiet.SoLuong,
                    _hoaDonDangChonRowVersion));

                if (!ketQuaCapNhat.ThanhCong)
                {
                    if (LaThongBaoXungDotDuLieu(ketQuaCapNhat.ThongBao))
                    {
                        TaiDanhSachHoaDon();
                    }
                    else
                    {
                        KhoiPhucSoLuongCu(chiTiet);
                    }

                    MessageBox.Show(ketQuaCapNhat.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _chiTietBindingList.ResetBindings();
                CapNhatTongTien();
                TaiDanhSachHoaDon();
            }
            finally
            {
                _dangXuLyChiTietHoaDon = false;
                CapNhatTrangThaiDangXuLyHoaDon();
                _soLuongCuTruocKhiSua = 0;
                _monIdDangSua = 0;
            }
        }

        private void KhoiPhucSoLuongCu(HoaDonChiTietDTO chiTiet)
        {
            if (_soLuongCuTruocKhiSua <= 0)
            {
                return;
            }

            _dangXuLyThayDoiChiTiet = true;
            try
            {
                chiTiet.SoLuong = _soLuongCuTruocKhiSua;
                _chiTietBindingList.ResetBindings();
            }
            finally
            {
                _dangXuLyThayDoiChiTiet = false;
            }

            CapNhatTongTien();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimKiemHoaDon.Clear();
            KhoiTaoBoLocMacDinh();
            TaiDanhSachHoaDon(giuHoaDonDangChon: false);
        }

        private void btnThemMoi_Click(object? sender, EventArgs e)
        {
            ChuyenStateManHinh(HoaDonManHinhState.ThemMoi);
            HienThiManHinhTaoMoi();
        }

        private void HienThiManHinhTaoMoi()
        {
            _dangNapDuLieu = true;
            try
            {
                _hoaDonDangChonId = null;
                _hoaDonDangChonRowVersion = null;
                txtMaHoaDon.Text = HoaDonBUS.DinhDangMaHoaDon(_hoaDonBUS.LayMaHoaDonTiepTheo());
                dtpNgayTao.Value = DateTime.Now;

                if (cboBanKhach.Items.Count > 0)
                {
                    cboBanKhach.SelectedIndex = 0;
                }

                cboTrangThai.SelectedValue = (int)HoaDonTrangThai.ChuaThanhToan;

                _chiTietBindingList = new BindingList<HoaDonChiTietDTO>();
                dgvChiTietHoaDon.DataSource = null;
                dgvChiTietHoaDon.DataSource = _chiTietBindingList;
                nudSoLuong.Value = nudSoLuong.Minimum;
                _tongTienDangChon = 0;
                lblTongTienValue.Text = _hoaDonTienService.DinhDangTien(0);
                txtTienKhachDua.Clear();
                lblTienThoiValue.Text = _hoaDonTienService.DinhDangTien(0);
            }
            finally
            {
                _dangNapDuLieu = false;
            }
        }

        private void btnSua_Click(object? sender, EventArgs e)
        {
            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hoaDon.TrangThai != (int)HoaDonTrangThai.ChuaThanhToan)
            {
                MessageBox.Show("Chỉ sửa được hóa đơn chưa thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ChuyenStateManHinh(HoaDonManHinhState.ChinhSua);
        }

        private async void btnLuu_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyLuuHoaDon || _dangXuLyThuTien || _dangXuLyHuyHoaDon || _dangXuLyChiTietHoaDon)
            {
                return;
            }

            if (!TryTaoThongTinLuu(out var request))
            {
                return;
            }

            _dangXuLyLuuHoaDon = true;
            CapNhatTrangThaiDangXuLyHoaDon();

            try
            {
                if (_manHinhState == HoaDonManHinhState.ThemMoi)
                {
                    var ketQuaThem = await Task.Run(() => _hoaDonBUS.ThemHoaDon(request));
                    if (!ketQuaThem.Result.ThanhCong)
                    {
                        MessageBox.Show(ketQuaThem.Result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _hoaDonDangChonId = ketQuaThem.HoaDonId;
                    ChuyenStateManHinh(HoaDonManHinhState.Xem);
                    TaiDanhSachHoaDon();
                    MessageBox.Show(ketQuaThem.Result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_manHinhState == HoaDonManHinhState.ChinhSua)
                {
                    request.ID = _hoaDonDangChonId ?? 0;
                    var ketQuaCapNhat = await Task.Run(() => _hoaDonBUS.CapNhatHoaDon(request));
                    if (!ketQuaCapNhat.ThanhCong)
                    {
                        if (LaThongBaoXungDotDuLieu(ketQuaCapNhat.ThongBao))
                        {
                            TaiDanhSachHoaDon();
                        }

                        MessageBox.Show(ketQuaCapNhat.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    ChuyenStateManHinh(HoaDonManHinhState.Xem);
                    TaiDanhSachHoaDon();
                    MessageBox.Show(ketQuaCapNhat.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            finally
            {
                _dangXuLyLuuHoaDon = false;
                CapNhatTrangThaiDangXuLyHoaDon();
            }
        }

        private bool TryTaoThongTinLuu(out HoaDonSaveRequestDTO request)
        {
            request = new HoaDonSaveRequestDTO();

            if (cboBanKhach.SelectedValue is not int banId || banId <= 0)
            {
                MessageBox.Show("Vui lòng chọn bàn hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboBanKhach.Focus();
                return false;
            }

            request.BanID = banId;
            request.NgayLap = dtpNgayTao.Value;
            request.TrangThai = (int)HoaDonTrangThai.ChuaThanhToan;
            request.RowVersion = _hoaDonDangChonRowVersion?.ToArray();

            if (_manHinhState == HoaDonManHinhState.ChinhSua && (request.RowVersion == null || request.RowVersion.Length == 0))
            {
                MessageBox.Show(InvoiceConcurrencyMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TaiDanhSachHoaDon();
                return false;
            }

            return true;
        }

        private void btnBoQua_Click(object? sender, EventArgs e)
        {
            ChuyenStateManHinh(HoaDonManHinhState.Xem);
            TaiDanhSachHoaDon();
        }

        private async void btnXoaHuy_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyHuyHoaDon || _dangXuLyThuTien || _dangXuLyChiTietHoaDon)
            {
                return;
            }

            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hoaDon.TrangThai == (int)HoaDonTrangThai.Paid)
            {
                MessageBox.Show("Hóa đơn đã thanh toán, không thể hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hoaDon.TrangThai == (int)HoaDonTrangThai.Closed)
            {
                MessageBox.Show("Hóa đơn đã hoàn tất phục vụ, không thể hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hoaDon.TrangThai == (int)HoaDonTrangThai.Cancelled)
            {
                MessageBox.Show("Hóa đơn này đã hủy trước đó.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!TryLayLyDoHuyHoaDon(out var lyDoHuy))
            {
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Xác nhận hủy hóa đơn {hoaDon.MaHoaDonHienThi}?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            _dangXuLyHuyHoaDon = true;
            CapNhatTrangThaiDangXuLyHoaDon();

            BanActionResultDTO ketQua;
            try
            {
                var hoaDonId = hoaDon.ID;
                var rowVersion = _hoaDonDangChonRowVersion?.ToArray();
                var nguoiThucHien = NguoiDungHienTaiService.LayNguoiDungDangNhap()?.TenDangNhap ?? "system";
                ketQua = await Task.Run(() => _hoaDonBUS.HuyHoaDon(hoaDonId, lyDoHuy, nguoiThucHien, rowVersion));
            }
            finally
            {
                _dangXuLyHuyHoaDon = false;
                CapNhatTrangThaiDangXuLyHoaDon();
            }

            if (!ketQua.ThanhCong)
            {
                if (LaThongBaoXungDotDuLieu(ketQua.ThongBao))
                {
                    TaiDanhSachHoaDon();
                }

                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSachHoaDon();
            MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool TryLayLyDoHuyHoaDon(out string lyDo)
        {
            lyDo = string.Empty;

            using var dialog = new Form
            {
                Text = "Lý do hủy hóa đơn",
                StartPosition = FormStartPosition.CenterParent,
                Width = 460,
                Height = 210,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblLyDo = new Label
            {
                Left = 16,
                Top = 18,
                Width = 420,
                Text = "Vui lòng nhập lý do hủy hóa đơn:"
            };

            var txtLyDo = new TextBox
            {
                Left = 16,
                Top = 44,
                Width = 420,
                Height = 60,
                Multiline = true
            };

            var btnXacNhan = new Button
            {
                Left = 276,
                Top = 118,
                Width = 78,
                Text = "Xác nhận",
                DialogResult = DialogResult.OK
            };

            var btnHuy = new Button
            {
                Left = 360,
                Top = 118,
                Width = 78,
                Text = "Hủy",
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.AddRange(new Control[]
            {
                lblLyDo,
                txtLyDo,
                btnXacNhan,
                btnHuy
            });

            dialog.AcceptButton = btnXacNhan;
            dialog.CancelButton = btnHuy;

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return false;
            }

            lyDo = txtLyDo.Text.Trim();
            if (lyDo.Length == 0)
            {
                MessageBox.Show("Lý do hủy không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async void btnThemMonVaoHoaDon_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyChiTietHoaDon || _dangXuLyLuuHoaDon || _dangXuLyThuTien || _dangXuLyHuyHoaDon)
            {
                return;
            }

            if (!TryLayHoaDonDangChonCoTheChinhSua(out var hoaDon))
            {
                return;
            }

            if (cboMon.SelectedValue is not int monId || monId <= 0)
            {
                MessageBox.Show("Vui lòng chọn món hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMon.Focus();
                return;
            }

            var soLuong = (short)nudSoLuong.Value;
            _dangXuLyChiTietHoaDon = true;
            CapNhatTrangThaiDangXuLyHoaDon();

            BanActionResultDTO ketQua;
            try
            {
                ketQua = await Task.Run(() => _orderService.AddItemToOrder(
                    hoaDon.ID,
                    monId,
                    soLuong,
                    _hoaDonDangChonRowVersion));
            }
            finally
            {
                _dangXuLyChiTietHoaDon = false;
                CapNhatTrangThaiDangXuLyHoaDon();
            }

            if (!ketQua.ThanhCong)
            {
                if (LaThongBaoXungDotDuLieu(ketQua.ThongBao))
                {
                    TaiDanhSachHoaDon();
                }

                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _dangXuLyThayDoiChiTiet = true;
            try
            {
                var dongTonTai = _chiTietBindingList.FirstOrDefault(x => x.MonID == monId);
                if (dongTonTai != null)
                {
                    dongTonTai.SoLuong = (short)Math.Clamp(dongTonTai.SoLuong + soLuong, 1, short.MaxValue);
                }
                else if (cboMon.SelectedItem is HoaDonMonItemDTO monMoi)
                {
                    _chiTietBindingList.Add(new HoaDonChiTietDTO
                    {
                        MonID = monMoi.MonID,
                        TenMon = monMoi.TenMon,
                        SoLuong = soLuong,
                        DonGia = monMoi.DonGia
                    });
                }

                _chiTietBindingList.ResetBindings();
            }
            finally
            {
                _dangXuLyThayDoiChiTiet = false;
            }

            CapNhatTongTien();

            TaiDanhSachHoaDon();
            MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task ThuXoaMonDangChonAsync()
        {
            if (_dangNapDuLieu
                || _dangXuLyThayDoiChiTiet
                || _dangXuLyChiTietHoaDon
                || _dangXuLyLuuHoaDon
                || _dangXuLyThuTien
                || _dangXuLyHuyHoaDon)
            {
                return;
            }

            if (!TryLayHoaDonDangChonCoTheChinhSua(out var hoaDon))
            {
                return;
            }

            if (dgvChiTietHoaDon.CurrentRow?.DataBoundItem is not HoaDonChiTietDTO chiTiet)
            {
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Xóa món '{chiTiet.TenMon}' khỏi hóa đơn?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            _dangXuLyChiTietHoaDon = true;
            CapNhatTrangThaiDangXuLyHoaDon();

            BanActionResultDTO ketQua;
            try
            {
                ketQua = await Task.Run(() => _orderService.RemoveItemFromOrder(
                    hoaDon.ID,
                    chiTiet.MonID,
                    chiTiet.SoLuong,
                    _hoaDonDangChonRowVersion));
            }
            finally
            {
                _dangXuLyChiTietHoaDon = false;
                CapNhatTrangThaiDangXuLyHoaDon();
            }

            if (!ketQua.ThanhCong)
            {
                if (LaThongBaoXungDotDuLieu(ketQua.ThongBao))
                {
                    TaiDanhSachHoaDon();
                }

                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _dangXuLyThayDoiChiTiet = true;
            try
            {
                _chiTietBindingList.Remove(chiTiet);
                _chiTietBindingList.ResetBindings();
            }
            finally
            {
                _dangXuLyThayDoiChiTiet = false;
            }

            CapNhatTongTien();
            TaiDanhSachHoaDon();
        }

        private async Task ThuDoiMonDangChonTheoMonDaChonAsync()
        {
            if (_dangNapDuLieu
                || _dangXuLyThayDoiChiTiet
                || _dangXuLyChiTietHoaDon
                || _dangXuLyLuuHoaDon
                || _dangXuLyThuTien
                || _dangXuLyHuyHoaDon)
            {
                return;
            }

            if (!TryLayHoaDonDangChonCoTheChinhSua(out var hoaDon))
            {
                return;
            }

            if (dgvChiTietHoaDon.CurrentRow?.DataBoundItem is not HoaDonChiTietDTO chiTietDangChon)
            {
                MessageBox.Show("Vui lòng chọn món cần đổi trên lưới chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboMon.SelectedValue is not int monMoiId || monMoiId <= 0)
            {
                MessageBox.Show("Vui lòng chọn món mới ở combobox trước khi đổi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMon.Focus();
                return;
            }

            if (chiTietDangChon.MonID == monMoiId)
            {
                MessageBox.Show("Món mới trùng món hiện tại, không cần đổi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Đổi món '{chiTietDangChon.TenMon}' thành món đang chọn?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            var soLuongCanDoi = chiTietDangChon.SoLuong;
            _dangXuLyChiTietHoaDon = true;
            CapNhatTrangThaiDangXuLyHoaDon();

            BanActionResultDTO ketQua;
            try
            {
                ketQua = await Task.Run(() => _orderService.ReplaceItemInOrder(
                    hoaDon.ID,
                    chiTietDangChon.MonID,
                    monMoiId,
                    soLuongCanDoi,
                    _hoaDonDangChonRowVersion));
            }
            finally
            {
                _dangXuLyChiTietHoaDon = false;
                CapNhatTrangThaiDangXuLyHoaDon();
            }

            if (!ketQua.ThanhCong)
            {
                if (LaThongBaoXungDotDuLieu(ketQua.ThongBao))
                {
                    TaiDanhSachHoaDon();
                }

                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboMon.SelectedItem is not HoaDonMonItemDTO monMoi)
            {
                TaiDanhSachHoaDon();
                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _dangXuLyThayDoiChiTiet = true;
            try
            {
                var dongTonTai = _chiTietBindingList.FirstOrDefault(x => x.MonID == monMoi.MonID && !ReferenceEquals(x, chiTietDangChon));
                if (dongTonTai != null)
                {
                    dongTonTai.SoLuong = (short)Math.Clamp(dongTonTai.SoLuong + chiTietDangChon.SoLuong, 1, short.MaxValue);
                    _chiTietBindingList.Remove(chiTietDangChon);
                }
                else
                {
                    chiTietDangChon.MonID = monMoi.MonID;
                    chiTietDangChon.TenMon = monMoi.TenMon;
                    chiTietDangChon.DonGia = monMoi.DonGia;
                }

                _chiTietBindingList.ResetBindings();
            }
            finally
            {
                _dangXuLyThayDoiChiTiet = false;
            }

            CapNhatTongTien();
            TaiDanhSachHoaDon();
            MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool TryLayHoaDonDangChonCoTheChinhSua(out HoaDonDTO hoaDon)
        {
            hoaDon = null!;

            var hoaDonDangChon = LayHoaDonDangChon();
            if (hoaDonDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn trước khi thao tác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (hoaDonDangChon.TrangThai != (int)HoaDonTrangThai.ChuaThanhToan)
            {
                MessageBox.Show("Chỉ được sửa hóa đơn ở trạng thái chưa thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            hoaDon = hoaDonDangChon;
            return true;
        }

        private void CapNhatTongTien()
        {
            _tongTienDangChon = _chiTietBindingList.Sum(x => x.ThanhTien);
            lblTongTienValue.Text = _hoaDonTienService.DinhDangTien(_tongTienDangChon);

            var tienKhachDua = _hoaDonTienService.ChuyenTextTienThanhSo(txtTienKhachDua.Text);
            var tienThoi = _hoaDonBUS.TinhTienThoi(_tongTienDangChon, tienKhachDua);
            lblTienThoiValue.Text = _hoaDonTienService.DinhDangTien(tienThoi);
        }

        private async void btnXacNhanThuTien_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyThuTien || _dangXuLyChiTietHoaDon)
            {
                return;
            }

            if (!CoQuyenXuLyThanhToanHoaDon())
            {
                MessageBox.Show("Bạn không có quyền xác nhận thu tiền hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần thu tiền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tienKhachDua = _hoaDonTienService.ChuyenTextTienThanhSo(txtTienKhachDua.Text);

            if (hoaDon.TrangThai != (int)HoaDonTrangThai.ChuaThanhToan)
            {
                MessageBox.Show("Hóa đơn này không còn ở trạng thái chờ thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_tongTienDangChon <= 0)
            {
                MessageBox.Show("Hóa đơn chưa có món, không thể xác nhận thu tiền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tienKhachDua < _tongTienDangChon)
            {
                MessageBox.Show("Tiền khách đưa chưa đủ để thanh toán hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _dangXuLyThuTien = true;
            CapNhatTrangThaiDangXuLyHoaDon();

            BanActionResultDTO ketQua;
            try
            {
                var hoaDonId = hoaDon.ID;
                var rowVersion = _hoaDonDangChonRowVersion?.ToArray();
                ketQua = await Task.Run(() => _orderService.Checkout(hoaDonId, rowVersion));
            }
            finally
            {
                _dangXuLyThuTien = false;
                CapNhatTrangThaiDangXuLyHoaDon();
            }

            if (!ketQua.ThanhCong)
            {
                if (LaThongBaoXungDotDuLieu(ketQua.ThongBao))
                {
                    TaiDanhSachHoaDon();
                }

                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSachHoaDon();
            MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnInHoaDon_Click(object? sender, EventArgs e)
        {
            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var chiTiet = _hoaDonBUS.LayHoaDonTheoId(hoaDon.ID);
            if (chiTiet == null)
            {
                MessageBox.Show("Không tìm thấy dữ liệu hóa đơn để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var noiDungXemTruoc = _hoaDonPreviewService.TaoNoiDungXemTruoc(chiTiet);
            MessageBox.Show(noiDungXemTruoc, "Xem trước hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtTienKhachDua_TextChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            CapNhatTongTien();
        }

        private void ChuyenStateManHinh(HoaDonManHinhState state)
        {
            _manHinhState = state;

            var hoaDon = state == HoaDonManHinhState.ThemMoi
                ? null
                : LayHoaDonDangChon();

            ApDungTrangThaiDieuKhien(hoaDon);
        }

        private void CapNhatDieuKienXuLyNut(HoaDonDTO? hoaDon)
        {
            ApDungTrangThaiDieuKhien(hoaDon);
        }

        private void ApDungTrangThaiDieuKhien(HoaDonDTO? hoaDon)
        {
            var trangThai = _hoaDonFormStateService.TaoTrangThai(_manHinhState, hoaDon);
            var isAdmin = _permissionBUS.IsAdmin();
            var coQuyenXem = isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);
            var coQuyenTao = isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Create);
            var coQuyenCapNhat = isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Update);
            var coQuyenCapNhatBanHang = isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update);
            var coTheChinhSuaHoaDon = NguoiDungHienTaiService.LayNguoiDungDangNhap() != null;
            var coQuyenChinhSuaHoaDon = coTheChinhSuaHoaDon && (coQuyenTao || coQuyenCapNhat);
            var coQuyenChinhSuaChiTietHoaDon = coTheChinhSuaHoaDon && (coQuyenTao || coQuyenCapNhat || coQuyenCapNhatBanHang);
            var coQuyenThuTien = CoQuyenXuLyThanhToanHoaDon();
            var coQuyenXoa = _permissionBUS.CanDeleteInvoice()
                             && (isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Delete));
            var choPhepSuaChiTiet = trangThai.ChoPhepThemMon && coQuyenChinhSuaChiTietHoaDon;

            cboBanKhach.Enabled = trangThai.ChoPhepSuaThongTinChung && coQuyenChinhSuaHoaDon;
            dtpNgayTao.Enabled = trangThai.ChoPhepSuaThongTinChung && coQuyenChinhSuaHoaDon;
            cboTrangThai.Enabled = false;

            panelMasterFilter.Enabled = trangThai.ChoPhepLocMaster && coQuyenXem;
            dgvDanhSachHoaDon.Enabled = trangThai.ChoPhepGridMaster && coQuyenXem;

            btnThemMoi.Visible = coQuyenTao;
            btnSua.Visible = coQuyenChinhSuaHoaDon;
            btnLuu.Visible = coQuyenChinhSuaHoaDon;
            btnBoQua.Visible = coQuyenChinhSuaHoaDon;
            btnThemMonVaoHoaDon.Visible = coQuyenChinhSuaChiTietHoaDon;
            btnXacNhanThuTien.Visible = coQuyenThuTien;
            btnInHoaDon.Visible = coQuyenXem;
            btnXoaHuy.Visible = coQuyenXoa;

            btnThemMoi.Enabled = btnThemMoi.Visible && trangThai.ChoPhepThemMoi;
            btnSua.Enabled = btnSua.Visible && trangThai.ChoPhepSua;
            btnXoaHuy.Enabled = btnXoaHuy.Visible && trangThai.ChoPhepHuy;
            btnLuu.Enabled = btnLuu.Visible && trangThai.ChoPhepLuu;
            btnBoQua.Enabled = btnBoQua.Visible && trangThai.ChoPhepBoQua;

            btnThemMonVaoHoaDon.Enabled = btnThemMonVaoHoaDon.Visible && choPhepSuaChiTiet;
            cboMon.Enabled = choPhepSuaChiTiet;
            nudSoLuong.Enabled = choPhepSuaChiTiet;
            btnXacNhanThuTien.Enabled = btnXacNhanThuTien.Visible && trangThai.ChoPhepThuTien;
            btnInHoaDon.Enabled = btnInHoaDon.Visible && trangThai.ChoPhepIn;

            dgvChiTietHoaDon.Enabled = coQuyenXem;
            dgvChiTietHoaDon.ReadOnly = !choPhepSuaChiTiet;
            dgvChiTietHoaDon.AllowUserToDeleteRows = false;
            colTenMon.ReadOnly = true;
            colSoLuong.ReadOnly = !choPhepSuaChiTiet;
            colDonGia.ReadOnly = true;
            colThanhTien.ReadOnly = true;
            _menuChiTietHoaDon.Enabled = choPhepSuaChiTiet;
        }

        private bool CoQuyenXuLyThanhToanHoaDon()
        {
            if (_permissionBUS.IsAdmin())
            {
                return true;
            }

            return _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Update)
                || _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update);
        }

        private void CapNhatTrangThaiDangXuLyHoaDon()
        {
            var dangXuLy = _dangXuLyThuTien || _dangXuLyHuyHoaDon || _dangXuLyLuuHoaDon || _dangXuLyChiTietHoaDon;
            UseWaitCursor = dangXuLy;

            if (dangXuLy)
            {
                btnThemMoi.Enabled = false;
                btnSua.Enabled = false;
                btnXacNhanThuTien.Enabled = false;
                btnXoaHuy.Enabled = false;
                btnLuu.Enabled = false;
                btnBoQua.Enabled = false;
                btnThemMonVaoHoaDon.Enabled = false;
                cboMon.Enabled = false;
                nudSoLuong.Enabled = false;
                dgvChiTietHoaDon.Enabled = false;
                return;
            }

            CapNhatDieuKienXuLyNut(LayHoaDonDangChon());
        }

        private HoaDonDTO? LayHoaDonDangChon()
        {
            if (!_hoaDonDangChonId.HasValue)
            {
                return null;
            }

            return _hoaDonBUS.LayHoaDonTheoId(_hoaDonDangChonId.Value);
        }

        private static bool LaThongBaoXungDotDuLieu(string? thongBao)
        {
            return string.Equals(thongBao?.Trim(), InvoiceConcurrencyMessage, StringComparison.Ordinal);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }

    }
}
