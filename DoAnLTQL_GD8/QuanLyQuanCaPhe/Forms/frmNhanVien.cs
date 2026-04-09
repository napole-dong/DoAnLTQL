using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmNhanVien : Form
    {
        private readonly bool _isEmbedded;
        private readonly NhanVienBUS _nhanVienBUS = new();
        private readonly PermissionBUS _permissionBUS = new();

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

            btnBanHang.Click += (_, _) => OpenStandaloneForm(new frmBanHang(), PermissionFeatures.BanHang);
            btnQuanLyBan.Click += (_, _) => OpenStandaloneForm(new frmQuanLiBan(), PermissionFeatures.Menu);
            btnQuanLyMon.Click += (_, _) => OpenStandaloneForm(new frmQuanLiMon(), PermissionFeatures.Menu);
            btnKhachHang.Click += (_, _) => OpenStandaloneForm(new frmKhachHang(), PermissionFeatures.KhachHang);
            btnThongKe.Click += (_, _) => MoTinhNangThongKe();
            btnHoaDon.Click += (_, _) => OpenStandaloneForm(new frmHoaDon(), PermissionFeatures.HoaDon);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void frmNhanVien_Load(object? sender, EventArgs e)
        {
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

            LoadDanhSachNhanVien();
            ResetForm();
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

            btnThemNhanVien.Enabled = coQuyenThemNhanVien;
            btnCapNhatNhanVien.Enabled = coQuyenCapNhatNhanVien;
            btnXoaNhanVien.Enabled = coQuyenXoaNhanVien;
            btnNhapNhanVien.Enabled = coQuyenThemNhanVien || coQuyenCapNhatNhanVien;
            btnXuatNhanVien.Enabled = coQuyenXemNhanVien;
            btnTimNhanVien.Enabled = coQuyenXemNhanVien;
            txtTimNhanVien.Enabled = coQuyenXemNhanVien;
            cboQuyenHan.Enabled = coQuyenThemNhanVien || coQuyenCapNhatNhanVien;
            txtMatKhau.Enabled = coQuyenThemNhanVien || coQuyenCapNhatNhanVien;

            btnBanHang.Visible = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View);

            var coQuyenMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
            var coQuyenKhachHang = _permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.View);
            var coQuyenHoaDon = _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);
            btnQuanLyBan.Visible = coQuyenMenu;
            btnQuanLyMon.Visible = coQuyenMenu;
            btnHoaDon.Visible = coQuyenHoaDon;
            btnKhachHang.Visible = coQuyenKhachHang;

            btnNhanVien.Visible = coQuyenXemNhanVien;
            btnThongKe.Visible = _permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View);
        }

        private void LoadDanhSachNhanVien()
        {
            var dsNhanVien = _nhanVienBUS.LayDanhSachNhanVien(txtTimNhanVien.Text);
            dgvDanhSachNhanVien.DataSource = dsNhanVien;

            lblTongNhanVienValue.Text = dsNhanVien.Count.ToString();
            lblQuanLyValue.Text = dsNhanVien.Count(x => x.QuyenHan.Equals("Quản lý", StringComparison.OrdinalIgnoreCase)).ToString();
            lblThuNganValue.Text = (dsNhanVien.Count - dsNhanVien.Count(x => x.QuyenHan.Equals("Quản lý", StringComparison.OrdinalIgnoreCase))).ToString();
        }

        private void btnThemNhanVien_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput(out var nhanVienDTO, false))
            {
                return;
            }

            var result = _nhanVienBUS.ThemNhanVien(nhanVienDTO);
            if (!result.ThanhCong || result.NhanVienMoi == null)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachNhanVien();
            SelectRow(result.NhanVienMoi.ID);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCapNhatNhanVien_Click(object? sender, EventArgs e)
        {
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
            var result = _nhanVienBUS.CapNhatNhanVien(nhanVienDTO);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachNhanVien();
            SelectRow(nhanVienId);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaNhanVien_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaNhanVien.Text, out var nhanVienId))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDanhSachNhanVien.CurrentRow?.DataBoundItem is not NhanVienDTO nhanVien)
            {
                MessageBox.Show("Không tìm thấy nhân viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa nhân viên '{nhanVien.HoVaTen}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            var result = _nhanVienBUS.XoaNhanVien(nhanVienId);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachNhanVien();
            ResetForm();
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            LoadDanhSachNhanVien();
        }

        private void btnTimNhanVien_Click(object? sender, EventArgs e)
        {
            LoadDanhSachNhanVien();
        }

        private void txtTimNhanVien_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadDanhSachNhanVien();
            e.SuppressKeyPress = true;
        }

        private void btnNhapNhanVien_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                Title = "Nhập danh sách nhân viên"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var lines = File.ReadAllLines(dialog.FileName, Encoding.UTF8);
            if (lines.Length == 0)
            {
                MessageBox.Show("Tệp nhập không có dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _nhanVienBUS.NhapNhanVienTuCsv(lines);
            LoadDanhSachNhanVien();
            ResetForm();

            MessageBox.Show(
                $"Nhập dữ liệu hoàn tất.\nThêm mới: {result.SoThemMoi}\nCập nhật: {result.SoCapNhat}\nBỏ qua: {result.SoBoQua}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnXuatNhanVien_Click(object? sender, EventArgs e)
        {
            if (dgvDanhSachNhanVien.DataSource is not List<NhanVienDTO> dsNhanVien || dsNhanVien.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"DanhSachNhanVien_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Xuất danh sách nhân viên"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var lines = new List<string>
            {
                "ID,HoVaTen,DienThoai,DiaChi,TenDangNhap,QuyenHan"
            };

            lines.AddRange(dsNhanVien.Select(x => string.Join(",",
                x.ID,
                EscapeCsv(x.HoVaTen),
                EscapeCsv(x.DienThoai ?? string.Empty),
                EscapeCsv(x.DiaChi ?? string.Empty),
                EscapeCsv(x.TenDangNhap),
                EscapeCsv(x.QuyenHan))));

            File.WriteAllLines(dialog.FileName, lines, Encoding.UTF8);
            MessageBox.Show("Xuất danh sách nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            foreach (DataGridViewRow row in dgvDanhSachNhanVien.Rows)
            {
                if (row.DataBoundItem is not NhanVienDTO nhanVien || nhanVien.ID != id)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachNhanVien.CurrentCell = row.Cells[0];
                break;
            }
        }

        private static string EscapeCsv(string value)
        {
            if (!value.Contains(',') && !value.Contains('"') && !value.Contains('\n') && !value.Contains('\r'))
            {
                return value;
            }

            return $"\"{value.Replace("\"", "\"\"")}\"";
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

        private void OpenStandaloneForm(Form targetForm, string feature)
        {
            if (_isEmbedded)
            {
                targetForm.Dispose();
                return;
            }

            if (!_permissionBUS.CheckPermission(feature, PermissionActions.View))
            {
                targetForm.Dispose();
                MessageBox.Show("Ban khong co quyen truy cap chuc nang nay.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Hide();
            targetForm.FormClosed += (_, _) =>
            {
                if (!IsDisposed && !Disposing)
                {
                    Show();
                    BringToFront();
                    Activate();
                    LoadDanhSachNhanVien();
                }
            };

            targetForm.Show(this);
        }

        private void MoTinhNangThongKe()
        {
            OpenStandaloneForm(new frmThongKe(), PermissionFeatures.ThongKe);
        }

        private void MoTinhNangHoaDon()
        {
            OpenStandaloneForm(new frmHoaDon(), PermissionFeatures.HoaDon);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }
    }
}
