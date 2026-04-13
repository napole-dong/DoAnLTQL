using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Presenters;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Permission;
using System.Linq;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.HoaDon;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.UI;
using QuanLyQuanCaPhe.Services.Diagnostics;
using QuanLyQuanCaPhe.Services.Reporting;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmHoaDon : Form
    {
        private const string InvoiceConcurrencyMessage = "Dữ liệu đã bị thay đổi bởi nhân viên khác. Vui lòng tải lại!";
        private readonly bool _isEmbedded;
        private readonly IHoaDonService _hoaDonBUS;
        private readonly IOrderService _orderService;
        private readonly IPermissionService _permissionBUS;
        private readonly HoaDonPreviewService _hoaDonPreviewService;
        private readonly HoaDonTienService _hoaDonTienService;
        private readonly HoaDonFormStateService _hoaDonFormStateService;
        private readonly HoaDonPresenter _hoaDonPresenter;
        private readonly PermissionService _formPermissionService = PermissionService.Shared;
        private readonly SearchDebounceHelper _timKiemDebounce;

        private bool _dangNapDuLieu;
        private bool _dangXuLyHuyHoaDon;
        private bool _dangXuLyThuTien;
        private bool _dangXuLyChiTietHoaDon;
        private int? _hoaDonDangChonId;
        private byte[]? _hoaDonDangChonRowVersion;
        private decimal _tongTienDangChon;
        private FormPermission _formPermission = FormPermission.Deny(nameof(frmHoaDon), UserRole.Staff);
        private BindingList<HoaDonChiTietDTO> _chiTietBindingList = new();
        private readonly ContextMenuStrip _menuChiTietHoaDon = new();
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;
        private readonly CaPheDbContext _db = new CaPheDbContext();

        private sealed class TrangThaiHoaDonOption
        {
            public int Value { get; set; }
            public string Text { get; set; } = string.Empty;
        }

        public frmHoaDon(
            IHoaDonService? hoaDonBUS = null,
            IOrderService? orderService = null,
            IPermissionService? permissionBUS = null,
            HoaDonPreviewService? hoaDonPreviewService = null,
            HoaDonTienService? hoaDonTienService = null,
            HoaDonFormStateService? hoaDonFormStateService = null,
            HoaDonPresenter? hoaDonPresenter = null,
            bool isEmbedded = false)
        {
            _hoaDonBUS = AppServiceProvider.Resolve(hoaDonBUS, () => new HoaDonBUS());
            _orderService = AppServiceProvider.Resolve(orderService, () => new OrderService());
            _permissionBUS = AppServiceProvider.Resolve(permissionBUS, () => new PermissionBUS());
            _hoaDonPreviewService = hoaDonPreviewService ?? new HoaDonPreviewService();
            _hoaDonTienService = hoaDonTienService ?? new HoaDonTienService();
            _hoaDonFormStateService = hoaDonFormStateService ?? new HoaDonFormStateService();
            _hoaDonPresenter = hoaDonPresenter ?? new HoaDonPresenter(InvoiceConcurrencyMessage);
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
            _timKiemDebounce = new SearchDebounceHelper(300, () => TaiDanhSachHoaDon());
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
            colSoLuong.ReadOnly = true;
            dgvChiTietHoaDon.EditMode = DataGridViewEditMode.EditProgrammatically;
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
            dgvChiTietHoaDon.SelectionChanged += dgvChiTietHoaDon_SelectionChanged;
            dgvChiTietHoaDon.CellMouseDown += dgvChiTietHoaDon_CellMouseDown;
            dgvChiTietHoaDon.KeyDown += dgvChiTietHoaDon_KeyDown;
            txtTimKiemHoaDon.TextChanged += txtTimKiemHoaDon_TextChanged;
            txtTimKiemHoaDon.KeyDown += txtTimKiemHoaDon_KeyDown;
            btnLocXem.Click += (_, _) => TaiDanhSachHoaDon();
            btnLamMoi.Click += btnLamMoi_Click;

            btnXoaHuy.Click += btnXoaHuy_Click;

            btnThemMonVaoHoaDon.Click += btnThemMonVaoHoaDon_Click;
            btnXoaMonKhoiHoaDon.Click += async (_, _) => await ThuXoaMonDangChonAsync();
            btnXacNhanThuTien.Click += btnXacNhanThuTien_Click;
            btnInHoaDon.Click += btnInHoaDon_Click;

            txtTienKhachDua.TextChanged += txtTienKhachDua_TextChanged;

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            _btnQuanLyKhoSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NguyenLieu, () => new frmQuanLiKho(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            _btnKhachHangSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
            _btnAuditLogSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmAuditLog(isEmbedded: true), onCurrentFormReactivated: () => TaiDanhSachHoaDon(), skipNavigation: _isEmbedded);
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
            _menuChiTietHoaDon.Items.Add(menuXoaMon);

            dgvChiTietHoaDon.ContextMenuStrip = _menuChiTietHoaDon;
        }

        private void frmHoaDon_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Hoa don"))
            {
                return;
            }

            var currentRole = PermissionExtensions.GetCurrentUserRole();
            _formPermission = _formPermissionService.GetPermission(nameof(frmHoaDon), currentRole);
            if (!this.EnsureCanView(_formPermission))
            {
                Close();
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
                EmbeddedFormLayoutHelper.UseContentOnlyLayout(panelMain, panelSidebar, panelTopbar);
            }

            HienThiNguoiDungDangNhap();
            ApDungPhanQuyenDieuHuong();
            this.ApplyPermission(_formPermission);
            KhoiTaoComboTrangThaiLoc();
            KhoiTaoBoLocMacDinh();
            KhoiTaoComboTrangThai();
            TaiDanhSachBanKhach();
            TaiDanhSachMon();
            TaiDanhSachHoaDon(giuHoaDonDangChon: false);
            CapNhatDieuKienXuLyNut(LayHoaDonDangChon());
        }

        private void ApDungPhanQuyenDieuHuong()
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
                btnHoaDon,
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
                "Open",
                "Paid",
                "Voided"
            });
        }

        private void KhoiTaoComboTrangThai()
        {
            var dsTrangThai = new List<TrangThaiHoaDonOption>
            {
                new() { Value = (int)HoaDonTrangThai.Open, Text = HoaDonBUS.ChuyenTrangThaiHoaDon((int)HoaDonTrangThai.Open) },
                new() { Value = (int)HoaDonTrangThai.Paid, Text = HoaDonBUS.ChuyenTrangThaiHoaDon((int)HoaDonTrangThai.Paid) },
                new() { Value = (int)HoaDonTrangThai.Voided, Text = HoaDonBUS.ChuyenTrangThaiHoaDon((int)HoaDonTrangThai.Voided) }
            };

            cboTrangThai.DataSource = dsTrangThai;
            cboTrangThai.DisplayMember = nameof(TrangThaiHoaDonOption.Text);
            cboTrangThai.ValueMember = nameof(TrangThaiHoaDonOption.Value);
            cboTrangThai.SelectedValue = (int)HoaDonTrangThai.Open;
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

        private async void TaiDanhSachHoaDon(bool giuHoaDonDangChon = true)
        {
            var boLoc = new HoaDonFilterDTO
            {
                TuKhoa = txtTimKiemHoaDon.Text,
                TuNgay = dtpTuNgay.Value,
                DenNgay = dtpDenNgay.Value,
                TrangThai = HoaDonBUS.ChuyenTextSangTrangThaiLoc(cboTrangThaiLoc.SelectedItem?.ToString()),
                PageNumber = 1,
                PageSize = 50
            };

            List<HoaDonDTO> dsHoaDon;
            try
            {
                dsHoaDon = await _hoaDonBUS.LayDanhSachHoaDonAsync(boLoc);
                dsHoaDon = LocDanhSachHoaDonTheoFormPermission(dsHoaDon);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "TaiDanhSachHoaDon failed.", nameof(frmHoaDon));
                MessageBox.Show("Không thể tải danh sách hóa đơn. Vui lòng thử lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

        private List<HoaDonDTO> LocDanhSachHoaDonTheoFormPermission(List<HoaDonDTO> dsHoaDon)
        {
            if (!_formPermission.CanView)
            {
                return new List<HoaDonDTO>();
            }

            if (!_formPermission.CanViewOwn)
            {
                return dsHoaDon;
            }

            return dsHoaDon.Where(LaHoaDonThuocNguoiDungHienTai).ToList();
        }

        private bool LaHoaDonThuocNguoiDungHienTai(HoaDonDTO hoaDon)
        {
            if (!_formPermission.CanViewOwn)
            {
                return true;
            }

            var nhanVienId = NguoiDungHienTaiService.LayNguoiDungDangNhap()?.NhanVienId;
            if (!nhanVienId.HasValue)
            {
                return false;
            }

            return hoaDon.NhanVienID == nhanVienId.Value;
        }

        private bool ChonDongHoaDon(int hoaDonId)
        {
            return DataGridViewSelectionHelper.TrySelectRow<HoaDonDTO>(dgvDanhSachHoaDon, x => x.ID == hoaDonId);
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

            if (!DataGridViewSelectionHelper.TryGetCurrentOrSelectedItem<HoaDonChiTietDTO>(dgvChiTietHoaDon, out var chiTiet, out _)
                || chiTiet == null)
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
            if (!DataGridViewSelectionHelper.TryGetCurrentOrSelectedItem<HoaDonDTO>(dgvDanhSachHoaDon, out var hoaDonGrid, out _)
                || hoaDonGrid == null)
            {
                _hoaDonDangChonId = null;
                HienThiTrangThaiKhongCoHoaDon();
                return;
            }

            if (!LaHoaDonThuocNguoiDungHienTai(hoaDonGrid))
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
            cboTrangThai.SelectedValue = (int)HoaDonTrangThai.Open;
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

        private void txtTimKiemHoaDon_TextChanged(object? sender, EventArgs e)
        {
            _timKiemDebounce.Signal();
        }

        private void txtTimKiemHoaDon_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            _timKiemDebounce.Flush();
            e.SuppressKeyPress = true;
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

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimKiemHoaDon.Clear();
            KhoiTaoBoLocMacDinh();
            TaiDanhSachHoaDon(giuHoaDonDangChon: false);
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

            if (HoaDonStateMachine.IsPaid(hoaDon.TrangThai))
            {
                MessageBox.Show("Hóa đơn đã thanh toán, không thể hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (HoaDonStateMachine.IsVoided(hoaDon.TrangThai))
            {
                MessageBox.Show("Hóa đơn này đã được Void trước đó.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!HoaDonStateMachine.IsOpen(hoaDon.TrangThai))
            {
                MessageBox.Show("Chỉ có thể Void hóa đơn Open.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (_dangXuLyChiTietHoaDon || _dangXuLyThuTien || _dangXuLyHuyHoaDon)
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

            TaiDanhSachHoaDon();
            MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task ThuXoaMonDangChonAsync()
        {
            if (_dangNapDuLieu
                || _dangXuLyChiTietHoaDon
                || _dangXuLyThuTien
                || _dangXuLyHuyHoaDon)
            {
                return;
            }

            if (!TryLayHoaDonDangChonCoTheChinhSua(out var hoaDon))
            {
                return;
            }

            if (!DataGridViewSelectionHelper.TryGetCurrentOrSelectedItem<HoaDonChiTietDTO>(dgvChiTietHoaDon, out var chiTiet, out _)
                || chiTiet == null)
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
            TaiDanhSachHoaDon();
        }

        private bool TryLayHoaDonDangChonCoTheChinhSua(out HoaDonDTO hoaDon)
        {
            hoaDon = null!;

            if (!_formPermission.CanEdit)
            {
                MessageBox.Show("Bạn không có quyền thao tác hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var hoaDonDangChon = LayHoaDonDangChon();
            if (hoaDonDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn trước khi thao tác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!LaHoaDonThuocNguoiDungHienTai(hoaDonDangChon))
            {
                MessageBox.Show("Bạn chỉ được thao tác hóa đơn của chính mình.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!HoaDonStateMachine.IsOpen(hoaDonDangChon.TrangThai))
            {
                MessageBox.Show("Chỉ thao tác trên hóa đơn Open.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            var hoaDon = LayHoaDonDangChon();
            var tienKhachDua = _hoaDonTienService.ChuyenTextTienThanhSo(txtTienKhachDua.Text);

            var ketQuaKiemTra = _hoaDonPresenter.ValidateCheckout(
                CoQuyenXuLyThanhToanHoaDon(),
                hoaDon,
                _tongTienDangChon,
                tienKhachDua);

            if (!ketQuaKiemTra.HopLe)
            {
                MessageBox.Show(ketQuaKiemTra.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (ketQuaKiemTra.TruongLoi == HoaDonInputField.TienKhachDua)
                {
                    txtTienKhachDua.Focus();
                }

                return;
            }

            _dangXuLyThuTien = true;
            CapNhatTrangThaiDangXuLyHoaDon();

            var hoaDonId = hoaDon!.ID;

            BanActionResultDTO ketQua;
            try
            {
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

        private async void btnInHoaDon_Click(object? sender, EventArgs e)
        {
            try
            {
                var hoaDon = LayHoaDonDangChon();
                if (hoaDon == null)
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn trước khi in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var printDataService = new QuanLyQuanCaPhe.Services.Reporting.HoaDonPrintService();
                var dto = await printDataService.GetHoaDonPrintDtoAsync(hoaDon.ID);
                if (dto == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn cần in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dto.Items.Count == 0)
                {
                    MessageBox.Show("Hóa đơn chưa có chi tiết, không thể in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var tienKhachDua = _hoaDonTienService.ChuyenTextTienThanhSo(txtTienKhachDua.Text);
                dto.CashReceived = tienKhachDua > 0 ? tienKhachDua : dto.GrandTotal;

                var thermalPrintService = new Thermal80InvoicePrintService();
                thermalPrintService.PrintWithDialog(this, dto);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "In hóa đơn thất bại.", nameof(frmHoaDon));
                MessageBox.Show($"Lỗi khi in hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTienKhachDua_TextChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            CapNhatTongTien();
        }

        private void CapNhatDieuKienXuLyNut(HoaDonDTO? hoaDon)
        {
            ApDungTrangThaiDieuKhien(hoaDon);
        }

        private void ApDungTrangThaiDieuKhien(HoaDonDTO? hoaDon)
        {
            var trangThai = _hoaDonFormStateService.TaoTrangThai(HoaDonManHinhState.Xem, hoaDon);
            var isAdmin = _permissionBUS.IsAdmin();
            var coQuyenXem = _formPermission.CanView && (isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View));
            var coQuyenTao = _formPermission.CanAdd && (isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Create));
            var coQuyenCapNhat = _formPermission.CanEdit && (isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Update));
            var coQuyenCapNhatBanHang = _formPermission.CanEdit && (isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update));
            var coTheChinhSuaHoaDon = NguoiDungHienTaiService.LayNguoiDungDangNhap() != null;
            var coQuyenChinhSuaChiTietHoaDon = coTheChinhSuaHoaDon && (coQuyenTao || coQuyenCapNhat || coQuyenCapNhatBanHang);
            var coQuyenThuTien = CoQuyenXuLyThanhToanHoaDon();
            var coQuyenVoid = _formPermission.CanDelete
                              && _permissionBUS.CanDeleteInvoice()
                              && (isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Delete));
            var khoaToanBoUiDoDaThanhToan = trangThai.KhoaToanBoChiTietDoDaThanhToan;
            var choPhepSuaChiTiet = trangThai.ChoPhepThemMon && coQuyenChinhSuaChiTietHoaDon && !khoaToanBoUiDoDaThanhToan;
            var choPhepXoaMon = trangThai.ChoPhepXoaMon && coQuyenChinhSuaChiTietHoaDon && !khoaToanBoUiDoDaThanhToan;
            var choPhepThanhToan = trangThai.ChoPhepThuTien && coQuyenThuTien && !khoaToanBoUiDoDaThanhToan;
            var choPhepVoid = trangThai.ChoPhepHuy && coQuyenVoid && !khoaToanBoUiDoDaThanhToan;

            cboBanKhach.Enabled = false;
            dtpNgayTao.Enabled = false;
            cboTrangThai.Enabled = false;

            panelMasterFilter.Enabled = coQuyenXem;
            dgvDanhSachHoaDon.Enabled = coQuyenXem;

            btnThemMonVaoHoaDon.Visible = coQuyenChinhSuaChiTietHoaDon;
            btnXoaMonKhoiHoaDon.Visible = coQuyenChinhSuaChiTietHoaDon;
            btnXacNhanThuTien.Visible = coQuyenThuTien;
            btnXoaHuy.Visible = coQuyenVoid;

            btnThemMonVaoHoaDon.Enabled = btnThemMonVaoHoaDon.Visible && choPhepSuaChiTiet;
            btnXoaMonKhoiHoaDon.Enabled = btnXoaMonKhoiHoaDon.Visible && choPhepXoaMon;
            cboMon.Enabled = choPhepSuaChiTiet;
            nudSoLuong.Enabled = choPhepSuaChiTiet;
            btnXacNhanThuTien.Enabled = btnXacNhanThuTien.Visible && choPhepThanhToan;
            txtTienKhachDua.Enabled = choPhepThanhToan;
            btnXoaHuy.Enabled = btnXoaHuy.Visible && choPhepVoid;

            dgvChiTietHoaDon.Enabled = coQuyenXem && !khoaToanBoUiDoDaThanhToan;
            dgvChiTietHoaDon.ReadOnly = true;
            dgvChiTietHoaDon.AllowUserToDeleteRows = false;
            colTenMon.ReadOnly = true;
            colSoLuong.ReadOnly = true;
            colDonGia.ReadOnly = true;
            colThanhTien.ReadOnly = true;
            _menuChiTietHoaDon.Enabled = choPhepXoaMon;
        }

        private bool CoQuyenXuLyThanhToanHoaDon()
        {
            if (!_formPermission.CanEdit)
            {
                return false;
            }

            if (_permissionBUS.IsAdmin())
            {
                return true;
            }

            return _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Update)
                || _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update);
        }

        private void CapNhatTrangThaiDangXuLyHoaDon()
        {
            var dangXuLy = _dangXuLyThuTien || _dangXuLyHuyHoaDon || _dangXuLyChiTietHoaDon;
            UiLoadingStateHelper.Apply(
                this,
                dangXuLy,
                btnXacNhanThuTien,
                btnXoaHuy,
                btnThemMonVaoHoaDon,
                btnXoaMonKhoiHoaDon,
                cboMon,
                nudSoLuong,
                dgvChiTietHoaDon,
                txtTienKhachDua,
                txtTimKiemHoaDon,
                btnLocXem,
                btnLamMoi);

            if (dangXuLy)
            {
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

        private bool LaThongBaoXungDotDuLieu(string? thongBao)
        {
            return _hoaDonPresenter.IsConcurrencyConflict(thongBao);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timKiemDebounce.Dispose();
            base.OnFormClosed(e);
        }

        // Example async print handler. Wire this to your print button's Click event in the Designer.
        private async void btnPrint_Click(object? sender, EventArgs e)
        {
            // Replace with actual selected invoice id from your UI
            if (!TryGetSelectedHoaDonId(out var hoaDonId))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var service = new QuanLyQuanCaPhe.Services.Reporting.HoaDonPrintService();
            var dto = await service.GetHoaDonPrintDtoAsync(hoaDonId);

            if (dto == null)
            {
                MessageBox.Show("Không tìm thấy hóa đơn hoặc lỗi khi tải dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ds = QuanLyQuanCaPhe.Services.Reporting.HoaDonPrintService.ToDataSet(dto);

            try
            {
                // Preview
                var rpt = new ReportService();
                rpt.ShowReportPreview(this, ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Example export to PDF action (can be wired to a different button)
        private async void btnExportPdf_Click(object? sender, EventArgs e)
        {
            if (!TryGetSelectedHoaDonId(out var hoaDonId))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xuất PDF.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var service = new QuanLyQuanCaPhe.Services.Reporting.HoaDonPrintService();
            var dto = await service.GetHoaDonPrintDtoAsync(hoaDonId);
            if (dto == null)
            {
                MessageBox.Show("Không tìm thấy hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ds = QuanLyQuanCaPhe.Services.Reporting.HoaDonPrintService.ToDataSet(dto);
            var rpt = new ReportService();

            try
            {
                var bytes = rpt.RenderPdf(ds);
                using var sfd = new SaveFileDialog { Filter = "PDF files (*.pdf)|*.pdf", FileName = dto.InvoiceNo + ".pdf" };
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    System.IO.File.WriteAllBytes(sfd.FileName, bytes);
                    MessageBox.Show("Xuất PDF thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper: replace with your actual selection logic
        private bool TryGetSelectedHoaDonId(out int hoaDonId)
        {
            hoaDonId = 0;
            // Example: if you have a selected row in a DataGridView named dgvHoaDon
            // if (dgvHoaDon.CurrentRow?.DataBoundItem is dtaHoadon hd) { hoaDonId = hd.ID; return true; }

            // For demo purposes return false — caller must implement selection extraction
            return false;
        }
    }
}
