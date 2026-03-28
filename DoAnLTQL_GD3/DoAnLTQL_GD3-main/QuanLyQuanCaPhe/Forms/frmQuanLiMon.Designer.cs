namespace QuanLyQuanCaPhe.Forms
{
    partial class frmQuanLiMon
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
            cardTongMon = new Panel();
            lblTongMonValue = new Label();
            lblTongMonTitle = new Label();
            lblTongMonIcon = new Label();
            cardLoaiMon = new Panel();
            lblLoaiMonValue = new Label();
            lblLoaiMonTitle = new Label();
            lblLoaiMonIcon = new Label();
            cardGiaTrungBinh = new Panel();
            lblGiaTrungBinhValue = new Label();
            lblGiaTrungBinhTitle = new Label();
            lblGiaTrungBinhIcon = new Label();
            cardCoHinh = new Panel();
            picCardAnhMon = new PictureBox();
            lblCoHinhTitle = new Label();
            lblCoHinhIcon = new Label();
            tableCenter = new TableLayoutPanel();
            panelThongTinMon = new Panel();
            btnLamMoi = new Button();
            btnXoaMon = new Button();
            btnCapNhatMon = new Button();
            btnThemMon = new Button();
            txtTimMon = new TextBox();
            lblTimMon = new Label();
            txtMoTa = new TextBox();
            lblMoTa = new Label();
            txtDuongDanAnh = new TextBox();
            btnChonHinh = new Button();
            txtDonGia = new TextBox();
            lblDonGia = new Label();
            cboLoaiMon = new ComboBox();
            lblLoaiMon = new Label();
            txtTenMon = new TextBox();
            lblTenMon = new Label();
            txtMaMon = new TextBox();
            lblMaMon = new Label();
            lblThongTinMonTitle = new Label();
            panelDanhSachMon = new Panel();
            dgvDanhSachMon = new DataGridView();
            colIDMon = new DataGridViewTextBoxColumn();
            colTenMon = new DataGridViewTextBoxColumn();
            colLoaiMon = new DataGridViewTextBoxColumn();
            colDonGia = new DataGridViewTextBoxColumn();
            colMoTa = new DataGridViewTextBoxColumn();
            panelDanhSachHeader = new Panel();
            btnNhapMon = new Button();
            btnXuatMon = new Button();
            lblDanhSachMonTitle = new Label();
            panelTopbar = new Panel();
            btnUserMenu = new Button();
            lblUserName = new Label();
            picAvatar = new PictureBox();
            txtSearch = new TextBox();
            lblPageTitle = new Label();
            panelSidebar.SuspendLayout();
            flowSidebarMenu.SuspendLayout();
            panelLogo.SuspendLayout();
            panelMain.SuspendLayout();
            panelContent.SuspendLayout();
            tableMain.SuspendLayout();
            tableStats.SuspendLayout();
            cardTongMon.SuspendLayout();
            cardLoaiMon.SuspendLayout();
            cardGiaTrungBinh.SuspendLayout();
            cardCoHinh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCardAnhMon).BeginInit();
            tableCenter.SuspendLayout();
            panelThongTinMon.SuspendLayout();
            panelDanhSachMon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachMon).BeginInit();
            panelDanhSachHeader.SuspendLayout();
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
            btnDashboard.Font = new Font("Segoe UI", 10F);
            btnDashboard.ForeColor = Color.Gainsboro;
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
            btnQuanLyMon.BackColor = Color.FromArgb(94, 64, 47);
            btnQuanLyMon.FlatAppearance.BorderSize = 0;
            btnQuanLyMon.FlatStyle = FlatStyle.Flat;
            btnQuanLyMon.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnQuanLyMon.ForeColor = Color.White;
            btnQuanLyMon.Location = new Point(0, 110);
            btnQuanLyMon.Margin = new Padding(0);
            btnQuanLyMon.Name = "btnQuanLyMon";
            btnQuanLyMon.Padding = new Padding(20, 0, 0, 0);
            btnQuanLyMon.Size = new Size(230, 48);
            btnQuanLyMon.TabIndex = 2;
            btnQuanLyMon.Text = "☕  Quản lý món";
            btnQuanLyMon.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyMon.UseVisualStyleBackColor = false;
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
            tableMain.Controls.Add(tableCenter, 0, 1);
            tableMain.Dock = DockStyle.Fill;
            tableMain.Location = new Point(22, 16);
            tableMain.Name = "tableMain";
            tableMain.RowCount = 2;
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
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
            tableStats.Controls.Add(cardTongMon, 0, 0);
            tableStats.Controls.Add(cardLoaiMon, 1, 0);
            tableStats.Controls.Add(cardGiaTrungBinh, 2, 0);
            tableStats.Controls.Add(cardCoHinh, 3, 0);
            tableStats.Dock = DockStyle.Fill;
            tableStats.Location = new Point(0, 0);
            tableStats.Margin = new Padding(0, 0, 0, 12);
            tableStats.Name = "tableStats";
            tableStats.RowCount = 1;
            tableStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableStats.Size = new Size(1090, 118);
            tableStats.TabIndex = 0;
            // 
            // cardTongMon
            // 
            cardTongMon.BackColor = Color.FromArgb(237, 247, 243);
            cardTongMon.Controls.Add(lblTongMonValue);
            cardTongMon.Controls.Add(lblTongMonTitle);
            cardTongMon.Controls.Add(lblTongMonIcon);
            cardTongMon.Dock = DockStyle.Fill;
            cardTongMon.Location = new Point(0, 0);
            cardTongMon.Margin = new Padding(0, 0, 8, 0);
            cardTongMon.Name = "cardTongMon";
            cardTongMon.Padding = new Padding(16, 12, 16, 12);
            cardTongMon.Size = new Size(264, 118);
            cardTongMon.TabIndex = 0;
            // 
            // lblTongMonValue
            // 
            lblTongMonValue.AutoSize = true;
            lblTongMonValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTongMonValue.ForeColor = Color.FromArgb(34, 111, 92);
            lblTongMonValue.Location = new Point(18, 61);
            lblTongMonValue.Name = "lblTongMonValue";
            lblTongMonValue.Size = new Size(28, 37);
            lblTongMonValue.TabIndex = 2;
            lblTongMonValue.Text = "-";
            // 
            // lblTongMonTitle
            // 
            lblTongMonTitle.AutoSize = true;
            lblTongMonTitle.Font = new Font("Segoe UI", 10F);
            lblTongMonTitle.ForeColor = Color.FromArgb(90, 106, 101);
            lblTongMonTitle.Location = new Point(18, 36);
            lblTongMonTitle.Name = "lblTongMonTitle";
            lblTongMonTitle.Size = new Size(89, 23);
            lblTongMonTitle.TabIndex = 1;
            lblTongMonTitle.Text = "Tổng món";
            // 
            // lblTongMonIcon
            // 
            lblTongMonIcon.AutoSize = true;
            lblTongMonIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblTongMonIcon.Location = new Point(16, 8);
            lblTongMonIcon.Name = "lblTongMonIcon";
            lblTongMonIcon.Size = new Size(39, 27);
            lblTongMonIcon.TabIndex = 0;
            lblTongMonIcon.Text = "☕";
            // 
            // cardLoaiMon
            // 
            cardLoaiMon.BackColor = Color.FromArgb(255, 247, 235);
            cardLoaiMon.Controls.Add(lblLoaiMonValue);
            cardLoaiMon.Controls.Add(lblLoaiMonTitle);
            cardLoaiMon.Controls.Add(lblLoaiMonIcon);
            cardLoaiMon.Dock = DockStyle.Fill;
            cardLoaiMon.Location = new Point(272, 0);
            cardLoaiMon.Margin = new Padding(0, 0, 8, 0);
            cardLoaiMon.Name = "cardLoaiMon";
            cardLoaiMon.Padding = new Padding(16, 12, 16, 12);
            cardLoaiMon.Size = new Size(264, 118);
            cardLoaiMon.TabIndex = 1;
            // 
            // lblLoaiMonValue
            // 
            lblLoaiMonValue.AutoSize = true;
            lblLoaiMonValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblLoaiMonValue.ForeColor = Color.FromArgb(161, 101, 27);
            lblLoaiMonValue.Location = new Point(18, 61);
            lblLoaiMonValue.Name = "lblLoaiMonValue";
            lblLoaiMonValue.Size = new Size(28, 37);
            lblLoaiMonValue.TabIndex = 2;
            lblLoaiMonValue.Text = "-";
            // 
            // lblLoaiMonTitle
            // 
            lblLoaiMonTitle.AutoSize = true;
            lblLoaiMonTitle.Font = new Font("Segoe UI", 10F);
            lblLoaiMonTitle.ForeColor = Color.FromArgb(119, 98, 72);
            lblLoaiMonTitle.Location = new Point(18, 36);
            lblLoaiMonTitle.Name = "lblLoaiMonTitle";
            lblLoaiMonTitle.Size = new Size(81, 23);
            lblLoaiMonTitle.TabIndex = 1;
            lblLoaiMonTitle.Text = "Loại món";
            // 
            // lblLoaiMonIcon
            // 
            lblLoaiMonIcon.AutoSize = true;
            lblLoaiMonIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblLoaiMonIcon.Location = new Point(16, 8);
            lblLoaiMonIcon.Name = "lblLoaiMonIcon";
            lblLoaiMonIcon.Size = new Size(39, 27);
            lblLoaiMonIcon.TabIndex = 0;
            lblLoaiMonIcon.Text = "🏷";
            // 
            // cardGiaTrungBinh
            // 
            cardGiaTrungBinh.BackColor = Color.FromArgb(246, 241, 255);
            cardGiaTrungBinh.Controls.Add(lblGiaTrungBinhValue);
            cardGiaTrungBinh.Controls.Add(lblGiaTrungBinhTitle);
            cardGiaTrungBinh.Controls.Add(lblGiaTrungBinhIcon);
            cardGiaTrungBinh.Dock = DockStyle.Fill;
            cardGiaTrungBinh.Location = new Point(544, 0);
            cardGiaTrungBinh.Margin = new Padding(0, 0, 8, 0);
            cardGiaTrungBinh.Name = "cardGiaTrungBinh";
            cardGiaTrungBinh.Padding = new Padding(16, 12, 16, 12);
            cardGiaTrungBinh.Size = new Size(264, 118);
            cardGiaTrungBinh.TabIndex = 2;
            // 
            // lblGiaTrungBinhValue
            // 
            lblGiaTrungBinhValue.AutoSize = true;
            lblGiaTrungBinhValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblGiaTrungBinhValue.ForeColor = Color.FromArgb(105, 72, 168);
            lblGiaTrungBinhValue.Location = new Point(18, 61);
            lblGiaTrungBinhValue.Name = "lblGiaTrungBinhValue";
            lblGiaTrungBinhValue.Size = new Size(28, 37);
            lblGiaTrungBinhValue.TabIndex = 2;
            lblGiaTrungBinhValue.Text = "-";
            // 
            // lblGiaTrungBinhTitle
            // 
            lblGiaTrungBinhTitle.AutoSize = true;
            lblGiaTrungBinhTitle.Font = new Font("Segoe UI", 10F);
            lblGiaTrungBinhTitle.ForeColor = Color.FromArgb(113, 96, 150);
            lblGiaTrungBinhTitle.Location = new Point(18, 36);
            lblGiaTrungBinhTitle.Name = "lblGiaTrungBinhTitle";
            lblGiaTrungBinhTitle.Size = new Size(121, 23);
            lblGiaTrungBinhTitle.TabIndex = 1;
            lblGiaTrungBinhTitle.Text = "Giá trung bình";
            // 
            // lblGiaTrungBinhIcon
            // 
            lblGiaTrungBinhIcon.AutoSize = true;
            lblGiaTrungBinhIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblGiaTrungBinhIcon.Location = new Point(16, 8);
            lblGiaTrungBinhIcon.Name = "lblGiaTrungBinhIcon";
            lblGiaTrungBinhIcon.Size = new Size(39, 27);
            lblGiaTrungBinhIcon.TabIndex = 0;
            lblGiaTrungBinhIcon.Text = "💰";
            // 
            // cardCoHinh
            // 
            cardCoHinh.BackColor = Color.FromArgb(237, 242, 255);
            cardCoHinh.Controls.Add(picCardAnhMon);
            cardCoHinh.Controls.Add(lblCoHinhTitle);
            cardCoHinh.Controls.Add(lblCoHinhIcon);
            cardCoHinh.Dock = DockStyle.Fill;
            cardCoHinh.Location = new Point(816, 0);
            cardCoHinh.Margin = new Padding(0);
            cardCoHinh.Name = "cardCoHinh";
            cardCoHinh.Padding = new Padding(16, 12, 16, 12);
            cardCoHinh.Size = new Size(274, 118);
            cardCoHinh.TabIndex = 3;
            // 
            // picCardAnhMon
            // 
            picCardAnhMon.BackColor = Color.White;
            picCardAnhMon.BorderStyle = BorderStyle.FixedSingle;
            picCardAnhMon.Location = new Point(18, 39);
            picCardAnhMon.Name = "picCardAnhMon";
            picCardAnhMon.Size = new Size(238, 67);
            picCardAnhMon.SizeMode = PictureBoxSizeMode.Zoom;
            picCardAnhMon.TabIndex = 2;
            picCardAnhMon.TabStop = false;
            // 
            // lblCoHinhTitle
            // 
            lblCoHinhTitle.AutoSize = true;
            lblCoHinhTitle.Font = new Font("Segoe UI", 10F);
            lblCoHinhTitle.ForeColor = Color.FromArgb(86, 102, 137);
            lblCoHinhTitle.Location = new Point(50, 10);
            lblCoHinhTitle.Name = "lblCoHinhTitle";
            lblCoHinhTitle.Size = new Size(124, 23);
            lblCoHinhTitle.TabIndex = 1;
            lblCoHinhTitle.Text = "Ảnh món chọn";
            // 
            // lblCoHinhIcon
            // 
            lblCoHinhIcon.AutoSize = true;
            lblCoHinhIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblCoHinhIcon.Location = new Point(16, 8);
            lblCoHinhIcon.Name = "lblCoHinhIcon";
            lblCoHinhIcon.Size = new Size(39, 27);
            lblCoHinhIcon.TabIndex = 0;
            lblCoHinhIcon.Text = "🖼";
            // 
            // tableCenter
            // 
            tableCenter.ColumnCount = 2;
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
            tableCenter.Controls.Add(panelThongTinMon, 0, 0);
            tableCenter.Controls.Add(panelDanhSachMon, 1, 0);
            tableCenter.Dock = DockStyle.Fill;
            tableCenter.Location = new Point(0, 130);
            tableCenter.Margin = new Padding(0);
            tableCenter.Name = "tableCenter";
            tableCenter.RowCount = 1;
            tableCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableCenter.Size = new Size(1090, 512);
            tableCenter.TabIndex = 1;
            // 
            // panelThongTinMon
            // 
            panelThongTinMon.BackColor = Color.White;
            panelThongTinMon.Controls.Add(btnLamMoi);
            panelThongTinMon.Controls.Add(btnXoaMon);
            panelThongTinMon.Controls.Add(btnCapNhatMon);
            panelThongTinMon.Controls.Add(btnThemMon);
            panelThongTinMon.Controls.Add(txtTimMon);
            panelThongTinMon.Controls.Add(lblTimMon);
            panelThongTinMon.Controls.Add(txtMoTa);
            panelThongTinMon.Controls.Add(lblMoTa);
            panelThongTinMon.Controls.Add(txtDuongDanAnh);
            panelThongTinMon.Controls.Add(btnChonHinh);
            panelThongTinMon.Controls.Add(txtDonGia);
            panelThongTinMon.Controls.Add(lblDonGia);
            panelThongTinMon.Controls.Add(cboLoaiMon);
            panelThongTinMon.Controls.Add(lblLoaiMon);
            panelThongTinMon.Controls.Add(txtTenMon);
            panelThongTinMon.Controls.Add(lblTenMon);
            panelThongTinMon.Controls.Add(txtMaMon);
            panelThongTinMon.Controls.Add(lblMaMon);
            panelThongTinMon.Controls.Add(lblThongTinMonTitle);
            panelThongTinMon.Dock = DockStyle.Fill;
            panelThongTinMon.Location = new Point(0, 0);
            panelThongTinMon.Margin = new Padding(0, 0, 10, 0);
            panelThongTinMon.Name = "panelThongTinMon";
            panelThongTinMon.Padding = new Padding(16, 14, 16, 14);
            panelThongTinMon.Size = new Size(382, 512);
            panelThongTinMon.TabIndex = 0;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(94, 64, 47);
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(16, 474);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(350, 38);
            btnLamMoi.TabIndex = 18;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnXoaMon
            // 
            btnXoaMon.BackColor = Color.FromArgb(254, 241, 241);
            btnXoaMon.FlatAppearance.BorderSize = 0;
            btnXoaMon.FlatStyle = FlatStyle.Flat;
            btnXoaMon.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnXoaMon.ForeColor = Color.FromArgb(162, 52, 64);
            btnXoaMon.Location = new Point(16, 430);
            btnXoaMon.Name = "btnXoaMon";
            btnXoaMon.Size = new Size(350, 38);
            btnXoaMon.TabIndex = 17;
            btnXoaMon.Text = "Xóa món";
            btnXoaMon.UseVisualStyleBackColor = false;
            btnXoaMon.Click += btnXoaMon_Click;
            // 
            // btnCapNhatMon
            // 
            btnCapNhatMon.BackColor = Color.FromArgb(246, 241, 255);
            btnCapNhatMon.FlatAppearance.BorderSize = 0;
            btnCapNhatMon.FlatStyle = FlatStyle.Flat;
            btnCapNhatMon.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnCapNhatMon.ForeColor = Color.FromArgb(105, 72, 168);
            btnCapNhatMon.Location = new Point(16, 386);
            btnCapNhatMon.Name = "btnCapNhatMon";
            btnCapNhatMon.Size = new Size(350, 38);
            btnCapNhatMon.TabIndex = 16;
            btnCapNhatMon.Text = "Cập nhật món";
            btnCapNhatMon.UseVisualStyleBackColor = false;
            btnCapNhatMon.Click += btnCapNhatMon_Click;
            // 
            // btnThemMon
            // 
            btnThemMon.BackColor = Color.FromArgb(236, 245, 241);
            btnThemMon.FlatAppearance.BorderSize = 0;
            btnThemMon.FlatStyle = FlatStyle.Flat;
            btnThemMon.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnThemMon.ForeColor = Color.FromArgb(34, 111, 92);
            btnThemMon.Location = new Point(16, 342);
            btnThemMon.Name = "btnThemMon";
            btnThemMon.Size = new Size(350, 38);
            btnThemMon.TabIndex = 15;
            btnThemMon.Text = "+ Thêm món";
            btnThemMon.UseVisualStyleBackColor = false;
            btnThemMon.Click += btnThemMon_Click;
            // 
            // txtTimMon
            // 
            txtTimMon.BackColor = Color.FromArgb(248, 245, 241);
            txtTimMon.BorderStyle = BorderStyle.FixedSingle;
            txtTimMon.Font = new Font("Segoe UI", 9.5F);
            txtTimMon.Location = new Point(16, 303);
            txtTimMon.Name = "txtTimMon";
            txtTimMon.PlaceholderText = "Nhập ID, tên món hoặc loại...";
            txtTimMon.Size = new Size(350, 29);
            txtTimMon.TabIndex = 14;
            // 
            // lblTimMon
            // 
            lblTimMon.AutoSize = true;
            lblTimMon.Font = new Font("Segoe UI", 9.5F);
            lblTimMon.ForeColor = Color.DimGray;
            lblTimMon.Location = new Point(16, 277);
            lblTimMon.Name = "lblTimMon";
            lblTimMon.Size = new Size(122, 21);
            lblTimMon.TabIndex = 13;
            lblTimMon.Text = "Tìm kiếm nhanh";
            // 
            // txtMoTa
            // 
            txtMoTa.BackColor = Color.FromArgb(248, 245, 241);
            txtMoTa.BorderStyle = BorderStyle.FixedSingle;
            txtMoTa.Font = new Font("Segoe UI", 9.5F);
            txtMoTa.Location = new Point(14, 222);
            txtMoTa.Multiline = true;
            txtMoTa.Name = "txtMoTa";
            txtMoTa.PlaceholderText = "Mô tả ngắn về món...";
            txtMoTa.Size = new Size(350, 52);
            txtMoTa.TabIndex = 12;
            // 
            // lblMoTa
            // 
            lblMoTa.AutoSize = true;
            lblMoTa.Font = new Font("Segoe UI", 9.5F);
            lblMoTa.ForeColor = Color.DimGray;
            lblMoTa.Location = new Point(14, 198);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new Size(50, 21);
            lblMoTa.TabIndex = 11;
            lblMoTa.Text = "Mô tả";
            // 
            // txtDuongDanAnh
            // 
            txtDuongDanAnh.BackColor = Color.FromArgb(248, 245, 241);
            txtDuongDanAnh.BorderStyle = BorderStyle.FixedSingle;
            txtDuongDanAnh.Font = new Font("Segoe UI", 9.5F);
            txtDuongDanAnh.Location = new Point(16, 168);
            txtDuongDanAnh.Name = "txtDuongDanAnh";
            txtDuongDanAnh.PlaceholderText = "Nhập link URL hoặc tải ảnh từ máy...";
            txtDuongDanAnh.Size = new Size(252, 29);
            txtDuongDanAnh.TabIndex = 10;
            // 
            // btnChonHinh
            // 
            btnChonHinh.BackColor = Color.FromArgb(248, 245, 241);
            btnChonHinh.FlatAppearance.BorderSize = 0;
            btnChonHinh.FlatStyle = FlatStyle.Flat;
            btnChonHinh.Font = new Font("Segoe UI", 8.5F);
            btnChonHinh.ForeColor = Color.FromArgb(65, 48, 39);
            btnChonHinh.Location = new Point(274, 168);
            btnChonHinh.Name = "btnChonHinh";
            btnChonHinh.Size = new Size(92, 29);
            btnChonHinh.TabIndex = 9;
            btnChonHinh.Text = "Tải lên";
            btnChonHinh.UseVisualStyleBackColor = false;
            btnChonHinh.Click += btnChonHinh_Click;
            // 
            // txtDonGia
            // 
            txtDonGia.BackColor = Color.FromArgb(248, 245, 241);
            txtDonGia.BorderStyle = BorderStyle.FixedSingle;
            txtDonGia.Font = new Font("Segoe UI", 9.5F);
            txtDonGia.Location = new Point(16, 129);
            txtDonGia.Name = "txtDonGia";
            txtDonGia.PlaceholderText = "Ví dụ: 30000";
            txtDonGia.Size = new Size(164, 29);
            txtDonGia.TabIndex = 8;
            // 
            // lblDonGia
            // 
            lblDonGia.AutoSize = true;
            lblDonGia.Font = new Font("Segoe UI", 9.5F);
            lblDonGia.ForeColor = Color.DimGray;
            lblDonGia.Location = new Point(16, 103);
            lblDonGia.Name = "lblDonGia";
            lblDonGia.Size = new Size(65, 21);
            lblDonGia.TabIndex = 7;
            lblDonGia.Text = "Đơn giá";
            // 
            // cboLoaiMon
            // 
            cboLoaiMon.BackColor = Color.FromArgb(248, 245, 241);
            cboLoaiMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiMon.FlatStyle = FlatStyle.Flat;
            cboLoaiMon.Font = new Font("Segoe UI", 9.5F);
            cboLoaiMon.FormattingEnabled = true;
            cboLoaiMon.Location = new Point(192, 129);
            cboLoaiMon.Name = "cboLoaiMon";
            cboLoaiMon.Size = new Size(174, 29);
            cboLoaiMon.TabIndex = 6;
            // 
            // lblLoaiMon
            // 
            lblLoaiMon.AutoSize = true;
            lblLoaiMon.Font = new Font("Segoe UI", 9.5F);
            lblLoaiMon.ForeColor = Color.DimGray;
            lblLoaiMon.Location = new Point(192, 103);
            lblLoaiMon.Name = "lblLoaiMon";
            lblLoaiMon.Size = new Size(75, 21);
            lblLoaiMon.TabIndex = 5;
            lblLoaiMon.Text = "Loại món";
            // 
            // txtTenMon
            // 
            txtTenMon.BackColor = Color.FromArgb(248, 245, 241);
            txtTenMon.BorderStyle = BorderStyle.FixedSingle;
            txtTenMon.Font = new Font("Segoe UI", 9.5F);
            txtTenMon.Location = new Point(16, 72);
            txtTenMon.Name = "txtTenMon";
            txtTenMon.PlaceholderText = "Nhập tên món...";
            txtTenMon.Size = new Size(164, 29);
            txtTenMon.TabIndex = 4;
            // 
            // lblTenMon
            // 
            lblTenMon.AutoSize = true;
            lblTenMon.Font = new Font("Segoe UI", 9.5F);
            lblTenMon.ForeColor = Color.DimGray;
            lblTenMon.Location = new Point(16, 46);
            lblTenMon.Name = "lblTenMon";
            lblTenMon.Size = new Size(69, 21);
            lblTenMon.TabIndex = 3;
            lblTenMon.Text = "Tên món";
            // 
            // txtMaMon
            // 
            txtMaMon.BackColor = Color.FromArgb(248, 245, 241);
            txtMaMon.BorderStyle = BorderStyle.FixedSingle;
            txtMaMon.Font = new Font("Segoe UI", 9.5F);
            txtMaMon.Location = new Point(192, 72);
            txtMaMon.Name = "txtMaMon";
            txtMaMon.PlaceholderText = "ID";
            txtMaMon.ReadOnly = true;
            txtMaMon.Size = new Size(171, 29);
            txtMaMon.TabIndex = 2;
            txtMaMon.TextAlign = HorizontalAlignment.Center;
            // 
            // lblMaMon
            // 
            lblMaMon.AutoSize = true;
            lblMaMon.Font = new Font("Segoe UI", 9F);
            lblMaMon.ForeColor = Color.Gray;
            lblMaMon.Location = new Point(192, 46);
            lblMaMon.Name = "lblMaMon";
            lblMaMon.Size = new Size(27, 20);
            lblMaMon.TabIndex = 1;
            lblMaMon.Text = "ID:";
            // 
            // lblThongTinMonTitle
            // 
            lblThongTinMonTitle.AutoSize = true;
            lblThongTinMonTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblThongTinMonTitle.Location = new Point(14, 16);
            lblThongTinMonTitle.Name = "lblThongTinMonTitle";
            lblThongTinMonTitle.Size = new Size(136, 25);
            lblThongTinMonTitle.TabIndex = 0;
            lblThongTinMonTitle.Text = "Thông tin món";
            // 
            // panelDanhSachMon
            // 
            panelDanhSachMon.BackColor = Color.White;
            panelDanhSachMon.Controls.Add(dgvDanhSachMon);
            panelDanhSachMon.Controls.Add(panelDanhSachHeader);
            panelDanhSachMon.Dock = DockStyle.Fill;
            panelDanhSachMon.Location = new Point(392, 0);
            panelDanhSachMon.Margin = new Padding(0);
            panelDanhSachMon.Name = "panelDanhSachMon";
            panelDanhSachMon.Padding = new Padding(14, 12, 14, 12);
            panelDanhSachMon.Size = new Size(698, 512);
            panelDanhSachMon.TabIndex = 1;
            // 
            // dgvDanhSachMon
            // 
            dgvDanhSachMon.AllowUserToAddRows = false;
            dgvDanhSachMon.AllowUserToDeleteRows = false;
            dgvDanhSachMon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhSachMon.BackgroundColor = Color.White;
            dgvDanhSachMon.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDanhSachMon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDanhSachMon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDanhSachMon.Columns.AddRange(new DataGridViewColumn[] { colIDMon, colTenMon, colLoaiMon, colDonGia, colMoTa });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDanhSachMon.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDanhSachMon.Dock = DockStyle.Fill;
            dgvDanhSachMon.GridColor = Color.Gainsboro;
            dgvDanhSachMon.Location = new Point(14, 62);
            dgvDanhSachMon.MultiSelect = false;
            dgvDanhSachMon.Name = "dgvDanhSachMon";
            dgvDanhSachMon.ReadOnly = true;
            dgvDanhSachMon.RowHeadersVisible = false;
            dgvDanhSachMon.RowHeadersWidth = 51;
            dgvDanhSachMon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhSachMon.Size = new Size(670, 438);
            dgvDanhSachMon.TabIndex = 1;
            dgvDanhSachMon.SelectionChanged += dgvDanhSachMon_SelectionChanged;
            // 
            // colIDMon
            // 
            colIDMon.FillWeight = 50F;
            colIDMon.HeaderText = "ID";
            colIDMon.MinimumWidth = 6;
            colIDMon.Name = "colIDMon";
            colIDMon.ReadOnly = true;
            // 
            // colTenMon
            // 
            colTenMon.FillWeight = 120F;
            colTenMon.HeaderText = "Tên món";
            colTenMon.MinimumWidth = 6;
            colTenMon.Name = "colTenMon";
            colTenMon.ReadOnly = true;
            // 
            // colLoaiMon
            // 
            colLoaiMon.FillWeight = 90F;
            colLoaiMon.HeaderText = "Loại món";
            colLoaiMon.MinimumWidth = 6;
            colLoaiMon.Name = "colLoaiMon";
            colLoaiMon.ReadOnly = true;
            // 
            // colDonGia
            // 
            colDonGia.FillWeight = 80F;
            colDonGia.HeaderText = "Đơn giá";
            colDonGia.MinimumWidth = 6;
            colDonGia.Name = "colDonGia";
            colDonGia.ReadOnly = true;
            // 
            // colMoTa
            // 
            colMoTa.FillWeight = 190F;
            colMoTa.HeaderText = "Mô tả";
            colMoTa.MinimumWidth = 6;
            colMoTa.Name = "colMoTa";
            colMoTa.ReadOnly = true;
            // 
            // panelDanhSachHeader
            // 
            panelDanhSachHeader.Controls.Add(btnNhapMon);
            panelDanhSachHeader.Controls.Add(btnXuatMon);
            panelDanhSachHeader.Controls.Add(lblDanhSachMonTitle);
            panelDanhSachHeader.Dock = DockStyle.Top;
            panelDanhSachHeader.Location = new Point(14, 12);
            panelDanhSachHeader.Name = "panelDanhSachHeader";
            panelDanhSachHeader.Size = new Size(670, 50);
            panelDanhSachHeader.TabIndex = 0;
            // 
            // btnNhapMon
            // 
            btnNhapMon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNhapMon.BackColor = Color.FromArgb(248, 245, 241);
            btnNhapMon.FlatAppearance.BorderSize = 0;
            btnNhapMon.FlatStyle = FlatStyle.Flat;
            btnNhapMon.Font = new Font("Segoe UI", 8.5F);
            btnNhapMon.ForeColor = Color.FromArgb(65, 48, 39);
            btnNhapMon.Location = new Point(486, 9);
            btnNhapMon.Name = "btnNhapMon";
            btnNhapMon.Size = new Size(88, 30);
            btnNhapMon.TabIndex = 2;
            btnNhapMon.Text = "Nhập";
            btnNhapMon.UseVisualStyleBackColor = false;
            // 
            // btnXuatMon
            // 
            btnXuatMon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXuatMon.BackColor = Color.FromArgb(248, 245, 241);
            btnXuatMon.FlatAppearance.BorderSize = 0;
            btnXuatMon.FlatStyle = FlatStyle.Flat;
            btnXuatMon.Font = new Font("Segoe UI", 8.5F);
            btnXuatMon.ForeColor = Color.FromArgb(65, 48, 39);
            btnXuatMon.Location = new Point(582, 9);
            btnXuatMon.Name = "btnXuatMon";
            btnXuatMon.Size = new Size(88, 30);
            btnXuatMon.TabIndex = 1;
            btnXuatMon.Text = "Xuất";
            btnXuatMon.UseVisualStyleBackColor = false;
            // 
            // lblDanhSachMonTitle
            // 
            lblDanhSachMonTitle.AutoSize = true;
            lblDanhSachMonTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblDanhSachMonTitle.Location = new Point(0, 12);
            lblDanhSachMonTitle.Name = "lblDanhSachMonTitle";
            lblDanhSachMonTitle.Size = new Size(138, 25);
            lblDanhSachMonTitle.TabIndex = 0;
            lblDanhSachMonTitle.Text = "Danh sách món";
            // 
            // panelTopbar
            // 
            panelTopbar.BackColor = Color.White;
            panelTopbar.Controls.Add(btnUserMenu);
            panelTopbar.Controls.Add(lblUserName);
            panelTopbar.Controls.Add(picAvatar);
            panelTopbar.Controls.Add(txtSearch);
            panelTopbar.Controls.Add(lblPageTitle);
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
            btnUserMenu.TabIndex = 4;
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
            lblUserName.Size = new Size(135, 23);
            lblUserName.TabIndex = 3;
            lblUserName.Text = "Manager Coffee";
            // 
            // picAvatar
            // 
            picAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picAvatar.BackColor = Color.FromArgb(221, 206, 189);
            picAvatar.Location = new Point(892, 20);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new Size(36, 36);
            picAvatar.TabIndex = 2;
            picAvatar.TabStop = false;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(248, 245, 241);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.ForeColor = Color.DimGray;
            txtSearch.Location = new Point(240, 23);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Tìm món theo ID, tên món hoặc loại...";
            txtSearch.Size = new Size(430, 30);
            txtSearch.TabIndex = 1;
            // 
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(65, 48, 39);
            lblPageTitle.Location = new Point(22, 23);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(130, 28);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Quản lý món";
            // 
            // frmQuanLiMon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 242, 236);
            ClientSize = new Size(1364, 760);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1220, 720);
            Name = "frmQuanLiMon";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Coffee Shop - Quản lý món";
            panelSidebar.ResumeLayout(false);
            flowSidebarMenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelMain.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            tableMain.ResumeLayout(false);
            tableStats.ResumeLayout(false);
            cardTongMon.ResumeLayout(false);
            cardTongMon.PerformLayout();
            cardLoaiMon.ResumeLayout(false);
            cardLoaiMon.PerformLayout();
            cardGiaTrungBinh.ResumeLayout(false);
            cardGiaTrungBinh.PerformLayout();
            cardCoHinh.ResumeLayout(false);
            cardCoHinh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picCardAnhMon).EndInit();
            tableCenter.ResumeLayout(false);
            panelThongTinMon.ResumeLayout(false);
            panelThongTinMon.PerformLayout();
            panelDanhSachMon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachMon).EndInit();
            panelDanhSachHeader.ResumeLayout(false);
            panelDanhSachHeader.PerformLayout();
            panelTopbar.ResumeLayout(false);
            panelTopbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
        private Button btnDangXuat;
        private FlowLayoutPanel flowSidebarMenu;
        private Button btnDashboard;
        private Button btnQuanLyBan;
        private Button btnQuanLyMon;
        private Button btnLoaiMon;
        private Button btnHoaDon;
        private Button btnNhanVien;
        private Button btnThongKe;
        private Panel panelLogo;
        private Label lblLogoSub;
        private Label lblLogo;
        private Panel panelMain;
        private Panel panelContent;
        private TableLayoutPanel tableMain;
        private TableLayoutPanel tableStats;
        private Panel cardTongMon;
        private Label lblTongMonValue;
        private Label lblTongMonTitle;
        private Label lblTongMonIcon;
        private Panel cardLoaiMon;
        private Label lblLoaiMonValue;
        private Label lblLoaiMonTitle;
        private Label lblLoaiMonIcon;
        private Panel cardGiaTrungBinh;
        private Label lblGiaTrungBinhValue;
        private Label lblGiaTrungBinhTitle;
        private Label lblGiaTrungBinhIcon;
        private Panel cardCoHinh;
        private PictureBox picCardAnhMon;
        private Label lblCoHinhTitle;
        private Label lblCoHinhIcon;
        private TableLayoutPanel tableCenter;
        private Panel panelThongTinMon;
        private Button btnLamMoi;
        private Button btnXoaMon;
        private Button btnCapNhatMon;
        private Button btnThemMon;
        private TextBox txtTimMon;
        private Label lblTimMon;
        private TextBox txtMoTa;
        private Label lblMoTa;
        private TextBox txtDuongDanAnh;
        private Button btnChonHinh;
        private TextBox txtDonGia;
        private Label lblDonGia;
        private ComboBox cboLoaiMon;
        private Label lblLoaiMon;
        private TextBox txtTenMon;
        private Label lblTenMon;
        private TextBox txtMaMon;
        private Label lblMaMon;
        private Label lblThongTinMonTitle;
        private Panel panelDanhSachMon;
        private DataGridView dgvDanhSachMon;
        private DataGridViewTextBoxColumn colIDMon;
        private DataGridViewTextBoxColumn colTenMon;
        private DataGridViewTextBoxColumn colLoaiMon;
        private DataGridViewTextBoxColumn colDonGia;
        private DataGridViewTextBoxColumn colMoTa;
        private Panel panelDanhSachHeader;
        private Button btnNhapMon;
        private Button btnXuatMon;
        private Label lblDanhSachMonTitle;
        private Panel panelTopbar;
        private Button btnUserMenu;
        private Label lblUserName;
        private PictureBox picAvatar;
        private TextBox txtSearch;
        private Label lblPageTitle;
    }
}