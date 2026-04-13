# Review Module GD7 - .NET WinForms POS

Phạm vi review: toàn bộ mã nguồn GD7 trong thư mục QuanLyQuanCaPhe.
Ngày review: 14/04/2026.

## 1. ✅ ĐÃ ĐẠT ĐƯỢC

### Chức năng đã hoàn thành
- Đăng nhập, phân vai trò cơ bản (Admin/Manager/Staff), điều hướng theo màn hình.
- Quản lý bàn, gọi món, lưu món chờ, thanh toán, tạo/cập nhật/hủy hóa đơn.
- Quản lý danh mục: món, loại món, nguyên liệu, khách hàng, nhân viên.
- Import CSV cho món, loại món, khách hàng, nhân viên.
- Tích hợp logging, correlation id, global exception handling.
- Sử dụng EF Core migrations + ràng buộc dữ liệu ở DbContext.

### Mức độ hoàn thiện
| Hạng mục | Mức độ | Nhận xét ngắn |
|---|---:|---|
| UI WinForms | 7/10 | Dùng được cho vận hành nội bộ, bố cục rõ, nhưng còn nhiều popup blocking và còn màn hình placeholder. |
| Logic nghiệp vụ | 7/10 | Luồng chính chạy ổn, có kiểm tra điều kiện nghiệp vụ; tuy nhiên vẫn còn vài điểm side-effect và hard-code quyền. |
| Database | 8/10 | Có check constraint, index, unique open invoice, decimal cho tiền; nền tảng tốt. |
| Performance | 6.5/10 | Có AsNoTracking/projection ở nhiều query, nhưng còn nhiều luồng sync trên UI và transaction chưa thống nhất với retry strategy. |

### Điểm tốt
- Chia lớp BUS/DAL/Forms rõ ràng, dễ đọc hơn các stage trước.
- Nhiều truy vấn đã tối ưu hướng read-model và filter SQL.
- Có tư duy logging và chuẩn hóa xử lý lỗi ở mức hệ thống.
- Nghiệp vụ gọi món có kiểm tra tồn kho và rollback khi thiếu nguyên liệu.

## 2. ⚠️ CÒN THIẾU

- Chưa có test tự động (unit/integration) cho các luồng rủi ro cao.
- Chưa có cơ chế hiển thị thông tin user đăng nhập xuyên suốt UI (hàm có nhưng còn để trống).
- Chưa tách riêng feature quyền cho Khách hàng, hiện còn dùng quyền Menu.
- Chưa hoàn thiện module thống kê/hóa đơn ở một số điểm điều hướng (còn thông báo đang phát triển).
- Chưa có chuẩn thống nhất response lỗi ở toàn bộ BUS (vẫn tồn tại API throw exception kiểu legacy).
- Chưa đạt production-ready về bảo mật tài khoản và consistency authorization.

## 3. 🐞 LỖI ĐÃ FIX

- Đã fix import quyền Create/Update tách biệt cho Mon/LoaiMon/KhachHang/NhanVien.
  - Nguyên nhân cũ: có thể có 1 quyền nhưng thực hiện cả thêm và cập nhật.
  - Hiện tại: đã kiểm soát theo từng action, ổn định.

- Đã fix chặn gán role trái phép khi import nhân viên.
  - Nguyên nhân cũ: dễ phát sinh gán quyền vượt thẩm quyền.
  - Hiện tại: role phải tồn tại và phải thuộc phạm vi có thể gán, ổn định.

- Đã fix nền tảng diagnostics (log + correlation id + map mã lỗi).
  - Nguyên nhân cũ: lỗi khó truy vết khi chạy thực tế.
  - Hiện tại: có log theo ngữ cảnh và mã lỗi, ổn định tốt cho debug vận hành.

- Đã fix luồng đăng xuất quay lại đăng nhập.
  - Nguyên nhân cũ: đăng xuất dễ rơi vào đóng app hoặc vòng đời form chưa chuẩn.
  - Hiện tại: Program chạy vòng login-main, ổn định.

## 4. 🔧 CÁCH FIX THÀNH CÔNG

- Tách kiểm tra quyền theo feature/action và áp gate ngay tại BUS.
  - Giải pháp: chuẩn hóa PermissionFeatures + PermissionActions, BUS check trước DAL.
  - Vì sao hiệu quả: giảm bypass quyền từ UI và kiểm soát nghiệp vụ tập trung.

