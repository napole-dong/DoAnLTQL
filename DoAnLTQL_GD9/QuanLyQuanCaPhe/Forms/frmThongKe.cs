using System.Globalization;
using System.Drawing.Drawing2D;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.UI;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmThongKe : Form
    {
        private const int SoCotBieuDo = 7;

        private readonly bool _isEmbedded;
        private readonly IPermissionService _permissionBUS;
        private readonly IThongKeService _thongKeBUS;
        private List<ThongKeHoaDonDTO> _duLieuHoaDonDangHienThi = new();
        private List<ThongKeTopMonDTO> _duLieuTopMonDangHienThi = new();
        private bool _dangTaiDuLieu;
        private bool _dangCapNhatPreset;
        private List<BieuDoDiem> _duLieuBieuDoDangVe = new();
        private ComboBox? _cboPresetNhanh;
        private Label? _lblPresetNhanh;
        private Panel? _cardTiLeHuy;
        private Label? _lblTiLeHuyValue;
        private Label? _lblTiLeHuyTitle;
        private Label? _lblTiLeHuyIcon;
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;

        private enum KieuThongKe
        {
            TheoNgay = 0,
            TheoTuan = 1,
            TheoThang = 2
        }

        private enum PresetBoLocNhanh
        {
            BayNgayGanDay = 0,
            HomNay = 1,
            TuanNay = 2,
            ThangNay = 3,
            QuyNay = 4,
            BaMuoiNgayGanDay = 5,
            TuyChinh = 6
        }

        private sealed class TopMonGridItem
        {
            public string TenMon { get; init; } = string.Empty;
            public int SoLuong { get; init; }
            public string DoanhThu { get; init; } = string.Empty;
        }

        private sealed class BieuDoDiem
        {
            public string Nhan { get; init; } = string.Empty;
            public decimal DoanhThu { get; init; }
        }

        public frmThongKe(
            IPermissionService? permissionBUS = null,
            IThongKeService? thongKeBUS = null,
            bool isEmbedded = false)
        {
            _permissionBUS = AppServiceProvider.Resolve(permissionBUS, () => new PermissionBUS());
            _thongKeBUS = AppServiceProvider.Resolve(thongKeBUS, () => new ThongKeBUS());
            _isEmbedded = isEmbedded;
            InitializeComponent();
            (_btnKhachHangSidebar, _btnQuanLyKhoSidebar, _btnAuditLogSidebar) = SidebarUiHelper.EnsureUnifiedSidebarMenu(
                flowSidebarMenu,
                btnBanHang,
                btnQuanLyBan,
                btnQuanLyMon,
                btnCongThuc,
                btnHoaDon,
                btnNhanVien,
                btnThongKe);

            CauHinhSuKienDieuHuong();
            CauHinhThongKeUi();
        }

        private void CauHinhSuKienDieuHuong()
        {
            Load += frmThongKe_Load;

            btnBanHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.BanHang, () => new frmBanHang(isEmbedded: true), skipNavigation: _isEmbedded);
            btnQuanLyBan.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiBan(isEmbedded: true), skipNavigation: _isEmbedded);
            btnQuanLyMon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmQuanLiMon(isEmbedded: true), skipNavigation: _isEmbedded);
            btnCongThuc.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.Menu, () => new frmCongThuc(isEmbedded: true), skipNavigation: _isEmbedded);
            btnHoaDon.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.HoaDon, () => new frmHoaDon(isEmbedded: true), skipNavigation: _isEmbedded);
            _btnQuanLyKhoSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NguyenLieu, () => new frmQuanLiKho(isEmbedded: true), skipNavigation: _isEmbedded);
            _btnKhachHangSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), skipNavigation: _isEmbedded);
            _btnAuditLogSidebar.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmAuditLog(isEmbedded: true), skipNavigation: _isEmbedded);
            btnNhanVien.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.NhanVien, () => new frmNhanVien(isEmbedded: true), skipNavigation: _isEmbedded);
            btnThongKe.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.ThongKe, () => new frmThongKe(isEmbedded: true), skipNavigation: _isEmbedded);
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void CauHinhThongKeUi()
        {
            dgvTopMon.AutoGenerateColumns = false;
            colTopTenMon.DataPropertyName = nameof(TopMonGridItem.TenMon);
            colTopSoLuong.DataPropertyName = nameof(TopMonGridItem.SoLuong);
            colTopDoanhThu.DataPropertyName = nameof(TopMonGridItem.DoanhThu);

            KhoiTaoCardTiLeHuy();
            KhoiTaoBieuDoDoanhThu();
            KhoiTaoBoLocNhanhUi();

            btnApDung.Click += (_, _) => TaiDuLieuThongKe();
            btnLamMoi.Click += (_, _) => LamMoiBoLoc();
            cboKieuThongKe.SelectedIndexChanged += (_, _) => cboKieuThongKe_SelectedIndexChanged();
            txtSearch.KeyDown += txtSearch_KeyDown;
            dtTuNgay.ValueChanged += (_, _) => DanhDauPresetTuyChinh();
            dtDenNgay.ValueChanged += (_, _) => DanhDauPresetTuyChinh();
        }

        private void KhoiTaoBieuDoDoanhThu()
        {
            panelBieuDoPlaceholder.Controls.Clear();
            panelBieuDoPlaceholder.Paint += PanelBieuDoPlaceholder_Paint;
            panelBieuDoPlaceholder.Resize += (_, _) => panelBieuDoPlaceholder.Invalidate();
            BatDoubleBufferChoControl(panelBieuDoPlaceholder);
        }

        private void KhoiTaoBoLocNhanhUi()
        {
            tableRight.RowStyles[0].Height = 248F;

            _lblPresetNhanh = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.DimGray,
                Location = new Point(14, 79),
                Name = "lblPresetNhanh",
                Size = new Size(84, 20),
                TabIndex = 50,
                Text = "Preset nhanh"
            };

            _cboPresetNhanh = new ComboBox
            {
                BackColor = Color.FromArgb(248, 245, 241),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                FormattingEnabled = true,
                Location = new Point(14, 102),
                Name = "cboPresetNhanh",
                Size = new Size(329, 28),
                TabIndex = 51
            };

            _cboPresetNhanh.Items.AddRange(new object[]
            {
                "7 ngay gan day",
                "Hom nay",
                "Tuan nay",
                "Thang nay",
                "Quy nay",
                "30 ngay gan day",
                "Tuy chinh"
            });

            _cboPresetNhanh.SelectedIndexChanged += (_, _) => cboPresetNhanh_SelectedIndexChanged();

            lblKieuThongKe.Location = new Point(14, 136);
            cboKieuThongKe.Location = new Point(14, 159);
            btnApDung.Location = new Point(14, 194);
            btnLamMoi.Location = new Point(231, 194);

            panelBoLoc.Controls.Add(_lblPresetNhanh);
            panelBoLoc.Controls.Add(_cboPresetNhanh);
            _lblPresetNhanh.BringToFront();
            _cboPresetNhanh.BringToFront();
        }

        private void KhoiTaoCardTiLeHuy()
        {
            tableStats.SuspendLayout();

            tableStats.ColumnCount = 5;
            tableStats.ColumnStyles.Clear();
            for (var i = 0; i < 5; i++)
            {
                tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            }

            tableStats.Controls.Clear();

            cardDoanhThuNgay.Margin = new Padding(0, 0, 8, 0);
            cardHoaDon.Margin = new Padding(0, 0, 8, 0);
            cardKhachHang.Margin = new Padding(0, 0, 8, 0);
            cardTrungBinh.Margin = new Padding(0, 0, 8, 0);

            tableStats.Controls.Add(cardDoanhThuNgay, 0, 0);
            tableStats.Controls.Add(cardHoaDon, 1, 0);
            tableStats.Controls.Add(cardKhachHang, 2, 0);
            tableStats.Controls.Add(cardTrungBinh, 3, 0);

            _cardTiLeHuy = new Panel
            {
                BackColor = Color.FromArgb(245, 239, 232),
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Name = "cardTiLeHuy",
                Padding = new Padding(16, 12, 16, 12)
            };

            _lblTiLeHuyIcon = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Emoji", 12F),
                Location = new Point(16, 8),
                Name = "lblTiLeHuyIcon",
                Size = new Size(39, 27),
                Text = "⚠"
            };

            _lblTiLeHuyTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(118, 93, 70),
                Location = new Point(18, 36),
                Name = "lblTiLeHuyTitle",
                Size = new Size(96, 23),
                Text = "Ti le huy don"
            };

            _lblTiLeHuyValue = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 74, 26),
                Location = new Point(18, 61),
                Name = "lblTiLeHuyValue",
                Size = new Size(47, 37),
                Text = "0%"
            };

            _cardTiLeHuy.Controls.Add(_lblTiLeHuyValue);
            _cardTiLeHuy.Controls.Add(_lblTiLeHuyTitle);
            _cardTiLeHuy.Controls.Add(_lblTiLeHuyIcon);

            tableStats.Controls.Add(_cardTiLeHuy, 4, 0);
            tableStats.ResumeLayout();
        }

        private void frmThongKe_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Thong ke"))
            {
                return;
            }

            try
            {
                _permissionBUS.EnsurePermission(PermissionFeatures.ThongKe, PermissionActions.View, "Ban khong co quyen truy cap chuc nang Thong ke.");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            if (_isEmbedded)
            {
                EmbeddedFormLayoutHelper.UseContentOnlyLayout(panelMain, panelSidebar, panelTopbar);
            }

            ApDungPhanQuyenDieuHuong();
            KhoiTaoBoLocMacDinh();
            TaiDuLieuThongKe();
        }

        private void KhoiTaoBoLocMacDinh()
        {
            lblDoanhThuNgayTitle.Text = "Doanh thu ngay";
            lblHoaDonTitle.Text = "Doanh thu thang";
            lblKhachHangTitle.Text = "Top mon";
            lblTrungBinhTitle.Text = "Top khung gio";

            lblHoaDonIcon.Text = "📅";
            lblKhachHangIcon.Text = "☕";
            lblTrungBinhIcon.Text = "⏰";

            lblTopMonHint.Text = "Xep hang theo so luong ban trong ky da chon";
            lblBieuDoHint.Text = "Doanh thu 7 ngay gan nhat (VND)";
            lblBoLocTitle.Text = "Bo loc du lieu nhanh";

            _dangCapNhatPreset = true;
            cboKieuThongKe.SelectedIndex = (int)KieuThongKe.TheoNgay;
            if (_cboPresetNhanh != null)
            {
                _cboPresetNhanh.SelectedIndex = (int)PresetBoLocNhanh.BayNgayGanDay;
            }

            _dangCapNhatPreset = false;
            ApDungPresetNhanh(PresetBoLocNhanh.BayNgayGanDay, taiDuLieu: false);

            CapNhatSoLieuTongQuan(Array.Empty<ThongKeHoaDonDTO>(), Array.Empty<ThongKeTopMonDTO>(), 0);
            CapNhatBangTopMon(Array.Empty<ThongKeTopMonDTO>());
            CapNhatBieuDo(Array.Empty<ThongKeHoaDonDTO>());
        }

        private void LamMoiBoLoc()
        {
            if (_dangTaiDuLieu)
            {
                return;
            }

            txtSearch.Clear();
            ApDungPresetNhanh(PresetBoLocNhanh.BayNgayGanDay, taiDuLieu: true);
        }

        private void cboKieuThongKe_SelectedIndexChanged()
        {
            if (_dangTaiDuLieu)
            {
                return;
            }

            if (cboKieuThongKe.SelectedIndex < 0)
            {
                return;
            }

            if (_dangCapNhatPreset)
            {
                return;
            }

            CapNhatKhoangNgayTheoKieuDangChon();
            DanhDauPresetTuyChinh();
            TaiDuLieuThongKe();
        }

        private void cboPresetNhanh_SelectedIndexChanged()
        {
            if (_dangTaiDuLieu)
            {
                return;
            }

            if (_dangCapNhatPreset || _cboPresetNhanh == null || _cboPresetNhanh.SelectedIndex < 0)
            {
                return;
            }

            var preset = LayPresetDangChon();
            if (preset == PresetBoLocNhanh.TuyChinh)
            {
                return;
            }

            ApDungPresetNhanh(preset, taiDuLieu: true);
        }

        private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (_dangTaiDuLieu)
            {
                return;
            }

            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            TaiDuLieuThongKe();
        }

        private PresetBoLocNhanh LayPresetDangChon()
        {
            return _cboPresetNhanh?.SelectedIndex switch
            {
                1 => PresetBoLocNhanh.HomNay,
                2 => PresetBoLocNhanh.TuanNay,
                3 => PresetBoLocNhanh.ThangNay,
                4 => PresetBoLocNhanh.QuyNay,
                5 => PresetBoLocNhanh.BaMuoiNgayGanDay,
                6 => PresetBoLocNhanh.TuyChinh,
                _ => PresetBoLocNhanh.BayNgayGanDay
            };
        }

        private void ApDungPresetNhanh(PresetBoLocNhanh preset, bool taiDuLieu)
        {
            _dangCapNhatPreset = true;

            var homNay = DateTime.Today;
            DateTime tuNgay;
            DateTime denNgay;
            KieuThongKe kieuThongKe;

            switch (preset)
            {
                case PresetBoLocNhanh.HomNay:
                    tuNgay = homNay;
                    denNgay = homNay;
                    kieuThongKe = KieuThongKe.TheoNgay;
                    break;

                case PresetBoLocNhanh.TuanNay:
                    tuNgay = LayNgayBatDauTuan(homNay);
                    denNgay = homNay;
                    kieuThongKe = KieuThongKe.TheoTuan;
                    break;

                case PresetBoLocNhanh.ThangNay:
                    tuNgay = new DateTime(homNay.Year, homNay.Month, 1);
                    denNgay = homNay;
                    kieuThongKe = KieuThongKe.TheoThang;
                    break;

                case PresetBoLocNhanh.QuyNay:
                {
                    var thangBatDauQuy = ((homNay.Month - 1) / 3) * 3 + 1;
                    tuNgay = new DateTime(homNay.Year, thangBatDauQuy, 1);
                    denNgay = homNay;
                    kieuThongKe = KieuThongKe.TheoThang;
                    break;
                }

                case PresetBoLocNhanh.BaMuoiNgayGanDay:
                    tuNgay = homNay.AddDays(-29);
                    denNgay = homNay;
                    kieuThongKe = KieuThongKe.TheoNgay;
                    break;

                default:
                    tuNgay = homNay.AddDays(-6);
                    denNgay = homNay;
                    kieuThongKe = KieuThongKe.TheoNgay;
                    break;
            }

            cboKieuThongKe.SelectedIndex = (int)kieuThongKe;
            dtTuNgay.Value = ClampDate(tuNgay, dtTuNgay.MinDate, dtTuNgay.MaxDate);
            dtDenNgay.Value = ClampDate(denNgay, dtDenNgay.MinDate, dtDenNgay.MaxDate);

            if (dtDenNgay.Value.Date < dtTuNgay.Value.Date)
            {
                dtDenNgay.Value = dtTuNgay.Value;
            }

            if (_cboPresetNhanh != null && _cboPresetNhanh.SelectedIndex != (int)preset)
            {
                _cboPresetNhanh.SelectedIndex = (int)preset;
            }

            _dangCapNhatPreset = false;

            if (taiDuLieu)
            {
                TaiDuLieuThongKe();
            }
        }

        private void DanhDauPresetTuyChinh()
        {
            if (_dangCapNhatPreset || _cboPresetNhanh == null)
            {
                return;
            }

            if (_cboPresetNhanh.SelectedIndex == (int)PresetBoLocNhanh.TuyChinh)
            {
                return;
            }

            _dangCapNhatPreset = true;
            _cboPresetNhanh.SelectedIndex = (int)PresetBoLocNhanh.TuyChinh;
            _dangCapNhatPreset = false;
        }

        private void CapNhatKhoangNgayTheoKieuDangChon()
        {
            var denNgay = dtDenNgay.Value.Date;
            DateTime tuNgay;

            switch (LayKieuThongKeDangChon())
            {
                case KieuThongKe.TheoTuan:
                {
                    var dauTuan = LayNgayBatDauTuan(denNgay);
                    tuNgay = dauTuan.AddDays(-42);
                    denNgay = dauTuan.AddDays(6);
                    break;
                }
                case KieuThongKe.TheoThang:
                {
                    var dauThang = new DateTime(denNgay.Year, denNgay.Month, 1);
                    tuNgay = dauThang.AddMonths(-6);
                    denNgay = dauThang.AddMonths(1).AddDays(-1);
                    break;
                }
                default:
                    tuNgay = denNgay.AddDays(-6);
                    break;
            }

            if (tuNgay < dtTuNgay.MinDate)
            {
                tuNgay = dtTuNgay.MinDate.Date;
            }

            if (denNgay > dtDenNgay.MaxDate)
            {
                denNgay = dtDenNgay.MaxDate.Date;
            }

            dtTuNgay.Value = tuNgay;
            dtDenNgay.Value = denNgay < tuNgay ? tuNgay : denNgay;
        }

        private void TaiDuLieuThongKe()
        {
            if (_dangTaiDuLieu)
            {
                return;
            }

            if (dtTuNgay.Value.Date > dtDenNgay.Value.Date)
            {
                MessageBox.Show("Khoang ngay khong hop le.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _dangTaiDuLieu = true;
            DatTrangThaiDangXuLyThongKe(true);

            try
            {
                var tuNgay = dtTuNgay.Value.Date;
                var denNgay = dtDenNgay.Value.Date;
                var tuKhoa = txtSearch.Text;

                var (dsHoaDonDaThanhToan, dsTopMon, soHoaDonHuy) = LayDuLieuThongKe(tuNgay, denNgay, tuKhoa);

                _duLieuHoaDonDangHienThi = dsHoaDonDaThanhToan;
                _duLieuTopMonDangHienThi = dsTopMon;

                CapNhatSoLieuTongQuan(dsHoaDonDaThanhToan, dsTopMon, soHoaDonHuy);
                CapNhatBangTopMon(dsTopMon);
                CapNhatBieuDo(dsHoaDonDaThanhToan);
                CapNhatMoTaThongKe();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Khong the tai du lieu thong ke: {ex.Message}", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                _dangTaiDuLieu = false;
                DatTrangThaiDangXuLyThongKe(false);
            }
        }

        private (List<ThongKeHoaDonDTO> HoaDon, List<ThongKeTopMonDTO> TopMon, int SoHoaDonHuy) LayDuLieuThongKe(
            DateTime tuNgay,
            DateTime denNgay,
            string? tuKhoa)
        {
            lblBoLocTitle.Text = "Bo loc du lieu nhanh";
            return (
                _thongKeBUS.LayDanhSachHoaDonDaThanhToan(tuNgay, denNgay, tuKhoa),
                _thongKeBUS.LayTopMonBanChay(tuNgay, denNgay, tuKhoa),
                _thongKeBUS.LaySoHoaDonHuy(tuNgay, denNgay, tuKhoa));
        }

        private void DatTrangThaiDangXuLyThongKe(bool dangXuLy)
        {
            var controls = new List<Control>
            {
                btnApDung,
                btnLamMoi,
                txtSearch,
                cboKieuThongKe,
                dtTuNgay,
                dtDenNgay
            };

            if (_cboPresetNhanh != null)
            {
                controls.Add(_cboPresetNhanh);
            }

            UiLoadingStateHelper.Apply(this, dangXuLy, controls.ToArray());
        }

        private static DateTime ClampDate(DateTime value, DateTime min, DateTime max)
        {
            if (value < min.Date)
            {
                return min.Date;
            }

            if (value > max.Date)
            {
                return max.Date;
            }

            return value.Date;
        }

        private void CapNhatSoLieuTongQuan(
            IReadOnlyCollection<ThongKeHoaDonDTO> dsHoaDonDaThanhToan,
            IReadOnlyCollection<ThongKeTopMonDTO> dsTopMon,
            int soHoaDonHuy)
        {
            var denNgay = dtDenNgay.Value.Date;
            var dauThang = new DateTime(denNgay.Year, denNgay.Month, 1);

            var doanhThuNgay = dsHoaDonDaThanhToan
                .Where(x => x.NgayLap.Date == denNgay)
                .Sum(x => x.TongTien);

            var doanhThuThang = dsHoaDonDaThanhToan
                .Where(x => x.NgayLap.Date >= dauThang && x.NgayLap.Date <= denNgay)
                .Sum(x => x.TongTien);

            var topMon = dsTopMon
                .OrderByDescending(x => x.SoLuongBan)
                .ThenByDescending(x => x.DoanhThu)
                .ThenBy(x => x.TenMon)
                .FirstOrDefault();

            var topKhungGio = TinhTopKhungGioBanChay(dsHoaDonDaThanhToan);
            var tongHoaDon = dsHoaDonDaThanhToan.Count + Math.Max(0, soHoaDonHuy);
            var tiLeHuy = tongHoaDon == 0
                ? 0m
                : (decimal)Math.Max(0, soHoaDonHuy) * 100m / tongHoaDon;

            lblDoanhThuNgayValue.Text = DinhDangTien(doanhThuNgay);
            lblHoaDonValue.Text = DinhDangTien(doanhThuThang);
            lblKhachHangValue.Text = topMon == null ? "Chua co" : TaoGiaTriTopMon(topMon);
            lblTrungBinhValue.Text = topKhungGio ?? "Chua co";

            if (_lblTiLeHuyValue != null)
            {
                _lblTiLeHuyValue.Text = DinhDangTyLe(tiLeHuy);
            }

            if (_lblTiLeHuyTitle != null)
            {
                _lblTiLeHuyTitle.Text = TaoThongTinTiLeHuy(soHoaDonHuy, tongHoaDon);
            }
        }

        private static string TaoGiaTriTopMon(ThongKeTopMonDTO topMon)
        {
            var tenMon = RutGonChuoi(topMon.TenMon, 14);
            return $"{tenMon} x{topMon.SoLuongBan}";
        }

        private static string? TinhTopKhungGioBanChay(IEnumerable<ThongKeHoaDonDTO> dsHoaDonDaThanhToan)
        {
            var topKhung = dsHoaDonDaThanhToan
                .GroupBy(x => (x.NgayLap.Hour / 2) * 2)
                .Select(x => new
                {
                    GioBatDau = x.Key,
                    SoHoaDon = x.Count(),
                    DoanhThu = x.Sum(hd => hd.TongTien)
                })
                .OrderByDescending(x => x.SoHoaDon)
                .ThenByDescending(x => x.DoanhThu)
                .ThenBy(x => x.GioBatDau)
                .FirstOrDefault();

            if (topKhung == null)
            {
                return null;
            }

            var gioKetThuc = (topKhung.GioBatDau + 2) % 24;
            return $"{topKhung.GioBatDau:00}:00-{gioKetThuc:00}:00";
        }

        private static string RutGonChuoi(string value, int doDaiToiDa)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length <= doDaiToiDa)
            {
                return value;
            }

            return string.Concat(value.AsSpan(0, Math.Max(0, doDaiToiDa - 1)), "...");
        }

        private void CapNhatBangTopMon(IReadOnlyCollection<ThongKeTopMonDTO> dsTopMon)
        {
            var nguon = dsTopMon
                .Select(x => new TopMonGridItem
                {
                    TenMon = x.TenMon,
                    SoLuong = x.SoLuongBan,
                    DoanhThu = DinhDangTien(x.DoanhThu)
                })
                .ToList();

            dgvTopMon.DataSource = null;
            dgvTopMon.DataSource = nguon;
        }

        private void CapNhatBieuDo(IReadOnlyCollection<ThongKeHoaDonDTO> dsHoaDonDaThanhToan)
        {
            _duLieuBieuDoDangVe = TaoDuLieuCotBieuDo(dsHoaDonDaThanhToan);
            panelBieuDoPlaceholder.Invalidate();
        }

        private void PanelBieuDoPlaceholder_Paint(object? sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var duLieuBieuDo = _duLieuBieuDoDangVe;
            if (duLieuBieuDo.Count == 0)
            {
                return;
            }

            var clientRect = panelBieuDoPlaceholder.ClientRectangle;
            if (clientRect.Width < 240 || clientRect.Height < 180)
            {
                return;
            }

            var khungVe = new RectangleF(
                clientRect.Left + 48,
                clientRect.Top + 18,
                clientRect.Width - 72,
                clientRect.Height - 80);

            using var axisPen = new Pen(Color.FromArgb(210, 197, 182), 1.2f);
            using var gridPen = new Pen(Color.FromArgb(232, 223, 213), 1f);

            for (var i = 0; i <= 4; i++)
            {
                var y = khungVe.Top + khungVe.Height * i / 4f;
                graphics.DrawLine(gridPen, khungVe.Left, y, khungVe.Right, y);
            }

            graphics.DrawLine(axisPen, khungVe.Left, khungVe.Bottom, khungVe.Right, khungVe.Bottom);

            var doanhThuMax = Math.Max(duLieuBieuDo.Max(x => x.DoanhThu), 1m);
            var soCot = duLieuBieuDo.Count;
            var slotWidth = khungVe.Width / soCot;
            var chieuRongCot = Math.Min(56f, slotWidth * 0.6f);
            var dayCotY = khungVe.Bottom - 1;

            using var fontNhan = new Font("Segoe UI", 8.8f);
            using var fontGiaTri = new Font("Segoe UI Semibold", 8.2f, FontStyle.Bold);
            using var brushNhan = new SolidBrush(Color.FromArgb(98, 88, 79));
            using var brushGiaTri = new SolidBrush(Color.FromArgb(90, 67, 52));

            for (var i = 0; i < soCot; i++)
            {
                var diem = duLieuBieuDo[i];
                var tiLe = (double)(diem.DoanhThu / doanhThuMax);
                var chieuCaoCot = Math.Max(4f, (float)(tiLe * (khungVe.Height - 8f)));

                var cotX = khungVe.Left + i * slotWidth + (slotWidth - chieuRongCot) / 2f;
                var cotY = dayCotY - chieuCaoCot;
                var cotRect = new RectangleF(cotX, cotY, chieuRongCot, chieuCaoCot);

                using (var cotPath = TaoRoundedRectanglePath(cotRect, 6f))
                using (var cotBrush = new LinearGradientBrush(cotRect,
                           Color.FromArgb(176, 133, 106),
                           Color.FromArgb(94, 64, 47),
                           LinearGradientMode.Vertical))
                {
                    graphics.FillPath(cotBrush, cotPath);
                }

                var giaTri = diem.DoanhThu <= 0 ? "0" : DinhDangGiaTriCot(diem.DoanhThu);
                var giaTriSize = graphics.MeasureString(giaTri, fontGiaTri);
                graphics.DrawString(
                    giaTri,
                    fontGiaTri,
                    brushGiaTri,
                    cotX + (chieuRongCot - giaTriSize.Width) / 2f,
                    Math.Max(khungVe.Top + 2f, cotY - giaTriSize.Height - 4f));

                var nhanSize = graphics.MeasureString(diem.Nhan, fontNhan);
                graphics.DrawString(
                    diem.Nhan,
                    fontNhan,
                    brushNhan,
                    cotX + (chieuRongCot - nhanSize.Width) / 2f,
                    khungVe.Bottom + 8f);
            }
        }

        private static GraphicsPath TaoRoundedRectanglePath(RectangleF rectangle, float radius)
        {
            var banKinh = Math.Max(0f, radius);
            var duongKinh = banKinh * 2f;
            var path = new GraphicsPath();

            if (duongKinh <= 0.1f)
            {
                path.AddRectangle(rectangle);
                return path;
            }

            path.AddArc(rectangle.X, rectangle.Y, duongKinh, duongKinh, 180, 90);
            path.AddArc(rectangle.Right - duongKinh, rectangle.Y, duongKinh, duongKinh, 270, 90);
            path.AddArc(rectangle.Right - duongKinh, rectangle.Bottom - duongKinh, duongKinh, duongKinh, 0, 90);
            path.AddArc(rectangle.X, rectangle.Bottom - duongKinh, duongKinh, duongKinh, 90, 90);
            path.CloseFigure();

            return path;
        }

        private List<BieuDoDiem> TaoDuLieuCotBieuDo(IReadOnlyCollection<ThongKeHoaDonDTO> dsHoaDonDaThanhToan)
        {
            var dsHoaDon = dsHoaDonDaThanhToan.ToList();
            var denNgay = dtDenNgay.Value.Date;

            return LayKieuThongKeDangChon() switch
            {
                KieuThongKe.TheoTuan => TaoDuLieuCotTheoTuan(dsHoaDon, denNgay),
                KieuThongKe.TheoThang => TaoDuLieuCotTheoThang(dsHoaDon, denNgay),
                _ => TaoDuLieuCotTheoNgay(dsHoaDon, denNgay)
            };
        }

        private static List<BieuDoDiem> TaoDuLieuCotTheoNgay(IReadOnlyCollection<ThongKeHoaDonDTO> dsHoaDon, DateTime denNgay)
        {
            var batDau = denNgay.AddDays(-(SoCotBieuDo - 1));
            var doanhThuTheoNgay = dsHoaDon
                .GroupBy(x => x.NgayLap.Date)
                .ToDictionary(x => x.Key, x => x.Sum(hd => hd.TongTien));

            var ketQua = new List<BieuDoDiem>(SoCotBieuDo);
            for (var i = 0; i < SoCotBieuDo; i++)
            {
                var ngay = batDau.AddDays(i);
                doanhThuTheoNgay.TryGetValue(ngay, out var doanhThu);

                ketQua.Add(new BieuDoDiem
                {
                    Nhan = ngay.ToString("dd/MM"),
                    DoanhThu = doanhThu
                });
            }

            return ketQua;
        }

        private static List<BieuDoDiem> TaoDuLieuCotTheoTuan(IReadOnlyCollection<ThongKeHoaDonDTO> dsHoaDon, DateTime denNgay)
        {
            var dauTuanHienTai = LayNgayBatDauTuan(denNgay);
            var ketQua = new List<BieuDoDiem>(SoCotBieuDo);

            for (var i = 0; i < SoCotBieuDo; i++)
            {
                var dauTuan = dauTuanHienTai.AddDays((i - (SoCotBieuDo - 1)) * 7);
                var cuoiTuan = dauTuan.AddDays(6);

                var doanhThu = dsHoaDon
                    .Where(x => x.NgayLap.Date >= dauTuan && x.NgayLap.Date <= cuoiTuan)
                    .Sum(x => x.TongTien);

                ketQua.Add(new BieuDoDiem
                {
                    Nhan = $"W{ISOWeek.GetWeekOfYear(dauTuan)}",
                    DoanhThu = doanhThu
                });
            }

            return ketQua;
        }

        private static List<BieuDoDiem> TaoDuLieuCotTheoThang(IReadOnlyCollection<ThongKeHoaDonDTO> dsHoaDon, DateTime denNgay)
        {
            var dauThangHienTai = new DateTime(denNgay.Year, denNgay.Month, 1);
            var ketQua = new List<BieuDoDiem>(SoCotBieuDo);

            for (var i = 0; i < SoCotBieuDo; i++)
            {
                var dauThang = dauThangHienTai.AddMonths(i - (SoCotBieuDo - 1));
                var cuoiThang = dauThang.AddMonths(1).AddDays(-1);

                var doanhThu = dsHoaDon
                    .Where(x => x.NgayLap.Date >= dauThang && x.NgayLap.Date <= cuoiThang)
                    .Sum(x => x.TongTien);

                ketQua.Add(new BieuDoDiem
                {
                    Nhan = dauThang.ToString("MM/yy"),
                    DoanhThu = doanhThu
                });
            }

            return ketQua;
        }

        private void CapNhatMoTaThongKe()
        {
            lblTopMonHint.Text = "Xep hang theo so luong ban trong ky da chon";

            lblBieuDoHint.Text = LayKieuThongKeDangChon() switch
            {
                KieuThongKe.TheoTuan => "Doanh thu 7 tuan gan nhat (VND)",
                KieuThongKe.TheoThang => "Doanh thu 7 thang gan nhat (VND)",
                _ => "Doanh thu 7 ngay gan nhat (VND)"
            };
        }

        private KieuThongKe LayKieuThongKeDangChon()
        {
            return cboKieuThongKe.SelectedIndex switch
            {
                1 => KieuThongKe.TheoTuan,
                2 => KieuThongKe.TheoThang,
                _ => KieuThongKe.TheoNgay
            };
        }

        private static DateTime LayNgayBatDauTuan(DateTime ngay)
        {
            var offset = ((int)ngay.DayOfWeek + 6) % 7;
            return ngay.Date.AddDays(-offset);
        }

        private static string DinhDangTien(decimal soTien)
        {
            return $"{soTien:N0} VND";
        }

        private static string DinhDangGiaTriCot(decimal soTien)
        {
            if (soTien >= 1_000_000m)
            {
                return $"{soTien / 1_000_000m:0.#}tr";
            }

            if (soTien >= 1_000m)
            {
                return $"{soTien / 1_000m:0.#}k";
            }

            return soTien.ToString("0");
        }

        private static void BatDoubleBufferChoControl(Control control)
        {
            var propertyInfo = typeof(Control).GetProperty(
                "DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            propertyInfo?.SetValue(control, true, null);
        }

        private static string DinhDangTyLe(decimal giaTri)
        {
            return $"{giaTri:0.#}%";
        }

        private static string TaoThongTinTiLeHuy(int soHoaDonHuy, int tongHoaDon)
        {
            if (tongHoaDon <= 0)
            {
                return "Ti le huy don";
            }

            return $"Ti le huy don ({soHoaDonHuy}/{tongHoaDon})";
        }

        private void ApDungPhanQuyenDieuHuong()
        {
            var coQuyenThongKeXem = _permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View);

            SidebarUiHelper.ApplySidebarVisibility(
                _permissionBUS,
                btnBanHang,
                btnQuanLyBan,
                btnQuanLyMon,
                btnCongThuc,
                btnHoaDon,
                btnNhanVien,
                btnThongKe,
                btnKhachHang: _btnKhachHangSidebar,
                btnQuanLyKho: _btnQuanLyKhoSidebar,
                btnAuditLog: _btnAuditLogSidebar);

            SidebarUiHelper.HighlightSidebarSelection(
                btnThongKe,
                btnBanHang,
                btnQuanLyBan,
                btnQuanLyMon,
                btnCongThuc,
                _btnQuanLyKhoSidebar,
                btnHoaDon,
                _btnKhachHangSidebar,
                btnNhanVien,
                btnThongKe,
                _btnAuditLogSidebar);

            btnNhap.Visible = false;
            btnNhap.Enabled = false;
            btnXuat.Visible = false;
            btnXuat.Enabled = false;
            txtSearch.Enabled = coQuyenThongKeXem;
        }
    }
}
