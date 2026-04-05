using System.IO;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmDangNhap : Form
    {
        private readonly DangNhapBUS _dangNhapBUS = new();
        private readonly ErrorProvider _errorProvider = new();
        private bool _dangXuLyDangNhap;

        private static readonly string DuongDanTaiKhoanDaNho = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "QuanLyQuanCaPhe",
            "remember-account.txt");

        public ThongTinDangNhapDTO? ThongTinDangNhap { get; private set; }

        public frmDangNhap()
        {
            InitializeComponent();

            _errorProvider.ContainerControl = this;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            Load += frmDangNhap_Load;
            btnDangNhap.Click += btnDangNhap_Click;
            btnThoat.Click += btnThoat_Click;
            txtTenDangNhap.TextChanged += Input_TextChanged;
            txtMatKhau.TextChanged += Input_TextChanged;
        }

        private void frmDangNhap_Load(object? sender, EventArgs e)
        {
            HienThiThongBaoLoi(string.Empty);
            TaiTaiKhoanDaNho();

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                txtTenDangNhap.Focus();
                return;
            }

            txtMatKhau.Focus();
        }

        private void btnDangNhap_Click(object? sender, EventArgs e)
        {
            XuLyDangNhap();
        }

        private void btnThoat_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void Input_TextChanged(object? sender, EventArgs e)
        {
            XoaTrangThaiLoi();
        }

        private void XuLyDangNhap()
        {
            if (_dangXuLyDangNhap)
            {
                return;
            }

            XoaTrangThaiLoi();
            DatTrangThaiDangNhap(true);

            try
            {
                var ketQua = _dangNhapBUS.DangNhap(txtTenDangNhap.Text, txtMatKhau.Text);
                if (!ketQua.ThanhCong || ketQua.ThongTinDangNhap == null)
                {
                    HienThiLoiTheoTruong(ketQua.TruongLoi, ketQua.ThongBao);
                    return;
                }

                ThongTinDangNhap = ketQua.ThongTinDangNhap;
                LuuTaiKhoanDaNho();

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                HienThiThongBaoLoi("Không thể kết nối cơ sở dữ liệu. Vui lòng thử lại.");
                MessageBox.Show(
                    $"Đăng nhập thất bại do lỗi hệ thống.\nChi tiết: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    DatTrangThaiDangNhap(false);
                }
            }
        }

        private void DatTrangThaiDangNhap(bool dangXuLy)
        {
            _dangXuLyDangNhap = dangXuLy;
            btnDangNhap.Enabled = !dangXuLy;
            btnThoat.Enabled = !dangXuLy;
            btnDangNhap.Text = dangXuLy ? "Đang xác thực..." : "Đăng nhập";
            Cursor = dangXuLy ? Cursors.WaitCursor : Cursors.Default;
        }

        private void HienThiLoiTheoTruong(string truongLoi, string thongBao)
        {
            HienThiThongBaoLoi(thongBao);

            switch (truongLoi)
            {
                case "TenDangNhap":
                    _errorProvider.SetError(txtTenDangNhap, thongBao);
                    txtTenDangNhap.Focus();
                    txtTenDangNhap.SelectAll();
                    break;
                case "MatKhau":
                    _errorProvider.SetError(txtMatKhau, thongBao);
                    txtMatKhau.Focus();
                    txtMatKhau.SelectAll();
                    break;
            }
        }

        private void HienThiThongBaoLoi(string thongBao)
        {
            lblThongBaoLoi.Text = thongBao;
            lblThongBaoLoi.Visible = !string.IsNullOrWhiteSpace(thongBao);
        }

        private void XoaTrangThaiLoi()
        {
            _errorProvider.SetError(txtTenDangNhap, string.Empty);
            _errorProvider.SetError(txtMatKhau, string.Empty);
            HienThiThongBaoLoi(string.Empty);
        }

        private void TaiTaiKhoanDaNho()
        {
            try
            {
                if (!File.Exists(DuongDanTaiKhoanDaNho))
                {
                    return;
                }

                var tenDangNhap = File.ReadAllText(DuongDanTaiKhoanDaNho).Trim();
                if (string.IsNullOrWhiteSpace(tenDangNhap))
                {
                    return;
                }

                txtTenDangNhap.Text = tenDangNhap;
                chkNhoDangNhap.Checked = true;
            }
            catch
            {
                // Bỏ qua lỗi đọc file để không chặn luồng đăng nhập.
            }
        }

        private void LuuTaiKhoanDaNho()
        {
            try
            {
                if (chkNhoDangNhap.Checked)
                {
                    var thuMuc = Path.GetDirectoryName(DuongDanTaiKhoanDaNho);
                    if (!string.IsNullOrWhiteSpace(thuMuc))
                    {
                        Directory.CreateDirectory(thuMuc);
                    }

                    File.WriteAllText(DuongDanTaiKhoanDaNho, txtTenDangNhap.Text.Trim());
                    return;
                }

                if (File.Exists(DuongDanTaiKhoanDaNho))
                {
                    File.Delete(DuongDanTaiKhoanDaNho);
                }
            }
            catch
            {
                // Bỏ qua lỗi ghi file để tránh ảnh hưởng phiên đăng nhập.
            }
        }
    }
}
