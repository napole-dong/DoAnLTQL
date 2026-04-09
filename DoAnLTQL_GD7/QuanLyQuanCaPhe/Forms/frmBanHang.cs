using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using System.Reflection;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmBanHang : Form
    {
        private const string MenuItemCardTag = "menu-card";
        private readonly BanBUS _banBUS = new();
        private readonly MonBUS _monBUS = new();
        private readonly BanHangBUS _banHangBUS = new();
        private readonly PermissionBUS _permissionBUS = new();
        private int? _banDangChonId;
        private string? _boLocLoaiMon;
        private readonly FlowLayoutPanel _flowBoLocLoaiMon = new();
        private readonly List<Button> _cacNutBoLocLoaiMon = new();
        private Button? _nutTatCaLoaiMon;

        public frmBanHang()
        {
            InitializeComponent();

            dgvOrder.AutoGenerateColumns = false;
            colTenMon.DataPropertyName = nameof(BanHangOrderItemDTO.TenMon);
            colSoLuong.DataPropertyName = nameof(BanHangOrderItemDTO.SoLuong);
            colDonGia.DataPropertyName = nameof(BanHangOrderItemDTO.DonGiaHienThi);
            colThanhTien.DataPropertyName = nameof(BanHangOrderItemDTO.ThanhTienHienThi);

            Load += frmBanHang_Load;
            txtSearch.TextChanged += txtSearch_TextChanged;
            btnLamMoi.Click += btnLamMoi_Click;
            KhoiTaoKhungBoLocLoaiMon();
            ToiUuGiaoDienMenu();

            btnTamTinh.Click += btnTamTinh_Click;
            btnThanhToan.Click += btnThanhToan_Click;
            btnChuyenBan.Click += (_, _) => ThucHienChuyenHoacGopBan(true);
            btnGopBan.Click += (_, _) => ThucHienChuyenHoacGopBan(false);

            btnQuanLyBan.Click += (_, _) => OpenManagementForm(new frmQuanLiBan(), PermissionFeatures.Menu);
            btnQuanLyMon.Click += (_, _) => OpenManagementForm(new frmQuanLiMon(), PermissionFeatures.Menu);
            btnKhachHang.Click += (_, _) => OpenManagementForm(new frmKhachHang(), PermissionFeatures.Menu);
            btnNhanVien.Click += (_, _) => OpenManagementForm(new frmNhanVien(), PermissionFeatures.NhanVien);
            btnThongKe.Click += (_, _) => MoTinhNangThongKe();
            btnHoaDon.Click += (_, _) => OpenManagementForm(new frmHoaDon(), PermissionFeatures.HoaDon);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void ToiUuGiaoDienMenu()
        {
            panelMenu.Padding = new Padding(14, 12, 14, 14);
            panelMenuFilter.Padding = new Padding(0, 0, 0, 6);

            flowMon.AutoScroll = true;
            flowMon.FlowDirection = FlowDirection.LeftToRight;
            flowMon.WrapContents = true;
            flowMon.Padding = new Padding(6);
            flowMon.Margin = new Padding(0);
            flowMon.Resize += (_, _) => CapNhatKichThuocTheMonDangHienThi();

            flowBan.Padding = new Padding(6);

            BatDoubleBuffer(flowMon);
            BatDoubleBuffer(flowBan);
        }

        private void frmBanHang_Load(object? sender, EventArgs e)
        {
            try
            {
                _permissionBUS.EnsurePermission(PermissionFeatures.BanHang, PermissionActions.View, "Bạn không có quyền truy cập chức năng Bán hàng.");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            HienThiNguoiDungDangNhap();
            ApplyPermissionUI();

            TaiBoLocLoaiMonTuDuLieu();
            if (_nutTatCaLoaiMon != null)
            {
                ChonBoLocLoaiMon(null, _nutTatCaLoaiMon);
            }
            else
            {
                _boLocLoaiMon = null;
                TaiDanhSachMon();
            }

            TaiSoDoBan();
            CapNhatPhieuDangChon();
        }

        private void KhoiTaoKhungBoLocLoaiMon()
        {
            btnTatCa.Visible = false;
            btnCafe.Visible = false;
            btnDaXay.Visible = false;
            btnTra.Visible = false;

            _flowBoLocLoaiMon.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _flowBoLocLoaiMon.AutoScroll = false;
            _flowBoLocLoaiMon.BackColor = Color.Transparent;
            _flowBoLocLoaiMon.FlowDirection = FlowDirection.LeftToRight;
            _flowBoLocLoaiMon.Margin = new Padding(0);
            _flowBoLocLoaiMon.Name = "flowBoLocLoaiMonRuntime";
            _flowBoLocLoaiMon.Size = new Size(120, 32);
            _flowBoLocLoaiMon.TabIndex = 5;
            _flowBoLocLoaiMon.WrapContents = true;

            panelMenuFilter.Controls.Add(_flowBoLocLoaiMon);
            _flowBoLocLoaiMon.BringToFront();

            panelMenuFilter.Resize += (_, _) => CapNhatLayoutBoLocLoaiMon();
            CapNhatLayoutBoLocLoaiMon();
        }

        private void TaiBoLocLoaiMonTuDuLieu()
        {
            var dsTenLoaiMon = _monBUS.LayDanhSachLoaiMon()
                .Select(x => x.TenLoai.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            _flowBoLocLoaiMon.SuspendLayout();
            _flowBoLocLoaiMon.Controls.Clear();
            _cacNutBoLocLoaiMon.Clear();

            _nutTatCaLoaiMon = TaoNutBoLocLoaiMon("Tất cả");
            _nutTatCaLoaiMon.Click += (_, _) => ChonBoLocLoaiMon(null, _nutTatCaLoaiMon);
            _flowBoLocLoaiMon.Controls.Add(_nutTatCaLoaiMon);
            _cacNutBoLocLoaiMon.Add(_nutTatCaLoaiMon);

            foreach (var tenLoaiMon in dsTenLoaiMon)
            {
                var giaTriBoLoc = tenLoaiMon;
                var nutLoaiMon = TaoNutBoLocLoaiMon(tenLoaiMon);
                nutLoaiMon.Click += (_, _) => ChonBoLocLoaiMon(giaTriBoLoc, nutLoaiMon);
                _flowBoLocLoaiMon.Controls.Add(nutLoaiMon);
                _cacNutBoLocLoaiMon.Add(nutLoaiMon);
            }

            _flowBoLocLoaiMon.ResumeLayout();
            CapNhatLayoutBoLocLoaiMon();
        }

        private void CapNhatLayoutBoLocLoaiMon()
        {
            var leTrai = lblMenuTitle.Right + 14;
            var leTren = 14;
            const int lePhai = 6;
            const int leDuoi = 8;
            const int chieuCaoToiThieu = 32;

            if (panelMenuFilter.ClientSize.Width < 360)
            {
                leTrai = 4;
                leTren = lblMenuTitle.Bottom + 8;
            }

            var chieuRongKhaDung = Math.Max(120, panelMenuFilter.ClientSize.Width - leTrai - lePhai);
            _flowBoLocLoaiMon.Location = new Point(leTrai, leTren);
            _flowBoLocLoaiMon.Width = chieuRongKhaDung;

            var chieuCaoNoiDung = _flowBoLocLoaiMon.GetPreferredSize(new Size(chieuRongKhaDung, 0)).Height;
            _flowBoLocLoaiMon.Height = Math.Max(chieuCaoToiThieu, chieuCaoNoiDung);

            var chieuCaoPanel = Math.Max(74, leTren + _flowBoLocLoaiMon.Height + leDuoi);
            if (panelMenuFilter.Height != chieuCaoPanel)
            {
                panelMenuFilter.Height = chieuCaoPanel;
            }
        }

        private static Button TaoNutBoLocLoaiMon(string text)
        {
            var width = Math.Clamp(TextRenderer.MeasureText(text, new Font("Segoe UI", 9F, FontStyle.Bold)).Width + 24, 78, 170);
            var btn = new Button
            {
                BackColor = Color.FromArgb(248, 245, 241),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(79, 56, 43),
                Margin = new Padding(6, 0, 0, 0),
                Size = new Size(width, 32),
                Text = text,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };

            btn.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btn.FlatAppearance.BorderSize = 1;
            return btn;
        }

        private void HienThiNguoiDungDangNhap()
        {
        }

        private void ApplyPermissionUI()
        {
            var isAdmin = _permissionBUS.IsAdmin();
            var isManager = _permissionBUS.IsManager();
            btnQuanLyBan.Visible = isAdmin || isManager;
            btnQuanLyMon.Visible = isAdmin || isManager;
            btnKhachHang.Visible = isAdmin || isManager;
            btnNhanVien.Visible = isAdmin || isManager;
            btnHoaDon.Visible = isAdmin || isManager;
            btnThongKe.Visible = isAdmin || _permissionBUS.CanViewReport();

            var coQuyenBanHang = _permissionBUS.CanSell();
            btnTamTinh.Visible = coQuyenBanHang;
            btnThanhToan.Visible = coQuyenBanHang;
            btnChuyenBan.Visible = coQuyenBanHang;
            btnGopBan.Visible = coQuyenBanHang;
        }

        private void txtSearch_TextChanged(object? sender, EventArgs e)
        {
            TaiSoDoBan();
            TaiDanhSachMon();
            CapNhatPhieuDangChon();
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();

            if (_nutTatCaLoaiMon != null)
            {
                ChonBoLocLoaiMon(null, _nutTatCaLoaiMon);
            }
            else
            {
                _boLocLoaiMon = null;
                TaiDanhSachMon();
            }

            TaiSoDoBan();
            CapNhatPhieuDangChon();
        }

        private void TaiSoDoBan()
        {
            var tuKhoa = txtSearch.Text.Trim();
            var dsBan = _banBUS.LayDanhSachBan(null, null, tuKhoa);
            if (!string.IsNullOrWhiteSpace(tuKhoa) && dsBan.Count == 0)
            {
                dsBan = _banBUS.LayDanhSachBan(null, null, null);
            }

            if (_banDangChonId.HasValue && dsBan.All(x => x.ID != _banDangChonId.Value))
            {
                _banDangChonId = dsBan.FirstOrDefault()?.ID;
            }

            _banDangChonId ??= dsBan.FirstOrDefault()?.ID;

            flowBan.SuspendLayout();
            flowBan.Controls.Clear();

            foreach (var ban in dsBan)
            {
                flowBan.Controls.Add(TaoNutBan(ban));
            }

            if (dsBan.Count == 0)
            {
                flowBan.Controls.Add(TaoLabelRong("Chưa có dữ liệu bàn."));
            }

            flowBan.ResumeLayout();
        }

        private Button TaoNutBan(BanDTO ban)
        {
            var dangChon = _banDangChonId == ban.ID;
            var trangThai = BanHangBUS.ChuyenTrangThaiBan(ban.TrangThai);

            var btn = new Button
            {
                Width = 95,
                Height = 80,
                Margin = new Padding(8),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", dangChon ? 10F : 9.5F, dangChon ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = Color.FromArgb(79, 56, 43),
                BackColor = dangChon
                    ? Color.FromArgb(255, 241, 230)
                    : ban.TrangThai == 1
                        ? Color.FromArgb(255, 248, 234)
                        : Color.FromArgb(248, 245, 241),
                Text = $"{ban.TenBan}\r\n{(dangChon ? "Đang chọn" : trangThai)}",
                Tag = ban.ID,
                UseVisualStyleBackColor = false
            };

            btn.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btn.Click += BtnBan_Click;
            return btn;
        }

        private void BtnBan_Click(object? sender, EventArgs e)
        {
            if (sender is not Button { Tag: int banId })
            {
                return;
            }

            _banDangChonId = banId;
            TaiSoDoBan();
            CapNhatPhieuDangChon();
        }

        private void TaiDanhSachMon()
        {
            var tuKhoa = txtSearch.Text.Trim();
            var dsMon = _banHangBUS.LocMonPhuHopBanHang(_monBUS.LayDanhSachMon(tuKhoa, null), _boLocLoaiMon);
            var chieuRongTheMon = TinhChieuRongTheMon();

            if (!string.IsNullOrWhiteSpace(tuKhoa) && dsMon.Count == 0)
            {
                dsMon = _banHangBUS.LocMonPhuHopBanHang(_monBUS.LayDanhSachMon(null, null), _boLocLoaiMon);
            }

            flowMon.SuspendLayout();
            flowMon.Controls.Clear();

            foreach (var mon in dsMon)
            {
                flowMon.Controls.Add(TaoTheMon(mon, chieuRongTheMon));
            }

            if (dsMon.Count == 0)
            {
                flowMon.Controls.Add(TaoLabelRong("Không có món phù hợp."));
            }

            flowMon.ResumeLayout();
        }

        private int TinhChieuRongTheMon()
        {
            var chieuRongKhaDung = Math.Max(260, flowMon.ClientSize.Width - flowMon.Padding.Horizontal - 18);
            var soCot = chieuRongKhaDung >= 720 ? 3 : chieuRongKhaDung >= 380 ? 2 : 1;
            var chieuRongMoiCot = (chieuRongKhaDung / soCot) - 12;
            return Math.Clamp(chieuRongMoiCot, 176, 240);
        }

        private void CapNhatKichThuocTheMonDangHienThi()
        {
            if (flowMon.Controls.Count == 0)
            {
                return;
            }

            var chieuRongTheMon = TinhChieuRongTheMon();
            flowMon.SuspendLayout();

            foreach (Control control in flowMon.Controls)
            {
                if (control is Panel panelMon && Equals(panelMon.Tag, MenuItemCardTag))
                {
                    panelMon.Width = chieuRongTheMon;
                }
            }

            flowMon.ResumeLayout();
        }

        private Control TaoTheMon(MonDTO mon, int chieuRongTheMon)
        {
            var mauNen = Color.FromArgb(252, 248, 243);
            var mauHover = Color.FromArgb(255, 252, 247);

            var panel = new Panel
            {
                BackColor = mauNen,
                Size = new Size(chieuRongTheMon, 134),
                Margin = new Padding(6),
                Tag = MenuItemCardTag
            };

            var lblTen = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(63, 45, 35),
                Location = new Point(14, 10),
                Size = new Size(chieuRongTheMon - 28, 40),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Text = mon.TenMon
            };

            var lblLoaiMon = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Font = new Font("Segoe UI", 8.4F),
                ForeColor = Color.FromArgb(136, 115, 98),
                Location = new Point(14, 52),
                Size = new Size(chieuRongTheMon - 28, 18),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Text = mon.TenLoaiMon
            };

            var lblGia = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(119, 63, 27),
                Location = new Point(14, 99),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Text = $"{mon.DonGia:N0}đ"
            };

            var btnThem = new Button
            {
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(79, 56, 43),
                Location = new Point(chieuRongTheMon - 100, 92),
                Size = new Size(86, 34),
                Text = "+ Thêm",
                Tag = mon,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                UseVisualStyleBackColor = false
            };

            btnThem.FlatAppearance.BorderColor = Color.FromArgb(220, 208, 194);
            btnThem.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 246, 232);
            btnThem.FlatAppearance.MouseDownBackColor = Color.FromArgb(250, 233, 212);
            btnThem.Click += BtnThemMon_Click;

            void BatHieuUngHover(object? _, EventArgs __) => panel.BackColor = mauHover;
            void TatHieuUngHover(object? _, EventArgs __) => panel.BackColor = mauNen;

            panel.MouseEnter += BatHieuUngHover;
            panel.MouseLeave += TatHieuUngHover;
            lblTen.MouseEnter += BatHieuUngHover;
            lblTen.MouseLeave += TatHieuUngHover;
            lblLoaiMon.MouseEnter += BatHieuUngHover;
            lblLoaiMon.MouseLeave += TatHieuUngHover;
            lblGia.MouseEnter += BatHieuUngHover;
            lblGia.MouseLeave += TatHieuUngHover;

            panel.Paint += (_, e) =>
            {
                var rect = panel.ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;
                ControlPaint.DrawBorder(e.Graphics, rect, Color.FromArgb(232, 222, 212), ButtonBorderStyle.Solid);
            };

            panel.Controls.Add(lblTen);
            panel.Controls.Add(lblLoaiMon);
            panel.Controls.Add(lblGia);
            panel.Controls.Add(btnThem);
            return panel;
        }

        private static void BatDoubleBuffer(Control control)
        {
            var doubleBufferedProperty = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferedProperty?.SetValue(control, true, null);
        }

        private static Label TaoLabelRong(string text)
        {
            return new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(130, 112, 96),
                Font = new Font("Segoe UI", 9.5F),
                Margin = new Padding(8),
                Text = text
            };
        }

        private void BtnThemMon_Click(object? sender, EventArgs e)
        {
            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thêm món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sender is not Button { Tag: MonDTO mon })
            {
                return;
            }

            var result = _banHangBUS.ThemMonVaoGioTam(_banDangChonId.Value, mon);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CapNhatPhieuDangChon();
        }

        private void CapNhatPhieuDangChon()
        {
            if (!_banDangChonId.HasValue)
            {
                dgvOrder.DataSource = null;
                lblOrderMeta.Text = "Chưa chọn bàn";
                lblThongTinBan.Text = "Chọn bàn để xem món đang phục vụ";
                lblTamTinhValue.Text = DinhDangTien(0);
                lblGiamGiaValue.Text = DinhDangTien(0);
                lblTongThanhToanValue.Text = DinhDangTien(0);
                return;
            }

            var banId = _banDangChonId.Value;
            var trangThaiPhieu = _banHangBUS.LayTrangThaiPhieuTheoBan(banId);

            dgvOrder.DataSource = null;
            dgvOrder.DataSource = trangThaiPhieu.ChiTietHienThi;

            var tenBan = trangThaiPhieu.TenBan;
            var trangThaiBan = BanHangBUS.ChuyenTrangThaiBan(trangThaiPhieu.TrangThaiBan);

            lblOrderMeta.Text = trangThaiPhieu.SoMonChoGoi > 0
                ? $"{tenBan} • {trangThaiBan} • {trangThaiPhieu.SoMonChoGoi} món chờ gọi"
                : $"{tenBan} • {trangThaiBan}";

            lblThongTinBan.Text = trangThaiPhieu.TongMon > 0
                ? $"{tenBan} đang có {trangThaiPhieu.TongMon} món phục vụ"
                : "Chọn bàn để xem món đang phục vụ";

            lblTamTinhValue.Text = DinhDangTien(trangThaiPhieu.TongTien);
            lblGiamGiaValue.Text = DinhDangTien(0);
            lblTongThanhToanValue.Text = DinhDangTien(trangThaiPhieu.TongTien);
        }

        private bool LuuMonChoGoiVaLamMoi(int banId, bool hienThongBaoThanhCong = false, string? thongBaoThanhCong = null)
        {
            var result = _banHangBUS.LuuMonChoGoi(banId);

            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            TaiSoDoBan();
            CapNhatPhieuDangChon();

            if (hienThongBaoThanhCong)
            {
                MessageBox.Show(
                    thongBaoThanhCong ?? result.ThongBao,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            return true;
        }

        private void btnTamTinh_Click(object? sender, EventArgs e)
        {
            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi lưu bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var banId = _banDangChonId.Value;
            _ = LuuMonChoGoiVaLamMoi(
                banId,
                hienThongBaoThanhCong: true,
                thongBaoThanhCong: "Đã lưu bàn thành công. Bạn có thể mở lại để thêm món sau.");
        }

        private void btnThanhToan_Click(object? sender, EventArgs e)
        {
            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var banId = _banDangChonId.Value;
            var xacNhanThanhToan = MessageBox.Show(
                "Xác nhận thanh toán bàn đang chọn?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhanThanhToan != DialogResult.Yes)
            {
                return;
            }

            var result = _banHangBUS.ThanhToanHoaDon(banId);
            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiSoDoBan();
            CapNhatPhieuDangChon();

            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ThucHienChuyenHoacGopBan(bool laChuyenBan)
        {
            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn nguồn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var banNguonId = _banDangChonId.Value;
            if (_banHangBUS.CoMonChoGoiTrongGioTam(banNguonId))
            {
                MessageBox.Show("Bàn nguồn còn món chờ gọi. Vui lòng gọi món trước khi chuyển/gộp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dsBanDich = _banBUS
                .LayDanhSachBanDich(banNguonId)
                .Where(x => laChuyenBan ? x.TrangThai == 0 : x.TrangThai == 1)
                .ToList();

            if (dsBanDich.Count == 0)
            {
                MessageBox.Show(
                    laChuyenBan ? "Không có bàn trống để chuyển." : "Không có bàn đang phục vụ để gộp.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new Form
            {
                Text = laChuyenBan ? "Chuyển bàn" : "Gộp bàn",
                StartPosition = FormStartPosition.CenterParent,
                Width = 390,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblBanDich = new Label { Left = 16, Top = 22, Width = 90, Text = "Bàn đích" };
            var cboBanDich = new ComboBox
            {
                Left = 106,
                Top = 18,
                Width = 252,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = dsBanDich,
                DisplayMember = nameof(BanDTO.TenBan),
                ValueMember = nameof(BanDTO.ID)
            };

            var btnLuu = new Button
            {
                Left = 202,
                Top = 78,
                Width = 75,
                Text = "Lưu",
                DialogResult = DialogResult.OK
            };

            var btnHuy = new Button
            {
                Left = 283,
                Top = 78,
                Width = 75,
                Text = "Hủy",
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.AddRange(new Control[] { lblBanDich, cboBanDich, btnLuu, btnHuy });
            dialog.AcceptButton = btnLuu;
            dialog.CancelButton = btnHuy;

            if (dialog.ShowDialog(this) != DialogResult.OK || cboBanDich.SelectedValue is not int banDichId)
            {
                return;
            }

            var result = _banBUS.ChuyenHoacGopBan(new BanChuyenGopRequestDTO
            {
                BanNguonId = banNguonId,
                BanDichId = banDichId,
                LaChuyenBan = laChuyenBan
            });

            if (!result.ThanhCong)
            {
                MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _banDangChonId = banDichId;
            TaiSoDoBan();
            CapNhatPhieuDangChon();

            MessageBox.Show(result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ChonBoLocLoaiMon(string? boLoc, Button nutDuocChon)
        {
            _boLocLoaiMon = boLoc;
            ToMauNutLocMon(nutDuocChon);
            TaiDanhSachMon();
        }

        private void ToMauNutLocMon(Button nutDuocChon)
        {
            if (_cacNutBoLocLoaiMon.Count == 0)
            {
                return;
            }

            foreach (var btn in _cacNutBoLocLoaiMon)
            {
                var laNutDangChon = btn == nutDuocChon;
                btn.BackColor = laNutDangChon ? Color.FromArgb(94, 64, 47) : Color.FromArgb(248, 245, 241);
                btn.ForeColor = laNutDangChon ? Color.White : Color.FromArgb(79, 56, 43);
                btn.FlatAppearance.BorderSize = laNutDangChon ? 0 : 1;
                btn.FlatAppearance.BorderColor = laNutDangChon ? Color.FromArgb(94, 64, 47) : Color.FromArgb(224, 214, 203);
            }
        }

        private static string DinhDangTien(decimal soTien)
        {
            return $"{soTien:N0}đ";
        }

        private void OpenManagementForm(Form targetForm, string feature)
        {
            if (_permissionBUS.IsStaff())
            {
                targetForm.Dispose();
                MessageBox.Show("Bạn không có quyền truy cập chức năng quản lý.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenStandaloneForm(targetForm, feature);
        }

        private void OpenStandaloneForm(Form targetForm, string feature)
        {
            try
            {
                _permissionBUS.EnsurePermission(feature, PermissionActions.View, "Ban khong co quyen truy cap chuc nang nay.");
            }
            catch (UnauthorizedAccessException ex)
            {
                targetForm.Dispose();
                MessageBox.Show(ex.Message, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Hide();
            targetForm.FormClosed += (_, _) =>
            {
                if (!IsDisposed && !Disposing)
                {
                    Show();
                    BringToFront();
                    Activate();
                }
            };

            targetForm.Show(this);
        }

        private void MoTinhNangThongKe()
        {
            if (!_permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View))
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng thống kê.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Chức năng thống kê đang được phát triển.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MoTinhNangHoaDon()
        {
            if (!_permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View))
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng hóa đơn.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Chức năng hóa đơn đang được phát triển.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panelTopbar_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
