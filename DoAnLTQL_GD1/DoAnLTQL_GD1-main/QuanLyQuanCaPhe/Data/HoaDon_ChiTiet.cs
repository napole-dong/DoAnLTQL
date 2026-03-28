namespace QuanLyQuanCaPhe.Data
{
    public class HoaDon_ChiTiet
    {
        public int ID { get; set; }
        public int HoaDonID { get; set; }
        public int MonID { get; set; }
        public short SoLuongBan { get; set; }
        public int DonGiaBan { get; set; }
        public string? GhiChu { get; set; } // Yêu cầu đặc biệt của đồ uống

        public virtual HoaDon HoaDon { get; set; } = null!;
        public virtual Mon Mon { get; set; } = null!;
    }
}