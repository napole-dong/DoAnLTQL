namespace QuanLyQuanCaPhe.Forms
{
    partial class frmCongThuc
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
            btnBanHang = new Button();
            btnQuanLyBan = new Button();
            btnQuanLyMon = new Button();
            btnCongThuc = new Button();
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
            cardTongCongThuc = new Panel();
            lblTongCongThucValue = new Label();
            lblTongCongThucTitle = new Label();
            lblTongCongThucIcon = new Label();
            cardMonCoCongThuc = new Panel();
            lblMonCoCongThucValue = new Label();
            lblMonCoCongThucTitle = new Label();
            lblMonCoCongThucIcon = new Label();
            cardNguyenLieuThamGia = new Panel();
            lblNguyenLieuThamGiaValue = new Label();
            lblNguyenLieuThamGiaTitle = new Label();
            lblNguyenLieuThamGiaIcon = new Label();
            cardCongThucCanhBao = new Panel();
            lblCongThucCanhBaoValue = new Label();
            lblCongThucCanhBaoTitle = new Label();
            lblCongThucCanhBaoIcon = new Label();
            tableCenter = new TableLayoutPanel();
            panelThongTinCongThuc = new Panel();
            btnLamMoiThongTin = new Button();
            btnXoaCongThuc = new Button();
            btnCapNhatCongThuc = new Button();
            btnThemCongThuc = new Button();
            lblGhiChuHint = new Label();
            txtTrangThaiTon = new TextBox();
            lblTrangThaiTon = new Label();
            txtSoLuongTon = new TextBox();
            lblSoLuongTon = new Label();
            txtDonViTinh = new TextBox();
            lblDonViTinh = new Label();
            txtDinhLuong = new TextBox();
            lblDinhLuong = new Label();
            cboNguyenLieu = new ComboBox();
            lblNguyenLieu = new Label();
            txtMaNguyenLieu = new TextBox();
            lblMaNguyenLieu = new Label();
            cboMon = new ComboBox();
            lblMon = new Label();
            txtMaMon = new TextBox();
            lblMaMon = new Label();
            lblThongTinCongThucTitle = new Label();
            panelDanhSachCongThuc = new Panel();
            dgvDanhSachCongThuc = new DataGridView();
            colMonID = new DataGridViewTextBoxColumn();
            colTenMon = new DataGridViewTextBoxColumn();
            colNguyenLieuID = new DataGridViewTextBoxColumn();
            colTenNguyenLieu = new DataGridViewTextBoxColumn();
            colSoLuong = new DataGridViewTextBoxColumn();
            colDonViTinh = new DataGridViewTextBoxColumn();
            colSoLuongTon = new DataGridViewTextBoxColumn();
            colTrangThaiTon = new DataGridViewTextBoxColumn();
            panelDanhSachHeader = new Panel();
            btnLamMoiDanhSach = new Button();
            cboLocMon = new ComboBox();
            lblLocMon = new Label();
            txtTimKiem = new TextBox();
            lblTimKiem = new Label();
            lblDanhSachCongThucTitle = new Label();
            panelTopbar = new Panel();
            lblPageTitle = new Label();
            panelSidebar.SuspendLayout();
            flowSidebarMenu.SuspendLayout();
            panelLogo.SuspendLayout();
            panelMain.SuspendLayout();
            panelContent.SuspendLayout();
            tableMain.SuspendLayout();
            tableStats.SuspendLayout();
            cardTongCongThuc.SuspendLayout();
            cardMonCoCongThuc.SuspendLayout();
            cardNguyenLieuThamGia.SuspendLayout();
            cardCongThucCanhBao.SuspendLayout();
            tableCenter.SuspendLayout();
            panelThongTinCongThuc.SuspendLayout();
            panelDanhSachCongThuc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachCongThuc).BeginInit();
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
            panelSidebar.Size = new Size(230, 821);
            panelSidebar.TabIndex = 0;
            // 
            // btnDangXuat
            // 
            btnDangXuat.Dock = DockStyle.Bottom;
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnDangXuat.ForeColor = Color.Gainsboro;
            btnDangXuat.Location = new Point(0, 765);
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
            flowSidebarMenu.Controls.Add(btnBanHang);
            flowSidebarMenu.Controls.Add(btnQuanLyBan);
            flowSidebarMenu.Controls.Add(btnQuanLyMon);
            flowSidebarMenu.Controls.Add(btnCongThuc);
            flowSidebarMenu.Controls.Add(btnQuanLyKho);
            flowSidebarMenu.Controls.Add(btnHoaDon);
            flowSidebarMenu.Controls.Add(btnNhanVien);
            flowSidebarMenu.Controls.Add(btnThongKe);
            flowSidebarMenu.Dock = DockStyle.Fill;
            flowSidebarMenu.FlowDirection = FlowDirection.TopDown;
            flowSidebarMenu.Location = new Point(0, 92);
            flowSidebarMenu.Name = "flowSidebarMenu";
            flowSidebarMenu.Padding = new Padding(0, 14, 0, 0);
            flowSidebarMenu.Size = new Size(230, 729);
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
            btnBanHang.Text = "\U0001f9fe  Bán hàng";
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
            // btnCongThuc
            // 
            btnCongThuc.BackColor = Color.FromArgb(94, 64, 47);
            btnCongThuc.FlatAppearance.BorderSize = 0;
            btnCongThuc.FlatStyle = FlatStyle.Flat;
            btnCongThuc.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCongThuc.ForeColor = Color.White;
            btnCongThuc.Location = new Point(0, 158);
            btnCongThuc.Margin = new Padding(0);
            btnCongThuc.Name = "btnCongThuc";
            btnCongThuc.Padding = new Padding(20, 0, 0, 0);
            btnCongThuc.Size = new Size(230, 48);
            btnCongThuc.TabIndex = 3;
            btnCongThuc.Text = "\U0001f9ea  Công thức";
            btnCongThuc.TextAlign = ContentAlignment.MiddleLeft;
            btnCongThuc.UseVisualStyleBackColor = false;
            // 
            // btnQuanLyKho
            // 
            btnQuanLyKho.FlatAppearance.BorderSize = 0;
            btnQuanLyKho.FlatStyle = FlatStyle.Flat;
            btnQuanLyKho.Font = new Font("Segoe UI", 10F);
            btnQuanLyKho.ForeColor = Color.Gainsboro;
            btnQuanLyKho.Location = new Point(0, 206);
            btnQuanLyKho.Margin = new Padding(0);
            btnQuanLyKho.Name = "btnQuanLyKho";
            btnQuanLyKho.Padding = new Padding(20, 0, 0, 0);
            btnQuanLyKho.Size = new Size(230, 48);
            btnQuanLyKho.TabIndex = 4;
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
            btnHoaDon.Location = new Point(0, 254);
            btnHoaDon.Margin = new Padding(0);
            btnHoaDon.Name = "btnHoaDon";
            btnHoaDon.Padding = new Padding(20, 0, 0, 0);
            btnHoaDon.Size = new Size(230, 48);
            btnHoaDon.TabIndex = 5;
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
            btnNhanVien.Location = new Point(0, 302);
            btnNhanVien.Margin = new Padding(0);
            btnNhanVien.Name = "btnNhanVien";
            btnNhanVien.Padding = new Padding(20, 0, 0, 0);
            btnNhanVien.Size = new Size(230, 48);
            btnNhanVien.TabIndex = 6;
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
            panelMain.Size = new Size(1134, 821);
            panelMain.TabIndex = 1;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(tableMain);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 80);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(22, 16, 22, 22);
            panelContent.Size = new Size(1134, 741);
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
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMain.Size = new Size(1090, 703);
            tableMain.TabIndex = 0;
            // 
            // tableStats
            // 
            tableStats.ColumnCount = 4;
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableStats.Controls.Add(cardTongCongThuc, 0, 0);
            tableStats.Controls.Add(cardMonCoCongThuc, 1, 0);
            tableStats.Controls.Add(cardNguyenLieuThamGia, 2, 0);
            tableStats.Controls.Add(cardCongThucCanhBao, 3, 0);
            tableStats.Dock = DockStyle.Fill;
            tableStats.Location = new Point(0, 0);
            tableStats.Margin = new Padding(0, 0, 0, 12);
            tableStats.Name = "tableStats";
            tableStats.RowCount = 1;
            tableStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableStats.Size = new Size(1090, 106);
            tableStats.TabIndex = 0;
            // 
            // cardTongCongThuc
            // 
            cardTongCongThuc.BackColor = Color.FromArgb(237, 247, 243);
            cardTongCongThuc.Controls.Add(lblTongCongThucValue);
            cardTongCongThuc.Controls.Add(lblTongCongThucTitle);
            cardTongCongThuc.Controls.Add(lblTongCongThucIcon);
            cardTongCongThuc.Dock = DockStyle.Fill;
            cardTongCongThuc.Location = new Point(0, 0);
            cardTongCongThuc.Margin = new Padding(0, 0, 8, 0);
            cardTongCongThuc.Name = "cardTongCongThuc";
            cardTongCongThuc.Padding = new Padding(16, 12, 16, 12);
            cardTongCongThuc.Size = new Size(264, 106);
            cardTongCongThuc.TabIndex = 0;
            // 
            // lblTongCongThucValue
            // 
            lblTongCongThucValue.AutoSize = true;
            lblTongCongThucValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTongCongThucValue.ForeColor = Color.FromArgb(34, 111, 92);
            lblTongCongThucValue.Location = new Point(18, 56);
            lblTongCongThucValue.Name = "lblTongCongThucValue";
            lblTongCongThucValue.Size = new Size(28, 37);
            lblTongCongThucValue.TabIndex = 2;
            lblTongCongThucValue.Text = "-";
            // 
            // lblTongCongThucTitle
            // 
            lblTongCongThucTitle.AutoSize = true;
            lblTongCongThucTitle.Font = new Font("Segoe UI", 10F);
            lblTongCongThucTitle.ForeColor = Color.FromArgb(90, 106, 101);
            lblTongCongThucTitle.Location = new Point(18, 33);
            lblTongCongThucTitle.Name = "lblTongCongThucTitle";
            lblTongCongThucTitle.Size = new Size(131, 23);
            lblTongCongThucTitle.TabIndex = 1;
            lblTongCongThucTitle.Text = "Tổng công thức";
            // 
            // lblTongCongThucIcon
            // 
            lblTongCongThucIcon.AutoSize = true;
            lblTongCongThucIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblTongCongThucIcon.Location = new Point(16, 8);
            lblTongCongThucIcon.Name = "lblTongCongThucIcon";
            lblTongCongThucIcon.Size = new Size(39, 27);
            lblTongCongThucIcon.TabIndex = 0;
            lblTongCongThucIcon.Text = "\U0001f9ea";
            // 
            // cardMonCoCongThuc
            // 
            cardMonCoCongThuc.BackColor = Color.FromArgb(236, 242, 252);
            cardMonCoCongThuc.Controls.Add(lblMonCoCongThucValue);
            cardMonCoCongThuc.Controls.Add(lblMonCoCongThucTitle);
            cardMonCoCongThuc.Controls.Add(lblMonCoCongThucIcon);
            cardMonCoCongThuc.Dock = DockStyle.Fill;
            cardMonCoCongThuc.Location = new Point(272, 0);
            cardMonCoCongThuc.Margin = new Padding(0, 0, 8, 0);
            cardMonCoCongThuc.Name = "cardMonCoCongThuc";
            cardMonCoCongThuc.Padding = new Padding(16, 12, 16, 12);
            cardMonCoCongThuc.Size = new Size(264, 106);
            cardMonCoCongThuc.TabIndex = 1;
            // 
            // lblMonCoCongThucValue
            // 
            lblMonCoCongThucValue.AutoSize = true;
            lblMonCoCongThucValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblMonCoCongThucValue.ForeColor = Color.FromArgb(35, 84, 148);
            lblMonCoCongThucValue.Location = new Point(18, 56);
            lblMonCoCongThucValue.Name = "lblMonCoCongThucValue";
            lblMonCoCongThucValue.Size = new Size(28, 37);
            lblMonCoCongThucValue.TabIndex = 2;
            lblMonCoCongThucValue.Text = "-";
            // 
            // lblMonCoCongThucTitle
            // 
            lblMonCoCongThucTitle.AutoSize = true;
            lblMonCoCongThucTitle.Font = new Font("Segoe UI", 10F);
            lblMonCoCongThucTitle.ForeColor = Color.FromArgb(89, 100, 116);
            lblMonCoCongThucTitle.Location = new Point(18, 33);
            lblMonCoCongThucTitle.Name = "lblMonCoCongThucTitle";
            lblMonCoCongThucTitle.Size = new Size(150, 23);
            lblMonCoCongThucTitle.TabIndex = 1;
            lblMonCoCongThucTitle.Text = "Món có công thức";
            // 
            // lblMonCoCongThucIcon
            // 
            lblMonCoCongThucIcon.AutoSize = true;
            lblMonCoCongThucIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblMonCoCongThucIcon.Location = new Point(16, 8);
            lblMonCoCongThucIcon.Name = "lblMonCoCongThucIcon";
            lblMonCoCongThucIcon.Size = new Size(39, 27);
            lblMonCoCongThucIcon.TabIndex = 0;
            lblMonCoCongThucIcon.Text = "☕";
            // 
            // cardNguyenLieuThamGia
            // 
            cardNguyenLieuThamGia.BackColor = Color.FromArgb(252, 244, 232);
            cardNguyenLieuThamGia.Controls.Add(lblNguyenLieuThamGiaValue);
            cardNguyenLieuThamGia.Controls.Add(lblNguyenLieuThamGiaTitle);
            cardNguyenLieuThamGia.Controls.Add(lblNguyenLieuThamGiaIcon);
            cardNguyenLieuThamGia.Dock = DockStyle.Fill;
            cardNguyenLieuThamGia.Location = new Point(544, 0);
            cardNguyenLieuThamGia.Margin = new Padding(0, 0, 8, 0);
            cardNguyenLieuThamGia.Name = "cardNguyenLieuThamGia";
            cardNguyenLieuThamGia.Padding = new Padding(16, 12, 16, 12);
            cardNguyenLieuThamGia.Size = new Size(264, 106);
            cardNguyenLieuThamGia.TabIndex = 2;
            // 
            // lblNguyenLieuThamGiaValue
            // 
            lblNguyenLieuThamGiaValue.AutoSize = true;
            lblNguyenLieuThamGiaValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblNguyenLieuThamGiaValue.ForeColor = Color.FromArgb(139, 90, 20);
            lblNguyenLieuThamGiaValue.Location = new Point(18, 56);
            lblNguyenLieuThamGiaValue.Name = "lblNguyenLieuThamGiaValue";
            lblNguyenLieuThamGiaValue.Size = new Size(28, 37);
            lblNguyenLieuThamGiaValue.TabIndex = 2;
            lblNguyenLieuThamGiaValue.Text = "-";
            // 
            // lblNguyenLieuThamGiaTitle
            // 
            lblNguyenLieuThamGiaTitle.AutoSize = true;
            lblNguyenLieuThamGiaTitle.Font = new Font("Segoe UI", 10F);
            lblNguyenLieuThamGiaTitle.ForeColor = Color.FromArgb(111, 95, 70);
            lblNguyenLieuThamGiaTitle.Location = new Point(18, 33);
            lblNguyenLieuThamGiaTitle.Name = "lblNguyenLieuThamGiaTitle";
            lblNguyenLieuThamGiaTitle.Size = new Size(175, 23);
            lblNguyenLieuThamGiaTitle.TabIndex = 1;
            lblNguyenLieuThamGiaTitle.Text = "Nguyên liệu tham gia";
            // 
            // lblNguyenLieuThamGiaIcon
            // 
            lblNguyenLieuThamGiaIcon.AutoSize = true;
            lblNguyenLieuThamGiaIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblNguyenLieuThamGiaIcon.Location = new Point(16, 8);
            lblNguyenLieuThamGiaIcon.Name = "lblNguyenLieuThamGiaIcon";
            lblNguyenLieuThamGiaIcon.Size = new Size(39, 27);
            lblNguyenLieuThamGiaIcon.TabIndex = 0;
            lblNguyenLieuThamGiaIcon.Text = "\U0001f9c2";
            // 
            // cardCongThucCanhBao
            // 
            cardCongThucCanhBao.BackColor = Color.FromArgb(250, 237, 237);
            cardCongThucCanhBao.Controls.Add(lblCongThucCanhBaoValue);
            cardCongThucCanhBao.Controls.Add(lblCongThucCanhBaoTitle);
            cardCongThucCanhBao.Controls.Add(lblCongThucCanhBaoIcon);
            cardCongThucCanhBao.Dock = DockStyle.Fill;
            cardCongThucCanhBao.Location = new Point(816, 0);
            cardCongThucCanhBao.Margin = new Padding(0);
            cardCongThucCanhBao.Name = "cardCongThucCanhBao";
            cardCongThucCanhBao.Padding = new Padding(16, 12, 16, 12);
            cardCongThucCanhBao.Size = new Size(274, 106);
            cardCongThucCanhBao.TabIndex = 3;
            // 
            // lblCongThucCanhBaoValue
            // 
            lblCongThucCanhBaoValue.AutoSize = true;
            lblCongThucCanhBaoValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblCongThucCanhBaoValue.ForeColor = Color.FromArgb(158, 52, 52);
            lblCongThucCanhBaoValue.Location = new Point(18, 56);
            lblCongThucCanhBaoValue.Name = "lblCongThucCanhBaoValue";
            lblCongThucCanhBaoValue.Size = new Size(28, 37);
            lblCongThucCanhBaoValue.TabIndex = 2;
            lblCongThucCanhBaoValue.Text = "-";
            // 
            // lblCongThucCanhBaoTitle
            // 
            lblCongThucCanhBaoTitle.AutoSize = true;
            lblCongThucCanhBaoTitle.Font = new Font("Segoe UI", 10F);
            lblCongThucCanhBaoTitle.ForeColor = Color.FromArgb(122, 88, 88);
            lblCongThucCanhBaoTitle.Location = new Point(18, 33);
            lblCongThucCanhBaoTitle.Name = "lblCongThucCanhBaoTitle";
            lblCongThucCanhBaoTitle.Size = new Size(165, 23);
            lblCongThucCanhBaoTitle.TabIndex = 1;
            lblCongThucCanhBaoTitle.Text = "Công thức thiếu tồn";
            // 
            // lblCongThucCanhBaoIcon
            // 
            lblCongThucCanhBaoIcon.AutoSize = true;
            lblCongThucCanhBaoIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblCongThucCanhBaoIcon.Location = new Point(16, 8);
            lblCongThucCanhBaoIcon.Name = "lblCongThucCanhBaoIcon";
            lblCongThucCanhBaoIcon.Size = new Size(39, 27);
            lblCongThucCanhBaoIcon.TabIndex = 0;
            lblCongThucCanhBaoIcon.Text = "⚠️";
            // 
            // tableCenter
            // 
            tableCenter.ColumnCount = 2;
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360F));
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableCenter.Controls.Add(panelThongTinCongThuc, 0, 0);
            tableCenter.Controls.Add(panelDanhSachCongThuc, 1, 0);
            tableCenter.Dock = DockStyle.Fill;
            tableCenter.Location = new Point(0, 118);
            tableCenter.Margin = new Padding(0);
            tableCenter.Name = "tableCenter";
            tableCenter.RowCount = 1;
            tableCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableCenter.Size = new Size(1090, 585);
            tableCenter.TabIndex = 1;
            // 
            // panelThongTinCongThuc
            // 
            panelThongTinCongThuc.BackColor = Color.White;
            panelThongTinCongThuc.Controls.Add(btnLamMoiThongTin);
            panelThongTinCongThuc.Controls.Add(btnXoaCongThuc);
            panelThongTinCongThuc.Controls.Add(btnCapNhatCongThuc);
            panelThongTinCongThuc.Controls.Add(btnThemCongThuc);
            panelThongTinCongThuc.Controls.Add(lblGhiChuHint);
            panelThongTinCongThuc.Controls.Add(txtTrangThaiTon);
            panelThongTinCongThuc.Controls.Add(lblTrangThaiTon);
            panelThongTinCongThuc.Controls.Add(txtSoLuongTon);
            panelThongTinCongThuc.Controls.Add(lblSoLuongTon);
            panelThongTinCongThuc.Controls.Add(txtDonViTinh);
            panelThongTinCongThuc.Controls.Add(lblDonViTinh);
            panelThongTinCongThuc.Controls.Add(txtDinhLuong);
            panelThongTinCongThuc.Controls.Add(lblDinhLuong);
            panelThongTinCongThuc.Controls.Add(cboNguyenLieu);
            panelThongTinCongThuc.Controls.Add(lblNguyenLieu);
            panelThongTinCongThuc.Controls.Add(txtMaNguyenLieu);
            panelThongTinCongThuc.Controls.Add(lblMaNguyenLieu);
            panelThongTinCongThuc.Controls.Add(cboMon);
            panelThongTinCongThuc.Controls.Add(lblMon);
            panelThongTinCongThuc.Controls.Add(txtMaMon);
            panelThongTinCongThuc.Controls.Add(lblMaMon);
            panelThongTinCongThuc.Controls.Add(lblThongTinCongThucTitle);
            panelThongTinCongThuc.Dock = DockStyle.Fill;
            panelThongTinCongThuc.Location = new Point(0, 0);
            panelThongTinCongThuc.Margin = new Padding(0, 0, 14, 0);
            panelThongTinCongThuc.Name = "panelThongTinCongThuc";
            panelThongTinCongThuc.Padding = new Padding(18, 16, 18, 16);
            panelThongTinCongThuc.Size = new Size(346, 585);
            panelThongTinCongThuc.TabIndex = 0;
            // 
            // btnLamMoiThongTin
            // 
            btnLamMoiThongTin.BackColor = Color.FromArgb(248, 245, 241);
            btnLamMoiThongTin.FlatAppearance.BorderSize = 0;
            btnLamMoiThongTin.FlatStyle = FlatStyle.Flat;
            btnLamMoiThongTin.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLamMoiThongTin.ForeColor = Color.FromArgb(65, 48, 39);
            btnLamMoiThongTin.Location = new Point(179, 538);
            btnLamMoiThongTin.Name = "btnLamMoiThongTin";
            btnLamMoiThongTin.Size = new Size(146, 32);
            btnLamMoiThongTin.TabIndex = 21;
            btnLamMoiThongTin.Text = "Làm mới";
            btnLamMoiThongTin.UseVisualStyleBackColor = false;
            // 
            // btnXoaCongThuc
            // 
            btnXoaCongThuc.BackColor = Color.FromArgb(220, 53, 69);
            btnXoaCongThuc.FlatAppearance.BorderSize = 0;
            btnXoaCongThuc.FlatStyle = FlatStyle.Flat;
            btnXoaCongThuc.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnXoaCongThuc.ForeColor = Color.White;
            btnXoaCongThuc.Location = new Point(20, 538);
            btnXoaCongThuc.Name = "btnXoaCongThuc";
            btnXoaCongThuc.Size = new Size(146, 32);
            btnXoaCongThuc.TabIndex = 20;
            btnXoaCongThuc.Text = "Xóa công thức";
            btnXoaCongThuc.UseVisualStyleBackColor = false;
            // 
            // btnCapNhatCongThuc
            // 
            btnCapNhatCongThuc.BackColor = Color.FromArgb(25, 135, 84);
            btnCapNhatCongThuc.FlatAppearance.BorderSize = 0;
            btnCapNhatCongThuc.FlatStyle = FlatStyle.Flat;
            btnCapNhatCongThuc.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCapNhatCongThuc.ForeColor = Color.White;
            btnCapNhatCongThuc.Location = new Point(179, 498);
            btnCapNhatCongThuc.Name = "btnCapNhatCongThuc";
            btnCapNhatCongThuc.Size = new Size(146, 32);
            btnCapNhatCongThuc.TabIndex = 19;
            btnCapNhatCongThuc.Text = "Cập nhật";
            btnCapNhatCongThuc.UseVisualStyleBackColor = false;
            // 
            // btnThemCongThuc
            // 
            btnThemCongThuc.BackColor = Color.FromArgb(13, 110, 253);
            btnThemCongThuc.FlatAppearance.BorderSize = 0;
            btnThemCongThuc.FlatStyle = FlatStyle.Flat;
            btnThemCongThuc.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnThemCongThuc.ForeColor = Color.White;
            btnThemCongThuc.Location = new Point(20, 498);
            btnThemCongThuc.Name = "btnThemCongThuc";
            btnThemCongThuc.Size = new Size(146, 32);
            btnThemCongThuc.TabIndex = 18;
            btnThemCongThuc.Text = "Thêm công thức";
            btnThemCongThuc.UseVisualStyleBackColor = false;
            // 
            // lblGhiChuHint
            // 
            lblGhiChuHint.AutoSize = true;
            lblGhiChuHint.ForeColor = Color.FromArgb(140, 112, 73);
            lblGhiChuHint.Location = new Point(21, 476);
            lblGhiChuHint.Name = "lblGhiChuHint";
            lblGhiChuHint.Size = new Size(291, 20);
            lblGhiChuHint.TabIndex = 17;
            lblGhiChuHint.Text = "Mẹo: ưu tiên nguyên liệu còn tồn kho thấp.";
            // 
            // txtTrangThaiTon
            // 
            txtTrangThaiTon.BorderStyle = BorderStyle.FixedSingle;
            txtTrangThaiTon.Location = new Point(21, 442);
            txtTrangThaiTon.Name = "txtTrangThaiTon";
            txtTrangThaiTon.ReadOnly = true;
            txtTrangThaiTon.Size = new Size(304, 27);
            txtTrangThaiTon.TabIndex = 16;
            // 
            // lblTrangThaiTon
            // 
            lblTrangThaiTon.AutoSize = true;
            lblTrangThaiTon.ForeColor = Color.FromArgb(88, 72, 62);
            lblTrangThaiTon.Location = new Point(21, 418);
            lblTrangThaiTon.Name = "lblTrangThaiTon";
            lblTrangThaiTon.Size = new Size(101, 20);
            lblTrangThaiTon.TabIndex = 15;
            lblTrangThaiTon.Text = "Trạng thái tồn";
            // 
            // txtSoLuongTon
            // 
            txtSoLuongTon.BorderStyle = BorderStyle.FixedSingle;
            txtSoLuongTon.Location = new Point(177, 386);
            txtSoLuongTon.Name = "txtSoLuongTon";
            txtSoLuongTon.ReadOnly = true;
            txtSoLuongTon.Size = new Size(148, 27);
            txtSoLuongTon.TabIndex = 14;
            // 
            // lblSoLuongTon
            // 
            lblSoLuongTon.AutoSize = true;
            lblSoLuongTon.ForeColor = Color.FromArgb(88, 72, 62);
            lblSoLuongTon.Location = new Point(177, 362);
            lblSoLuongTon.Name = "lblSoLuongTon";
            lblSoLuongTon.Size = new Size(95, 20);
            lblSoLuongTon.TabIndex = 13;
            lblSoLuongTon.Text = "Số lượng tồn";
            // 
            // txtDonViTinh
            // 
            txtDonViTinh.BorderStyle = BorderStyle.FixedSingle;
            txtDonViTinh.Location = new Point(21, 386);
            txtDonViTinh.Name = "txtDonViTinh";
            txtDonViTinh.ReadOnly = true;
            txtDonViTinh.Size = new Size(148, 27);
            txtDonViTinh.TabIndex = 12;
            // 
            // lblDonViTinh
            // 
            lblDonViTinh.AutoSize = true;
            lblDonViTinh.ForeColor = Color.FromArgb(88, 72, 62);
            lblDonViTinh.Location = new Point(21, 362);
            lblDonViTinh.Name = "lblDonViTinh";
            lblDonViTinh.Size = new Size(81, 20);
            lblDonViTinh.TabIndex = 11;
            lblDonViTinh.Text = "Đơn vị tính";
            // 
            // txtDinhLuong
            // 
            txtDinhLuong.BorderStyle = BorderStyle.FixedSingle;
            txtDinhLuong.Location = new Point(21, 330);
            txtDinhLuong.Name = "txtDinhLuong";
            txtDinhLuong.Size = new Size(304, 27);
            txtDinhLuong.TabIndex = 10;
            // 
            // lblDinhLuong
            // 
            lblDinhLuong.AutoSize = true;
            lblDinhLuong.ForeColor = Color.FromArgb(88, 72, 62);
            lblDinhLuong.Location = new Point(21, 306);
            lblDinhLuong.Name = "lblDinhLuong";
            lblDinhLuong.Size = new Size(127, 20);
            lblDinhLuong.TabIndex = 9;
            lblDinhLuong.Text = "Định lượng / món";
            // 
            // cboNguyenLieu
            // 
            cboNguyenLieu.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNguyenLieu.FormattingEnabled = true;
            cboNguyenLieu.Location = new Point(21, 273);
            cboNguyenLieu.Name = "cboNguyenLieu";
            cboNguyenLieu.Size = new Size(304, 28);
            cboNguyenLieu.TabIndex = 8;
            // 
            // lblNguyenLieu
            // 
            lblNguyenLieu.AutoSize = true;
            lblNguyenLieu.ForeColor = Color.FromArgb(88, 72, 62);
            lblNguyenLieu.Location = new Point(21, 249);
            lblNguyenLieu.Name = "lblNguyenLieu";
            lblNguyenLieu.Size = new Size(112, 20);
            lblNguyenLieu.TabIndex = 7;
            lblNguyenLieu.Text = "Tên nguyên liệu";
            // 
            // txtMaNguyenLieu
            // 
            txtMaNguyenLieu.BorderStyle = BorderStyle.FixedSingle;
            txtMaNguyenLieu.Location = new Point(21, 217);
            txtMaNguyenLieu.Name = "txtMaNguyenLieu";
            txtMaNguyenLieu.ReadOnly = true;
            txtMaNguyenLieu.Size = new Size(130, 27);
            txtMaNguyenLieu.TabIndex = 6;
            txtMaNguyenLieu.TextAlign = HorizontalAlignment.Center;
            // 
            // lblMaNguyenLieu
            // 
            lblMaNguyenLieu.AutoSize = true;
            lblMaNguyenLieu.ForeColor = Color.FromArgb(88, 72, 62);
            lblMaNguyenLieu.Location = new Point(21, 193);
            lblMaNguyenLieu.Name = "lblMaNguyenLieu";
            lblMaNguyenLieu.Size = new Size(52, 20);
            lblMaNguyenLieu.TabIndex = 5;
            lblMaNguyenLieu.Text = "Mã NL";
            // 
            // cboMon
            // 
            cboMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMon.FormattingEnabled = true;
            cboMon.Location = new Point(21, 160);
            cboMon.Name = "cboMon";
            cboMon.Size = new Size(304, 28);
            cboMon.TabIndex = 4;
            // 
            // lblMon
            // 
            lblMon.AutoSize = true;
            lblMon.ForeColor = Color.FromArgb(88, 72, 62);
            lblMon.Location = new Point(21, 136);
            lblMon.Name = "lblMon";
            lblMon.Size = new Size(66, 20);
            lblMon.TabIndex = 3;
            lblMon.Text = "Tên món";
            // 
            // txtMaMon
            // 
            txtMaMon.BorderStyle = BorderStyle.FixedSingle;
            txtMaMon.Location = new Point(21, 104);
            txtMaMon.Name = "txtMaMon";
            txtMaMon.ReadOnly = true;
            txtMaMon.Size = new Size(130, 27);
            txtMaMon.TabIndex = 2;
            txtMaMon.TextAlign = HorizontalAlignment.Center;
            // 
            // lblMaMon
            // 
            lblMaMon.AutoSize = true;
            lblMaMon.ForeColor = Color.FromArgb(88, 72, 62);
            lblMaMon.Location = new Point(21, 80);
            lblMaMon.Name = "lblMaMon";
            lblMaMon.Size = new Size(64, 20);
            lblMaMon.TabIndex = 1;
            lblMaMon.Text = "Mã món";
            // 
            // lblThongTinCongThucTitle
            // 
            lblThongTinCongThucTitle.AutoSize = true;
            lblThongTinCongThucTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblThongTinCongThucTitle.ForeColor = Color.FromArgb(62, 45, 36);
            lblThongTinCongThucTitle.Location = new Point(20, 20);
            lblThongTinCongThucTitle.Name = "lblThongTinCongThucTitle";
            lblThongTinCongThucTitle.Size = new Size(185, 25);
            lblThongTinCongThucTitle.TabIndex = 0;
            lblThongTinCongThucTitle.Text = "Thông tin công thức";
            // 
            // panelDanhSachCongThuc
            // 
            panelDanhSachCongThuc.BackColor = Color.White;
            panelDanhSachCongThuc.Controls.Add(dgvDanhSachCongThuc);
            panelDanhSachCongThuc.Controls.Add(panelDanhSachHeader);
            panelDanhSachCongThuc.Dock = DockStyle.Fill;
            panelDanhSachCongThuc.Location = new Point(360, 0);
            panelDanhSachCongThuc.Margin = new Padding(0);
            panelDanhSachCongThuc.Name = "panelDanhSachCongThuc";
            panelDanhSachCongThuc.Size = new Size(730, 585);
            panelDanhSachCongThuc.TabIndex = 1;
            // 
            // dgvDanhSachCongThuc
            // 
            dgvDanhSachCongThuc.AllowUserToAddRows = false;
            dgvDanhSachCongThuc.AllowUserToDeleteRows = false;
            dgvDanhSachCongThuc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhSachCongThuc.BackgroundColor = Color.White;
            dgvDanhSachCongThuc.BorderStyle = BorderStyle.None;
            dgvDanhSachCongThuc.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDanhSachCongThuc.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(246, 239, 232);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(86, 62, 46);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(246, 239, 232);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(86, 62, 46);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDanhSachCongThuc.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDanhSachCongThuc.ColumnHeadersHeight = 42;
            dgvDanhSachCongThuc.Columns.AddRange(new DataGridViewColumn[] { colMonID, colTenMon, colNguyenLieuID, colTenNguyenLieu, colSoLuong, colDonViTinh, colSoLuongTon, colTrangThaiTon });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(58, 58, 58);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(244, 233, 220);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(58, 58, 58);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDanhSachCongThuc.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDanhSachCongThuc.Dock = DockStyle.Fill;
            dgvDanhSachCongThuc.EnableHeadersVisualStyles = false;
            dgvDanhSachCongThuc.GridColor = Color.FromArgb(238, 230, 220);
            dgvDanhSachCongThuc.Location = new Point(0, 98);
            dgvDanhSachCongThuc.MultiSelect = false;
            dgvDanhSachCongThuc.Name = "dgvDanhSachCongThuc";
            dgvDanhSachCongThuc.ReadOnly = true;
            dgvDanhSachCongThuc.RowHeadersVisible = false;
            dgvDanhSachCongThuc.RowHeadersWidth = 51;
            dgvDanhSachCongThuc.RowTemplate.Height = 34;
            dgvDanhSachCongThuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhSachCongThuc.Size = new Size(730, 487);
            dgvDanhSachCongThuc.TabIndex = 1;
            // 
            // colMonID
            // 
            colMonID.DataPropertyName = "MonID";
            colMonID.FillWeight = 70F;
            colMonID.HeaderText = "Mã món";
            colMonID.MinimumWidth = 6;
            colMonID.Name = "colMonID";
            colMonID.ReadOnly = true;
            // 
            // colTenMon
            // 
            colTenMon.DataPropertyName = "TenMon";
            colTenMon.FillWeight = 150F;
            colTenMon.HeaderText = "Tên món";
            colTenMon.MinimumWidth = 6;
            colTenMon.Name = "colTenMon";
            colTenMon.ReadOnly = true;
            // 
            // colNguyenLieuID
            // 
            colNguyenLieuID.DataPropertyName = "NguyenLieuID";
            colNguyenLieuID.FillWeight = 75F;
            colNguyenLieuID.HeaderText = "Mã NL";
            colNguyenLieuID.MinimumWidth = 6;
            colNguyenLieuID.Name = "colNguyenLieuID";
            colNguyenLieuID.ReadOnly = true;
            // 
            // colTenNguyenLieu
            // 
            colTenNguyenLieu.DataPropertyName = "TenNguyenLieu";
            colTenNguyenLieu.FillWeight = 160F;
            colTenNguyenLieu.HeaderText = "Nguyên liệu";
            colTenNguyenLieu.MinimumWidth = 6;
            colTenNguyenLieu.Name = "colTenNguyenLieu";
            colTenNguyenLieu.ReadOnly = true;
            // 
            // colSoLuong
            // 
            colSoLuong.DataPropertyName = "SoLuongHienThi";
            colSoLuong.FillWeight = 90F;
            colSoLuong.HeaderText = "Định lượng";
            colSoLuong.MinimumWidth = 6;
            colSoLuong.Name = "colSoLuong";
            colSoLuong.ReadOnly = true;
            // 
            // colDonViTinh
            // 
            colDonViTinh.DataPropertyName = "DonViTinh";
            colDonViTinh.FillWeight = 85F;
            colDonViTinh.HeaderText = "ĐVT";
            colDonViTinh.MinimumWidth = 6;
            colDonViTinh.Name = "colDonViTinh";
            colDonViTinh.ReadOnly = true;
            // 
            // colSoLuongTon
            // 
            colSoLuongTon.DataPropertyName = "SoLuongTonHienThi";
            colSoLuongTon.FillWeight = 95F;
            colSoLuongTon.HeaderText = "Tồn kho";
            colSoLuongTon.MinimumWidth = 6;
            colSoLuongTon.Name = "colSoLuongTon";
            colSoLuongTon.ReadOnly = true;
            // 
            // colTrangThaiTon
            // 
            colTrangThaiTon.DataPropertyName = "TrangThaiTonHienThi";
            colTrangThaiTon.FillWeight = 110F;
            colTrangThaiTon.HeaderText = "Trạng thái";
            colTrangThaiTon.MinimumWidth = 6;
            colTrangThaiTon.Name = "colTrangThaiTon";
            colTrangThaiTon.ReadOnly = true;
            // 
            // panelDanhSachHeader
            // 
            panelDanhSachHeader.BackColor = Color.White;
            panelDanhSachHeader.Controls.Add(btnLamMoiDanhSach);
            panelDanhSachHeader.Controls.Add(cboLocMon);
            panelDanhSachHeader.Controls.Add(lblLocMon);
            panelDanhSachHeader.Controls.Add(txtTimKiem);
            panelDanhSachHeader.Controls.Add(lblTimKiem);
            panelDanhSachHeader.Controls.Add(lblDanhSachCongThucTitle);
            panelDanhSachHeader.Dock = DockStyle.Top;
            panelDanhSachHeader.Location = new Point(0, 0);
            panelDanhSachHeader.Name = "panelDanhSachHeader";
            panelDanhSachHeader.Size = new Size(730, 98);
            panelDanhSachHeader.TabIndex = 0;
            // 
            // btnLamMoiDanhSach
            // 
            btnLamMoiDanhSach.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLamMoiDanhSach.BackColor = Color.FromArgb(248, 245, 241);
            btnLamMoiDanhSach.FlatAppearance.BorderSize = 0;
            btnLamMoiDanhSach.FlatStyle = FlatStyle.Flat;
            btnLamMoiDanhSach.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLamMoiDanhSach.ForeColor = Color.FromArgb(65, 48, 39);
            btnLamMoiDanhSach.Location = new Point(629, 62);
            btnLamMoiDanhSach.Name = "btnLamMoiDanhSach";
            btnLamMoiDanhSach.Size = new Size(90, 30);
            btnLamMoiDanhSach.TabIndex = 5;
            btnLamMoiDanhSach.Text = "Làm mới";
            btnLamMoiDanhSach.UseVisualStyleBackColor = false;
            // 
            // cboLocMon
            // 
            cboLocMon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cboLocMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocMon.FormattingEnabled = true;
            cboLocMon.Location = new Point(466, 63);
            cboLocMon.Name = "cboLocMon";
            cboLocMon.Size = new Size(156, 28);
            cboLocMon.TabIndex = 4;
            // 
            // lblLocMon
            // 
            lblLocMon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLocMon.AutoSize = true;
            lblLocMon.ForeColor = Color.FromArgb(88, 72, 62);
            lblLocMon.Location = new Point(398, 66);
            lblLocMon.Name = "lblLocMon";
            lblLocMon.Size = new Size(66, 20);
            lblLocMon.TabIndex = 3;
            lblLocMon.Text = "Lọc món";
            // 
            // txtTimKiem
            // 
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
            txtTimKiem.Location = new Point(83, 64);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Tên món, nguyên liệu, mã món, mã nguyên liệu...";
            txtTimKiem.Size = new Size(306, 27);
            txtTimKiem.TabIndex = 2;
            // 
            // lblTimKiem
            // 
            lblTimKiem.AutoSize = true;
            lblTimKiem.ForeColor = Color.FromArgb(88, 72, 62);
            lblTimKiem.Location = new Point(14, 66);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(70, 20);
            lblTimKiem.TabIndex = 1;
            lblTimKiem.Text = "Tìm kiếm";
            // 
            // lblDanhSachCongThucTitle
            // 
            lblDanhSachCongThucTitle.AutoSize = true;
            lblDanhSachCongThucTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblDanhSachCongThucTitle.ForeColor = Color.FromArgb(62, 45, 36);
            lblDanhSachCongThucTitle.Location = new Point(14, 16);
            lblDanhSachCongThucTitle.Name = "lblDanhSachCongThucTitle";
            lblDanhSachCongThucTitle.Size = new Size(192, 25);
            lblDanhSachCongThucTitle.TabIndex = 0;
            lblDanhSachCongThucTitle.Text = "Danh sách công thức";
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
            lblPageTitle.Size = new Size(305, 37);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Quản lý công thức món";
            // 
            // frmCongThuc
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1364, 821);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            MinimumSize = new Size(1220, 720);
            Name = "frmCongThuc";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý công thức món";
            panelSidebar.ResumeLayout(false);
            flowSidebarMenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelMain.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            tableMain.ResumeLayout(false);
            tableStats.ResumeLayout(false);
            cardTongCongThuc.ResumeLayout(false);
            cardTongCongThuc.PerformLayout();
            cardMonCoCongThuc.ResumeLayout(false);
            cardMonCoCongThuc.PerformLayout();
            cardNguyenLieuThamGia.ResumeLayout(false);
            cardNguyenLieuThamGia.PerformLayout();
            cardCongThucCanhBao.ResumeLayout(false);
            cardCongThucCanhBao.PerformLayout();
            tableCenter.ResumeLayout(false);
            panelThongTinCongThuc.ResumeLayout(false);
            panelThongTinCongThuc.PerformLayout();
            panelDanhSachCongThuc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhSachCongThuc).EndInit();
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
        private Button btnCongThuc;
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
        private Panel cardTongCongThuc;
        private Label lblTongCongThucValue;
        private Label lblTongCongThucTitle;
        private Label lblTongCongThucIcon;
        private Panel cardMonCoCongThuc;
        private Label lblMonCoCongThucValue;
        private Label lblMonCoCongThucTitle;
        private Label lblMonCoCongThucIcon;
        private Panel cardNguyenLieuThamGia;
        private Label lblNguyenLieuThamGiaValue;
        private Label lblNguyenLieuThamGiaTitle;
        private Label lblNguyenLieuThamGiaIcon;
        private Panel cardCongThucCanhBao;
        private Label lblCongThucCanhBaoValue;
        private Label lblCongThucCanhBaoTitle;
        private Label lblCongThucCanhBaoIcon;
        private TableLayoutPanel tableCenter;
        private Panel panelThongTinCongThuc;
        private Label lblThongTinCongThucTitle;
        private Label lblMaMon;
        private TextBox txtMaMon;
        private Label lblMon;
        private ComboBox cboMon;
        private Label lblMaNguyenLieu;
        private TextBox txtMaNguyenLieu;
        private Label lblNguyenLieu;
        private ComboBox cboNguyenLieu;
        private Label lblDinhLuong;
        private TextBox txtDinhLuong;
        private Label lblDonViTinh;
        private TextBox txtDonViTinh;
        private Label lblSoLuongTon;
        private TextBox txtSoLuongTon;
        private Label lblTrangThaiTon;
        private TextBox txtTrangThaiTon;
        private Label lblGhiChuHint;
        private Button btnThemCongThuc;
        private Button btnCapNhatCongThuc;
        private Button btnXoaCongThuc;
        private Button btnLamMoiThongTin;
        private Panel panelDanhSachCongThuc;
        private DataGridView dgvDanhSachCongThuc;
        private Panel panelDanhSachHeader;
        private Button btnLamMoiDanhSach;
        private ComboBox cboLocMon;
        private Label lblLocMon;
        private TextBox txtTimKiem;
        private Label lblTimKiem;
        private Label lblDanhSachCongThucTitle;
        private Panel panelTopbar;
        private Label lblPageTitle;
        private DataGridViewTextBoxColumn colMonID;
        private DataGridViewTextBoxColumn colTenMon;
        private DataGridViewTextBoxColumn colNguyenLieuID;
        private DataGridViewTextBoxColumn colTenNguyenLieu;
        private DataGridViewTextBoxColumn colSoLuong;
        private DataGridViewTextBoxColumn colDonViTinh;
        private DataGridViewTextBoxColumn colSoLuongTon;
        private DataGridViewTextBoxColumn colTrangThaiTon;
    }
}