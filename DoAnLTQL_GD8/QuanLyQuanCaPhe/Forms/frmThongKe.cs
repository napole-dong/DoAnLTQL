using System.Globalization;
using System.IO;
using System.Text;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Export;
using QuanLyQuanCaPhe.Services.Navigation;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmThongKe : Form
    {
        private const int SoCotBieuDo = 7;

        private readonly bool _isEmbedded;
        private readonly PermissionBUS _permissionBUS = new();
        private readonly ThongKeBUS _thongKeBUS = new();
        private readonly DataExportService _dataExportService = new();
        private List<ThongKeHoaDonDTO> _duLieuHoaDonDangHienThi = new();
        private List<ThongKeTopMonDTO> _duLieuTopMonDangHienThi = new();
        private List<ThongKeHoaDonDTO>? _duLieuHoaDonTuFile;
        private List<ThongKeTopMonDTO>? _duLieuTopMonTuFile;
        private bool _dangDungDuLieuTuFile;
        private bool _dangTaiDuLieu;

        private enum KieuThongKe
        {
            TheoNgay = 0,
            TheoTuan = 1,
            TheoThang = 2
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

        public frmThongKe(bool isEmbedded = false)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();

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
            btnKhachHang.Click += (_, _) => FormNavigationService.Navigate(this, _permissionBUS, PermissionFeatures.KhachHang, () => new frmKhachHang(isEmbedded: true), skipNavigation: _isEmbedded);
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

            btnApDung.Click += (_, _) => TaiDuLieuThongKe();
            btnLamMoi.Click += (_, _) => LamMoiBoLoc();
            btnNhap.Click += (_, _) => NhapDuLieuThongKe();
            btnXuat.Click += (_, _) => XuatDuLieuThongKe();
            cboKieuThongKe.SelectedIndexChanged += (_, _) => cboKieuThongKe_SelectedIndexChanged();
            txtSearch.KeyDown += txtSearch_KeyDown;
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
                panelSidebar.Visible = false;
                panelTopbar.Visible = false;
                panelMain.Dock = DockStyle.Fill;
            }

            ApDungPhanQuyenDieuHuong();
            KhoiTaoBoLocMacDinh();
            TaiDuLieuThongKe();
        }

        private void KhoiTaoBoLocMacDinh()
        {
            cboKieuThongKe.SelectedIndex = (int)KieuThongKe.TheoNgay;
            dtDenNgay.Value = DateTime.Today;
            dtTuNgay.Value = DateTime.Today.AddDays(-6);

            lblDoanhThuNgayTitle.Text = "Doanh thu trong ky";
            lblHoaDonTitle.Text = "Hoa don trong ky";
            lblKhachHangTitle.Text = "Khach phuc vu";
            lblTrungBinhTitle.Text = "Gia tri hoa don TB";

            lblTopMonHint.Text = "Xep hang theo so luong ban trong ky da chon";
            lblBieuDoHint.Text = "Doanh thu 7 ngay gan nhat (VND)";
            lblBoLocTitle.Text = "Bo loc du lieu nhanh";

            CapNhatSoLieuTongQuan(Array.Empty<ThongKeHoaDonDTO>());
            CapNhatBangTopMon(Array.Empty<ThongKeTopMonDTO>());
            CapNhatBieuDo(Array.Empty<ThongKeHoaDonDTO>());
        }

        private void LamMoiBoLoc()
        {
            if (_dangDungDuLieuTuFile)
            {
                _dangDungDuLieuTuFile = false;
                _duLieuHoaDonTuFile = null;
                _duLieuTopMonTuFile = null;
                lblBoLocTitle.Text = "Bo loc du lieu nhanh";
            }

            txtSearch.Clear();
            dtDenNgay.Value = DateTime.Today;
            CapNhatKhoangNgayTheoKieuDangChon();
            TaiDuLieuThongKe();
        }

        private void cboKieuThongKe_SelectedIndexChanged()
        {
            if (cboKieuThongKe.SelectedIndex < 0)
            {
                return;
            }

            CapNhatKhoangNgayTheoKieuDangChon();
            TaiDuLieuThongKe();
        }

        private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            TaiDuLieuThongKe();
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
            UseWaitCursor = true;

            try
            {
                var tuNgay = dtTuNgay.Value.Date;
                var denNgay = dtDenNgay.Value.Date;
                var tuKhoa = txtSearch.Text;

                var (dsHoaDonDaThanhToan, dsTopMon) = LayDuLieuThongKe(tuNgay, denNgay, tuKhoa);

                _duLieuHoaDonDangHienThi = dsHoaDonDaThanhToan;
                _duLieuTopMonDangHienThi = dsTopMon;

                CapNhatSoLieuTongQuan(dsHoaDonDaThanhToan);
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
                UseWaitCursor = false;
                _dangTaiDuLieu = false;
            }
        }

        private (List<ThongKeHoaDonDTO> HoaDon, List<ThongKeTopMonDTO> TopMon) LayDuLieuThongKe(
            DateTime tuNgay,
            DateTime denNgay,
            string? tuKhoa)
        {
            if (!_dangDungDuLieuTuFile)
            {
                lblBoLocTitle.Text = "Bo loc du lieu nhanh";
                return (
                    _thongKeBUS.LayDanhSachHoaDonDaThanhToan(tuNgay, denNgay, tuKhoa),
                    _thongKeBUS.LayTopMonBanChay(tuNgay, denNgay, tuKhoa));
            }

            lblBoLocTitle.Text = "Bo loc du lieu (nguon: tep CSV)";
            var dsHoaDon = LocHoaDonTuDuLieuNhap(tuNgay, denNgay, tuKhoa);
            var dsTopMon = LocTopMonTuDuLieuNhap(tuKhoa);
            return (dsHoaDon, dsTopMon);
        }

        private List<ThongKeHoaDonDTO> LocHoaDonTuDuLieuNhap(DateTime tuNgay, DateTime denNgay, string? tuKhoa)
        {
            var duLieuNguon = _duLieuHoaDonTuFile ?? new List<ThongKeHoaDonDTO>();

            IEnumerable<ThongKeHoaDonDTO> query = duLieuNguon.Where(x =>
                x.NgayLap.Date >= tuNgay.Date && x.NgayLap.Date <= denNgay.Date);

            var tuKhoaDaChuanHoa = BusInputHelper.NormalizeNullableText(tuKhoa);
            if (!string.IsNullOrWhiteSpace(tuKhoaDaChuanHoa))
            {
                var hasKeywordId = int.TryParse(tuKhoaDaChuanHoa, out var keywordId);
                query = query.Where(x =>
                    (hasKeywordId && x.HoaDonId == keywordId)
                    || x.HoaDonId.ToString().Contains(tuKhoaDaChuanHoa, StringComparison.OrdinalIgnoreCase));
            }

            return query
                .OrderByDescending(x => x.NgayLap)
                .ThenByDescending(x => x.HoaDonId)
                .ToList();
        }

        private List<ThongKeTopMonDTO> LocTopMonTuDuLieuNhap(string? tuKhoa)
        {
            var duLieuNguon = _duLieuTopMonTuFile ?? new List<ThongKeTopMonDTO>();

            IEnumerable<ThongKeTopMonDTO> query = duLieuNguon;
            var tuKhoaDaChuanHoa = BusInputHelper.NormalizeNullableText(tuKhoa);
            if (!string.IsNullOrWhiteSpace(tuKhoaDaChuanHoa))
            {
                var hasMonId = int.TryParse(tuKhoaDaChuanHoa, out var monId);
                query = query.Where(x =>
                    (hasMonId && x.MonID == monId)
                    || x.TenMon.Contains(tuKhoaDaChuanHoa, StringComparison.OrdinalIgnoreCase));
            }

            return query
                .OrderByDescending(x => x.SoLuongBan)
                .ThenByDescending(x => x.DoanhThu)
                .ThenBy(x => x.TenMon)
                .ToList();
        }

        private void NhapDuLieuThongKe()
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                Title = "Nhap du lieu thong ke"
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
                MessageBox.Show("Tep CSV khong co du lieu.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var (dsHoaDon, dsTopMon, soBoQua) = DocDuLieuThongKeTuCsv(lines);
            if (dsHoaDon.Count == 0 && dsTopMon.Count == 0)
            {
                MessageBox.Show("Khong tim thay dong du lieu hop le trong tep CSV.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _duLieuHoaDonTuFile = dsHoaDon;
            _duLieuTopMonTuFile = dsTopMon;
            _dangDungDuLieuTuFile = true;

            if (dsHoaDon.Count > 0)
            {
                var minNgay = dsHoaDon.Min(x => x.NgayLap).Date;
                var maxNgay = dsHoaDon.Max(x => x.NgayLap).Date;

                dtTuNgay.Value = ClampDate(minNgay, dtTuNgay.MinDate, dtTuNgay.MaxDate);
                dtDenNgay.Value = ClampDate(maxNgay, dtDenNgay.MinDate, dtDenNgay.MaxDate);

                if (dtDenNgay.Value.Date < dtTuNgay.Value.Date)
                {
                    dtDenNgay.Value = dtTuNgay.Value;
                }
            }

            TaiDuLieuThongKe();

            MessageBox.Show(
                $"Nhap du lieu thong ke thanh cong.\nHoa don: {dsHoaDon.Count}\nTop mon: {dsTopMon.Count}\nBo qua: {soBoQua}\nNhan 'Lam moi' de quay lai du lieu he thong.",
                "Thong bao",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void XuatDuLieuThongKe()
        {
            if (_duLieuHoaDonDangHienThi.Count == 0 && _duLieuTopMonDangHienThi.Count == 0)
            {
                MessageBox.Show("Khong co du lieu thong ke de xuat.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var tieuDeCot = new[]
            {
                "LoaiDuLieu",
                "HoaDonID",
                "NgayLap",
                "KhachHangID",
                "TongTien",
                "MonID",
                "TenMon",
                "SoLuongBan",
                "DoanhThuMon"
            };

            var duLieu = new List<IReadOnlyList<string>>();

            duLieu.AddRange(_duLieuHoaDonDangHienThi.Select(x => (IReadOnlyList<string>)new[]
            {
                "HoaDon",
                x.HoaDonId.ToString(CultureInfo.InvariantCulture),
                x.NgayLap.ToString("O", CultureInfo.InvariantCulture),
                x.KhachHangID?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                x.TongTien.ToString(CultureInfo.InvariantCulture),
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
            }));

            duLieu.AddRange(_duLieuTopMonDangHienThi.Select(x => (IReadOnlyList<string>)new[]
            {
                "TopMon",
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                x.MonID.ToString(CultureInfo.InvariantCulture),
                x.TenMon,
                x.SoLuongBan.ToString(CultureInfo.InvariantCulture),
                x.DoanhThu.ToString(CultureInfo.InvariantCulture)
            }));

            var ketQua = _dataExportService.XuatBangDuLieu(
                this,
                "Xuat du lieu thong ke",
                $"ThongKe_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                "Du lieu thong ke",
                tieuDeCot,
                duLieu);

            if (ketQua.DaHuy)
            {
                return;
            }

            MessageBox.Show(
                ketQua.ThongBao,
                "Thong bao",
                MessageBoxButtons.OK,
                ketQua.ThanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private static (List<ThongKeHoaDonDTO> HoaDon, List<ThongKeTopMonDTO> TopMon, int SoBoQua) DocDuLieuThongKeTuCsv(string[] lines)
        {
            var dsHoaDon = new List<ThongKeHoaDonDTO>();
            var dsTopMon = new List<ThongKeTopMonDTO>();
            var soBoQua = 0;

            var startIndex = lines[0].Contains("LoaiDuLieu", StringComparison.OrdinalIgnoreCase)
                ? 1
                : 0;

            for (var i = startIndex; i < lines.Length; i++)
            {
                if (!BusInputHelper.TrySplitCsvLine(lines[i], out var cot, out _))
                {
                    soBoQua++;
                    continue;
                }

                if (cot.Count < 9)
                {
                    soBoQua++;
                    continue;
                }

                var loaiDuLieu = BusInputHelper.NormalizeText(cot[0]);
                if (loaiDuLieu.Equals("HoaDon", StringComparison.OrdinalIgnoreCase))
                {
                    if (!int.TryParse(BusInputHelper.NormalizeText(cot[1]), out var hoaDonId)
                        || !TryParseDateTimeValue(cot[2], out var ngayLap)
                        || !TryParseDecimalValue(cot[4], out var tongTien))
                    {
                        soBoQua++;
                        continue;
                    }

                    var khachHangId = int.TryParse(BusInputHelper.NormalizeText(cot[3]), out var parsedKhachHangId)
                        && parsedKhachHangId > 0
                        ? parsedKhachHangId
                        : (int?)null;

                    dsHoaDon.Add(new ThongKeHoaDonDTO
                    {
                        HoaDonId = hoaDonId,
                        NgayLap = ngayLap,
                        KhachHangID = khachHangId,
                        TongTien = tongTien
                    });

                    continue;
                }

                if (loaiDuLieu.Equals("TopMon", StringComparison.OrdinalIgnoreCase))
                {
                    if (!int.TryParse(BusInputHelper.NormalizeText(cot[5]), out var monId)
                        || string.IsNullOrWhiteSpace(BusInputHelper.NormalizeText(cot[6]))
                        || !int.TryParse(BusInputHelper.NormalizeText(cot[7]), out var soLuongBan)
                        || !TryParseDecimalValue(cot[8], out var doanhThuMon))
                    {
                        soBoQua++;
                        continue;
                    }

                    dsTopMon.Add(new ThongKeTopMonDTO
                    {
                        MonID = monId,
                        TenMon = BusInputHelper.NormalizeText(cot[6]),
                        SoLuongBan = soLuongBan,
                        DoanhThu = doanhThuMon
                    });

                    continue;
                }

                soBoQua++;
            }

            return (dsHoaDon, dsTopMon, soBoQua);
        }

        private static bool TryParseDateTimeValue(string value, out DateTime result)
        {
            var normalized = BusInputHelper.NormalizeText(value);
            return DateTime.TryParse(normalized, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out result)
                   || DateTime.TryParse(normalized, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out result);
        }

        private static bool TryParseDecimalValue(string value, out decimal result)
        {
            var normalized = BusInputHelper.NormalizeText(value);
            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out result)
                   || decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.CurrentCulture, out result);
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

        private void CapNhatSoLieuTongQuan(IReadOnlyCollection<ThongKeHoaDonDTO> dsHoaDonDaThanhToan)
        {
            var tongDoanhThu = dsHoaDonDaThanhToan.Sum(x => x.TongTien);
            var soHoaDon = dsHoaDonDaThanhToan.Count;
            var soKhachPhucVu = TinhSoLuotKhachPhucVu(dsHoaDonDaThanhToan);
            var giaTriTrungBinh = soHoaDon == 0 ? 0 : tongDoanhThu / soHoaDon;

            lblDoanhThuNgayValue.Text = DinhDangTien(tongDoanhThu);
            lblHoaDonValue.Text = soHoaDon.ToString("N0");
            lblKhachHangValue.Text = soKhachPhucVu.ToString("N0");
            lblTrungBinhValue.Text = DinhDangTien(giaTriTrungBinh);
        }

        private static int TinhSoLuotKhachPhucVu(IEnumerable<ThongKeHoaDonDTO> dsHoaDonDaThanhToan)
        {
            var ds = dsHoaDonDaThanhToan.ToList();

            var khachThanhVien = ds
                .Where(x => x.KhachHangID.HasValue && x.KhachHangID.Value > 0)
                .Select(x => x.KhachHangID!.Value)
                .Distinct()
                .Count();

            var khachLe = ds.Count(x => !x.KhachHangID.HasValue || x.KhachHangID.Value <= 0);

            return khachThanhVien + khachLe;
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
            var duLieuCot = TaoDuLieuCotBieuDo(dsHoaDonDaThanhToan);
            var nhanCot = LayNhanCotBieuDo();
            var cotGiaTri = LayCotGiaTriBieuDo();

            var doanhThuMax = duLieuCot.Max(x => x.DoanhThu);
            const int chieuCaoToiThieu = 36;
            const int chieuCaoToiDa = 296;

            for (var i = 0; i < SoCotBieuDo; i++)
            {
                var cot = cotGiaTri[i];
                var duLieu = duLieuCot[i];

                nhanCot[i].Text = duLieu.Nhan;
                cot.Text = duLieu.DoanhThu <= 0 ? "0" : DinhDangGiaTriCot(duLieu.DoanhThu);

                var chieuCao = chieuCaoToiThieu;
                if (doanhThuMax > 0)
                {
                    var tiLe = duLieu.DoanhThu / doanhThuMax;
                    chieuCao = (int)Math.Round((double)(tiLe * (chieuCaoToiDa - chieuCaoToiThieu))) + chieuCaoToiThieu;
                }

                cot.Height = Math.Clamp(chieuCao, chieuCaoToiThieu, chieuCaoToiDa);
            }
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

        private Label[] LayCotGiaTriBieuDo()
        {
            return new[]
            {
                lblGiaTriChuNhat,
                lblGiaTriThu2,
                lblGiaTriThu3,
                lblGiaTriThu4,
                lblGiaTriThu5,
                lblGiaTriThu6,
                lblGiaTriThu7
            };
        }

        private Label[] LayNhanCotBieuDo()
        {
            return new[]
            {
                lblChuNhat,
                lblThu2,
                lblThu3,
                lblThu4,
                lblThu5,
                lblThu6,
                lblThu7
            };
        }

        private void ApDungPhanQuyenDieuHuong()
        {
            var coQuyenMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
            var coQuyenKhachHang = _permissionBUS.CheckPermission(PermissionFeatures.KhachHang, PermissionActions.View);
            var coQuyenThongKeXem = _permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View);
            var coQuyenThongKeNhap = _permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.Create)
                                  || _permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.Update);

            btnBanHang.Visible = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View);
            btnQuanLyBan.Visible = coQuyenMenu;
            btnQuanLyMon.Visible = coQuyenMenu;
            btnCongThuc.Visible = coQuyenMenu;
            btnKhachHang.Visible = coQuyenKhachHang;
            btnNhanVien.Visible = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View);
            btnHoaDon.Visible = _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);
            btnThongKe.Visible = _permissionBUS.CanViewReport();
            btnNhap.Visible = coQuyenThongKeNhap;
            btnXuat.Visible = coQuyenThongKeXem;
        }
    }
}
