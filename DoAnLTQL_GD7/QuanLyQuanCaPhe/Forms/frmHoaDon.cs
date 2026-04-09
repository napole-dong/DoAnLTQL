using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.HoaDon;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmHoaDon : Form
    {
        private readonly HoaDonBUS _hoaDonBUS = new();
        private readonly PermissionBUS _permissionBUS = new();
        private readonly HoaDonPreviewService _hoaDonPreviewService = new();
        private readonly HoaDonTienService _hoaDonTienService = new();
        private readonly HoaDonFormStateService _hoaDonFormStateService = new();

        private HoaDonManHinhState _manHinhState = HoaDonManHinhState.Xem;
        private bool _dangNapDuLieu;
        private int? _hoaDonDangChonId;
        private decimal _tongTienDangChon;

        private sealed class TrangThaiHoaDonOption
        {
            public int Value { get; set; }
            public string Text { get; set; } = string.Empty;
        }

        public frmHoaDon()
        {
            InitializeComponent();
            CauHinhBangDuLieu();
            CauHinhSuKien();
        }

        private void CauHinhBangDuLieu()
        {
            dgvDanhSachHoaDon.AutoGenerateColumns = false;
            colMaHoaDon.DataPropertyName = nameof(HoaDonDTO.MaHoaDonHienThi);
            colNgayLap.DataPropertyName = nameof(HoaDonDTO.NgayLapHienThi);
            colBanKhach.DataPropertyName = nameof(HoaDonDTO.BanKhachHienThi);
            colTongTien.DataPropertyName = nameof(HoaDonDTO.TongTienHienThi);
            colTrangThaiHoaDon.DataPropertyName = nameof(HoaDonDTO.TrangThaiText);
            colNhanVienLap.DataPropertyName = nameof(HoaDonDTO.TenNhanVien);

            dgvChiTietHoaDon.AutoGenerateColumns = false;
            colTenMon.DataPropertyName = nameof(HoaDonChiTietDTO.TenMon);
            colSoLuong.DataPropertyName = nameof(HoaDonChiTietDTO.SoLuong);
            colDonGia.DataPropertyName = nameof(HoaDonChiTietDTO.DonGiaHienThi);
            colThanhTien.DataPropertyName = nameof(HoaDonChiTietDTO.ThanhTienHienThi);

            cboBanKhach.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMon.DropDownStyle = ComboBoxStyle.DropDownList;

            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = short.MaxValue;
        }

        private void CauHinhSuKien()
        {
            Load += frmHoaDon_Load;
            dgvDanhSachHoaDon.SelectionChanged += dgvDanhSachHoaDon_SelectionChanged;
            txtTimKiemHoaDon.KeyDown += txtTimKiemHoaDon_KeyDown;
            btnLocXem.Click += (_, _) => TaiDanhSachHoaDon();
            btnLamMoi.Click += btnLamMoi_Click;

            btnThemMoi.Click += btnThemMoi_Click;
            btnSua.Click += btnSua_Click;
            btnLuu.Click += btnLuu_Click;
            btnBoQua.Click += btnBoQua_Click;
            btnXoaHuy.Click += btnXoaHuy_Click;

            btnThemMonVaoHoaDon.Click += btnThemMonVaoHoaDon_Click;
            btnXacNhanThuTien.Click += btnXacNhanThuTien_Click;
            btnInHoaDon.Click += btnInHoaDon_Click;

            txtTienKhachDua.TextChanged += txtTienKhachDua_TextChanged;

            btnBanHang.Click += (_, _) => OpenStandaloneForm(new frmBanHang(), PermissionFeatures.BanHang);
            btnQuanLyBan.Click += (_, _) => OpenStandaloneForm(new frmQuanLiBan(), PermissionFeatures.Menu);
            btnQuanLyMon.Click += (_, _) => OpenStandaloneForm(new frmQuanLiMon(), PermissionFeatures.Menu);
            btnKhachHang.Click += (_, _) => OpenStandaloneForm(new frmKhachHang(), PermissionFeatures.Menu);
            btnNhanVien.Click += (_, _) => OpenStandaloneForm(new frmNhanVien(), PermissionFeatures.NhanVien);
            btnHoaDon.Click += (_, _) => OpenStandaloneForm(new frmHoaDon(), PermissionFeatures.HoaDon);
            btnThongKe.Click += (_, _) => MoTinhNangThongKe();
            btnDangXuat.Click += (_, _) => DangXuatDieuHuongService.DangXuatVaQuayVeDangNhap();
        }

        private void frmHoaDon_Load(object? sender, EventArgs e)
        {
            try
            {
                _permissionBUS.EnsurePermission(PermissionFeatures.HoaDon, PermissionActions.View, "Ban khong co quyen truy cap chuc nang hoa don.");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            HienThiNguoiDungDangNhap();
            ApDungPhanQuyenDieuHuong();
            KhoiTaoBoLocMacDinh();
            KhoiTaoComboTrangThai();
            TaiDanhSachBanKhach();
            TaiDanhSachMon();
            TaiDanhSachHoaDon(giuHoaDonDangChon: false);
            ChuyenStateManHinh(HoaDonManHinhState.Xem);
        }

        private void ApDungPhanQuyenDieuHuong()
        {
            if (_permissionBUS.IsAdmin())
            {
                btnBanHang.Visible = true;
                btnQuanLyBan.Visible = true;
                btnQuanLyMon.Visible = true;
                btnKhachHang.Visible = true;
                btnNhanVien.Visible = true;
                btnHoaDon.Visible = true;
                btnThongKe.Visible = true;
                return;
            }

            var coQuyenMenu = _permissionBUS.CheckPermission(PermissionFeatures.Menu, PermissionActions.View);
            btnBanHang.Visible = _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.View);
            btnQuanLyBan.Visible = coQuyenMenu;
            btnQuanLyMon.Visible = coQuyenMenu;
            btnKhachHang.Visible = coQuyenMenu;
            btnNhanVien.Visible = _permissionBUS.CheckPermission(PermissionFeatures.NhanVien, PermissionActions.View);
            btnHoaDon.Visible = _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);
            btnThongKe.Visible = _permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View);
        }

        private void HienThiNguoiDungDangNhap()
        {
        }

        private void KhoiTaoBoLocMacDinh()
        {
            dtpTuNgay.Value = DateTime.Today.AddDays(-30);
            dtpDenNgay.Value = DateTime.Today;
            cboTrangThaiLoc.SelectedIndex = 0;
        }

        private void KhoiTaoComboTrangThai()
        {
            var dsTrangThai = new List<TrangThaiHoaDonOption>
            {
                new() { Value = 0, Text = HoaDonBUS.ChuyenTrangThaiHoaDon(0) },
                new() { Value = 1, Text = HoaDonBUS.ChuyenTrangThaiHoaDon(1) },
                new() { Value = 2, Text = HoaDonBUS.ChuyenTrangThaiHoaDon(2) }
            };

            cboTrangThai.DataSource = dsTrangThai;
            cboTrangThai.DisplayMember = nameof(TrangThaiHoaDonOption.Text);
            cboTrangThai.ValueMember = nameof(TrangThaiHoaDonOption.Value);
            cboTrangThai.SelectedValue = 0;
            cboTrangThai.Enabled = false;
        }

        private void TaiDanhSachBanKhach()
        {
            var dsBanKhach = _hoaDonBUS.LayDanhSachBanKhach();

            cboBanKhach.DataSource = dsBanKhach;
            cboBanKhach.DisplayMember = nameof(HoaDonBanKhachItemDTO.TenHienThi);
            cboBanKhach.ValueMember = nameof(HoaDonBanKhachItemDTO.BanID);

            if (dsBanKhach.Count > 0)
            {
                cboBanKhach.SelectedIndex = 0;
            }
        }

        private void TaiDanhSachMon()
        {
            var dsMon = _hoaDonBUS.LayDanhSachMonDangKinhDoanh();

            cboMon.DataSource = dsMon;
            cboMon.DisplayMember = nameof(HoaDonMonItemDTO.TenHienThi);
            cboMon.ValueMember = nameof(HoaDonMonItemDTO.MonID);

            if (dsMon.Count > 0)
            {
                cboMon.SelectedIndex = 0;
            }
        }

        private void TaiDanhSachHoaDon(bool giuHoaDonDangChon = true)
        {
            var boLoc = new HoaDonFilterDTO
            {
                TuKhoa = txtTimKiemHoaDon.Text,
                TuNgay = dtpTuNgay.Value,
                DenNgay = dtpDenNgay.Value,
                TrangThai = HoaDonBUS.ChuyenTextSangTrangThaiLoc(cboTrangThaiLoc.SelectedItem?.ToString())
            };

            var dsHoaDon = _hoaDonBUS.LayDanhSachHoaDon(boLoc);

            _dangNapDuLieu = true;
            try
            {
                dgvDanhSachHoaDon.DataSource = null;
                dgvDanhSachHoaDon.DataSource = dsHoaDon;

                var mucTieuId = giuHoaDonDangChon ? _hoaDonDangChonId : null;

                var daChonHoaDonMucTieu = mucTieuId.HasValue && ChonDongHoaDon(mucTieuId.Value);

                if (!daChonHoaDonMucTieu && dgvDanhSachHoaDon.Rows.Count > 0)
                {
                    dgvDanhSachHoaDon.Rows[0].Selected = true;
                    dgvDanhSachHoaDon.CurrentCell = dgvDanhSachHoaDon.Rows[0].Cells[0];
                }
                else if (dgvDanhSachHoaDon.Rows.Count == 0)
                {
                    _hoaDonDangChonId = null;
                    HienThiTrangThaiKhongCoHoaDon();
                }
            }
            finally
            {
                _dangNapDuLieu = false;
            }

            TaiChiTietTheoDongDangChon();
        }

        private bool ChonDongHoaDon(int hoaDonId)
        {
            foreach (DataGridViewRow row in dgvDanhSachHoaDon.Rows)
            {
                if (row.DataBoundItem is not HoaDonDTO hoaDon || hoaDon.ID != hoaDonId)
                {
                    continue;
                }

                row.Selected = true;
                dgvDanhSachHoaDon.CurrentCell = row.Cells[0];
                return true;
            }

            return false;
        }

        private void dgvDanhSachHoaDon_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            TaiChiTietTheoDongDangChon();
        }

        private void TaiChiTietTheoDongDangChon()
        {
            if (dgvDanhSachHoaDon.CurrentRow?.DataBoundItem is not HoaDonDTO hoaDonGrid)
            {
                _hoaDonDangChonId = null;
                HienThiTrangThaiKhongCoHoaDon();
                return;
            }

            _hoaDonDangChonId = hoaDonGrid.ID;
            var hoaDon = _hoaDonBUS.LayHoaDonTheoId(hoaDonGrid.ID);
            if (hoaDon == null)
            {
                HienThiTrangThaiKhongCoHoaDon();
                return;
            }

            _dangNapDuLieu = true;
            try
            {
                txtMaHoaDon.Text = hoaDon.MaHoaDonHienThi;
                dtpNgayTao.Value = hoaDon.NgayLap;

                if (cboBanKhach.Items.Count > 0)
                {
                    cboBanKhach.SelectedValue = hoaDon.BanID;
                }

                cboTrangThai.SelectedValue = hoaDon.TrangThai;

                dgvChiTietHoaDon.DataSource = null;
                dgvChiTietHoaDon.DataSource = hoaDon.ChiTiet;

                _tongTienDangChon = hoaDon.TongTien;
                lblTongTienValue.Text = hoaDon.TongTienHienThi;

                txtTienKhachDua.Clear();
                lblTienThoiValue.Text = _hoaDonTienService.DinhDangTien(0);

                CapNhatDieuKienXuLyNut(hoaDon);
            }
            finally
            {
                _dangNapDuLieu = false;
            }
        }

        private void HienThiTrangThaiKhongCoHoaDon()
        {
            txtMaHoaDon.Text = string.Empty;
            dtpNgayTao.Value = DateTime.Now;
            cboTrangThai.SelectedValue = 0;

            dgvChiTietHoaDon.DataSource = null;
            _tongTienDangChon = 0;
            lblTongTienValue.Text = _hoaDonTienService.DinhDangTien(0);
            txtTienKhachDua.Clear();
            lblTienThoiValue.Text = _hoaDonTienService.DinhDangTien(0);

            CapNhatDieuKienXuLyNut(null);
        }

        private void txtTimKiemHoaDon_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            TaiDanhSachHoaDon();
            e.SuppressKeyPress = true;
        }

        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimKiemHoaDon.Clear();
            KhoiTaoBoLocMacDinh();
            TaiDanhSachHoaDon(giuHoaDonDangChon: false);
        }

        private void btnThemMoi_Click(object? sender, EventArgs e)
        {
            ChuyenStateManHinh(HoaDonManHinhState.ThemMoi);
            HienThiManHinhTaoMoi();
        }

        private void HienThiManHinhTaoMoi()
        {
            _dangNapDuLieu = true;
            try
            {
                _hoaDonDangChonId = null;
                txtMaHoaDon.Text = HoaDonBUS.DinhDangMaHoaDon(_hoaDonBUS.LayMaHoaDonTiepTheo());
                dtpNgayTao.Value = DateTime.Now;

                if (cboBanKhach.Items.Count > 0)
                {
                    cboBanKhach.SelectedIndex = 0;
                }

                cboTrangThai.SelectedValue = 0;

                dgvChiTietHoaDon.DataSource = null;
                _tongTienDangChon = 0;
                lblTongTienValue.Text = _hoaDonTienService.DinhDangTien(0);
                txtTienKhachDua.Clear();
                lblTienThoiValue.Text = _hoaDonTienService.DinhDangTien(0);
            }
            finally
            {
                _dangNapDuLieu = false;
            }
        }

        private void btnSua_Click(object? sender, EventArgs e)
        {
            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hoaDon.TrangThai != 0)
            {
                MessageBox.Show("Chỉ sửa được hóa đơn chưa thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ChuyenStateManHinh(HoaDonManHinhState.ChinhSua);
        }

        private void btnLuu_Click(object? sender, EventArgs e)
        {
            if (!TryTaoThongTinLuu(out var request))
            {
                return;
            }

            if (_manHinhState == HoaDonManHinhState.ThemMoi)
            {
                var ketQuaThem = _hoaDonBUS.ThemHoaDon(request);
                if (!ketQuaThem.Result.ThanhCong)
                {
                    MessageBox.Show(ketQuaThem.Result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _hoaDonDangChonId = ketQuaThem.HoaDonId;
                ChuyenStateManHinh(HoaDonManHinhState.Xem);
                TaiDanhSachHoaDon();
                MessageBox.Show(ketQuaThem.Result.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_manHinhState == HoaDonManHinhState.ChinhSua)
            {
                request.ID = _hoaDonDangChonId ?? 0;
                var ketQuaCapNhat = _hoaDonBUS.CapNhatHoaDon(request);
                if (!ketQuaCapNhat.ThanhCong)
                {
                    MessageBox.Show(ketQuaCapNhat.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ChuyenStateManHinh(HoaDonManHinhState.Xem);
                TaiDanhSachHoaDon();
                MessageBox.Show(ketQuaCapNhat.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool TryTaoThongTinLuu(out HoaDonSaveRequestDTO request)
        {
            request = new HoaDonSaveRequestDTO();

            if (cboBanKhach.SelectedValue is not int banId || banId <= 0)
            {
                MessageBox.Show("Vui lòng chọn bàn hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboBanKhach.Focus();
                return false;
            }

            request.BanID = banId;
            request.NgayLap = dtpNgayTao.Value;
            request.TrangThai = 0;
            return true;
        }

        private void btnBoQua_Click(object? sender, EventArgs e)
        {
            ChuyenStateManHinh(HoaDonManHinhState.Xem);
            TaiDanhSachHoaDon();
        }

        private void btnXoaHuy_Click(object? sender, EventArgs e)
        {
            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hoaDon.TrangThai == 1)
            {
                MessageBox.Show("Hóa đơn đã thanh toán, không thể hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hoaDon.TrangThai == 2)
            {
                MessageBox.Show("Hóa đơn này đã hủy trước đó.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Xác nhận hủy hóa đơn {hoaDon.MaHoaDonHienThi}?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            var ketQua = _hoaDonBUS.HuyHoaDon(hoaDon.ID);
            if (!ketQua.ThanhCong)
            {
                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSachHoaDon();
            MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnThemMonVaoHoaDon_Click(object? sender, EventArgs e)
        {
            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn trước khi thêm món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (hoaDon.TrangThai != 0)
            {
                MessageBox.Show("Chỉ thêm món cho hóa đơn chưa thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboMon.SelectedValue is not int monId || monId <= 0)
            {
                MessageBox.Show("Vui lòng chọn món hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMon.Focus();
                return;
            }

            var soLuong = (short)nudSoLuong.Value;
            var ketQua = _hoaDonBUS.ThemMonVaoHoaDon(hoaDon.ID, monId, soLuong);

            if (!ketQua.ThanhCong)
            {
                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSachHoaDon();
            MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXacNhanThuTien_Click(object? sender, EventArgs e)
        {
            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần thu tiền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tienKhachDua = _hoaDonTienService.ChuyenTextTienThanhSo(txtTienKhachDua.Text);
            var ketQua = _hoaDonBUS.XacNhanThuTien(hoaDon.ID, tienKhachDua);
            if (!ketQua.ThanhCong)
            {
                MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSachHoaDon();
            MessageBox.Show(ketQua.ThongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnInHoaDon_Click(object? sender, EventArgs e)
        {
            var hoaDon = LayHoaDonDangChon();
            if (hoaDon == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var chiTiet = _hoaDonBUS.LayHoaDonTheoId(hoaDon.ID);
            if (chiTiet == null)
            {
                MessageBox.Show("Không tìm thấy dữ liệu hóa đơn để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var noiDungXemTruoc = _hoaDonPreviewService.TaoNoiDungXemTruoc(chiTiet);
            MessageBox.Show(noiDungXemTruoc, "Xem trước hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtTienKhachDua_TextChanged(object? sender, EventArgs e)
        {
            if (_dangNapDuLieu)
            {
                return;
            }

            var tienKhachDua = _hoaDonTienService.ChuyenTextTienThanhSo(txtTienKhachDua.Text);
            var tienThoi = _hoaDonBUS.TinhTienThoi(_tongTienDangChon, tienKhachDua);
            lblTienThoiValue.Text = _hoaDonTienService.DinhDangTien(tienThoi);
        }

        private void ChuyenStateManHinh(HoaDonManHinhState state)
        {
            _manHinhState = state;

            var hoaDon = state == HoaDonManHinhState.Xem
                ? LayHoaDonDangChon()
                : null;

            ApDungTrangThaiDieuKhien(hoaDon);
        }

        private void CapNhatDieuKienXuLyNut(HoaDonDTO? hoaDon)
        {
            ApDungTrangThaiDieuKhien(hoaDon);
        }

        private void ApDungTrangThaiDieuKhien(HoaDonDTO? hoaDon)
        {
            var trangThai = _hoaDonFormStateService.TaoTrangThai(_manHinhState, hoaDon);
            var isAdmin = _permissionBUS.IsAdmin();
            var coQuyenXem = isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View);
            var coQuyenTao = isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Create);
            var coQuyenCapNhat = isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Update);
            var coQuyenXoa = isAdmin || _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Delete);

            cboBanKhach.Enabled = trangThai.ChoPhepSuaThongTinChung && (coQuyenTao || coQuyenCapNhat);
            dtpNgayTao.Enabled = trangThai.ChoPhepSuaThongTinChung && (coQuyenTao || coQuyenCapNhat);
            cboTrangThai.Enabled = false;

            panelMasterFilter.Enabled = trangThai.ChoPhepLocMaster && coQuyenXem;
            dgvDanhSachHoaDon.Enabled = trangThai.ChoPhepGridMaster && coQuyenXem;

            btnThemMoi.Enabled = trangThai.ChoPhepThemMoi && coQuyenTao;
            btnSua.Enabled = trangThai.ChoPhepSua && coQuyenCapNhat;
            btnXoaHuy.Enabled = trangThai.ChoPhepHuy && coQuyenXoa;
            btnLuu.Enabled = trangThai.ChoPhepLuu && (coQuyenTao || coQuyenCapNhat);
            btnBoQua.Enabled = trangThai.ChoPhepBoQua;

            btnThemMonVaoHoaDon.Enabled = trangThai.ChoPhepThemMon && coQuyenCapNhat;
            btnXacNhanThuTien.Enabled = trangThai.ChoPhepThuTien && coQuyenCapNhat;
            btnInHoaDon.Enabled = trangThai.ChoPhepIn && coQuyenXem;
        }

        private HoaDonDTO? LayHoaDonDangChon()
        {
            if (!_hoaDonDangChonId.HasValue)
            {
                return null;
            }

            return _hoaDonBUS.LayHoaDonTheoId(_hoaDonDangChonId.Value);
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
                    TaiDanhSachHoaDon();
                }
            };

            targetForm.Show(this);
        }

        private void MoTinhNangThongKe()
        {
            if (!_permissionBUS.CheckPermission(PermissionFeatures.ThongKe, PermissionActions.View))
            {
                MessageBox.Show("Ban khong co quyen truy cap chuc nang thong ke.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Chuc nang thong ke dang duoc phat trien.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }

    }
}
