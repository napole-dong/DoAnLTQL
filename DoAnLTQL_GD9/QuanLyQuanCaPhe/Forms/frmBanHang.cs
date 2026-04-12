using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Presenters;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Navigation;
using QuanLyQuanCaPhe.Services.UI;
using System.Reflection;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmBanHang : Form
    {
        private const string MenuItemCardTag = "menu-card";
        private readonly bool _isEmbedded;
        private readonly IBanService _banBUS;
        private readonly IMonService _monBUS;
        private readonly IBanHangService _banHangBUS;
        private readonly IKhachHangService _khachHangBUS;
        private readonly IPermissionService _permissionBUS;
        private readonly BanHangPresenter _banHangPresenter;
        private readonly SearchDebounceHelper _timKiemDebounce;
        private readonly Dictionary<int, int?> _khachHangChonTheoBan = new();
        private int? _banDangChonId;
        private string? _boLocLoaiMon;
        private bool _dangDongBoKhachHangUI;
        private bool _dangXuLyBanHang;
        private readonly FlowLayoutPanel _flowBoLocLoaiMon = new();
        private readonly List<Button> _cacNutBoLocLoaiMon = new();
        private Button? _nutTatCaLoaiMon;
        private Form? _childFormDangMo;
        private readonly FlowLayoutPanel _flowHinhThucPhucVu = new();
        private readonly RadioButton _rdoTaiQuan = new();
        private readonly RadioButton _rdoMangDi = new();
        private readonly ContextMenuStrip _menuNguCanhBan = new();
        private readonly ToolStripMenuItem _menuItemDonBan = new("Dọn bàn");
        private bool _cheDoMangDi;
        private bool _dangDongBoCheDoPhucVu;
        private int? _banMangDiId;
        private int? _banTaiQuanDangChonId;
        private int? _banNguCanhId;
        private readonly Button _btnKhachHangSidebar;
        private readonly Button _btnQuanLyKhoSidebar;
        private readonly Button _btnAuditLogSidebar;

        public frmBanHang(
            IBanService? banBUS = null,
            IMonService? monBUS = null,
            IBanHangService? banHangBUS = null,
            IKhachHangService? khachHangBUS = null,
            IPermissionService? permissionBUS = null,
            BanHangPresenter? banHangPresenter = null,
            bool isEmbedded = false)
        {
            _banBUS = AppServiceProvider.Resolve(banBUS, () => new BanBUS());
            _monBUS = AppServiceProvider.Resolve(monBUS, () => new MonBUS());
            _banHangBUS = AppServiceProvider.Resolve(banHangBUS, () => new BanHangBUS());
            _khachHangBUS = AppServiceProvider.Resolve(khachHangBUS, () => new KhachHangBUS());
            _permissionBUS = AppServiceProvider.Resolve(permissionBUS, () => new PermissionBUS());
            _banHangPresenter = banHangPresenter ?? new BanHangPresenter();
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
            _timKiemDebounce = new SearchDebounceHelper(250, () =>
            {
                TaiSoDoBan();
                TaiDanhSachMon();
                CapNhatPhieuDangChon();
            });

            dgvOrder.AutoGenerateColumns = false;
            colTenMon.DataPropertyName = nameof(BanHangOrderItemDTO.TenMon);
            colSoLuong.DataPropertyName = nameof(BanHangOrderItemDTO.SoLuong);
            colDonGia.DataPropertyName = nameof(BanHangOrderItemDTO.DonGiaHienThi);
            colThanhTien.DataPropertyName = nameof(BanHangOrderItemDTO.ThanhTienHienThi);
            dgvOrder.CellContentClick += dgvOrder_CellContentClick;

            Load += frmBanHang_Load;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            btnLamMoi.Click += btnLamMoi_Click;
            cboKhachHang.SelectedIndexChanged += cboKhachHang_SelectedIndexChanged;
            btnTaiLaiKhach.Click += (_, _) => TaiDanhSachKhachHang();
            btnThemNhanhKhach.Click += btnThemNhanhKhach_Click;
            KhoiTaoKhungBoLocLoaiMon();
            ToiUuGiaoDienMenu();
            KhoiTaoLuaChonHinhThucPhucVu();
            KhoiTaoMenuNguCanhBan();

            btnTamTinh.Click += btnTamTinh_Click;
            btnThanhToan.Click += btnThanhToan_Click;
            btnChuyenBan.Click += (_, _) => ThucHienChuyenHoacGopBan(true);
            btnGopBan.Click += (_, _) => ThucHienChuyenHoacGopBan(false);

            btnBanHang.Click += (_, _) => HienThiManHinhBanHang();
            btnQuanLyBan.Click += (_, _) => DieuHuongManHinhCon(btnQuanLyBan, PermissionFeatures.Menu, "Bạn không có quyền truy cập chức năng Quản lý bàn.", () => new frmQuanLiBan(isEmbedded: true));
            btnQuanLyMon.Click += (_, _) => DieuHuongManHinhCon(btnQuanLyMon, PermissionFeatures.Menu, "Bạn không có quyền truy cập chức năng Quản lý món.", () => new frmQuanLiMon(isEmbedded: true));
            btnCongThuc.Click += (_, _) => DieuHuongManHinhCon(btnCongThuc, PermissionFeatures.Menu, "Bạn không có quyền truy cập chức năng Công thức.", () => new frmCongThuc(isEmbedded: true));
            _btnQuanLyKhoSidebar.Click += (_, _) => DieuHuongManHinhCon(_btnQuanLyKhoSidebar, PermissionFeatures.NguyenLieu, "Bạn không có quyền truy cập chức năng Quản lý kho.", () => new frmQuanLiKho(isEmbedded: true));
            _btnKhachHangSidebar.Click += (_, _) => DieuHuongManHinhCon(_btnKhachHangSidebar, PermissionFeatures.KhachHang, "Bạn không có quyền truy cập chức năng Khách hàng.", () => new frmKhachHang(isEmbedded: true));
            _btnAuditLogSidebar.Click += (_, _) => DieuHuongManHinhCon(_btnAuditLogSidebar, PermissionFeatures.ThongKe, "Bạn không có quyền truy cập chức năng Audit log.", () => new frmAuditLog(isEmbedded: true));
            btnNhanVien.Click += (_, _) => DieuHuongManHinhCon(btnNhanVien, PermissionFeatures.NhanVien, "Bạn không có quyền truy cập chức năng Nhân viên.", () => new frmNhanVien(isEmbedded: true));
            btnThongKe.Click += (_, _) => DieuHuongManHinhCon(btnThongKe, PermissionFeatures.ThongKe, "Bạn không có quyền truy cập chức năng Thống kê.", () => new frmThongKe(isEmbedded: true));
            btnHoaDon.Click += (_, _) => DieuHuongManHinhCon(btnHoaDon, PermissionFeatures.HoaDon, "Bạn không có quyền truy cập chức năng Hóa đơn.", () => new frmHoaDon(isEmbedded: true));
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

        private void KhoiTaoLuaChonHinhThucPhucVu()
        {
            _flowHinhThucPhucVu.AutoSize = true;
            _flowHinhThucPhucVu.WrapContents = false;
            _flowHinhThucPhucVu.FlowDirection = FlowDirection.LeftToRight;
            _flowHinhThucPhucVu.BackColor = Color.Transparent;
            _flowHinhThucPhucVu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _flowHinhThucPhucVu.Margin = new Padding(0);

            _rdoTaiQuan.AutoSize = true;
            _rdoTaiQuan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _rdoTaiQuan.ForeColor = Color.FromArgb(79, 56, 43);
            _rdoTaiQuan.Text = "Tại quán";
            _rdoTaiQuan.Checked = true;
            _rdoTaiQuan.Margin = new Padding(0, 0, 10, 0);

            _rdoMangDi.AutoSize = true;
            _rdoMangDi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _rdoMangDi.ForeColor = Color.FromArgb(79, 56, 43);
            _rdoMangDi.Text = "Mang đi";
            _rdoMangDi.Margin = new Padding(0);

            _rdoTaiQuan.CheckedChanged += HinhThucPhucVu_CheckedChanged;
            _rdoMangDi.CheckedChanged += HinhThucPhucVu_CheckedChanged;

            _flowHinhThucPhucVu.Controls.Add(_rdoTaiQuan);
            _flowHinhThucPhucVu.Controls.Add(_rdoMangDi);
            panelTablesHeader.Controls.Add(_flowHinhThucPhucVu);
            panelTablesHeader.Resize += (_, _) => DatViTriLuaChonHinhThucPhucVu();
            DatViTriLuaChonHinhThucPhucVu();
        }

        private void DatViTriLuaChonHinhThucPhucVu()
        {
            var width = _flowHinhThucPhucVu.PreferredSize.Width;
            var height = _flowHinhThucPhucVu.PreferredSize.Height;

            _flowHinhThucPhucVu.Size = new Size(width, height);
            _flowHinhThucPhucVu.Location = new Point(
                Math.Max(0, panelTablesHeader.ClientSize.Width - width - 6),
                7);
        }

        private void KhoiTaoMenuNguCanhBan()
        {
            _menuItemDonBan.Click += async (_, _) => await DonBanDangChonAsync();
            _menuNguCanhBan.Items.Add(_menuItemDonBan);
        }

        private async Task DonBanDangChonAsync()
        {
            if (!_banNguCanhId.HasValue)
            {
                return;
            }

            var banId = _banNguCanhId.Value;
            _banDangChonId = banId;

            _ = await ThucThiTacVuBanHangAsync(
                () => _banBUS.DonBan(banId),
                hienThongBaoThanhCong: true);
        }

        private void HinhThucPhucVu_CheckedChanged(object? sender, EventArgs e)
        {
            if (_dangDongBoCheDoPhucVu)
            {
                return;
            }

            ApDungCheDoPhucVu(_rdoMangDi.Checked);
        }

        private void ApDungCheDoPhucVu(bool laMangDi)
        {
            if (laMangDi)
            {
                if (!_banMangDiId.HasValue)
                {
                    _banMangDiId = _banBUS.LayHoacTaoBanMangDi();
                }

                if (!_banMangDiId.HasValue || _banMangDiId.Value <= 0)
                {
                    MessageBox.Show("Không thể khởi tạo bàn mang đi. Vui lòng thử lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    _dangDongBoCheDoPhucVu = true;
                    try
                    {
                        _rdoTaiQuan.Checked = true;
                        _rdoMangDi.Checked = false;
                    }
                    finally
                    {
                        _dangDongBoCheDoPhucVu = false;
                    }

                    laMangDi = false;
                }
            }

            _cheDoMangDi = laMangDi;

            if (_cheDoMangDi)
            {
                if (_banDangChonId.HasValue && (!_banMangDiId.HasValue || _banDangChonId.Value != _banMangDiId.Value))
                {
                    _banTaiQuanDangChonId = _banDangChonId;
                }

                _banDangChonId = _banMangDiId;
            }
            else if (_banMangDiId.HasValue && _banDangChonId == _banMangDiId)
            {
                _banDangChonId = _banTaiQuanDangChonId;
            }

            flowBan.Visible = !_cheDoMangDi;
            btnChuyenBan.Enabled = !_cheDoMangDi && btnChuyenBan.Visible;
            btnGopBan.Enabled = !_cheDoMangDi && btnGopBan.Visible;

            TaiSoDoBan();
            CapNhatPhieuDangChon();
            DatTrangThaiDangXuLyBanHang(_dangXuLyBanHang);
        }

        private void frmBanHang_Load(object? sender, EventArgs e)
        {
            if (ChildFormRuntimePolicy.TryBlockStandalone(this, _isEmbedded, "Ban hang"))
            {
                return;
            }

            if (!_isEmbedded)
            {
                WindowState = FormWindowState.Maximized;
            }

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

            if (_isEmbedded)
            {
                EmbeddedFormLayoutHelper.UseContentOnlyLayout(panelMain, panelSidebar, panelTopbar);
            }

            HienThiNguoiDungDangNhap();
            ApplyPermissionUI();
            CapNhatNutMenuDangChon(btnBanHang);
            TaiDanhSachKhachHang();

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

            _dangDongBoCheDoPhucVu = true;
            try
            {
                _rdoTaiQuan.Checked = true;
                _rdoMangDi.Checked = false;
            }
            finally
            {
                _dangDongBoCheDoPhucVu = false;
            }

            ApDungCheDoPhucVu(laMangDi: false);
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

            var coQuyenBanHang = _permissionBUS.CanSell();
            var coQuyenCapNhatBanHang = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update);
            btnTamTinh.Visible = coQuyenBanHang;
            btnThanhToan.Visible = coQuyenBanHang;
            btnChuyenBan.Visible = coQuyenBanHang && coQuyenCapNhatBanHang;
            btnGopBan.Visible = coQuyenBanHang && coQuyenCapNhatBanHang;
            btnThemNhanhKhach.Visible = coQuyenBanHang && coQuyenCapNhatBanHang;
        }

        private void TaiDanhSachKhachHang()
        {
            var khachDangChon = LayKhachHangDangChonId();

            var dsLuaChon = new List<KhachHangLuaChonItem>
            {
                new() { Id = 0, TenHienThi = "Khách lẻ" }
            };

            var dsKhach = _khachHangBUS
                .LayDanhSachKhachChoBanHang(null)
                .OrderBy(x => x.HoVaTen)
                .ThenBy(x => x.ID)
                .ToList();

            dsLuaChon.AddRange(dsKhach.Select(x => new KhachHangLuaChonItem
            {
                Id = x.ID,
                TenHienThi = TaoTenKhachHangHienThi(x)
            }));

            _dangDongBoKhachHangUI = true;
            try
            {
                cboKhachHang.DataSource = null;
                cboKhachHang.DisplayMember = nameof(KhachHangLuaChonItem.TenHienThi);
                cboKhachHang.ValueMember = nameof(KhachHangLuaChonItem.Id);
                cboKhachHang.DataSource = dsLuaChon;
            }
            finally
            {
                _dangDongBoKhachHangUI = false;
            }

            ChonKhachHangTheoId(khachDangChon);
        }

        private static string TaoTenKhachHangHienThi(KhachHangDTO khachHang)
        {
            var tenKhach = string.IsNullOrWhiteSpace(khachHang.HoVaTen)
                ? $"Khách #{khachHang.ID}"
                : khachHang.HoVaTen.Trim();

            return string.IsNullOrWhiteSpace(khachHang.DienThoai)
                ? tenKhach
                : $"{tenKhach} • {khachHang.DienThoai}";
        }

        private void cboKhachHang_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangDongBoKhachHangUI || !_banDangChonId.HasValue)
            {
                return;
            }

            _khachHangChonTheoBan[_banDangChonId.Value] = LayKhachHangDangChonId();
        }

        private int? LayKhachHangDangChonId()
        {
            if (cboKhachHang.SelectedValue is int khachId && khachId > 0)
            {
                return khachId;
            }

            if (cboKhachHang.SelectedItem is KhachHangLuaChonItem luaChon && luaChon.Id > 0)
            {
                return luaChon.Id;
            }

            return null;
        }

        private void ChonKhachHangTheoId(int? khachHangId)
        {
            if (cboKhachHang.DataSource == null)
            {
                return;
            }

            var giaTriKhach = khachHangId.HasValue && khachHangId.Value > 0
                ? khachHangId.Value
                : 0;

            var coKhachTrongDanhSach = cboKhachHang.Items
                .OfType<KhachHangLuaChonItem>()
                .Any(x => x.Id == giaTriKhach);

            _dangDongBoKhachHangUI = true;
            try
            {
                cboKhachHang.SelectedValue = coKhachTrongDanhSach ? giaTriKhach : 0;
            }
            finally
            {
                _dangDongBoKhachHangUI = false;
            }
        }

        private void DongBoKhachHangTheoTrangThaiBan(int banId, BanHangTrangThaiPhieuDTO trangThaiPhieu)
        {
            if (trangThaiPhieu.HoaDonID.HasValue)
            {
                _khachHangChonTheoBan[banId] = trangThaiPhieu.KhachHangID;
                ChonKhachHangTheoId(trangThaiPhieu.KhachHangID);
                return;
            }

            if (!_khachHangChonTheoBan.TryGetValue(banId, out var khachHangTam))
            {
                khachHangTam = null;
                _khachHangChonTheoBan[banId] = null;
            }

            ChonKhachHangTheoId(khachHangTam);
        }

        private void DieuHuongManHinhCon(
            Button nutMenu,
            string feature,
            string thongBaoTuChoi,
            Func<Form> childFormFactory)
        {
            if (_isEmbedded)
            {
                return;
            }

            try
            {
                _permissionBUS.EnsurePermission(feature, PermissionActions.View, thongBaoTuChoi);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var childForm = childFormFactory();
            OpenChildForm(childForm);
            lblPageTitle.Text = _childFormDangMo?.Text ?? childForm.Text;
            txtSearch.Visible = false;
            btnLamMoi.Visible = false;
            CapNhatNutMenuDangChon(nutMenu);
        }

        public void OpenChildForm(Form childForm)
        {
            if (_isEmbedded)
            {
                childForm.Dispose();
                return;
            }

            if (_childFormDangMo != null && !_childFormDangMo.IsDisposed && _childFormDangMo.GetType() == childForm.GetType())
            {
                childForm.Dispose();
                return;
            }

            DongChildFormDangMo();

            panelContent.AutoScroll = false;
            tableMain.Visible = false;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.FormClosed += ChildFormDangMo_FormClosed;

            panelContent.Controls.Add(childForm);
            panelContent.Tag = childForm;
            _childFormDangMo = childForm;

            childForm.BringToFront();
            childForm.Visible = true;
        }

        public void HienThiManHinhBanHang()
        {
            HienThiManHinhBanHangInternal(dongFormConDangMo: true);
        }

        private void HienThiManHinhBanHangInternal(bool dongFormConDangMo)
        {
            if (dongFormConDangMo)
            {
                DongChildFormDangMo();
            }

            panelContent.AutoScroll = true;
            tableMain.Visible = true;
            tableMain.BringToFront();

            txtSearch.Visible = true;
            btnLamMoi.Visible = true;
            lblPageTitle.Text = "Quầy bán hàng";
            CapNhatNutMenuDangChon(btnBanHang);
        }

        private void CapNhatNutMenuDangChon(Button nutDangChon)
        {
            SidebarUiHelper.HighlightSidebarSelection(
                nutDangChon,
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
        }

        private void DongChildFormDangMo()
        {
            if (_childFormDangMo == null)
            {
                return;
            }

            var childForm = _childFormDangMo;
            _childFormDangMo = null;

            childForm.FormClosed -= ChildFormDangMo_FormClosed;

            if (panelContent.Controls.Contains(childForm))
            {
                panelContent.Controls.Remove(childForm);
            }

            if (!childForm.IsDisposed)
            {
                childForm.Close();
                childForm.Dispose();
            }

            panelContent.Tag = null;
        }

        private void ChildFormDangMo_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (sender is not Form childForm)
            {
                return;
            }

            childForm.FormClosed -= ChildFormDangMo_FormClosed;

            if (!ReferenceEquals(_childFormDangMo, childForm))
            {
                return;
            }

            _childFormDangMo = null;
            panelContent.Tag = null;

            if (panelContent.Controls.Contains(childForm))
            {
                panelContent.Controls.Remove(childForm);
            }

            HienThiManHinhBanHangInternal(dongFormConDangMo: false);
        }

        private void txtSearch_TextChanged(object? sender, EventArgs e)
        {
            _timKiemDebounce.Signal();
        }

        private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            _timKiemDebounce.Flush();
            e.SuppressKeyPress = true;
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyBanHang)
            {
                return;
            }

            txtSearch.Clear();

            if (_nutTatCaLoaiMon != null)
            {
                ChonBoLocLoaiMon(null, _nutTatCaLoaiMon);
            }
            else
            {
                _boLocLoaiMon = null;
            }

            _timKiemDebounce.Flush();
        }

        private void TaiSoDoBan()
        {
            if (_cheDoMangDi)
            {
                flowBan.SuspendLayout();
                flowBan.Controls.Clear();
                flowBan.Controls.Add(TaoLabelRong("Đang ở chế độ mang đi. Không cần chọn bàn tại quán."));
                flowBan.ResumeLayout();

                if (_banMangDiId.HasValue)
                {
                    _banDangChonId = _banMangDiId;
                }

                return;
            }

            var tuKhoa = txtSearch.Text.Trim();
            var dsBan = _banBUS.LayDanhSachBan(null, null, tuKhoa);
            if (_banHangPresenter.ShouldFallbackToUnfilteredData(tuKhoa, dsBan.Count))
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
            var trangThaiHienThi = XacDinhTrangThaiHienThiBan(ban);
            var trangThaiNgan = ChuyenTrangThaiBanNgan(trangThaiHienThi);
            var bangMau = LayBangMauTrangThaiBan(trangThaiHienThi);

            var btn = new Button
            {
                Width = 95,
                Height = 80,
                Margin = new Padding(8),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", dangChon ? 10F : 9.5F, dangChon ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = bangMau.Chu,
                BackColor = bangMau.Nen,
                Text = $"{ban.TenBan}\r\n{trangThaiNgan}",
                Tag = ban,
                UseVisualStyleBackColor = false
            };

            btn.FlatAppearance.BorderSize = dangChon ? 2 : 1;
            btn.FlatAppearance.BorderColor = dangChon ? bangMau.VienDangChon : bangMau.Vien;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(255, bangMau.Nen.R + 8),
                Math.Min(255, bangMau.Nen.G + 8),
                Math.Min(255, bangMau.Nen.B + 8));
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(
                Math.Max(0, bangMau.Nen.R - 8),
                Math.Max(0, bangMau.Nen.G - 8),
                Math.Max(0, bangMau.Nen.B - 8));

            btn.Click += BtnBan_Click;
            btn.MouseUp += BtnBan_MouseUp;
            return btn;
        }

        private static string ChuyenTrangThaiBanNgan(int trangThai)
        {
            return trangThai switch
            {
                1 => "Có khách",
                2 => "Đã thanh toán",
                _ => "Trống"
            };
        }

        private static int XacDinhTrangThaiHienThiBan(BanDTO ban)
        {
            if (ban.TrangThai == 0 && !ban.CoHoaDonDangHoatDong)
            {
                return 0;
            }

            if (ban.TrangThaiHoaDonDangHoatDong == (int)HoaDonTrangThai.Paid)
            {
                return 2;
            }

            if (ban.TrangThaiHoaDonDangHoatDong == (int)HoaDonTrangThai.Draft)
            {
                return 1;
            }

            if (ban.TrangThai == 2)
            {
                return 2;
            }

            return ban.TrangThai == 0 ? 0 : 1;
        }

        private static (Color Nen, Color Chu, Color Vien, Color VienDangChon) LayBangMauTrangThaiBan(int trangThai)
        {
            return trangThai switch
            {
                1 => (
                    Nen: Color.FromArgb(255, 230, 227),
                    Chu: Color.FromArgb(169, 42, 29),
                    Vien: Color.FromArgb(243, 168, 160),
                    VienDangChon: Color.FromArgb(213, 65, 50)),
                2 => (
                    Nen: Color.FromArgb(255, 246, 215),
                    Chu: Color.FromArgb(143, 106, 0),
                    Vien: Color.FromArgb(232, 210, 137),
                    VienDangChon: Color.FromArgb(195, 138, 0)),
                _ => (
                    Nen: Color.FromArgb(232, 247, 236),
                    Chu: Color.FromArgb(31, 106, 58),
                    Vien: Color.FromArgb(159, 215, 174),
                    VienDangChon: Color.FromArgb(47, 141, 78))
            };
        }

        private void BtnBan_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyBanHang)
            {
                return;
            }

            if (_cheDoMangDi || sender is not Button { Tag: BanDTO ban })
            {
                return;
            }

            _banDangChonId = ban.ID;
            _banTaiQuanDangChonId = ban.ID;
            TaiSoDoBan();
            CapNhatPhieuDangChon();
        }

        private void BtnBan_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_cheDoMangDi || e.Button != MouseButtons.Right)
            {
                return;
            }

            if (sender is not Button { Tag: BanDTO ban })
            {
                return;
            }

            _banNguCanhId = ban.ID;
            _menuItemDonBan.Enabled = ban.TrangThai != 0 || ban.CoHoaDonDangHoatDong;
            _menuNguCanhBan.Show((Control)sender, e.Location);
        }

        private void TaiDanhSachMon()
        {
            var tuKhoa = txtSearch.Text.Trim();
            var dsMon = _banHangBUS.LocMonPhuHopBanHang(_monBUS.LayDanhSachMon(tuKhoa, null), _boLocLoaiMon);
            var chieuRongTheMon = TinhChieuRongTheMon();

            if (_banHangPresenter.ShouldFallbackToUnfilteredData(tuKhoa, dsMon.Count))
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

        private async void BtnThemMon_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyBanHang)
            {
                return;
            }

            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thêm món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (sender is not Button { Tag: MonDTO mon })
            {
                return;
            }

            _ = await ThucThiTacVuBanHangAsync(() => _banHangBUS.ThemMonVaoGioTam(_banDangChonId.Value, mon));
        }

        private async void dgvOrder_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colXoa.Index)
            {
                return;
            }

            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi xóa món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvOrder.Rows[e.RowIndex].DataBoundItem is not BanHangOrderItemDTO monDaChon
                || monDaChon.MonID <= 0
                || monDaChon.SoLuong <= 0)
            {
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Xóa món '{monDaChon.TenMon}' khỏi order hiện tại?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            _ = await ThucThiTacVuBanHangAsync(
                () => _banHangBUS.XoaMonKhoiBan(_banDangChonId.Value, monDaChon.MonID, monDaChon.DonGia, monDaChon.SoLuong));
        }

        private async void btnThemNhanhKhach_Click(object? sender, EventArgs e)
        {
            if (_dangXuLyBanHang)
            {
                return;
            }

            using var dialog = new Form
            {
                Text = "Thêm nhanh khách hàng",
                StartPosition = FormStartPosition.CenterParent,
                Width = 420,
                Height = 255,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblHoTen = new Label { Left = 18, Top = 22, Width = 95, Text = "Họ và tên" };
            var txtHoTen = new TextBox { Left = 118, Top = 18, Width = 270 };

            var lblDienThoai = new Label { Left = 18, Top = 62, Width = 95, Text = "Điện thoại" };
            var txtDienThoai = new TextBox { Left = 118, Top = 58, Width = 270 };

            var lblDiaChi = new Label { Left = 18, Top = 102, Width = 95, Text = "Địa chỉ" };
            var txtDiaChi = new TextBox { Left = 118, Top = 98, Width = 270 };

            var btnLuu = new Button
            {
                Left = 226,
                Top = 152,
                Width = 78,
                Text = "Lưu",
                DialogResult = DialogResult.OK
            };

            var btnHuy = new Button
            {
                Left = 310,
                Top = 152,
                Width = 78,
                Text = "Hủy",
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.AddRange(new Control[]
            {
                lblHoTen, txtHoTen,
                lblDienThoai, txtDienThoai,
                lblDiaChi, txtDiaChi,
                btnLuu, btnHuy
            });

            dialog.AcceptButton = btnLuu;
            dialog.CancelButton = btnHuy;

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var khachMoi = new KhachHangDTO
            {
                HoVaTen = txtHoTen.Text,
                DienThoai = txtDienThoai.Text,
                DiaChi = txtDiaChi.Text
            };

            if (!ThuBatDauXuLyBanHang())
            {
                return;
            }

            (bool ThanhCong, string ThongBao, KhachHangDTO? KhachMoi) ketQua;
            try
            {
                ketQua = await Task.Run(() => _khachHangBUS.ThemKhachNhanhChoBanHang(khachMoi));
            }
            finally
            {
                KetThucXuLyBanHang();
            }

            if (!ketQua.ThanhCong)
            {
                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSachKhachHang();

            if (ketQua.KhachMoi?.ID > 0)
            {
                ChonKhachHangTheoId(ketQua.KhachMoi.ID);
                if (_banDangChonId.HasValue)
                {
                    _khachHangChonTheoBan[_banDangChonId.Value] = ketQua.KhachMoi.ID;
                }
            }

            MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CapNhatPhieuDangChon()
        {
            if (!_banDangChonId.HasValue)
            {
                dgvOrder.DataSource = null;
                lblOrderMeta.Text = _cheDoMangDi ? "Đơn mang đi" : "Chưa chọn bàn";
                lblThongTinBan.Text = _cheDoMangDi
                    ? "Chọn món để tạo đơn mang đi"
                    : "Chọn bàn để xem món đang phục vụ";
                lblTamTinhValue.Text = DinhDangTien(0);
                lblGiamGiaValue.Text = DinhDangTien(0);
                lblTongThanhToanValue.Text = DinhDangTien(0);
                cboKhachHang.Enabled = false;
                btnThemNhanhKhach.Enabled = false;
                ChonKhachHangTheoId(null);
                return;
            }

            var banId = _banDangChonId.Value;
            var trangThaiPhieu = _banHangBUS.LayTrangThaiPhieuTheoBan(banId);
            DongBoKhachHangTheoTrangThaiBan(banId, trangThaiPhieu);
            cboKhachHang.Enabled = true;
            btnThemNhanhKhach.Enabled = true;

            dgvOrder.DataSource = null;
            dgvOrder.DataSource = trangThaiPhieu.ChiTietHienThi;

            var viewModel = _banHangPresenter.BuildOrderViewModel(trangThaiPhieu, _cheDoMangDi);
            lblOrderMeta.Text = viewModel.OrderMeta;
            lblThongTinBan.Text = viewModel.TableInfo;
            lblTamTinhValue.Text = DinhDangTien(viewModel.TongTien);
            lblGiamGiaValue.Text = DinhDangTien(0);
            lblTongThanhToanValue.Text = DinhDangTien(viewModel.TongTien);
        }

        private async Task<bool> ThucThiTacVuBanHangAsync(
            Func<BanActionResultDTO> tacVu,
            bool hienThongBaoThanhCong = false,
            string? thongBaoThanhCong = null)
        {
            if (!ThuBatDauXuLyBanHang())
            {
                return false;
            }

            try
            {
                var result = await Task.Run(tacVu);
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
            finally
            {
                KetThucXuLyBanHang();
            }
        }

        private bool ThuBatDauXuLyBanHang()
        {
            if (_dangXuLyBanHang)
            {
                return false;
            }

            _dangXuLyBanHang = true;
            DatTrangThaiDangXuLyBanHang(true);
            return true;
        }

        private void KetThucXuLyBanHang()
        {
            _dangXuLyBanHang = false;
            DatTrangThaiDangXuLyBanHang(false);
        }

        private void DatTrangThaiDangXuLyBanHang(bool dangXuLy)
        {
            UiLoadingStateHelper.Apply(
                this,
                dangXuLy,
                btnTamTinh,
                btnThanhToan,
                btnChuyenBan,
                btnGopBan,
                cboKhachHang,
                btnTaiLaiKhach,
                btnThemNhanhKhach,
                flowBan,
                txtSearch,
                btnLamMoi,
                flowMon);

            var hoaDonDaThanhToan = false;
            if (!dangXuLy && _banDangChonId.HasValue)
            {
                var trangThaiPhieu = _banHangBUS.LayTrangThaiPhieuTheoBan(_banDangChonId.Value);
                hoaDonDaThanhToan = trangThaiPhieu.TrangThaiHoaDon == (int)HoaDonTrangThai.Paid;
            }

            btnTamTinh.Enabled = !dangXuLy && _banDangChonId.HasValue && !_cheDoMangDi && !hoaDonDaThanhToan;
            btnThanhToan.Enabled = !dangXuLy && _banDangChonId.HasValue && !hoaDonDaThanhToan;
            btnChuyenBan.Enabled = !dangXuLy && !_cheDoMangDi && btnChuyenBan.Visible;
            btnGopBan.Enabled = !dangXuLy && !_cheDoMangDi && btnGopBan.Visible;

            cboKhachHang.Enabled = !dangXuLy && _banDangChonId.HasValue;
            btnTaiLaiKhach.Enabled = !dangXuLy;
            btnThemNhanhKhach.Enabled = !dangXuLy && _banDangChonId.HasValue;
            flowBan.Enabled = !dangXuLy && !_cheDoMangDi;
        }

        private async void btnTamTinh_Click(object? sender, EventArgs e)
        {
            if (_cheDoMangDi)
            {
                MessageBox.Show("Đơn mang đi không hỗ trợ lưu bàn. Vui lòng bấm Thanh toán liền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!_banDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi lưu bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var banId = _banDangChonId.Value;
            var khachHangId = LayKhachHangDangChonId();
            _khachHangChonTheoBan[banId] = khachHangId;

            _ = await ThucThiTacVuBanHangAsync(
                () => _banHangBUS.LuuMonChoGoiVoiKhachHang(banId, khachHangId),
                hienThongBaoThanhCong: true,
                thongBaoThanhCong: "Đã lưu bàn thành công. Bạn có thể mở lại để thêm món sau.");
        }

        private async void btnThanhToan_Click(object? sender, EventArgs e)
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

            var khachHangId = LayKhachHangDangChonId();
            _khachHangChonTheoBan[banId] = khachHangId;

            _ = await ThucThiTacVuBanHangAsync(
                () => _banHangBUS.ThanhToanHoaDonVoiKhachHang(banId, khachHangId),
                hienThongBaoThanhCong: true);
        }

        private async void ThucHienChuyenHoacGopBan(bool laChuyenBan)
        {
            if (!_permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update))
            {
                MessageBox.Show(
                    laChuyenBan ? "Bạn không có quyền chuyển bàn." : "Bạn không có quyền gộp bàn.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (_cheDoMangDi)
            {
                MessageBox.Show("Không áp dụng chuyển/gộp bàn khi đang ở chế độ mang đi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
                .Where(x => laChuyenBan
                    ? (x.TrangThai == 0 && !x.CoHoaDonDangHoatDong)
                    : x.TrangThaiHoaDonDangHoatDong == (int)HoaDonTrangThai.Draft)
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

            if (!ThuBatDauXuLyBanHang())
            {
                return;
            }

            BanActionResultDTO result;
            try
            {
                result = await Task.Run(() => _banBUS.ChuyenHoacGopBan(new BanChuyenGopRequestDTO
                {
                    BanNguonId = banNguonId,
                    BanDichId = banDichId,
                    LaChuyenBan = laChuyenBan
                }));
            }
            finally
            {
                KetThucXuLyBanHang();
            }

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

        private sealed class KhachHangLuaChonItem
        {
            public int Id { get; init; }
            public string TenHienThi { get; init; } = string.Empty;
        }

        private void panelTopbar_Paint(object sender, PaintEventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DongChildFormDangMo();
            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timKiemDebounce.Dispose();
            base.OnFormClosed(e);
        }
    }
}
