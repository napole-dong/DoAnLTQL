using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Export;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.UI;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmNhanVien : Form
    {
        private readonly bool _isEmbedded;
        private readonly NhanVienBUS _nhanVienBUS = new();
        private readonly PermissionBUS _permissionBUS = new();
        private readonly DataExportService _dataExportService = new();
        private CheckBox? _chkHienThiDaXoa;
        private bool _dangXuLyTacVuNhanVien;

        public frmNhanVien(bool isEmbedded = false)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();

            dgvDanhSachNhanVien.AutoGenerateColumns = false;
            colIDNhanVien.DataPropertyName = nameof(NhanVienDTO.ID);
            colHoVaTenNhanVien.DataPropertyName = nameof(NhanVienDTO.HoVaTen);
            colDienThoaiNhanVien.DataPropertyName = nameof(NhanVienDTO.DienThoai);
            colDiaChiNhanVien.DataPropertyName = nameof(NhanVienDTO.DiaChi);
            colTenDangNhapNhanVien.DataPropertyName = nameof(NhanVienDTO.TenDangNhap);
            colQuyenHanNhanVien.DataPropertyName = nameof(NhanVienDTO.QuyenHan);

            Load += frmNhanVien_Load;
            txtTimNhanVien.KeyDown += txtTimNhanVien_KeyDown;
            dgvDanhSachNhanVien.SelectionChanged += dgvDanhSachNhanVien_SelectionChanged;

            btnThemNhanVien.Click += btnThemNhanVien_Click;
            btnCapNhatNhanVien.Click += btnCapNhatNhanVien_Click;
            btnXoaNhanVien.Click += btnXoaNhanVien_Click;
            btnTimNhanVien.Click += btnTimNhanVien_Click;
            btnNhapNhanVien.Click += btnNhapNhanVien_Click;
            btnXuatNhanVien.Click += btnXuatNhanVien_Click;
            KhoiTaoNutLamMoi();
            KhoiTaoBoLocDuLieuDaXoa();

            btnXoaNhanVien.Text = "Ngừng hoạt động";

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
            btnKhachHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachNhanVien, skipNavigation: _isEmbedded);
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

            if (!_permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View))
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng Nhân viên.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            TaiDanhSachVaiTroTheoQuyen();
            ApDungPhanQuyenLenUI();

            await LoadDanhSachNhanVienAsync(false, -1);
        }

        private void KhoiTaoNutLamMoi()
        {
            var btnLamMoi = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(248, 245, 241),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(65, 48, 39),
                Location = new Point(Math.Max(0, btnNhapNhanVien.Left - 96), btnNhapNhanVien.Top),
                Name = "btnLamMoiNhanVien",
                Size = new Size(88, 30),
                TabStop = false,
                Text = "Làm mới",
                UseVisualStyleBackColor = false
            };

            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.Click += btnLamMoi_Click;

            panelDanhSachHeader.Controls.Add(btnLamMoi);
            btnLamMoi.BringToFront();
        }

        private void KhoiTaoBoLocDuLieuDaXoa()
        {
            _chkHienThiDaXoa = new CheckBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(65, 48, 39),
                Location = new Point(Math.Max(0, btnNhapNhanVien.Left - 292), btnNhapNhanVien.Top + 6),
                Name = "chkHienThiNhanVienDaXoa",
                TabStop = false,
                Text = "Hiển thị dữ liệu đã xóa"
            };

            _chkHienThiDaXoa.CheckedChanged += (_, _) =>
            {
                _ = LoadDanhSachNhanVienAsync(false, -1);
            };

            panelDanhSachHeader.Controls.Add(_chkHienThiDaXoa);
            _chkHienThiDaXoa.BringToFront();
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
            var coQuyenXemNhanVien = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View);
            var coQuyenThemNhanVien = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Create);
            var coQuyenCapNhatNhanVien = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Update);
            var coQuyenXoaNhanVien = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.Delete);

            btnThemNhanVien.Visible = coQuyenThemNhanVien;
            btnCapNhatNhanVien.Visible = coQuyenCapNhatNhanVien;
            btnXoaNhanVien.Visible = coQuyenXoaNhanVien;
            btnNhapNhanVien.Visible = coQuyenThemNhanVien || coQuyenCapNhatNhanVien;
            btnXuatNhanVien.Visible = coQuyenXemNhanVien;
            btnTimNhanVien.Visible = coQuyenXemNhanVien;

            btnThemNhanVien.Enabled = btnThemNhanVien.Visible;
            btnCapNhatNhanVien.Enabled = btnCapNhatNhanVien.Visible;
            btnXoaNhanVien.Enabled = btnXoaNhanVien.Visible;
            btnNhapNhanVien.Enabled = btnNhapNhanVien.Visible;
            btnXuatNhanVien.Enabled = btnXuatNhanVien.Visible;
            btnTimNhanVien.Enabled = btnTimNhanVien.Visible;

            txtTimNhanVien.Enabled = coQuyenXemNhanVien;
            cboQuyenHan.Enabled = coQuyenThemNhanVien || coQuyenCapNhatNhanVien;
            txtMatKhau.Enabled = coQuyenThemNhanVien || coQuyenCapNhatNhanVien;

            btnBanHang.Visible = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View);

            var coQuyenMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
            var coQuyenKhachHang = _permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.View);
            var coQuyenHoaDon = _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);
            btnQuanLyBan.Visible = coQuyenMenu;
            btnQuanLyMon.Visible = coQuyenMenu;
            btnCongThuc.Visible = coQuyenMenu;
            btnHoaDon.Visible = coQuyenHoaDon;
            btnKhachHang.Visible = coQuyenKhachHang;

            btnNhanVien.Visible = coQuyenXemNhanVien;
            btnThongKe.Visible = _permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View);
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
            var includeDeleted = _chkHienThiDaXoa?.Checked == true;
            try
            {
                var dsNhanVien = await _nhanVienBUS.LayDanhSachNhanVienAsync(txtTimNhanVien.Text, includeDeleted);
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

                lblTongNhanVienValue.Text = dsNhanVien.Count.ToString();
                lblQuanLyValue.Text = dsNhanVien.Count(x => x.QuyenHan.Equals("Quản lý", StringComparison.OrdinalIgnoreCase)).ToString();
                lblThuNganValue.Text = (dsNhanVien.Count - dsNhanVien.Count(x => x.QuyenHan.Equals("Quản lý", StringComparison.OrdinalIgnoreCase))).ToString();
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
            UseWaitCursor = dangXuLy;
            btnThemNhanVien.Enabled = !dangXuLy;
            btnCapNhatNhanVien.Enabled = !dangXuLy;
            btnXoaNhanVien.Enabled = !dangXuLy;
            btnNhapNhanVien.Enabled = !dangXuLy;
            btnXuatNhanVien.Enabled = !dangXuLy;

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

            if (!int.TryParse(txtMaNhanVien.Text, out var nhanVienId))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out var nhanVienDTO, true))
            {
                return;
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

            try
            {
                var result = _nhanVienBUS.XoaNhanVien(nhanVien.ID, softDelete: true);
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
        }

        private async void btnLamMoi_Click(object? sender, EventArgs e)
        {
            await LoadDanhSachNhanVienAsync(false, -1);
        }

        private async void btnTimNhanVien_Click(object? sender, EventArgs e)
        {
            await LoadDanhSachNhanVienAsync(false, -1);
        }

        private async void txtTimNhanVien_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            await LoadDanhSachNhanVienAsync(false, -1);
            e.SuppressKeyPress = true;
        }

        private async void btnNhapNhanVien_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyTacVuNhanVien)
            {
                return;
            }

            using var dialog = new OpenFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                Title = "Nhập danh sách nhân viên"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            _dangXuLyTacVuNhanVien = true;
            DatTrangThaiDangXuLyNhanVien(true);

            try
            {
                var lines = await File.ReadAllLinesAsync(dialog.FileName, Encoding.UTF8);
                if (lines.Length == 0)
                {
                    MessageBox.Show("Tệp nhập không có dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = await _nhanVienBUS.NhapNhanVienTuCsvAsync(lines);
                await LoadDanhSachNhanVienAsync(false, -1);
                ResetForm();

                MessageBox.Show(
                    $"Nhập dữ liệu hoàn tất.\nThêm mới: {result.SoThemMoi}\nCập nhật: {result.SoCapNhat}\nBỏ qua: {result.SoBoQua}",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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

        private void btnXuatNhanVien_Click(object? sender, EventArgs e)
        {
            if (dgvDanhSachNhanVien.DataSource is not List<NhanVienDTO> dsNhanVien || dsNhanVien.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var tieuDeCot = new[]
            {
                "ID",
                "Ho va ten",
                "Dien thoai",
                "Dia chi",
                "Ten dang nhap",
                "Quyen han",
                "Trang thai"
            };

            var duLieu = dsNhanVien
                .Select(x => (IReadOnlyList<string>)new[]
                {
                    x.ID.ToString(),
                    x.HoVaTen,
                    x.DienThoai ?? string.Empty,
                    x.DiaChi ?? string.Empty,
                    x.TenDangNhap,
                    x.QuyenHan,
                    x.IsDeleted ? "Ngung hoat dong" : "Hoat dong"
                })
                .ToList();

            var ketQua = _dataExportService.XuatBangDuLieu(
                this,
                "Xuat danh sach nhan vien",
                $"DanhSachNhanVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                "Danh sach nhan vien",
                tieuDeCot,
                duLieu);

            if (ketQua.DaHuy)
            {
                return;
            }

            MessageBox.Show(
                ketQua.ThongBao,
                "Thong bao",
                MessageBoxButtons.OK,
                ketQua.ThanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
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
            nhanVienDTO = new NhanVienDTO();

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

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Tên đăng nhập không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return false;
            }

            if (!laCapNhat && string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Mật khẩu không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            if (cboQuyenHan.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn quyền hạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboQuyenHan.Focus();
                return false;
            }

            nhanVienDTO = new NhanVienDTO
            {
                HoVaTen = txtHoVaTen.Text.Trim(),
                DienThoai = string.IsNullOrWhiteSpace(soDienThoai) ? null : soDienThoai,
                DiaChi = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim(),
                TenDangNhap = txtTenDangNhap.Text.Trim(),
                MatKhau = string.IsNullOrWhiteSpace(txtMatKhau.Text) ? null : txtMatKhau.Text.Trim(),
                QuyenHan = cboQuyenHan.SelectedItem.ToString() ?? string.Empty
            };

            return true;
        }

        private void SelectRow(int id)
        {
            DataGridViewSelectionHelper.ClearSelection(dgvDanhSachNhanVien);

            foreach (DataGridViewRow row in dgvDanhSachNhanVien.Rows)
            {
                if (row.DataBoundItem is not NhanVienDTO nhanVien || nhanVien.ID != id)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachNhanVien.CurrentCell = row.Cells[0];
                return;
            }
        }

        private void dgvDanhSachNhanVien_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachNhanVien.CurrentRow?.DataBoundItem is not NhanVienDTO nhanVien)
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
            base.OnFormClosed(e);
        }
    }
}
