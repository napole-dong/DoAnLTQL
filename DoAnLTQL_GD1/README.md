# QuanLyQuanCaPhe

Ứng dụng WinForms quản lý quán cà phê sử dụng .NET 10 và Entity Framework Core.

## Giai đoạn 1 đã hoàn thành

- Thiết lập `DbContext` cho cơ sở dữ liệu `CaPheDbContext`.
- Khai báo các bảng/dữ liệu ban đầu:
  - `Ban`
  - `LoaiMon`
  - `Mon`
  - `NhanVien`
  - `KhachHang`
  - `HoaDon`
  - `HoaDon_ChiTiet`
- Cấu hình các quan hệ giữa các bảng:
  - `LoaiMon` - `Mon`
  - `Mon` - `HoaDon_ChiTiet`
  - `HoaDon` - `HoaDon_ChiTiet`
  - `Ban` - `HoaDon`
  - `NhanVien` - `HoaDon`
  - `KhachHang` - `HoaDon`
- Tạo migration khởi tạo cơ sở dữ liệu: `KhoiTaoCSDL`.
- Cấu hình chuỗi kết nối SQL Server trong `App.config`.

## Công nghệ sử dụng

- .NET 10
- WinForms
- Entity Framework Core 10
- SQL Server Express

## Cấu hình kết nối

Chuỗi kết nối nằm trong `App.config` với tên `CaPheConnection`.

## Ghi chú

Repo hiện mới hoàn thành giai đoạn 1: phần khởi tạo mô hình dữ liệu và kết nối cơ sở dữ liệu.
