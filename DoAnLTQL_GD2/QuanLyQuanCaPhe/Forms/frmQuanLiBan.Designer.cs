namespace QuanLyQuanCaPhe.Forms
{
    partial class frmQuanLiBan
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
            cardTongBan = new Panel();
            lblTongBanValue = new Label();
            lblTongBanTitle = new Label();
            lblTongBanIcon = new Label();
            cardDangPhucVu = new Panel();
            lblDangPhucVuValue = new Label();
            lblDangPhucVuTitle = new Label();
            lblDangPhucVuIcon = new Label();
            cardBanTrong = new Panel();
            lblBanTrongValue = new Label();
            lblBanTrongTitle = new Label();
            lblBanTrongIcon = new Label();
            cardDatTruoc = new Panel();
            lblDatTruocValue = new Label();
            lblDatTruocTitle = new Label();
            lblDatTruocIcon = new Label();
            tableCenter = new TableLayoutPanel();
            panelFilter = new Panel();
            btnLamMoi = new Button();
            btnGopBan = new Button();
            btnXoaBan = new Button();
            btnThemBan = new Button();
            cboTrangThai = new ComboBox();
            lblTrangThai = new Label();
            cboKhuVuc = new ComboBox();
            lblKhuVuc = new Label();
            lblBoLocTitle = new Label();
            panelFloor = new Panel();
            flowBanSoDo = new FlowLayoutPanel();
            lblFloorTitle = new Label();
            panelBottom = new Panel();
            dgvDanhSachBan = new DataGridView();
            colMaBan = new DataGridViewTextBoxColumn();
            colTenBan = new DataGridViewTextBoxColumn();
            colKhuVuc = new DataGridViewTextBoxColumn();
            colSucChua = new DataGridViewTextBoxColumn();
            colTinhTrang = new DataGridViewTextBoxColumn();
            lblDanhSachBanTitle = new Label();
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
            cardTongBan.SuspendLayout();
            cardDangPhucVu.SuspendLayout();
            cardBanTrong.SuspendLayout();
            cardDatTruoc.SuspendLayout();
            tableCenter.SuspendLayout();
            panelFilter.SuspendLayout();
            panelFloor.SuspendLayout();
            panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachBan).BeginInit();
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
            btnQuanLyBan.BackColor = Color.FromArgb(94, 64, 47);
            btnQuanLyBan.FlatAppearance.BorderSize = 0;
            btnQuanLyBan.FlatStyle = FlatStyle.Flat;
            btnQuanLyBan.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnQuanLyBan.ForeColor = Color.White;
            btnQuanLyBan.Location = new Point(0, 62);
            btnQuanLyBan.Margin = new Padding(0);
            btnQuanLyBan.Name = "btnQuanLyBan";
            btnQuanLyBan.Padding = new Padding(20, 0, 0, 0);
            btnQuanLyBan.Size = new Size(230, 48);
            btnQuanLyBan.TabIndex = 1;
            btnQuanLyBan.Text = "\U0001fa91  Quản lý bàn";
            btnQuanLyBan.TextAlign = ContentAlignment.MiddleLeft;
            btnQuanLyBan.UseVisualStyleBackColor = false;
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
            tableMain.Controls.Add(tableCenter, 0, 1);
            tableMain.Controls.Add(panelBottom, 0, 2);
            tableMain.Dock = DockStyle.Fill;
            tableMain.Location = new Point(22, 16);
            tableMain.Name = "tableMain";
            tableMain.RowCount = 3;
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 320F));
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
            tableStats.Controls.Add(cardTongBan, 0, 0);
            tableStats.Controls.Add(cardDangPhucVu, 1, 0);
            tableStats.Controls.Add(cardBanTrong, 2, 0);
            tableStats.Controls.Add(cardDatTruoc, 3, 0);
            tableStats.Dock = DockStyle.Fill;
            tableStats.Location = new Point(0, 0);
            tableStats.Margin = new Padding(0, 0, 0, 12);
            tableStats.Name = "tableStats";
            tableStats.RowCount = 1;
            tableStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableStats.Size = new Size(1090, 118);
            tableStats.TabIndex = 0;
            // 
            // cardTongBan
            // 
            cardTongBan.BackColor = Color.FromArgb(237, 247, 243);
            cardTongBan.Controls.Add(lblTongBanValue);
            cardTongBan.Controls.Add(lblTongBanTitle);
            cardTongBan.Controls.Add(lblTongBanIcon);
            cardTongBan.Dock = DockStyle.Fill;
            cardTongBan.Location = new Point(0, 0);
            cardTongBan.Margin = new Padding(0, 0, 8, 0);
            cardTongBan.Name = "cardTongBan";
            cardTongBan.Padding = new Padding(16, 12, 16, 12);
            cardTongBan.Size = new Size(264, 118);
            cardTongBan.TabIndex = 0;
            // 
            // lblTongBanValue
            // 
            lblTongBanValue.AutoSize = true;
            lblTongBanValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTongBanValue.ForeColor = Color.FromArgb(34, 111, 92);
            lblTongBanValue.Location = new Point(18, 61);
            lblTongBanValue.Name = "lblTongBanValue";
            lblTongBanValue.Size = new Size(0, 37);
            lblTongBanValue.TabIndex = 2;
            // 
            // lblTongBanTitle
            // 
            lblTongBanTitle.AutoSize = true;
            lblTongBanTitle.Font = new Font("Segoe UI", 10F);
            lblTongBanTitle.ForeColor = Color.FromArgb(90, 106, 101);
            lblTongBanTitle.Location = new Point(18, 36);
            lblTongBanTitle.Name = "lblTongBanTitle";
            lblTongBanTitle.Size = new Size(83, 23);
            lblTongBanTitle.TabIndex = 1;
            lblTongBanTitle.Text = "Tổng bàn";
            // 
            // lblTongBanIcon
            // 
            lblTongBanIcon.AutoSize = true;
            lblTongBanIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblTongBanIcon.Location = new Point(16, 8);
            lblTongBanIcon.Name = "lblTongBanIcon";
            lblTongBanIcon.Size = new Size(39, 27);
            lblTongBanIcon.TabIndex = 0;
            lblTongBanIcon.Text = "\U0001fa91";
            // 
            // cardDangPhucVu
            // 
            cardDangPhucVu.BackColor = Color.FromArgb(255, 247, 235);
            cardDangPhucVu.Controls.Add(lblDangPhucVuValue);
            cardDangPhucVu.Controls.Add(lblDangPhucVuTitle);
            cardDangPhucVu.Controls.Add(lblDangPhucVuIcon);
            cardDangPhucVu.Dock = DockStyle.Fill;
            cardDangPhucVu.Location = new Point(272, 0);
            cardDangPhucVu.Margin = new Padding(0, 0, 8, 0);
            cardDangPhucVu.Name = "cardDangPhucVu";
            cardDangPhucVu.Padding = new Padding(16, 12, 16, 12);
            cardDangPhucVu.Size = new Size(264, 118);
            cardDangPhucVu.TabIndex = 1;
            // 
            // lblDangPhucVuValue
            // 
            lblDangPhucVuValue.AutoSize = true;
            lblDangPhucVuValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblDangPhucVuValue.ForeColor = Color.FromArgb(161, 101, 27);
            lblDangPhucVuValue.Location = new Point(18, 61);
            lblDangPhucVuValue.Name = "lblDangPhucVuValue";
            lblDangPhucVuValue.Size = new Size(0, 37);
            lblDangPhucVuValue.TabIndex = 5;
            // 
            // lblDangPhucVuTitle
            // 
            lblDangPhucVuTitle.AutoSize = true;
            lblDangPhucVuTitle.Font = new Font("Segoe UI", 10F);
            lblDangPhucVuTitle.ForeColor = Color.FromArgb(119, 98, 72);
            lblDangPhucVuTitle.Location = new Point(18, 36);
            lblDangPhucVuTitle.Name = "lblDangPhucVuTitle";
            lblDangPhucVuTitle.Size = new Size(117, 23);
            lblDangPhucVuTitle.TabIndex = 4;
            lblDangPhucVuTitle.Text = "Đang phục vụ";
            // 
            // lblDangPhucVuIcon
            // 
            lblDangPhucVuIcon.AutoSize = true;
            lblDangPhucVuIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblDangPhucVuIcon.Location = new Point(16, 8);
            lblDangPhucVuIcon.Name = "lblDangPhucVuIcon";
            lblDangPhucVuIcon.Size = new Size(39, 27);
            lblDangPhucVuIcon.TabIndex = 3;
            lblDangPhucVuIcon.Text = "🍽";
            // 
            // cardBanTrong
            // 
            cardBanTrong.BackColor = Color.FromArgb(246, 241, 255);
            cardBanTrong.Controls.Add(lblBanTrongValue);
            cardBanTrong.Controls.Add(lblBanTrongTitle);
            cardBanTrong.Controls.Add(lblBanTrongIcon);
            cardBanTrong.Dock = DockStyle.Fill;
            cardBanTrong.Location = new Point(544, 0);
            cardBanTrong.Margin = new Padding(0, 0, 8, 0);
            cardBanTrong.Name = "cardBanTrong";
            cardBanTrong.Padding = new Padding(16, 12, 16, 12);
            cardBanTrong.Size = new Size(264, 118);
            cardBanTrong.TabIndex = 2;
            // 
            // lblBanTrongValue
            // 
            lblBanTrongValue.AutoSize = true;
            lblBanTrongValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblBanTrongValue.ForeColor = Color.FromArgb(105, 72, 168);
            lblBanTrongValue.Location = new Point(18, 61);
            lblBanTrongValue.Name = "lblBanTrongValue";
            lblBanTrongValue.Size = new Size(0, 37);
            lblBanTrongValue.TabIndex = 5;
            // 
            // lblBanTrongTitle
            // 
            lblBanTrongTitle.AutoSize = true;
            lblBanTrongTitle.Font = new Font("Segoe UI", 10F);
            lblBanTrongTitle.ForeColor = Color.FromArgb(113, 96, 150);
            lblBanTrongTitle.Location = new Point(18, 36);
            lblBanTrongTitle.Name = "lblBanTrongTitle";
            lblBanTrongTitle.Size = new Size(86, 23);
            lblBanTrongTitle.TabIndex = 4;
            lblBanTrongTitle.Text = "Bàn trống";
            // 
            // lblBanTrongIcon
            // 
            lblBanTrongIcon.AutoSize = true;
            lblBanTrongIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblBanTrongIcon.Location = new Point(16, 8);
            lblBanTrongIcon.Name = "lblBanTrongIcon";
            lblBanTrongIcon.Size = new Size(39, 27);
            lblBanTrongIcon.TabIndex = 3;
            lblBanTrongIcon.Text = "\U0001f7e2";
            // 
            // cardDatTruoc
            // 
            cardDatTruoc.BackColor = Color.FromArgb(254, 241, 241);
            cardDatTruoc.Controls.Add(lblDatTruocValue);
            cardDatTruoc.Controls.Add(lblDatTruocTitle);
            cardDatTruoc.Controls.Add(lblDatTruocIcon);
            cardDatTruoc.Dock = DockStyle.Fill;
            cardDatTruoc.Location = new Point(816, 0);
            cardDatTruoc.Margin = new Padding(0);
            cardDatTruoc.Name = "cardDatTruoc";
            cardDatTruoc.Padding = new Padding(16, 12, 16, 12);
            cardDatTruoc.Size = new Size(274, 118);
            cardDatTruoc.TabIndex = 3;
            // 
            // lblDatTruocValue
            // 
            lblDatTruocValue.AutoSize = true;
            lblDatTruocValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblDatTruocValue.ForeColor = Color.FromArgb(162, 52, 64);
            lblDatTruocValue.Location = new Point(18, 61);
            lblDatTruocValue.Name = "lblDatTruocValue";
            lblDatTruocValue.Size = new Size(0, 37);
            lblDatTruocValue.TabIndex = 5;
            // 
            // lblDatTruocTitle
            // 
            lblDatTruocTitle.AutoSize = true;
            lblDatTruocTitle.Font = new Font("Segoe UI", 10F);
            lblDatTruocTitle.ForeColor = Color.FromArgb(130, 90, 95);
            lblDatTruocTitle.Location = new Point(18, 36);
            lblDatTruocTitle.Name = "lblDatTruocTitle";
            lblDatTruocTitle.Size = new Size(82, 23);
            lblDatTruocTitle.TabIndex = 4;
            lblDatTruocTitle.Text = "Đặt trước";
            // 
            // lblDatTruocIcon
            // 
            lblDatTruocIcon.AutoSize = true;
            lblDatTruocIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblDatTruocIcon.Location = new Point(16, 8);
            lblDatTruocIcon.Name = "lblDatTruocIcon";
            lblDatTruocIcon.Size = new Size(39, 27);
            lblDatTruocIcon.TabIndex = 3;
            lblDatTruocIcon.Text = "📌";
            // 
            // tableCenter
            // 
            tableCenter.ColumnCount = 2;
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableCenter.Controls.Add(panelFilter, 0, 0);
            tableCenter.Controls.Add(panelFloor, 1, 0);
            tableCenter.Dock = DockStyle.Fill;
            tableCenter.Location = new Point(0, 130);
            tableCenter.Margin = new Padding(0, 0, 0, 12);
            tableCenter.Name = "tableCenter";
            tableCenter.RowCount = 1;
            tableCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableCenter.Size = new Size(1090, 308);
            tableCenter.TabIndex = 1;
            // 
            // panelFilter
            // 
            panelFilter.BackColor = Color.White;
            panelFilter.Controls.Add(btnLamMoi);
            panelFilter.Controls.Add(btnGopBan);
            panelFilter.Controls.Add(btnXoaBan);
            panelFilter.Controls.Add(btnThemBan);
            panelFilter.Controls.Add(cboTrangThai);
            panelFilter.Controls.Add(lblTrangThai);
            panelFilter.Controls.Add(cboKhuVuc);
            panelFilter.Controls.Add(lblKhuVuc);
            panelFilter.Controls.Add(lblBoLocTitle);
            panelFilter.Dock = DockStyle.Fill;
            panelFilter.Location = new Point(0, 0);
            panelFilter.Margin = new Padding(0, 0, 8, 0);
            panelFilter.Name = "panelFilter";
            panelFilter.Padding = new Padding(16, 14, 16, 14);
            panelFilter.Size = new Size(319, 308);
            panelFilter.TabIndex = 0;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(94, 64, 47);
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(16, 272);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(287, 30);
            btnLamMoi.TabIndex = 8;
            btnLamMoi.Text = "Làm mới layout";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnGopBan
            // 
            btnGopBan.BackColor = Color.FromArgb(226, 214, 199);
            btnGopBan.FlatAppearance.BorderSize = 0;
            btnGopBan.FlatStyle = FlatStyle.Flat;
            btnGopBan.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnGopBan.ForeColor = Color.FromArgb(65, 48, 39);
            btnGopBan.Location = new Point(16, 235);
            btnGopBan.Name = "btnGopBan";
            btnGopBan.Size = new Size(287, 38);
            btnGopBan.TabIndex = 7;
            btnGopBan.Text = "Chuyển / Gộp bàn";
            btnGopBan.UseVisualStyleBackColor = false;
            btnGopBan.Click += btnGopBan_Click;
            // 
            // btnXoaBan
            // 
            btnXoaBan.BackColor = Color.FromArgb(254, 241, 241);
            btnXoaBan.FlatAppearance.BorderSize = 0;
            btnXoaBan.FlatStyle = FlatStyle.Flat;
            btnXoaBan.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnXoaBan.ForeColor = Color.FromArgb(162, 52, 64);
            btnXoaBan.Location = new Point(16, 197);
            btnXoaBan.Name = "btnXoaBan";
            btnXoaBan.Size = new Size(287, 38);
            btnXoaBan.TabIndex = 6;
            btnXoaBan.Text = "Xóa bàn đang chọn";
            btnXoaBan.UseVisualStyleBackColor = false;
            btnXoaBan.Click += btnXoaBan_Click;
            // 
            // btnThemBan
            // 
            btnThemBan.BackColor = Color.FromArgb(236, 245, 241);
            btnThemBan.FlatAppearance.BorderSize = 0;
            btnThemBan.FlatStyle = FlatStyle.Flat;
            btnThemBan.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnThemBan.ForeColor = Color.FromArgb(34, 111, 92);
            btnThemBan.Location = new Point(16, 158);
            btnThemBan.Name = "btnThemBan";
            btnThemBan.Size = new Size(287, 38);
            btnThemBan.TabIndex = 5;
            btnThemBan.Text = "+ Thêm bàn mới";
            btnThemBan.UseVisualStyleBackColor = false;
            btnThemBan.Click += btnThemBan_Click;
            // 
            // cboTrangThai
            // 
            cboTrangThai.BackColor = Color.FromArgb(248, 245, 241);
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.FlatStyle = FlatStyle.Flat;
            cboTrangThai.FormattingEnabled = true;
            cboTrangThai.Items.AddRange(new object[] { "Tất cả trạng thái", "Trống", "Đang phục vụ", "Đặt trước" });
            cboTrangThai.Location = new Point(16, 116);
            cboTrangThai.Name = "cboTrangThai";
            cboTrangThai.Size = new Size(287, 28);
            cboTrangThai.TabIndex = 4;
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.Font = new Font("Segoe UI", 9.5F);
            lblTrangThai.ForeColor = Color.DimGray;
            lblTrangThai.Location = new Point(16, 91);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(79, 21);
            lblTrangThai.TabIndex = 3;
            lblTrangThai.Text = "Trạng thái";
            // 
            // cboKhuVuc
            // 
            cboKhuVuc.BackColor = Color.FromArgb(248, 245, 241);
            cboKhuVuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhuVuc.FlatStyle = FlatStyle.Flat;
            cboKhuVuc.FormattingEnabled = true;
            cboKhuVuc.Items.AddRange(new object[] { "Tất cả khu vực", "Khu trong nhà", "Khu sân vườn", "Khu phòng riêng" });
            cboKhuVuc.Location = new Point(16, 60);
            cboKhuVuc.Name = "cboKhuVuc";
            cboKhuVuc.Size = new Size(287, 28);
            cboKhuVuc.TabIndex = 2;
            // 
            // lblKhuVuc
            // 
            lblKhuVuc.AutoSize = true;
            lblKhuVuc.Font = new Font("Segoe UI", 9.5F);
            lblKhuVuc.ForeColor = Color.DimGray;
            lblKhuVuc.Location = new Point(16, 35);
            lblKhuVuc.Name = "lblKhuVuc";
            lblKhuVuc.Size = new Size(65, 21);
            lblKhuVuc.TabIndex = 1;
            lblKhuVuc.Text = "Khu vực";
            // 
            // lblBoLocTitle
            // 
            lblBoLocTitle.AutoSize = true;
            lblBoLocTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblBoLocTitle.Location = new Point(14, 8);
            lblBoLocTitle.Name = "lblBoLocTitle";
            lblBoLocTitle.Size = new Size(119, 25);
            lblBoLocTitle.TabIndex = 0;
            lblBoLocTitle.Text = "Bộ lọc nhanh";
            // 
            // panelFloor
            // 
            panelFloor.BackColor = Color.White;
            panelFloor.Controls.Add(flowBanSoDo);
            panelFloor.Controls.Add(lblFloorTitle);
            panelFloor.Dock = DockStyle.Fill;
            panelFloor.Location = new Point(327, 0);
            panelFloor.Margin = new Padding(0);
            panelFloor.Name = "panelFloor";
            panelFloor.Padding = new Padding(14);
            panelFloor.Size = new Size(763, 308);
            panelFloor.TabIndex = 1;
            // 
            // flowBanSoDo
            // 
            flowBanSoDo.AutoScroll = true;
            flowBanSoDo.Dock = DockStyle.Fill;
            flowBanSoDo.Location = new Point(14, 39);
            flowBanSoDo.Name = "flowBanSoDo";
            flowBanSoDo.Padding = new Padding(0, 4, 0, 0);
            flowBanSoDo.Size = new Size(735, 255);
            flowBanSoDo.TabIndex = 1;
            // 
            // lblFloorTitle
            // 
            lblFloorTitle.AutoSize = true;
            lblFloorTitle.Dock = DockStyle.Top;
            lblFloorTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblFloorTitle.Location = new Point(14, 14);
            lblFloorTitle.Name = "lblFloorTitle";
            lblFloorTitle.Size = new Size(207, 25);
            lblFloorTitle.TabIndex = 0;
            lblFloorTitle.Text = "Sơ đồ bàn theo khu vực";
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.White;
            panelBottom.Controls.Add(dgvDanhSachBan);
            panelBottom.Controls.Add(lblDanhSachBanTitle);
            panelBottom.Dock = DockStyle.Fill;
            panelBottom.Location = new Point(0, 450);
            panelBottom.Margin = new Padding(0);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new Padding(12);
            panelBottom.Size = new Size(1090, 192);
            panelBottom.TabIndex = 2;
            // 
            // dgvDanhSachBan
            // 
            dgvDanhSachBan.AllowUserToAddRows = false;
            dgvDanhSachBan.AllowUserToDeleteRows = false;
            dgvDanhSachBan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhSachBan.BackgroundColor = Color.White;
            dgvDanhSachBan.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDanhSachBan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDanhSachBan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDanhSachBan.Columns.AddRange(new DataGridViewColumn[] { colMaBan, colTenBan, colKhuVuc, colSucChua, colTinhTrang });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDanhSachBan.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDanhSachBan.Dock = DockStyle.Fill;
            dgvDanhSachBan.GridColor = Color.Gainsboro;
            dgvDanhSachBan.Location = new Point(12, 37);
            dgvDanhSachBan.MultiSelect = false;
            dgvDanhSachBan.Name = "dgvDanhSachBan";
            dgvDanhSachBan.ReadOnly = true;
            dgvDanhSachBan.RowHeadersVisible = false;
            dgvDanhSachBan.RowHeadersWidth = 51;
            dgvDanhSachBan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhSachBan.Size = new Size(1066, 143);
            dgvDanhSachBan.TabIndex = 1;
            // 
            // colMaBan
            // 
            colMaBan.HeaderText = "Mã bàn";
            colMaBan.MinimumWidth = 6;
            colMaBan.Name = "colMaBan";
            colMaBan.ReadOnly = true;
            // 
            // colTenBan
            // 
            colTenBan.HeaderText = "Tên bàn";
            colTenBan.MinimumWidth = 6;
            colTenBan.Name = "colTenBan";
            colTenBan.ReadOnly = true;
            // 
            // colKhuVuc
            // 
            colKhuVuc.HeaderText = "Khu vực";
            colKhuVuc.MinimumWidth = 6;
            colKhuVuc.Name = "colKhuVuc";
            colKhuVuc.ReadOnly = true;
            // 
            // colSucChua
            // 
            colSucChua.HeaderText = "Sức chứa";
            colSucChua.MinimumWidth = 6;
            colSucChua.Name = "colSucChua";
            colSucChua.ReadOnly = true;
            // 
            // colTinhTrang
            // 
            colTinhTrang.HeaderText = "Tình trạng";
            colTinhTrang.MinimumWidth = 6;
            colTinhTrang.Name = "colTinhTrang";
            colTinhTrang.ReadOnly = true;
            // 
            // lblDanhSachBanTitle
            // 
            lblDanhSachBanTitle.AutoSize = true;
            lblDanhSachBanTitle.Dock = DockStyle.Top;
            lblDanhSachBanTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblDanhSachBanTitle.Location = new Point(12, 12);
            lblDanhSachBanTitle.Name = "lblDanhSachBanTitle";
            lblDanhSachBanTitle.Size = new Size(193, 25);
            lblDanhSachBanTitle.TabIndex = 0;
            lblDanhSachBanTitle.Text = "Danh sách bàn chi tiết";
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
            txtSearch.PlaceholderText = "🔍  Tìm bàn theo mã, khu vực, trạng thái...";
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
            lblPageTitle.Size = new Size(122, 28);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Quản lý bàn";
            // 
            // frmQuanLiBan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 242, 236);
            ClientSize = new Size(1364, 760);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1220, 720);
            Name = "frmQuanLiBan";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Coffee Shop - Quản lý bàn";
            panelSidebar.ResumeLayout(false);
            flowSidebarMenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelMain.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            tableMain.ResumeLayout(false);
            tableStats.ResumeLayout(false);
            cardTongBan.ResumeLayout(false);
            cardTongBan.PerformLayout();
            cardDangPhucVu.ResumeLayout(false);
            cardDangPhucVu.PerformLayout();
            cardBanTrong.ResumeLayout(false);
            cardBanTrong.PerformLayout();
            cardDatTruoc.ResumeLayout(false);
            cardDatTruoc.PerformLayout();
            tableCenter.ResumeLayout(false);
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            panelFloor.ResumeLayout(false);
            panelFloor.PerformLayout();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachBan).EndInit();
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
        private Panel cardTongBan;
        private Label lblTongBanValue;
        private Label lblTongBanTitle;
        private Label lblTongBanIcon;
        private Panel cardDangPhucVu;
        private Label lblDangPhucVuValue;
        private Label lblDangPhucVuTitle;
        private Label lblDangPhucVuIcon;
        private Panel cardBanTrong;
        private Label lblBanTrongValue;
        private Label lblBanTrongTitle;
        private Label lblBanTrongIcon;
        private Panel cardDatTruoc;
        private Label lblDatTruocValue;
        private Label lblDatTruocTitle;
        private Label lblDatTruocIcon;
        private TableLayoutPanel tableCenter;
        private Panel panelFilter;
        private Button btnLamMoi;
        private Button btnGopBan;
        private Button btnXoaBan;
        private Button btnThemBan;
        private ComboBox cboTrangThai;
        private Label lblTrangThai;
        private ComboBox cboKhuVuc;
        private Label lblKhuVuc;
        private Label lblBoLocTitle;
        private Panel panelFloor;
        private FlowLayoutPanel flowBanSoDo;
        private Label lblFloorTitle;
        private Panel panelBottom;
        private DataGridView dgvDanhSachBan;
        private DataGridViewTextBoxColumn colMaBan;
        private DataGridViewTextBoxColumn colTenBan;
        private DataGridViewTextBoxColumn colKhuVuc;
        private DataGridViewTextBoxColumn colSucChua;
        private DataGridViewTextBoxColumn colTinhTrang;
        private Label lblDanhSachBanTitle;
        private Panel panelTopbar;
        private Button btnUserMenu;
        private Label lblUserName;
        private PictureBox picAvatar;
        private TextBox txtSearch;
        private Label lblPageTitle;
    }
}