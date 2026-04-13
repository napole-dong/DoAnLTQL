# DoAnLTQL_GD4 - Senior Review (.NET WinForms POS)

## Phạm vi đánh giá
- Module được review: toàn bộ source trong thư mục QuanLyQuanCaPhe (Forms, BUS, DAL, DTO, Data, Migrations).
- Cách đánh giá: đọc code theo luồng UI -> BUS -> DAL -> Data/Migrations và đối chiếu build thực tế.
- Trạng thái build hiện tại: dotnet build thành công, có 7 warning nullable.

## 1. ✅ ĐÃ ĐẠT ĐƯỢC

### Các chức năng đã hoàn thành
- Dashboard tổng quan: doanh thu ngày, số hóa đơn, trạng thái bàn, top món, danh sách hóa đơn gần đây.
- Quản lý món: CRUD, tìm kiếm/lọc, nhập-xuất CSV, xem trước ảnh local/URL, validate đầu vào.
- Quản lý loại món: CRUD, tìm kiếm/lọc, nhập-xuất CSV, chặn xóa loại đang dùng.
- Quản lý khách hàng: CRUD, tìm kiếm, nhập-xuất CSV, validate số điện thoại và chặn trùng.
- Quản lý nhân viên: CRUD, tìm kiếm, nhập-xuất CSV, quản lý tài khoản User và VaiTro.
- Quản lý bàn: thống kê bàn, sơ đồ bàn động, thêm/xóa bàn, chuyển bàn/gộp bàn theo hóa đơn mở.

### Mức độ hoàn thiện
- UI: 7/10.
- Logic nghiệp vụ: 7/10.
- Database và migration: 7/10.
- Performance ở quy mô nhỏ-vừa: 6/10.

### Điểm tốt
- Tách lớp khá rõ ràng theo mô hình Forms -> BUS -> DAL -> DTO/Data.
- Nghiệp vụ bảo vệ dữ liệu tham chiếu đã được quan tâm (chặn xóa khi đã phát sinh hóa đơn).
- Nhiều truy vấn đọc đã dùng AsNoTracking.
- Import CSV có thống kê thêm/cập nhật/bỏ qua, UX phản hồi rõ cho người dùng.

## 2. ⚠️ CÒN THIẾU

### Chức năng chưa có hoặc chưa đầy đủ
- Chưa có luồng đăng nhập trước khi vào hệ thống, app chạy thẳng dashboard.
- Chưa có module hóa đơn/bán hàng hoàn chỉnh trong GD4 (mới có nền dữ liệu và một phần điều hướng).
- Một số hướng điều hướng vẫn là placeholder (thống kê/hóa đơn ở màn nhân viên).
- Click bàn trên sơ đồ hiện mới hiển thị thông báo chọn bàn, chưa mở workflow thao tác bàn tại chỗ.

### Best practice chưa áp dụng đầy đủ
- Chưa hash mật khẩu (đang lưu plain text), còn fallback mật khẩu mặc định.
- Chưa có transaction bao toàn bộ các thao tác nhiều bước ở một số luồng ghi dữ liệu.
- Chưa có logging/telemetry khi catch exception.
- Chưa có test tự động cho BUS/DAL quan trọng.

### Những phần cần để đạt production-ready
- Bổ sung authentication + session + phân quyền action-level.
- Chuẩn hóa bảo mật dữ liệu nhạy cảm (hash/salt, rotation, policy mật khẩu).
- Hoàn thiện luồng bán hàng đầu-cuối và báo cáo vận hành.
- Bổ sung test và cơ chế theo dõi lỗi runtime.

## 3. 🐞 LỖI ĐÃ FIX

### Các lỗi đã gặp trước đây (thể hiện qua migration/refactor)
- Đã tách tài khoản khỏi bảng NhanVien sang bảng User riêng.
- Đã chuẩn hóa quyền từ field text sang bảng VaiTro + khóa ngoại VaiTroID.
- Đã bỏ các cột tài khoản khỏi NhanVien để tránh trùng trách nhiệm dữ liệu.
- Đã thêm unique index cho TenDangNhap, NhanVienID (1-1 User-NhanVien), TenVaiTro.
- Đã bổ sung nhiều guard ở BUS để chặn xóa dữ liệu đã phát sinh hóa đơn.

