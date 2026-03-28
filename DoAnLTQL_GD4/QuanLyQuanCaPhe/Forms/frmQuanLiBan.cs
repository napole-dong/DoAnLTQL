using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiBan : Form
    {
        private readonly bool _isEmbedded;
        private readonly BanBUS _banBUS = new();

        public frmQuanLiBan(bool isEmbedded = false)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();
            InitializePlaceholders();
            Load += frmQuanLiBan_Load;

            dgvDanhSachBan.AutoGenerateColumns = false;
            colMaBan.DataPropertyName = nameof(BanDTO.ID);
            colTenBan.DataPropertyName = nameof(BanDTO.TenBan);
            colKhuVuc.DataPropertyName = nameof(BanDTO.KhuVuc);
            colSucChua.DataPropertyName = nameof(BanDTO.SucChua);
            colTinhTrang.DataPropertyName = nameof(BanDTO.TinhTrang);

            cboKhuVuc.SelectedIndexChanged += FilterControl_Changed;
            cboTrangThai.SelectedIndexChanged += FilterControl_Changed;
            txtSearch.TextChanged += FilterControl_Changed;
            dgvDanhSachBan.KeyDown += dgvDanhSachBan_KeyDown;
        }

        private void InitializePlaceholders()
        {
            lblTongBanValue.Text = string.Empty;
            lblDangPhucVuValue.Text = string.Empty;
            lblBanTrongValue.Text = string.Empty;
            lblDatTruocValue.Text = string.Empty;

            dgvDanhSachBan.DataSource = null;
            dgvDanhSachBan.Rows.Clear();
        }

        private void frmQuanLiBan_Load(object? sender, EventArgs e)
        {
            if (_isEmbedded)
            {
                panelSidebar.Visible = false;
                panelTopbar.Visible = false;
                panelMain.Dock = DockStyle.Fill;
            }

            try
            {
                LoadThongKe();
                LoadSoDoBanDong();
                LoadDanhSachBanLenGrid();

                if (cboKhuVuc.Items.Count > 0)
                {
                    cboKhuVuc.SelectedIndex = 0;
                }

                if (cboTrangThai.Items.Count > 0)
                {
                    cboTrangThai.SelectedIndex = 0;
                }
            }
            catch
            {
                lblTongBanValue.Text = "0";
                lblDangPhucVuValue.Text = "0";
                lblBanTrongValue.Text = "0";
                lblDatTruocValue.Text = "0";
                dgvDanhSachBan.Rows.Clear();
            }
        }

        private void LoadThongKe()
        {
            var thongKe = _banBUS.LayThongKe();
            lblTongBanValue.Text = thongKe.TongBan.ToString();
            lblDangPhucVuValue.Text = thongKe.BanDangPhucVu.ToString();
            lblBanTrongValue.Text = thongKe.BanTrong.ToString();
            lblDatTruocValue.Text = thongKe.BanDatTruoc.ToString();
        }

        private void LoadDanhSachBanLenGrid()
        {
            var khuVuc = cboKhuVuc.SelectedItem?.ToString();
            var trangThai = cboTrangThai.SelectedItem?.ToString();
            var tuKhoa = txtSearch.Text.Trim();

            var dsBan = _banBUS.LayDanhSachBan(khuVuc, trangThai, tuKhoa);

            dgvDanhSachBan.DataSource = dsBan;
        }

        private void FilterControl_Changed(object? sender, EventArgs e)
        {
            LoadDanhSachBanLenGrid();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();

            if (cboKhuVuc.Items.Count > 0)
            {
                cboKhuVuc.SelectedIndex = 0;
            }

            if (cboTrangThai.Items.Count > 0)
            {
                cboTrangThai.SelectedIndex = 0;
            }

            RefreshView();
        }

        private void LoadSoDoBanDong()
        {
            flowBanSoDo.Controls.Clear();

            var dsBan = _banBUS.LaySoDoBan();

            foreach (var ban in dsBan)
            {
                var trangThaiText = ban.TinhTrang;

                var btnBan = new Button
                {
                    Width = 120,
                    Height = 80,
                    Margin = new Padding(10),
                    Text = $"{ban.TenBan}\n\n{trangThaiText}",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Tag = ban,
                    TextAlign = ContentAlignment.MiddleCenter,
                    UseVisualStyleBackColor = false
                };

                btnBan.FlatAppearance.BorderSize = 0;

                switch (trangThaiText)
                {
                    case "Sẵn sàng":
                        btnBan.BackColor = Color.LightCyan;
                        btnBan.ForeColor = Color.DarkCyan;
                        break;
                    case "Đang phục vụ":
                        btnBan.BackColor = Color.LightGoldenrodYellow;
                        btnBan.ForeColor = Color.DarkOrange;
                        break;
                    case "Đặt trước":
                        btnBan.BackColor = Color.MistyRose;
                        btnBan.ForeColor = Color.DarkRed;
                        break;
                }

                btnBan.Click += BtnBan_Click;
                flowBanSoDo.Controls.Add(btnBan);
            }
        }

        private void BtnBan_Click(object? sender, EventArgs e)
        {
            if (sender is not Button { Tag: BanDTO ban })
            {
                return;
            }

            MessageBox.Show(
                $"Bạn đang chọn: {ban.TenBan}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnThemBan_Click(object sender, EventArgs e)
        {
            using var dialog = new Form
            {
                Text = "Thêm bàn mới",
                StartPosition = FormStartPosition.CenterParent,
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblTenBan = new Label { Left = 20, Top = 24, Width = 80, Text = "Tên bàn" };
            var txtTenBan = new TextBox { Left = 105, Top = 20, Width = 255 };
            var btnLuu = new Button { Left = 204, Top = 70, Width = 75, Text = "Lưu", DialogResult = DialogResult.OK };
            var btnHuy = new Button { Left = 285, Top = 70, Width = 75, Text = "Hủy", DialogResult = DialogResult.Cancel };

            dialog.Controls.AddRange(new Control[] { lblTenBan, txtTenBan, btnLuu, btnHuy });
            dialog.AcceptButton = btnLuu;
            dialog.CancelButton = btnHuy;

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var tenBan = txtTenBan.Text.Trim();
            var result = _banBUS.ThemBan(tenBan);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshView();
        }

        private void btnGopBan_Click(object? sender, EventArgs e)
        {
            var banNguon = GetBanDangChon();
            if (banNguon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn nguồn trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dsBanDich = _banBUS.LayDanhSachBanDich(banNguon.ID);

            if (dsBanDich.Count == 0)
            {
                MessageBox.Show("Không có bàn đích khả dụng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new Form
            {
                Text = "Chuyển / Gộp bàn",
                StartPosition = FormStartPosition.CenterParent,
                Width = 430,
                Height = 210,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblBanDich = new Label { Left = 20, Top = 22, Width = 80, Text = "Bàn đích" };
            var cboBanDich = new ComboBox
            {
                Left = 105,
                Top = 18,
                Width = 290,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = dsBanDich,
                DisplayMember = nameof(BanDTO.TenBan),
                ValueMember = nameof(BanDTO.ID)
            };

            var rdoChuyenBan = new RadioButton { Left = 105, Top = 58, Width = 120, Text = "Chuyển bàn", Checked = true };
            var rdoGopBan = new RadioButton { Left = 230, Top = 58, Width = 120, Text = "Gộp bàn" };
            var btnThucHien = new Button { Left = 239, Top = 108, Width = 75, Text = "Lưu", DialogResult = DialogResult.OK };
            var btnHuy = new Button { Left = 320, Top = 108, Width = 75, Text = "Hủy", DialogResult = DialogResult.Cancel };

            dialog.Controls.AddRange(new Control[] { lblBanDich, cboBanDich, rdoChuyenBan, rdoGopBan, btnThucHien, btnHuy });
            dialog.AcceptButton = btnThucHien;
            dialog.CancelButton = btnHuy;

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (cboBanDich.SelectedValue is not int banDichId)
            {
                MessageBox.Show("Vui lòng chọn bàn đích.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _banBUS.ChuyenHoacGopBan(new BanChuyenGopRequestDTO
            {
                BanNguonId = banNguon.ID,
                BanDichId = banDichId,
                LaChuyenBan = rdoChuyenBan.Checked
            });
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshView();
        }

        private void btnXoaBan_Click(object? sender, EventArgs e)
        {
            var ban = GetBanDangChon();
            if (ban == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa bàn '{ban.TenBan}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            var result = _banBUS.XoaBan(ban.ID);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshView();
        }

        private BanDTO? GetBanDangChon()
        {
            return dgvDanhSachBan.CurrentRow?.DataBoundItem as BanDTO;
        }

        private void dgvDanhSachBan_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
            {
                return;
            }

            btnXoaBan_Click(sender, e);
            e.Handled = true;
        }

        private void RefreshView()
        {
            LoadThongKe();
            LoadSoDoBanDong();
            LoadDanhSachBanLenGrid();
        }

    }
}
