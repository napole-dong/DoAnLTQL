namespace QuanLyQuanCaPhe.Forms
{
    partial class frmDangNhap
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
            panelBrand = new Panel();
            lblBrandSub = new Label();
            lblBrand = new Label();
            panelLoginArea = new Panel();
            lblHoTro = new Label();
            tableLoginHost = new TableLayoutPanel();
            panelLoginCardBorder = new Panel();
            panelLoginCard = new Panel();
            btnThoat = new Button();
            btnDangNhap = new Button();
            chkNhoDangNhap = new CheckBox();
            txtMatKhau = new TextBox();
            lblMatKhau = new Label();
            txtTenDangNhap = new TextBox();
            lblTenDangNhap = new Label();
            lblDangNhapSub = new Label();
            lblThongBaoLoi = new Label();
            lblDangNhapTitle = new Label();
            panelBrand.SuspendLayout();
            panelLoginArea.SuspendLayout();
            tableLoginHost.SuspendLayout();
            panelLoginCardBorder.SuspendLayout();
            panelLoginCard.SuspendLayout();
            SuspendLayout();
            // 
            // panelBrand
            // 
            panelBrand.BackColor = Color.FromArgb(52, 36, 29);
            panelBrand.Controls.Add(lblBrandSub);
            panelBrand.Controls.Add(lblBrand);
            panelBrand.Dock = DockStyle.Left;
            panelBrand.Location = new Point(0, 0);
            panelBrand.Name = "panelBrand";
            panelBrand.Size = new Size(418, 760);
            panelBrand.TabIndex = 0;
            // 
            // lblBrandSub
            // 
            lblBrandSub.AutoSize = true;
            lblBrandSub.Font = new Font("Segoe UI", 10F);
            lblBrandSub.ForeColor = Color.FromArgb(196, 176, 157);
            lblBrandSub.Location = new Point(46, 128);
            lblBrandSub.Name = "lblBrandSub";
            lblBrandSub.Size = new Size(166, 23);
            lblBrandSub.TabIndex = 1;
            lblBrandSub.Text = "Coffee Management";
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Font = new Font("Segoe UI Semibold", 30F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(40, 62);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new Size(362, 67);
            lblBrand.TabIndex = 0;
            lblBrand.Text = "☕ Cà phê Pro";
            // 
            // panelLoginArea
            // 
            panelLoginArea.BackColor = Color.FromArgb(246, 242, 236);
            panelLoginArea.Controls.Add(lblHoTro);
            panelLoginArea.Controls.Add(tableLoginHost);
            panelLoginArea.Dock = DockStyle.Fill;
            panelLoginArea.Location = new Point(418, 0);
            panelLoginArea.Name = "panelLoginArea";
            panelLoginArea.Padding = new Padding(34, 24, 34, 24);
            panelLoginArea.Size = new Size(946, 760);
            panelLoginArea.TabIndex = 1;
            // 
            // lblHoTro
            // 
            lblHoTro.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblHoTro.AutoSize = true;
            lblHoTro.ForeColor = Color.FromArgb(120, 103, 92);
            lblHoTro.Location = new Point(612, 724);
            lblHoTro.Name = "lblHoTro";
            lblHoTro.Size = new Size(344, 20);
            lblHoTro.TabIndex = 1;
            lblHoTro.Text = "Cần hỗ trợ? Vui lòng liên hệ quản trị viên hệ thống.";
            // 
            // tableLoginHost
            // 
            tableLoginHost.ColumnCount = 3;
            tableLoginHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
            tableLoginHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tableLoginHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
            tableLoginHost.Controls.Add(panelLoginCardBorder, 1, 1);
            tableLoginHost.Dock = DockStyle.Fill;
            tableLoginHost.Location = new Point(34, 24);
            tableLoginHost.Name = "tableLoginHost";
            tableLoginHost.RowCount = 3;
            tableLoginHost.RowStyles.Add(new RowStyle(SizeType.Percent, 14F));
            tableLoginHost.RowStyles.Add(new RowStyle(SizeType.Percent, 72F));
            tableLoginHost.RowStyles.Add(new RowStyle(SizeType.Percent, 14F));
            tableLoginHost.Size = new Size(878, 712);
            tableLoginHost.TabIndex = 0;
            // 
            // panelLoginCardBorder
            // 
            panelLoginCardBorder.BackColor = Color.FromArgb(228, 219, 209);
            panelLoginCardBorder.Controls.Add(panelLoginCard);
            panelLoginCardBorder.Dock = DockStyle.Fill;
            panelLoginCardBorder.Location = new Point(228, 99);
            panelLoginCardBorder.Margin = new Padding(0);
            panelLoginCardBorder.Name = "panelLoginCardBorder";
            panelLoginCardBorder.Padding = new Padding(1);
            panelLoginCardBorder.Size = new Size(421, 512);
            panelLoginCardBorder.TabIndex = 0;
            // 
            // panelLoginCard
            // 
            panelLoginCard.BackColor = Color.White;
            panelLoginCard.Controls.Add(btnThoat);
            panelLoginCard.Controls.Add(btnDangNhap);
            panelLoginCard.Controls.Add(chkNhoDangNhap);
            panelLoginCard.Controls.Add(txtMatKhau);
            panelLoginCard.Controls.Add(lblMatKhau);
            panelLoginCard.Controls.Add(txtTenDangNhap);
            panelLoginCard.Controls.Add(lblTenDangNhap);
            panelLoginCard.Controls.Add(lblDangNhapSub);
            panelLoginCard.Controls.Add(lblThongBaoLoi);
            panelLoginCard.Controls.Add(lblDangNhapTitle);
            panelLoginCard.Dock = DockStyle.Fill;
            panelLoginCard.Location = new Point(1, 1);
            panelLoginCard.Name = "panelLoginCard";
            panelLoginCard.Size = new Size(419, 510);
            panelLoginCard.TabIndex = 0;
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.FromArgb(244, 233, 220);
            btnThoat.DialogResult = DialogResult.Cancel;
            btnThoat.FlatAppearance.BorderSize = 0;
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnThoat.ForeColor = Color.FromArgb(77, 55, 42);
            btnThoat.Location = new Point(32, 390);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(354, 42);
            btnThoat.TabIndex = 8;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = false;
            // 
            // btnDangNhap
            // 
            btnDangNhap.BackColor = Color.FromArgb(94, 64, 47);
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Location = new Point(32, 340);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(354, 42);
            btnDangNhap.TabIndex = 7;
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.UseVisualStyleBackColor = false;
            // 
            // chkNhoDangNhap
            // 
            chkNhoDangNhap.AutoSize = true;
            chkNhoDangNhap.ForeColor = Color.FromArgb(90, 84, 79);
            chkNhoDangNhap.Location = new Point(32, 295);
            chkNhoDangNhap.Name = "chkNhoDangNhap";
            chkNhoDangNhap.Size = new Size(124, 24);
            chkNhoDangNhap.TabIndex = 6;
            chkNhoDangNhap.Text = "Nhớ tài khoản";
            chkNhoDangNhap.UseVisualStyleBackColor = true;
            // 
            // txtMatKhau
            // 
            txtMatKhau.BorderStyle = BorderStyle.FixedSingle;
            txtMatKhau.Font = new Font("Segoe UI", 10F);
            txtMatKhau.Location = new Point(32, 248);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PlaceholderText = "Nhập mật khẩu";
            txtMatKhau.Size = new Size(354, 30);
            txtMatKhau.TabIndex = 5;
            txtMatKhau.UseSystemPasswordChar = true;
            // 
            // lblMatKhau
            // 
            lblMatKhau.AutoSize = true;
            lblMatKhau.Font = new Font("Segoe UI", 9.5F);
            lblMatKhau.ForeColor = Color.FromArgb(90, 84, 79);
            lblMatKhau.Location = new Point(32, 223);
            lblMatKhau.Name = "lblMatKhau";
            lblMatKhau.Size = new Size(75, 21);
            lblMatKhau.TabIndex = 4;
            lblMatKhau.Text = "Mật khẩu";
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.BorderStyle = BorderStyle.FixedSingle;
            txtTenDangNhap.Font = new Font("Segoe UI", 10F);
            txtTenDangNhap.Location = new Point(32, 176);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.PlaceholderText = "Nhập tên đăng nhập";
            txtTenDangNhap.Size = new Size(354, 30);
            txtTenDangNhap.TabIndex = 3;
            // 
            // lblTenDangNhap
            // 
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Font = new Font("Segoe UI", 9.5F);
            lblTenDangNhap.ForeColor = Color.FromArgb(90, 84, 79);
            lblTenDangNhap.Location = new Point(32, 151);
            lblTenDangNhap.Name = "lblTenDangNhap";
            lblTenDangNhap.Size = new Size(111, 21);
            lblTenDangNhap.TabIndex = 2;
            lblTenDangNhap.Text = "Tên đăng nhập";
            // 
            // lblDangNhapSub
            // 
            lblDangNhapSub.ForeColor = Color.FromArgb(110, 95, 85);
            lblDangNhapSub.Location = new Point(32, 90);
            lblDangNhapSub.Name = "lblDangNhapSub";
            lblDangNhapSub.Size = new Size(351, 42);
            lblDangNhapSub.TabIndex = 1;
            lblDangNhapSub.Text = "Sử dụng tài khoản nhân viên để truy cập hệ thống quản lý quán cà phê.";
            // 
            // lblThongBaoLoi
            // 
            lblThongBaoLoi.ForeColor = Color.FromArgb(189, 63, 50);
            lblThongBaoLoi.Location = new Point(32, 132);
            lblThongBaoLoi.Name = "lblThongBaoLoi";
            lblThongBaoLoi.Size = new Size(354, 19);
            lblThongBaoLoi.TabIndex = 9;
            lblThongBaoLoi.Text = "Thông báo lỗi";
            lblThongBaoLoi.Visible = false;
            // 
            // lblDangNhapTitle
            // 
            lblDangNhapTitle.AutoSize = true;
            lblDangNhapTitle.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblDangNhapTitle.ForeColor = Color.FromArgb(77, 55, 42);
            lblDangNhapTitle.Location = new Point(32, 38);
            lblDangNhapTitle.Name = "lblDangNhapTitle";
            lblDangNhapTitle.Size = new Size(189, 46);
            lblDangNhapTitle.TabIndex = 0;
            lblDangNhapTitle.Text = "Đăng nhập";
            // 
            // frmDangNhap
            // 
            AcceptButton = btnDangNhap;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 242, 236);
            CancelButton = btnThoat;
            ClientSize = new Size(1364, 760);
            Controls.Add(panelLoginArea);
            Controls.Add(panelBrand);
            MinimumSize = new Size(1120, 680);
            Name = "frmDangNhap";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            panelBrand.ResumeLayout(false);
            panelBrand.PerformLayout();
            panelLoginArea.ResumeLayout(false);
            panelLoginArea.PerformLayout();
            tableLoginHost.ResumeLayout(false);
            panelLoginCardBorder.ResumeLayout(false);
            panelLoginCard.ResumeLayout(false);
            panelLoginCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelBrand;
        private Label lblBrandSub;
        private Label lblBrand;
        private Panel panelLoginArea;
        private Label lblHoTro;
        private TableLayoutPanel tableLoginHost;
        private Panel panelLoginCardBorder;
        private Panel panelLoginCard;
        private Button btnThoat;
        private Button btnDangNhap;
        private CheckBox chkNhoDangNhap;
        private TextBox txtMatKhau;
        private Label lblMatKhau;
        private TextBox txtTenDangNhap;
        private Label lblTenDangNhap;
        private Label lblDangNhapSub;
        private Label lblThongBaoLoi;
        private Label lblDangNhapTitle;
    }
}