namespace QuanLyQuanCaPhe.DTO;

public enum HoaDonTrangThai
{
    Draft = 0,
    Paid = 1,
    Closed = 2,
    Cancelled = 3,

    ChuaThanhToan = Draft,
    DaThanhToan = Paid,
    DaDong = Closed,
    DaHuy = Cancelled
}
