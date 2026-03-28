using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Mon;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiMon : Form
    {
        private readonly bool _isEmbedded;
        private readonly MonBUS _monBUS = new();
        private readonly MonInputValidator _monInputValidator = new();
        private readonly MonCsvService _monCsvService = new();
        private string? _selectedImagePath;

        public frmQuanLiMon(bool isEmbedded = false)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();

            dgvDanhSachMon.AutoGenerateColumns = false;
            colIDMon.DataPropertyName = nameof(MonDTO.ID);
            colTenMon.DataPropertyName = nameof(MonDTO.TenMon);
            colLoaiMon.DataPropertyName = nameof(MonDTO.TenLoaiMon);
            colDonGia.DataPropertyName = nameof(MonDTO.DonGiaHienThi);
            colMoTa.DataPropertyName = nameof(MonDTO.MoTa);

            Load += frmQuanLiMon_Load;
            txtTimMon.TextChanged += FilterChanged;
            txtSearch.TextChanged += FilterChanged;
            txtDuongDanAnh.Leave += txtDuongDanAnh_Leave;
            btnNhapMon.Click += btnNhapMon_Click;
            btnXuatMon.Click += btnXuatMon_Click;
        }

        private void frmQuanLiMon_Load(object? sender, EventArgs e)
        {
            if (_isEmbedded)
            {
                panelSidebar.Visible = false;
                panelTopbar.Visible = false;
                panelMain.Dock = DockStyle.Fill;
            }

            picCardAnhMon.Image = null;
            LoadLoaiMon();
            LoadDanhSachMon();
            ResetForm();
        }

        private void SetPreviewImage(string? imagePath)
        {
            var oldImage = picCardAnhMon.Image;
            picCardAnhMon.Image = null;
            oldImage?.Dispose();

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return;
            }

            try
            {
                if (Uri.TryCreate(imagePath, UriKind.Absolute, out var uri)
                    && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    picCardAnhMon.LoadAsync(imagePath);
                    return;
                }

                if (!File.Exists(imagePath))
                {
                    return;
                }

                using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                using var buffer = new MemoryStream();
                stream.CopyTo(buffer);
                buffer.Position = 0;
                picCardAnhMon.Image = Image.FromStream(buffer);
            }
            catch
            {
                picCardAnhMon.Image = null;
            }
        }

        private void LoadLoaiMon()
        {
            var dsLoai = _monBUS.LayDanhSachLoaiMon();

            cboLoaiMon.DataSource = dsLoai;
            cboLoaiMon.DisplayMember = nameof(LoaiMonDTO.TenLoai);
            cboLoaiMon.ValueMember = nameof(LoaiMonDTO.ID);
            cboLoaiMon.SelectedIndex = dsLoai.Count > 0 ? 0 : -1;
        }

        private void LoadDanhSachMon()
        {
            var dsMon = _monBUS.LayDanhSachMon(txtSearch.Text, txtTimMon.Text);

            dgvDanhSachMon.DataSource = dsMon;

            lblTongMonValue.Text = dsMon.Count.ToString();
            lblLoaiMonValue.Text = dsMon.Select(x => x.LoaiMonID).Distinct().Count().ToString();
            lblGiaTrungBinhValue.Text = dsMon.Count == 0
                ? "0đ"
                : $"{Math.Round(dsMon.Average(x => x.DonGia), 0):N0}đ";

            txtMaMon.Text = _monBUS.LayMaMonTiepTheo().ToString();
        }

        private void btnChonHinh_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.webp",
                Title = "Chọn hình ảnh món"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            _selectedImagePath = dialog.FileName;
            txtDuongDanAnh.Text = _selectedImagePath;
            SetPreviewImage(_selectedImagePath);
        }

        private void btnThemMon_Click(object? sender, EventArgs e)
        {
            if (!TryGetValidatedMon(out var monDTO))
            {
                return;
            }

            var result = _monBUS.ThemMon(monDTO);
            if (!result.ThanhCong || result.MonMoi == null)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachMon();
            SelectRow(result.MonMoi.ID);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCapNhatMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaMon.Text, out var id))
            {
                MessageBox.Show("Vui lòng chọn món cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryGetValidatedMon(out var monDTO))
            {
                return;
            }

            monDTO.ID = id;
            var result = _monBUS.CapNhatMon(monDTO);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachMon();
            SelectRow(id);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaMon.Text, out var id))
            {
                MessageBox.Show("Vui lòng chọn món cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDanhSachMon.CurrentRow?.DataBoundItem is not MonDTO mon)
            {
                MessageBox.Show("Không tìm thấy món để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa món '{mon.TenMon}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            var result = _monBUS.XoaMon(id);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachMon();
            ResetForm();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            txtTimMon.Clear();
            LoadDanhSachMon();
            ResetForm();
        }

        private void btnXuatMon_Click(object? sender, EventArgs e)
        {
            if (dgvDanhSachMon.DataSource is not List<MonDTO> dsMon || dsMon.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"DanhSachMon_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Xuất danh sách món"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            _monCsvService.XuatCsv(dialog.FileName, dsMon);
            MessageBox.Show("Xuất danh sách món thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNhapMon_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                Title = "Nhập danh sách món"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var lines = _monCsvService.DocCsv(dialog.FileName);
            if (lines.Length == 0)
            {
                MessageBox.Show("Tệp nhập không có dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _monBUS.NhapMonTuCsv(lines);
            LoadDanhSachMon();
            ResetForm();

            MessageBox.Show(
                $"Nhập dữ liệu hoàn tất.\nThêm mới: {result.SoThemMoi}\nCập nhật: {result.SoCapNhat}\nBỏ qua: {result.SoBoQua}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void FilterChanged(object? sender, EventArgs e)
        {
            LoadDanhSachMon();
        }

        private void txtDuongDanAnh_Leave(object? sender, EventArgs e)
        {
            _selectedImagePath = string.IsNullOrWhiteSpace(txtDuongDanAnh.Text)
                ? null
                : txtDuongDanAnh.Text.Trim();
            SetPreviewImage(_selectedImagePath);
        }

        private bool TryGetValidatedMon(out MonDTO monDTO)
        {
            var validation = _monInputValidator.Validate(
                txtTenMon.Text,
                cboLoaiMon.SelectedValue,
                txtDonGia.Text,
                txtMoTa.Text,
                txtDuongDanAnh.Text);

            if (validation.HopLe && validation.Mon is not null)
            {
                monDTO = validation.Mon;
                return true;
            }

            monDTO = new MonDTO();
            MessageBox.Show(validation.ThongBao ?? "Dữ liệu không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            FocusInvalidField(validation.TruongLoi);
            return false;
        }

        private void FocusInvalidField(MonInputField truongLoi)
        {
            switch (truongLoi)
            {
                case MonInputField.TenMon:
                    txtTenMon.Focus();
                    break;
                case MonInputField.DonGia:
                    txtDonGia.Focus();
                    break;
                case MonInputField.LoaiMon:
                    cboLoaiMon.Focus();
                    break;
            }
        }

        private void SelectRow(int id)
        {
            foreach (DataGridViewRow row in dgvDanhSachMon.Rows)
            {
                if (row.DataBoundItem is MonDTO mon && mon.ID == id)
                {
                    row.Selected = true;
                    dgvDanhSachMon.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void ResetForm()
        {
            txtTenMon.Clear();
            txtDonGia.Clear();
            txtMoTa.Clear();
            txtDuongDanAnh.Clear();
            _selectedImagePath = null;
            SetPreviewImage(null);
            txtMaMon.Text = _monBUS.LayMaMonTiepTheo().ToString();
        }

        private void dgvDanhSachMon_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachMon.CurrentRow?.DataBoundItem is not MonDTO mon)
            {
                SetPreviewImage(null);
                return;
            }

            txtMaMon.Text = mon.ID.ToString();
            txtTenMon.Text = mon.TenMon;
            txtDonGia.Text = mon.DonGia.ToString();
            txtMoTa.Text = mon.MoTa;
            cboLoaiMon.SelectedValue = mon.LoaiMonID;
            _selectedImagePath = mon.HinhAnh;
            txtDuongDanAnh.Text = mon.HinhAnh ?? string.Empty;
            SetPreviewImage(mon.HinhAnh);
        }

    }
}