### Nguyên nhân gốc (ngắn gọn)
- Thiết kế ban đầu trộn thông tin hồ sơ nhân viên và tài khoản đăng nhập vào cùng một bảng.
- Vai trò lưu chuỗi tự do dẫn đến khó chuẩn hóa và khó bảo toàn toàn vẹn dữ liệu.
- Thiếu ràng buộc unique và guard nghiệp vụ gây rủi ro dữ liệu trùng/lệch.

### Tình trạng hiện tại
- Ổn định hơn đáng kể ở mức Dev/Beta nội bộ.
- Chưa đạt production vì còn thiếu bảo mật tài khoản và luồng vận hành đầy đủ.

## 4. 🔧 CÁCH FIX THÀNH CÔNG

### Cách đã xử lý
- Dùng migration để chuyển mô hình dữ liệu theo từng bước: tạo bảng mới, map dữ liệu cũ, áp ràng buộc, loại bỏ cột legacy.
- Cố định quan hệ dữ liệu bằng unique index và foreign key rõ ràng.
- Tăng guard nghiệp vụ ở BUS trước khi gọi DAL (kiểm tra dữ liệu vào, kiểm tra điều kiện xóa/cập nhật).

### Giải pháp kỹ thuật đã áp dụng
- DB: migration ThemBangUserPhanQuyen và ChuanHoaTaiKhoanVaVaiTro.
- ORM: cấu hình index unique và quan hệ 1-1/1-n trong CaPheDbContext.
- Code: tách luồng import/validate và trả kết quả theo DTO thống nhất.

### Vì sao cách này hiệu quả
- Giải quyết nguyên nhân tại mô hình dữ liệu, không chỉ vá ở UI.
- Tăng tính nhất quán dữ liệu giữa các màn hình quản trị.
- Giảm lỗi vận hành do thao tác xóa/sửa không hợp lệ.

## 5. ❗ LỖI TỒN ĐỘNG

| STT | Vấn đề | Mức độ | Ảnh hưởng |
|---|---|---|---|
| 1 | Mật khẩu đang lưu plain text và còn fallback mặc định 123456 | High | Rủi ro bảo mật nghiêm trọng, không đạt tiêu chuẩn production |
| 2 | App vào thẳng dashboard, chưa có cổng đăng nhập bắt buộc | High | Bỏ qua kiểm soát truy cập từ đầu phiên |
| 3 | Top món dashboard chưa lọc trạng thái hóa đơn đã thanh toán | Medium | Sai lệch KPI bán hàng |
| 4 | NhanVienDAL có các luồng SaveChanges nhiều bước không bao transaction tổng thể | Medium | Có thể phát sinh trạng thái dữ liệu nửa vời khi lỗi giữa chừng |
| 5 | Dashboard catch exception kiểu nuốt lỗi, không log | Medium | Khó truy vết lỗi thật khi vận hành |
| 6 | Một số chức năng còn placeholder thông báo đang phát triển | Medium | Trải nghiệm không liền mạch, chưa sẵn sàng demo end-to-end |
| 7 | Một số query lọc trên bộ nhớ sau khi lấy toàn bộ dữ liệu | Medium | Kém hiệu năng khi dữ liệu lớn |
| 8 | CSV parser thủ công lặp ở nhiều BUS, chưa xử lý đầy đủ edge case quote phức tạp | Medium | Có thể parse sai dữ liệu nhập |
| 9 | Connection string hardcode local + TrustServerCertificate=True | Low | Khó tách môi trường, giảm chuẩn an toàn khi triển khai thật |
| 10 | Còn comment rác trong code | Low | Ảnh hưởng chất lượng code review và tính chuyên nghiệp |
| 11 | 7 cảnh báo nullable chưa xử lý | Low | Tăng nguy cơ lỗi NullReference về sau |

