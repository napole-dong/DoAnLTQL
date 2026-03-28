using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QuanLyQuanCaPhe.Data
{
    public class dtaKhachHang
    {
        public int ID { get; set; }
        public string HoVaTen { get; set; } = string.Empty;
        public string? DienThoai { get; set; }
        public string? DiaChi { get; set; }

        public virtual ObservableCollectionListSource<dtaHoadon> HoaDon { get; } = new();
    }
}