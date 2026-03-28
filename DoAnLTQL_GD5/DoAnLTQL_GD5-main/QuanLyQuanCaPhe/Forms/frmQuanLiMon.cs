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
        private readonly LoaiMonBUS _loaiMonBUS = new();
        private readonly MonInputValidator _monInputValidator = new();
        private readonly MonCsvService _monCsvService = new();
        private string? _selectedImagePath;
        private int? _selectedLoaiMonId;

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

            dgvLoaiMon.AutoGenerateColumns = false;
            colIDLoaiMon.DataPropertyName = nameof(LoaiMonDTO.ID);
            colTenLoaiMon.DataPropertyName = nameof(LoaiMonDTO.TenLoai);

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
            btnLoaiMon.Visible = false;
            LoadDanhSachLoaiMon();
            LoadLoaiMonCombobox();
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

        private void LoadDanhSachLoaiMon(int? selectedLoaiMonId = null)
        {
            var dsLoai = _loaiMonBUS.LayDanhSachLoai(null, null);
            dgvLoaiMon.DataSource = dsLoai;

            if (dsLoai.Count == 0)
            {
                _selectedLoaiMonId = null;
                txtMaLoaiMon.Clear();
                txtTenLoaiMon.Clear();
                LoadDanhSachMon();
                return;
            }

            var idCanChon = selectedLoaiMonId ?? _selectedLoaiMonId ?? dsLoai[0].ID;
            SelectLoaiMonRow(idCanChon);
        }

        private void LoadLoaiMonCombobox()
        {
            var dsLoai = _loaiMonBUS.LayDanhSachLoai(null, null);

            cboLoaiMon.DataSource = dsLoai;
            cboLoaiMon.DisplayMember = nameof(LoaiMonDTO.TenLoai);
            cboLoaiMon.ValueMember = nameof(LoaiMonDTO.ID);

            if (_selectedLoaiMonId.HasValue && dsLoai.Any(x => x.ID == _selectedLoaiMonId.Value))
            {
                cboLoaiMon.SelectedValue = _selectedLoaiMonId.Value;
            }
            else
            {
                cboLoaiMon.SelectedIndex = dsLoai.Count > 0 ? 0 : -1;
            }
        }

        private void LoadDanhSachMon()
        {
            var dsMon = _monBUS.LayDanhSachMon(txtSearch.Text, txtTimMon.Text, _selectedLoaiMonId);

            dgvDanhSachMon.DataSource = dsMon;

            lblTongMonValue.Text = dsMon.Count.ToString();
            lblLoaiMonValue.Text = (dgvLoaiMon.DataSource as List<LoaiMonDTO>)?.Count.ToString() ?? "0";
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

            LoadDanhSachLoaiMon(_selectedLoaiMonId);
            LoadLoaiMonCombobox();
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

            LoadDanhSachLoaiMon(_selectedLoaiMonId);
            LoadLoaiMonCombobox();
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

            LoadDanhSachLoaiMon(_selectedLoaiMonId);
            LoadLoaiMonCombobox();
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

        private void btnThemLoaiMon_Click(object? sender, EventArgs e)
        {
            var result = _loaiMonBUS.ThemLoai(txtTenLoaiMon.Text);
            if (!result.ThanhCong || result.LoaiMoi == null)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenLoaiMon.Focus();
                return;
            }

            LoadDanhSachLoaiMon(result.LoaiMoi.ID);
            LoadLoaiMonCombobox();
            ResetForm();
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSuaLoaiMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLoaiMon.Text, out var id))
            {
                MessageBox.Show("Vui lòng chọn loại món cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _loaiMonBUS.CapNhatLoai(id, txtTenLoaiMon.Text);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenLoaiMon.Focus();
                return;
            }

            LoadDanhSachLoaiMon(id);
            LoadLoaiMonCombobox();
            ResetForm();
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaLoaiMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLoaiMon.Text, out var id))
            {
                MessageBox.Show("Vui lòng chọn loại món cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tenLoai = txtTenLoaiMon.Text.Trim();
            if (MessageBox.Show($"Xóa loại món '{tenLoai}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            var result = _loaiMonBUS.XoaLoai(id);
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
                        var loaiDichId = ChonLoaiMonDich(id);
                        if (!loaiDichId.HasValue)
                        {
                            return;
                        }

                        var chuyenResult = _loaiMonBUS.ChuyenMonSangLoaiKhac(id, loaiDichId.Value);
                        if (!chuyenResult.ThanhCong)
                        {
                            MessageBox.Show(chuyenResult.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var xoaSauKhiChuyen = _loaiMonBUS.XoaLoai(id);
                        if (!xoaSauKhiChuyen.ThanhCong)
                        {
                            MessageBox.Show(xoaSauKhiChuyen.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        _selectedLoaiMonId = loaiDichId.Value;
                        LoadDanhSachLoaiMon(_selectedLoaiMonId);
                        LoadLoaiMonCombobox();
                        ResetForm();
                        MessageBox.Show("Đã chuyển món sang loại mới và xóa loại món cũ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _selectedLoaiMonId = null;
            LoadDanhSachLoaiMon();
            LoadLoaiMonCombobox();
            ResetForm();
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dgvLoaiMon_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvLoaiMon.CurrentRow?.DataBoundItem is not LoaiMonDTO loai)
            {
                _selectedLoaiMonId = null;
                txtMaLoaiMon.Clear();
                txtTenLoaiMon.Clear();
                lblDanhSachMonTitle.Text = "Danh sách món";
                LoadDanhSachMon();
                return;
            }

            _selectedLoaiMonId = loai.ID;
            txtMaLoaiMon.Text = loai.ID.ToString();
            txtTenLoaiMon.Text = loai.TenLoai;
            lblDanhSachMonTitle.Text = $"Danh sách món - {loai.TenLoai}";

            if (cboLoaiMon.DataSource is List<LoaiMonDTO> dsLoai && dsLoai.Any(x => x.ID == loai.ID))
            {
                cboLoaiMon.SelectedValue = loai.ID;
            }

            LoadDanhSachMon();
            ResetForm();
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

        private void SelectLoaiMonRow(int loaiMonId)
        {
            foreach (DataGridViewRow row in dgvLoaiMon.Rows)
            {
                if (row.DataBoundItem is not LoaiMonDTO loai || loai.ID != loaiMonId)
                {
                    continue;
                }

                row.Selected = true;
                dgvLoaiMon.CurrentCell = row.Cells[0];
                break;
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

            if (_selectedLoaiMonId.HasValue && cboLoaiMon.Items.Count > 0)
            {
                cboLoaiMon.SelectedValue = _selectedLoaiMonId.Value;
            }

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
