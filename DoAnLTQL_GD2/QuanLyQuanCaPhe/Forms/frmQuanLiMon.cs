using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiMon : Form
    {
        private readonly CaPheDbContext db = new CaPheDbContext();

        public frmQuanLiMon()
        {
            InitializeComponent();
            Load += frmQuanLyMon_Load;

            dgvDanhSachMon.AutoGenerateColumns = false;
            colMaMon.DataPropertyName = "ID";
            colTenMon.DataPropertyName = "TenMon";
            colLoaiMon.DataPropertyName = "LoaiMon";
            colDonGia.DataPropertyName = "DonGia";
            colTrangThai.DataPropertyName = "TrangThai";
            colMoTa.DataPropertyName = "MoTa";

            btnThemMon.Click += btnThemMon_Click;
            btnCapNhatMon.Click += btnCapNhatMon_Click;
            btnXoaMon.Click += btnXoaMon_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnNhapMon.Click += btnNhapMon_Click;
            btnXuatMon.Click += btnXuatMon_Click;
            cboLoaiMon.SelectedIndexChanged += FilterControl_Changed;
            cboTrangThai.SelectedIndexChanged += FilterControl_Changed;
            txtTimMon.TextChanged += FilterControl_Changed;
            txtSearch.TextChanged += FilterControl_Changed;
        }

        private void frmQuanLyMon_Load(object? sender, EventArgs e)
        {
            LoadLoaiMonFilter();
            cboTrangThai.SelectedIndex = 0;

            LoadDanhSachMon();
        }

        private void LoadLoaiMonFilter()
        {
            var dsLoai = db.LoaiMon
                .OrderBy(x => x.TenLoai)
                .Select(x => new LoaiMonFilterOption
                {
                    ID = x.ID,
                    TenLoai = x.TenLoai
                })
                .ToList();

            dsLoai.Insert(0, new LoaiMonFilterOption { ID = null, TenLoai = "Tất cả loại" });
            cboLoaiMon.DataSource = dsLoai;
            cboLoaiMon.DisplayMember = nameof(LoaiMonFilterOption.TenLoai);
            cboLoaiMon.ValueMember = nameof(LoaiMonFilterOption.ID);
            cboLoaiMon.SelectedIndex = 0;
        }

        private void LoadDanhSachMon()
        {
            var selectedLoaiMonId = (cboLoaiMon.SelectedItem as LoaiMonFilterOption)?.ID;
            var trangThai = cboTrangThai.SelectedItem?.ToString();
            var tuKhoa = txtTimMon.Text.Trim();
            var topSearch = txtSearch.Text.Trim();

            var danhSachMon = db.Mon
                .Where(m => !selectedLoaiMonId.HasValue || m.LoaiMonID == selectedLoaiMonId.Value)
                .Where(m => string.IsNullOrWhiteSpace(tuKhoa)
                    || m.TenMon.Contains(tuKhoa)
                    || m.ID.ToString().Contains(tuKhoa)
                    || (m.MoTa != null && m.MoTa.Contains(tuKhoa)))
                .Where(m => string.IsNullOrWhiteSpace(topSearch)
                    || m.TenMon.Contains(topSearch)
                    || m.ID.ToString().Contains(topSearch)
                    || m.LoaiMon.TenLoai.Contains(topSearch))
                .OrderBy(m => m.ID)
                .Select(m => new
                {
                    ID = m.ID,
                    TenMon = m.TenMon,
                    LoaiMon = m.LoaiMon.TenLoai,
                    DonGia = string.Format("{0:N0}", m.DonGia),
                    TrangThai = "Đang kinh doanh",
                    MoTa = m.MoTa
                })
                .ToList();

            if (!string.IsNullOrWhiteSpace(trangThai)
                && !string.Equals(trangThai, "Tất cả trạng thái", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(trangThai, "Đang kinh doanh", StringComparison.OrdinalIgnoreCase))
            {
                danhSachMon.Clear();
            }

            dgvDanhSachMon.DataSource = danhSachMon;
            lblTongMonValue.Text = db.Mon.Count().ToString();
            lblDangKinhDoanhValue.Text = db.Mon.Count().ToString();
            lblHetMonValue.Text = "0";
            lblLoaiMonValue.Text = db.LoaiMon.Count().ToString();
        }

        private void FilterControl_Changed(object? sender, EventArgs e)
        {
            LoadDanhSachMon();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimMon.Clear();
            txtSearch.Clear();
            cboLoaiMon.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            LoadDanhSachMon();
        }

        private void btnNhapMon_Click(object? sender, EventArgs e)
        {
            LoadDanhSachMon();
            MessageBox.Show("Đã tải lại dữ liệu món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXuatMon_Click(object? sender, EventArgs e)
        {
            if (dgvDanhSachMon.DataSource is not IEnumerable<object> data)
            {
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"DanhSachMon_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var lines = new List<string> { "ID,TenMon,LoaiMon,DonGia,TrangThai,MoTa" };

            foreach (var item in data)
            {
                var type = item.GetType();
                var id = type.GetProperty("ID")?.GetValue(item)?.ToString() ?? string.Empty;
                var tenMon = type.GetProperty("TenMon")?.GetValue(item)?.ToString() ?? string.Empty;
                var loaiMon = type.GetProperty("LoaiMon")?.GetValue(item)?.ToString() ?? string.Empty;
                var donGia = type.GetProperty("DonGia")?.GetValue(item)?.ToString() ?? string.Empty;
                var trangThai = type.GetProperty("TrangThai")?.GetValue(item)?.ToString() ?? string.Empty;
                var moTa = type.GetProperty("MoTa")?.GetValue(item)?.ToString() ?? string.Empty;

                lines.Add($"{EscapeCsv(id)},{EscapeCsv(tenMon)},{EscapeCsv(loaiMon)},{EscapeCsv(donGia)},{EscapeCsv(trangThai)},{EscapeCsv(moTa)}");
            }

            File.WriteAllLines(dialog.FileName, lines);
            MessageBox.Show("Xuất danh sách món thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string EscapeCsv(string value)
        {
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }

            return value;
        }

        private void btnThemMon_Click(object? sender, EventArgs e)
        {
            var giaTri = ShowMonEditor();
            if (giaTri == null)
            {
                return;
            }

            var mon = new dtaMon
            {
                TenMon = giaTri.TenMon,
                DonGia = giaTri.DonGia,
                LoaiMonID = giaTri.LoaiMonID,
                MoTa = giaTri.MoTa
            };

            db.Mon.Add(mon);
            db.SaveChanges();
            LoadDanhSachMon();
        }

        private void btnCapNhatMon_Click(object? sender, EventArgs e)
        {
            var mon = GetMonDangChon();
            if (mon == null)
            {
                MessageBox.Show("Vui lòng chọn món cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var giaTri = ShowMonEditor(mon);
            if (giaTri == null)
            {
                return;
            }

            mon.TenMon = giaTri.TenMon;
            mon.DonGia = giaTri.DonGia;
            mon.LoaiMonID = giaTri.LoaiMonID;
            mon.MoTa = giaTri.MoTa;

            db.SaveChanges();
            LoadDanhSachMon();
        }

        private void btnXoaMon_Click(object? sender, EventArgs e)
        {
            var mon = GetMonDangChon();
            if (mon == null)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var daPhatSinhHoaDon = db.HoaDon_ChiTiet.Any(x => x.MonID == mon.ID);
            if (daPhatSinhHoaDon)
            {
                MessageBox.Show("Món đã phát sinh trong hóa đơn, không thể xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa món '{mon.TenMon}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            db.Mon.Remove(mon);
            db.SaveChanges();
            LoadDanhSachMon();
        }

        private dtaMon? GetMonDangChon()
        {
            if (dgvDanhSachMon.CurrentRow?.Cells[colMaMon.Name].Value == null)
            {
                return null;
            }

            if (!int.TryParse(dgvDanhSachMon.CurrentRow.Cells[colMaMon.Name].Value.ToString(), out var monId))
            {
                return null;
            }

            return db.Mon.FirstOrDefault(x => x.ID == monId);
        }

        private MonEditorValue? ShowMonEditor(dtaMon? mon = null)
        {
            using var dialog = new Form
            {
                Text = mon == null ? "Thêm món" : "Cập nhật món",
                StartPosition = FormStartPosition.CenterParent,
                Width = 420,
                Height = 320,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblTenMon = new Label { Left = 20, Top = 20, Width = 90, Text = "Tên món" };
            var txtTenMon = new TextBox { Left = 120, Top = 18, Width = 250, Text = mon?.TenMon ?? string.Empty };
            var lblLoaiMon = new Label { Left = 20, Top = 62, Width = 90, Text = "Loại món" };
            var cboLoai = new ComboBox { Left = 120, Top = 58, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblDonGia = new Label { Left = 20, Top = 104, Width = 90, Text = "Đơn giá" };
            var txtDonGia = new TextBox { Left = 120, Top = 100, Width = 250, Text = mon?.DonGia.ToString() ?? string.Empty };
            var lblMoTa = new Label { Left = 20, Top = 146, Width = 90, Text = "Mô tả" };
            var txtMoTa = new TextBox
            {
                Left = 120,
                Top = 142,
                Width = 250,
                Height = 70,
                Multiline = true,
                Text = mon?.MoTa ?? string.Empty
            };

            var btnLuu = new Button
            {
                Left = 214,
                Top = 225,
                Width = 75,
                Text = "Lưu",
                DialogResult = DialogResult.OK
            };

            var btnHuy = new Button
            {
                Left = 295,
                Top = 225,
                Width = 75,
                Text = "Hủy",
                DialogResult = DialogResult.Cancel
            };

            var dsLoai = db.LoaiMon.OrderBy(x => x.TenLoai).ToList();
            cboLoai.DataSource = dsLoai;
            cboLoai.DisplayMember = nameof(dtaLoaiMon.TenLoai);
            cboLoai.ValueMember = nameof(dtaLoaiMon.ID);

            if (mon != null)
            {
                cboLoai.SelectedValue = mon.LoaiMonID;
            }

            dialog.Controls.AddRange(new Control[]
            {
                lblTenMon, txtTenMon, lblLoaiMon, cboLoai, lblDonGia, txtDonGia, lblMoTa, txtMoTa, btnLuu, btnHuy
            });

            dialog.AcceptButton = btnLuu;
            dialog.CancelButton = btnHuy;

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Tên món không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (!int.TryParse(txtDonGia.Text.Trim(), out var donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (cboLoai.SelectedValue is not int loaiMonId)
            {
                MessageBox.Show("Vui lòng chọn loại món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return new MonEditorValue
            {
                TenMon = txtTenMon.Text.Trim(),
                LoaiMonID = loaiMonId,
                DonGia = donGia,
                MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim()
            };
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }

        private sealed class LoaiMonFilterOption
        {
            public int? ID { get; set; }
            public string TenLoai { get; set; } = string.Empty;
        }

        private sealed class MonEditorValue
        {
            public int LoaiMonID { get; set; }
            public string TenMon { get; set; } = string.Empty;
            public int DonGia { get; set; }
            public string? MoTa { get; set; }
        }

    }
}