- Siết import bằng cờ quyền cho phép thêm/cập nhật.
  - Giải pháp: truyền coQuyenThemMoi/coQuyenCapNhat xuống DAL, record nào sai quyền thì bỏ qua.
  - Vì sao hiệu quả: không còn tình trạng import vượt quyền.

- Bổ sung diagnostics cấp ứng dụng.
  - Giải pháp: AppLogger, CorrelationContext, AppExceptionHandler, hook global exception.
  - Vì sao hiệu quả: truy vết được lỗi theo correlation id, giảm thời gian khoanh vùng sự cố.

- Nâng chất lượng transaction ở một số luồng trọng yếu.
  - Giải pháp: áp transaction cho flow gọi món/thu tiền và ghi audit theo từng bước.
  - Vì sao hiệu quả: giảm dữ liệu dở dang khi luồng nghiệp vụ thất bại giữa chừng.

## 5. ❗ LỖI TỒN ĐỘNG

| Lỗi còn lại | Mức độ | Ảnh hưởng |
|---|---|---|
| Fallback mật khẩu mặc định 123456 trong thêm/cập nhật/import nhân viên | High | Rủi ro bảo mật cao, dễ bị chiếm tài khoản mới tạo/import. |
| PermissionDAL.CheckPermission có gọi đồng bộ dữ liệu quyền và SaveChanges (read-path có side-effect) | High | Khó audit, tăng lock/contention, hành vi khó dự đoán khi chỉ kiểm tra quyền. |
| DAL hóa đơn/bán hàng tự sinh nhân viên mặc định và khách lẻ | High | Sai audit nghiệp vụ, không truy đúng người thao tác thực tế. |
| Dùng BeginTransaction trực tiếp trong bối cảnh EnableRetryOnFailure | Medium | Tiềm ẩn lỗi runtime transaction strategy ở môi trường retry thực sự. |
| Logic quyền bị trộn matrix + hard-code role trong UI (IsStaff, Admin bypass) | Medium | Dễ lệch hành vi quyền giữa các màn hình, khó mở rộng. |
| Parser CSV tự viết còn đơn giản, chưa xử lý đầy đủ edge cases | Medium | Dễ sai lệch import khi dữ liệu thực tế có quote/escape phức tạp. |
| Nhiều điều hướng còn thông báo đang phát triển | Low | Trải nghiệm chưa hoàn thiện cho người dùng cuối. |

## 6. 🚀 ĐỀ XUẤT CẢI TIẾN

### Refactor code
- Tách read-path authorization khỏi bootstrap/sync dữ liệu quyền.
- Chuẩn hóa hợp đồng lỗi BUS theo một kiểu duy nhất (Result pattern).
- Loại bỏ hoàn toàn logic role hard-code ở UI, chỉ dùng permission matrix.

### Tối ưu performance
- Chuyển các thao tác nặng sang async end-to-end (Form -> BUS -> DAL).
- Chuẩn hóa pattern execution strategy + transaction cho mọi luồng ghi nhiều bước.
- Bổ sung cache ngắn hạn cho danh mục ít đổi (loại món, cấu hình quyền).

### Cải thiện UI/UX
- Giảm MessageBox blocking, chuyển sang inline notification ở các tác vụ thường xuyên.
- Ẩn hoặc khóa cứng các menu chưa hoàn thiện thay vì mở rồi báo đang phát triển.
- Hiển thị rõ user đang đăng nhập và quyền hiện tại trên top bar.

### Nâng cấp kiến trúc
- Bổ sung test project cho các flow quan trọng: permission, order, payment, import.
- Tách rõ Application Service cho use-case lớn để giảm logic dàn trải ở form.
- Bổ sung audit field đầy đủ cho phiếu nhập/xuất kho theo user thao tác.

## 7. 📊 ĐÁNH GIÁ TỔNG QUAN

- Điểm tổng quan: 7.0/10.
- Mức độ sẵn sàng deploy: Beta nội bộ (không phải Production).
- Nhận xét reviewer thực tế:
  - GD7 có nền tảng kỹ thuật khá tốt, chạy được luồng POS chính và có tư duy kiến trúc rõ hơn nhiều bản trước.
  - Tuy nhiên còn 3 nhóm lỗi cần xử lý trước khi production: bảo mật mật khẩu mặc định, side-effect trong check quyền, và audit người thao tác hóa đơn.
  - Sau khi xử lý triệt để nhóm High ở mục 5, có thể nâng lên mức production candidate.
