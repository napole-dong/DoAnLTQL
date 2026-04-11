using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Navigation;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmQuanLiBan : Form
    {
        private readonly bool _isEmbedded;
        private readonly BanBUS _banBUS = new();
        private readonly PermissionBUS _permissionBUS = new();
        private int? _banDangChonId;
        private bool _dangDongBoChonTrenGrid;
        private bool _dangXuLyChuyenGopBan;

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
            btnLamMoi.Click += btnLamMoi_Click;
            dgvDanhSachBan.KeyDown += dgvDanhSachBan_KeyDown;
            dgvDanhSachBan.SelectionChanged += dgvDanhSachBan_SelectionChanged;

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), onCurrentFormReactivated: RefreshView, skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), onCurrentFormReactivated: RefreshView, skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), onCurrentFormReactivated: RefreshView, skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), onCurrentFormReactivated: RefreshView, skipNavigation: _isEmbedded);
            btnKhachHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), onCurrentFormReactivated: RefreshView, skipNavigation: _isEmbedded);
            btnNhanVien.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NhanVien, () => new frmNhanVien(isEmbedded: true), onCurrentFormReactivated: RefreshView, skipNavigation: _isEmbedded);
            btnHoaDon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.HoaDon, () => new frmHoaDon(isEmbedded: true), onCurrentFormReactivated: RefreshView, skipNavigation: _isEmbedded);
            btnThongKe.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmThongKe(isEmbedded: true), onCurrentFormReactivated: RefreshView, skipNavigation: _isEmbedded);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
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
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Quan ly ban"))
            {
                return;
            }

            try
            {
                _permissionBUS.EnsurePermission(PermissionFeatures.Menu, PermissionActions.View, "Bạn không có quyền truy cập chức năng Quản lý bàn.");
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
            ApDungPhanQuyenDieuHuong();

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

        private void HienThiNguoiDungDangNhap()
        {
        }

        private void ApDungPhanQuyenDieuHuong()
        {
            var coQuyenMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
            var coQuyenKhachHang = _permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.View);
            btnBanHang.Visible = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View);
            btnQuanLyBan.Visible = coQuyenMenu;
            btnQuanLyMon.Visible = coQuyenMenu;
            btnCongThuc.Visible = coQuyenMenu;
            btnKhachHang.Visible = coQuyenKhachHang;
            btnHoaDon.Visible = _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);
            btnNhanVien.Visible = _permissionBUS.CanManageEmployees();
            btnThongKe.Visible = _permissionBUS.CanViewReport();
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
            var tuKhoa = string.Empty;

            var dsBan = _banBUS.LayDanhSachBan(khuVuc, trangThai, tuKhoa);

            _banDangChonId ??= dsBan.FirstOrDefault()?.ID;

            dgvDanhSachBan.DataSource = dsBan;
            ChonBanTrenGrid(_banDangChonId);
        }

        private void FilterControl_Changed(object? sender, EventArgs e)
        {
            LoadDanhSachBanLenGrid();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            RefreshView();
        }

        private void LoadSoDoBanDong()
        {
            flowBanSoDo.Controls.Clear();

            var dsBan = _banBUS.LaySoDoBan();

            if (_banDangChonId.HasValue && dsBan.All(x => x.ID != _banDangChonId.Value))
            {
                _banDangChonId = dsBan.FirstOrDefault()?.ID;
            }

            _banDangChonId ??= dsBan.FirstOrDefault()?.ID;

            foreach (var ban in dsBan)
            {
                var dangChon = _banDangChonId == ban.ID;
                var trangThaiText = ban.TinhTrang;

                var btnBan = new Button
                {
                    Width = 120,
                    Height = 80,
                    Margin = new Padding(10),
                    Text = $"{ban.TenBan}\n\n{(dangChon ? "Đang chọn" : trangThaiText)}",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Tag = ban,
                    TextAlign = ContentAlignment.MiddleCenter,
                    UseVisualStyleBackColor = false
                };

                btnBan.FlatAppearance.BorderSize = dangChon ? 2 : 0;
                btnBan.FlatAppearance.BorderColor = Color.FromArgb(121, 85, 72);

                switch (trangThaiText)
                {
                    case "Trống":
                        btnBan.BackColor = Color.FromArgb(230, 246, 236);
                        btnBan.ForeColor = Color.FromArgb(33, 112, 57);
                        break;
                    case "Có khách":
                        btnBan.BackColor = Color.FromArgb(255, 236, 232);
                        btnBan.ForeColor = Color.FromArgb(156, 43, 30);
                        break;
                    case "Chờ dọn / Đã thanh toán":
                        btnBan.BackColor = Color.FromArgb(255, 246, 214);
                        btnBan.ForeColor = Color.FromArgb(148, 108, 0);
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

            _banDangChonId = ban.ID;
            ChonBanTrenGrid(_banDangChonId);
            LoadSoDoBanDong();
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

        private async void btnGopBan_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyChuyenGopBan)
            {
                return;
            }

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
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            var rdoChuyenBan = new RadioButton { Left = 105, Top = 58, Width = 120, Text = "Chuyển bàn", Checked = true };
            var rdoGopBan = new RadioButton { Left = 230, Top = 58, Width = 120, Text = "Gộp bàn" };
            var btnThucHien = new Button { Left = 239, Top = 108, Width = 75, Text = "Lưu", DialogResult = DialogResult.OK };
            var btnHuy = new Button { Left = 320, Top = 108, Width = 75, Text = "Hủy", DialogResult = DialogResult.Cancel };

            void CapNhatDanhSachBanDich(bool laChuyenBan)
            {
                var danhSachLoc = dsBanDich
                    .Where(x => laChuyenBan ? x.TrangThai == 0 : x.TrangThai == 1)
                    .ToList();

                cboBanDich.DataSource = danhSachLoc;
                cboBanDich.DisplayMember = nameof(BanDTO.TenBan);
                cboBanDich.ValueMember = nameof(BanDTO.ID);

                cboBanDich.Enabled = danhSachLoc.Count > 0;
                btnThucHien.Enabled = danhSachLoc.Count > 0;
            }

            rdoChuyenBan.CheckedChanged += (_, _) =>
            {
                if (rdoChuyenBan.Checked)
                {
                    CapNhatDanhSachBanDich(laChuyenBan: true);
                }
            };

            rdoGopBan.CheckedChanged += (_, _) =>
            {
                if (rdoGopBan.Checked)
                {
                    CapNhatDanhSachBanDich(laChuyenBan: false);
                }
            };

            CapNhatDanhSachBanDich(laChuyenBan: true);

            dialog.Controls.AddRange(new Control[] { lblBanDich, cboBanDich, rdoChuyenBan, rdoGopBan, btnThucHien, btnHuy });
            dialog.AcceptButton = btnThucHien;
            dialog.CancelButton = btnHuy;

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (cboBanDich.SelectedValue is not int banDichId)
            {
                MessageBox.Show(
                    rdoChuyenBan.Checked
                        ? "Không có bàn trống để chuyển."
                        : "Không có bàn đang phục vụ để gộp.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var request = new BanChuyenGopRequestDTO
            {
                BanNguonId = banNguon.ID,
                BanDichId = banDichId,
                LaChuyenBan = rdoChuyenBan.Checked
            };

            _dangXuLyChuyenGopBan = true;
            CapNhatTrangThaiDangXuLyBan(true);

            BanActionResultDTO result;
            try
            {
                result = await Task.Run(() => _banBUS.ChuyenHoacGopBan(request));
            }
            finally
            {
                _dangXuLyChuyenGopBan = false;
                CapNhatTrangThaiDangXuLyBan(false);
            }

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
            if (_banDangChonId.HasValue)
            {
                var banDangChon = TimBanTheoId(_banDangChonId.Value);
                if (banDangChon != null)
                {
                    return banDangChon;
                }
            }

            return dgvDanhSachBan.CurrentRow?.DataBoundItem as BanDTO;
        }

        private BanDTO? TimBanTheoId(int banId)
        {
            foreach (DataGridViewRow row in dgvDanhSachBan.Rows)
            {
                if (row.DataBoundItem is BanDTO ban && ban.ID == banId)
                {
                    return ban;
                }
            }

            return _banBUS.LaySoDoBan().FirstOrDefault(x => x.ID == banId);
        }

        private void ChonBanTrenGrid(int? banId)
        {
            _dangDongBoChonTrenGrid = true;
            try
            {
                foreach (DataGridViewRow row in dgvDanhSachBan.Rows)
                {
                    row.Selected = false;
                }

                if (!banId.HasValue)
                {
                    if (dgvDanhSachBan.Rows.Count == 0)
                    {
                        return;
                    }

                    dgvDanhSachBan.CurrentCell = null;
                    return;
                }

                foreach (DataGridViewRow row in dgvDanhSachBan.Rows)
                {
                    if (row.DataBoundItem is not BanDTO ban || ban.ID != banId.Value)
                    {
                        continue;
                    }

                    row.Selected = true;
                    if (row.Cells.Count > 0)
                    {
                        dgvDanhSachBan.CurrentCell = row.Cells[0];
                    }

                    return;
                }

                if (dgvDanhSachBan.Rows.Count > 0)
                {
                    dgvDanhSachBan.CurrentCell = null;
                }
            }
            finally
            {
                _dangDongBoChonTrenGrid = false;
            }
        }

        private void dgvDanhSachBan_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dangDongBoChonTrenGrid || dgvDanhSachBan.CurrentRow?.DataBoundItem is not BanDTO banDangChon)
            {
                return;
            }

            if (_banDangChonId == banDangChon.ID)
            {
                return;
            }

            _banDangChonId = banDangChon.ID;
            LoadSoDoBanDong();
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

        private void CapNhatTrangThaiDangXuLyBan(bool dangXuLy)
        {
            UseWaitCursor = dangXuLy;

            btnThemBan.Enabled = !dangXuLy;
            btnGopBan.Enabled = !dangXuLy;
            btnXoaBan.Enabled = !dangXuLy;
            btnLamMoi.Enabled = !dangXuLy;

            cboKhuVuc.Enabled = !dangXuLy;
            cboTrangThai.Enabled = !dangXuLy;
            dgvDanhSachBan.Enabled = !dangXuLy;
            flowBanSoDo.Enabled = !dangXuLy;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }

    }
}
