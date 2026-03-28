namespace QuanLyQuanCaPhe.Forms
{
    partial class frmKhachHang
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
            btnKhachHang = new Button();
            btnNhanVien = new Button();
            btnThongKe = new Button();
            panelLogo = new Panel();
            lblLogoSub = new Label();
            lblLogo = new Label();
            panelMain = new Panel();
            panelContent = new Panel();
            tableMain = new TableLayoutPanel();
            tableStats = new TableLayoutPanel();
            cardTongKhach = new Panel();
            lblTongKhachValue = new Label();
            lblTongKhachTitle = new Label();
            lblTongKhachIcon = new Label();
            cardCoSoDienThoai = new Panel();
            lblCoSoDienThoaiValue = new Label();
            lblCoSoDienThoaiTitle = new Label();
            lblCoSoDienThoaiIcon = new Label();
            cardCoDiaChi = new Panel();
            lblCoDiaChiValue = new Label();
            lblCoDiaChiTitle = new Label();
            lblCoDiaChiIcon = new Label();
            tableCenter = new TableLayoutPanel();
            panelThongTinKhach = new Panel();
            btnLamMoi = new Button();
            btnXoaKhach = new Button();
            btnCapNhatKhach = new Button();
            btnThemKhach = new Button();
            btnTimKhach = new Button();
            txtTimKhach = new TextBox();
            lblTimKhach = new Label();
            txtDiaChi = new TextBox();
            lblDiaChi = new Label();
            txtDienThoai = new TextBox();
            lblDienThoai = new Label();
            txtHoVaTen = new TextBox();
            lblHoVaTen = new Label();
            txtMaKhach = new TextBox();
            lblMaKhach = new Label();
            lblThongTinKhachTitle = new Label();
            panelDanhSachKhach = new Panel();
            dgvDanhSachKhach = new DataGridView();
            colIDKhach = new DataGridViewTextBoxColumn();
            colHoVaTen = new DataGridViewTextBoxColumn();
            colDienThoai = new DataGridViewTextBoxColumn();
            colDiaChi = new DataGridViewTextBoxColumn();
            panelDanhSachHeader = new Panel();
            btnNhapKhach = new Button();
            btnXuatKhach = new Button();
            lblDanhSachKhachTitle = new Label();
            panelTopbar = new Panel();
            btnUserMenu = new Button();
            lblUserName = new Label();
            picAvatar = new PictureBox();
            lblPageTitle = new Label();
            panelSidebar.SuspendLayout();
            flowSidebarMenu.SuspendLayout();
            panelLogo.SuspendLayout();
            panelMain.SuspendLayout();
            panelContent.SuspendLayout();
            tableMain.SuspendLayout();
            tableStats.SuspendLayout();
            cardTongKhach.SuspendLayout();
            cardCoSoDienThoai.SuspendLayout();
            cardCoDiaChi.SuspendLayout();
            tableCenter.SuspendLayout();
            panelThongTinKhach.SuspendLayout();
            panelDanhSachKhach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachKhach).BeginInit();
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
            flowSidebarMenu.Controls.Add(btnKhachHang);
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
            // btnKhachHang
            // 
            btnKhachHang.BackColor = Color.FromArgb(94, 64, 47);
            btnKhachHang.FlatAppearance.BorderSize = 0;
            btnKhachHang.FlatStyle = FlatStyle.Flat;
            btnKhachHang.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnKhachHang.ForeColor = Color.White;
            btnKhachHang.Location = new Point(0, 254);
            btnKhachHang.Margin = new Padding(0);
            btnKhachHang.Name = "btnKhachHang";
            btnKhachHang.Padding = new Padding(20, 0, 0, 0);
            btnKhachHang.Size = new Size(230, 48);
            btnKhachHang.TabIndex = 5;
            btnKhachHang.Text = "👤  Khách hàng";
            btnKhachHang.TextAlign = ContentAlignment.MiddleLeft;
            btnKhachHang.UseVisualStyleBackColor = false;
            // 
            // btnThongKe
            // 
            btnNhanVien.FlatAppearance.BorderSize = 0;
            btnNhanVien.FlatStyle = FlatStyle.Flat;
            btnNhanVien.Font = new Font("Segoe UI", 10F);
            btnNhanVien.ForeColor = Color.Gainsboro;
            btnNhanVien.Location = new Point(0, 302);
            btnNhanVien.Margin = new Padding(0);
            btnNhanVien.Name = "btnNhanVien";
            btnNhanVien.Padding = new Padding(20, 0, 0, 0);
            btnNhanVien.Size = new Size(230, 48);
            btnNhanVien.TabIndex = 6;
            btnNhanVien.Text = "🧑‍💼  Nhân viên";
            btnNhanVien.TextAlign = ContentAlignment.MiddleLeft;
            btnNhanVien.UseVisualStyleBackColor = true;
            // 
            // btnThongKe
            // 
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.FlatStyle = FlatStyle.Flat;
            btnThongKe.Font = new Font("Segoe UI", 10F);
            btnThongKe.ForeColor = Color.Gainsboro;
            btnThongKe.Location = new Point(0, 350);
            btnThongKe.Margin = new Padding(0);
            btnThongKe.Name = "btnThongKe";
            btnThongKe.Padding = new Padding(20, 0, 0, 0);
            btnThongKe.Size = new Size(230, 48);
            btnThongKe.TabIndex = 7;
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
            tableStats.Controls.Add(cardTongKhach, 0, 0);
            tableStats.Controls.Add(cardCoSoDienThoai, 1, 0);
            tableStats.Controls.Add(cardCoDiaChi, 2, 0);
            tableStats.Dock = DockStyle.Fill;
            tableStats.Location = new Point(0, 0);
            tableStats.Margin = new Padding(0, 0, 0, 12);
            tableStats.Name = "tableStats";
            tableStats.RowCount = 1;
            tableStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableStats.Size = new Size(1090, 118);
            tableStats.TabIndex = 0;
            // 
            // cardTongKhach
            // 
            cardTongKhach.BackColor = Color.FromArgb(237, 247, 243);
            cardTongKhach.Controls.Add(lblTongKhachValue);
            cardTongKhach.Controls.Add(lblTongKhachTitle);
            cardTongKhach.Controls.Add(lblTongKhachIcon);
            cardTongKhach.Dock = DockStyle.Fill;
            cardTongKhach.Location = new Point(0, 0);
            cardTongKhach.Margin = new Padding(0, 0, 8, 0);
            cardTongKhach.Name = "cardTongKhach";
            cardTongKhach.Padding = new Padding(16, 12, 16, 12);
            cardTongKhach.Size = new Size(355, 118);
            cardTongKhach.TabIndex = 0;
            // 
            // lblTongKhachValue
            // 
            lblTongKhachValue.AutoSize = true;
            lblTongKhachValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTongKhachValue.ForeColor = Color.FromArgb(34, 111, 92);
            lblTongKhachValue.Location = new Point(18, 61);
            lblTongKhachValue.Name = "lblTongKhachValue";
            lblTongKhachValue.Size = new Size(28, 37);
            lblTongKhachValue.TabIndex = 2;
            lblTongKhachValue.Text = "-";
            // 
            // lblTongKhachTitle
            // 
            lblTongKhachTitle.AutoSize = true;
            lblTongKhachTitle.Font = new Font("Segoe UI", 10F);
            lblTongKhachTitle.ForeColor = Color.FromArgb(90, 106, 101);
            lblTongKhachTitle.Location = new Point(18, 36);
            lblTongKhachTitle.Name = "lblTongKhachTitle";
            lblTongKhachTitle.Size = new Size(143, 23);
            lblTongKhachTitle.TabIndex = 1;
            lblTongKhachTitle.Text = "Tổng khách hàng";
            // 
            // lblTongKhachIcon
            // 
            lblTongKhachIcon.AutoSize = true;
            lblTongKhachIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblTongKhachIcon.Location = new Point(16, 8);
            lblTongKhachIcon.Name = "lblTongKhachIcon";
            lblTongKhachIcon.Size = new Size(39, 27);
            lblTongKhachIcon.TabIndex = 0;
            lblTongKhachIcon.Text = "👥";
            // 
            // cardCoSoDienThoai
            // 
            cardCoSoDienThoai.BackColor = Color.FromArgb(255, 247, 235);
            cardCoSoDienThoai.Controls.Add(lblCoSoDienThoaiValue);
            cardCoSoDienThoai.Controls.Add(lblCoSoDienThoaiTitle);
            cardCoSoDienThoai.Controls.Add(lblCoSoDienThoaiIcon);
            cardCoSoDienThoai.Dock = DockStyle.Fill;
            cardCoSoDienThoai.Location = new Point(363, 0);
            cardCoSoDienThoai.Margin = new Padding(0, 0, 8, 0);
            cardCoSoDienThoai.Name = "cardCoSoDienThoai";
            cardCoSoDienThoai.Padding = new Padding(16, 12, 16, 12);
            cardCoSoDienThoai.Size = new Size(355, 118);
            cardCoSoDienThoai.TabIndex = 1;
            // 
            // lblCoSoDienThoaiValue
            // 
            lblCoSoDienThoaiValue.AutoSize = true;
            lblCoSoDienThoaiValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblCoSoDienThoaiValue.ForeColor = Color.FromArgb(158, 104, 51);
            lblCoSoDienThoaiValue.Location = new Point(18, 61);
            lblCoSoDienThoaiValue.Name = "lblCoSoDienThoaiValue";
            lblCoSoDienThoaiValue.Size = new Size(28, 37);
            lblCoSoDienThoaiValue.TabIndex = 2;
            lblCoSoDienThoaiValue.Text = "-";
            // 
            // lblCoSoDienThoaiTitle
            // 
            lblCoSoDienThoaiTitle.AutoSize = true;
            lblCoSoDienThoaiTitle.Font = new Font("Segoe UI", 10F);
            lblCoSoDienThoaiTitle.ForeColor = Color.FromArgb(146, 117, 84);
            lblCoSoDienThoaiTitle.Location = new Point(18, 36);
            lblCoSoDienThoaiTitle.Name = "lblCoSoDienThoaiTitle";
            lblCoSoDienThoaiTitle.Size = new Size(113, 23);
            lblCoSoDienThoaiTitle.TabIndex = 1;
            lblCoSoDienThoaiTitle.Text = "Có điện thoại";
            // 
            // lblCoSoDienThoaiIcon
            // 
            lblCoSoDienThoaiIcon.AutoSize = true;
            lblCoSoDienThoaiIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblCoSoDienThoaiIcon.Location = new Point(16, 8);
            lblCoSoDienThoaiIcon.Name = "lblCoSoDienThoaiIcon";
            lblCoSoDienThoaiIcon.Size = new Size(39, 27);
            lblCoSoDienThoaiIcon.TabIndex = 0;
            lblCoSoDienThoaiIcon.Text = "📞";
            // 
            // cardCoDiaChi
            // 
            cardCoDiaChi.BackColor = Color.FromArgb(237, 240, 255);
            cardCoDiaChi.Controls.Add(lblCoDiaChiValue);
            cardCoDiaChi.Controls.Add(lblCoDiaChiTitle);
            cardCoDiaChi.Controls.Add(lblCoDiaChiIcon);
            cardCoDiaChi.Dock = DockStyle.Fill;
            cardCoDiaChi.Location = new Point(726, 0);
            cardCoDiaChi.Margin = new Padding(0);
            cardCoDiaChi.Name = "cardCoDiaChi";
            cardCoDiaChi.Padding = new Padding(16, 12, 16, 12);
            cardCoDiaChi.Size = new Size(364, 118);
            cardCoDiaChi.TabIndex = 2;
            // 
            // lblCoDiaChiValue
            // 
            lblCoDiaChiValue.AutoSize = true;
            lblCoDiaChiValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblCoDiaChiValue.ForeColor = Color.FromArgb(61, 75, 160);
            lblCoDiaChiValue.Location = new Point(18, 61);
            lblCoDiaChiValue.Name = "lblCoDiaChiValue";
            lblCoDiaChiValue.Size = new Size(28, 37);
            lblCoDiaChiValue.TabIndex = 2;
            lblCoDiaChiValue.Text = "-";
            // 
            // lblCoDiaChiTitle
            // 
            lblCoDiaChiTitle.AutoSize = true;
            lblCoDiaChiTitle.Font = new Font("Segoe UI", 10F);
            lblCoDiaChiTitle.ForeColor = Color.FromArgb(85, 97, 170);
            lblCoDiaChiTitle.Location = new Point(18, 36);
            lblCoDiaChiTitle.Name = "lblCoDiaChiTitle";
            lblCoDiaChiTitle.Size = new Size(86, 23);
            lblCoDiaChiTitle.TabIndex = 1;
            lblCoDiaChiTitle.Text = "Có địa chỉ";
            // 
            // lblCoDiaChiIcon
            // 
            lblCoDiaChiIcon.AutoSize = true;
            lblCoDiaChiIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblCoDiaChiIcon.Location = new Point(16, 8);
            lblCoDiaChiIcon.Name = "lblCoDiaChiIcon";
            lblCoDiaChiIcon.Size = new Size(39, 27);
            lblCoDiaChiIcon.TabIndex = 0;
            lblCoDiaChiIcon.Text = "📍";
            // 
            // tableCenter
            // 
            tableCenter.ColumnCount = 2;
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            tableCenter.Controls.Add(panelThongTinKhach, 0, 0);
            tableCenter.Controls.Add(panelDanhSachKhach, 1, 0);
            tableCenter.Dock = DockStyle.Fill;
            tableCenter.Location = new Point(0, 130);
            tableCenter.Margin = new Padding(0);
            tableCenter.Name = "tableCenter";
            tableCenter.RowCount = 1;
            tableCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableCenter.Size = new Size(1090, 512);
            tableCenter.TabIndex = 1;
            // 
            // panelThongTinKhach
            // 
            panelThongTinKhach.BackColor = Color.White;
            panelThongTinKhach.Controls.Add(btnLamMoi);
            panelThongTinKhach.Controls.Add(btnXoaKhach);
            panelThongTinKhach.Controls.Add(btnCapNhatKhach);
            panelThongTinKhach.Controls.Add(btnThemKhach);
            panelThongTinKhach.Controls.Add(btnTimKhach);
            panelThongTinKhach.Controls.Add(txtTimKhach);
            panelThongTinKhach.Controls.Add(lblTimKhach);
            panelThongTinKhach.Controls.Add(txtDiaChi);
            panelThongTinKhach.Controls.Add(lblDiaChi);
            panelThongTinKhach.Controls.Add(txtDienThoai);
            panelThongTinKhach.Controls.Add(lblDienThoai);
            panelThongTinKhach.Controls.Add(txtHoVaTen);
            panelThongTinKhach.Controls.Add(lblHoVaTen);
            panelThongTinKhach.Controls.Add(txtMaKhach);
            panelThongTinKhach.Controls.Add(lblMaKhach);
            panelThongTinKhach.Controls.Add(lblThongTinKhachTitle);
            panelThongTinKhach.Dock = DockStyle.Fill;
            panelThongTinKhach.Location = new Point(0, 0);
            panelThongTinKhach.Margin = new Padding(0, 0, 10, 0);
            panelThongTinKhach.Name = "panelThongTinKhach";
            panelThongTinKhach.Padding = new Padding(16, 14, 16, 14);
            panelThongTinKhach.Size = new Size(338, 512);
            panelThongTinKhach.TabIndex = 0;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(94, 64, 47);
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(176, 431);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(146, 38);
            btnLamMoi.TabIndex = 14;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnXoaKhach
            // 
            btnXoaKhach.BackColor = Color.FromArgb(254, 241, 241);
            btnXoaKhach.FlatAppearance.BorderSize = 0;
            btnXoaKhach.FlatStyle = FlatStyle.Flat;
            btnXoaKhach.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnXoaKhach.ForeColor = Color.FromArgb(162, 52, 64);
            btnXoaKhach.Location = new Point(16, 431);
            btnXoaKhach.Name = "btnXoaKhach";
            btnXoaKhach.Size = new Size(146, 38);
            btnXoaKhach.TabIndex = 13;
            btnXoaKhach.Text = "Xóa khách hàng";
            btnXoaKhach.UseVisualStyleBackColor = false;
            btnXoaKhach.Click += btnXoaKhach_Click;
            // 
            // btnCapNhatKhach
            // 
            btnCapNhatKhach.BackColor = Color.FromArgb(246, 241, 255);
            btnCapNhatKhach.FlatAppearance.BorderSize = 0;
            btnCapNhatKhach.FlatStyle = FlatStyle.Flat;
            btnCapNhatKhach.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnCapNhatKhach.ForeColor = Color.FromArgb(105, 72, 168);
            btnCapNhatKhach.Location = new Point(176, 385);
            btnCapNhatKhach.Name = "btnCapNhatKhach";
            btnCapNhatKhach.Size = new Size(146, 38);
            btnCapNhatKhach.TabIndex = 12;
            btnCapNhatKhach.Text = "Cập nhật khách hàng";
            btnCapNhatKhach.UseVisualStyleBackColor = false;
            btnCapNhatKhach.Click += btnCapNhatKhach_Click;
            // 
            // btnThemKhach
            // 
            btnThemKhach.BackColor = Color.FromArgb(236, 245, 241);
            btnThemKhach.FlatAppearance.BorderSize = 0;
            btnThemKhach.FlatStyle = FlatStyle.Flat;
            btnThemKhach.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnThemKhach.ForeColor = Color.FromArgb(34, 111, 92);
            btnThemKhach.Location = new Point(16, 385);
            btnThemKhach.Name = "btnThemKhach";
            btnThemKhach.Size = new Size(146, 38);
            btnThemKhach.TabIndex = 11;
            btnThemKhach.Text = "+ Thêm khách hàng";
            btnThemKhach.UseVisualStyleBackColor = false;
            btnThemKhach.Click += btnThemKhach_Click;
            // 
            // btnTimKhach
            // 
            btnTimKhach.BackColor = Color.FromArgb(248, 245, 241);
            btnTimKhach.FlatAppearance.BorderSize = 0;
            btnTimKhach.FlatStyle = FlatStyle.Flat;
            btnTimKhach.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            btnTimKhach.ForeColor = Color.FromArgb(65, 48, 39);
            btnTimKhach.Location = new Point(239, 339);
            btnTimKhach.Name = "btnTimKhach";
            btnTimKhach.Size = new Size(83, 29);
            btnTimKhach.TabIndex = 15;
            btnTimKhach.Text = "Tìm";
            btnTimKhach.UseVisualStyleBackColor = false;
            btnTimKhach.Click += btnTimKhach_Click;
            // 
            // txtTimKhach
            // 
            txtTimKhach.BackColor = Color.FromArgb(248, 245, 241);
            txtTimKhach.BorderStyle = BorderStyle.FixedSingle;
            txtTimKhach.Font = new Font("Segoe UI", 9.5F);
            txtTimKhach.Location = new Point(16, 339);
            txtTimKhach.Name = "txtTimKhach";
            txtTimKhach.PlaceholderText = "Nhập ID, tên hoặc số điện thoại...";
            txtTimKhach.Size = new Size(217, 29);
            txtTimKhach.TabIndex = 10;
            // 
            // lblTimKhach
            // 
            lblTimKhach.AutoSize = true;
            lblTimKhach.Font = new Font("Segoe UI", 9.5F);
            lblTimKhach.ForeColor = Color.DimGray;
            lblTimKhach.Location = new Point(16, 313);
            lblTimKhach.Name = "lblTimKhach";
            lblTimKhach.Size = new Size(122, 21);
            lblTimKhach.TabIndex = 9;
            lblTimKhach.Text = "Tìm kiếm nhanh";
            // 
            // txtDiaChi
            // 
            txtDiaChi.BackColor = Color.FromArgb(248, 245, 241);
            txtDiaChi.BorderStyle = BorderStyle.FixedSingle;
            txtDiaChi.Font = new Font("Segoe UI", 9.5F);
            txtDiaChi.Location = new Point(16, 243);
            txtDiaChi.Multiline = true;
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.PlaceholderText = "Nhập địa chỉ khách hàng...";
            txtDiaChi.Size = new Size(306, 64);
            txtDiaChi.TabIndex = 8;
            // 
            // lblDiaChi
            // 
            lblDiaChi.AutoSize = true;
            lblDiaChi.Font = new Font("Segoe UI", 9.5F);
            lblDiaChi.ForeColor = Color.DimGray;
            lblDiaChi.Location = new Point(16, 217);
            lblDiaChi.Name = "lblDiaChi";
            lblDiaChi.Size = new Size(57, 21);
            lblDiaChi.TabIndex = 7;
            lblDiaChi.Text = "Địa chỉ";
            // 
            // txtDienThoai
            // 
            txtDienThoai.BackColor = Color.FromArgb(248, 245, 241);
            txtDienThoai.BorderStyle = BorderStyle.FixedSingle;
            txtDienThoai.Font = new Font("Segoe UI", 9.5F);
            txtDienThoai.Location = new Point(16, 186);
            txtDienThoai.Name = "txtDienThoai";
            txtDienThoai.PlaceholderText = "Nhập số điện thoại...";
            txtDienThoai.Size = new Size(306, 29);
            txtDienThoai.TabIndex = 6;
            // 
            // lblDienThoai
            // 
            lblDienThoai.AutoSize = true;
            lblDienThoai.Font = new Font("Segoe UI", 9.5F);
            lblDienThoai.ForeColor = Color.DimGray;
            lblDienThoai.Location = new Point(16, 160);
            lblDienThoai.Name = "lblDienThoai";
            lblDienThoai.Size = new Size(81, 21);
            lblDienThoai.TabIndex = 5;
            lblDienThoai.Text = "Điện thoại";
            // 
            // txtHoVaTen
            // 
            txtHoVaTen.BackColor = Color.FromArgb(248, 245, 241);
            txtHoVaTen.BorderStyle = BorderStyle.FixedSingle;
            txtHoVaTen.Font = new Font("Segoe UI", 9.5F);
            txtHoVaTen.Location = new Point(16, 129);
            txtHoVaTen.Name = "txtHoVaTen";
            txtHoVaTen.PlaceholderText = "Nhập họ và tên khách hàng...";
            txtHoVaTen.Size = new Size(306, 29);
            txtHoVaTen.TabIndex = 4;
            // 
            // lblHoVaTen
            // 
            lblHoVaTen.AutoSize = true;
            lblHoVaTen.Font = new Font("Segoe UI", 9.5F);
            lblHoVaTen.ForeColor = Color.DimGray;
            lblHoVaTen.Location = new Point(16, 103);
            lblHoVaTen.Name = "lblHoVaTen";
            lblHoVaTen.Size = new Size(76, 21);
            lblHoVaTen.TabIndex = 3;
            lblHoVaTen.Text = "Họ và tên";
            // 
            // txtMaKhach
            // 
            txtMaKhach.BackColor = Color.FromArgb(248, 245, 241);
            txtMaKhach.BorderStyle = BorderStyle.FixedSingle;
            txtMaKhach.Font = new Font("Segoe UI", 9.5F);
            txtMaKhach.Location = new Point(16, 72);
            txtMaKhach.Name = "txtMaKhach";
            txtMaKhach.PlaceholderText = "Tự động tăng";
            txtMaKhach.ReadOnly = true;
            txtMaKhach.Size = new Size(306, 29);
            txtMaKhach.TabIndex = 2;
            // 
            // lblMaKhach
            // 
            lblMaKhach.AutoSize = true;
            lblMaKhach.Font = new Font("Segoe UI", 9.5F);
            lblMaKhach.ForeColor = Color.DimGray;
            lblMaKhach.Location = new Point(16, 46);
            lblMaKhach.Name = "lblMaKhach";
            lblMaKhach.Size = new Size(70, 21);
            lblMaKhach.TabIndex = 1;
            lblMaKhach.Text = "ID khách";
            // 
            // lblThongTinKhachTitle
            // 
            lblThongTinKhachTitle.AutoSize = true;
            lblThongTinKhachTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblThongTinKhachTitle.Location = new Point(16, 12);
            lblThongTinKhachTitle.Name = "lblThongTinKhachTitle";
            lblThongTinKhachTitle.Size = new Size(190, 25);
            lblThongTinKhachTitle.TabIndex = 0;
            lblThongTinKhachTitle.Text = "Thông tin khách hàng";
            // 
            // panelDanhSachKhach
            // 
            panelDanhSachKhach.BackColor = Color.White;
            panelDanhSachKhach.Controls.Add(dgvDanhSachKhach);
            panelDanhSachKhach.Controls.Add(panelDanhSachHeader);
            panelDanhSachKhach.Dock = DockStyle.Fill;
            panelDanhSachKhach.Location = new Point(348, 0);
            panelDanhSachKhach.Margin = new Padding(0);
            panelDanhSachKhach.Name = "panelDanhSachKhach";
            panelDanhSachKhach.Padding = new Padding(12);
            panelDanhSachKhach.Size = new Size(742, 512);
            panelDanhSachKhach.TabIndex = 1;
            // 
            // dgvDanhSachKhach
            // 
            dgvDanhSachKhach.AllowUserToAddRows = false;
            dgvDanhSachKhach.AllowUserToDeleteRows = false;
            dgvDanhSachKhach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhSachKhach.BackgroundColor = Color.White;
            dgvDanhSachKhach.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 237, 227);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(70, 53, 40);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(245, 237, 227);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(70, 53, 40);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDanhSachKhach.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDanhSachKhach.ColumnHeadersHeight = 42;
            dgvDanhSachKhach.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDanhSachKhach.Columns.AddRange(new DataGridViewColumn[] { colIDKhach, colHoVaTen, colDienThoai, colDiaChi });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(248, 240, 232);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(63, 45, 35);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDanhSachKhach.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDanhSachKhach.Dock = DockStyle.Fill;
            dgvDanhSachKhach.EnableHeadersVisualStyles = false;
            dgvDanhSachKhach.GridColor = Color.FromArgb(238, 230, 220);
            dgvDanhSachKhach.Location = new Point(12, 55);
            dgvDanhSachKhach.MultiSelect = false;
            dgvDanhSachKhach.Name = "dgvDanhSachKhach";
            dgvDanhSachKhach.ReadOnly = true;
            dgvDanhSachKhach.RowHeadersVisible = false;
            dgvDanhSachKhach.RowHeadersWidth = 51;
            dgvDanhSachKhach.RowTemplate.Height = 32;
            dgvDanhSachKhach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhSachKhach.Size = new Size(718, 445);
            dgvDanhSachKhach.TabIndex = 1;
            // 
            // colIDKhach
            // 
            colIDKhach.DataPropertyName = "ID";
            colIDKhach.FillWeight = 20F;
            colIDKhach.HeaderText = "Mã";
            colIDKhach.MinimumWidth = 6;
            colIDKhach.Name = "colIDKhach";
            colIDKhach.ReadOnly = true;
            // 
            // colHoVaTen
            // 
            colHoVaTen.DataPropertyName = "HoVaTen";
            colHoVaTen.FillWeight = 42F;
            colHoVaTen.HeaderText = "Họ và tên";
            colHoVaTen.MinimumWidth = 6;
            colHoVaTen.Name = "colHoVaTen";
            colHoVaTen.ReadOnly = true;
            // 
            // colDienThoai
            // 
            colDienThoai.DataPropertyName = "DienThoai";
            colDienThoai.FillWeight = 25F;
            colDienThoai.HeaderText = "Điện thoại";
            colDienThoai.MinimumWidth = 6;
            colDienThoai.Name = "colDienThoai";
            colDienThoai.ReadOnly = true;
            // 
            // colDiaChi
            // 
            colDiaChi.DataPropertyName = "DiaChi";
            colDiaChi.FillWeight = 55F;
            colDiaChi.HeaderText = "Địa chỉ";
            colDiaChi.MinimumWidth = 6;
            colDiaChi.Name = "colDiaChi";
            colDiaChi.ReadOnly = true;
            // 
            // panelDanhSachHeader
            // 
            panelDanhSachHeader.Controls.Add(btnNhapKhach);
            panelDanhSachHeader.Controls.Add(btnXuatKhach);
            panelDanhSachHeader.Controls.Add(lblDanhSachKhachTitle);
            panelDanhSachHeader.Dock = DockStyle.Top;
            panelDanhSachHeader.Location = new Point(12, 12);
            panelDanhSachHeader.Name = "panelDanhSachHeader";
            panelDanhSachHeader.Size = new Size(718, 43);
            panelDanhSachHeader.TabIndex = 0;
            // 
            // btnNhapKhach
            // 
            btnNhapKhach.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNhapKhach.BackColor = Color.FromArgb(248, 245, 241);
            btnNhapKhach.FlatAppearance.BorderSize = 0;
            btnNhapKhach.FlatStyle = FlatStyle.Flat;
            btnNhapKhach.Font = new Font("Segoe UI", 8.5F);
            btnNhapKhach.ForeColor = Color.FromArgb(65, 48, 39);
            btnNhapKhach.Location = new Point(536, 4);
            btnNhapKhach.Name = "btnNhapKhach";
            btnNhapKhach.Size = new Size(84, 32);
            btnNhapKhach.TabIndex = 2;
            btnNhapKhach.Text = "Nhập";
            btnNhapKhach.UseVisualStyleBackColor = false;
            btnNhapKhach.Click += btnNhapKhach_Click;
            // 
            // btnXuatKhach
            // 
            btnXuatKhach.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXuatKhach.BackColor = Color.FromArgb(248, 245, 241);
            btnXuatKhach.FlatAppearance.BorderSize = 0;
            btnXuatKhach.FlatStyle = FlatStyle.Flat;
            btnXuatKhach.Font = new Font("Segoe UI", 8.5F);
            btnXuatKhach.ForeColor = Color.FromArgb(65, 48, 39);
            btnXuatKhach.Location = new Point(626, 4);
            btnXuatKhach.Name = "btnXuatKhach";
            btnXuatKhach.Size = new Size(84, 32);
            btnXuatKhach.TabIndex = 1;
            btnXuatKhach.Text = "Xuất";
            btnXuatKhach.UseVisualStyleBackColor = false;
            btnXuatKhach.Click += btnXuatKhach_Click;
            // 
            // lblDanhSachKhachTitle
            // 
            lblDanhSachKhachTitle.AutoSize = true;
            lblDanhSachKhachTitle.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            lblDanhSachKhachTitle.Location = new Point(0, 8);
            lblDanhSachKhachTitle.Name = "lblDanhSachKhachTitle";
            lblDanhSachKhachTitle.Size = new Size(192, 25);
            lblDanhSachKhachTitle.TabIndex = 0;
            lblDanhSachKhachTitle.Text = "Danh sách khách hàng";
            // 
            // panelTopbar
            // 
            panelTopbar.BackColor = Color.White;
            panelTopbar.Controls.Add(btnUserMenu);
            panelTopbar.Controls.Add(lblUserName);
            panelTopbar.Controls.Add(picAvatar);
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
            lblUserName.Text = "Trần Minh Quân";
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
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(65, 48, 39);
            lblPageTitle.Location = new Point(22, 23);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(194, 28);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Quản lý khách hàng";
            // 
            // frmKhachHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 242, 236);
            ClientSize = new Size(1364, 760);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1220, 720);
            Name = "frmKhachHang";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Coffee Shop - Khách hàng";
            panelSidebar.ResumeLayout(false);
            flowSidebarMenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelMain.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            tableMain.ResumeLayout(false);
            tableStats.ResumeLayout(false);
            cardTongKhach.ResumeLayout(false);
            cardTongKhach.PerformLayout();
            cardCoSoDienThoai.ResumeLayout(false);
            cardCoSoDienThoai.PerformLayout();
            cardCoDiaChi.ResumeLayout(false);
            cardCoDiaChi.PerformLayout();
            tableCenter.ResumeLayout(false);
            panelThongTinKhach.ResumeLayout(false);
            panelThongTinKhach.PerformLayout();
            panelDanhSachKhach.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachKhach).EndInit();
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
        private Button btnKhachHang;
        private Button btnNhanVien;
        private Button btnThongKe;
        private Panel panelLogo;
        private Label lblLogoSub;
        private Label lblLogo;
        private Panel panelMain;
        private Panel panelContent;
        private TableLayoutPanel tableMain;
        private TableLayoutPanel tableStats;
        private Panel cardTongKhach;
        private Label lblTongKhachValue;
        private Label lblTongKhachTitle;
        private Label lblTongKhachIcon;
        private Panel cardCoSoDienThoai;
        private Label lblCoSoDienThoaiValue;
        private Label lblCoSoDienThoaiTitle;
        private Label lblCoSoDienThoaiIcon;
        private Panel cardCoDiaChi;
        private Label lblCoDiaChiValue;
        private Label lblCoDiaChiTitle;
        private Label lblCoDiaChiIcon;
        private TableLayoutPanel tableCenter;
        private Panel panelThongTinKhach;
        private Button btnLamMoi;
        private Button btnXoaKhach;
        private Button btnCapNhatKhach;
        private Button btnThemKhach;
        private Button btnTimKhach;
        private TextBox txtTimKhach;
        private Label lblTimKhach;
        private TextBox txtDiaChi;
        private Label lblDiaChi;
        private TextBox txtDienThoai;
        private Label lblDienThoai;
        private TextBox txtHoVaTen;
        private Label lblHoVaTen;
        private TextBox txtMaKhach;
        private Label lblMaKhach;
        private Label lblThongTinKhachTitle;
        private Panel panelDanhSachKhach;
        private DataGridView dgvDanhSachKhach;
        private DataGridViewTextBoxColumn colIDKhach;
        private DataGridViewTextBoxColumn colHoVaTen;
        private DataGridViewTextBoxColumn colDienThoai;
        private DataGridViewTextBoxColumn colDiaChi;
        private Panel panelDanhSachHeader;
        private Button btnNhapKhach;
        private Button btnXuatKhach;
        private Label lblDanhSachKhachTitle;
        private Panel panelTopbar;
        private Button btnUserMenu;
        private Label lblUserName;
        private PictureBox picAvatar;
        private Label lblPageTitle;
    }
}