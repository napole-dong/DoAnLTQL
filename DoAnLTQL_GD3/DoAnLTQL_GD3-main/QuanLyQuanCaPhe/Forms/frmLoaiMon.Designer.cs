namespace QuanLyQuanCaPhe.Forms
{
    partial class frmLoaiMon
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
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
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
            cardTongLoai = new Panel();
            lblTongLoaiValue = new Label();
            lblTongLoaiTitle = new Label();
            lblTongLoaiIcon = new Label();
            cardDangSuDung = new Panel();
            lblDangSuDungValue = new Label();
            lblDangSuDungTitle = new Label();
            lblDangSuDungIcon = new Label();
            cardChuaSuDung = new Panel();
            lblChuaSuDungValue = new Label();
            lblChuaSuDungTitle = new Label();
            lblChuaSuDungIcon = new Label();
            tableCenter = new TableLayoutPanel();
            panelThongTinLoai = new Panel();
            btnLamMoi = new Button();
            btnXoaLoai = new Button();
            btnCapNhatLoai = new Button();
            btnThemLoai = new Button();
            txtTimLoai = new TextBox();
            lblTimLoai = new Label();
            txtTenLoai = new TextBox();
            lblTenLoai = new Label();
            txtMaLoai = new TextBox();
            lblMaLoai = new Label();
            lblThongTinLoaiTitle = new Label();
            panelDanhSachLoai = new Panel();
            dgvDanhSachLoai = new DataGridView();
            colIDLoai = new DataGridViewTextBoxColumn();
            colTenLoai = new DataGridViewTextBoxColumn();
            colSoMon = new DataGridViewTextBoxColumn();
            panelDanhSachHeader = new Panel();
            btnNhapLoai = new Button();
            btnXuatLoai = new Button();
            lblDanhSachLoaiTitle = new Label();
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
            cardTongLoai.SuspendLayout();
            cardDangSuDung.SuspendLayout();
            cardChuaSuDung.SuspendLayout();
            tableCenter.SuspendLayout();
            panelThongTinLoai.SuspendLayout();
            panelDanhSachLoai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachLoai).BeginInit();
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
            btnLoaiMon.BackColor = Color.FromArgb(94, 64, 47);
            btnLoaiMon.FlatAppearance.BorderSize = 0;
            btnLoaiMon.FlatStyle = FlatStyle.Flat;
            btnLoaiMon.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnLoaiMon.ForeColor = Color.White;
            btnLoaiMon.Location = new Point(0, 158);
            btnLoaiMon.Margin = new Padding(0);
            btnLoaiMon.Name = "btnLoaiMon";
            btnLoaiMon.Padding = new Padding(20, 0, 0, 0);
            btnLoaiMon.Size = new Size(230, 48);
            btnLoaiMon.TabIndex = 3;
            btnLoaiMon.Text = "🏷  Loại món";
            btnLoaiMon.TextAlign = ContentAlignment.MiddleLeft;
            btnLoaiMon.UseVisualStyleBackColor = false;
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
            tableStats.ColumnCount = 3;
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            tableStats.Controls.Add(cardTongLoai, 0, 0);
            tableStats.Controls.Add(cardDangSuDung, 1, 0);
            tableStats.Controls.Add(cardChuaSuDung, 2, 0);
            tableStats.Dock = DockStyle.Fill;
            tableStats.Location = new Point(0, 0);
            tableStats.Margin = new Padding(0, 0, 0, 12);
            tableStats.Name = "tableStats";
            tableStats.RowCount = 1;
            tableStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableStats.Size = new Size(1090, 118);
            tableStats.TabIndex = 0;
            // 
            // cardTongLoai
            // 
            cardTongLoai.BackColor = Color.FromArgb(237, 247, 243);
            cardTongLoai.Controls.Add(lblTongLoaiValue);
            cardTongLoai.Controls.Add(lblTongLoaiTitle);
            cardTongLoai.Controls.Add(lblTongLoaiIcon);
            cardTongLoai.Dock = DockStyle.Fill;
            cardTongLoai.Location = new Point(0, 0);
            cardTongLoai.Margin = new Padding(0, 0, 8, 0);
            cardTongLoai.Name = "cardTongLoai";
            cardTongLoai.Padding = new Padding(16, 12, 16, 12);
            cardTongLoai.Size = new Size(355, 118);
            cardTongLoai.TabIndex = 0;
            // 
            // lblTongLoaiValue
            // 
            lblTongLoaiValue.AutoSize = true;
            lblTongLoaiValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTongLoaiValue.ForeColor = Color.FromArgb(34, 111, 92);
            lblTongLoaiValue.Location = new Point(18, 61);
            lblTongLoaiValue.Name = "lblTongLoaiValue";
            lblTongLoaiValue.Size = new Size(28, 37);
            lblTongLoaiValue.TabIndex = 2;
            lblTongLoaiValue.Text = "-";
            // 
            // lblTongLoaiTitle
            // 
            lblTongLoaiTitle.AutoSize = true;
            lblTongLoaiTitle.Font = new Font("Segoe UI", 10F);
            lblTongLoaiTitle.ForeColor = Color.FromArgb(90, 106, 101);
            lblTongLoaiTitle.Location = new Point(18, 36);
            lblTongLoaiTitle.Name = "lblTongLoaiTitle";
            lblTongLoaiTitle.Size = new Size(81, 23);
            lblTongLoaiTitle.TabIndex = 1;
            lblTongLoaiTitle.Text = "Tổng loại";
            // 
            // lblTongLoaiIcon
            // 
            lblTongLoaiIcon.AutoSize = true;
            lblTongLoaiIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblTongLoaiIcon.Location = new Point(16, 8);
            lblTongLoaiIcon.Name = "lblTongLoaiIcon";
            lblTongLoaiIcon.Size = new Size(39, 27);
            lblTongLoaiIcon.TabIndex = 0;
            lblTongLoaiIcon.Text = "🏷";
            // 
            // cardDangSuDung
            // 
            cardDangSuDung.BackColor = Color.FromArgb(255, 247, 235);
            cardDangSuDung.Controls.Add(lblDangSuDungValue);
            cardDangSuDung.Controls.Add(lblDangSuDungTitle);
            cardDangSuDung.Controls.Add(lblDangSuDungIcon);
            cardDangSuDung.Dock = DockStyle.Fill;
            cardDangSuDung.Location = new Point(363, 0);
            cardDangSuDung.Margin = new Padding(0, 0, 8, 0);
            cardDangSuDung.Name = "cardDangSuDung";
            cardDangSuDung.Padding = new Padding(16, 12, 16, 12);
            cardDangSuDung.Size = new Size(355, 118);
            cardDangSuDung.TabIndex = 1;
            // 
            // lblDangSuDungValue
            // 
            lblDangSuDungValue.AutoSize = true;
            lblDangSuDungValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblDangSuDungValue.ForeColor = Color.FromArgb(161, 101, 27);
            lblDangSuDungValue.Location = new Point(18, 61);
            lblDangSuDungValue.Name = "lblDangSuDungValue";
            lblDangSuDungValue.Size = new Size(28, 37);
            lblDangSuDungValue.TabIndex = 5;
            lblDangSuDungValue.Text = "-";
            // 
            // lblDangSuDungTitle
            // 
            lblDangSuDungTitle.AutoSize = true;
            lblDangSuDungTitle.Font = new Font("Segoe UI", 10F);
            lblDangSuDungTitle.ForeColor = Color.FromArgb(119, 98, 72);
            lblDangSuDungTitle.Location = new Point(18, 36);
            lblDangSuDungTitle.Name = "lblDangSuDungTitle";
            lblDangSuDungTitle.Size = new Size(118, 23);
            lblDangSuDungTitle.TabIndex = 4;
            lblDangSuDungTitle.Text = "Đang sử dụng";
            // 
            // lblDangSuDungIcon
            // 
            lblDangSuDungIcon.AutoSize = true;
            lblDangSuDungIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblDangSuDungIcon.Location = new Point(16, 8);
            lblDangSuDungIcon.Name = "lblDangSuDungIcon";
            lblDangSuDungIcon.Size = new Size(39, 27);
            lblDangSuDungIcon.TabIndex = 3;
            lblDangSuDungIcon.Text = "✅";
            // 
            // cardChuaSuDung
            // 
            cardChuaSuDung.BackColor = Color.FromArgb(246, 241, 255);
            cardChuaSuDung.Controls.Add(lblChuaSuDungValue);
            cardChuaSuDung.Controls.Add(lblChuaSuDungTitle);
            cardChuaSuDung.Controls.Add(lblChuaSuDungIcon);
            cardChuaSuDung.Dock = DockStyle.Fill;
            cardChuaSuDung.Location = new Point(726, 0);
            cardChuaSuDung.Margin = new Padding(0);
            cardChuaSuDung.Name = "cardChuaSuDung";
            cardChuaSuDung.Padding = new Padding(16, 12, 16, 12);
            cardChuaSuDung.Size = new Size(364, 118);
            cardChuaSuDung.TabIndex = 2;
            // 
            // lblChuaSuDungValue
            // 
            lblChuaSuDungValue.AutoSize = true;
            lblChuaSuDungValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblChuaSuDungValue.ForeColor = Color.FromArgb(105, 72, 168);
            lblChuaSuDungValue.Location = new Point(18, 61);
            lblChuaSuDungValue.Name = "lblChuaSuDungValue";
            lblChuaSuDungValue.Size = new Size(28, 37);
            lblChuaSuDungValue.TabIndex = 5;
            lblChuaSuDungValue.Text = "-";
            // 
            // lblChuaSuDungTitle
            // 
            lblChuaSuDungTitle.AutoSize = true;
            lblChuaSuDungTitle.Font = new Font("Segoe UI", 10F);
            lblChuaSuDungTitle.ForeColor = Color.FromArgb(113, 96, 150);
            lblChuaSuDungTitle.Location = new Point(18, 36);
            lblChuaSuDungTitle.Name = "lblChuaSuDungTitle";
            lblChuaSuDungTitle.Size = new Size(117, 23);
            lblChuaSuDungTitle.TabIndex = 4;
            lblChuaSuDungTitle.Text = "Chưa sử dụng";
            // 
            // lblChuaSuDungIcon
            // 
            lblChuaSuDungIcon.AutoSize = true;
            lblChuaSuDungIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblChuaSuDungIcon.Location = new Point(16, 8);
            lblChuaSuDungIcon.Name = "lblChuaSuDungIcon";
            lblChuaSuDungIcon.Size = new Size(39, 27);
            lblChuaSuDungIcon.TabIndex = 3;
            lblChuaSuDungIcon.Text = "🗂";
            // 
            // tableCenter
            // 
            tableCenter.ColumnCount = 2;
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            tableCenter.Controls.Add(panelThongTinLoai, 0, 0);
            tableCenter.Controls.Add(panelDanhSachLoai, 1, 0);
            tableCenter.Dock = DockStyle.Fill;
            tableCenter.Location = new Point(0, 130);
            tableCenter.Margin = new Padding(0);
            tableCenter.Name = "tableCenter";
            tableCenter.RowCount = 1;
            tableCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableCenter.Size = new Size(1090, 512);
            tableCenter.TabIndex = 1;
            // 
            // panelThongTinLoai
            // 
            panelThongTinLoai.BackColor = Color.White;
            panelThongTinLoai.Controls.Add(btnLamMoi);
            panelThongTinLoai.Controls.Add(btnXoaLoai);
            panelThongTinLoai.Controls.Add(btnCapNhatLoai);
            panelThongTinLoai.Controls.Add(btnThemLoai);
            panelThongTinLoai.Controls.Add(txtTimLoai);
            panelThongTinLoai.Controls.Add(lblTimLoai);
            panelThongTinLoai.Controls.Add(txtTenLoai);
            panelThongTinLoai.Controls.Add(lblTenLoai);
            panelThongTinLoai.Controls.Add(txtMaLoai);
            panelThongTinLoai.Controls.Add(lblMaLoai);
            panelThongTinLoai.Controls.Add(lblThongTinLoaiTitle);
            panelThongTinLoai.Dock = DockStyle.Fill;
            panelThongTinLoai.Location = new Point(0, 0);
            panelThongTinLoai.Margin = new Padding(0, 0, 10, 0);
            panelThongTinLoai.Name = "panelThongTinLoai";
            panelThongTinLoai.Padding = new Padding(16, 14, 16, 14);
            panelThongTinLoai.Size = new Size(338, 512);
            panelThongTinLoai.TabIndex = 0;
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
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnXoaLoai
            // 
            btnXoaLoai.BackColor = Color.FromArgb(254, 241, 241);
            btnXoaLoai.FlatAppearance.BorderSize = 0;
            btnXoaLoai.FlatStyle = FlatStyle.Flat;
            btnXoaLoai.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnXoaLoai.ForeColor = Color.FromArgb(162, 52, 64);
            btnXoaLoai.Location = new Point(16, 408);
            btnXoaLoai.Name = "btnXoaLoai";
            btnXoaLoai.Size = new Size(306, 38);
            btnXoaLoai.TabIndex = 9;
            btnXoaLoai.Text = "Xóa loại món";
            btnXoaLoai.UseVisualStyleBackColor = false;
            btnXoaLoai.Click += btnXoaLoai_Click;
            // 
            // btnCapNhatLoai
            // 
            btnCapNhatLoai.BackColor = Color.FromArgb(246, 241, 255);
            btnCapNhatLoai.FlatAppearance.BorderSize = 0;
            btnCapNhatLoai.FlatStyle = FlatStyle.Flat;
            btnCapNhatLoai.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnCapNhatLoai.ForeColor = Color.FromArgb(105, 72, 168);
            btnCapNhatLoai.Location = new Point(16, 364);
            btnCapNhatLoai.Name = "btnCapNhatLoai";
            btnCapNhatLoai.Size = new Size(306, 38);
            btnCapNhatLoai.TabIndex = 8;
            btnCapNhatLoai.Text = "Cập nhật loại món";
            btnCapNhatLoai.UseVisualStyleBackColor = false;
            btnCapNhatLoai.Click += btnCapNhatLoai_Click;
            // 
            // btnThemLoai
            // 
            btnThemLoai.BackColor = Color.FromArgb(236, 245, 241);
            btnThemLoai.FlatAppearance.BorderSize = 0;
            btnThemLoai.FlatStyle = FlatStyle.Flat;
            btnThemLoai.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnThemLoai.ForeColor = Color.FromArgb(34, 111, 92);
            btnThemLoai.Location = new Point(16, 320);
            btnThemLoai.Name = "btnThemLoai";
            btnThemLoai.Size = new Size(306, 38);
            btnThemLoai.TabIndex = 7;
            btnThemLoai.Text = "+ Thêm loại món";
            btnThemLoai.UseVisualStyleBackColor = false;
            btnThemLoai.Click += btnThemLoai_Click;
            // 
            // txtTimLoai
            // 
            txtTimLoai.BackColor = Color.FromArgb(248, 245, 241);
            txtTimLoai.BorderStyle = BorderStyle.FixedSingle;
            txtTimLoai.Font = new Font("Segoe UI", 9.5F);
            txtTimLoai.Location = new Point(16, 198);
            txtTimLoai.Name = "txtTimLoai";
            txtTimLoai.PlaceholderText = "Nhập ID hoặc tên loại...";
            txtTimLoai.Size = new Size(306, 29);
            txtTimLoai.TabIndex = 6;
            // 
            // lblTimLoai
            // 
            lblTimLoai.AutoSize = true;
            lblTimLoai.Font = new Font("Segoe UI", 9.5F);
            lblTimLoai.ForeColor = Color.DimGray;
            lblTimLoai.Location = new Point(16, 172);
            lblTimLoai.Name = "lblTimLoai";
            lblTimLoai.Size = new Size(122, 21);
            lblTimLoai.TabIndex = 5;
            lblTimLoai.Text = "Tìm kiếm nhanh";
            // 
            // txtTenLoai
            // 
            txtTenLoai.BackColor = Color.FromArgb(248, 245, 241);
            txtTenLoai.BorderStyle = BorderStyle.FixedSingle;
            txtTenLoai.Font = new Font("Segoe UI", 9.5F);
            txtTenLoai.Location = new Point(16, 129);
            txtTenLoai.Name = "txtTenLoai";
            txtTenLoai.PlaceholderText = "Nhập tên loại món...";
            txtTenLoai.Size = new Size(306, 29);
            txtTenLoai.TabIndex = 4;
            // 
            // lblTenLoai
            // 
            lblTenLoai.AutoSize = true;
            lblTenLoai.Font = new Font("Segoe UI", 9.5F);
            lblTenLoai.ForeColor = Color.DimGray;
            lblTenLoai.Location = new Point(16, 103);
            lblTenLoai.Name = "lblTenLoai";
            lblTenLoai.Size = new Size(62, 21);
            lblTenLoai.TabIndex = 3;
            lblTenLoai.Text = "Tên loại";
            // 
            // txtMaLoai
            // 
            txtMaLoai.BackColor = Color.FromArgb(248, 245, 241);
            txtMaLoai.BorderStyle = BorderStyle.FixedSingle;
            txtMaLoai.Font = new Font("Segoe UI", 9.5F);
            txtMaLoai.Location = new Point(16, 72);
            txtMaLoai.Name = "txtMaLoai";
            txtMaLoai.PlaceholderText = "Tự động tăng";
            txtMaLoai.ReadOnly = true;
            txtMaLoai.Size = new Size(306, 29);
            txtMaLoai.TabIndex = 2;
            // 
            // lblMaLoai
            // 
            lblMaLoai.AutoSize = true;
            lblMaLoai.Font = new Font("Segoe UI", 9.5F);
            lblMaLoai.ForeColor = Color.DimGray;
            lblMaLoai.Location = new Point(16, 46);
            lblMaLoai.Name = "lblMaLoai";
            lblMaLoai.Size = new Size(54, 21);
            lblMaLoai.TabIndex = 1;
            lblMaLoai.Text = "ID loại";
            // 
            // lblThongTinLoaiTitle
            // 
            lblThongTinLoaiTitle.AutoSize = true;
            lblThongTinLoaiTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblThongTinLoaiTitle.Location = new Point(14, 16);
            lblThongTinLoaiTitle.Name = "lblThongTinLoaiTitle";
            lblThongTinLoaiTitle.Size = new Size(171, 25);
            lblThongTinLoaiTitle.TabIndex = 0;
            lblThongTinLoaiTitle.Text = "Thông tin loại món";
            // 
            // panelDanhSachLoai
            // 
            panelDanhSachLoai.BackColor = Color.White;
            panelDanhSachLoai.Controls.Add(dgvDanhSachLoai);
            panelDanhSachLoai.Controls.Add(panelDanhSachHeader);
            panelDanhSachLoai.Dock = DockStyle.Fill;
            panelDanhSachLoai.Location = new Point(348, 0);
            panelDanhSachLoai.Margin = new Padding(0);
            panelDanhSachLoai.Name = "panelDanhSachLoai";
            panelDanhSachLoai.Padding = new Padding(12);
            panelDanhSachLoai.Size = new Size(742, 512);
            panelDanhSachLoai.TabIndex = 1;
            // 
            // dgvDanhSachLoai
            // 
            dgvDanhSachLoai.AllowUserToAddRows = false;
            dgvDanhSachLoai.AllowUserToDeleteRows = false;
            dgvDanhSachLoai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhSachLoai.BackgroundColor = Color.White;
            dgvDanhSachLoai.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Control;
            dataGridViewCellStyle11.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle11.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.True;
            dgvDanhSachLoai.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            dgvDanhSachLoai.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDanhSachLoai.Columns.AddRange(new DataGridViewColumn[] { colIDLoai, colTenLoai, colSoMon });
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = SystemColors.Window;
            dataGridViewCellStyle12.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle12.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.False;
            dgvDanhSachLoai.DefaultCellStyle = dataGridViewCellStyle12;
            dgvDanhSachLoai.Dock = DockStyle.Fill;
            dgvDanhSachLoai.GridColor = Color.Gainsboro;
            dgvDanhSachLoai.Location = new Point(12, 55);
            dgvDanhSachLoai.MultiSelect = false;
            dgvDanhSachLoai.Name = "dgvDanhSachLoai";
            dgvDanhSachLoai.ReadOnly = true;
            dgvDanhSachLoai.RowHeadersVisible = false;
            dgvDanhSachLoai.RowHeadersWidth = 51;
            dgvDanhSachLoai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhSachLoai.Size = new Size(718, 445);
            dgvDanhSachLoai.TabIndex = 1;
            // 
            // colIDLoai
            // 
            colIDLoai.FillWeight = 60F;
            colIDLoai.HeaderText = "ID";
            colIDLoai.MinimumWidth = 6;
            colIDLoai.Name = "colIDLoai";
            colIDLoai.ReadOnly = true;
            // 
            // colTenLoai
            // 
            colTenLoai.FillWeight = 180F;
            colTenLoai.HeaderText = "Tên loại";
            colTenLoai.MinimumWidth = 6;
            colTenLoai.Name = "colTenLoai";
            colTenLoai.ReadOnly = true;
            // 
            // colSoMon
            // 
            colSoMon.FillWeight = 90F;
            colSoMon.HeaderText = "Số món";
            colSoMon.MinimumWidth = 6;
            colSoMon.Name = "colSoMon";
            colSoMon.ReadOnly = true;
            // 
            // panelDanhSachHeader
            // 
            panelDanhSachHeader.Controls.Add(btnNhapLoai);
            panelDanhSachHeader.Controls.Add(btnXuatLoai);
            panelDanhSachHeader.Controls.Add(lblDanhSachLoaiTitle);
            panelDanhSachHeader.Dock = DockStyle.Top;
            panelDanhSachHeader.Location = new Point(12, 12);
            panelDanhSachHeader.Name = "panelDanhSachHeader";
            panelDanhSachHeader.Size = new Size(718, 43);
            panelDanhSachHeader.TabIndex = 0;
            // 
            // btnNhapLoai
            // 
            btnNhapLoai.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNhapLoai.BackColor = Color.FromArgb(248, 245, 241);
            btnNhapLoai.FlatAppearance.BorderSize = 0;
            btnNhapLoai.FlatStyle = FlatStyle.Flat;
            btnNhapLoai.Font = new Font("Segoe UI", 8.5F);
            btnNhapLoai.ForeColor = Color.FromArgb(65, 48, 39);
            btnNhapLoai.Location = new Point(536, 4);
            btnNhapLoai.Name = "btnNhapLoai";
            btnNhapLoai.Size = new Size(84, 32);
            btnNhapLoai.TabIndex = 2;
            btnNhapLoai.Text = "Nhập";
            btnNhapLoai.UseVisualStyleBackColor = false;
            btnNhapLoai.Click += btnNhapLoai_Click;
            // 
            // btnXuatLoai
            // 
            btnXuatLoai.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXuatLoai.BackColor = Color.FromArgb(248, 245, 241);
            btnXuatLoai.FlatAppearance.BorderSize = 0;
            btnXuatLoai.FlatStyle = FlatStyle.Flat;
            btnXuatLoai.Font = new Font("Segoe UI", 8.5F);
            btnXuatLoai.ForeColor = Color.FromArgb(65, 48, 39);
            btnXuatLoai.Location = new Point(626, 4);
            btnXuatLoai.Name = "btnXuatLoai";
            btnXuatLoai.Size = new Size(84, 32);
            btnXuatLoai.TabIndex = 1;
            btnXuatLoai.Text = "Xuất";
            btnXuatLoai.UseVisualStyleBackColor = false;
            btnXuatLoai.Click += btnXuatLoai_Click;
            // 
            // lblDanhSachLoaiTitle
            // 
            lblDanhSachLoaiTitle.AutoSize = true;
            lblDanhSachLoaiTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblDanhSachLoaiTitle.Location = new Point(0, 8);
            lblDanhSachLoaiTitle.Name = "lblDanhSachLoaiTitle";
            lblDanhSachLoaiTitle.Size = new Size(173, 25);
            lblDanhSachLoaiTitle.TabIndex = 0;
            lblDanhSachLoaiTitle.Text = "Danh sách loại món";
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
            txtSearch.PlaceholderText = "🔍  Tìm loại món theo ID hoặc tên...";
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
            lblPageTitle.Size = new Size(97, 28);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Loại món";
            // 
            // frmLoaiMon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 242, 236);
            ClientSize = new Size(1364, 760);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1220, 720);
            Name = "frmLoaiMon";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Coffee Shop - Loại món";
            panelSidebar.ResumeLayout(false);
            flowSidebarMenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelMain.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            tableMain.ResumeLayout(false);
            tableStats.ResumeLayout(false);
            cardTongLoai.ResumeLayout(false);
            cardTongLoai.PerformLayout();
            cardDangSuDung.ResumeLayout(false);
            cardDangSuDung.PerformLayout();
            cardChuaSuDung.ResumeLayout(false);
            cardChuaSuDung.PerformLayout();
            tableCenter.ResumeLayout(false);
            panelThongTinLoai.ResumeLayout(false);
            panelThongTinLoai.PerformLayout();
            panelDanhSachLoai.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachLoai).EndInit();
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
        private Panel cardTongLoai;
        private Label lblTongLoaiValue;
        private Label lblTongLoaiTitle;
        private Label lblTongLoaiIcon;
        private Panel cardDangSuDung;
        private Label lblDangSuDungValue;
        private Label lblDangSuDungTitle;
        private Label lblDangSuDungIcon;
        private Panel cardChuaSuDung;
        private Label lblChuaSuDungValue;
        private Label lblChuaSuDungTitle;
        private Label lblChuaSuDungIcon;
        private TableLayoutPanel tableCenter;
        private Panel panelThongTinLoai;
        private Button btnLamMoi;
        private Button btnXoaLoai;
        private Button btnCapNhatLoai;
        private Button btnThemLoai;
        private TextBox txtTimLoai;
        private Label lblTimLoai;
        private TextBox txtTenLoai;
        private Label lblTenLoai;
        private TextBox txtMaLoai;
        private Label lblMaLoai;
        private Label lblThongTinLoaiTitle;
        private Panel panelDanhSachLoai;
        private DataGridView dgvDanhSachLoai;
        private DataGridViewTextBoxColumn colIDLoai;
        private DataGridViewTextBoxColumn colTenLoai;
        private DataGridViewTextBoxColumn colSoMon;
        private Panel panelDanhSachHeader;
        private Button btnNhapLoai;
        private Button btnXuatLoai;
        private Label lblDanhSachLoaiTitle;
        private Panel panelTopbar;
        private Button btnUserMenu;
        private Label lblUserName;
        private PictureBox picAvatar;
        private TextBox txtSearch;
        private Label lblPageTitle;
    }
}