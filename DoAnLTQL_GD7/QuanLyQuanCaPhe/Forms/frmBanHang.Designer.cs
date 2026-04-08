namespace QuanLyQuanCaPhe.Forms
{
    partial class frmBanHang
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
            panelOrder = new Panel();
            dgvOrder = new DataGridView();
            colTenMon = new DataGridViewTextBoxColumn();
            colSoLuong = new DataGridViewTextBoxColumn();
            colDonGia = new DataGridViewTextBoxColumn();
            colThanhTien = new DataGridViewTextBoxColumn();
            colXoa = new DataGridViewButtonColumn();
            panelOrderFooter = new Panel();
            btnThanhToan = new Button();
            btnTamTinh = new Button();
            lblTongThanhToanValue = new Label();
            lblTongThanhToan = new Label();
            lblGiamGiaValue = new Label();
            lblGiamGia = new Label();
            lblTamTinhValue = new Label();
            lblTamTinh = new Label();
            panelOrderHeader = new Panel();
            btnGopBan = new Button();
            btnChuyenBan = new Button();
            lblOrderMeta = new Label();
            lblOrderTitle = new Label();
            panelTables = new Panel();
            flowBan = new FlowLayoutPanel();
            btnBan01 = new Button();
            btnBan02 = new Button();
            btnBan03 = new Button();
            btnBan04 = new Button();
            btnBan05 = new Button();
            btnBan06 = new Button();
            panelTablesHeader = new Panel();
            lblThongTinBan = new Label();
            lblTablesTitle = new Label();
            tableWorkArea = new TableLayoutPanel();
            panelMenu = new Panel();
            flowMon = new FlowLayoutPanel();
            panelMon1 = new Panel();
            lblMon1Gia = new Label();
            lblMon1Ten = new Label();
            btnMon1 = new Button();
            panelMon2 = new Panel();
            lblMon2Gia = new Label();
            lblMon2Ten = new Label();
            btnMon2 = new Button();
            panelMenuFilter = new Panel();
            btnTatCa = new Button();
            btnTra = new Button();
            btnDaXay = new Button();
            btnCafe = new Button();
            lblMenuTitle = new Label();
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
            panelOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrder).BeginInit();
            panelOrderFooter.SuspendLayout();
            panelOrderHeader.SuspendLayout();
            panelTables.SuspendLayout();
            flowBan.SuspendLayout();
            panelTablesHeader.SuspendLayout();
            tableWorkArea.SuspendLayout();
            panelMenu.SuspendLayout();
            flowMon.SuspendLayout();
            panelMon1.SuspendLayout();
            panelMon2.SuspendLayout();
            panelMenuFilter.SuspendLayout();
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
            flowSidebarMenu.Controls.Add(btnBanHang);
            flowSidebarMenu.Controls.Add(btnQuanLyBan);
            flowSidebarMenu.Controls.Add(btnQuanLyMon);
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
            // btnBanHang
            // 
            btnBanHang.BackColor = Color.FromArgb(94, 64, 47);
            btnBanHang.FlatAppearance.BorderSize = 0;
            btnBanHang.FlatStyle = FlatStyle.Flat;
            btnBanHang.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnBanHang.ForeColor = Color.White;
            btnBanHang.Location = new Point(0, 14);
            btnBanHang.Margin = new Padding(0);
            btnBanHang.Name = "btnBanHang";
            btnBanHang.Padding = new Padding(20, 0, 0, 0);
            btnBanHang.Size = new Size(230, 48);
            btnBanHang.TabIndex = 0;
            btnBanHang.Text = "\U0001f9fe  Bán hàng";
            btnBanHang.TextAlign = ContentAlignment.MiddleLeft;
            btnBanHang.UseVisualStyleBackColor = false;
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
            // btnHoaDon
            // 
            btnHoaDon.FlatAppearance.BorderSize = 0;
            btnHoaDon.FlatStyle = FlatStyle.Flat;
            btnHoaDon.Font = new Font("Segoe UI", 10F);
            btnHoaDon.ForeColor = Color.Gainsboro;
            btnHoaDon.Location = new Point(0, 158);
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
            btnKhachHang.FlatAppearance.BorderSize = 0;
            btnKhachHang.FlatStyle = FlatStyle.Flat;
            btnKhachHang.Font = new Font("Segoe UI", 10F);
            btnKhachHang.ForeColor = Color.Gainsboro;
            btnKhachHang.Location = new Point(0, 206);
            btnKhachHang.Margin = new Padding(0);
            btnKhachHang.Name = "btnKhachHang";
            btnKhachHang.Padding = new Padding(20, 0, 0, 0);
            btnKhachHang.Size = new Size(230, 48);
            btnKhachHang.TabIndex = 5;
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
            btnNhanVien.Location = new Point(0, 254);
            btnNhanVien.Margin = new Padding(0);
            btnNhanVien.Name = "btnNhanVien";
            btnNhanVien.Padding = new Padding(20, 0, 0, 0);
            btnNhanVien.Size = new Size(230, 48);
            btnNhanVien.TabIndex = 6;
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
            btnThongKe.Location = new Point(0, 302);
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
            panelContent.AutoScroll = true;
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
            tableMain.ColumnCount = 2;
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tableMain.Controls.Add(panelOrder, 1, 0);
            tableMain.Controls.Add(tableWorkArea, 0, 0);
            tableMain.Dock = DockStyle.Fill;
            tableMain.Location = new Point(22, 16);
            tableMain.Name = "tableMain";
            tableMain.RowCount = 1;
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMain.Size = new Size(1090, 642);
            tableMain.TabIndex = 0;
            // 
            // panelOrder
            // 
            panelOrder.BackColor = Color.White;
            panelOrder.Controls.Add(dgvOrder);
            panelOrder.Controls.Add(panelOrderFooter);
            panelOrder.Controls.Add(panelOrderHeader);
            panelOrder.Controls.Add(panelTables);
            panelOrder.Dock = DockStyle.Fill;
            panelOrder.Location = new Point(490, 0);
            panelOrder.Margin = new Padding(0);
            panelOrder.Name = "panelOrder";
            panelOrder.Padding = new Padding(12, 12, 12, 14);
            panelOrder.Size = new Size(600, 642);
            panelOrder.TabIndex = 1;
            // 
            // dgvOrder
            // 
            dgvOrder.AllowUserToAddRows = false;
            dgvOrder.AllowUserToDeleteRows = false;
            dgvOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrder.BackgroundColor = Color.White;
            dgvOrder.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 245, 241);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(84, 62, 48);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(248, 245, 241);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(84, 62, 48);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvOrder.ColumnHeadersHeight = 38;
            dgvOrder.Columns.AddRange(new DataGridViewColumn[] { colTenMon, colSoLuong, colDonGia, colThanhTien, colXoa });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(245, 238, 230);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvOrder.DefaultCellStyle = dataGridViewCellStyle2;
            dgvOrder.Dock = DockStyle.Fill;
            dgvOrder.EnableHeadersVisualStyles = false;
            dgvOrder.GridColor = Color.FromArgb(236, 229, 221);
            dgvOrder.Location = new Point(12, 244);
            dgvOrder.MultiSelect = false;
            dgvOrder.Name = "dgvOrder";
            dgvOrder.ReadOnly = true;
            dgvOrder.RowHeadersVisible = false;
            dgvOrder.RowHeadersWidth = 51;
            dgvOrder.RowTemplate.Height = 32;
            dgvOrder.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrder.Size = new Size(576, 178);
            dgvOrder.TabIndex = 1;
            // 
            // colTenMon
            // 
            colTenMon.FillWeight = 135F;
            colTenMon.HeaderText = "Tên món";
            colTenMon.MinimumWidth = 6;
            colTenMon.Name = "colTenMon";
            colTenMon.ReadOnly = true;
            // 
            // colSoLuong
            // 
            colSoLuong.FillWeight = 78F;
            colSoLuong.HeaderText = "Số lượng (+/-)";
            colSoLuong.MinimumWidth = 6;
            colSoLuong.Name = "colSoLuong";
            colSoLuong.ReadOnly = true;
            // 
            // colDonGia
            // 
            colDonGia.FillWeight = 86F;
            colDonGia.HeaderText = "Đơn giá";
            colDonGia.MinimumWidth = 6;
            colDonGia.Name = "colDonGia";
            colDonGia.ReadOnly = true;
            // 
            // colThanhTien
            // 
            colThanhTien.FillWeight = 94F;
            colThanhTien.HeaderText = "Thành tiền";
            colThanhTien.MinimumWidth = 6;
            colThanhTien.Name = "colThanhTien";
            colThanhTien.ReadOnly = true;
            // 
            // colXoa
            // 
            colXoa.FillWeight = 58F;
            colXoa.HeaderText = "Xóa";
            colXoa.MinimumWidth = 6;
            colXoa.Name = "colXoa";
            colXoa.ReadOnly = true;
            colXoa.Resizable = DataGridViewTriState.True;
            colXoa.SortMode = DataGridViewColumnSortMode.Automatic;
            colXoa.Text = "Xóa";
            colXoa.UseColumnTextForButtonValue = true;
            // 
            // panelOrderFooter
            // 
            panelOrderFooter.Controls.Add(btnThanhToan);
            panelOrderFooter.Controls.Add(btnTamTinh);
            panelOrderFooter.Controls.Add(lblTongThanhToanValue);
            panelOrderFooter.Controls.Add(lblTongThanhToan);
            panelOrderFooter.Controls.Add(lblGiamGiaValue);
            panelOrderFooter.Controls.Add(lblGiamGia);
            panelOrderFooter.Controls.Add(lblTamTinhValue);
            panelOrderFooter.Controls.Add(lblTamTinh);
            panelOrderFooter.Dock = DockStyle.Bottom;
            panelOrderFooter.Location = new Point(12, 422);
            panelOrderFooter.Name = "panelOrderFooter";
            panelOrderFooter.Padding = new Padding(0, 8, 0, 0);
            panelOrderFooter.Size = new Size(576, 206);
            panelOrderFooter.TabIndex = 2;
            // 
            // btnThanhToan
            // 
            btnThanhToan.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnThanhToan.BackColor = Color.FromArgb(46, 125, 50);
            btnThanhToan.FlatAppearance.BorderSize = 0;
            btnThanhToan.FlatStyle = FlatStyle.Flat;
            btnThanhToan.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnThanhToan.ForeColor = Color.White;
            btnThanhToan.Location = new Point(16, 154);
            btnThanhToan.Name = "btnThanhToan";
            btnThanhToan.Size = new Size(544, 42);
            btnThanhToan.TabIndex = 8;
            btnThanhToan.Text = "Thanh toán liền";
            btnThanhToan.UseVisualStyleBackColor = false;
            // 
            // btnTamTinh
            // 
            btnTamTinh.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnTamTinh.BackColor = Color.FromArgb(255, 183, 77);
            btnTamTinh.FlatAppearance.BorderSize = 0;
            btnTamTinh.FlatStyle = FlatStyle.Flat;
            btnTamTinh.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnTamTinh.ForeColor = Color.FromArgb(79, 56, 43);
            btnTamTinh.Location = new Point(16, 104);
            btnTamTinh.Name = "btnTamTinh";
            btnTamTinh.Size = new Size(544, 42);
            btnTamTinh.TabIndex = 7;
            btnTamTinh.Text = "Thanh toán sau / Lưu bàn";
            btnTamTinh.UseVisualStyleBackColor = false;
            // 
            // lblTongThanhToanValue
            // 
            lblTongThanhToanValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTongThanhToanValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTongThanhToanValue.ForeColor = Color.FromArgb(119, 63, 27);
            lblTongThanhToanValue.Location = new Point(352, 66);
            lblTongThanhToanValue.Name = "lblTongThanhToanValue";
            lblTongThanhToanValue.Size = new Size(208, 33);
            lblTongThanhToanValue.TabIndex = 5;
            lblTongThanhToanValue.Text = "0đ";
            lblTongThanhToanValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTongThanhToan
            // 
            lblTongThanhToan.AutoSize = true;
            lblTongThanhToan.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblTongThanhToan.ForeColor = Color.FromArgb(79, 56, 43);
            lblTongThanhToan.Location = new Point(16, 71);
            lblTongThanhToan.Name = "lblTongThanhToan";
            lblTongThanhToan.Size = new Size(129, 25);
            lblTongThanhToan.TabIndex = 4;
            lblTongThanhToan.Text = "Khách cần trả";
            // 
            // lblGiamGiaValue
            // 
            lblGiamGiaValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblGiamGiaValue.Font = new Font("Segoe UI", 10F);
            lblGiamGiaValue.ForeColor = Color.DimGray;
            lblGiamGiaValue.Location = new Point(352, 38);
            lblGiamGiaValue.Name = "lblGiamGiaValue";
            lblGiamGiaValue.Size = new Size(208, 23);
            lblGiamGiaValue.TabIndex = 3;
            lblGiamGiaValue.Text = "0đ";
            lblGiamGiaValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblGiamGia
            // 
            lblGiamGia.AutoSize = true;
            lblGiamGia.Font = new Font("Segoe UI", 10F);
            lblGiamGia.ForeColor = Color.DimGray;
            lblGiamGia.Location = new Point(16, 38);
            lblGiamGia.Name = "lblGiamGia";
            lblGiamGia.Size = new Size(78, 23);
            lblGiamGia.TabIndex = 2;
            lblGiamGia.Text = "Giảm giá";
            // 
            // lblTamTinhValue
            // 
            lblTamTinhValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTamTinhValue.Font = new Font("Segoe UI", 10F);
            lblTamTinhValue.ForeColor = Color.DimGray;
            lblTamTinhValue.Location = new Point(352, 10);
            lblTamTinhValue.Name = "lblTamTinhValue";
            lblTamTinhValue.Size = new Size(208, 23);
            lblTamTinhValue.TabIndex = 1;
            lblTamTinhValue.Text = "0đ";
            lblTamTinhValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTamTinh
            // 
            lblTamTinh.AutoSize = true;
            lblTamTinh.Font = new Font("Segoe UI", 10F);
            lblTamTinh.ForeColor = Color.DimGray;
            lblTamTinh.Location = new Point(16, 10);
            lblTamTinh.Name = "lblTamTinh";
            lblTamTinh.Size = new Size(83, 23);
            lblTamTinh.TabIndex = 0;
            lblTamTinh.Text = "Tổng tiền";
            // 
            // panelOrderHeader
            // 
            panelOrderHeader.Controls.Add(btnGopBan);
            panelOrderHeader.Controls.Add(btnChuyenBan);
            panelOrderHeader.Controls.Add(lblOrderMeta);
            panelOrderHeader.Controls.Add(lblOrderTitle);
            panelOrderHeader.Dock = DockStyle.Top;
            panelOrderHeader.Location = new Point(12, 180);
            panelOrderHeader.Name = "panelOrderHeader";
            panelOrderHeader.Padding = new Padding(0, 4, 0, 6);
            panelOrderHeader.Size = new Size(576, 64);
            panelOrderHeader.TabIndex = 0;
            // 
            // btnGopBan
            // 
            btnGopBan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGopBan.BackColor = Color.FromArgb(248, 245, 241);
            btnGopBan.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnGopBan.FlatStyle = FlatStyle.Flat;
            btnGopBan.Font = new Font("Segoe UI", 8.8F);
            btnGopBan.ForeColor = Color.FromArgb(79, 56, 43);
            btnGopBan.Location = new Point(484, 27);
            btnGopBan.Name = "btnGopBan";
            btnGopBan.Size = new Size(86, 30);
            btnGopBan.TabIndex = 3;
            btnGopBan.Text = "Gộp bàn";
            btnGopBan.UseVisualStyleBackColor = false;
            // 
            // btnChuyenBan
            // 
            btnChuyenBan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnChuyenBan.BackColor = Color.FromArgb(248, 245, 241);
            btnChuyenBan.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnChuyenBan.FlatStyle = FlatStyle.Flat;
            btnChuyenBan.Font = new Font("Segoe UI", 8.8F);
            btnChuyenBan.ForeColor = Color.FromArgb(79, 56, 43);
            btnChuyenBan.Location = new Point(394, 27);
            btnChuyenBan.Name = "btnChuyenBan";
            btnChuyenBan.Size = new Size(84, 30);
            btnChuyenBan.TabIndex = 2;
            btnChuyenBan.Text = "Chuyển bàn";
            btnChuyenBan.UseVisualStyleBackColor = false;
            // 
            // lblOrderMeta
            // 
            lblOrderMeta.AutoSize = true;
            lblOrderMeta.Font = new Font("Segoe UI", 9F);
            lblOrderMeta.ForeColor = Color.FromArgb(130, 112, 96);
            lblOrderMeta.Location = new Point(4, 35);
            lblOrderMeta.Name = "lblOrderMeta";
            lblOrderMeta.Size = new Size(125, 20);
            lblOrderMeta.TabIndex = 1;
            lblOrderMeta.Text = "Bàn 01 • Tại quán";
            // 
            // lblOrderTitle
            // 
            lblOrderTitle.AutoSize = true;
            lblOrderTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblOrderTitle.ForeColor = Color.FromArgb(63, 45, 35);
            lblOrderTitle.Location = new Point(4, 8);
            lblOrderTitle.Name = "lblOrderTitle";
            lblOrderTitle.Size = new Size(125, 25);
            lblOrderTitle.TabIndex = 0;
            lblOrderTitle.Text = "Chi tiết order";
            // 
            // panelTables
            // 
            panelTables.BackColor = Color.White;
            panelTables.Controls.Add(flowBan);
            panelTables.Controls.Add(panelTablesHeader);
            panelTables.Dock = DockStyle.Top;
            panelTables.Location = new Point(12, 12);
            panelTables.Margin = new Padding(0, 0, 0, 10);
            panelTables.Name = "panelTables";
            panelTables.Padding = new Padding(12);
            panelTables.Size = new Size(576, 168);
            panelTables.TabIndex = 0;
            // 
            // flowBan
            // 
            flowBan.AutoScroll = true;
            flowBan.Controls.Add(btnBan01);
            flowBan.Controls.Add(btnBan02);
            flowBan.Controls.Add(btnBan03);
            flowBan.Controls.Add(btnBan04);
            flowBan.Controls.Add(btnBan05);
            flowBan.Controls.Add(btnBan06);
            flowBan.Dock = DockStyle.Fill;
            flowBan.Location = new Point(12, 65);
            flowBan.Name = "flowBan";
            flowBan.Padding = new Padding(4);
            flowBan.Size = new Size(552, 91);
            flowBan.TabIndex = 1;
            // 
            // btnBan01
            // 
            btnBan01.BackColor = Color.FromArgb(255, 241, 230);
            btnBan01.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnBan01.FlatStyle = FlatStyle.Flat;
            btnBan01.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnBan01.ForeColor = Color.FromArgb(79, 56, 43);
            btnBan01.Location = new Point(12, 12);
            btnBan01.Margin = new Padding(8);
            btnBan01.Name = "btnBan01";
            btnBan01.Size = new Size(95, 80);
            btnBan01.TabIndex = 0;
            btnBan01.Text = "Bàn 01\r\nĐang chọn";
            btnBan01.UseVisualStyleBackColor = false;
            // 
            // btnBan02
            // 
            btnBan02.BackColor = Color.FromArgb(248, 245, 241);
            btnBan02.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnBan02.FlatStyle = FlatStyle.Flat;
            btnBan02.Font = new Font("Segoe UI", 10F);
            btnBan02.ForeColor = Color.FromArgb(79, 56, 43);
            btnBan02.Location = new Point(123, 12);
            btnBan02.Margin = new Padding(8);
            btnBan02.Name = "btnBan02";
            btnBan02.Size = new Size(95, 80);
            btnBan02.TabIndex = 1;
            btnBan02.Text = "Bàn 02\r\nTrống";
            btnBan02.UseVisualStyleBackColor = false;
            // 
            // btnBan03
            // 
            btnBan03.BackColor = Color.FromArgb(248, 245, 241);
            btnBan03.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnBan03.FlatStyle = FlatStyle.Flat;
            btnBan03.Font = new Font("Segoe UI", 10F);
            btnBan03.ForeColor = Color.FromArgb(79, 56, 43);
            btnBan03.Location = new Point(234, 12);
            btnBan03.Margin = new Padding(8);
            btnBan03.Name = "btnBan03";
            btnBan03.Size = new Size(95, 80);
            btnBan03.TabIndex = 2;
            btnBan03.Text = "Bàn 03\r\nTrống";
            btnBan03.UseVisualStyleBackColor = false;
            // 
            // btnBan04
            // 
            btnBan04.BackColor = Color.FromArgb(248, 245, 241);
            btnBan04.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnBan04.FlatStyle = FlatStyle.Flat;
            btnBan04.Font = new Font("Segoe UI", 10F);
            btnBan04.ForeColor = Color.FromArgb(79, 56, 43);
            btnBan04.Location = new Point(345, 12);
            btnBan04.Margin = new Padding(8);
            btnBan04.Name = "btnBan04";
            btnBan04.Size = new Size(95, 80);
            btnBan04.TabIndex = 3;
            btnBan04.Text = "Bàn 04\r\nTrống";
            btnBan04.UseVisualStyleBackColor = false;
            // 
            // btnBan05
            // 
            btnBan05.BackColor = Color.FromArgb(248, 245, 241);
            btnBan05.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnBan05.FlatStyle = FlatStyle.Flat;
            btnBan05.Font = new Font("Segoe UI", 10F);
            btnBan05.ForeColor = Color.FromArgb(79, 56, 43);
            btnBan05.Location = new Point(12, 108);
            btnBan05.Margin = new Padding(8);
            btnBan05.Name = "btnBan05";
            btnBan05.Size = new Size(95, 80);
            btnBan05.TabIndex = 4;
            btnBan05.Text = "Bàn 05\r\nĐang dùng";
            btnBan05.UseVisualStyleBackColor = false;
            // 
            // btnBan06
            // 
            btnBan06.BackColor = Color.FromArgb(248, 245, 241);
            btnBan06.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnBan06.FlatStyle = FlatStyle.Flat;
            btnBan06.Font = new Font("Segoe UI", 10F);
            btnBan06.ForeColor = Color.FromArgb(79, 56, 43);
            btnBan06.Location = new Point(123, 108);
            btnBan06.Margin = new Padding(8);
            btnBan06.Name = "btnBan06";
            btnBan06.Size = new Size(95, 80);
            btnBan06.TabIndex = 5;
            btnBan06.Text = "Bàn 06\r\nTrống";
            btnBan06.UseVisualStyleBackColor = false;
            // 
            // panelTablesHeader
            // 
            panelTablesHeader.Controls.Add(lblThongTinBan);
            panelTablesHeader.Controls.Add(lblTablesTitle);
            panelTablesHeader.Dock = DockStyle.Top;
            panelTablesHeader.Location = new Point(12, 12);
            panelTablesHeader.Name = "panelTablesHeader";
            panelTablesHeader.Size = new Size(552, 53);
            panelTablesHeader.TabIndex = 0;
            // 
            // lblThongTinBan
            // 
            lblThongTinBan.AutoSize = true;
            lblThongTinBan.Font = new Font("Segoe UI", 9F);
            lblThongTinBan.ForeColor = Color.FromArgb(130, 112, 96);
            lblThongTinBan.Location = new Point(4, 30);
            lblThongTinBan.Name = "lblThongTinBan";
            lblThongTinBan.Size = new Size(209, 20);
            lblThongTinBan.TabIndex = 1;
            lblThongTinBan.Text = "Chọn bàn hoặc khách mang đi";
            // 
            // lblTablesTitle
            // 
            lblTablesTitle.AutoSize = true;
            lblTablesTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblTablesTitle.ForeColor = Color.FromArgb(63, 45, 35);
            lblTablesTitle.Location = new Point(4, 4);
            lblTablesTitle.Name = "lblTablesTitle";
            lblTablesTitle.Size = new Size(184, 25);
            lblTablesTitle.TabIndex = 0;
            lblTablesTitle.Text = "Thông tin đơn / bàn";
            // 
            // tableWorkArea
            // 
            tableWorkArea.ColumnCount = 1;
            tableWorkArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableWorkArea.Controls.Add(panelMenu, 0, 0);
            tableWorkArea.Dock = DockStyle.Fill;
            tableWorkArea.Location = new Point(0, 0);
            tableWorkArea.Margin = new Padding(0, 0, 12, 0);
            tableWorkArea.Name = "tableWorkArea";
            tableWorkArea.RowCount = 1;
            tableWorkArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableWorkArea.Size = new Size(478, 642);
            tableWorkArea.TabIndex = 0;
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.White;
            panelMenu.Controls.Add(flowMon);
            panelMenu.Controls.Add(panelMenuFilter);
            panelMenu.Dock = DockStyle.Fill;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Margin = new Padding(0, 0, 12, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Padding = new Padding(12);
            panelMenu.Size = new Size(466, 642);
            panelMenu.TabIndex = 1;
            // 
            // flowMon
            // 
            flowMon.AutoScroll = true;
            flowMon.Controls.Add(panelMon1);
            flowMon.Controls.Add(panelMon2);
            flowMon.Dock = DockStyle.Fill;
            flowMon.Location = new Point(12, 78);
            flowMon.Name = "flowMon";
            flowMon.Padding = new Padding(4);
            flowMon.Size = new Size(442, 552);
            flowMon.TabIndex = 1;
            // 
            // panelMon1
            // 
            panelMon1.BackColor = Color.FromArgb(250, 247, 243);
            panelMon1.Controls.Add(lblMon1Gia);
            panelMon1.Controls.Add(lblMon1Ten);
            panelMon1.Controls.Add(btnMon1);
            panelMon1.Location = new Point(7, 7);
            panelMon1.Name = "panelMon1";
            panelMon1.Size = new Size(200, 120);
            panelMon1.TabIndex = 0;
            // 
            // lblMon1Gia
            // 
            lblMon1Gia.AutoSize = true;
            lblMon1Gia.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblMon1Gia.ForeColor = Color.FromArgb(119, 63, 27);
            lblMon1Gia.Location = new Point(14, 68);
            lblMon1Gia.Name = "lblMon1Gia";
            lblMon1Gia.Size = new Size(60, 23);
            lblMon1Gia.TabIndex = 2;
            lblMon1Gia.Text = "45.000";
            // 
            // lblMon1Ten
            // 
            lblMon1Ten.AutoSize = true;
            lblMon1Ten.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblMon1Ten.ForeColor = Color.FromArgb(63, 45, 35);
            lblMon1Ten.Location = new Point(14, 18);
            lblMon1Ten.Name = "lblMon1Ten";
            lblMon1Ten.Size = new Size(104, 23);
            lblMon1Ten.TabIndex = 1;
            lblMon1Ten.Text = "Latte đá xay";
            // 
            // btnMon1
            // 
            btnMon1.BackColor = Color.White;
            btnMon1.FlatAppearance.BorderColor = Color.FromArgb(230, 220, 210);
            btnMon1.FlatStyle = FlatStyle.Flat;
            btnMon1.Font = new Font("Segoe UI", 9F);
            btnMon1.ForeColor = Color.FromArgb(79, 56, 43);
            btnMon1.Location = new Point(100, 76);
            btnMon1.Name = "btnMon1";
            btnMon1.Size = new Size(86, 32);
            btnMon1.TabIndex = 0;
            btnMon1.Text = "+ Thêm";
            btnMon1.UseVisualStyleBackColor = false;
            // 
            // panelMon2
            // 
            panelMon2.BackColor = Color.FromArgb(250, 247, 243);
            panelMon2.Controls.Add(lblMon2Gia);
            panelMon2.Controls.Add(lblMon2Ten);
            panelMon2.Controls.Add(btnMon2);
            panelMon2.Location = new Point(213, 7);
            panelMon2.Name = "panelMon2";
            panelMon2.Size = new Size(200, 120);
            panelMon2.TabIndex = 1;
            // 
            // lblMon2Gia
            // 
            lblMon2Gia.AutoSize = true;
            lblMon2Gia.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblMon2Gia.ForeColor = Color.FromArgb(119, 63, 27);
            lblMon2Gia.Location = new Point(14, 68);
            lblMon2Gia.Name = "lblMon2Gia";
            lblMon2Gia.Size = new Size(59, 23);
            lblMon2Gia.TabIndex = 2;
            lblMon2Gia.Text = "39.000";
            // 
            // lblMon2Ten
            // 
            lblMon2Ten.AutoSize = true;
            lblMon2Ten.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblMon2Ten.ForeColor = Color.FromArgb(63, 45, 35);
            lblMon2Ten.Location = new Point(14, 18);
            lblMon2Ten.Name = "lblMon2Ten";
            lblMon2Ten.Size = new Size(104, 23);
            lblMon2Ten.TabIndex = 1;
            lblMon2Ten.Text = "Trà đào cam";
            // 
            // btnMon2
            // 
            btnMon2.BackColor = Color.White;
            btnMon2.FlatAppearance.BorderColor = Color.FromArgb(230, 220, 210);
            btnMon2.FlatStyle = FlatStyle.Flat;
            btnMon2.Font = new Font("Segoe UI", 9F);
            btnMon2.ForeColor = Color.FromArgb(79, 56, 43);
            btnMon2.Location = new Point(100, 76);
            btnMon2.Name = "btnMon2";
            btnMon2.Size = new Size(86, 32);
            btnMon2.TabIndex = 0;
            btnMon2.Text = "+ Thêm";
            btnMon2.UseVisualStyleBackColor = false;
            // 
            // panelMenuFilter
            // 
            panelMenuFilter.Controls.Add(btnTatCa);
            panelMenuFilter.Controls.Add(btnTra);
            panelMenuFilter.Controls.Add(btnDaXay);
            panelMenuFilter.Controls.Add(btnCafe);
            panelMenuFilter.Controls.Add(lblMenuTitle);
            panelMenuFilter.Dock = DockStyle.Top;
            panelMenuFilter.Location = new Point(12, 12);
            panelMenuFilter.Name = "panelMenuFilter";
            panelMenuFilter.Size = new Size(442, 66);
            panelMenuFilter.TabIndex = 0;
            // 
            // btnTatCa
            // 
            btnTatCa.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTatCa.BackColor = Color.FromArgb(94, 64, 47);
            btnTatCa.FlatAppearance.BorderSize = 0;
            btnTatCa.FlatStyle = FlatStyle.Flat;
            btnTatCa.Font = new Font("Segoe UI", 9F);
            btnTatCa.ForeColor = Color.White;
            btnTatCa.Location = new Point(367, 18);
            btnTatCa.Name = "btnTatCa";
            btnTatCa.Size = new Size(72, 32);
            btnTatCa.TabIndex = 4;
            btnTatCa.Text = "Tất cả";
            btnTatCa.UseVisualStyleBackColor = false;
            // 
            // btnTra
            // 
            btnTra.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTra.BackColor = Color.FromArgb(248, 245, 241);
            btnTra.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnTra.FlatStyle = FlatStyle.Flat;
            btnTra.Font = new Font("Segoe UI", 9F);
            btnTra.ForeColor = Color.FromArgb(79, 56, 43);
            btnTra.Location = new Point(263, 18);
            btnTra.Name = "btnTra";
            btnTra.Size = new Size(98, 32);
            btnTra.TabIndex = 3;
            btnTra.Text = "Trà trái cây";
            btnTra.UseVisualStyleBackColor = false;
            // 
            // btnDaXay
            // 
            btnDaXay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDaXay.BackColor = Color.FromArgb(248, 245, 241);
            btnDaXay.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnDaXay.FlatStyle = FlatStyle.Flat;
            btnDaXay.Font = new Font("Segoe UI", 9F);
            btnDaXay.ForeColor = Color.FromArgb(79, 56, 43);
            btnDaXay.Location = new Point(181, 18);
            btnDaXay.Name = "btnDaXay";
            btnDaXay.Size = new Size(76, 32);
            btnDaXay.TabIndex = 2;
            btnDaXay.Text = "Sinh tố";
            btnDaXay.UseVisualStyleBackColor = false;
            // 
            // btnCafe
            // 
            btnCafe.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCafe.BackColor = Color.FromArgb(248, 245, 241);
            btnCafe.FlatAppearance.BorderColor = Color.FromArgb(224, 214, 203);
            btnCafe.FlatStyle = FlatStyle.Flat;
            btnCafe.Font = new Font("Segoe UI", 9F);
            btnCafe.ForeColor = Color.FromArgb(79, 56, 43);
            btnCafe.Location = new Point(99, 18);
            btnCafe.Name = "btnCafe";
            btnCafe.Size = new Size(76, 32);
            btnCafe.TabIndex = 1;
            btnCafe.Text = "Cà phê";
            btnCafe.UseVisualStyleBackColor = false;
            // 
            // lblMenuTitle
            // 
            lblMenuTitle.AutoSize = true;
            lblMenuTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lblMenuTitle.ForeColor = Color.FromArgb(63, 45, 35);
            lblMenuTitle.Location = new Point(4, 21);
            lblMenuTitle.Name = "lblMenuTitle";
            lblMenuTitle.Size = new Size(62, 25);
            lblMenuTitle.TabIndex = 0;
            lblMenuTitle.Text = "Menu";
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
            btnUserMenu.Location = new Point(1087, 24);
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
            lblUserName.ForeColor = Color.FromArgb(63, 45, 35);
            lblUserName.Location = new Point(936, 27);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(167, 23);
            lblUserName.TabIndex = 3;
            lblUserName.Text = "Nhân viên bán hàng";
            // 
            // picAvatar
            // 
            picAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picAvatar.BackColor = Color.FromArgb(221, 206, 189);
            picAvatar.Location = new Point(894, 20);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new Size(36, 36);
            picAvatar.TabIndex = 2;
            picAvatar.TabStop = false;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.BackColor = Color.FromArgb(248, 245, 241);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.ForeColor = Color.DimGray;
            txtSearch.Location = new Point(245, 23);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "🔍  Tìm bàn hoặc món";
            txtSearch.Size = new Size(280, 30);
            txtSearch.TabIndex = 1;
            // 
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(63, 45, 35);
            lblPageTitle.Location = new Point(22, 20);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(181, 32);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Quầy bán hàng";
            // 
            // frmBanHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(246, 242, 236);
            ClientSize = new Size(1364, 760);
            Controls.Add(panelMain);
            Controls.Add(panelSidebar);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1220, 720);
            Name = "frmBanHang";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bán hàng";
            panelSidebar.ResumeLayout(false);
            flowSidebarMenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            panelLogo.PerformLayout();
            panelMain.ResumeLayout(false);
            panelContent.ResumeLayout(false);
            tableMain.ResumeLayout(false);
            panelOrder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvOrder).EndInit();
            panelOrderFooter.ResumeLayout(false);
            panelOrderFooter.PerformLayout();
            panelOrderHeader.ResumeLayout(false);
            panelOrderHeader.PerformLayout();
            panelTables.ResumeLayout(false);
            flowBan.ResumeLayout(false);
            panelTablesHeader.ResumeLayout(false);
            panelTablesHeader.PerformLayout();
            tableWorkArea.ResumeLayout(false);
            panelMenu.ResumeLayout(false);
            flowMon.ResumeLayout(false);
            panelMon1.ResumeLayout(false);
            panelMon1.PerformLayout();
            panelMon2.ResumeLayout(false);
            panelMon2.PerformLayout();
            panelMenuFilter.ResumeLayout(false);
            panelMenuFilter.PerformLayout();
            panelTopbar.ResumeLayout(false);
            panelTopbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.FlowLayoutPanel flowSidebarMenu;
        private System.Windows.Forms.Button btnBanHang;
        private System.Windows.Forms.Button btnQuanLyBan;
        private System.Windows.Forms.Button btnQuanLyMon;
        private System.Windows.Forms.Button btnHoaDon;
        private System.Windows.Forms.Button btnKhachHang;
        private System.Windows.Forms.Button btnNhanVien;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.Label lblLogoSub;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.Panel panelOrder;
        private System.Windows.Forms.Panel panelOrderFooter;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Button btnTamTinh;
        private System.Windows.Forms.Label lblTongThanhToanValue;
        private System.Windows.Forms.Label lblTongThanhToan;
        private System.Windows.Forms.Label lblGiamGiaValue;
        private System.Windows.Forms.Label lblGiamGia;
        private System.Windows.Forms.Label lblTamTinhValue;
        private System.Windows.Forms.Label lblTamTinh;
        private System.Windows.Forms.DataGridView dgvOrder;
        private System.Windows.Forms.Panel panelOrderHeader;
        private System.Windows.Forms.Button btnGopBan;
        private System.Windows.Forms.Button btnChuyenBan;
        private System.Windows.Forms.Label lblOrderMeta;
        private System.Windows.Forms.Label lblOrderTitle;
        private System.Windows.Forms.TableLayoutPanel tableWorkArea;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.FlowLayoutPanel flowMon;
        private System.Windows.Forms.Panel panelMon2;
        private System.Windows.Forms.Label lblMon2Gia;
        private System.Windows.Forms.Label lblMon2Ten;
        private System.Windows.Forms.Button btnMon2;
        private System.Windows.Forms.Panel panelMon1;
        private System.Windows.Forms.Label lblMon1Gia;
        private System.Windows.Forms.Label lblMon1Ten;
        private System.Windows.Forms.Button btnMon1;
        private System.Windows.Forms.Panel panelMenuFilter;
        private System.Windows.Forms.Button btnTatCa;
        private System.Windows.Forms.Button btnTra;
        private System.Windows.Forms.Button btnDaXay;
        private System.Windows.Forms.Button btnCafe;
        private System.Windows.Forms.Label lblMenuTitle;
        private System.Windows.Forms.Panel panelTables;
        private System.Windows.Forms.FlowLayoutPanel flowBan;
        private System.Windows.Forms.Button btnBan06;
        private System.Windows.Forms.Button btnBan05;
        private System.Windows.Forms.Button btnBan04;
        private System.Windows.Forms.Button btnBan03;
        private System.Windows.Forms.Button btnBan02;
        private System.Windows.Forms.Button btnBan01;
        private System.Windows.Forms.Panel panelTablesHeader;
        private System.Windows.Forms.Label lblThongTinBan;
        private System.Windows.Forms.Label lblTablesTitle;
        private System.Windows.Forms.Panel panelTopbar;
        private System.Windows.Forms.Button btnUserMenu;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenMon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
        private System.Windows.Forms.DataGridViewButtonColumn colXoa;
    }
}
