using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QuanLyQuanCaPhe.Data
{
    public class HoaDon
    {
        public int ID { get; set; }
        public int NhanVienID { get; set; }
        public int KhachHangID { get; set; }
        public int BanID { get; set; } // Liên kết với bàn đang ngồi
        public DateTime NgayLap { get; set; }
        public int TrangThai { get; set; } // 0: Chưa thanh toán, 1: Đã thanh toán
        public string? GhiChuHoaDon { get; set; }

        public virtual ObservableCollectionListSource<HoaDon_ChiTiet> HoaDon_ChiTiet { get; } = new();
        public virtual KhachHang KhachHang { get; set; } = null!;
        public virtual NhanVien NhanVien { get; set; } = null!;
        public virtual Ban Ban { get; set; } = null!;
    }
}