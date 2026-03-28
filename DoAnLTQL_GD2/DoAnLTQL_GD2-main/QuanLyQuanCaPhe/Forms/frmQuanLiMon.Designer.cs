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
            cardDangKinhDoanh = new Panel();
            lblDangKinhDoanhValue = new Label();
            lblDangKinhDoanhTitle = new Label();
            lblDangKinhDoanhIcon = new Label();
            cardHetMon = new Panel();
            lblHetMonValue = new Label();
            lblHetMonTitle = new Label();
            lblHetMonIcon = new Label();
            cardLoaiMon = new Panel();
            lblLoaiMonValue = new Label();
            lblLoaiMonTitle = new Label();
            lblLoaiMonIcon = new Label();
            tableCenter = new TableLayoutPanel();
            panelFilter = new Panel();
            btnLamMoi = new Button();
            btnXoaMon = new Button();
            btnCapNhatMon = new Button();
            btnThemMon = new Button();
            cboTrangThai = new ComboBox();
            lblTrangThai = new Label();
            cboLoaiMon = new ComboBox();
            lblLoaiMonFilter = new Label();
            txtTimMon = new TextBox();
            lblTimMon = new Label();
            lblBoLocTitle = new Label();
            panelDanhSachMon = new Panel();
            dgvDanhSachMon = new DataGridView();
            colMaMon = new DataGridViewTextBoxColumn();
            colTenMon = new DataGridViewTextBoxColumn();
            colLoaiMon = new DataGridViewTextBoxColumn();
            colDonGia = new DataGridViewTextBoxColumn();
            colTrangThai = new DataGridViewTextBoxColumn();
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
            cardDangKinhDoanh.SuspendLayout();
            cardHetMon.SuspendLayout();
            cardLoaiMon.SuspendLayout();
            tableCenter.SuspendLayout();
            panelFilter.SuspendLayout();
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
            tableStats.Controls.Add(cardDangKinhDoanh, 1, 0);
            tableStats.Controls.Add(cardHetMon, 2, 0);
            tableStats.Controls.Add(cardLoaiMon, 3, 0);
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
            // cardDangKinhDoanh
            // 
            cardDangKinhDoanh.BackColor = Color.FromArgb(255, 247, 235);
            cardDangKinhDoanh.Controls.Add(lblDangKinhDoanhValue);
            cardDangKinhDoanh.Controls.Add(lblDangKinhDoanhTitle);
            cardDangKinhDoanh.Controls.Add(lblDangKinhDoanhIcon);
            cardDangKinhDoanh.Dock = DockStyle.Fill;
            cardDangKinhDoanh.Location = new Point(272, 0);
            cardDangKinhDoanh.Margin = new Padding(0, 0, 8, 0);
            cardDangKinhDoanh.Name = "cardDangKinhDoanh";
            cardDangKinhDoanh.Padding = new Padding(16, 12, 16, 12);
            cardDangKinhDoanh.Size = new Size(264, 118);
            cardDangKinhDoanh.TabIndex = 1;
            // 
            // lblDangKinhDoanhValue
            // 
            lblDangKinhDoanhValue.AutoSize = true;
            lblDangKinhDoanhValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblDangKinhDoanhValue.ForeColor = Color.FromArgb(161, 101, 27);
            lblDangKinhDoanhValue.Location = new Point(18, 61);
            lblDangKinhDoanhValue.Name = "lblDangKinhDoanhValue";
            lblDangKinhDoanhValue.Size = new Size(28, 37);
            lblDangKinhDoanhValue.TabIndex = 5;
            lblDangKinhDoanhValue.Text = "-";
            // 
            // lblDangKinhDoanhTitle
            // 
            lblDangKinhDoanhTitle.AutoSize = true;
            lblDangKinhDoanhTitle.Font = new Font("Segoe UI", 10F);
            lblDangKinhDoanhTitle.ForeColor = Color.FromArgb(119, 98, 72);
            lblDangKinhDoanhTitle.Location = new Point(18, 36);
            lblDangKinhDoanhTitle.Name = "lblDangKinhDoanhTitle";
            lblDangKinhDoanhTitle.Size = new Size(142, 23);
            lblDangKinhDoanhTitle.TabIndex = 4;
            lblDangKinhDoanhTitle.Text = "Đang kinh doanh";
            // 
            // lblDangKinhDoanhIcon
            // 
            lblDangKinhDoanhIcon.AutoSize = true;
            lblDangKinhDoanhIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblDangKinhDoanhIcon.Location = new Point(16, 8);
            lblDangKinhDoanhIcon.Name = "lblDangKinhDoanhIcon";
            lblDangKinhDoanhIcon.Size = new Size(39, 27);
            lblDangKinhDoanhIcon.TabIndex = 3;
            lblDangKinhDoanhIcon.Text = "✅";
            // 
            // cardHetMon
            // 
            cardHetMon.BackColor = Color.FromArgb(246, 241, 255);
            cardHetMon.Controls.Add(lblHetMonValue);
            cardHetMon.Controls.Add(lblHetMonTitle);
            cardHetMon.Controls.Add(lblHetMonIcon);
            cardHetMon.Dock = DockStyle.Fill;
            cardHetMon.Location = new Point(544, 0);
            cardHetMon.Margin = new Padding(0, 0, 8, 0);
            cardHetMon.Name = "cardHetMon";
            cardHetMon.Padding = new Padding(16, 12, 16, 12);
            cardHetMon.Size = new Size(264, 118);
            cardHetMon.TabIndex = 2;
            // 
            // lblHetMonValue
            // 
            lblHetMonValue.AutoSize = true;
            lblHetMonValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblHetMonValue.ForeColor = Color.FromArgb(105, 72, 168);
            lblHetMonValue.Location = new Point(18, 61);
            lblHetMonValue.Name = "lblHetMonValue";
            lblHetMonValue.Size = new Size(28, 37);
            lblHetMonValue.TabIndex = 5;
            lblHetMonValue.Text = "-";
            // 
            // lblHetMonTitle
            // 
            lblHetMonTitle.AutoSize = true;
            lblHetMonTitle.Font = new Font("Segoe UI", 10F);
            lblHetMonTitle.ForeColor = Color.FromArgb(113, 96, 150);
            lblHetMonTitle.Location = new Point(18, 36);
            lblHetMonTitle.Name = "lblHetMonTitle";
            lblHetMonTitle.Size = new Size(73, 23);
            lblHetMonTitle.TabIndex = 4;
            lblHetMonTitle.Text = "Tạm hết";
            // 
            // lblHetMonIcon
            // 
            lblHetMonIcon.AutoSize = true;
            lblHetMonIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblHetMonIcon.Location = new Point(16, 8);
            lblHetMonIcon.Name = "lblHetMonIcon";
            lblHetMonIcon.Size = new Size(39, 27);
            lblHetMonIcon.TabIndex = 3;
            lblHetMonIcon.Text = "⏳";
            // 
            // cardLoaiMon
            // 
            cardLoaiMon.BackColor = Color.FromArgb(254, 241, 241);
            cardLoaiMon.Controls.Add(lblLoaiMonValue);
            cardLoaiMon.Controls.Add(lblLoaiMonTitle);
            cardLoaiMon.Controls.Add(lblLoaiMonIcon);
            cardLoaiMon.Dock = DockStyle.Fill;
            cardLoaiMon.Location = new Point(816, 0);
            cardLoaiMon.Margin = new Padding(0);
            cardLoaiMon.Name = "cardLoaiMon";
            cardLoaiMon.Padding = new Padding(16, 12, 16, 12);
            cardLoaiMon.Size = new Size(274, 118);
            cardLoaiMon.TabIndex = 3;
            // 
            // lblLoaiMonValue
            // 
            lblLoaiMonValue.AutoSize = true;
            lblLoaiMonValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblLoaiMonValue.ForeColor = Color.FromArgb(162, 52, 64);
            lblLoaiMonValue.Location = new Point(18, 61);
            lblLoaiMonValue.Name = "lblLoaiMonValue";
            lblLoaiMonValue.Size = new Size(28, 37);
            lblLoaiMonValue.TabIndex = 5;
            lblLoaiMonValue.Text = "-";
            // 
            // lblLoaiMonTitle
            // 
            lblLoaiMonTitle.AutoSize = true;
            lblLoaiMonTitle.Font = new Font("Segoe UI", 10F);
            lblLoaiMonTitle.ForeColor = Color.FromArgb(130, 90, 95);
            lblLoaiMonTitle.Location = new Point(18, 36);
            lblLoaiMonTitle.Name = "lblLoaiMonTitle";
            lblLoaiMonTitle.Size = new Size(81, 23);
            lblLoaiMonTitle.TabIndex = 4;
            lblLoaiMonTitle.Text = "Loại món";
            // 
            // lblLoaiMonIcon
            // 
            lblLoaiMonIcon.AutoSize = true;
            lblLoaiMonIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblLoaiMonIcon.Location = new Point(16, 8);
            lblLoaiMonIcon.Name = "lblLoaiMonIcon";
            lblLoaiMonIcon.Size = new Size(39, 27);
            lblLoaiMonIcon.TabIndex = 3;
            lblLoaiMonIcon.Text = "🏷";
            // 
            // tableCenter
            // 
            tableCenter.ColumnCount = 2;
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            tableCenter.Controls.Add(panelFilter, 0, 0);
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
            // panelFilter
            // 
            panelFilter.BackColor = Color.White;
            panelFilter.Controls.Add(btnLamMoi);
            panelFilter.Controls.Add(btnXoaMon);
            panelFilter.Controls.Add(btnCapNhatMon);
            panelFilter.Controls.Add(btnThemMon);
            panelFilter.Controls.Add(cboTrangThai);
            panelFilter.Controls.Add(lblTrangThai);
            panelFilter.Controls.Add(cboLoaiMon);
            panelFilter.Controls.Add(lblLoaiMonFilter);
            panelFilter.Controls.Add(txtTimMon);
            panelFilter.Controls.Add(lblTimMon);
            panelFilter.Controls.Add(lblBoLocTitle);
            panelFilter.Dock = DockStyle.Fill;
            panelFilter.Location = new Point(0, 0);
            panelFilter.Margin = new Padding(0, 0, 10, 0);
            panelFilter.Name = "panelFilter";
            panelFilter.Padding = new Padding(16, 14, 16, 14);
            panelFilter.Size = new Size(338, 512);
            panelFilter.TabIndex = 0;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(94, 64, 47);
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(16, 452);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(306, 38);
            btnLamMoi.TabIndex = 10;
            btnLamMoi.Text = "Làm mới bộ lọc";
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
            btnXoaMon.Location = new Point(16, 408);
            btnXoaMon.Name = "btnXoaMon";
            btnXoaMon.Size = new Size(306, 38);
            btnXoaMon.TabIndex = 9;
            btnXoaMon.Text = "Xóa món đã chọn";
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
            btnCapNhatMon.Location = new Point(16, 364);
            btnCapNhatMon.Name = "btnCapNhatMon";
            btnCapNhatMon.Size = new Size(306, 38);
            btnCapNhatMon.TabIndex = 8;
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
            btnThemMon.Location = new Point(16, 320);
            btnThemMon.Name = "btnThemMon";
            btnThemMon.Size = new Size(306, 38);
            btnThemMon.TabIndex = 7;
            btnThemMon.Text = "+ Thêm món mới";
            btnThemMon.UseVisualStyleBackColor = false;
            btnThemMon.Click += btnThemMon_Click;
            // 
            // cboTrangThai
            // 
            cboTrangThai.BackColor = Color.FromArgb(248, 245, 241);
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.FlatStyle = FlatStyle.Flat;
            cboTrangThai.FormattingEnabled = true;
            cboTrangThai.Items.AddRange(new object[] { "Tất cả trạng thái", "Đang kinh doanh", "Tạm hết", "Ngừng bán" });
            cboTrangThai.Location = new Point(16, 264);
            cboTrangThai.Name = "cboTrangThai";
            cboTrangThai.Size = new Size(306, 28);
            cboTrangThai.TabIndex = 6;
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Font = new Font("Segoe UI", 9.5F);
            lblTrangThai.ForeColor = Color.DimGray;
            lblTrangThai.Location = new Point(16, 238);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(79, 21);
            lblTrangThai.TabIndex = 5;
            lblTrangThai.Text = "Trạng thái";
            // 
            // cboLoaiMon
            // 
            cboLoaiMon.BackColor = Color.FromArgb(248, 245, 241);
            cboLoaiMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiMon.FlatStyle = FlatStyle.Flat;
            cboLoaiMon.FormattingEnabled = true;
            cboLoaiMon.Items.AddRange(new object[] { "Tất cả loại", "Cà phê", "Trà", "Nước ép", "Bánh ngọt" });
            cboLoaiMon.Location = new Point(16, 197);
            cboLoaiMon.Name = "cboLoaiMon";
            cboLoaiMon.Size = new Size(306, 28);
            cboLoaiMon.TabIndex = 4;
            // 
            // lblLoaiMonFilter
            // 
            lblLoaiMonFilter.AutoSize = true;
            lblLoaiMonFilter.Font = new Font("Segoe UI", 9.5F);
            lblLoaiMonFilter.ForeColor = Color.DimGray;
            lblLoaiMonFilter.Location = new Point(16, 171);
            lblLoaiMonFilter.Name = "lblLoaiMonFilter";
            lblLoaiMonFilter.Size = new Size(75, 21);
            lblLoaiMonFilter.TabIndex = 3;
            lblLoaiMonFilter.Text = "Loại món";
            // 
            // txtTimMon
            // 
            txtTimMon.BackColor = Color.FromArgb(248, 245, 241);
            txtTimMon.BorderStyle = BorderStyle.FixedSingle;
            txtTimMon.Font = new Font("Segoe UI", 9.5F);
            txtTimMon.Location = new Point(16, 129);
            txtTimMon.Name = "txtTimMon";
            txtTimMon.PlaceholderText = "Nhập mã hoặc tên món...";
            txtTimMon.Size = new Size(306, 29);
            txtTimMon.TabIndex = 2;
            // 
            // lblTimMon
            // 
            lblTimMon.AutoSize = true;
            lblTimMon.Font = new Font("Segoe UI", 9.5F);
            lblTimMon.ForeColor = Color.DimGray;
            lblTimMon.Location = new Point(16, 103);
            lblTimMon.Name = "lblTimMon";
            lblTimMon.Size = new Size(122, 21);
            lblTimMon.TabIndex = 1;
            lblTimMon.Text = "Tìm kiếm nhanh";
            // 
            // lblBoLocTitle
            // 
            lblBoLocTitle.AutoSize = true;
            lblBoLocTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblBoLocTitle.Location = new Point(14, 16);
            lblBoLocTitle.Name = "lblBoLocTitle";
            lblBoLocTitle.Size = new Size(157, 25);
            lblBoLocTitle.TabIndex = 0;
            lblBoLocTitle.Text = "Bộ lọc và thao tác";
            // 
            // panelDanhSachMon
            // 
            panelDanhSachMon.BackColor = Color.White;
            panelDanhSachMon.Controls.Add(dgvDanhSachMon);
            panelDanhSachMon.Controls.Add(panelDanhSachHeader);
            panelDanhSachMon.Dock = DockStyle.Fill;
            panelDanhSachMon.Location = new Point(348, 0);
            panelDanhSachMon.Margin = new Padding(0);
            panelDanhSachMon.Name = "panelDanhSachMon";
            panelDanhSachMon.Padding = new Padding(12);
            panelDanhSachMon.Size = new Size(742, 512);
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
            dgvDanhSachMon.Columns.AddRange(new DataGridViewColumn[] { colMaMon, colTenMon, colLoaiMon, colDonGia, colTrangThai, colMoTa });
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
            dgvDanhSachMon.Location = new Point(12, 55);
            dgvDanhSachMon.MultiSelect = false;
            dgvDanhSachMon.Name = "dgvDanhSachMon";
            dgvDanhSachMon.ReadOnly = true;
            dgvDanhSachMon.RowHeadersVisible = false;
            dgvDanhSachMon.RowHeadersWidth = 51;
            dgvDanhSachMon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhSachMon.Size = new Size(718, 445);
            dgvDanhSachMon.TabIndex = 1;
            // 
            // colMaMon
            // 
            colMaMon.FillWeight = 65F;
            colMaMon.HeaderText = "Mã món";
            colMaMon.MinimumWidth = 6;
            colMaMon.Name = "colMaMon";
            colMaMon.ReadOnly = true;
            // 
            // colTenMon
            // 
            colTenMon.FillWeight = 130F;
            colTenMon.HeaderText = "Tên món";
            colTenMon.MinimumWidth = 6;
            colTenMon.Name = "colTenMon";
            colTenMon.ReadOnly = true;
            // 
            // colLoaiMon
            // 
            colLoaiMon.FillWeight = 90F;
            colLoaiMon.HeaderText = "Loại";
            colLoaiMon.MinimumWidth = 6;
            colLoaiMon.Name = "colLoaiMon";
            colLoaiMon.ReadOnly = true;
            // 
            // colDonGia
            // 
            colDonGia.FillWeight = 90F;
            colDonGia.HeaderText = "Đơn giá";
            colDonGia.MinimumWidth = 6;
            colDonGia.Name = "colDonGia";
            colDonGia.ReadOnly = true;
            // 
            // colTrangThai
            // 
            colTrangThai.FillWeight = 95F;
            colTrangThai.HeaderText = "Trạng thái";
            colTrangThai.MinimumWidth = 6;
            colTrangThai.Name = "colTrangThai";
            colTrangThai.ReadOnly = true;
            // 
            // colMoTa
            // 
            colMoTa.FillWeight = 140F;
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
            panelDanhSachHeader.Location = new Point(12, 12);
            panelDanhSachHeader.Name = "panelDanhSachHeader";
            panelDanhSachHeader.Size = new Size(718, 43);
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
            btnNhapMon.Location = new Point(536, 4);
            btnNhapMon.Name = "btnNhapMon";
            btnNhapMon.Size = new Size(84, 32);
            btnNhapMon.TabIndex = 2;
            btnNhapMon.Text = "Nhập";
            btnNhapMon.UseVisualStyleBackColor = false;
            btnNhapMon.Click += btnNhapMon_Click;
            // 
            // btnXuatMon
            // 
            btnXuatMon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXuatMon.BackColor = Color.FromArgb(248, 245, 241);
            btnXuatMon.FlatAppearance.BorderSize = 0;
            btnXuatMon.FlatStyle = FlatStyle.Flat;
            btnXuatMon.Font = new Font("Segoe UI", 8.5F);
            btnXuatMon.ForeColor = Color.FromArgb(65, 48, 39);
            btnXuatMon.Location = new Point(626, 4);
            btnXuatMon.Name = "btnXuatMon";
            btnXuatMon.Size = new Size(84, 32);
            btnXuatMon.TabIndex = 1;
            btnXuatMon.Text = "Xuất";
            btnXuatMon.UseVisualStyleBackColor = false;
            btnXuatMon.Click += btnXuatMon_Click;
            // 
            // lblDanhSachMonTitle
            // 
            lblDanhSachMonTitle.AutoSize = true;
            lblDanhSachMonTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblDanhSachMonTitle.Location = new Point(0, 8);
            lblDanhSachMonTitle.Name = "lblDanhSachMonTitle";
            lblDanhSachMonTitle.Size = new Size(163, 25);
            lblDanhSachMonTitle.TabIndex = 0;
            lblDanhSachMonTitle.Text = "Danh sách món ăn";
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
            txtSearch.PlaceholderText = "🔍  Tìm món theo tên, loại, mã...";
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
            cardDangKinhDoanh.ResumeLayout(false);
            cardDangKinhDoanh.PerformLayout();
            cardHetMon.ResumeLayout(false);
            cardHetMon.PerformLayout();
            cardLoaiMon.ResumeLayout(false);
            cardLoaiMon.PerformLayout();
            tableCenter.ResumeLayout(false);
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
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
        private Panel cardDangKinhDoanh;
        private Label lblDangKinhDoanhValue;
        private Label lblDangKinhDoanhTitle;
        private Label lblDangKinhDoanhIcon;
        private Panel cardHetMon;
        private Label lblHetMonValue;
        private Label lblHetMonTitle;
        private Label lblHetMonIcon;
        private Panel cardLoaiMon;
        private Label lblLoaiMonValue;
        private Label lblLoaiMonTitle;
        private Label lblLoaiMonIcon;
        private TableLayoutPanel tableCenter;
        private Panel panelFilter;
        private Button btnLamMoi;
        private Button btnXoaMon;
        private Button btnCapNhatMon;
        private Button btnThemMon;
        private ComboBox cboTrangThai;
        private Label lblTrangThai;
        private ComboBox cboLoaiMon;
        private Label lblLoaiMonFilter;
        private TextBox txtTimMon;
        private Label lblTimMon;
        private Label lblBoLocTitle;
        private Panel panelDanhSachMon;
        private DataGridView dgvDanhSachMon;
        private DataGridViewTextBoxColumn colMaMon;
        private DataGridViewTextBoxColumn colTenMon;
        private DataGridViewTextBoxColumn colLoaiMon;
        private DataGridViewTextBoxColumn colDonGia;
        private DataGridViewTextBoxColumn colTrangThai;
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