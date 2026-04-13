using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.UI;
using System.Text.Json;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmAuditLog : Form
    {
        private readonly bool _isEmbedded;
        private readonly IAuditLogService _auditLogBUS;
        private readonly IPermissionService _permissionBUS;
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;
        private readonly Panel _panelChiTietBanGhi = new();
        private readonly Label _lblChiTietBanGhi = new();
        private readonly TextBox _txtChiTietBanGhi = new();
        private List<AuditLogDTO> _danhSachDangHienThi = new();
        private bool _dangTaiDuLieu;

        public frmAuditLog(
            IAuditLogService? auditLogBUS = null,
            IPermissionService? permissionBUS = null,
            bool isEmbedded = false)
        {
            _auditLogBUS = AppServiceProvider.Resolve(auditLogBUS, () => new AuditLogBUS());
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

            CauHinhSuKienDieuHuong();
            CauHinhAuditUi();
        }

        private void CauHinhSuKienDieuHuong()
        {
            Load += frmAuditLog_Load;

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), skipNavigation: _isEmbedded);
            btnHoaDon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.HoaDon, () => new frmHoaDon(isEmbedded: true), skipNavigation: _isEmbedded);
            _btnQuanLyKhoSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NguyenLieu, () => new frmQuanLiKho(isEmbedded: true), skipNavigation: _isEmbedded);
            _btnKhachHangSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), skipNavigation: _isEmbedded);
            btnNhanVien.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NhanVien, () => new frmNhanVien(isEmbedded: true), skipNavigation: _isEmbedded);
            btnThongKe.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmThongKe(isEmbedded: true), skipNavigation: _isEmbedded);
            _btnAuditLogSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmAuditLog(isEmbedded: true), onCurrentFormReactivated: TaiDanhSachAuditLog, skipNavigation: _isEmbedded);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void CauHinhAuditUi()
        {
            KhoiTaoKhungChiTietBanGhi();

            dgvAuditLog.AutoGenerateColumns = false;
            colThoiGian.DataPropertyName = nameof(AuditLogDTO.ThoiGianHienThi);
            colNguoiThucHien.DataPropertyName = nameof(AuditLogDTO.NguoiThucHienHienThi);
            colHanhDong.DataPropertyName = nameof(AuditLogDTO.HanhDongHienThi);
            colBangDuLieu.DataPropertyName = nameof(AuditLogDTO.BangDuLieuHienThi);
            colDoiTuong.DataPropertyName = nameof(AuditLogDTO.DoiTuongHienThi);
            colChiTiet.DataPropertyName = nameof(AuditLogDTO.ChiTietHienThi);
            colDiaChiIP.DataPropertyName = nameof(AuditLogDTO.DiaChiIp);
            dgvAuditLog.SelectionChanged += dgvAuditLog_SelectionChanged;
            dgvAuditLog.RowPrePaint += dgvAuditLog_RowPrePaint;

            btnApDung.Click += (_, _) => TaiDanhSachAuditLog();
            btnLamMoi.Click += (_, _) => LamMoiBoLoc();

            txtTimKiem.KeyDown += txtTimKiem_KeyDown;
            txtNguoiDung.KeyDown += txtNguoiDung_KeyDown;
        }

        private void frmAuditLog_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Audit log"))
            {
                return;
            }

            try
            {
                if (!_permissionBUS.IsAdmin())
                {
                    throw new UnauthorizedAccessException("Ban khong co quyen truy cap chuc nang Audit log.");
                }
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
            KhoiTaoBoLocMacDinh();
            NapDanhMucBoLoc();
            TaiDanhSachAuditLog();
        }

        private void HienThiNguoiDungDangNhap()
        {
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
                _btnAuditLogSidebar,
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

            btnXuatExcel.Visible = false;
            btnXuatExcel.Enabled = false;
        }

        private void KhoiTaoBoLocMacDinh()
        {
            dtTuNgay.Value = DateTime.Today.AddDays(-7);
            dtDenNgay.Value = DateTime.Today;

            txtNguoiDung.Clear();
            txtTimKiem.Clear();
        }

        private void NapDanhMucBoLoc()
        {
            CapNhatDanhMucCombo(cboMucDo, new[] { "Info", "Warning", "Error", "Critical" });
            CapNhatDanhMucCombo(cboHanhDong, _auditLogBUS.LayDanhSachHanhDong());
            CapNhatDanhMucCombo(cboBang, _auditLogBUS.LayDanhSachBangDuLieu());
        }

        private static void CapNhatDanhMucCombo(ComboBox comboBox, IEnumerable<string> giaTri)
        {
            var giaTriDangChon = comboBox.SelectedItem?.ToString();
            var danhSach = giaTri
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            comboBox.BeginUpdate();
            comboBox.Items.Clear();
            comboBox.Items.Add("Tat ca");

            foreach (var item in danhSach)
            {
                comboBox.Items.Add(item);
            }

            if (!string.IsNullOrWhiteSpace(giaTriDangChon)
                && comboBox.Items.Cast<object>().Any(x => string.Equals(x.ToString(), giaTriDangChon, StringComparison.OrdinalIgnoreCase)))
            {
                comboBox.SelectedItem = comboBox.Items
                    .Cast<object>()
                    .First(x => string.Equals(x.ToString(), giaTriDangChon, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                comboBox.SelectedIndex = 0;
            }

            comboBox.EndUpdate();
        }

        private void KhoiTaoKhungChiTietBanGhi()
        {
            _panelChiTietBanGhi.Dock = DockStyle.Bottom;
            _panelChiTietBanGhi.Height = 170;
            _panelChiTietBanGhi.Padding = new Padding(12, 8, 12, 10);
            _panelChiTietBanGhi.BackColor = Color.FromArgb(252, 247, 241);

            _lblChiTietBanGhi.AutoSize = true;
            _lblChiTietBanGhi.Dock = DockStyle.Top;
            _lblChiTietBanGhi.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            _lblChiTietBanGhi.ForeColor = Color.FromArgb(102, 78, 62);
            _lblChiTietBanGhi.Text = "Chi tiết Old/New của bản ghi đang chọn";

            _txtChiTietBanGhi.Dock = DockStyle.Fill;
            _txtChiTietBanGhi.Multiline = true;
            _txtChiTietBanGhi.ReadOnly = true;
            _txtChiTietBanGhi.ScrollBars = ScrollBars.Both;
            _txtChiTietBanGhi.Font = new Font("Consolas", 9F, FontStyle.Regular);
            _txtChiTietBanGhi.BorderStyle = BorderStyle.FixedSingle;
            _txtChiTietBanGhi.WordWrap = false;
            _txtChiTietBanGhi.BackColor = Color.White;
            _txtChiTietBanGhi.Text = "Chọn một bản ghi để xem chi tiết.";

            _panelChiTietBanGhi.Controls.Add(_txtChiTietBanGhi);
            _panelChiTietBanGhi.Controls.Add(_lblChiTietBanGhi);
            panelDanhSach.Controls.Add(_panelChiTietBanGhi);
            _panelChiTietBanGhi.BringToFront();
        }

        private void dgvAuditLog_SelectionChanged(object? sender, EventArgs e)
        {
            CapNhatChiTietBanGhiDangChon();
        }

        private void dgvAuditLog_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvAuditLog.Rows.Count)
            {
                return;
            }

            if (dgvAuditLog.Rows[e.RowIndex].DataBoundItem is not AuditLogDTO log)
            {
                return;
            }

            var actionKey = log.Action?.Trim() ?? string.Empty;
            var isCriticalAction =
                string.Equals(actionKey, AuditActions.LoginFailed, StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionKey, AuditActions.DeleteUser, StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionKey, AuditActions.DeleteProduct, StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionKey, AuditActions.VoidInvoice, StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionKey, AuditActions.DeleteInvoice, StringComparison.OrdinalIgnoreCase);

            var isWarningAction =
                string.Equals(actionKey, AuditActions.RemoveItem, StringComparison.OrdinalIgnoreCase)
                || string.Equals(actionKey, AuditActions.ReplaceItem, StringComparison.OrdinalIgnoreCase)
                || string.Equals(log.MucDo, "Warning", StringComparison.OrdinalIgnoreCase);

            if (isCriticalAction)
            {
                dgvAuditLog.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 239, 239);
                return;
            }

            if (isWarningAction)
            {
                dgvAuditLog.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 232);
                return;
            }

            dgvAuditLog.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
        }

        private void CapNhatChiTietBanGhiDangChon()
        {
            if (dgvAuditLog.CurrentRow?.DataBoundItem is not AuditLogDTO log)
            {
                _txtChiTietBanGhi.Text = "Chọn một bản ghi để xem chi tiết.";
                return;
            }

            _txtChiTietBanGhi.Text = TaoNoiDungChiTietBanGhi(log);
        }

        private static string TaoNoiDungChiTietBanGhi(AuditLogDTO log)
        {
            return string.Join(Environment.NewLine,
                $"Thời gian    : {log.ThoiGianHienThi}",
                $"Người dùng   : {log.NguoiThucHienHienThi}",
                $"Hành động    : {log.HanhDongHienThi}",
                $"Bảng/Entity  : {log.BangDuLieuHienThi}",
                $"Đối tượng    : {log.DoiTuongHienThi}",
                $"Mức độ       : {log.MucDo}",
                $"Mô tả        : {log.ChiTietHienThi}",
                string.Empty,
                "OldValue:",
                DinhDangJsonDeDoc(log.OldValue),
                string.Empty,
                "NewValue:",
                DinhDangJsonDeDoc(log.NewValue));
        }

        private static string DinhDangJsonDeDoc(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return "-";
            }

            try
            {
                using var taiLieu = JsonDocument.Parse(json);
                return JsonSerializer.Serialize(taiLieu.RootElement, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch (JsonException)
            {
                return json;
            }
        }

        private void TaiDanhSachAuditLog()
        {
            if (_dangTaiDuLieu)
            {
                return;
            }

            _dangTaiDuLieu = true;
            DatTrangThaiDangTaiDuLieu(true);

            try
            {
                var boLoc = TaoBoLocHienTai();
                _danhSachDangHienThi = _auditLogBUS.LayDanhSachAuditLog(boLoc)
                    .OrderByDescending(x => x.CreatedAt)
                    .ThenByDescending(x => x.Id)
                    .ToList();

                dgvAuditLog.DataSource = null;
                dgvAuditLog.DataSource = _danhSachDangHienThi;

                if (dgvAuditLog.Rows.Count > 0)
                {
                    dgvAuditLog.ClearSelection();
                    dgvAuditLog.Rows[0].Selected = true;
                    dgvAuditLog.CurrentCell = dgvAuditLog.Rows[0].Cells[colThoiGian.Index];
                }
                else
                {
                    dgvAuditLog.ClearSelection();
                    _txtChiTietBanGhi.Text = "Không có bản ghi phù hợp với bộ lọc hiện tại.";
                }

                var thongKe = _auditLogBUS.TinhTongQuan(_danhSachDangHienThi);
                CapNhatCardTongQuan(thongKe);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Khong the tai audit log. Chi tiet: {ex.Message}", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DatTrangThaiDangTaiDuLieu(false);
                _dangTaiDuLieu = false;
            }
        }

        private AuditLogFilterDTO TaoBoLocHienTai()
        {
            var tuNgay = dtTuNgay.Value.Date;
            var denNgay = dtDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                (tuNgay, denNgay) = (denNgay, tuNgay);
            }

            return new AuditLogFilterDTO
            {
                TuNgay = tuNgay,
                DenNgay = denNgay,
                MucDo = cboMucDo.SelectedItem?.ToString(),
                HanhDong = cboHanhDong.SelectedItem?.ToString(),
                BangDuLieu = cboBang.SelectedItem?.ToString(),
                NguoiDung = txtNguoiDung.Text,
                TuKhoa = txtTimKiem.Text,
                SoLuongToiDa = 2000
            };
        }

        private void CapNhatCardTongQuan(AuditLogSummaryDTO thongKe)
        {
            lblTongLogValue.Text = thongKe.TongBanGhi.ToString("N0");
            lblQuanTrongValue.Text = thongKe.SuKienQuanTrong.ToString("N0");
            lblNguoiDungValue.Text = thongKe.NguoiDungHoatDong.ToString("N0");
            lblHomNayValue.Text = thongKe.PhatSinhHomNay.ToString("N0");
        }

        private void LamMoiBoLoc()
        {
            if (_dangTaiDuLieu)
            {
                return;
            }

            KhoiTaoBoLocMacDinh();

            cboMucDo.SelectedIndex = cboMucDo.Items.Count > 0 ? 0 : -1;
            cboHanhDong.SelectedIndex = cboHanhDong.Items.Count > 0 ? 0 : -1;
            cboBang.SelectedIndex = cboBang.Items.Count > 0 ? 0 : -1;

            TaiDanhSachAuditLog();
        }

        private void txtTimKiem_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            TaiDanhSachAuditLog();
            e.SuppressKeyPress = true;
        }

        private void txtNguoiDung_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            TaiDanhSachAuditLog();
            e.SuppressKeyPress = true;
        }

        private void DatTrangThaiDangTaiDuLieu(bool dangTai)
        {
            UseWaitCursor = dangTai;
            btnApDung.Enabled = !dangTai;
            btnLamMoi.Enabled = !dangTai;
        }
    }
}
