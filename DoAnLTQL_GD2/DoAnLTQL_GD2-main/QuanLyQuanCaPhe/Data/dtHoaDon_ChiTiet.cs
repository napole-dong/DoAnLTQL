namespace QuanLyQuanCaPhe.Data
{
    public class dtHoaDon_ChiTiet
    {
        public int ID { get; set; }
        public int HoaDonID { get; set; }
        public int MonID { get; set; }
        public short SoLuongBan { get; set; }
        public int DonGiaBan { get; set; }
        public string? GhiChu { get; set; } // Yêu cầu đặc biệt của đồ uống

        public virtual dtaHoadon HoaDon { get; set; } = null!;
        public virtual dtaMon Mon { get; set; } = null!;
    }
}