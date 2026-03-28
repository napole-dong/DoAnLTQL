using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmDashboard : Form
    {
        private Button? _activeSidebarButton;
        private Form? _currentPageForm;
        private readonly Panel _pageHost = new();
        private readonly List<int> _revenueValues = new();
        private readonly DashboardBUS _dashboardBUS = new();
        public frmDashboard()
        {
            InitializeComponent();
            ConfigureDashboard();
        }

        private void ConfigureDashboard()
        {
            ConfigureSidebarInteractions();
            ConfigureTopbarMenu();
            ConfigurePageHost();
            InitializeDashboardPlaceholders();
            Load += frmDashboard_Load;
            SetActiveSidebarButton(btnDashboard);
            picRevenueChart.Resize += (_, _) => DrawRevenueChart();
        }

        private void ConfigurePageHost()
        {
            _pageHost.Dock = DockStyle.Fill;
            _pageHost.Visible = false;
            panelContent.Controls.Add(_pageHost);
            _pageHost.BringToFront();
        }

        private void InitializeDashboardPlaceholders()
        {
            lblDoanhThuValue.Text = string.Empty;
            lblHoaDonValue.Text = string.Empty;
            lblBanValue.Text = string.Empty;
            lblMonBanValue.Text = string.Empty;

            _revenueValues.Clear();
            _revenueValues.AddRange(Enumerable.Repeat(0, 7));

            SetTopSellingFallback();
            dgvRecentBills.Rows.Clear();
            listRecentActivity.Items.Clear();
        }

        private void frmDashboard_Load(object? sender, EventArgs e)
        {
            try
            {
                var today = DateTime.Today;
                var sevenDaysAgo = today.AddDays(-6);
                var dashboardData = _dashboardBUS.LayDuLieuDashboard(10);

                lblDoanhThuValue.Text = FormatCurrency(dashboardData.TongQuan.DoanhThuHomNay);
                lblHoaDonValue.Text = dashboardData.TongQuan.SoHoaDonHomNay.ToString();
                lblBanValue.Text = dashboardData.TongQuan.TongBan == 0
                    ? "0/0"
                    : $"{dashboardData.TongQuan.BanDangSuDung}/{dashboardData.TongQuan.TongBan}";
                lblMonBanValue.Text = dashboardData.TongQuan.SoMonBanHomNay.ToString();

                LoadRevenueChartData(dashboardData.DoanhThu7Ngay, sevenDaysAgo);
                LoadTopSellingData(dashboardData.TopMonBanChay);
                LoadRecentData(dashboardData.HoaDonGanDay);
            }
            catch
            {
                _revenueValues.Clear();
                _revenueValues.AddRange(Enumerable.Repeat(0, 7));
                lblDoanhThuValue.Text = FormatCurrency(0);
                lblHoaDonValue.Text = "0";
                lblBanValue.Text = "0/0";
                lblMonBanValue.Text = "0";
                SetTopSellingFallback();
                dgvRecentBills.Rows.Clear();
                listRecentActivity.Items.Clear();
            }

            DrawRevenueChart();
        }

        private void ConfigureSidebarInteractions()
        {
            foreach (var control in flowSidebarMenu.Controls)
            {
                if (control is not Button btn) continue;

                btn.MouseEnter += (_, _) =>
                {
                    if (btn != _activeSidebarButton)
                    {
                        btn.BackColor = Color.FromArgb(76, 56, 45);
                    }
                };

                btn.MouseLeave += (_, _) =>
                {
                    if (btn != _activeSidebarButton)
                    {
                        btn.BackColor = Color.FromArgb(52, 36, 29);
                    }
                };

                btn.Click += (_, _) => SetActiveSidebarButton(btn);
            }

            btnQuanLyMon.Click += btnQuanLyMon_Click;
            btnQuanLyBan.Click += btnQuanLyBan_Click;
            btnLoaiMon.Click += btnLoaiMon_Click;
            btnKhachHang.Click += btnKhachHang_Click;
            btnNhanVien.Click += btnNhanVien_Click;
            btnDashboard.Click += btnDashboard_Click;

            btnDangXuat.MouseEnter += (_, _) => btnDangXuat.BackColor = Color.FromArgb(76, 56, 45);
            btnDangXuat.MouseLeave += (_, _) => btnDangXuat.BackColor = Color.FromArgb(52, 36, 29);
        }

        private void SetActiveSidebarButton(Button button)
        {
            if (_activeSidebarButton != null)
            {
                _activeSidebarButton.BackColor = Color.FromArgb(52, 36, 29);
                _activeSidebarButton.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                _activeSidebarButton.ForeColor = Color.Gainsboro;
            }

            _activeSidebarButton = button;
            _activeSidebarButton.BackColor = Color.FromArgb(94, 64, 47);
            _activeSidebarButton.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            _activeSidebarButton.ForeColor = Color.White;
        }

        private void ConfigureTopbarMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Profile");
            menu.Items.Add("Logout");

            btnUserMenu.Click += (_, _) => menu.Show(btnUserMenu, new Point(0, btnUserMenu.Height));
        }

        private void LoadRevenueChartData(List<DashboardRevenueDTO> revenueData, DateTime fromDate)
        {
            var doanhThu7Ngay = revenueData.ToDictionary(x => x.Ngay.Date, x => x.DoanhThu);

            _revenueValues.Clear();
            for (var i = 0; i < 7; i++)
            {
                var ngay = fromDate.AddDays(i);
                var revenue = doanhThu7Ngay.GetValueOrDefault(ngay, 0);
                _revenueValues.Add((int)Math.Min(int.MaxValue, revenue));
            }

            if (_revenueValues.Count == 0)
            {
                _revenueValues.AddRange(Enumerable.Repeat(0, 7));
            }
        }

        private void LoadTopSellingData(List<DashboardTopMonDTO> topMon)
        {
            var labels = new[] { lblTop1, lblTop2, lblTop3, lblTop4, lblTop5 };
            var bars = new[] { progressTop1, progressTop2, progressTop3, progressTop4, progressTop5 };

            var maxSoLuong = topMon.FirstOrDefault()?.SoLuong ?? 0;

            for (var i = 0; i < labels.Length; i++)
            {
                if (i < topMon.Count)
                {
                    labels[i].Text = topMon[i].TenMon;
                    bars[i].Value = maxSoLuong == 0
                        ? 0
                        : Math.Clamp((int)Math.Round(topMon[i].SoLuong * 100d / maxSoLuong), 0, 100);
                }
                else
                {
                    labels[i].Text = "-";
                    bars[i].Value = 0;
                }
            }
        }

        private void LoadRecentData(List<DashboardRecentBillDTO> recentBills)
        {
            dgvRecentBills.Rows.Clear();
            foreach (var bill in recentBills)
            {
                dgvRecentBills.Rows.Add(
                    $"HD{bill.ID:D5}",
                    bill.TenBan,
                    bill.NgayLap.ToString("HH:mm"),
                    FormatCurrency(bill.TongTien),
                    bill.TrangThai == 1 ? "Đã thanh toán" : "Đang phục vụ");
            }

            listRecentActivity.Items.Clear();
            foreach (var bill in recentBills)
            {
                var activity = bill.TrangThai == 1
                    ? $"{bill.NgayLap:HH:mm} - {bill.TenBan} đã thanh toán HD{bill.ID:D5}"
                    : $"{bill.NgayLap:HH:mm} - {bill.TenNhanVien} tạo hóa đơn HD{bill.ID:D5}";
                listRecentActivity.Items.Add(activity);
            }
        }

        private void SetTopSellingFallback()
        {
            lblTop1.Text = "-";
            lblTop2.Text = "-";
            lblTop3.Text = "-";
            lblTop4.Text = "-";
            lblTop5.Text = "-";

            progressTop1.Value = 0;
            progressTop2.Value = 0;
            progressTop3.Value = 0;
            progressTop4.Value = 0;
            progressTop5.Value = 0;
        }

        private static string FormatCurrency(long amount)
        {
            return string.Format("{0:N0}đ", amount);
        }

        private void DrawRevenueChart()
        {
            if (picRevenueChart.Width <= 0 || picRevenueChart.Height <= 0)
            {
                return;
            }

            var bmp = new Bitmap(picRevenueChart.Width, picRevenueChart.Height);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(252, 250, 247));

            const int leftPadding = 42;
            const int rightPadding = 22;
            const int topPadding = 18;
            const int bottomPadding = 28;

            var plotWidth = bmp.Width - leftPadding - rightPadding;
            var plotHeight = bmp.Height - topPadding - bottomPadding;
            if (plotWidth <= 0 || plotHeight <= 0) return;

            using var axisPen = new Pen(Color.Gainsboro, 1);
            g.DrawLine(axisPen, leftPadding, topPadding, leftPadding, topPadding + plotHeight);
            g.DrawLine(axisPen, leftPadding, topPadding + plotHeight, leftPadding + plotWidth, topPadding + plotHeight);

            var maxValue = _revenueValues.Max();
            var minValue = _revenueValues.Min();
            var range = Math.Max(1, maxValue - minValue);
            var stepX = _revenueValues.Count > 1 ? plotWidth / (float)(_revenueValues.Count - 1) : 0;

            var points = new PointF[_revenueValues.Count];
            for (var i = 0; i < _revenueValues.Count; i++)
            {
                var normalized = (_revenueValues[i] - minValue) / (float)range;
                var x = leftPadding + (i * stepX);
                var y = topPadding + (plotHeight - (normalized * plotHeight));
                points[i] = new PointF(x, y);
            }

            using var linePen = new Pen(Color.FromArgb(160, 100, 58), 3);
            if (points.Length > 1)
            {
                g.DrawLines(linePen, points);
            }

            using var pointBrush = new SolidBrush(Color.FromArgb(121, 71, 38));
            foreach (var point in points)
            {
                g.FillEllipse(pointBrush, point.X - 4, point.Y - 4, 8, 8);
            }

            picRevenueChart.Image?.Dispose();
            picRevenueChart.Image = bmp;
        }

        private void btnQuanLyMon_Click(object? sender, EventArgs e)
        {
            OpenPage(new frmQuanLiMon(true), btnQuanLyMon);
        }

        private void btnQuanLyBan_Click(object? sender, EventArgs e)
        {
            OpenPage(new frmQuanLiBan(true), btnQuanLyBan);
        }

        private void btnLoaiMon_Click(object? sender, EventArgs e)
        {
            OpenPage(new frmLoaiMon(true), btnLoaiMon);
        }

        private void btnKhachHang_Click(object? sender, EventArgs e)
        {
            OpenPage(new frmKhachHang(true), btnKhachHang);
        }

        private void btnNhanVien_Click(object? sender, EventArgs e)
        {
            OpenPage(new frmNhanVien(true), btnNhanVien);
        }

        private void btnDashboard_Click(object? sender, EventArgs e)
        {
            ShowDashboardHome();
        }

        private void OpenPage(Form page, Button navigationButton)
        {
            SetActiveSidebarButton(navigationButton);

            _currentPageForm?.Close();
            _currentPageForm?.Dispose();

            tableMain.Visible = false;
            _pageHost.Visible = true;
            _pageHost.Controls.Clear();

            page.TopLevel = false;
            page.FormBorderStyle = FormBorderStyle.None;
            page.Dock = DockStyle.Fill;
            _pageHost.Controls.Add(page);
            _currentPageForm = page;
            page.Show();
        }

        private void ShowDashboardHome()
        {
            _currentPageForm?.Close();
            _currentPageForm?.Dispose();
            _currentPageForm = null;

            _pageHost.Controls.Clear();
            _pageHost.Visible = false;
            tableMain.Visible = true;
            SetActiveSidebarButton(btnDashboard);
        }
    }
}
