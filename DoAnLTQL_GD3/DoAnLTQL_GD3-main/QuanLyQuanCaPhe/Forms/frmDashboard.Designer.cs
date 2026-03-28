namespace QuanLyQuanCaPhe.Forms
{
    partial class frmDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panelSidebar = new Panel();
            btnDangXuat = new Button();
            flowSidebarMenu = new FlowLayoutPanel();
            btnDashboard = new Button();
            btnQuanLyBan = new Button();
            btnQuanLyMon = new Button();
            btnLoaiMon = new Button();
            btnHoaDon = new Button();
            btnNhanVien = new Button();
            btnThongKe = new Button();
            panelLogo = new Panel();
            lblLogoSub = new Label();
            lblLogo = new Label();
            panelMain = new Panel();
            panelContent = new Panel();
            tableMain = new TableLayoutPanel();
            tableStats = new TableLayoutPanel();
            cardDoanhThu = new Panel();
            lblDoanhThuValue = new Label();
            lblDoanhThuTitle = new Label();
            lblDoanhThuIcon = new Label();
            cardHoaDon = new Panel();
            lblHoaDonValue = new Label();
            lblHoaDonTitle = new Label();
            lblHoaDonIcon = new Label();
            cardBan = new Panel();
            lblBanValue = new Label();
            lblBanTitle = new Label();
            lblBanIcon = new Label();
            cardMonBan = new Panel();
            lblMonBanValue = new Label();
            lblMonBanTitle = new Label();
            lblMonBanIcon = new Label();
            tableCharts = new TableLayoutPanel();
            panelRevenueChart = new Panel();
            picRevenueChart = new PictureBox();
            lblRevenueChartTitle = new Label();
            panelTopSellingChart = new Panel();
            tableTopSellingBars = new TableLayoutPanel();
            progressTop5 = new ProgressBar();
            progressTop4 = new ProgressBar();
            progressTop3 = new ProgressBar();
            progressTop2 = new ProgressBar();
            progressTop1 = new ProgressBar();
            lblTop5 = new Label();
            lblTop4 = new Label();
            lblTop3 = new Label();
            lblTop2 = new Label();
            lblTop1 = new Label();
            lblTopSellingTitle = new Label();
            tableBottom = new TableLayoutPanel();
            panelRecentBills = new Panel();
            dgvRecentBills = new DataGridView();
            colMaHoaDon = new DataGridViewTextBoxColumn();
            colBan = new DataGridViewTextBoxColumn();
            colThoiGian = new DataGridViewTextBoxColumn();
            colTongTien = new DataGridViewTextBoxColumn();
            colTrangThai = new DataGridViewTextBoxColumn();
            lblRecentBillsTitle = new Label();
            panelRecentActivity = new Panel();
            listRecentActivity = new ListBox();
            lblRecentActivityTitle = new Label();
            panelTopbar = new Panel();
            btnUserMenu = new Button();
            lblUserName = new Label();
            picAvatar = new PictureBox();
            txtSearch = new TextBox();
            panelSidebar.SuspendLayout();
            flowSidebarMenu.SuspendLayout();
            panelLogo.SuspendLayout();
            panelMain.SuspendLayout();
            panelContent.SuspendLayout();
            tableMain.SuspendLayout();
            tableStats.SuspendLayout();
            cardDoanhThu.SuspendLayout();
            cardHoaDon.SuspendLayout();
            cardBan.SuspendLayout();
            cardMonBan.SuspendLayout();
            tableCharts.SuspendLayout();
            panelRevenueChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picRevenueChart).BeginInit();
            panelTopSellingChart.SuspendLayout();
            tableTopSellingBars.SuspendLayout();
            tableBottom.SuspendLayout();
            panelRecentBills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecentBills).BeginInit();
            panelRecentActivity.SuspendLayout();
            panelTopbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(52, 36, 29);
            panelSidebar.Controls.Add(btnDangXuat);
            panelSidebar.Controls.Add(flowSidebarMenu);
            panelSidebar.Controls.Add(panelLogo);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(230, 760);
            panelSidebar.TabIndex = 0;
            // 
            // btnDangXuat
            // 
            btnDangXuat.Dock = DockStyle.Bottom;
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnDangXuat.ForeColor = Color.Gainsboro;
            btnDangXuat.ImageAlign = ContentAlignment.MiddleLeft;
            btnDangXuat.Location = new Point(0, 704);
            btnDangXuat.Margin = new Padding(0);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Padding = new Padding(20, 0, 0, 0);
            btnDangXuat.Size = new Size(230, 56);
            btnDangXuat.TabIndex = 2;
            btnDangXuat.Text = "↩  Đăng xuất";
            btnDangXuat.TextAlign = ContentAlignment.MiddleLeft;
            btnDangXuat.UseVisualStyleBackColor = true;
            // 
            // flowSidebarMenu
            // 
            flowSidebarMenu.Controls.Add(btnDashboard);
            flowSidebarMenu.Controls.Add(btnQuanLyBan);
            flowSidebarMenu.Controls.Add(btnQuanLyMon);
            flowSidebarMenu.Controls.Add(btnLoaiMon);
            flowSidebarMenu.Controls.Add(btnHoaDon);
            flowSidebarMenu.Controls.Add(btnNhanVien);
            flowSidebarMenu.Controls.Add(btnThongKe);
            flowSidebarMenu.Dock = DockStyle.Fill;
            flowSidebarMenu.FlowDirection = FlowDirection.TopDown;
            flowSidebarMenu.Location = new Point(0, 92);
            flowSidebarMenu.Name = "flowSidebarMenu";
            flowSidebarMenu.Padding = new Padding(0, 14, 0, 0);
            flowSidebarMenu.Size = new Size(230, 668);
            flowSidebarMenu.TabIndex = 1;
            flowSidebarMenu.WrapContents = false;
            // 
            // btnDashboard
            // 
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(0, 14);
            btnDashboard.Margin = new Padding(0);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(20, 0, 0, 0);
            btnDashboard.Size = new Size(230, 48);
            btnDashboard.TabIndex = 0;
            btnDashboard.Text = "🏠  Dashboard";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = true;
            // 
            // btnQuanLyBan
            // 
            btnQuanLyBan.FlatAppearance.BorderSize = 0;
            btnQuanLyBan.FlatStyle = FlatStyle.Flat;
            btnQuanLyBan.Font = new Font("Segoe UI", 10F);
            btnQuanLyBan.ForeColor = Color.Gainsboro;
            btnQuanLyBan.Location = new Point(0, 62);
            btnQuanLyBan.Margin = new Padding(0);
            btnQuanLyBan.Name = "btnQuanLyBan";
            btnQuanLyBan.Padding = new Padding(20, 0, 0, 0);
            btnQuanLyBan.Size = new Size(230, 48);
            btnQuanLyBan.TabIndex = 1;
            btnQuanLyBan.Text = "\U0001fa91  Quản lý bàn";
            btnQuanLyBan.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyBan.UseVisualStyleBackColor = true;
            // 
            // btnQuanLyMon
            // 
            btnQuanLyMon.FlatAppearance.BorderSize = 0;
            btnQuanLyMon.FlatStyle = FlatStyle.Flat;
            btnQuanLyMon.Font = new Font("Segoe UI", 10F);
            btnQuanLyMon.ForeColor = Color.Gainsboro;
            btnQuanLyMon.Location = new Point(0, 110);
            btnQuanLyMon.Margin = new Padding(0);
            btnQuanLyMon.Name = "btnQuanLyMon";
            btnQuanLyMon.Padding = new Padding(20, 0, 0, 0);
            btnQuanLyMon.Size = new Size(230, 48);
            btnQuanLyMon.TabIndex = 2;
            btnQuanLyMon.Text = "☕  Quản lý món";
            btnQuanLyMon.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyMon.UseVisualStyleBackColor = true;
            // 
            // btnLoaiMon
            // 
            btnLoaiMon.FlatAppearance.BorderSize = 0;
            btnLoaiMon.FlatStyle = FlatStyle.Flat;
            btnLoaiMon.Font = new Font("Segoe UI", 10F);
            btnLoaiMon.ForeColor = Color.Gainsboro;
            btnLoaiMon.Location = new Point(0, 158);
            btnLoaiMon.Margin = new Padding(0);
            btnLoaiMon.Name = "btnLoaiMon";
            btnLoaiMon.Padding = new Padding(20, 0, 0, 0);
            btnLoaiMon.Size = new Size(230, 48);
            btnLoaiMon.TabIndex = 3;
            btnLoaiMon.Text = "🏷  Loại món";
            btnLoaiMon.TextAlign = ContentAlignment.MiddleLeft;
            btnLoaiMon.UseVisualStyleBackColor = true;
            // 
            // btnHoaDon
            // 
            btnHoaDon.FlatAppearance.BorderSize = 0;
            btnHoaDon.FlatStyle = FlatStyle.Flat;
            btnHoaDon.Font = new Font("Segoe UI", 10F);
            btnHoaDon.ForeColor = Color.Gainsboro;
            btnHoaDon.Location = new Point(0, 206);
            btnHoaDon.Margin = new Padding(0);
            btnHoaDon.Name = "btnHoaDon";
            btnHoaDon.Padding = new Padding(20, 0, 0, 0);
            btnHoaDon.Size = new Size(230, 48);
            btnHoaDon.TabIndex = 4;
            btnHoaDon.Text = "\U0001f9fe  Hóa đơn";
            btnHoaDon.TextAlign = ContentAlignment.MiddleLeft;
            btnHoaDon.UseVisualStyleBackColor = true;
            // 
            // btnNhanVien
            // 
            btnNhanVien.FlatAppearance.BorderSize = 0;
            btnNhanVien.FlatStyle = FlatStyle.Flat;
            btnNhanVien.Font = new Font("Segoe UI", 10F);
            btnNhanVien.ForeColor = Color.Gainsboro;
            btnNhanVien.Location = new Point(0, 254);
            btnNhanVien.Margin = new Padding(0);
            btnNhanVien.Name = "btnNhanVien";
            btnNhanVien.Padding = new Padding(20, 0, 0, 0);
            btnNhanVien.Size = new Size(230, 48);
            btnNhanVien.TabIndex = 5;
            btnNhanVien.Text = "👤  Nhân viên";
            btnNhanVien.TextAlign = ContentAlignment.MiddleLeft;
            btnNhanVien.UseVisualStyleBackColor = true;
            // 
            // btnThongKe
            // 
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.FlatStyle = FlatStyle.Flat;
            btnThongKe.Font = new Font("Segoe UI", 10F);
            btnThongKe.ForeColor = Color.Gainsboro;
            btnThongKe.Location = new Point(0, 302);
            btnThongKe.Margin = new Padding(0);
            btnThongKe.Name = "btnThongKe";
            btnThongKe.Padding = new Padding(20, 0, 0, 0);
            btnThongKe.Size = new Size(230, 48);
            btnThongKe.TabIndex = 6;
            btnThongKe.Text = "📈  Thống kê";
            btnThongKe.TextAlign = ContentAlignment.MiddleLeft;
            btnThongKe.UseVisualStyleBackColor = true;
            // 
            // panelLogo
            // 
            panelLogo.Controls.Add(lblLogoSub);
            panelLogo.Controls.Add(lblLogo);
            panelLogo.Dock = DockStyle.Top;
            panelLogo.Location = new Point(0, 0);
            panelLogo.Name = "panelLogo";
            panelLogo.Size = new Size(230, 92);
            panelLogo.TabIndex = 0;
            // 
            // lblLogoSub
            // 
            lblLogoSub.AutoSize = true;
            lblLogoSub.ForeColor = Color.FromArgb(196, 176, 157);
            lblLogoSub.Location = new Point(24, 52);
            lblLogoSub.Name = "lblLogoSub";
            lblLogoSub.Size = new Size(145, 20);
            lblLogoSub.TabIndex = 1;
            lblLogoSub.Text = "Coffee Management";
            // 
            // lblLogo
            // 
            lblLogo.AutoSize = true;
            lblLogo.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(22, 21);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(159, 30);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "☕ Cà phê Pro";
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(246, 242, 236);
            panelMain.Controls.Add(panelContent);
            panelMain.Controls.Add(panelTopbar);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(230, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1134, 760);
            panelMain.TabIndex = 1;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(tableMain);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 80);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(22, 16, 22, 22);
            panelContent.Size = new Size(1134, 680);
            panelContent.TabIndex = 1;
            // 
            // tableMain
            // 
            tableMain.ColumnCount = 1;
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableMain.Controls.Add(tableStats, 0, 0);
            tableMain.Controls.Add(tableCharts, 0, 1);
            tableMain.Controls.Add(tableBottom, 0, 2);
            tableMain.Dock = DockStyle.Fill;
            tableMain.Location = new Point(22, 16);
            tableMain.Name = "tableMain";
            tableMain.RowCount = 3;
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 260F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMain.Size = new Size(1090, 642);
            tableMain.TabIndex = 0;
            // 
            // tableStats
            // 
            tableStats.ColumnCount = 4;
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableStats.Controls.Add(cardDoanhThu, 0, 0);
            tableStats.Controls.Add(cardHoaDon, 1, 0);
            tableStats.Controls.Add(cardBan, 2, 0);
            tableStats.Controls.Add(cardMonBan, 3, 0);
            tableStats.Dock = DockStyle.Fill;
            tableStats.Location = new Point(0, 0);
            tableStats.Margin = new Padding(0, 0, 0, 12);
            tableStats.Name = "tableStats";
            tableStats.RowCount = 1;
            tableStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableStats.Size = new Size(1090, 118);
            tableStats.TabIndex = 0;
            // 
            // cardDoanhThu
            // 
            cardDoanhThu.BackColor = Color.FromArgb(237, 247, 243);
            cardDoanhThu.Controls.Add(lblDoanhThuValue);
            cardDoanhThu.Controls.Add(lblDoanhThuTitle);
            cardDoanhThu.Controls.Add(lblDoanhThuIcon);
            cardDoanhThu.Dock = DockStyle.Fill;
            cardDoanhThu.Location = new Point(0, 0);
            cardDoanhThu.Margin = new Padding(0, 0, 8, 0);
            cardDoanhThu.Name = "cardDoanhThu";
            cardDoanhThu.Padding = new Padding(16, 12, 16, 12);
            cardDoanhThu.Size = new Size(264, 118);
            cardDoanhThu.TabIndex = 0;
            // 
            // lblDoanhThuValue
            // 
            lblDoanhThuValue.AutoSize = true;
            lblDoanhThuValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblDoanhThuValue.ForeColor = Color.FromArgb(34, 111, 92);
            lblDoanhThuValue.Location = new Point(18, 61);
            lblDoanhThuValue.Name = "lblDoanhThuValue";
            lblDoanhThuValue.Size = new Size(0, 37);
            lblDoanhThuValue.TabIndex = 2;
            // 
            // lblDoanhThuTitle
            // 
            lblDoanhThuTitle.AutoSize = true;
            lblDoanhThuTitle.Font = new Font("Segoe UI", 10F);
            lblDoanhThuTitle.ForeColor = Color.FromArgb(90, 106, 101);
            lblDoanhThuTitle.Location = new Point(18, 36);
            lblDoanhThuTitle.Name = "lblDoanhThuTitle";
            lblDoanhThuTitle.Size = new Size(206, 23);
            lblDoanhThuTitle.TabIndex = 1;
            lblDoanhThuTitle.Text = "Tổng doanh thu hôm nay";
            // 
            // lblDoanhThuIcon
            // 
            lblDoanhThuIcon.AutoSize = true;
            lblDoanhThuIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblDoanhThuIcon.Location = new Point(16, 8);
            lblDoanhThuIcon.Name = "lblDoanhThuIcon";
            lblDoanhThuIcon.Size = new Size(39, 27);
            lblDoanhThuIcon.TabIndex = 0;
            lblDoanhThuIcon.Text = "💰";
            // 
            // cardHoaDon
            // 
            cardHoaDon.BackColor = Color.FromArgb(255, 247, 235);
            cardHoaDon.Controls.Add(lblHoaDonValue);
            cardHoaDon.Controls.Add(lblHoaDonTitle);
            cardHoaDon.Controls.Add(lblHoaDonIcon);
            cardHoaDon.Dock = DockStyle.Fill;
            cardHoaDon.Location = new Point(272, 0);
            cardHoaDon.Margin = new Padding(0, 0, 8, 0);
            cardHoaDon.Name = "cardHoaDon";
            cardHoaDon.Padding = new Padding(16, 12, 16, 12);
            cardHoaDon.Size = new Size(264, 118);
            cardHoaDon.TabIndex = 1;
            // 
            // lblHoaDonValue
            // 
            lblHoaDonValue.AutoSize = true;
            lblHoaDonValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblHoaDonValue.ForeColor = Color.FromArgb(161, 101, 27);
            lblHoaDonValue.Location = new Point(18, 61);
            lblHoaDonValue.Name = "lblHoaDonValue";
            lblHoaDonValue.Size = new Size(0, 37);
            lblHoaDonValue.TabIndex = 5;
            // 
            // lblHoaDonTitle
            // 
            lblHoaDonTitle.AutoSize = true;
            lblHoaDonTitle.Font = new Font("Segoe UI", 10F);
            lblHoaDonTitle.ForeColor = Color.FromArgb(119, 98, 72);
            lblHoaDonTitle.Location = new Point(18, 36);
            lblHoaDonTitle.Name = "lblHoaDonTitle";
            lblHoaDonTitle.Size = new Size(98, 23);
            lblHoaDonTitle.TabIndex = 4;
            lblHoaDonTitle.Text = "Số hóa đơn";
            // 
            // lblHoaDonIcon
            // 
            lblHoaDonIcon.AutoSize = true;
            lblHoaDonIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblHoaDonIcon.Location = new Point(16, 8);
            lblHoaDonIcon.Name = "lblHoaDonIcon";
            lblHoaDonIcon.Size = new Size(39, 27);
            lblHoaDonIcon.TabIndex = 3;
            lblHoaDonIcon.Text = "📄";
            // 
            // cardBan
            // 
            cardBan.BackColor = Color.FromArgb(246, 241, 255);
            cardBan.Controls.Add(lblBanValue);
            cardBan.Controls.Add(lblBanTitle);
            cardBan.Controls.Add(lblBanIcon);
            cardBan.Dock = DockStyle.Fill;
            cardBan.Location = new Point(544, 0);
            cardBan.Margin = new Padding(0, 0, 8, 0);
            cardBan.Name = "cardBan";
            cardBan.Padding = new Padding(16, 12, 16, 12);
            cardBan.Size = new Size(264, 118);
            cardBan.TabIndex = 2;
            // 
            // lblBanValue
            // 
            lblBanValue.AutoSize = true;
            lblBanValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblBanValue.ForeColor = Color.FromArgb(105, 72, 168);
            lblBanValue.Location = new Point(18, 61);
            lblBanValue.Name = "lblBanValue";
            lblBanValue.Size = new Size(0, 37);
            lblBanValue.TabIndex = 5;
            // 
            // lblBanTitle
            // 
            lblBanTitle.AutoSize = true;
            lblBanTitle.Font = new Font("Segoe UI", 10F);
            lblBanTitle.ForeColor = Color.FromArgb(113, 96, 150);
            lblBanTitle.Location = new Point(18, 36);
            lblBanTitle.Name = "lblBanTitle";
            lblBanTitle.Size = new Size(174, 23);
            lblBanTitle.TabIndex = 4;
            lblBanTitle.Text = "Số bàn đang sử dụng";
            // 
            // lblBanIcon
            // 
            lblBanIcon.AutoSize = true;
            lblBanIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblBanIcon.Location = new Point(16, 8);
            lblBanIcon.Name = "lblBanIcon";
            lblBanIcon.Size = new Size(39, 27);
            lblBanIcon.TabIndex = 3;
            lblBanIcon.Text = "\U0001fa91";
            // 
            // cardMonBan
            // 
            cardMonBan.BackColor = Color.FromArgb(254, 241, 241);
            cardMonBan.Controls.Add(lblMonBanValue);
            cardMonBan.Controls.Add(lblMonBanTitle);
            cardMonBan.Controls.Add(lblMonBanIcon);
            cardMonBan.Dock = DockStyle.Fill;
            cardMonBan.Location = new Point(816, 0);
            cardMonBan.Margin = new Padding(0);
            cardMonBan.Name = "cardMonBan";
            cardMonBan.Padding = new Padding(16, 12, 16, 12);
            cardMonBan.Size = new Size(274, 118);
            cardMonBan.TabIndex = 3;
            // 
            // lblMonBanValue
            // 
            lblMonBanValue.AutoSize = true;
            lblMonBanValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblMonBanValue.ForeColor = Color.FromArgb(162, 52, 64);
            lblMonBanValue.Location = new Point(18, 61);
            lblMonBanValue.Name = "lblMonBanValue";
            lblMonBanValue.Size = new Size(0, 37);
            lblMonBanValue.TabIndex = 5;
            // 
            // lblMonBanTitle
            // 
            lblMonBanTitle.AutoSize = true;
            lblMonBanTitle.Font = new Font("Segoe UI", 10F);
            lblMonBanTitle.ForeColor = Color.FromArgb(130, 90, 95);
            lblMonBanTitle.Location = new Point(18, 36);
            lblMonBanTitle.Name = "lblMonBanTitle";
            lblMonBanTitle.Size = new Size(123, 23);
            lblMonBanTitle.TabIndex = 4;
            lblMonBanTitle.Text = "Số món bán ra";
            // 
            // lblMonBanIcon
            // 
            lblMonBanIcon.AutoSize = true;
            lblMonBanIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblMonBanIcon.Location = new Point(16, 8);
            lblMonBanIcon.Name = "lblMonBanIcon";
            lblMonBanIcon.Size = new Size(39, 27);
            lblMonBanIcon.TabIndex = 3;
            lblMonBanIcon.Text = "🍩";
            // 
            // tableCharts
            // 
            tableCharts.ColumnCount = 2;
            tableCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableCharts.Controls.Add(panelRevenueChart, 0, 0);
            tableCharts.Controls.Add(panelTopSellingChart, 1, 0);
            tableCharts.Dock = DockStyle.Fill;
            tableCharts.Location = new Point(0, 130);
            tableCharts.Margin = new Padding(0, 0, 0, 12);
            tableCharts.Name = "tableCharts";
            tableCharts.RowCount = 1;
            tableCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableCharts.Size = new Size(1090, 248);
            tableCharts.TabIndex = 1;
            // 
            // panelRevenueChart
            // 
            panelRevenueChart.BackColor = Color.White;
            panelRevenueChart.Controls.Add(picRevenueChart);
            panelRevenueChart.Controls.Add(lblRevenueChartTitle);
            panelRevenueChart.Dock = DockStyle.Fill;
            panelRevenueChart.Location = new Point(0, 0);
            panelRevenueChart.Margin = new Padding(0, 0, 8, 0);
            panelRevenueChart.Name = "panelRevenueChart";
            panelRevenueChart.Padding = new Padding(12);
            panelRevenueChart.Size = new Size(646, 248);
            panelRevenueChart.TabIndex = 0;
            // 
            // picRevenueChart
            // 
            picRevenueChart.BackColor = Color.FromArgb(252, 250, 247);
            picRevenueChart.Dock = DockStyle.Fill;
            picRevenueChart.Location = new Point(12, 37);
            picRevenueChart.Name = "picRevenueChart";
            picRevenueChart.Size = new Size(622, 199);
            picRevenueChart.TabIndex = 1;
            picRevenueChart.TabStop = false;
            // 
            // lblRevenueChartTitle
            // 
            lblRevenueChartTitle.AutoSize = true;
            lblRevenueChartTitle.Dock = DockStyle.Top;
            lblRevenueChartTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblRevenueChartTitle.Location = new Point(12, 12);
            lblRevenueChartTitle.Margin = new Padding(3, 0, 3, 8);
            lblRevenueChartTitle.Name = "lblRevenueChartTitle";
            lblRevenueChartTitle.Size = new Size(238, 25);
            lblRevenueChartTitle.TabIndex = 0;
            lblRevenueChartTitle.Text = "Doanh thu theo 7 ngày gần";
            // 
            // panelTopSellingChart
            // 
            panelTopSellingChart.BackColor = Color.White;
            panelTopSellingChart.Controls.Add(tableTopSellingBars);
            panelTopSellingChart.Controls.Add(lblTopSellingTitle);
            panelTopSellingChart.Dock = DockStyle.Fill;
            panelTopSellingChart.Location = new Point(654, 0);
            panelTopSellingChart.Margin = new Padding(0);
            panelTopSellingChart.Name = "panelTopSellingChart";
            panelTopSellingChart.Padding = new Padding(12);
            panelTopSellingChart.Size = new Size(436, 248);
            panelTopSellingChart.TabIndex = 1;
            // 
            // tableTopSellingBars
            // 
            tableTopSellingBars.ColumnCount = 2;
            tableTopSellingBars.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableTopSellingBars.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tableTopSellingBars.Controls.Add(progressTop5, 1, 4);
            tableTopSellingBars.Controls.Add(progressTop4, 1, 3);
            tableTopSellingBars.Controls.Add(progressTop3, 1, 2);
            tableTopSellingBars.Controls.Add(progressTop2, 1, 1);
            tableTopSellingBars.Controls.Add(progressTop1, 1, 0);
            tableTopSellingBars.Controls.Add(lblTop5, 0, 4);
            tableTopSellingBars.Controls.Add(lblTop4, 0, 3);
            tableTopSellingBars.Controls.Add(lblTop3, 0, 2);
            tableTopSellingBars.Controls.Add(lblTop2, 0, 1);
            tableTopSellingBars.Controls.Add(lblTop1, 0, 0);
            tableTopSellingBars.Dock = DockStyle.Fill;
            tableTopSellingBars.Location = new Point(12, 37);
            tableTopSellingBars.Name = "tableTopSellingBars";
            tableTopSellingBars.RowCount = 5;
            tableTopSellingBars.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableTopSellingBars.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableTopSellingBars.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableTopSellingBars.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableTopSellingBars.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableTopSellingBars.Size = new Size(412, 199);
            tableTopSellingBars.TabIndex = 2;
            // 
            // progressTop5
            // 
            progressTop5.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            progressTop5.Location = new Point(188, 165);
            progressTop5.Name = "progressTop5";
            progressTop5.Size = new Size(221, 24);
            progressTop5.Style = ProgressBarStyle.Continuous;
            progressTop5.TabIndex = 9;
            // 
            // progressTop4
            // 
            progressTop4.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            progressTop4.Location = new Point(188, 124);
            progressTop4.Name = "progressTop4";
            progressTop4.Size = new Size(221, 24);
            progressTop4.Style = ProgressBarStyle.Continuous;
            progressTop4.TabIndex = 8;
            // 
            // progressTop3
            // 
            progressTop3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            progressTop3.Location = new Point(188, 85);
            progressTop3.Name = "progressTop3";
            progressTop3.Size = new Size(221, 24);
            progressTop3.Style = ProgressBarStyle.Continuous;
            progressTop3.TabIndex = 7;
            // 
            // progressTop2
            // 
            progressTop2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            progressTop2.Location = new Point(188, 46);
            progressTop2.Name = "progressTop2";
            progressTop2.Size = new Size(221, 24);
            progressTop2.Style = ProgressBarStyle.Continuous;
            progressTop2.TabIndex = 6;
            // 
            // progressTop1
            // 
            progressTop1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            progressTop1.Location = new Point(188, 7);
            progressTop1.Name = "progressTop1";
            progressTop1.Size = new Size(221, 24);
            progressTop1.Style = ProgressBarStyle.Continuous;
            progressTop1.TabIndex = 5;
            // 
            // lblTop5
            // 
            lblTop5.Anchor = AnchorStyles.Left;
            lblTop5.AutoSize = true;
            lblTop5.Font = new Font("Segoe UI", 9.5F);
            lblTop5.ForeColor = Color.DimGray;
            lblTop5.Location = new Point(3, 167);
            lblTop5.Name = "lblTop5";
            lblTop5.Size = new Size(16, 21);
            lblTop5.TabIndex = 4;
            lblTop5.Text = "-";
            // 
            // lblTop4
            // 
            lblTop4.Anchor = AnchorStyles.Left;
            lblTop4.AutoSize = true;
            lblTop4.Font = new Font("Segoe UI", 9.5F);
            lblTop4.ForeColor = Color.DimGray;
            lblTop4.Location = new Point(3, 126);
            lblTop4.Name = "lblTop4";
            lblTop4.Size = new Size(16, 21);
            lblTop4.TabIndex = 3;
            lblTop4.Text = "-";
            // 
            // lblTop3
            // 
            lblTop3.Anchor = AnchorStyles.Left;
            lblTop3.AutoSize = true;
            lblTop3.Font = new Font("Segoe UI", 9.5F);
            lblTop3.ForeColor = Color.DimGray;
            lblTop3.Location = new Point(3, 87);
            lblTop3.Name = "lblTop3";
            lblTop3.Size = new Size(16, 21);
            lblTop3.TabIndex = 2;
            lblTop3.Text = "-";
            // 
            // lblTop2
            // 
            lblTop2.Anchor = AnchorStyles.Left;
            lblTop2.AutoSize = true;
            lblTop2.Font = new Font("Segoe UI", 9.5F);
            lblTop2.ForeColor = Color.DimGray;
            lblTop2.Location = new Point(3, 48);
            lblTop2.Name = "lblTop2";
            lblTop2.Size = new Size(16, 21);
            lblTop2.TabIndex = 1;
            lblTop2.Text = "-";
            // 
            // lblTop1
            // 
            lblTop1.Anchor = AnchorStyles.Left;
            lblTop1.AutoSize = true;
            lblTop1.Font = new Font("Segoe UI", 9.5F);
            lblTop1.ForeColor = Color.DimGray;
            lblTop1.Location = new Point(3, 9);
            lblTop1.Name = "lblTop1";
            lblTop1.Size = new Size(16, 21);
            lblTop1.TabIndex = 0;
            lblTop1.Text = "-";
            // 
            // lblTopSellingTitle
            // 
            lblTopSellingTitle.AutoSize = true;
            lblTopSellingTitle.Dock = DockStyle.Top;
            lblTopSellingTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblTopSellingTitle.Location = new Point(12, 12);
            lblTopSellingTitle.Name = "lblTopSellingTitle";
            lblTopSellingTitle.Size = new Size(128, 25);
            lblTopSellingTitle.TabIndex = 0;
            lblTopSellingTitle.Text = "Món bán chạy";
            // 
            // tableBottom
            // 
            tableBottom.ColumnCount = 2;
            tableBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67F));
            tableBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tableBottom.Controls.Add(panelRecentBills, 0, 0);
            tableBottom.Controls.Add(panelRecentActivity, 1, 0);
            tableBottom.Dock = DockStyle.Fill;
            tableBottom.Location = new Point(0, 390);
            tableBottom.Margin = new Padding(0);
            tableBottom.Name = "tableBottom";
            tableBottom.RowCount = 1;
            tableBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableBottom.Size = new Size(1090, 252);
            tableBottom.TabIndex = 2;
            // 
            // panelRecentBills
            // 
            panelRecentBills.BackColor = Color.White;
            panelRecentBills.Controls.Add(dgvRecentBills);
            panelRecentBills.Controls.Add(lblRecentBillsTitle);
            panelRecentBills.Dock = DockStyle.Fill;
            panelRecentBills.Location = new Point(0, 0);
            panelRecentBills.Margin = new Padding(0, 0, 8, 0);
            panelRecentBills.Name = "panelRecentBills";
            panelRecentBills.Padding = new Padding(12);
            panelRecentBills.Size = new Size(722, 252);
            panelRecentBills.TabIndex = 0;
            // 
            // dgvRecentBills
            // 
            dgvRecentBills.AllowUserToAddRows = false;
            dgvRecentBills.AllowUserToDeleteRows = false;
            dgvRecentBills.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentBills.BackgroundColor = Color.White;
            dgvRecentBills.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvRecentBills.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvRecentBills.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecentBills.Columns.AddRange(new DataGridViewColumn[] { colMaHoaDon, colBan, colThoiGian, colTongTien, colTrangThai });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvRecentBills.DefaultCellStyle = dataGridViewCellStyle2;
            dgvRecentBills.Dock = DockStyle.Fill;
            dgvRecentBills.GridColor = Color.Gainsboro;
            dgvRecentBills.Location = new Point(12, 37);
            dgvRecentBills.MultiSelect = false;
            dgvRecentBills.Name = "dgvRecentBills";
            dgvRecentBills.ReadOnly = true;
            dgvRecentBills.RowHeadersVisible = false;
            dgvRecentBills.RowHeadersWidth = 51;
            dgvRecentBills.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentBills.Size = new Size(698, 203);
            dgvRecentBills.TabIndex = 1;
            // 
            // colMaHoaDon
            // 
            colMaHoaDon.HeaderText = "Mã hóa đơn";
            colMaHoaDon.MinimumWidth = 6;
            colMaHoaDon.Name = "colMaHoaDon";
            colMaHoaDon.ReadOnly = true;
            // 
            // colBan
            // 
            colBan.HeaderText = "Bàn";
            colBan.MinimumWidth = 6;
            colBan.Name = "colBan";
            colBan.ReadOnly = true;
            // 
            // colThoiGian
            // 
            colThoiGian.HeaderText = "Thời gian";
            colThoiGian.MinimumWidth = 6;
            colThoiGian.Name = "colThoiGian";
            colThoiGian.ReadOnly = true;
            // 
            // colTongTien
            // 
            colTongTien.HeaderText = "Tổng tiền";
            colTongTien.MinimumWidth = 6;
            colTongTien.Name = "colTongTien";
            colTongTien.ReadOnly = true;
            // 
            // colTrangThai
            // 
            colTrangThai.HeaderText = "Trạng thái";
            colTrangThai.MinimumWidth = 6;
            colTrangThai.Name = "colTrangThai";
            colTrangThai.ReadOnly = true;
            // 
            // lblRecentBillsTitle
            // 
            lblRecentBillsTitle.AutoSize = true;
            lblRecentBillsTitle.Dock = DockStyle.Top;
            lblRecentBillsTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblRecentBillsTitle.Location = new Point(12, 12);
            lblRecentBillsTitle.Name = "lblRecentBillsTitle";
            lblRecentBillsTitle.Size = new Size(195, 25);
            lblRecentBillsTitle.TabIndex = 0;
            lblRecentBillsTitle.Text = "Hóa đơn gần đây nhất";
            // 
            // panelRecentActivity
            // 
            panelRecentActivity.BackColor = Color.White;
            panelRecentActivity.Controls.Add(listRecentActivity);
            panelRecentActivity.Controls.Add(lblRecentActivityTitle);
            panelRecentActivity.Dock = DockStyle.Fill;
            panelRecentActivity.Location = new Point(730, 0);
            panelRecentActivity.Margin = new Padding(0);
            panelRecentActivity.Name = "panelRecentActivity";
            panelRecentActivity.Padding = new Padding(12);
            panelRecentActivity.Size = new Size(360, 252);
            panelRecentActivity.TabIndex = 1;
            // 
            // listRecentActivity
            // 
            listRecentActivity.BorderStyle = BorderStyle.None;
            listRecentActivity.Dock = DockStyle.Fill;
            listRecentActivity.Font = new Font("Segoe UI", 10F);
            listRecentActivity.FormattingEnabled = true;
            listRecentActivity.Location = new Point(12, 37);
            listRecentActivity.Name = "listRecentActivity";
            listRecentActivity.Size = new Size(336, 203);
            listRecentActivity.TabIndex = 1;
            // 
            // lblRecentActivityTitle
            // 
            lblRecentActivityTitle.AutoSize = true;
            lblRecentActivityTitle.Dock = DockStyle.Top;
            lblRecentActivityTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblRecentActivityTitle.Location = new Point(12, 12);
            lblRecentActivityTitle.Name = "lblRecentActivityTitle";
            lblRecentActivityTitle.Size = new Size(171, 25);
            lblRecentActivityTitle.TabIndex = 0;
            lblRecentActivityTitle.Text = "Hoạt động gần đây";
            // 
            // panelTopbar
            // 
            panelTopbar.BackColor = Color.White;
            panelTopbar.Controls.Add(btnUserMenu);
            panelTopbar.Controls.Add(lblUserName);
            panelTopbar.Controls.Add(picAvatar);
            panelTopbar.Controls.Add(txtSearch);
            panelTopbar.Dock = DockStyle.Top;
            panelTopbar.Location = new Point(0, 0);
            panelTopbar.Name = "panelTopbar";
            panelTopbar.Padding = new Padding(22, 16, 22, 16);
            panelTopbar.Size = new Size(1134, 80);
            panelTopbar.TabIndex = 0;
            // 
            // btnUserMenu
            // 
            btnUserMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUserMenu.FlatAppearance.BorderSize = 0;
            btnUserMenu.FlatStyle = FlatStyle.Flat;
            btnUserMenu.Font = new Font("Segoe UI", 10F);
            btnUserMenu.ForeColor = Color.DimGray;
            btnUserMenu.Location = new Point(1088, 24);
            btnUserMenu.Name = "btnUserMenu";
            btnUserMenu.Size = new Size(24, 28);
            btnUserMenu.TabIndex = 3;
            btnUserMenu.Text = "▾";
            btnUserMenu.UseVisualStyleBackColor = true;
            // 
            // lblUserName
            // 
            lblUserName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblUserName.Location = new Point(936, 27);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(0, 23);
            lblUserName.TabIndex = 2;
            // 
            // picAvatar
            // 
            picAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picAvatar.BackColor = Color.FromArgb(221, 206, 189);
            picAvatar.Location = new Point(892, 20);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new Size(36, 36);
            picAvatar.TabIndex = 1;
            picAvatar.TabStop = false;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(248, 245, 241);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.ForeColor = Color.DimGray;
            txtSearch.Location = new Point(22, 23);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Tìm kiếm bàn, hóa đơn, món...";
            txtSearch.Size = new Size(420, 30);
            txtSearch.TabIndex = 0;
            // 
            // frmDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 242, 236);
            ClientSize = new Size(1364, 760);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1220, 720);
            Name = "frmDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Coffee Shop Dashboard";
            panelSidebar.ResumeLayout(false);
            flowSidebarMenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelMain.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            tableMain.ResumeLayout(false);
            tableStats.ResumeLayout(false);
            cardDoanhThu.ResumeLayout(false);
            cardDoanhThu.PerformLayout();
            cardHoaDon.ResumeLayout(false);
            cardHoaDon.PerformLayout();
            cardBan.ResumeLayout(false);
            cardBan.PerformLayout();
            cardMonBan.ResumeLayout(false);
            cardMonBan.PerformLayout();
            tableCharts.ResumeLayout(false);
            panelRevenueChart.ResumeLayout(false);
            panelRevenueChart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picRevenueChart).EndInit();
            panelTopSellingChart.ResumeLayout(false);
            panelTopSellingChart.PerformLayout();
            tableTopSellingBars.ResumeLayout(false);
            tableTopSellingBars.PerformLayout();
            tableBottom.ResumeLayout(false);
            panelRecentBills.ResumeLayout(false);
            panelRecentBills.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecentBills).EndInit();
            panelRecentActivity.ResumeLayout(false);
            panelRecentActivity.PerformLayout();
            panelTopbar.ResumeLayout(false);
            panelTopbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.FlowLayoutPanel flowSidebarMenu;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnQuanLyBan;
        private System.Windows.Forms.Button btnQuanLyMon;
        private System.Windows.Forms.Button btnLoaiMon;
        private System.Windows.Forms.Button btnHoaDon;
        private System.Windows.Forms.Button btnNhanVien;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.Label lblLogoSub;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelTopbar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.TableLayoutPanel tableStats;
        private System.Windows.Forms.Panel cardDoanhThu;
        private System.Windows.Forms.Label lblDoanhThuValue;
        private System.Windows.Forms.Label lblDoanhThuTitle;
        private System.Windows.Forms.Label lblDoanhThuIcon;
        private System.Windows.Forms.Panel cardHoaDon;
        private System.Windows.Forms.Label lblHoaDonValue;
        private System.Windows.Forms.Label lblHoaDonTitle;
        private System.Windows.Forms.Label lblHoaDonIcon;
        private System.Windows.Forms.Panel cardBan;
        private System.Windows.Forms.Label lblBanValue;
        private System.Windows.Forms.Label lblBanTitle;
        private System.Windows.Forms.Label lblBanIcon;
        private System.Windows.Forms.Panel cardMonBan;
        private System.Windows.Forms.Label lblMonBanValue;
        private System.Windows.Forms.Label lblMonBanTitle;
        private System.Windows.Forms.Label lblMonBanIcon;
        private System.Windows.Forms.TableLayoutPanel tableCharts;
        private System.Windows.Forms.Panel panelRevenueChart;
        private System.Windows.Forms.PictureBox picRevenueChart;
        private System.Windows.Forms.Label lblRevenueChartTitle;
        private System.Windows.Forms.Panel panelTopSellingChart;
        private System.Windows.Forms.TableLayoutPanel tableTopSellingBars;
        private System.Windows.Forms.ProgressBar progressTop5;
        private System.Windows.Forms.ProgressBar progressTop4;
        private System.Windows.Forms.ProgressBar progressTop3;
        private System.Windows.Forms.ProgressBar progressTop2;
        private System.Windows.Forms.ProgressBar progressTop1;
        private System.Windows.Forms.Label lblTop5;
        private System.Windows.Forms.Label lblTop4;
        private System.Windows.Forms.Label lblTop3;
        private System.Windows.Forms.Label lblTop2;
        private System.Windows.Forms.Label lblTop1;
        private System.Windows.Forms.Label lblTopSellingTitle;
        private System.Windows.Forms.TableLayoutPanel tableBottom;
        private System.Windows.Forms.Panel panelRecentBills;
        private System.Windows.Forms.DataGridView dgvRecentBills;
        private System.Windows.Forms.Label lblRecentBillsTitle;
        private System.Windows.Forms.Panel panelRecentActivity;
        private System.Windows.Forms.ListBox listRecentActivity;
        private System.Windows.Forms.Label lblRecentActivityTitle;
        private System.Windows.Forms.Button btnUserMenu;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaHoaDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThoiGian;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
    }
}