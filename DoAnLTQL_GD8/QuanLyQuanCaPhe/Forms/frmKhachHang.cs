using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class frmKhachHang : Form
    {
        private readonly bool _isEmbedded;
        private readonly KhachHangBUS _khachHangBUS = new();
        private readonly PermissionBUS _permissionBUS = new();
        private readonly DataExportService _dataExportService = new();
        private CheckBox? _chkHienThiDaXoa;

        public frmKhachHang(bool isEmbedded = false)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();

            dgvDanhSachKhach.AutoGenerateColumns = false;
            colIDKhach.DataPropertyName = nameof(KhachHangDTO.ID);
            colHoVaTen.DataPropertyName = nameof(KhachHangDTO.HoVaTen);
            colDienThoai.DataPropertyName = nameof(KhachHangDTO.DienThoai);
            colDiaChi.DataPropertyName = nameof(KhachHangDTO.DiaChi);

            Load += frmKhachHang_Load;
            txtTimKhach.KeyDown += txtTimKhach_KeyDown;
            dgvDanhSachKhach.SelectionChanged += dgvDanhSachKhach_SelectionChanged;
            btnTimKhach.Click += btnTimKhach_Click;
            KhoiTaoNutLamMoi();
            KhoiTaoBoLocDuLieuDaXoa();

            btnXoaKhach.Text = "Ngừng hoạt động";

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
            btnKhachHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), onCurrentFormReactivated: LoadDanhSachKhach, skipNavigation: _isEmbedded);
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
                panelSidebar.Visible = false;
                panelTopbar.Visible = false;
                panelMain.Dock = DockStyle.Fill;
            }

            HienThiNguoiDungDangNhap();

            LoadDanhSachKhach();
        }

        private void HienThiNguoiDungDangNhap()
        {
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
                Location = new Point(Math.Max(0, btnNhapKhach.Left - 90), btnNhapKhach.Top),
                Name = "btnLamMoiKhach",
                Size = new Size(84, 32),
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
                Location = new Point(Math.Max(0, btnNhapKhach.Left - 280), btnNhapKhach.Top + 8),
                Name = "chkHienThiKhachDaXoa",
                TabStop = false,
                Text = "Hiển thị dữ liệu đã xóa"
            };

            _chkHienThiDaXoa.CheckedChanged += (_, _) =>
            {
                LoadDanhSachKhach();
            };

            panelDanhSachHeader.Controls.Add(_chkHienThiDaXoa);
            _chkHienThiDaXoa.BringToFront();
        }

        private void LoadDanhSachKhach()
        {
            LoadDanhSachKhach(false, -1);
        }

        private void LoadDanhSachKhach(bool khoiPhucLuaChon, int viTriUuTien)
        {
            var includeDeleted = _chkHienThiDaXoa?.Checked == true;
            var dsKhach = _khachHangBUS.LayDanhSachKhach(txtTimKhach.Text, includeDeleted);
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
            if (!ValidateInput(out var khachDTO))
            {
                return;
            }

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

        private void btnCapNhatKhach_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaKhach.Text, out var khachId))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out var khachDTO))
            {
                return;
            }

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

        private void btnXoaKhach_Click(object? sender, EventArgs e)
        {
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

            var result = _khachHangBUS.XoaKhach(khach.ID);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachKhach(khoiPhucLuaChon: false, viTriUuTien: viTriDangChon);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            LoadDanhSachKhach();
        }

        private void btnNhapKhach_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                Title = "Nhập danh sách khách hàng"
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

            var result = _khachHangBUS.NhapKhachTuCsv(lines);
            LoadDanhSachKhach();
            ResetForm();

            MessageBox.Show(
                $"Nhập dữ liệu hoàn tất.\nThêm mới: {result.SoThemMoi}\nCập nhật: {result.SoCapNhat}\nBỏ qua: {result.SoBoQua}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnXuatKhach_Click(object? sender, EventArgs e)
        {
            if (dgvDanhSachKhach.DataSource is not List<KhachHangDTO> dsKhach || dsKhach.Count == 0)
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
                "Trang thai"
            };

            var duLieu = dsKhach
                .Select(x => (IReadOnlyList<string>)new[]
                {
                    x.ID.ToString(),
                    x.HoVaTen,
                    x.DienThoai ?? string.Empty,
                    x.DiaChi ?? string.Empty,
                    x.IsDeleted ? "Ngung hoat dong" : "Hoat dong"
                })
                .ToList();

            var ketQua = _dataExportService.XuatBangDuLieu(
                this,
                "Xuat danh sach khach hang",
                $"DanhSachKhachHang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                "Danh sach khach hang",
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

        private void btnTimKhach_Click(object? sender, EventArgs e)
        {
            LoadDanhSachKhach();
        }

        private void txtTimKhach_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadDanhSachKhach();
            e.SuppressKeyPress = true;
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
