using System.IO;
using System.Linq;
using System.Text;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiKho : Form
    {
        private readonly NguyenLieuBUS _nguyenLieuBUS = new();
        private readonly PermissionBUS _permissionBUS = new();

        public frmQuanLiKho()
        {
            InitializeComponent();

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

            btnThemNguyenLieu.Click += btnThemNguyenLieu_Click;
            btnCapNhatNguyenLieu.Click += btnCapNhatNguyenLieu_Click;
            btnXoaNguyenLieu.Click += btnXoaNguyenLieu_Click;
            btnNhapKho.Click += btnNhapKho_Click;
            btnXuatKho.Click += btnXuatKho_Click;
            KhoiTaoNutLamMoi();

            btnBanHang.Click += (_, _) => OpenStandaloneForm(new frmBanHang(), PermissionFeatures.BanHang);
            btnQuanLyBan.Click += (_, _) => OpenStandaloneForm(new frmQuanLiBan(), PermissionFeatures.Menu);
            btnQuanLyMon.Click += (_, _) => OpenStandaloneForm(new frmQuanLiMon(), PermissionFeatures.Menu);
            btnQuanLyKho.Click += (_, _) => OpenStandaloneForm(new frmQuanLiKho(), PermissionFeatures.NguyenLieu);
            btnNhanVien.Click += (_, _) => OpenStandaloneForm(new frmNhanVien(), PermissionFeatures.NhanVien);
            btnHoaDon.Click += (_, _) => OpenStandaloneForm(new frmHoaDon(), PermissionFeatures.HoaDon);
            btnThongKe.Click += (_, _) => MoTinhNangThongKe();
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void frmQuanLiKho_Load(object? sender, EventArgs e)
        {
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
            ResetForm();
        }

        private void HienThiNguoiDungDangNhap()
        {
        }

        private void ApDungPhanQuyenLenUI()
        {
            if (_permissionBUS.IsAdmin())
            {
                btnThemNguyenLieu.Enabled = true;
                btnCapNhatNguyenLieu.Enabled = true;
                btnXoaNguyenLieu.Enabled = true;
                btnNhapKho.Enabled = true;
                btnXuatKho.Enabled = true;
                txtTimNguyenLieu.Enabled = true;
                dgvDanhSachKho.Enabled = true;

                btnBanHang.Visible = true;
                btnQuanLyBan.Visible = true;
                btnQuanLyMon.Visible = true;
                btnQuanLyKho.Visible = true;
                btnNhanVien.Visible = true;
                btnHoaDon.Visible = true;
                btnThongKe.Visible = true;
                return;
            }

            var coQuyenKhoXem = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.View);
            var coQuyenKhoThem = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Create);
            var coQuyenKhoCapNhat = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Update);
            var coQuyenKhoXoa = _permissionBUS.CheckPermission(PermissionFeatures.NguyenLieu, PermissionActions.Delete);
            var coQuyenMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
            var coQuyenHoaDon = _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);

            btnThemNguyenLieu.Enabled = coQuyenKhoThem;
            btnCapNhatNguyenLieu.Enabled = coQuyenKhoCapNhat;
            btnXoaNguyenLieu.Enabled = coQuyenKhoXoa;
            btnNhapKho.Enabled = coQuyenKhoCapNhat;
            btnXuatKho.Enabled = coQuyenKhoXem;
            txtTimNguyenLieu.Enabled = coQuyenKhoXem;
            dgvDanhSachKho.Enabled = coQuyenKhoXem;

            btnBanHang.Visible = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View);
            btnQuanLyBan.Visible = coQuyenMenu;
            btnQuanLyMon.Visible = coQuyenMenu;
            btnQuanLyKho.Visible = coQuyenKhoXem;
            btnNhanVien.Visible = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View);
            btnHoaDon.Visible = coQuyenHoaDon;
            btnThongKe.Visible = _permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View);
        }

        private void KhoiTaoNutLamMoi()
        {
            var btnLamMoi = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(248, 245, 241),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(65, 48, 39),
                Location = new Point(Math.Max(0, btnNhapKho.Left - 101), btnNhapKho.Top),
                Name = "btnLamMoiKho",
                Size = new Size(95, 32),
                TabStop = false,
                Text = "Làm mới",
                UseVisualStyleBackColor = false
            };

            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.Click += btnLamMoi_Click;

            panelDanhSachHeader.Controls.Add(btnLamMoi);
            btnLamMoi.BringToFront();
        }

        private void LoadDanhSachKho()
        {
            var dsNguyenLieu = _nguyenLieuBUS.LayDanhSachNguyenLieu(txtTimNguyenLieu.Text);
            dgvDanhSachKho.DataSource = dsNguyenLieu;

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
            if (!ValidateInput(out var nguyenLieuDTO))
            {
                return;
            }

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

        private void btnCapNhatNguyenLieu_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaNguyenLieu.Text, out var maNguyenLieu))
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out var nguyenLieuDTO))
            {
                return;
            }

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

        private void btnXoaNguyenLieu_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaNguyenLieu.Text, out var maNguyenLieu))
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDanhSachKho.CurrentRow?.DataBoundItem is not NguyenLieuDTO nguyenLieu)
            {
                MessageBox.Show("Không tìm thấy nguyên liệu để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa nguyên liệu '{nguyenLieu.TenNguyenLieu}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            var result = _nguyenLieuBUS.XoaNguyenLieu(maNguyenLieu);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachKho();
            ResetForm();
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimNguyenLieu.Clear();
            LoadDanhSachKho();
            ResetForm();
        }

        private void btnNhapKho_Click(object? sender, EventArgs e)
        {
            if (dgvDanhSachKho.CurrentRow?.DataBoundItem is not NguyenLieuDTO nguyenLieuDangChon)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu để nhập kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtSoLuongTon.Text.Trim(), out var soLuongNhap) || soLuongNhap <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng nhập lớn hơn 0 vào ô Số lượng tồn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongTon.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaNhapGanNhat.Text.Trim(), out var giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhapGanNhat.Focus();
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Xác nhận nhập thêm {soLuongNhap:N2} {nguyenLieuDangChon.DonViTinh} cho '{nguyenLieuDangChon.TenNguyenLieu}'?",
                "Xác nhận nhập kho",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            var result = _nguyenLieuBUS.NhapKho(
                nguyenLieuDangChon.MaNguyenLieu,
                soLuongNhap,
                giaNhap,
                "Nhap kho thu cong tu man hinh quan ly kho.");

            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachKho();
            SelectRow(nguyenLieuDangChon.MaNguyenLieu);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXuatKho_Click(object? sender, EventArgs e)
        {
            if (dgvDanhSachKho.DataSource is not List<NguyenLieuDTO> dsNguyenLieu || dsNguyenLieu.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"DanhSachKho_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Xuất danh sách nguyên liệu"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var lines = new List<string>
            {
                "MaNguyenLieu,TenNguyenLieu,DonViTinh,SoLuongTon,MucCanhBao,GiaNhapGanNhat,TrangThai"
            };

            lines.AddRange(dsNguyenLieu.Select(x => string.Join(",",
                x.MaNguyenLieu,
                EscapeCsv(x.TenNguyenLieu),
                EscapeCsv(x.DonViTinh),
                x.SoLuongTon,
                x.MucCanhBao,
                x.GiaNhapGanNhat,
                EscapeCsv(x.TrangThaiHienThi))));

            File.WriteAllLines(dialog.FileName, lines, Encoding.UTF8);
            MessageBox.Show("Xuất danh sách kho thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtTimNguyenLieu_TextChanged(object? sender, EventArgs e)
        {
            LoadDanhSachKho();
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
            foreach (DataGridViewRow row in dgvDanhSachKho.Rows)
            {
                if (row.DataBoundItem is not NguyenLieuDTO nguyenLieu || nguyenLieu.MaNguyenLieu != maNguyenLieu)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachKho.CurrentCell = row.Cells[0];
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

        private void OpenStandaloneForm(Form targetForm, string feature)
        {
            try
            {
                _permissionBUS.EnsurePermission(feature, PermissionActions.View, "Ban khong co quyen truy cap chuc nang nay.");
            }
            catch (UnauthorizedAccessException ex)
            {
                targetForm.Dispose();
                MessageBox.Show(ex.Message, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    LoadDanhSachKho();
                }
            };

            targetForm.Show(this);
        }

        private void MoTinhNangThongKe()
        {
            if (!_permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View))
            {
                MessageBox.Show("Ban khong co quyen truy cap chuc nang thong ke.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Chuc nang thong ke dang duoc phat trien.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }
    }
}
