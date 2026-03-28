using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QuanLyQuanCaPhe.Data
{
    public class dtaNhanVien
    {
        public int ID { get; set; }
        public string HoVaTen { get; set; }
        public string? DienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public bool QuyenHan { get; set; } // True: Quản lý, False: Thu ngân/Pha chế

        public virtual ObservableCollectionListSource<dtaHoadon> HoaDon { get; } = new();
    }
}