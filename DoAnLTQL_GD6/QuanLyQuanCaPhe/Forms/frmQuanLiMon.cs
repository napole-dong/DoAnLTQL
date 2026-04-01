using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private readonly MonBUS _monBUS = new();
        private readonly LoaiMonBUS _loaiMonBUS = new();
        private readonly MonInputValidator _monInputValidator = new();
        private readonly MonCsvService _monCsvService = new();

        public frmQuanLiMon()
        {
            InitializeComponent();

            dgvDanhSachMon.AutoGenerateColumns = false;
            colMaMon.DataPropertyName = nameof(MonDTO.ID);
            colTenMon.DataPropertyName = nameof(MonDTO.TenMon);
            colLoaiMon.DataPropertyName = nameof(MonDTO.TenLoaiMon);
            colDonGia.DataPropertyName = nameof(MonDTO.DonGiaHienThi);
            colTrangThai.DataPropertyName = nameof(MonDTO.TrangThai);
            colMoTaMon.DataPropertyName = nameof(MonDTO.MoTa);

            dgvDanhSachLoaiMon.AutoGenerateColumns = false;
            colMaLoaiMon.DataPropertyName = nameof(LoaiMonDTO.ID);
            colTenLoaiMon.DataPropertyName = nameof(LoaiMonDTO.TenLoai);
            colSoLuongMon.DataPropertyName = nameof(LoaiMonDTO.SoMon);
            colMoTaLoaiMon.DataPropertyName = nameof(LoaiMonDTO.MoTa);

            Load += FrmQuanLiMon_Load;
            SizeChanged += FrmQuanLiMon_SizeChanged;

            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
            tabDanhSach.SelectedIndexChanged += tabDanhSach_SelectedIndexChanged;

            dgvDanhSachMon.SelectionChanged += dgvDanhSachMon_SelectionChanged;
            dgvDanhSachLoaiMon.SelectionChanged += dgvDanhSachLoaiMon_SelectionChanged;

            btnThemMon.Click += btnThemMon_Click;
            btnCapNhatMon.Click += btnCapNhatMon_Click;
            btnXoaMon.Click += btnXoaMon_Click;
            btnThemLoaiMon.Click += btnThemLoaiMon_Click;
            btnCapNhatLoaiMon.Click += btnCapNhatLoaiMon_Click;
            btnXoaLoaiMon.Click += btnXoaLoaiMon_Click;
            btnNhap.Click += btnNhap_Click;
            btnXuat.Click += btnXuat_Click;
        }

        private void FrmQuanLiMon_Load(object? sender, EventArgs e)
        {
            ApplyRoundedUi();
            LoadLoaiMonComboBox();
            RefreshAllData();
            ResetFormMon();
            ResetFormLoai();
        }

        private void FrmQuanLiMon_SizeChanged(object? sender, EventArgs e)
        {
            ApplyRoundedUi();
        }

        private void ApplyRoundedUi()
        {
            ApplyRoundRegion(panelThongTin, 14);
            ApplyRoundRegion(panelDanhSach, 14);

            ApplyRoundRegion(cardTongMon, 14);
            ApplyRoundRegion(cardDangBan, 14);
            ApplyRoundRegion(cardTongLoai, 14);
            ApplyRoundRegion(cardNgungBan, 14);

            ApplyRoundRegion(btnThemMon, 8);
            ApplyRoundRegion(btnCapNhatMon, 8);
            ApplyRoundRegion(btnXoaMon, 8);
            ApplyRoundRegion(btnThemLoaiMon, 8);
            ApplyRoundRegion(btnCapNhatLoaiMon, 8);
            ApplyRoundRegion(btnXoaLoaiMon, 8);
            ApplyRoundRegion(btnNhap, 8);
            ApplyRoundRegion(btnXuat, 8);
        }

        private void LoadLoaiMonComboBox()
        {
            var dsLoaiMon = _monBUS.LayDanhSachLoaiMon();
            cboLoaiMon.DataSource = dsLoaiMon;
            cboLoaiMon.DisplayMember = nameof(LoaiMonDTO.TenLoai);
            cboLoaiMon.ValueMember = nameof(LoaiMonDTO.ID);
            cboLoaiMon.SelectedIndex = dsLoaiMon.Count > 0 ? 0 : -1;

            if (cboTrangThai.Items.Count > 0)
            {
                cboTrangThai.SelectedIndex = 0;
            }
        }

        private void RefreshAllData()
        {
            var tuKhoa = txtTimKiem.Text.Trim();
            var dsMon = _monBUS.LayDanhSachMon(tuKhoa, null);
            var dsLoai = _loaiMonBUS.LayDanhSachLoai(tuKhoa, null);

            dgvDanhSachMon.DataSource = dsMon;
            dgvDanhSachLoaiMon.DataSource = dsLoai;

            lblTongMonValue.Text = dsMon.Count.ToString();
            lblDangBanValue.Text = dsMon.Count(x => x.TrangThai.Equals("Đang kinh doanh", StringComparison.OrdinalIgnoreCase)).ToString();
            lblTongLoaiValue.Text = dsLoai.Count.ToString();
            lblNgungBanValue.Text = dsMon.Count(x => x.TrangThai.Equals("Ngừng bán", StringComparison.OrdinalIgnoreCase)).ToString();

            txtMaMon.Text = _monBUS.LayMaMonTiepTheo().ToString();
            txtMaLoai.Text = _loaiMonBUS.LayMaLoaiTiepTheo().ToString();
        }

        private void txtTimKiem_TextChanged(object? sender, EventArgs e)
        {
            RefreshAllData();
        }

        private void tabDanhSach_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabDanhSach.SelectedTab == tabMon)
            {
                ResetFormLoai();
                return;
            }

            ResetFormMon();
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

            RefreshAllData();
            SelectMonRow(result.MonMoi.ID);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCapNhatMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaMon.Text, out var maMon))
            {
                MessageBox.Show("Vui lòng chọn món cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryGetValidatedMon(out var monDTO))
            {
                return;
            }

            monDTO.ID = maMon;
            var result = _monBUS.CapNhatMon(monDTO);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshAllData();
            SelectMonRow(maMon);
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaMon.Text, out var maMon))
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

            var result = _monBUS.XoaMon(maMon);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshAllData();
            ResetFormMon();
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnThemLoaiMon_Click(object? sender, EventArgs e)
        {
            var result = _loaiMonBUS.ThemLoai(txtTenLoai.Text, txtMoTaLoai.Text);
            if (!result.ThanhCong || result.LoaiMoi == null)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshAllData();
            SelectLoaiRow(result.LoaiMoi.ID);
            LoadLoaiMonComboBox();
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCapNhatLoaiMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLoai.Text, out var maLoai))
            {
                MessageBox.Show("Vui lòng chọn loại món cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _loaiMonBUS.CapNhatLoai(maLoai, txtTenLoai.Text, txtMoTaLoai.Text);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshAllData();
            SelectLoaiRow(maLoai);
            LoadLoaiMonComboBox();
            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaLoaiMon_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLoai.Text, out var maLoai))
            {
                MessageBox.Show("Vui lòng chọn loại món cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDanhSachLoaiMon.CurrentRow?.DataBoundItem is not LoaiMonDTO loai)
            {
                MessageBox.Show("Không tìm thấy loại món để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa loại món '{loai.TenLoai}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            var result = _loaiMonBUS.XoaLoai(maLoai);
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
                        var loaiDichId = ChonLoaiMonDich(maLoai);
                        if (!loaiDichId.HasValue)
                        {
                            return;
                        }

                        var chuyenResult = _loaiMonBUS.ChuyenMonSangLoaiKhac(maLoai, loaiDichId.Value);
                        if (!chuyenResult.ThanhCong)
                        {
                            MessageBox.Show(chuyenResult.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var xoaSauKhiChuyen = _loaiMonBUS.XoaLoai(maLoai);
                        if (!xoaSauKhiChuyen.ThanhCong)
                        {
                            MessageBox.Show(xoaSauKhiChuyen.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        RefreshAllData();
                        LoadLoaiMonComboBox();
                        SelectLoaiRow(loaiDichId.Value);
                        MessageBox.Show("Đã chuyển món sang loại mới và xóa loại món cũ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshAllData();
            ResetFormLoai();
            LoadLoaiMonComboBox();
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

        private void btnNhap_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                Title = tabDanhSach.SelectedTab == tabMon ? "Nhập danh sách món" : "Nhập danh sách loại món"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var lines = File.ReadAllLines(dialog.FileName, Encoding.UTF8)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (lines.Length == 0)
            {
                MessageBox.Show("Tệp nhập không có dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tabDanhSach.SelectedTab == tabMon)
            {
                var resultMon = _monBUS.NhapMonTuCsv(lines);
                RefreshAllData();
                ResetFormMon();

                MessageBox.Show(
                    $"Nhập dữ liệu món hoàn tất.\nThêm mới: {resultMon.SoThemMoi}\nCập nhật: {resultMon.SoCapNhat}\nBỏ qua: {resultMon.SoBoQua}",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var resultLoai = _loaiMonBUS.NhapLoaiMonTuCsv(lines);
            RefreshAllData();
            ResetFormLoai();
            LoadLoaiMonComboBox();

            MessageBox.Show(
                $"Nhập dữ liệu loại món hoàn tất.\nThêm mới: {resultLoai.SoThemMoi}\nCập nhật: {resultLoai.SoCapNhat}\nBỏ qua: {resultLoai.SoBoQua}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnXuat_Click(object? sender, EventArgs e)
        {
            if (tabDanhSach.SelectedTab == tabMon)
            {
                if (dgvDanhSachMon.DataSource is not List<MonDTO> dsMon || dsMon.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu món để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using var dialogMon = new SaveFileDialog
                {
                    Filter = "CSV (*.csv)|*.csv",
                    FileName = $"DanhSachMon_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                    Title = "Xuất danh sách món"
                };

                if (dialogMon.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                _monCsvService.XuatCsv(dialogMon.FileName, dsMon);
                MessageBox.Show("Xuất danh sách món thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvDanhSachLoaiMon.DataSource is not List<LoaiMonDTO> dsLoai || dsLoai.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu loại món để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialogLoai = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"DanhSachLoaiMon_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Xuất danh sách loại món"
            };

            if (dialogLoai.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var lines = new List<string>
            {
                "ID,TenLoai,SoMon,MoTa"
            };

            lines.AddRange(dsLoai.Select(x => string.Join(",",
                x.ID,
                EscapeCsv(x.TenLoai),
                x.SoMon,
                EscapeCsv(x.MoTa))));

            File.WriteAllLines(dialogLoai.FileName, lines, Encoding.UTF8);
            MessageBox.Show("Xuất danh sách loại món thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool TryGetValidatedMon(out MonDTO monDTO)
        {
            var validation = _monInputValidator.Validate(
                txtTenMon.Text,
                cboLoaiMon.SelectedValue,
                cboTrangThai.SelectedItem,
                txtDonGia.Text,
                string.Empty,
                string.Empty);

            if (validation.HopLe && validation.Mon != null)
            {
                monDTO = validation.Mon;
                return true;
            }

            monDTO = new MonDTO();
            MessageBox.Show(validation.ThongBao ?? "Dữ liệu không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void dgvDanhSachMon_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachMon.CurrentRow?.DataBoundItem is not MonDTO mon)
            {
                return;
            }

            txtMaMon.Text = mon.ID.ToString();
            txtTenMon.Text = mon.TenMon;
            txtDonGia.Text = mon.DonGia.ToString();
            cboLoaiMon.SelectedValue = mon.LoaiMonID;

            var trangThaiIndex = cboTrangThai.FindStringExact(mon.TrangThai);
            cboTrangThai.SelectedIndex = trangThaiIndex >= 0 ? trangThaiIndex : 0;
        }

        private void dgvDanhSachLoaiMon_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDanhSachLoaiMon.CurrentRow?.DataBoundItem is not LoaiMonDTO loaiMon)
            {
                return;
            }

            txtMaLoai.Text = loaiMon.ID.ToString();
            txtTenLoai.Text = loaiMon.TenLoai;
            txtMoTaLoai.Text = loaiMon.MoTa;
        }

        private void SelectMonRow(int maMon)
        {
            foreach (DataGridViewRow row in dgvDanhSachMon.Rows)
            {
                if (row.DataBoundItem is not MonDTO mon || mon.ID != maMon)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachMon.CurrentCell = row.Cells[0];
                break;
            }
        }

        private void SelectLoaiRow(int maLoai)
        {
            foreach (DataGridViewRow row in dgvDanhSachLoaiMon.Rows)
            {
                if (row.DataBoundItem is not LoaiMonDTO loaiMon || loaiMon.ID != maLoai)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachLoaiMon.CurrentCell = row.Cells[0];
                break;
            }
        }

        private void ResetFormMon()
        {
            txtMaMon.Text = _monBUS.LayMaMonTiepTheo().ToString();
            txtTenMon.Clear();
            txtDonGia.Text = "0";

            if (cboLoaiMon.Items.Count > 0)
            {
                cboLoaiMon.SelectedIndex = 0;
            }

            if (cboTrangThai.Items.Count > 0)
            {
                cboTrangThai.SelectedIndex = 0;
            }
        }

        private void ResetFormLoai()
        {
            txtMaLoai.Text = _loaiMonBUS.LayMaLoaiTiepTheo().ToString();
            txtTenLoai.Clear();
            txtMoTaLoai.Clear();
        }

        private static string EscapeCsv(string value)
        {
            if (!value.Contains(',') && !value.Contains('"') && !value.Contains('\n') && !value.Contains('\r'))
            {
                return value;
            }

            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        private static void ApplyRoundRegion(Control? control, int radius)
        {
            if (control == null || control.Width <= 0 || control.Height <= 0)
            {
                return;
            }

            using var path = new GraphicsPath();
            var rect = new Rectangle(0, 0, control.Width, control.Height);
            var diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            control.Region = new Region(path);
        }
    }
}