### Một số bằng chứng chính trong code
- Startup bỏ qua login: [QuanLyQuanCaPhe/Program.cs#L14](QuanLyQuanCaPhe/Program.cs#L14).
- Password plain/default: [QuanLyQuanCaPhe/Data/dtaUser.cs#L9](QuanLyQuanCaPhe/Data/dtaUser.cs#L9), [QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L81](QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L81), [QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L117](QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L117).
- Top món chưa lọc trạng thái: [QuanLyQuanCaPhe/DAL/DashboardDAL.cs#L60](QuanLyQuanCaPhe/DAL/DashboardDAL.cs#L60).
- Nuốt lỗi dashboard: [QuanLyQuanCaPhe/Forms/frmDashboard.cs#L78](QuanLyQuanCaPhe/Forms/frmDashboard.cs#L78).
- Placeholder thống kê/hóa đơn: [QuanLyQuanCaPhe/Forms/frmNhanVien.cs#L47](QuanLyQuanCaPhe/Forms/frmNhanVien.cs#L47), [QuanLyQuanCaPhe/Forms/frmNhanVien.cs#L48](QuanLyQuanCaPhe/Forms/frmNhanVien.cs#L48).
- Lọc in-memory ở quản lý bàn: [QuanLyQuanCaPhe/DAL/BanDAL.cs#L29](QuanLyQuanCaPhe/DAL/BanDAL.cs#L29).
- Comment rác: [QuanLyQuanCaPhe/BUS/KhachHangBUS.cs#L190](QuanLyQuanCaPhe/BUS/KhachHangBUS.cs#L190).

## 6. 🚀 ĐỀ XUẤT CẢI TIẾN

### Refactor code
- Tách parser CSV dùng chung thành service duy nhất để loại bỏ lặp code ở 4 BUS.
- Thêm lớp Result/ErrorCode thống nhất thông điệp lỗi giữa BUS và UI.
- Chuẩn hóa async cho các luồng truy vấn và import nặng để tránh block UI.

### Tối ưu performance
- Đẩy toàn bộ filter/sort về SQL, tránh ToList sớm rồi mới Where trên memory.
- Thêm index cho các cột tìm kiếm thường dùng (TenBan, TenLoai, DienThoai, TenMon).
- Giảm SaveChanges lặp trong vòng lặp import, gom batch commit hợp lý.

### Cải thiện UI/UX
- Hoàn thiện luồng thao tác bàn trực tiếp từ sơ đồ bàn (xem hóa đơn mở, chuyển/gộp, thanh toán).
- Bổ sung trạng thái loading/disable nút trong thao tác lâu (nhập CSV, tổng hợp dashboard).
- Chuẩn hóa điều hướng, loại bỏ toàn bộ message placeholder.

### Nâng cấp kiến trúc
- Bổ sung authentication + authorization đầy đủ (login form, current user context, gate theo feature/action).
- Mã hóa mật khẩu bằng BCrypt và chính sách mật khẩu mạnh.
- Thêm logging tập trung, global exception handler, audit log cho thao tác quan trọng.
- Tách cấu hình theo môi trường (Dev/Test/Prod), không hardcode connection string trong app config local.

## 7. 📊 ĐÁNH GIÁ TỔNG QUAN

- Điểm tổng quan: 7.0/10.
- Mức sẵn sàng deploy: Beta (nội bộ/demo), chưa đạt Production.
- Nhận xét reviewer: Kiến trúc nền tảng và các module quản trị đã đi đúng hướng, migration cũng cho thấy team đã xử lý được các vấn đề thiết kế dữ liệu quan trọng. Tuy nhiên hệ thống còn thiếu các tiêu chí bắt buộc để chạy thật (đăng nhập/phân quyền đầy đủ, bảo mật mật khẩu, hoàn thiện luồng bán hàng end-to-end, logging và test tự động).
