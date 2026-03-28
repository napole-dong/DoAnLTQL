using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QuanLyQuanCaPhe.Data
{
    public class dtaBan
    {
        public int ID { get; set; }
        public string TenBan { get; set; }
        public int TrangThai { get; set; } // 0: Bàn trống, 1: Đang có khách

        // Tạo liên kết 1-Nhiều với bảng Hóa đơn
        public virtual ObservableCollectionListSource<dtaHoadon> HoaDon { get; } = new();
    }
}