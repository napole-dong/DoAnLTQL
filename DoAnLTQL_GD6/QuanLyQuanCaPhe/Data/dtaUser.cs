namespace QuanLyQuanCaPhe.Data
{
    public class dtaUser
    {
        public int ID { get; set; }
        public int NhanVienID { get; set; }
        public int VaiTroID { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public bool HoatDong { get; set; }

        public virtual dtaNhanVien NhanVien { get; set; } = null!;
        public virtual dtaVaiTro VaiTro { get; set; } = null!;
    }
}
