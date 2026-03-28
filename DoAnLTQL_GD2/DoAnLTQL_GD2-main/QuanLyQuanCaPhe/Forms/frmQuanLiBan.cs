using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiBan : Form
    {
        private CaPheDbContext context = new CaPheDbContext();

        public frmQuanLiBan()
        {
            InitializeComponent();
            InitializePlaceholders();
            Load += frmQuanLiBan_Load;

            dgvDanhSachBan.AutoGenerateColumns = false;
            colMaBan.DataPropertyName = "MaBan";
            colTenBan.DataPropertyName = "TenBan";
            colKhuVuc.DataPropertyName = "KhuVuc";
            colSucChua.DataPropertyName = "SucChua";
            colTinhTrang.DataPropertyName = "TinhTrang";

            btnLamMoi.Click += btnLamMoi_Click;
            btnGopBan.Click += btnGopBan_Click;
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
            var tongBan = context.Ban.AsNoTracking().Count();
            var banDangPhucVu = context.Ban.AsNoTracking().Count(b => b.TrangThai == 1);
            var banTrong = context.Ban.AsNoTracking().Count(b => b.TrangThai == 0);
            var banDatTruoc = context.HoaDon
                .AsNoTracking()
                .Count(hd => hd.TrangThai == 0 && hd.GhiChuHoaDon != null && EF.Functions.Like(hd.GhiChuHoaDon, "%đặt trước%"));

            lblTongBanValue.Text = tongBan.ToString();
            lblDangPhucVuValue.Text = banDangPhucVu.ToString();
            lblBanTrongValue.Text = banTrong.ToString();
            lblDatTruocValue.Text = banDatTruoc.ToString();
        }

        private void LoadDanhSachBanLenGrid()
        {
            var khuVuc = cboKhuVuc.SelectedItem?.ToString();
            var trangThai = cboTrangThai.SelectedItem?.ToString();
            var tuKhoa = txtSearch.Text.Trim();

            var dsBan = context.Ban
                .AsNoTracking()
                .Where(b => string.IsNullOrWhiteSpace(khuVuc)
                    || khuVuc == "Tất cả khu vực"
                    || XacDinhKhuVuc(b.TenBan) == khuVuc)
                .Where(b => string.IsNullOrWhiteSpace(trangThai)
                    || trangThai == "Tất cả trạng thái"
                    || (trangThai == "Trống" && b.TrangThai == 0)
                    || (trangThai == "Đang phục vụ" && b.TrangThai == 1)
                    || (trangThai == "Đặt trước" && b.TrangThai == 2))
                .Where(b => string.IsNullOrWhiteSpace(tuKhoa)
                    || b.ID.ToString().Contains(tuKhoa)
                    || b.TenBan.Contains(tuKhoa)
                    || XacDinhKhuVuc(b.TenBan).Contains(tuKhoa)
                    || ChuyenTrangThaiBan(b.TrangThai).Contains(tuKhoa)
                    || (tuKhoa.Equals("trống", StringComparison.OrdinalIgnoreCase) && b.TrangThai == 0))
                .OrderBy(b => b.ID)
                .Select(b => new
                {
                    MaBan = b.ID,
                    TenBan = b.TenBan,
                    KhuVuc = XacDinhKhuVuc(b.TenBan),
                    SucChua = "4 người",
                    TinhTrang = ChuyenTrangThaiBan(b.TrangThai)
                })
                .ToList();

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

            var dsBan = context.Ban
                .AsNoTracking()
                .OrderBy(b => b.ID)
                .ToList();

            foreach (var ban in dsBan)
            {
                var trangThaiText = ChuyenTrangThaiBan(ban.TrangThai);

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
            if (sender is not Button { Tag: dtaBan ban })
            {
                return;
            }

            MessageBox.Show(
                $"Bạn đang chọn: {ban.TenBan}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private static string ChuyenTrangThaiBan(int trangThai)
        {
            return trangThai switch
            {
                0 => "Sẵn sàng",
                1 => "Đang phục vụ",
                2 => "Đặt trước",
                _ => "Sẵn sàng"
            };
        }

        private static string XacDinhKhuVuc(string tenBan)
        {
            if (string.IsNullOrWhiteSpace(tenBan))
            {
                return "-";
            }

            var kyTu = char.ToUpperInvariant(tenBan.Trim().FirstOrDefault(char.IsLetter));
            return kyTu switch
            {
                'A' or 'B' => "Khu trong nhà",
                'C' => "Khu sân vườn",
                'D' => "Khu phòng riêng",
                _ => "Khu khác"
            };
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            context.Dispose();
            base.OnFormClosed(e);
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
            if (string.IsNullOrWhiteSpace(tenBan))
            {
                MessageBox.Show("Tên bàn không được để trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var daTonTai = context.Ban.Any(x => x.TenBan == tenBan);
            if (daTonTai)
            {
                MessageBox.Show("Tên bàn đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            context.Ban.Add(new dtaBan
            {
                TenBan = tenBan,
                TrangThai = 0
            });

            context.SaveChanges();
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

            var hoaDonNguon = context.HoaDon
                .Include(h => h.HoaDon_ChiTiet)
                .FirstOrDefault(h => h.BanID == banNguon.ID && h.TrangThai == 0);

            if (hoaDonNguon == null)
            {
                MessageBox.Show("Bàn nguồn chưa có hóa đơn đang phục vụ để chuyển/gộp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dsBanDich = context.Ban
                .AsNoTracking()
                .Where(b => b.ID != banNguon.ID)
                .OrderBy(b => b.TenBan)
                .ToList();

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
                DisplayMember = nameof(dtaBan.TenBan),
                ValueMember = nameof(dtaBan.ID)
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

            var banDich = context.Ban.FirstOrDefault(b => b.ID == banDichId);
            if (banDich == null)
            {
                MessageBox.Show("Không tìm thấy bàn đích.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rdoChuyenBan.Checked)
            {
                if (banDich.TrangThai != 0)
                {
                    MessageBox.Show("Chỉ có thể chuyển sang bàn trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                hoaDonNguon.BanID = banDich.ID;
                banNguon.TrangThai = 0;
                banDich.TrangThai = 1;
                context.SaveChanges();
                RefreshView();
                return;
            }

            var hoaDonDich = context.HoaDon
                .Include(h => h.HoaDon_ChiTiet)
                .FirstOrDefault(h => h.BanID == banDich.ID && h.TrangThai == 0);

            if (hoaDonDich == null)
            {
                MessageBox.Show("Bàn đích chưa có hóa đơn đang phục vụ để gộp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var ctNguon in hoaDonNguon.HoaDon_ChiTiet.ToList())
            {
                var ctDich = hoaDonDich.HoaDon_ChiTiet.FirstOrDefault(x => x.MonID == ctNguon.MonID && x.DonGiaBan == ctNguon.DonGiaBan && x.GhiChu == ctNguon.GhiChu);
                if (ctDich == null)
                {
                    context.HoaDon_ChiTiet.Add(new dtHoaDon_ChiTiet
                    {
                        HoaDonID = hoaDonDich.ID,
                        MonID = ctNguon.MonID,
                        SoLuongBan = ctNguon.SoLuongBan,
                        DonGiaBan = ctNguon.DonGiaBan,
                        GhiChu = ctNguon.GhiChu
                    });
                }
                else
                {
                    ctDich.SoLuongBan = (short)Math.Clamp(ctDich.SoLuongBan + ctNguon.SoLuongBan, 0, short.MaxValue);
                }
            }

            context.HoaDon_ChiTiet.RemoveRange(hoaDonNguon.HoaDon_ChiTiet);
            hoaDonNguon.TrangThai = 1;
            hoaDonNguon.GhiChuHoaDon = $"{hoaDonNguon.GhiChuHoaDon} [Đã gộp vào HD{hoaDonDich.ID:D5}]".Trim();

            banNguon.TrangThai = 0;
            banDich.TrangThai = 1;

            context.SaveChanges();
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

            if (ban.TrangThai != 0)
            {
                MessageBox.Show("Bàn đang được sử dụng, không thể xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var daPhatSinhHoaDon = context.HoaDon.Any(x => x.BanID == ban.ID);
            if (daPhatSinhHoaDon)
            {
                MessageBox.Show("Bàn đã phát sinh hóa đơn, không thể xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            context.Ban.Remove(ban);
            context.SaveChanges();
            RefreshView();
        }

        private dtaBan? GetBanDangChon()
        {
            if (dgvDanhSachBan.CurrentRow?.Cells[colMaBan.Name].Value == null)
            {
                return null;
            }

            if (!int.TryParse(dgvDanhSachBan.CurrentRow.Cells[colMaBan.Name].Value.ToString(), out var banId))
            {
                return null;
            }

            return context.Ban.FirstOrDefault(x => x.ID == banId);
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
