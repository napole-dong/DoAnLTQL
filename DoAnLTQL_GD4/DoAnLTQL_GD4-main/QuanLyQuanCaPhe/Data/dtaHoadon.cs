using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QuanLyQuanCaPhe.Data
{
    public class dtaHoadon
    {
        public int ID { get; set; }
        public int NhanVienID { get; set; }
        public int KhachHangID { get; set; }
        public int BanID { get; set; } // Liên kết với bàn đang ngồi
        public DateTime NgayLap { get; set; }
        public int TrangThai { get; set; } // 0: Chưa thanh toán, 1: Đã thanh toán
        public string? GhiChuHoaDon { get; set; }

        public virtual ObservableCollectionListSource<dtHoaDon_ChiTiet> HoaDon_ChiTiet { get; } = new();
        public virtual dtaKhachHang KhachHang { get; set; } = null!;
        public virtual dtaNhanVien NhanVien { get; set; } = null!;
        public virtual dtaBan Ban { get; set; } = null!;
    }
}