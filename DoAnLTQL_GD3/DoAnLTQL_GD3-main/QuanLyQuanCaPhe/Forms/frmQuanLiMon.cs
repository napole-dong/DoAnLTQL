using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiMon : Form
    {
        private readonly bool _isEmbedded;
        private readonly CaPheDbContext _context = new();
        private string? _selectedImagePath;

        public frmQuanLiMon(bool isEmbedded = false)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();

            dgvDanhSachMon.AutoGenerateColumns = false;
            colIDMon.DataPropertyName = nameof(MonGridItem.ID);
            colTenMon.DataPropertyName = nameof(MonGridItem.TenMon);
            colLoaiMon.DataPropertyName = nameof(MonGridItem.TenLoaiMon);
            colDonGia.DataPropertyName = nameof(MonGridItem.DonGiaHienThi);
            colMoTa.DataPropertyName = nameof(MonGridItem.MoTa);

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
            var dsLoai = _context.LoaiMon
                .AsNoTracking()
                .OrderBy(x => x.TenLoai)
                .ToList();

            cboLoaiMon.DataSource = dsLoai;
            cboLoaiMon.DisplayMember = nameof(dtaLoaiMon.TenLoai);
            cboLoaiMon.ValueMember = nameof(dtaLoaiMon.ID);
            cboLoaiMon.SelectedIndex = dsLoai.Count > 0 ? 0 : -1;
        }

        private void LoadDanhSachMon()
        {
            var tuKhoa = ($"{txtSearch.Text} {txtTimMon.Text}").Trim();

            var query = _context.Mon
                .AsNoTracking()
                .Include(x => x.LoaiMon)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                query = query.Where(x =>
                    x.ID.ToString().Contains(tuKhoa)
                    || x.TenMon.Contains(tuKhoa)
                    || x.LoaiMon.TenLoai.Contains(tuKhoa)
                    || (x.MoTa ?? string.Empty).Contains(tuKhoa));
            }

            var dsMon = query
                .OrderBy(x => x.ID)
                .Select(x => new MonGridItem
                {
                    ID = x.ID,
                    TenMon = x.TenMon,
                    LoaiMonID = x.LoaiMonID,
                    TenLoaiMon = x.LoaiMon.TenLoai,
                    DonGia = x.DonGia,
                    MoTa = x.MoTa ?? string.Empty,
                    HinhAnh = x.HinhAnh
                })
                .ToList();

            dgvDanhSachMon.DataSource = dsMon;

            lblTongMonValue.Text = dsMon.Count.ToString();
            lblLoaiMonValue.Text = dsMon.Select(x => x.LoaiMonID).Distinct().Count().ToString();
            lblGiaTrungBinhValue.Text = dsMon.Count == 0
                ? "0đ"
                : $"{Math.Round(dsMon.Average(x => x.DonGia), 0):N0}đ";

            txtMaMon.Text = ((_context.Mon.Max(x => (int?)x.ID) ?? 0) + 1).ToString();
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
            if (!ValidateInput(out var donGia, out var loaiMonId))
            {
                return;
            }

            var mon = new dtaMon
            {
                TenMon = txtTenMon.Text.Trim(),
                LoaiMonID = loaiMonId,
                DonGia = donGia,
                MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim(),
                HinhAnh = string.IsNullOrWhiteSpace(txtDuongDanAnh.Text) ? null : txtDuongDanAnh.Text.Trim()
            };

            _context.Mon.Add(mon);
            _context.SaveChanges();

            LoadDanhSachMon();
            SelectRow(mon.ID);
            MessageBox.Show("Thêm món thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCapNhatMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaMon.Text, out var id))
            {
                MessageBox.Show("Vui lòng chọn món cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out var donGia, out var loaiMonId))
            {
                return;
            }

            var mon = _context.Mon.FirstOrDefault(x => x.ID == id);
            if (mon == null)
            {
                MessageBox.Show("Không tìm thấy món để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            mon.TenMon = txtTenMon.Text.Trim();
            mon.LoaiMonID = loaiMonId;
            mon.DonGia = donGia;
            mon.MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim();
            mon.HinhAnh = string.IsNullOrWhiteSpace(txtDuongDanAnh.Text) ? null : txtDuongDanAnh.Text.Trim();

            _context.SaveChanges();
            LoadDanhSachMon();
            SelectRow(mon.ID);
            MessageBox.Show("Cập nhật món thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaMon.Text, out var id))
            {
                MessageBox.Show("Vui lòng chọn món cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_context.HoaDon_ChiTiet.Any(x => x.MonID == id))
            {
                MessageBox.Show("Món đã phát sinh hóa đơn, không thể xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var mon = _context.Mon.FirstOrDefault(x => x.ID == id);
            if (mon == null)
            {
                MessageBox.Show("Không tìm thấy món để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa món '{mon.TenMon}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            _context.Mon.Remove(mon);
            _context.SaveChanges();
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
            if (dgvDanhSachMon.DataSource is not List<MonGridItem> dsMon || dsMon.Count == 0)
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

            var lines = new List<string>
            {
                "ID,TenMon,LoaiMonID,TenLoaiMon,DonGia,MoTa,HinhAnh"
            };

            lines.AddRange(dsMon.Select(x => string.Join(",",
                x.ID,
                EscapeCsv(x.TenMon),
                x.LoaiMonID,
                EscapeCsv(x.TenLoaiMon),
                x.DonGia,
                EscapeCsv(x.MoTa),
                EscapeCsv(x.HinhAnh ?? string.Empty))));

            File.WriteAllLines(dialog.FileName, lines, Encoding.UTF8);
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

            var lines = File.ReadAllLines(dialog.FileName, Encoding.UTF8)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (lines.Count == 0)
            {
                MessageBox.Show("Tệp nhập không có dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var startIndex = lines[0].Contains("TenMon", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
            var dsLoai = _context.LoaiMon.AsNoTracking().ToList();
            var soThemMoi = 0;
            var soCapNhat = 0;
            var soBoQua = 0;

            for (var i = startIndex; i < lines.Count; i++)
            {
                var cot = SplitCsvLine(lines[i]);
                if (cot.Count < 3)
                {
                    soBoQua++;
                    continue;
                }

                string tenMon;
                string loaiMonText;
                string donGiaText;
                string moTa;
                string hinhAnh;

                if (cot.Count >= 7)
                {
                    tenMon = cot[1].Trim();
                    loaiMonText = cot[2].Trim();
                    donGiaText = cot[4].Trim();
                    moTa = cot[5].Trim();
                    hinhAnh = cot[6].Trim();
                }
                else
                {
                    tenMon = cot[0].Trim();
                    loaiMonText = cot[1].Trim();
                    donGiaText = cot[2].Trim();
                    moTa = cot.Count > 3 ? cot[3].Trim() : string.Empty;
                    hinhAnh = cot.Count > 4 ? cot[4].Trim() : string.Empty;
                }

                if (string.IsNullOrWhiteSpace(tenMon) || !int.TryParse(donGiaText, out var donGia) || donGia < 0)
                {
                    soBoQua++;
                    continue;
                }

                var loaiMonId = 0;
                if (int.TryParse(loaiMonText, out var loaiId))
                {
                    loaiMonId = loaiId;
                }
                else
                {
                    loaiMonId = dsLoai.FirstOrDefault(x =>
                        string.Equals(x.TenLoai, loaiMonText, StringComparison.OrdinalIgnoreCase))?.ID ?? 0;
                }

                if (!dsLoai.Any(x => x.ID == loaiMonId))
                {
                    soBoQua++;
                    continue;
                }

                var mon = _context.Mon.FirstOrDefault(x => x.TenMon == tenMon && x.LoaiMonID == loaiMonId);
                if (mon == null)
                {
                    _context.Mon.Add(new dtaMon
                    {
                        TenMon = tenMon,
                        LoaiMonID = loaiMonId,
                        DonGia = donGia,
                        MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa,
                        HinhAnh = string.IsNullOrWhiteSpace(hinhAnh) ? null : hinhAnh
                    });
                    soThemMoi++;
                }
                else
                {
                    mon.DonGia = donGia;
                    mon.MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa;
                    mon.HinhAnh = string.IsNullOrWhiteSpace(hinhAnh) ? null : hinhAnh;
                    soCapNhat++;
                }
            }

            _context.SaveChanges();
            LoadDanhSachMon();
            ResetForm();

            MessageBox.Show(
                $"Nhập dữ liệu hoàn tất.\nThêm mới: {soThemMoi}\nCập nhật: {soCapNhat}\nBỏ qua: {soBoQua}",
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

        private bool ValidateInput(out int donGia, out int loaiMonId)
        {
            donGia = 0;
            loaiMonId = 0;

            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Tên món không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenMon.Focus();
                return false;
            }

            if (cboLoaiMon.SelectedValue is not int loaiId)
            {
                MessageBox.Show("Vui lòng chọn loại món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtDonGia.Text.Trim(), out donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGia.Focus();
                return false;
            }

            loaiMonId = loaiId;
            return true;
        }

        private void SelectRow(int id)
        {
            foreach (DataGridViewRow row in dgvDanhSachMon.Rows)
            {
                if (row.DataBoundItem is MonGridItem mon && mon.ID == id)
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
            txtMaMon.Text = ((_context.Mon.Max(x => (int?)x.ID) ?? 0) + 1).ToString();
        }

        private static string EscapeCsv(string value)
        {
            if (!value.Contains(',') && !value.Contains('"') && !value.Contains('\n') && !value.Contains('\r'))
            {
                return value;
            }

            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        private static List<string> SplitCsvLine(string line)
        {
            var result = new List<string>();
            var current = new StringBuilder();
            var inQuotes = false;

            foreach (var c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                    continue;
                }

                current.Append(c);
            }

            result.Add(current.ToString());
            return result;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _context.Dispose();
            base.OnFormClosed(e);
        }

        private sealed class MonGridItem
        {
            public int ID { get; set; }
            public int LoaiMonID { get; set; }
            public string TenMon { get; set; } = string.Empty;
            public string TenLoaiMon { get; set; } = string.Empty;
            public int DonGia { get; set; }
            public string DonGiaHienThi => $"{DonGia:N0}đ";
            public string MoTa { get; set; } = string.Empty;
            public string? HinhAnh { get; set; }
        }

        private void dgvDanhSachMon_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachMon.CurrentRow?.DataBoundItem is not MonGridItem mon)
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
