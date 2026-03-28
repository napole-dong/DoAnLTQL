using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmDashboard : Form
    {
        private Button? _activeSidebarButton;
        private readonly List<int> _revenueValues = new();
        private CaPheDbContext db = new CaPheDbContext();
        public frmDashboard()
        {
            InitializeComponent();
            ConfigureDashboard();
        }

        private void ConfigureDashboard()
        {
            ConfigureSidebarInteractions();
            ConfigureTopbarMenu();
            InitializeDashboardPlaceholders();
            Load += frmDashboard_Load;
            SetActiveSidebarButton(btnDashboard);
            picRevenueChart.Resize += (_, _) => DrawRevenueChart();
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
                var tomorrow = today.AddDays(1);
                var sevenDaysAgo = today.AddDays(-6);

                var doanhThuHomNay = db.HoaDon_ChiTiet
                    .AsNoTracking()
                    .Where(ct => ct.HoaDon.TrangThai == 1 && ct.HoaDon.NgayLap >= today && ct.HoaDon.NgayLap < tomorrow)
                    .Sum(ct => (long)ct.SoLuongBan * ct.DonGiaBan);

                var soHoaDonHomNay = db.HoaDon
                    .AsNoTracking()
                    .Count(hd => hd.NgayLap >= today && hd.NgayLap < tomorrow);

                var tongBan = db.Ban.AsNoTracking().Count();
                var banDangSuDung = db.Ban.AsNoTracking().Count(b => b.TrangThai == 1);

                var soMonBanHomNay = db.HoaDon_ChiTiet
                    .AsNoTracking()
                    .Where(ct => ct.HoaDon.NgayLap >= today && ct.HoaDon.NgayLap < tomorrow)
                    .Sum(ct => (int?)ct.SoLuongBan) ?? 0;

                lblDoanhThuValue.Text = FormatCurrency(doanhThuHomNay);
                lblHoaDonValue.Text = soHoaDonHomNay.ToString();
                lblBanValue.Text = tongBan == 0 ? "0/0" : $"{banDangSuDung}/{tongBan}";
                lblMonBanValue.Text = soMonBanHomNay.ToString();

                LoadRevenueChartData(db, sevenDaysAgo, tomorrow);
                LoadTopSellingData(db, sevenDaysAgo, tomorrow);
                LoadRecentData(db, 10);
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

        private void LoadRevenueChartData(CaPheDbContext db, DateTime fromDate, DateTime toDate)
        {
            var doanhThu7Ngay = db.HoaDon_ChiTiet
                .AsNoTracking()
                .Where(ct => ct.HoaDon.TrangThai == 1 && ct.HoaDon.NgayLap >= fromDate && ct.HoaDon.NgayLap < toDate)
                .GroupBy(ct => ct.HoaDon.NgayLap.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(ct => (long)ct.SoLuongBan * ct.DonGiaBan)
                })
                .ToList()
                .ToDictionary(x => x.Date, x => x.Revenue);

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

        private void LoadTopSellingData(CaPheDbContext db, DateTime fromDate, DateTime toDate)
        {
            var topMon = db.HoaDon_ChiTiet
                .AsNoTracking()
                .Where(ct => ct.HoaDon.NgayLap >= fromDate && ct.HoaDon.NgayLap < toDate)
                .GroupBy(ct => ct.Mon.TenMon)
                .Select(g => new
                {
                    TenMon = g.Key,
                    SoLuong = g.Sum(ct => (int)ct.SoLuongBan)
                })
                .OrderByDescending(x => x.SoLuong)
                .Take(5)
                .ToList();

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

        private void LoadRecentData(CaPheDbContext db, int soLuong = 5)
        {
            var recentBills = db.HoaDon
                .AsNoTracking()
                .OrderByDescending(hd => hd.NgayLap)
                .Select(hd => new
                {
                    hd.ID,
                    hd.NgayLap,
                    hd.TrangThai,
                    TenBan = hd.Ban.TenBan,
                    TenNhanVien = hd.NhanVien.HoVaTen,
                    TongTien = hd.HoaDon_ChiTiet.Sum(ct => (long)ct.SoLuongBan * ct.DonGiaBan)
                })
                .Take(Math.Clamp(soLuong, 5, 10))
                .ToList();

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

        private void cardDoanhThu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void picRevenueChart_Click(object sender, EventArgs e)
        {

        }

        private void btnQuanLyMon_Click(object? sender, EventArgs e)
        {
            using var fMon = new frmQuanLiMon();
            fMon.ShowDialog();
        }

        private void btnQuanLyBan_Click(object? sender, EventArgs e)
        {
            using var fBan = new frmQuanLiBan();
            fBan.ShowDialog();
        }
    }
}
