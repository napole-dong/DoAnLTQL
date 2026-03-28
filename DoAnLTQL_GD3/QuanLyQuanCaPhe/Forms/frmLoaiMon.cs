using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmLoaiMon : Form
    {
        private readonly bool _isEmbedded;

        public frmLoaiMon(bool isEmbedded = false)
        {
            _isEmbedded = isEmbedded;
            InitializeComponent();
            Load += frmLoaiMon_Load;
        }

        private void frmLoaiMon_Load(object? sender, EventArgs e)
        {
            if (_isEmbedded)
            {
                panelSidebar.Visible = false;
                panelTopbar.Visible = false;
                panelMain.Dock = DockStyle.Fill;
            }
        }

        private void btnThemLoai_Click(object sender, EventArgs e)
        {

        }

        private void btnCapNhatLoai_Click(object sender, EventArgs e)
        {

        }

        private void btnXoaLoai_Click(object sender, EventArgs e)
        {

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {

        }

        private void btnNhapLoai_Click(object sender, EventArgs e)
        {

        }

        private void btnXuatLoai_Click(object sender, EventArgs e)
        {

        }
    }
}
