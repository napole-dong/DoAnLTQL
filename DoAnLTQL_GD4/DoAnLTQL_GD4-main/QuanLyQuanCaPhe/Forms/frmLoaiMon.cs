using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmLoaiMon : Form
    {
        private readonly bool _isEmbedded;
        private readonly LoaiMonBUS _loaiMonBUS = new();

        public frmLoaiMon(bool isEmbedded = false)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();

            dgvDanhSachLoai.AutoGenerateColumns = false;
            colIDLoai.DataPropertyName = nameof(LoaiMonDTO.ID);
            colTenLoai.DataPropertyName = nameof(LoaiMonDTO.TenLoai);
            colSoMon.DataPropertyName = nameof(LoaiMonDTO.SoMon);

            Load += frmLoaiMon_Load;
            txtSearch.TextChanged += FilterChanged;
            txtTimLoai.TextChanged += FilterChanged;
            dgvDanhSachLoai.SelectionChanged += dgvDanhSachLoai_SelectionChanged;
        }

        private void frmLoaiMon_Load(object? sender, EventArgs e)
        {
            if (_isEmbedded)
            {
                panelSidebar.Visible = false;
                panelTopbar.Visible = false;
                panelMain.Dock = DockStyle.Fill;
            }

            LoadDanhSachLoai();
            ResetForm();
        }

        private void LoadDanhSachLoai()
        {
            var dsLoai = _loaiMonBUS.LayDanhSachLoai(txtSearch.Text, txtTimLoai.Text);
            dgvDanhSachLoai.DataSource = dsLoai;

            lblTongLoaiValue.Text = dsLoai.Count.ToString();
            lblDangSuDungValue.Text = dsLoai.Count(x => x.SoMon > 0).ToString();
            lblChuaSuDungValue.Text = dsLoai.Count(x => x.SoMon == 0).ToString();
        }

        private void btnThemLoai_Click(object sender, EventArgs e)
        {
            var result = _loaiMonBUS.ThemLoai(txtTenLoai.Text);
            if (!result.ThanhCong || result.LoaiMoi == null)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachLoai();
            SelectRow(result.LoaiMoi.ID);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCapNhatLoai_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLoai.Text, out var loaiId))
            {
                MessageBox.Show("Vui lòng chọn loại món cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _loaiMonBUS.CapNhatLoai(loaiId, txtTenLoai.Text);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachLoai();
            SelectRow(loaiId);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaLoai_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLoai.Text, out var loaiId))
            {
                MessageBox.Show("Vui lòng chọn loại món cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDanhSachLoai.CurrentRow?.DataBoundItem is not LoaiMonDTO loai)
            {
                MessageBox.Show("Không tìm thấy loại món để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa loại món '{loai.TenLoai}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            var result = _loaiMonBUS.XoaLoai(loaiId);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadDanhSachLoai();
            ResetForm();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            txtTimLoai.Clear();
            LoadDanhSachLoai();
            ResetForm();
        }

        private void btnNhapLoai_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                Title = "Nhập danh sách loại món"
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

            var result = _loaiMonBUS.NhapLoaiMonTuCsv(lines);
            LoadDanhSachLoai();
            ResetForm();

            MessageBox.Show(
                $"Nhập dữ liệu hoàn tất.\nThêm mới: {result.SoThemMoi}\nĐã có sẵn: {result.SoCapNhat}\nBỏ qua: {result.SoBoQua}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnXuatLoai_Click(object sender, EventArgs e)
        {
            if (dgvDanhSachLoai.DataSource is not List<LoaiMonDTO> dsLoai || dsLoai.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"DanhSachLoaiMon_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Xuất danh sách loại món"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var lines = new List<string>
            {
                "ID,TenLoai,SoMon"
            };

            lines.AddRange(dsLoai.Select(x => string.Join(",",
                x.ID,
                EscapeCsv(x.TenLoai),
                x.SoMon)));

            File.WriteAllLines(dialog.FileName, lines, Encoding.UTF8);
            MessageBox.Show("Xuất danh sách loại món thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FilterChanged(object? sender, EventArgs e)
        {
            LoadDanhSachLoai();
        }

        private void ResetForm()
        {
            txtTenLoai.Clear();
            txtMaLoai.Text = _loaiMonBUS.LayMaLoaiTiepTheo().ToString();
        }

        private void SelectRow(int id)
        {
            foreach (DataGridViewRow row in dgvDanhSachLoai.Rows)
            {
                if (row.DataBoundItem is LoaiMonDTO loai && loai.ID == id)
                {
                    row.Selected = true;
                    dgvDanhSachLoai.CurrentCell = row.Cells[0];
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

        private void dgvDanhSachLoai_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachLoai.CurrentRow?.DataBoundItem is not LoaiMonDTO loai)
            {
                return;
            }

            txtMaLoai.Text = loai.ID.ToString();
            txtTenLoai.Text = loai.TenLoai;
        }
    }
}
