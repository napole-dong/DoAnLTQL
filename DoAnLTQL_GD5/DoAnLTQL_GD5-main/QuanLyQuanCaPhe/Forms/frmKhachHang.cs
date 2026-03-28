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

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmKhachHang : Form
    {
        private readonly bool _isEmbedded;
        private readonly KhachHangBUS _khachHangBUS = new();

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
        }

        private void frmKhachHang_Load(object? sender, EventArgs e)
        {
            if (_isEmbedded)
            {
                panelSidebar.Visible = false;
                panelTopbar.Visible = false;
                panelMain.Dock = DockStyle.Fill;
            }

            LoadDanhSachKhach();
            ResetForm();
        }

        private void LoadDanhSachKhach()
        {
            var dsKhach = _khachHangBUS.LayDanhSachKhach(txtTimKhach.Text);
            dgvDanhSachKhach.DataSource = dsKhach;

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
            if (!int.TryParse(txtMaKhach.Text, out var khachId))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDanhSachKhach.CurrentRow?.DataBoundItem is not KhachHangDTO khach)
            {
                MessageBox.Show("Không tìm thấy khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa khách hàng '{khach.HoVaTen}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            var result = _khachHangBUS.XoaKhach(khachId);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachKhach();
            ResetForm();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimKhach.Clear();
            LoadDanhSachKhach();
            ResetForm();
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

            using var dialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"DanhSachKhachHang_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Xuất danh sách khách hàng"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var lines = new List<string>
            {
                "ID,HoVaTen,DienThoai,DiaChi"
            };

            lines.AddRange(dsKhach.Select(x => string.Join(",",
                x.ID,
                EscapeCsv(x.HoVaTen),
                EscapeCsv(x.DienThoai ?? string.Empty),
                EscapeCsv(x.DiaChi ?? string.Empty))));

            File.WriteAllLines(dialog.FileName, lines, Encoding.UTF8);
            MessageBox.Show("Xuất danh sách khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            foreach (DataGridViewRow row in dgvDanhSachKhach.Rows)
            {
                if (row.DataBoundItem is KhachHangDTO khach && khach.ID == id)
                {
                    row.Selected = true;
                    dgvDanhSachKhach.CurrentCell = row.Cells[0];
                    break;
                }
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
    }
}
