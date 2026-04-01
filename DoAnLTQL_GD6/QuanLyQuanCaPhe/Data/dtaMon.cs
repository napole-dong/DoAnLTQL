using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QuanLyQuanCaPhe.Data
{
    public class dtaMon
    {
        public int ID { get; set; }
        public int LoaiMonID { get; set; }
        public string TenMon { get; set; } = string.Empty;
        public int DonGia { get; set; }
        public string TrangThai { get; set; } = "Đang kinh doanh";
        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }

        public virtual ObservableCollectionListSource<dtHoaDon_ChiTiet> HoaDon_ChiTiet { get; } = new();
        public virtual dtaLoaiMon LoaiMon { get; set; } = null!;
    }
}