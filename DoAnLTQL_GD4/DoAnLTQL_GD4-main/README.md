# DoAnLTQL_GD4

## Tiến độ đã làm

### 1) Kiến trúc & công nghệ
- Đã tổ chức theo các lớp: `Forms` (UI), `BUS` (nghiệp vụ), `DAL` (truy xuất dữ liệu), `DTO` (truyền dữ liệu), `Data` (Entity `dta*`).
- Dự án đang dùng `WinForms` + `Entity Framework Core` + `SQL Server`.
- Target framework: `.NET 10` (`net10.0-windows`).

### 2) Cơ sở dữ liệu
- Đã có `DbContext` (`CaPheDbContext`) và cấu hình kết nối `CaPheConnection`.
- Đã tạo migration khởi tạo CSDL (`KhoiTaoCSDL`).
- Đã có các bảng chính:
  - `Ban`
  - `LoaiMon`
  - `Mon`
  - `KhachHang`
  - `NhanVien`
  - `HoaDon`
  - `HoaDon_ChiTiet`
- Thêm bảng:
  - `User`
  - `Vaitro`
### 3) Chức năng đã triển khai
- **Dashboard**
  - Thống kê tổng quan: doanh thu hôm nay, số hóa đơn, tình trạng bàn, số món bán.
  - Biểu đồ doanh thu 7 ngày.
  - Top món bán chạy.
  - Hóa đơn gần đây và hoạt động gần đây.

- **Quản lý món**
  - Thêm / sửa / xóa món.
  - Tìm kiếm, lọc danh sách.
  - Nhập / xuất CSV.
  - Validate dữ liệu đầu vào.
  - Hiển thị ảnh món từ đường dẫn local hoặc URL.

- **Quản lý loại món**
  - Thêm / sửa / xóa loại món.
  - Tìm kiếm, lọc danh sách.
  - Nhập / xuất CSV.
  - Ràng buộc không cho xóa loại đang được sử dụng.

- **Quản lý khách hàng**
  - Thêm / sửa / xóa khách hàng.
  - Tìm kiếm theo từ khóa.
  - Nhập / xuất CSV.
  - Validate họ tên, số điện thoại; kiểm tra trùng số điện thoại.

- **Quản lý bàn**
  - Hiển thị thống kê bàn (tổng bàn, đang phục vụ, trống, đặt trước).
  - Hiển thị sơ đồ bàn động.
  - Thêm / xóa bàn.
  - Chuyển bàn / gộp bàn theo nghiệp vụ hóa đơn.
- **Quản lý nhân viên**
  - Thêm / sửa / xóa nhân viên.
  - Tìm kiếm theo từ khóa.
  - Nhập / xuất CSV.
  - Validate.

## Những phần còn thiếu / cần hoàn thiện

### 1) Nghiệp vụ lõi bán hàng
- Chưa thấy đầy đủ luồng **lập hóa đơn tại quầy** (chọn bàn, thêm món, cập nhật số lượng, thanh toán).
- Chưa hoàn thiện quy trình **mở/đóng hóa đơn** và đồng bộ trạng thái bàn trong mọi trường hợp.

### 2) Báo cáo & thống kê
- Dashboard đã có dữ liệu cơ bản, nhưng chưa có báo cáo chi tiết theo:
  - Khoảng thời gian tùy chọn.
  - Theo món / theo loại món / theo nhân viên.
  - Xuất báo cáo (PDF/Excel) nếu yêu cầu đồ án cần.

- Viết test cho các nghiệp vụ quan trọng (món, khách hàng, bàn, hóa đơn).
- Đóng gói bản demo và hoàn thiện tài liệu nộp đồ án.
