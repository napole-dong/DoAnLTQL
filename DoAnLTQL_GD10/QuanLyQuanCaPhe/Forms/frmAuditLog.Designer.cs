namespace QuanLyQuanCaPhe.Forms
{
    partial class frmAuditLog
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
            btnQuanLyKho = new Button();
            btnCongThuc = new Button();
            btnHoaDon = new Button();
            btnKhachHang = new Button();
            btnNhanVien = new Button();
            btnThongKe = new Button();
            btnAuditLog = new Button();
            panelLogo = new Panel();
            lblLogoSub = new Label();
            lblLogo = new Label();
            panelMain = new Panel();
            panelContent = new Panel();
            tableMain = new TableLayoutPanel();
            tableStats = new TableLayoutPanel();
            cardTongLog = new Panel();
            lblTongLogValue = new Label();
            lblTongLogTitle = new Label();
            lblTongLogIcon = new Label();
            cardQuanTrong = new Panel();
            lblQuanTrongValue = new Label();
            lblQuanTrongTitle = new Label();
            lblQuanTrongIcon = new Label();
            cardNguoiDung = new Panel();
            lblNguoiDungValue = new Label();
            lblNguoiDungTitle = new Label();
            lblNguoiDungIcon = new Label();
            cardHomNay = new Panel();
            lblHomNayValue = new Label();
            lblHomNayTitle = new Label();
            lblHomNayIcon = new Label();
            tableCenter = new TableLayoutPanel();
            panelBoLoc = new Panel();
            btnLamMoi = new Button();
            btnApDung = new Button();
            dtDenNgay = new DateTimePicker();
            lblDenNgay = new Label();
            dtTuNgay = new DateTimePicker();
            lblTuNgay = new Label();
            txtNguoiDung = new TextBox();
            lblNguoiDung = new Label();
            cboBang = new ComboBox();
            lblBang = new Label();
            cboHanhDong = new ComboBox();
            lblHanhDong = new Label();
            cboMucDo = new ComboBox();
            lblMucDo = new Label();
            lblBoLocTitle = new Label();
            panelDanhSach = new Panel();
            dgvAuditLog = new DataGridView();
            colThoiGian = new DataGridViewTextBoxColumn();
            colNguoiThucHien = new DataGridViewTextBoxColumn();
            colHanhDong = new DataGridViewTextBoxColumn();
            colBangDuLieu = new DataGridViewTextBoxColumn();
            colDoiTuong = new DataGridViewTextBoxColumn();
            colChiTiet = new DataGridViewTextBoxColumn();
            colDiaChiIP = new DataGridViewTextBoxColumn();
            panelDanhSachHeader = new Panel();
            btnXuatExcel = new Button();
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
            cardTongLog.SuspendLayout();
            cardQuanTrong.SuspendLayout();
            cardNguoiDung.SuspendLayout();
            cardHomNay.SuspendLayout();
            tableCenter.SuspendLayout();
            panelBoLoc.SuspendLayout();
            panelDanhSach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuditLog).BeginInit();
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
            flowSidebarMenu.Controls.Add(btnQuanLyKho);
            flowSidebarMenu.Controls.Add(btnCongThuc);
            flowSidebarMenu.Controls.Add(btnHoaDon);
            flowSidebarMenu.Controls.Add(btnKhachHang);
            flowSidebarMenu.Controls.Add(btnNhanVien);
            flowSidebarMenu.Controls.Add(btnThongKe);
            flowSidebarMenu.Controls.Add(btnAuditLog);
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
            // btnCongThuc
            // 
            btnCongThuc.FlatAppearance.BorderSize = 0;
            btnCongThuc.FlatStyle = FlatStyle.Flat;
            btnCongThuc.Font = new Font("Segoe UI", 10F);
            btnCongThuc.ForeColor = Color.Gainsboro;
            btnCongThuc.Location = new Point(0, 206);
            btnCongThuc.Margin = new Padding(0);
            btnCongThuc.Name = "btnCongThuc";
            btnCongThuc.Padding = new Padding(20, 0, 0, 0);
            btnCongThuc.Size = new Size(230, 48);
            btnCongThuc.TabIndex = 4;
            btnCongThuc.Text = "\U0001f9ea  Công thức";
            btnCongThuc.TextAlign = ContentAlignment.MiddleLeft;
            btnCongThuc.UseVisualStyleBackColor = true;
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
            // btnKhachHang
            // 
            btnKhachHang.FlatAppearance.BorderSize = 0;
            btnKhachHang.FlatStyle = FlatStyle.Flat;
            btnKhachHang.Font = new Font("Segoe UI", 10F);
            btnKhachHang.ForeColor = Color.Gainsboro;
            btnKhachHang.Location = new Point(0, 302);
            btnKhachHang.Margin = new Padding(0);
            btnKhachHang.Name = "btnKhachHang";
            btnKhachHang.Padding = new Padding(20, 0, 0, 0);
            btnKhachHang.Size = new Size(230, 48);
            btnKhachHang.TabIndex = 6;
            btnKhachHang.Text = "👤  Khách hàng";
            btnKhachHang.TextAlign = ContentAlignment.MiddleLeft;
            btnKhachHang.UseVisualStyleBackColor = true;
            // 
            // btnNhanVien
            // 
            btnNhanVien.FlatAppearance.BorderSize = 0;
            btnNhanVien.FlatStyle = FlatStyle.Flat;
            btnNhanVien.Font = new Font("Segoe UI", 10F);
            btnNhanVien.ForeColor = Color.Gainsboro;
            btnNhanVien.Location = new Point(0, 350);
            btnNhanVien.Margin = new Padding(0);
            btnNhanVien.Name = "btnNhanVien";
            btnNhanVien.Padding = new Padding(20, 0, 0, 0);
            btnNhanVien.Size = new Size(230, 48);
            btnNhanVien.TabIndex = 7;
            btnNhanVien.Text = "\U0001f9d1‍💼  Nhân viên";
            btnNhanVien.TextAlign = ContentAlignment.MiddleLeft;
            btnNhanVien.UseVisualStyleBackColor = true;
            // 
            // btnThongKe
            // 
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.FlatStyle = FlatStyle.Flat;
            btnThongKe.Font = new Font("Segoe UI", 10F);
            btnThongKe.ForeColor = Color.Gainsboro;
            btnThongKe.Location = new Point(0, 398);
            btnThongKe.Margin = new Padding(0);
            btnThongKe.Name = "btnThongKe";
            btnThongKe.Padding = new Padding(20, 0, 0, 0);
            btnThongKe.Size = new Size(230, 48);
            btnThongKe.TabIndex = 8;
            btnThongKe.Text = "📈  Thống kê";
            btnThongKe.TextAlign = ContentAlignment.MiddleLeft;
            btnThongKe.UseVisualStyleBackColor = true;
            // 
            // btnAuditLog
            // 
            btnAuditLog.BackColor = Color.FromArgb(94, 64, 47);
            btnAuditLog.FlatAppearance.BorderSize = 0;
            btnAuditLog.FlatStyle = FlatStyle.Flat;
            btnAuditLog.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnAuditLog.ForeColor = Color.White;
            btnAuditLog.Location = new Point(0, 446);
            btnAuditLog.Margin = new Padding(0);
            btnAuditLog.Name = "btnAuditLog";
            btnAuditLog.Padding = new Padding(20, 0, 0, 0);
            btnAuditLog.Size = new Size(230, 48);
            btnAuditLog.TabIndex = 9;
            btnAuditLog.Text = "🛡  Audit log";
            btnAuditLog.TextAlign = ContentAlignment.MiddleLeft;
            btnAuditLog.UseVisualStyleBackColor = false;
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
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
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
            tableStats.Controls.Add(cardTongLog, 0, 0);
            tableStats.Controls.Add(cardQuanTrong, 1, 0);
            tableStats.Controls.Add(cardNguoiDung, 2, 0);
            tableStats.Controls.Add(cardHomNay, 3, 0);
            tableStats.Dock = DockStyle.Fill;
            tableStats.Location = new Point(0, 0);
            tableStats.Margin = new Padding(0, 0, 0, 12);
            tableStats.Name = "tableStats";
            tableStats.RowCount = 1;
            tableStats.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableStats.Size = new Size(1090, 106);
            tableStats.TabIndex = 0;
            // 
            // cardTongLog
            // 
            cardTongLog.BackColor = Color.FromArgb(237, 247, 243);
            cardTongLog.Controls.Add(lblTongLogValue);
            cardTongLog.Controls.Add(lblTongLogTitle);
            cardTongLog.Controls.Add(lblTongLogIcon);
            cardTongLog.Dock = DockStyle.Fill;
            cardTongLog.Location = new Point(0, 0);
            cardTongLog.Margin = new Padding(0, 0, 8, 0);
            cardTongLog.Name = "cardTongLog";
            cardTongLog.Padding = new Padding(16, 12, 16, 12);
            cardTongLog.Size = new Size(264, 106);
            cardTongLog.TabIndex = 0;
            // 
            // lblTongLogValue
            // 
            lblTongLogValue.AutoSize = true;
            lblTongLogValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTongLogValue.ForeColor = Color.FromArgb(34, 111, 92);
            lblTongLogValue.Location = new Point(18, 61);
            lblTongLogValue.Name = "lblTongLogValue";
            lblTongLogValue.Size = new Size(28, 37);
            lblTongLogValue.TabIndex = 2;
            lblTongLogValue.Text = "-";
            // 
            // lblTongLogTitle
            // 
            lblTongLogTitle.AutoSize = true;
            lblTongLogTitle.Font = new Font("Segoe UI", 10F);
            lblTongLogTitle.ForeColor = Color.FromArgb(90, 106, 101);
            lblTongLogTitle.Location = new Point(18, 36);
            lblTongLogTitle.Name = "lblTongLogTitle";
            lblTongLogTitle.Size = new Size(112, 23);
            lblTongLogTitle.TabIndex = 1;
            lblTongLogTitle.Text = "Tổng bản ghi";
            // 
            // lblTongLogIcon
            // 
            lblTongLogIcon.AutoSize = true;
            lblTongLogIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblTongLogIcon.Location = new Point(16, 8);
            lblTongLogIcon.Name = "lblTongLogIcon";
            lblTongLogIcon.Size = new Size(39, 27);
            lblTongLogIcon.TabIndex = 0;
            lblTongLogIcon.Text = "📚";
            // 
            // cardQuanTrong
            // 
            cardQuanTrong.BackColor = Color.FromArgb(255, 238, 240);
            cardQuanTrong.Controls.Add(lblQuanTrongValue);
            cardQuanTrong.Controls.Add(lblQuanTrongTitle);
            cardQuanTrong.Controls.Add(lblQuanTrongIcon);
            cardQuanTrong.Dock = DockStyle.Fill;
            cardQuanTrong.Location = new Point(272, 0);
            cardQuanTrong.Margin = new Padding(0, 0, 8, 0);
            cardQuanTrong.Name = "cardQuanTrong";
            cardQuanTrong.Padding = new Padding(16, 12, 16, 12);
            cardQuanTrong.Size = new Size(264, 106);
            cardQuanTrong.TabIndex = 1;
            // 
            // lblQuanTrongValue
            // 
            lblQuanTrongValue.AutoSize = true;
            lblQuanTrongValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblQuanTrongValue.ForeColor = Color.FromArgb(168, 61, 75);
            lblQuanTrongValue.Location = new Point(18, 61);
            lblQuanTrongValue.Name = "lblQuanTrongValue";
            lblQuanTrongValue.Size = new Size(28, 37);
            lblQuanTrongValue.TabIndex = 2;
            lblQuanTrongValue.Text = "-";
            // 
            // lblQuanTrongTitle
            // 
            lblQuanTrongTitle.AutoSize = true;
            lblQuanTrongTitle.Font = new Font("Segoe UI", 10F);
            lblQuanTrongTitle.ForeColor = Color.FromArgb(136, 86, 96);
            lblQuanTrongTitle.Location = new Point(18, 36);
            lblQuanTrongTitle.Name = "lblQuanTrongTitle";
            lblQuanTrongTitle.Size = new Size(156, 23);
            lblQuanTrongTitle.TabIndex = 1;
            lblQuanTrongTitle.Text = "Sự kiện quan trọng";
            // 
            // lblQuanTrongIcon
            // 
            lblQuanTrongIcon.AutoSize = true;
            lblQuanTrongIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblQuanTrongIcon.Location = new Point(16, 8);
            lblQuanTrongIcon.Name = "lblQuanTrongIcon";
            lblQuanTrongIcon.Size = new Size(39, 27);
            lblQuanTrongIcon.TabIndex = 0;
            lblQuanTrongIcon.Text = "🚨";
            // 
            // cardNguoiDung
            // 
            cardNguoiDung.BackColor = Color.FromArgb(235, 244, 255);
            cardNguoiDung.Controls.Add(lblNguoiDungValue);
            cardNguoiDung.Controls.Add(lblNguoiDungTitle);
            cardNguoiDung.Controls.Add(lblNguoiDungIcon);
            cardNguoiDung.Dock = DockStyle.Fill;
            cardNguoiDung.Location = new Point(544, 0);
            cardNguoiDung.Margin = new Padding(0, 0, 8, 0);
            cardNguoiDung.Name = "cardNguoiDung";
            cardNguoiDung.Padding = new Padding(16, 12, 16, 12);
            cardNguoiDung.Size = new Size(264, 106);
            cardNguoiDung.TabIndex = 2;
            // 
            // lblNguoiDungValue
            // 
            lblNguoiDungValue.AutoSize = true;
            lblNguoiDungValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblNguoiDungValue.ForeColor = Color.FromArgb(42, 96, 164);
            lblNguoiDungValue.Location = new Point(18, 61);
            lblNguoiDungValue.Name = "lblNguoiDungValue";
            lblNguoiDungValue.Size = new Size(28, 37);
            lblNguoiDungValue.TabIndex = 2;
            lblNguoiDungValue.Text = "-";
            // 
            // lblNguoiDungTitle
            // 
            lblNguoiDungTitle.AutoSize = true;
            lblNguoiDungTitle.Font = new Font("Segoe UI", 10F);
            lblNguoiDungTitle.ForeColor = Color.FromArgb(74, 107, 141);
            lblNguoiDungTitle.Location = new Point(18, 36);
            lblNguoiDungTitle.Name = "lblNguoiDungTitle";
            lblNguoiDungTitle.Size = new Size(187, 23);
            lblNguoiDungTitle.TabIndex = 1;
            lblNguoiDungTitle.Text = "Người dùng hoạt động";
            // 
            // lblNguoiDungIcon
            // 
            lblNguoiDungIcon.AutoSize = true;
            lblNguoiDungIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblNguoiDungIcon.Location = new Point(16, 8);
            lblNguoiDungIcon.Name = "lblNguoiDungIcon";
            lblNguoiDungIcon.Size = new Size(39, 27);
            lblNguoiDungIcon.TabIndex = 0;
            lblNguoiDungIcon.Text = "👤";
            // 
            // cardHomNay
            // 
            cardHomNay.BackColor = Color.FromArgb(255, 247, 235);
            cardHomNay.Controls.Add(lblHomNayValue);
            cardHomNay.Controls.Add(lblHomNayTitle);
            cardHomNay.Controls.Add(lblHomNayIcon);
            cardHomNay.Dock = DockStyle.Fill;
            cardHomNay.Location = new Point(816, 0);
            cardHomNay.Margin = new Padding(0);
            cardHomNay.Name = "cardHomNay";
            cardHomNay.Padding = new Padding(16, 12, 16, 12);
            cardHomNay.Size = new Size(274, 106);
            cardHomNay.TabIndex = 3;
            // 
            // lblHomNayValue
            // 
            lblHomNayValue.AutoSize = true;
            lblHomNayValue.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblHomNayValue.ForeColor = Color.FromArgb(161, 97, 23);
            lblHomNayValue.Location = new Point(18, 61);
            lblHomNayValue.Name = "lblHomNayValue";
            lblHomNayValue.Size = new Size(28, 37);
            lblHomNayValue.TabIndex = 2;
            lblHomNayValue.Text = "-";
            // 
            // lblHomNayTitle
            // 
            lblHomNayTitle.AutoSize = true;
            lblHomNayTitle.Font = new Font("Segoe UI", 10F);
            lblHomNayTitle.ForeColor = Color.FromArgb(140, 112, 73);
            lblHomNayTitle.Location = new Point(18, 36);
            lblHomNayTitle.Name = "lblHomNayTitle";
            lblHomNayTitle.Size = new Size(153, 23);
            lblHomNayTitle.TabIndex = 1;
            lblHomNayTitle.Text = "Phát sinh hôm nay";
            // 
            // lblHomNayIcon
            // 
            lblHomNayIcon.AutoSize = true;
            lblHomNayIcon.Font = new Font("Segoe UI Emoji", 12F);
            lblHomNayIcon.Location = new Point(16, 8);
            lblHomNayIcon.Name = "lblHomNayIcon";
            lblHomNayIcon.Size = new Size(39, 27);
            lblHomNayIcon.TabIndex = 0;
            lblHomNayIcon.Text = "📅";
            // 
            // tableCenter
            // 
            tableCenter.ColumnCount = 2;
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 330F));
            tableCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableCenter.Controls.Add(panelBoLoc, 0, 0);
            tableCenter.Controls.Add(panelDanhSach, 1, 0);
            tableCenter.Dock = DockStyle.Fill;
            tableCenter.Location = new Point(0, 118);
            tableCenter.Margin = new Padding(0);
            tableCenter.Name = "tableCenter";
            tableCenter.RowCount = 1;
            tableCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableCenter.Size = new Size(1090, 524);
            tableCenter.TabIndex = 1;
            // 
            // panelBoLoc
            // 
            panelBoLoc.BackColor = Color.White;
            panelBoLoc.Controls.Add(btnLamMoi);
            panelBoLoc.Controls.Add(btnApDung);
            panelBoLoc.Controls.Add(dtDenNgay);
            panelBoLoc.Controls.Add(lblDenNgay);
            panelBoLoc.Controls.Add(dtTuNgay);
            panelBoLoc.Controls.Add(lblTuNgay);
            panelBoLoc.Controls.Add(txtNguoiDung);
            panelBoLoc.Controls.Add(lblNguoiDung);
            panelBoLoc.Controls.Add(cboBang);
            panelBoLoc.Controls.Add(lblBang);
            panelBoLoc.Controls.Add(cboHanhDong);
            panelBoLoc.Controls.Add(lblHanhDong);
            panelBoLoc.Controls.Add(cboMucDo);
            panelBoLoc.Controls.Add(lblMucDo);
            panelBoLoc.Controls.Add(lblBoLocTitle);
            panelBoLoc.Dock = DockStyle.Fill;
            panelBoLoc.Location = new Point(0, 0);
            panelBoLoc.Margin = new Padding(0, 0, 14, 0);
            panelBoLoc.Name = "panelBoLoc";
            panelBoLoc.Padding = new Padding(18, 16, 18, 16);
            panelBoLoc.Size = new Size(316, 524);
            panelBoLoc.TabIndex = 0;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(154, 106, 71);
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(164, 439);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(132, 36);
            btnLamMoi.TabIndex = 14;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // btnApDung
            // 
            btnApDung.BackColor = Color.FromArgb(25, 135, 84);
            btnApDung.FlatAppearance.BorderSize = 0;
            btnApDung.FlatStyle = FlatStyle.Flat;
            btnApDung.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnApDung.ForeColor = Color.White;
            btnApDung.Location = new Point(21, 439);
            btnApDung.Name = "btnApDung";
            btnApDung.Size = new Size(132, 36);
            btnApDung.TabIndex = 13;
            btnApDung.Text = "Áp dụng";
            btnApDung.UseVisualStyleBackColor = false;
            // 
            // dtDenNgay
            // 
            dtDenNgay.Format = DateTimePickerFormat.Short;
            dtDenNgay.Location = new Point(21, 381);
            dtDenNgay.Name = "dtDenNgay";
            dtDenNgay.Size = new Size(275, 27);
            dtDenNgay.TabIndex = 12;
            // 
            // lblDenNgay
            // 
            lblDenNgay.AutoSize = true;
            lblDenNgay.ForeColor = Color.FromArgb(88, 72, 62);
            lblDenNgay.Location = new Point(21, 357);
            lblDenNgay.Name = "lblDenNgay";
            lblDenNgay.Size = new Size(72, 20);
            lblDenNgay.TabIndex = 11;
            lblDenNgay.Text = "Đến ngày";
            // 
            // dtTuNgay
            // 
            dtTuNgay.Format = DateTimePickerFormat.Short;
            dtTuNgay.Location = new Point(21, 325);
            dtTuNgay.Name = "dtTuNgay";
            dtTuNgay.Size = new Size(275, 27);
            dtTuNgay.TabIndex = 10;
            // 
            // lblTuNgay
            // 
            lblTuNgay.AutoSize = true;
            lblTuNgay.ForeColor = Color.FromArgb(88, 72, 62);
            lblTuNgay.Location = new Point(21, 301);
            lblTuNgay.Name = "lblTuNgay";
            lblTuNgay.Size = new Size(62, 20);
            lblTuNgay.TabIndex = 9;
            lblTuNgay.Text = "Từ ngày";
            // 
            // txtNguoiDung
            // 
            txtNguoiDung.BorderStyle = BorderStyle.FixedSingle;
            txtNguoiDung.Location = new Point(21, 270);
            txtNguoiDung.Name = "txtNguoiDung";
            txtNguoiDung.Size = new Size(275, 27);
            txtNguoiDung.TabIndex = 8;
            // 
            // lblNguoiDung
            // 
            lblNguoiDung.AutoSize = true;
            lblNguoiDung.ForeColor = Color.FromArgb(88, 72, 62);
            lblNguoiDung.Location = new Point(21, 246);
            lblNguoiDung.Name = "lblNguoiDung";
            lblNguoiDung.Size = new Size(89, 20);
            lblNguoiDung.TabIndex = 7;
            lblNguoiDung.Text = "Người dùng";
            // 
            // cboBang
            // 
            cboBang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBang.FormattingEnabled = true;
            cboBang.Items.AddRange(new object[] { "Tất cả", "HoaDon", "KhachHang", "Mon", "NguyenLieu", "NhanVien", "Ban" });
            cboBang.Location = new Point(21, 215);
            cboBang.Name = "cboBang";
            cboBang.Size = new Size(275, 28);
            cboBang.TabIndex = 6;
            // 
            // lblBang
            // 
            lblBang.AutoSize = true;
            lblBang.ForeColor = Color.FromArgb(88, 72, 62);
            lblBang.Location = new Point(21, 191);
            lblBang.Name = "lblBang";
            lblBang.Size = new Size(93, 20);
            lblBang.TabIndex = 5;
            lblBang.Text = "Bảng dữ liệu";
            // 
            // cboHanhDong
            // 
            cboHanhDong.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHanhDong.FormattingEnabled = true;
            cboHanhDong.Items.AddRange(new object[] { "Tất cả", "CREATE", "UPDATE", "DELETE", "LOGIN", "LOGOUT" });
            cboHanhDong.Location = new Point(21, 159);
            cboHanhDong.Name = "cboHanhDong";
            cboHanhDong.Size = new Size(275, 28);
            cboHanhDong.TabIndex = 4;
            // 
            // lblHanhDong
            // 
            lblHanhDong.AutoSize = true;
            lblHanhDong.ForeColor = Color.FromArgb(88, 72, 62);
            lblHanhDong.Location = new Point(21, 135);
            lblHanhDong.Name = "lblHanhDong";
            lblHanhDong.Size = new Size(83, 20);
            lblHanhDong.TabIndex = 3;
            lblHanhDong.Text = "Hành động";
            // 
            // cboMucDo
            // 
            cboMucDo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMucDo.FormattingEnabled = true;
            cboMucDo.Items.AddRange(new object[] { "Tất cả", "Info", "Warning", "Error", "Critical" });
            cboMucDo.Location = new Point(21, 103);
            cboMucDo.Name = "cboMucDo";
            cboMucDo.Size = new Size(275, 28);
            cboMucDo.TabIndex = 2;
            // 
            // lblMucDo
            // 
            lblMucDo.AutoSize = true;
            lblMucDo.ForeColor = Color.FromArgb(88, 72, 62);
            lblMucDo.Location = new Point(21, 79);
            lblMucDo.Name = "lblMucDo";
            lblMucDo.Size = new Size(60, 20);
            lblMucDo.TabIndex = 1;
            lblMucDo.Text = "Mức độ";
            // 
            // lblBoLocTitle
            // 
            lblBoLocTitle.AutoSize = true;
            lblBoLocTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblBoLocTitle.ForeColor = Color.FromArgb(62, 45, 36);
            lblBoLocTitle.Location = new Point(20, 20);
            lblBoLocTitle.Name = "lblBoLocTitle";
            lblBoLocTitle.Size = new Size(133, 25);
            lblBoLocTitle.TabIndex = 0;
            lblBoLocTitle.Text = "Bộ lọc nhật ký";
            // 
            // panelDanhSach
            // 
            panelDanhSach.BackColor = Color.White;
            panelDanhSach.Controls.Add(dgvAuditLog);
            panelDanhSach.Controls.Add(panelDanhSachHeader);
            panelDanhSach.Dock = DockStyle.Fill;
            panelDanhSach.Location = new Point(330, 0);
            panelDanhSach.Margin = new Padding(0);
            panelDanhSach.Name = "panelDanhSach";
            panelDanhSach.Size = new Size(760, 524);
            panelDanhSach.TabIndex = 1;
            // 
            // dgvAuditLog
            // 
            dgvAuditLog.AllowUserToAddRows = false;
            dgvAuditLog.AllowUserToDeleteRows = false;
            dgvAuditLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAuditLog.BackgroundColor = Color.White;
            dgvAuditLog.BorderStyle = BorderStyle.None;
            dgvAuditLog.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAuditLog.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(246, 239, 232);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(86, 62, 46);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(246, 239, 232);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(86, 62, 46);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvAuditLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvAuditLog.ColumnHeadersHeight = 42;
            dgvAuditLog.Columns.AddRange(new DataGridViewColumn[] { colThoiGian, colNguoiThucHien, colHanhDong, colBangDuLieu, colDoiTuong, colChiTiet, colDiaChiIP });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(58, 58, 58);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(244, 233, 220);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(58, 58, 58);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvAuditLog.DefaultCellStyle = dataGridViewCellStyle2;
            dgvAuditLog.Dock = DockStyle.Fill;
            dgvAuditLog.EnableHeadersVisualStyles = false;
            dgvAuditLog.GridColor = Color.FromArgb(237, 229, 221);
            dgvAuditLog.Location = new Point(0, 90);
            dgvAuditLog.MultiSelect = false;
            dgvAuditLog.Name = "dgvAuditLog";
            dgvAuditLog.ReadOnly = true;
            dgvAuditLog.RowHeadersVisible = false;
            dgvAuditLog.RowHeadersWidth = 51;
            dgvAuditLog.RowTemplate.Height = 34;
            dgvAuditLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuditLog.Size = new Size(760, 434);
            dgvAuditLog.TabIndex = 1;
            // 
            // colThoiGian
            // 
            colThoiGian.FillWeight = 105F;
            colThoiGian.HeaderText = "Thời gian";
            colThoiGian.MinimumWidth = 6;
            colThoiGian.Name = "colThoiGian";
            colThoiGian.ReadOnly = true;
            // 
            // colNguoiThucHien
            // 
            colNguoiThucHien.FillWeight = 95F;
            colNguoiThucHien.HeaderText = "Người dùng";
            colNguoiThucHien.MinimumWidth = 6;
            colNguoiThucHien.Name = "colNguoiThucHien";
            colNguoiThucHien.ReadOnly = true;
            // 
            // colHanhDong
            // 
            colHanhDong.FillWeight = 80F;
            colHanhDong.HeaderText = "Hành động";
            colHanhDong.MinimumWidth = 6;
            colHanhDong.Name = "colHanhDong";
            colHanhDong.ReadOnly = true;
            // 
            // colBangDuLieu
            // 
            colBangDuLieu.FillWeight = 90F;
            colBangDuLieu.HeaderText = "Bảng";
            colBangDuLieu.MinimumWidth = 6;
            colBangDuLieu.Name = "colBangDuLieu";
            colBangDuLieu.ReadOnly = true;
            // 
            // colDoiTuong
            // 
            colDoiTuong.FillWeight = 85F;
            colDoiTuong.HeaderText = "Đối tượng";
            colDoiTuong.MinimumWidth = 6;
            colDoiTuong.Name = "colDoiTuong";
            colDoiTuong.ReadOnly = true;
            // 
            // colChiTiet
            // 
            colChiTiet.FillWeight = 210F;
            colChiTiet.HeaderText = "Chi tiết";
            colChiTiet.MinimumWidth = 6;
            colChiTiet.Name = "colChiTiet";
            colChiTiet.ReadOnly = true;
            // 
            // colDiaChiIP
            // 
            colDiaChiIP.FillWeight = 95F;
            colDiaChiIP.HeaderText = "IP";
            colDiaChiIP.MinimumWidth = 6;
            colDiaChiIP.Name = "colDiaChiIP";
            colDiaChiIP.ReadOnly = true;
            // 
            // panelDanhSachHeader
            // 
            panelDanhSachHeader.BackColor = Color.White;
            panelDanhSachHeader.Controls.Add(btnXuatExcel);
            panelDanhSachHeader.Controls.Add(txtTimKiem);
            panelDanhSachHeader.Controls.Add(lblTimKiem);
            panelDanhSachHeader.Controls.Add(lblDanhSachTitle);
            panelDanhSachHeader.Dock = DockStyle.Top;
            panelDanhSachHeader.Location = new Point(0, 0);
            panelDanhSachHeader.Name = "panelDanhSachHeader";
            panelDanhSachHeader.Size = new Size(760, 90);
            panelDanhSachHeader.TabIndex = 0;
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnXuatExcel.BackColor = Color.FromArgb(94, 64, 47);
            btnXuatExcel.FlatAppearance.BorderSize = 0;
            btnXuatExcel.FlatStyle = FlatStyle.Flat;
            btnXuatExcel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnXuatExcel.ForeColor = Color.White;
            btnXuatExcel.Location = new Point(648, 48);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.Size = new Size(98, 32);
            btnXuatExcel.TabIndex = 3;
            btnXuatExcel.Text = "Xuất";
            btnXuatExcel.UseVisualStyleBackColor = false;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTimKiem.BorderStyle = BorderStyle.FixedSingle;
            txtTimKiem.Location = new Point(102, 50);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(533, 27);
            txtTimKiem.TabIndex = 2;
            // 
            // lblTimKiem
            // 
            lblTimKiem.AutoSize = true;
            lblTimKiem.ForeColor = Color.FromArgb(88, 72, 62);
            lblTimKiem.Location = new Point(14, 53);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(73, 20);
            lblTimKiem.TabIndex = 1;
            lblTimKiem.Text = "Tìm kiếm:";
            // 
            // lblDanhSachTitle
            // 
            lblDanhSachTitle.AutoSize = true;
            lblDanhSachTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblDanhSachTitle.ForeColor = Color.FromArgb(62, 45, 36);
            lblDanhSachTitle.Location = new Point(12, 15);
            lblDanhSachTitle.Name = "lblDanhSachTitle";
            lblDanhSachTitle.Size = new Size(182, 25);
            lblDanhSachTitle.TabIndex = 0;
            lblDanhSachTitle.Text = "Danh sách audit log";
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
            lblPageTitle.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(68, 47, 35);
            lblPageTitle.Location = new Point(24, 22);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(214, 35);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Lịch sử Audit Log";
            // 
            // frmAuditLog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 242, 236);
            ClientSize = new Size(1364, 760);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmAuditLog";
            Text = "frmAuditLog";
            panelSidebar.ResumeLayout(false);
            flowSidebarMenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelMain.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            tableMain.ResumeLayout(false);
            tableStats.ResumeLayout(false);
            cardTongLog.ResumeLayout(false);
            cardTongLog.PerformLayout();
            cardQuanTrong.ResumeLayout(false);
            cardQuanTrong.PerformLayout();
            cardNguoiDung.ResumeLayout(false);
            cardNguoiDung.PerformLayout();
            cardHomNay.ResumeLayout(false);
            cardHomNay.PerformLayout();
            tableCenter.ResumeLayout(false);
            panelBoLoc.ResumeLayout(false);
            panelBoLoc.PerformLayout();
            panelDanhSach.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAuditLog).EndInit();
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
        private Button btnCongThuc;
        private Button btnHoaDon;
        private Button btnKhachHang;
        private Button btnNhanVien;
        private Button btnThongKe;
        private Button btnAuditLog;
        private Panel panelLogo;
        private Label lblLogoSub;
        private Label lblLogo;
        private Panel panelMain;
        private Panel panelTopbar;
        private Label lblPageTitle;
        private Panel panelContent;
        private TableLayoutPanel tableMain;
        private TableLayoutPanel tableStats;
        private Panel cardTongLog;
        private Label lblTongLogValue;
        private Label lblTongLogTitle;
        private Label lblTongLogIcon;
        private Panel cardQuanTrong;
        private Label lblQuanTrongValue;
        private Label lblQuanTrongTitle;
        private Label lblQuanTrongIcon;
        private Panel cardNguoiDung;
        private Label lblNguoiDungValue;
        private Label lblNguoiDungTitle;
        private Label lblNguoiDungIcon;
        private Panel cardHomNay;
        private Label lblHomNayValue;
        private Label lblHomNayTitle;
        private Label lblHomNayIcon;
        private TableLayoutPanel tableCenter;
        private Panel panelBoLoc;
        private Button btnLamMoi;
        private Button btnApDung;
        private DateTimePicker dtDenNgay;
        private Label lblDenNgay;
        private DateTimePicker dtTuNgay;
        private Label lblTuNgay;
        private TextBox txtNguoiDung;
        private Label lblNguoiDung;
        private ComboBox cboBang;
        private Label lblBang;
        private ComboBox cboHanhDong;
        private Label lblHanhDong;
        private ComboBox cboMucDo;
        private Label lblMucDo;
        private Label lblBoLocTitle;
        private Panel panelDanhSach;
        private DataGridView dgvAuditLog;
        private Panel panelDanhSachHeader;
        private Button btnXuatExcel;
        private TextBox txtTimKiem;
        private Label lblTimKiem;
        private Label lblDanhSachTitle;
        private DataGridViewTextBoxColumn colThoiGian;
        private DataGridViewTextBoxColumn colNguoiThucHien;
        private DataGridViewTextBoxColumn colHanhDong;
        private DataGridViewTextBoxColumn colBangDuLieu;
        private DataGridViewTextBoxColumn colDoiTuong;
        private DataGridViewTextBoxColumn colChiTiet;
        private DataGridViewTextBoxColumn colDiaChiIP;
    }
}