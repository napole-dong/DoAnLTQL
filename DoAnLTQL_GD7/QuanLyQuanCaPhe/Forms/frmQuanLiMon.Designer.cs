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
                components?.Dispose();
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            panelSidebar = new Panel();
            btnDangXuat = new Button();
            flowSidebarMenu = new FlowLayoutPanel();
            btnBanHang = new Button();
            btnQuanLyBan = new Button();
            btnQuanLyMon = new Button();
            btnQuanLyKho = new Button();
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
            lblTongMonIcon = new Label();
            lblTongMonValue = new Label();
            lblTongMonTitle = new Label();
            cardDangBan = new Panel();
            lblDangBanIcon = new Label();
            lblDangBanValue = new Label();
            lblDangBanTitle = new Label();
            cardTongLoai = new Panel();
            lblTongLoaiIcon = new Label();
            lblTongLoaiValue = new Label();
            lblTongLoaiTitle = new Label();
            cardNgungBan = new Panel();
            lblNgungBanIcon = new Label();
            lblNgungBanValue = new Label();
            lblNgungBanTitle = new Label();
            tableCenter = new TableLayoutPanel();
            panelThongTin = new Panel();
            btnXoaLoaiMon = new Button();
            btnCapNhatLoaiMon = new Button();
            btnThemLoaiMon = new Button();
            txtMoTaLoai = new TextBox();
            lblMoTaLoai = new Label();
            txtTenLoai = new TextBox();
            lblTenLoai = new Label();
            txtMaLoai = new TextBox();
            lblMaLoai = new Label();
            lblThongTinLoaiTitle = new Label();
            btnXoaMon = new Button();
            btnCapNhatMon = new Button();
            btnThemMon = new Button();
            txtDonGia = new TextBox();
            lblDonGia = new Label();
            cboTrangThai = new ComboBox();
            lblTrangThai = new Label();
            cboLoaiMon = new ComboBox();
            lblLoaiMon = new Label();
            txtTenMon = new TextBox();
            lblTenMon = new Label();
            txtMaMon = new TextBox();
            lblMaMon = new Label();
            lblThongTinMonTitle = new Label();
            panelDanhSach = new Panel();
            tabDanhSach = new TabControl();
            tabMon = new TabPage();
            dgvDanhSachMon = new DataGridView();
            colMaMon = new DataGridViewTextBoxColumn();
            colTenMon = new DataGridViewTextBoxColumn();
            colLoaiMon = new DataGridViewTextBoxColumn();
            colDonGia = new DataGridViewTextBoxColumn();
            colTrangThai = new DataGridViewTextBoxColumn();
            colMoTaMon = new DataGridViewTextBoxColumn();
            tabLoaiMon = new TabPage();
            dgvDanhSachLoaiMon = new DataGridView();
            colMaLoaiMon = new DataGridViewTextBoxColumn();
            colTenLoaiMon = new DataGridViewTextBoxColumn();
            colSoLuongMon = new DataGridViewTextBoxColumn();
            colMoTaLoaiMon = new DataGridViewTextBoxColumn();
            panelDanhSachHeader = new Panel();
            btnXuat = new Button();
            btnNhap = new Button();
            btnLamMoi = new Button();
            txtTimKiem = new TextBox();
            lblTimKiem = new Label();
            lblDanhSachTitle = new Label();
            panelTopbar = new Panel();
            lblPageTitle = new Label();
            panelSidebar.SuspendLayout();
            flowSidebarMenu.SuspendLayout();
            panelLogo.SuspendLayout();
            panelMain.SuspendLayout();
            panelContent.SuspendLayout();
            tableMain.SuspendLayout();
            tableStats.SuspendLayout();
            cardTongMon.SuspendLayout();
            cardDangBan.SuspendLayout();
            cardTongLoai.SuspendLayout();
            cardNgungBan.SuspendLayout();
            tableCenter.SuspendLayout();
            panelThongTin.SuspendLayout();
            panelDanhSach.SuspendLayout();
            tabDanhSach.SuspendLayout();
            tabMon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachMon).BeginInit();
            tabLoaiMon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachLoaiMon).BeginInit();
            panelDanhSachHeader.SuspendLayout();
            panelTopbar.SuspendLayout();
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
            flowSidebarMenu.Controls.Add(btnBanHang);
            flowSidebarMenu.Controls.Add(btnQuanLyBan);
            flowSidebarMenu.Controls.Add(btnQuanLyMon);
            flowSidebarMenu.Controls.Add(btnQuanLyKho);
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
            // btnBanHang
            // 
            btnBanHang.FlatAppearance.BorderSize = 0;
            btnBanHang.FlatStyle = FlatStyle.Flat;
            btnBanHang.Font = new Font("Segoe UI", 10F);
            btnBanHang.ForeColor = Color.Gainsboro;
            btnBanHang.Location = new Point(0, 14);
            btnBanHang.Margin = new Padding(0);
            btnBanHang.Name = "btnBanHang";
            btnBanHang.Padding = new Padding(20, 0, 0, 0);
            btnBanHang.Size = new Size(230, 48);
            btnBanHang.TabIndex = 0;
            btnBanHang.Text = "\U0001f6d2  Bán hàng";
            btnBanHang.TextAlign = ContentAlignment.MiddleLeft;
            btnBanHang.UseVisualStyleBackColor = true;
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
            // btnQuanLyKho
            // 
            btnQuanLyKho.FlatAppearance.BorderSize = 0;
            btnQuanLyKho.FlatStyle = FlatStyle.Flat;
            btnQuanLyKho.Font = new Font("Segoe UI", 10F);
            btnQuanLyKho.ForeColor = Color.Gainsboro;
            btnQuanLyKho.Location = new Point(0, 158);
            btnQuanLyKho.Margin = new Padding(0);
            btnQuanLyKho.Name = "btnQuanLyKho";
            btnQuanLyKho.Padding = new Padding(20, 0, 0, 0);
            btnQuanLyKho.Size = new Size(230, 48);
            btnQuanLyKho.TabIndex = 3;
            btnQuanLyKho.Text = "📦  Quản lý kho";
            btnQuanLyKho.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyKho.UseVisualStyleBackColor = true;
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
            lblLogo.Size = new Size(122, 30);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "Ca phe Pro";
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
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 104F));
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
            tableStats.Controls.Add(cardDangBan, 1, 0);
            tableStats.Controls.Add(cardTongLoai, 2, 0);
            tableStats.Controls.Add(cardNgungBan, 3, 0);
            tableStats.Dock = DockStyle.Fill;
            tableStats.Location = new Point(0, 0);
            tableStats.Margin = new Padding(0, 0, 0, 12);
            tableStats.Name = "tableStats";
            tableStats.RowCount = 1;
            tableStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableStats.Size = new Size(1090, 92);
            tableStats.TabIndex = 0;
            // 
            // cardTongMon
            // 
            cardTongMon.BackColor = Color.FromArgb(237, 247, 243);
            cardTongMon.Controls.Add(lblTongMonIcon);
            cardTongMon.Controls.Add(lblTongMonValue);
            cardTongMon.Controls.Add(lblTongMonTitle);
            cardTongMon.Dock = DockStyle.Fill;
            cardTongMon.Location = new Point(0, 0);
            cardTongMon.Margin = new Padding(0, 0, 8, 0);
            cardTongMon.Name = "cardTongMon";
            cardTongMon.Padding = new Padding(16, 10, 16, 10);
            cardTongMon.Size = new Size(264, 92);
            cardTongMon.TabIndex = 0;
            // 
            // lblTongMonIcon
            // 
            lblTongMonIcon.AutoSize = true;
            lblTongMonIcon.Font = new Font("Segoe UI Emoji", 14F);
            lblTongMonIcon.ForeColor = Color.FromArgb(34, 111, 92);
            lblTongMonIcon.Location = new Point(227, 8);
            lblTongMonIcon.Name = "lblTongMonIcon";
            lblTongMonIcon.Size = new Size(47, 32);
            lblTongMonIcon.TabIndex = 2;
            lblTongMonIcon.Text = "☕";
            // 
            // lblTongMonValue
            // 
            lblTongMonValue.AutoSize = true;
            lblTongMonValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTongMonValue.ForeColor = Color.FromArgb(34, 111, 92);
            lblTongMonValue.Location = new Point(18, 40);
            lblTongMonValue.Name = "lblTongMonValue";
            lblTongMonValue.Size = new Size(24, 32);
            lblTongMonValue.TabIndex = 1;
            lblTongMonValue.Text = "-";
            // 
            // lblTongMonTitle
            // 
            lblTongMonTitle.AutoSize = true;
            lblTongMonTitle.ForeColor = Color.FromArgb(90, 106, 101);
            lblTongMonTitle.Location = new Point(18, 15);
            lblTongMonTitle.Name = "lblTongMonTitle";
            lblTongMonTitle.Size = new Size(96, 20);
            lblTongMonTitle.TabIndex = 0;
            lblTongMonTitle.Text = "Tổng số món";
            // 
            // cardDangBan
            // 
            cardDangBan.BackColor = Color.FromArgb(236, 242, 252);
            cardDangBan.Controls.Add(lblDangBanIcon);
            cardDangBan.Controls.Add(lblDangBanValue);
            cardDangBan.Controls.Add(lblDangBanTitle);
            cardDangBan.Dock = DockStyle.Fill;
            cardDangBan.Location = new Point(272, 0);
            cardDangBan.Margin = new Padding(0, 0, 8, 0);
            cardDangBan.Name = "cardDangBan";
            cardDangBan.Padding = new Padding(16, 10, 16, 10);
            cardDangBan.Size = new Size(264, 92);
            cardDangBan.TabIndex = 1;
            // 
            // lblDangBanIcon
            // 
            lblDangBanIcon.AutoSize = true;
            lblDangBanIcon.Font = new Font("Segoe UI Emoji", 14F);
            lblDangBanIcon.ForeColor = Color.FromArgb(35, 84, 148);
            lblDangBanIcon.Location = new Point(227, 8);
            lblDangBanIcon.Name = "lblDangBanIcon";
            lblDangBanIcon.Size = new Size(47, 32);
            lblDangBanIcon.TabIndex = 2;
            lblDangBanIcon.Text = "✅";
            // 
            // lblDangBanValue
            // 
            lblDangBanValue.AutoSize = true;
            lblDangBanValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblDangBanValue.ForeColor = Color.FromArgb(35, 84, 148);
            lblDangBanValue.Location = new Point(18, 40);
            lblDangBanValue.Name = "lblDangBanValue";
            lblDangBanValue.Size = new Size(24, 32);
            lblDangBanValue.TabIndex = 1;
            lblDangBanValue.Text = "-";
            // 
            // lblDangBanTitle
            // 
            lblDangBanTitle.AutoSize = true;
            lblDangBanTitle.ForeColor = Color.FromArgb(89, 100, 116);
            lblDangBanTitle.Location = new Point(18, 15);
            lblDangBanTitle.Name = "lblDangBanTitle";
            lblDangBanTitle.Size = new Size(122, 20);
            lblDangBanTitle.TabIndex = 0;
            lblDangBanTitle.Text = "Đang kinh doanh";
            // 
            // cardTongLoai
            // 
            cardTongLoai.BackColor = Color.FromArgb(252, 244, 232);
            cardTongLoai.Controls.Add(lblTongLoaiIcon);
            cardTongLoai.Controls.Add(lblTongLoaiValue);
            cardTongLoai.Controls.Add(lblTongLoaiTitle);
            cardTongLoai.Dock = DockStyle.Fill;
            cardTongLoai.Location = new Point(544, 0);
            cardTongLoai.Margin = new Padding(0, 0, 8, 0);
            cardTongLoai.Name = "cardTongLoai";
            cardTongLoai.Padding = new Padding(16, 10, 16, 10);
            cardTongLoai.Size = new Size(264, 92);
            cardTongLoai.TabIndex = 2;
            // 
            // lblTongLoaiIcon
            // 
            lblTongLoaiIcon.AutoSize = true;
            lblTongLoaiIcon.Font = new Font("Segoe UI Emoji", 14F);
            lblTongLoaiIcon.ForeColor = Color.FromArgb(139, 90, 20);
            lblTongLoaiIcon.Location = new Point(227, 8);
            lblTongLoaiIcon.Name = "lblTongLoaiIcon";
            lblTongLoaiIcon.Size = new Size(47, 32);
            lblTongLoaiIcon.TabIndex = 2;
            lblTongLoaiIcon.Text = "🏷";
            // 
            // lblTongLoaiValue
            // 
            lblTongLoaiValue.AutoSize = true;
            lblTongLoaiValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTongLoaiValue.ForeColor = Color.FromArgb(139, 90, 20);
            lblTongLoaiValue.Location = new Point(18, 40);
            lblTongLoaiValue.Name = "lblTongLoaiValue";
            lblTongLoaiValue.Size = new Size(24, 32);
            lblTongLoaiValue.TabIndex = 1;
            lblTongLoaiValue.Text = "-";
            // 
            // lblTongLoaiTitle
            // 
            lblTongLoaiTitle.AutoSize = true;
            lblTongLoaiTitle.ForeColor = Color.FromArgb(111, 95, 70);
            lblTongLoaiTitle.Location = new Point(18, 15);
            lblTongLoaiTitle.Name = "lblTongLoaiTitle";
            lblTongLoaiTitle.Size = new Size(106, 20);
            lblTongLoaiTitle.TabIndex = 0;
            lblTongLoaiTitle.Text = "Tổng loại món";
            // 
            // cardNgungBan
            // 
            cardNgungBan.BackColor = Color.FromArgb(250, 237, 237);
            cardNgungBan.Controls.Add(lblNgungBanIcon);
            cardNgungBan.Controls.Add(lblNgungBanValue);
            cardNgungBan.Controls.Add(lblNgungBanTitle);
            cardNgungBan.Dock = DockStyle.Fill;
            cardNgungBan.Location = new Point(816, 0);
            cardNgungBan.Margin = new Padding(0);
            cardNgungBan.Name = "cardNgungBan";
            cardNgungBan.Padding = new Padding(16, 10, 16, 10);
            cardNgungBan.Size = new Size(274, 92);
            cardNgungBan.TabIndex = 3;
            // 
            // lblNgungBanIcon
            // 
            lblNgungBanIcon.AutoSize = true;
            lblNgungBanIcon.Font = new Font("Segoe UI Emoji", 14F);
            lblNgungBanIcon.ForeColor = Color.FromArgb(158, 52, 52);
            lblNgungBanIcon.Location = new Point(237, 8);
            lblNgungBanIcon.Name = "lblNgungBanIcon";
            lblNgungBanIcon.Size = new Size(47, 32);
            lblNgungBanIcon.TabIndex = 2;
            lblNgungBanIcon.Text = "⛔";
            // 
            // lblNgungBanValue
            // 
            lblNgungBanValue.AutoSize = true;
            lblNgungBanValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblNgungBanValue.ForeColor = Color.FromArgb(158, 52, 52);
            lblNgungBanValue.Location = new Point(18, 40);
            lblNgungBanValue.Name = "lblNgungBanValue";
            lblNgungBanValue.Size = new Size(24, 32);
            lblNgungBanValue.TabIndex = 1;
            lblNgungBanValue.Text = "-";
            // 
            // lblNgungBanTitle
            // 
            lblNgungBanTitle.AutoSize = true;
            lblNgungBanTitle.ForeColor = Color.FromArgb(122, 88, 88);
            lblNgungBanTitle.Location = new Point(18, 15);
            lblNgungBanTitle.Name = "lblNgungBanTitle";
            lblNgungBanTitle.Size = new Size(84, 20);
            lblNgungBanTitle.TabIndex = 0;
            lblNgungBanTitle.Text = "Ngừng bán";
            // 
            // tableCenter
            // 
            tableCenter.ColumnCount = 2;
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360F));
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableCenter.Controls.Add(panelThongTin, 0, 0);
            tableCenter.Controls.Add(panelDanhSach, 1, 0);
            tableCenter.Dock = DockStyle.Fill;
            tableCenter.Location = new Point(0, 104);
            tableCenter.Margin = new Padding(0);
            tableCenter.Name = "tableCenter";
            tableCenter.RowCount = 1;
            tableCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableCenter.Size = new Size(1090, 538);
            tableCenter.TabIndex = 1;
            // 
            // panelThongTin
            // 
            panelThongTin.BackColor = Color.White;
            panelThongTin.Controls.Add(btnXoaLoaiMon);
            panelThongTin.Controls.Add(btnCapNhatLoaiMon);
            panelThongTin.Controls.Add(btnThemLoaiMon);
            panelThongTin.Controls.Add(txtMoTaLoai);
            panelThongTin.Controls.Add(lblMoTaLoai);
            panelThongTin.Controls.Add(txtTenLoai);
            panelThongTin.Controls.Add(lblTenLoai);
            panelThongTin.Controls.Add(txtMaLoai);
            panelThongTin.Controls.Add(lblMaLoai);
            panelThongTin.Controls.Add(lblThongTinLoaiTitle);
            panelThongTin.Controls.Add(btnXoaMon);
            panelThongTin.Controls.Add(btnCapNhatMon);
            panelThongTin.Controls.Add(btnThemMon);
            panelThongTin.Controls.Add(txtDonGia);
            panelThongTin.Controls.Add(lblDonGia);
            panelThongTin.Controls.Add(cboTrangThai);
            panelThongTin.Controls.Add(lblTrangThai);
            panelThongTin.Controls.Add(cboLoaiMon);
            panelThongTin.Controls.Add(lblLoaiMon);
            panelThongTin.Controls.Add(txtTenMon);
            panelThongTin.Controls.Add(lblTenMon);
            panelThongTin.Controls.Add(txtMaMon);
            panelThongTin.Controls.Add(lblMaMon);
            panelThongTin.Controls.Add(lblThongTinMonTitle);
            panelThongTin.Dock = DockStyle.Fill;
            panelThongTin.Location = new Point(0, 0);
            panelThongTin.Margin = new Padding(0, 0, 14, 0);
            panelThongTin.Name = "panelThongTin";
            panelThongTin.Size = new Size(346, 538);
            panelThongTin.TabIndex = 0;
            // 
            // btnXoaLoaiMon
            // 
            btnXoaLoaiMon.BackColor = Color.FromArgb(220, 53, 69);
            btnXoaLoaiMon.FlatAppearance.BorderSize = 0;
            btnXoaLoaiMon.FlatStyle = FlatStyle.Flat;
            btnXoaLoaiMon.ForeColor = Color.White;
            btnXoaLoaiMon.Location = new Point(223, 458);
            btnXoaLoaiMon.Name = "btnXoaLoaiMon";
            btnXoaLoaiMon.Size = new Size(102, 32);
            btnXoaLoaiMon.TabIndex = 23;
            btnXoaLoaiMon.Text = "Xóa loại";
            btnXoaLoaiMon.UseVisualStyleBackColor = false;
            // 
            // btnCapNhatLoaiMon
            // 
            btnCapNhatLoaiMon.BackColor = Color.FromArgb(25, 135, 84);
            btnCapNhatLoaiMon.FlatAppearance.BorderSize = 0;
            btnCapNhatLoaiMon.FlatStyle = FlatStyle.Flat;
            btnCapNhatLoaiMon.ForeColor = Color.White;
            btnCapNhatLoaiMon.Location = new Point(122, 458);
            btnCapNhatLoaiMon.Name = "btnCapNhatLoaiMon";
            btnCapNhatLoaiMon.Size = new Size(95, 32);
            btnCapNhatLoaiMon.TabIndex = 22;
            btnCapNhatLoaiMon.Text = "Cập nhật";
            btnCapNhatLoaiMon.UseVisualStyleBackColor = false;
            // 
            // btnThemLoaiMon
            // 
            btnThemLoaiMon.BackColor = Color.FromArgb(13, 110, 253);
            btnThemLoaiMon.FlatAppearance.BorderSize = 0;
            btnThemLoaiMon.FlatStyle = FlatStyle.Flat;
            btnThemLoaiMon.ForeColor = Color.White;
            btnThemLoaiMon.Location = new Point(21, 458);
            btnThemLoaiMon.Name = "btnThemLoaiMon";
            btnThemLoaiMon.Size = new Size(95, 32);
            btnThemLoaiMon.TabIndex = 21;
            btnThemLoaiMon.Text = "Thêm loại";
            btnThemLoaiMon.UseVisualStyleBackColor = false;
            // 
            // txtMoTaLoai
            // 
            txtMoTaLoai.BorderStyle = BorderStyle.FixedSingle;
            txtMoTaLoai.Location = new Point(21, 419);
            txtMoTaLoai.Name = "txtMoTaLoai";
            txtMoTaLoai.Size = new Size(304, 27);
            txtMoTaLoai.TabIndex = 20;
            // 
            // lblMoTaLoai
            // 
            lblMoTaLoai.AutoSize = true;
            lblMoTaLoai.ForeColor = Color.FromArgb(88, 72, 62);
            lblMoTaLoai.Location = new Point(21, 396);
            lblMoTaLoai.Name = "lblMoTaLoai";
            lblMoTaLoai.Size = new Size(77, 20);
            lblMoTaLoai.TabIndex = 19;
            lblMoTaLoai.Text = "Mô tả loại";
            // 
            // txtTenLoai
            // 
            txtTenLoai.BorderStyle = BorderStyle.FixedSingle;
            txtTenLoai.Location = new Point(21, 365);
            txtTenLoai.Name = "txtTenLoai";
            txtTenLoai.Size = new Size(304, 27);
            txtTenLoai.TabIndex = 18;
            // 
            // lblTenLoai
            // 
            lblTenLoai.AutoSize = true;
            lblTenLoai.ForeColor = Color.FromArgb(88, 72, 62);
            lblTenLoai.Location = new Point(21, 342);
            lblTenLoai.Name = "lblTenLoai";
            lblTenLoai.Size = new Size(61, 20);
            lblTenLoai.TabIndex = 17;
            lblTenLoai.Text = "Tên loại";
            // 
            // txtMaLoai
            // 
            txtMaLoai.BorderStyle = BorderStyle.FixedSingle;
            txtMaLoai.Location = new Point(21, 311);
            txtMaLoai.Name = "txtMaLoai";
            txtMaLoai.ReadOnly = true;
            txtMaLoai.Size = new Size(124, 27);
            txtMaLoai.TabIndex = 16;
            txtMaLoai.TextAlign = HorizontalAlignment.Center;
            // 
            // lblMaLoai
            // 
            lblMaLoai.AutoSize = true;
            lblMaLoai.ForeColor = Color.FromArgb(88, 72, 62);
            lblMaLoai.Location = new Point(21, 288);
            lblMaLoai.Name = "lblMaLoai";
            lblMaLoai.Size = new Size(59, 20);
            lblMaLoai.TabIndex = 15;
            lblMaLoai.Text = "Mã loại";
            // 
            // lblThongTinLoaiTitle
            // 
            lblThongTinLoaiTitle.AutoSize = true;
            lblThongTinLoaiTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblThongTinLoaiTitle.ForeColor = Color.FromArgb(62, 45, 36);
            lblThongTinLoaiTitle.Location = new Point(21, 259);
            lblThongTinLoaiTitle.Name = "lblThongTinLoaiTitle";
            lblThongTinLoaiTitle.Size = new Size(116, 23);
            lblThongTinLoaiTitle.TabIndex = 14;
            lblThongTinLoaiTitle.Text = "Thông tin loại";
            // 
            // btnXoaMon
            // 
            btnXoaMon.BackColor = Color.FromArgb(220, 53, 69);
            btnXoaMon.FlatAppearance.BorderSize = 0;
            btnXoaMon.FlatStyle = FlatStyle.Flat;
            btnXoaMon.ForeColor = Color.White;
            btnXoaMon.Location = new Point(223, 217);
            btnXoaMon.Name = "btnXoaMon";
            btnXoaMon.Size = new Size(102, 32);
            btnXoaMon.TabIndex = 13;
            btnXoaMon.Text = "Xóa món";
            btnXoaMon.UseVisualStyleBackColor = false;
            btnXoaMon.Click += btnXoaMon_Click;
            // 
            // btnCapNhatMon
            // 
            btnCapNhatMon.BackColor = Color.FromArgb(25, 135, 84);
            btnCapNhatMon.FlatAppearance.BorderSize = 0;
            btnCapNhatMon.FlatStyle = FlatStyle.Flat;
            btnCapNhatMon.ForeColor = Color.White;
            btnCapNhatMon.Location = new Point(122, 217);
            btnCapNhatMon.Name = "btnCapNhatMon";
            btnCapNhatMon.Size = new Size(95, 32);
            btnCapNhatMon.TabIndex = 12;
            btnCapNhatMon.Text = "Cập nhật";
            btnCapNhatMon.UseVisualStyleBackColor = false;
            btnCapNhatMon.Click += btnCapNhatMon_Click;
            // 
            // btnThemMon
            // 
            btnThemMon.BackColor = Color.FromArgb(13, 110, 253);
            btnThemMon.FlatAppearance.BorderSize = 0;
            btnThemMon.FlatStyle = FlatStyle.Flat;
            btnThemMon.ForeColor = Color.White;
            btnThemMon.Location = new Point(21, 217);
            btnThemMon.Name = "btnThemMon";
            btnThemMon.Size = new Size(95, 32);
            btnThemMon.TabIndex = 11;
            btnThemMon.Text = "Thêm món";
            btnThemMon.UseVisualStyleBackColor = false;
            btnThemMon.Click += btnThemMon_Click;
            // 
            // txtDonGia
            // 
            txtDonGia.BorderStyle = BorderStyle.FixedSingle;
            txtDonGia.Location = new Point(21, 184);
            txtDonGia.Name = "txtDonGia";
            txtDonGia.Size = new Size(304, 27);
            txtDonGia.TabIndex = 10;
            // 
            // lblDonGia
            // 
            lblDonGia.AutoSize = true;
            lblDonGia.ForeColor = Color.FromArgb(88, 72, 62);
            lblDonGia.Location = new Point(21, 161);
            lblDonGia.Name = "lblDonGia";
            lblDonGia.Size = new Size(62, 20);
            lblDonGia.TabIndex = 9;
            lblDonGia.Text = "Đơn giá";
            // 
            // cboTrangThai
            // 
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.FormattingEnabled = true;
            cboTrangThai.Items.AddRange(new object[] { "Đang kinh doanh", "Ngừng bán" });
            cboTrangThai.Location = new Point(177, 131);
            cboTrangThai.Name = "cboTrangThai";
            cboTrangThai.Size = new Size(148, 28);
            cboTrangThai.TabIndex = 8;
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.ForeColor = Color.FromArgb(88, 72, 62);
            lblTrangThai.Location = new Point(177, 108);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(75, 20);
            lblTrangThai.TabIndex = 7;
            lblTrangThai.Text = "Trạng thái";
            // 
            // cboLoaiMon
            // 
            cboLoaiMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiMon.FormattingEnabled = true;
            cboLoaiMon.Location = new Point(21, 131);
            cboLoaiMon.Name = "cboLoaiMon";
            cboLoaiMon.Size = new Size(149, 28);
            cboLoaiMon.TabIndex = 6;
            // 
            // lblLoaiMon
            // 
            lblLoaiMon.AutoSize = true;
            lblLoaiMon.ForeColor = Color.FromArgb(88, 72, 62);
            lblLoaiMon.Location = new Point(21, 108);
            lblLoaiMon.Name = "lblLoaiMon";
            lblLoaiMon.Size = new Size(71, 20);
            lblLoaiMon.TabIndex = 5;
            lblLoaiMon.Text = "Loại món";
            // 
            // txtTenMon
            // 
            txtTenMon.BorderStyle = BorderStyle.FixedSingle;
            txtTenMon.Location = new Point(151, 77);
            txtTenMon.Name = "txtTenMon";
            txtTenMon.Size = new Size(174, 27);
            txtTenMon.TabIndex = 4;
            // 
            // lblTenMon
            // 
            lblTenMon.AutoSize = true;
            lblTenMon.ForeColor = Color.FromArgb(88, 72, 62);
            lblTenMon.Location = new Point(151, 54);
            lblTenMon.Name = "lblTenMon";
            lblTenMon.Size = new Size(66, 20);
            lblTenMon.TabIndex = 3;
            lblTenMon.Text = "Tên món";
            // 
            // txtMaMon
            // 
            txtMaMon.BorderStyle = BorderStyle.FixedSingle;
            txtMaMon.Location = new Point(21, 77);
            txtMaMon.Name = "txtMaMon";
            txtMaMon.ReadOnly = true;
            txtMaMon.Size = new Size(124, 27);
            txtMaMon.TabIndex = 2;
            txtMaMon.TextAlign = HorizontalAlignment.Center;
            // 
            // lblMaMon
            // 
            lblMaMon.AutoSize = true;
            lblMaMon.ForeColor = Color.FromArgb(88, 72, 62);
            lblMaMon.Location = new Point(21, 54);
            lblMaMon.Name = "lblMaMon";
            lblMaMon.Size = new Size(64, 20);
            lblMaMon.TabIndex = 1;
            lblMaMon.Text = "Mã món";
            // 
            // lblThongTinMonTitle
            // 
            lblThongTinMonTitle.AutoSize = true;
            lblThongTinMonTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblThongTinMonTitle.ForeColor = Color.FromArgb(62, 45, 36);
            lblThongTinMonTitle.Location = new Point(21, 23);
            lblThongTinMonTitle.Name = "lblThongTinMonTitle";
            lblThongTinMonTitle.Size = new Size(124, 23);
            lblThongTinMonTitle.TabIndex = 0;
            lblThongTinMonTitle.Text = "Thông tin món";
            // 
            // panelDanhSach
            // 
            panelDanhSach.BackColor = Color.White;
            panelDanhSach.Controls.Add(tabDanhSach);
            panelDanhSach.Controls.Add(panelDanhSachHeader);
            panelDanhSach.Dock = DockStyle.Fill;
            panelDanhSach.Location = new Point(360, 0);
            panelDanhSach.Margin = new Padding(0);
            panelDanhSach.Name = "panelDanhSach";
            panelDanhSach.Size = new Size(730, 538);
            panelDanhSach.TabIndex = 1;
            // 
            // tabDanhSach
            // 
            tabDanhSach.Controls.Add(tabMon);
            tabDanhSach.Controls.Add(tabLoaiMon);
            tabDanhSach.Dock = DockStyle.Fill;
            tabDanhSach.Location = new Point(0, 78);
            tabDanhSach.Name = "tabDanhSach";
            tabDanhSach.SelectedIndex = 0;
            tabDanhSach.Size = new Size(730, 460);
            tabDanhSach.TabIndex = 1;
            // 
            // tabMon
            // 
            tabMon.Controls.Add(dgvDanhSachMon);
            tabMon.Location = new Point(4, 29);
            tabMon.Name = "tabMon";
            tabMon.Padding = new Padding(8);
            tabMon.Size = new Size(722, 427);
            tabMon.TabIndex = 0;
            tabMon.Text = "Danh sách món";
            tabMon.UseVisualStyleBackColor = true;
            // 
            // dgvDanhSachMon
            // 
            dgvDanhSachMon.AllowUserToAddRows = false;
            dgvDanhSachMon.AllowUserToDeleteRows = false;
            dgvDanhSachMon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhSachMon.BackgroundColor = Color.White;
            dgvDanhSachMon.BorderStyle = BorderStyle.None;
            dgvDanhSachMon.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDanhSachMon.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(246, 239, 232);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(86, 62, 46);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(246, 239, 232);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(86, 62, 46);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDanhSachMon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDanhSachMon.ColumnHeadersHeight = 42;
            dgvDanhSachMon.Columns.AddRange(new DataGridViewColumn[] { colMaMon, colTenMon, colLoaiMon, colDonGia, colTrangThai, colMoTaMon });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(58, 58, 58);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(244, 233, 220);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(58, 58, 58);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDanhSachMon.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDanhSachMon.Dock = DockStyle.Fill;
            dgvDanhSachMon.EnableHeadersVisualStyles = false;
            dgvDanhSachMon.GridColor = Color.FromArgb(238, 230, 220);
            dgvDanhSachMon.Location = new Point(8, 8);
            dgvDanhSachMon.MultiSelect = false;
            dgvDanhSachMon.Name = "dgvDanhSachMon";
            dgvDanhSachMon.ReadOnly = true;
            dgvDanhSachMon.RowHeadersVisible = false;
            dgvDanhSachMon.RowHeadersWidth = 51;
            dgvDanhSachMon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhSachMon.Size = new Size(706, 411);
            dgvDanhSachMon.TabIndex = 0;
            // 
            // colMaMon
            // 
            colMaMon.FillWeight = 60F;
            colMaMon.HeaderText = "Mã";
            colMaMon.MinimumWidth = 6;
            colMaMon.Name = "colMaMon";
            colMaMon.ReadOnly = true;
            // 
            // colTenMon
            // 
            colTenMon.HeaderText = "Tên món";
            colTenMon.MinimumWidth = 6;
            colTenMon.Name = "colTenMon";
            colTenMon.ReadOnly = true;
            // 
            // colLoaiMon
            // 
            colLoaiMon.HeaderText = "Loại";
            colLoaiMon.MinimumWidth = 6;
            colLoaiMon.Name = "colLoaiMon";
            colLoaiMon.ReadOnly = true;
            // 
            // colDonGia
            // 
            colDonGia.HeaderText = "Đơn giá";
            colDonGia.MinimumWidth = 6;
            colDonGia.Name = "colDonGia";
            colDonGia.ReadOnly = true;
            // 
            // colTrangThai
            // 
            colTrangThai.HeaderText = "Trạng thái";
            colTrangThai.MinimumWidth = 6;
            colTrangThai.Name = "colTrangThai";
            colTrangThai.ReadOnly = true;
            // 
            // colMoTaMon
            // 
            colMoTaMon.HeaderText = "Mô tả";
            colMoTaMon.MinimumWidth = 6;
            colMoTaMon.Name = "colMoTaMon";
            colMoTaMon.ReadOnly = true;
            // 
            // tabLoaiMon
            // 
            tabLoaiMon.Controls.Add(dgvDanhSachLoaiMon);
            tabLoaiMon.Location = new Point(4, 29);
            tabLoaiMon.Name = "tabLoaiMon";
            tabLoaiMon.Padding = new Padding(8);
            tabLoaiMon.Size = new Size(722, 427);
            tabLoaiMon.TabIndex = 1;
            tabLoaiMon.Text = "Danh sách loại món";
            tabLoaiMon.UseVisualStyleBackColor = true;
            // 
            // dgvDanhSachLoaiMon
            // 
            dgvDanhSachLoaiMon.AllowUserToAddRows = false;
            dgvDanhSachLoaiMon.AllowUserToDeleteRows = false;
            dgvDanhSachLoaiMon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhSachLoaiMon.BackgroundColor = Color.White;
            dgvDanhSachLoaiMon.BorderStyle = BorderStyle.None;
            dgvDanhSachLoaiMon.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDanhSachLoaiMon.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(246, 239, 232);
            dataGridViewCellStyle3.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(86, 62, 46);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(246, 239, 232);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(86, 62, 46);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvDanhSachLoaiMon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvDanhSachLoaiMon.ColumnHeadersHeight = 42;
            dgvDanhSachLoaiMon.Columns.AddRange(new DataGridViewColumn[] { colMaLoaiMon, colTenLoaiMon, colSoLuongMon, colMoTaLoaiMon });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(58, 58, 58);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(244, 233, 220);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(58, 58, 58);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvDanhSachLoaiMon.DefaultCellStyle = dataGridViewCellStyle4;
            dgvDanhSachLoaiMon.Dock = DockStyle.Fill;
            dgvDanhSachLoaiMon.EnableHeadersVisualStyles = false;
            dgvDanhSachLoaiMon.GridColor = Color.FromArgb(238, 230, 220);
            dgvDanhSachLoaiMon.Location = new Point(8, 8);
            dgvDanhSachLoaiMon.MultiSelect = false;
            dgvDanhSachLoaiMon.Name = "dgvDanhSachLoaiMon";
            dgvDanhSachLoaiMon.ReadOnly = true;
            dgvDanhSachLoaiMon.RowHeadersVisible = false;
            dgvDanhSachLoaiMon.RowHeadersWidth = 51;
            dgvDanhSachLoaiMon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhSachLoaiMon.Size = new Size(706, 411);
            dgvDanhSachLoaiMon.TabIndex = 0;
            // 
            // colMaLoaiMon
            // 
            colMaLoaiMon.FillWeight = 70F;
            colMaLoaiMon.HeaderText = "Mã loại";
            colMaLoaiMon.MinimumWidth = 6;
            colMaLoaiMon.Name = "colMaLoaiMon";
            colMaLoaiMon.ReadOnly = true;
            // 
            // colTenLoaiMon
            // 
            colTenLoaiMon.HeaderText = "Tên loại";
            colTenLoaiMon.MinimumWidth = 6;
            colTenLoaiMon.Name = "colTenLoaiMon";
            colTenLoaiMon.ReadOnly = true;
            // 
            // colSoLuongMon
            // 
            colSoLuongMon.HeaderText = "Số món";
            colSoLuongMon.MinimumWidth = 6;
            colSoLuongMon.Name = "colSoLuongMon";
            colSoLuongMon.ReadOnly = true;
            // 
            // colMoTaLoaiMon
            // 
            colMoTaLoaiMon.HeaderText = "Mô tả";
            colMoTaLoaiMon.MinimumWidth = 6;
            colMoTaLoaiMon.Name = "colMoTaLoaiMon";
            colMoTaLoaiMon.ReadOnly = true;
            // 
            // panelDanhSachHeader
            // 
            panelDanhSachHeader.Controls.Add(btnXuat);
            panelDanhSachHeader.Controls.Add(btnNhap);
            panelDanhSachHeader.Controls.Add(btnLamMoi);
            panelDanhSachHeader.Controls.Add(txtTimKiem);
            panelDanhSachHeader.Controls.Add(lblTimKiem);
            panelDanhSachHeader.Controls.Add(lblDanhSachTitle);
            panelDanhSachHeader.Dock = DockStyle.Top;
            panelDanhSachHeader.Location = new Point(0, 0);
            panelDanhSachHeader.Name = "panelDanhSachHeader";
            panelDanhSachHeader.Size = new Size(730, 78);
            panelDanhSachHeader.TabIndex = 0;
            // 
            // btnXuat
            // 
            btnXuat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXuat.BackColor = Color.FromArgb(255, 193, 7);
            btnXuat.FlatAppearance.BorderSize = 0;
            btnXuat.FlatStyle = FlatStyle.Flat;
            btnXuat.Location = new Point(637, 43);
            btnXuat.Name = "btnXuat";
            btnXuat.Size = new Size(82, 28);
            btnXuat.TabIndex = 4;
            btnXuat.Text = "Xuất";
            btnXuat.UseVisualStyleBackColor = false;
            btnXuat.Click += btnXuat_Click;
            // 
            // btnNhap
            // 
            btnNhap.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNhap.BackColor = Color.FromArgb(13, 110, 253);
            btnNhap.FlatAppearance.BorderSize = 0;
            btnNhap.FlatStyle = FlatStyle.Flat;
            btnNhap.ForeColor = Color.White;
            btnNhap.Location = new Point(549, 43);
            btnNhap.Name = "btnNhap";
            btnNhap.Size = new Size(82, 28);
            btnNhap.TabIndex = 3;
            btnNhap.Text = "Nhập";
            btnNhap.UseVisualStyleBackColor = false;
            btnNhap.Click += btnNhap_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLamMoi.BackColor = Color.FromArgb(248, 245, 241);
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.ForeColor = Color.FromArgb(65, 48, 39);
            btnLamMoi.Location = new Point(461, 43);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(82, 28);
            btnLamMoi.TabIndex = 5;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
            txtTimKiem.Location = new Point(83, 44);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(459, 27);
            txtTimKiem.TabIndex = 2;
            // 
            // lblTimKiem
            // 
            lblTimKiem.AutoSize = true;
            lblTimKiem.ForeColor = Color.FromArgb(88, 72, 62);
            lblTimKiem.Location = new Point(14, 46);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(70, 20);
            lblTimKiem.TabIndex = 1;
            lblTimKiem.Text = "Tìm kiếm";
            // 
            // lblDanhSachTitle
            // 
            lblDanhSachTitle.AutoSize = true;
            lblDanhSachTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblDanhSachTitle.ForeColor = Color.FromArgb(62, 45, 36);
            lblDanhSachTitle.Location = new Point(14, 12);
            lblDanhSachTitle.Name = "lblDanhSachTitle";
            lblDanhSachTitle.Size = new Size(194, 25);
            lblDanhSachTitle.TabIndex = 0;
            lblDanhSachTitle.Text = "Danh sách món / loại";
            // 
            // panelTopbar
            // 
            panelTopbar.BackColor = Color.White;
            panelTopbar.Controls.Add(lblPageTitle);
            panelTopbar.Dock = DockStyle.Top;
            panelTopbar.Location = new Point(0, 0);
            panelTopbar.Name = "panelTopbar";
            panelTopbar.Size = new Size(1134, 80);
            panelTopbar.TabIndex = 0;
            // 
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(77, 55, 42);
            lblPageTitle.Location = new Point(22, 21);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(295, 37);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Quản lý món & loại món";
            // 
            // frmQuanLiMon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1364, 760);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            MinimumSize = new Size(1220, 720);
            Name = "frmQuanLiMon";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý món";
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
            cardDangBan.ResumeLayout(false);
            cardDangBan.PerformLayout();
            cardTongLoai.ResumeLayout(false);
            cardTongLoai.PerformLayout();
            cardNgungBan.ResumeLayout(false);
            cardNgungBan.PerformLayout();
            tableCenter.ResumeLayout(false);
            panelThongTin.ResumeLayout(false);
            panelThongTin.PerformLayout();
            panelDanhSach.ResumeLayout(false);
            tabDanhSach.ResumeLayout(false);
            tabMon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachMon).EndInit();
            tabLoaiMon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachLoaiMon).EndInit();
            panelDanhSachHeader.ResumeLayout(false);
            panelDanhSachHeader.PerformLayout();
            panelTopbar.ResumeLayout(false);
            panelTopbar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
        private Button btnDangXuat;
        private FlowLayoutPanel flowSidebarMenu;
        private Button btnBanHang;
        private Button btnQuanLyBan;
        private Button btnQuanLyMon;
        private Button btnQuanLyKho;
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
        private Panel cardDangBan;
        private Label lblDangBanValue;
        private Label lblDangBanTitle;
        private Label lblDangBanIcon;
        private Panel cardTongLoai;
        private Label lblTongLoaiValue;
        private Label lblTongLoaiTitle;
        private Label lblTongLoaiIcon;
        private Panel cardNgungBan;
        private Label lblNgungBanValue;
        private Label lblNgungBanTitle;
        private Label lblNgungBanIcon;
        private TableLayoutPanel tableCenter;
        private Panel panelThongTin;
        private Button btnXoaLoaiMon;
        private Button btnCapNhatLoaiMon;
        private Button btnThemLoaiMon;
        private TextBox txtMoTaLoai;
        private Label lblMoTaLoai;
        private TextBox txtTenLoai;
        private Label lblTenLoai;
        private TextBox txtMaLoai;
        private Label lblMaLoai;
        private Label lblThongTinLoaiTitle;
        private Button btnXoaMon;
        private Button btnCapNhatMon;
        private Button btnThemMon;
        private TextBox txtDonGia;
        private Label lblDonGia;
        private ComboBox cboTrangThai;
        private Label lblTrangThai;
        private ComboBox cboLoaiMon;
        private Label lblLoaiMon;
        private TextBox txtTenMon;
        private Label lblTenMon;
        private TextBox txtMaMon;
        private Label lblMaMon;
        private Label lblThongTinMonTitle;
        private Panel panelDanhSach;
        private TabControl tabDanhSach;
        private TabPage tabMon;
        private DataGridView dgvDanhSachMon;
        private DataGridViewTextBoxColumn colMaMon;
        private DataGridViewTextBoxColumn colTenMon;
        private DataGridViewTextBoxColumn colLoaiMon;
        private DataGridViewTextBoxColumn colDonGia;
        private DataGridViewTextBoxColumn colTrangThai;
        private DataGridViewTextBoxColumn colMoTaMon;
        private TabPage tabLoaiMon;
        private DataGridView dgvDanhSachLoaiMon;
        private DataGridViewTextBoxColumn colMaLoaiMon;
        private DataGridViewTextBoxColumn colTenLoaiMon;
        private DataGridViewTextBoxColumn colSoLuongMon;
        private DataGridViewTextBoxColumn colMoTaLoaiMon;
        private Panel panelDanhSachHeader;
        private Button btnLamMoi;
        private Button btnXuat;
        private Button btnNhap;
        private TextBox txtTimKiem;
        private Label lblTimKiem;
        private Label lblDanhSachTitle;
        private Panel panelTopbar;
        private Label lblPageTitle;
    }
}