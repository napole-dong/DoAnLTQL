# Review Đồ Án GD7 (Góc Nhìn Senior Developer)

## 0) Findings Ưu Tiên (Sắp Theo Mức Độ Nghiêm Trọng)

### CRITICAL

1. **Mật khẩu mặc định `123456` xuất hiện ở nhiều luồng tạo/import nhân viên**
- Bằng chứng:
	- [QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L83](QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L83)
	- [QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L132](QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L132)
	- [QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L246](QuanLyQuanCaPhe/DAL/NhanVienDAL.cs#L246)
	- [QuanLyQuanCaPhe/BUS/NhanVienBUS.cs#L221](QuanLyQuanCaPhe/BUS/NhanVienBUS.cs#L221)
- Nhận xét thẳng: đây là lỗi bảo mật nghiêm trọng, không được phép tồn tại khi đi production.
- Hậu quả: tài khoản mới/import có mật khẩu yếu dự đoán được, tăng rủi ro takeover hàng loạt.

### HIGH

2. **Hàm kiểm tra quyền có side-effect ghi DB (read-path nhưng lại mutate state)**
- Bằng chứng:
	- [QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L53](QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L53)
	- [QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L81](QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L81)
	- [QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L105](QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L105)
	- [QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L243](QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L243)
	- [QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L312](QuanLyQuanCaPhe/DAL/PermissionDAL.cs#L312)
- Nhận xét thẳng: logic permission check phải **pure read**, không được âm thầm sửa dữ liệu mỗi lần check.
- Hậu quả: khó audit, khó debug, khó scale, tăng lock/contention khi nhiều user online.

3. **Tạo hóa đơn/gọi món tự sinh “Nhân viên bán hàng” và “Khách lẻ” trong DAL**
- Bằng chứng:
	- [QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L99](QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L99)
	- [QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L631](QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L631)
	- [QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L641](QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L641)
	- [QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L95](QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L95)
	- [QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L562](QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L562)
	- [QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L575](QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L575)
- Nhận xét thẳng: đây là sai chuẩn audit nghiệp vụ. Hóa đơn phải gắn user/session thực tế đang thao tác.

4. **Chiến lược retry của EF đang bật, nhưng vẫn dùng user transaction trực tiếp ở một số DAL**
- Bằng chứng:
	- [QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L424](QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L424)
	- [QuanLyQuanCaPhe/DAL/NguyenLieuDAL.cs#L96](QuanLyQuanCaPhe/DAL/NguyenLieuDAL.cs#L96)
- Nhận xét thẳng: pattern này dễ phát sinh lỗi runtime liên quan execution strategy nếu môi trường DB có retry thực sự.

5. **Phân quyền đang bị trộn role cứng + matrix quyền, dẫn đến khó mở rộng**
- Bằng chứng:
	- [QuanLyQuanCaPhe/Services/Auth/Permission.cs#L78](QuanLyQuanCaPhe/Services/Auth/Permission.cs#L78)
	- [QuanLyQuanCaPhe/Services/Auth/Permission.cs#L80](QuanLyQuanCaPhe/Services/Auth/Permission.cs#L80)
	- [QuanLyQuanCaPhe/Forms/frmBanHang.cs#L212](QuanLyQuanCaPhe/Forms/frmBanHang.cs#L212)
	- [QuanLyQuanCaPhe/Forms/frmBanHang.cs#L753](QuanLyQuanCaPhe/Forms/frmBanHang.cs#L753)
- Nhận xét thẳng: đã có matrix thì đừng chặn cứng theo role ở UI nữa. Hiện tại đang mâu thuẫn triết lý.

### MEDIUM

6. **CSV parser tự viết quá đơn giản, dễ lỗi dữ liệu thực tế**
- Bằng chứng:
	- [QuanLyQuanCaPhe/BUS/BusInputHelper.cs#L37](QuanLyQuanCaPhe/BUS/BusInputHelper.cs#L37)
	- [QuanLyQuanCaPhe/BUS/BusInputHelper.cs#L45](QuanLyQuanCaPhe/BUS/BusInputHelper.cs#L45)
	- [QuanLyQuanCaPhe/BUS/BusInputHelper.cs#L47](QuanLyQuanCaPhe/BUS/BusInputHelper.cs#L47)
- Nhận xét thẳng: parser toggle quote kiểu này sẽ vỡ với escaped quote, dữ liệu import lớn sẽ lỗi âm thầm.

7. **Luồng chuyển/gộp bàn có nhiều bước cập nhật nhưng không mở transaction tường minh**
- Bằng chứng:
	- [QuanLyQuanCaPhe/DAL/BanDAL.cs#L157](QuanLyQuanCaPhe/DAL/BanDAL.cs#L157)
	- [QuanLyQuanCaPhe/DAL/BanDAL.cs#L191](QuanLyQuanCaPhe/DAL/BanDAL.cs#L191)
	- [QuanLyQuanCaPhe/DAL/BanDAL.cs#L233](QuanLyQuanCaPhe/DAL/BanDAL.cs#L233)
- Nhận xét thẳng: luồng nhiều thao tác ghi phải transaction hóa rõ ràng để chống trạng thái dở dang.

8. **Quản lý khách hàng đang “mượn” quyền Menu thay vì feature riêng**
- Bằng chứng:
	- [QuanLyQuanCaPhe/BUS/KhachHangBUS.cs#L14](QuanLyQuanCaPhe/BUS/KhachHangBUS.cs#L14)
	- [QuanLyQuanCaPhe/BUS/KhachHangBUS.cs#L24](QuanLyQuanCaPhe/BUS/KhachHangBUS.cs#L24)
	- [QuanLyQuanCaPhe/BUS/KhachHangBUS.cs#L91](QuanLyQuanCaPhe/BUS/KhachHangBUS.cs#L91)
- Nhận xét thẳng: domain khách hàng không nên phụ thuộc quyền Menu. Thiết kế quyền đang dính chùm.

9. **Nhiều tính năng điều hướng chỉ là placeholder “đang phát triển”**
- Bằng chứng:
	- [QuanLyQuanCaPhe/Forms/frmBanHang.cs#L798](QuanLyQuanCaPhe/Forms/frmBanHang.cs#L798)
	- [QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L671](QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L671)
	- [QuanLyQuanCaPhe/Forms/frmNhanVien.cs#L478](QuanLyQuanCaPhe/Forms/frmNhanVien.cs#L478)
	- [QuanLyQuanCaPhe/Forms/frmQuanLiMon.cs#L779](QuanLyQuanCaPhe/Forms/frmQuanLiMon.cs#L779)
- Nhận xét thẳng: demo đồ án ổn, nhưng chuẩn phần mềm vận hành thì đây là điểm trừ rất rõ.

10. **API tầng BUS chưa thống nhất style trả lỗi (vừa tuple, vừa throw Exception thô)**
- Bằng chứng:
	- [QuanLyQuanCaPhe/BUS/TaiKhoanBUS.cs#L11](QuanLyQuanCaPhe/BUS/TaiKhoanBUS.cs#L11)
	- [QuanLyQuanCaPhe/BUS/TaiKhoanBUS.cs#L15](QuanLyQuanCaPhe/BUS/TaiKhoanBUS.cs#L15)
	- [QuanLyQuanCaPhe/BUS/MonBUS.cs#L107](QuanLyQuanCaPhe/BUS/MonBUS.cs#L107)
	- [QuanLyQuanCaPhe/BUS/HoaDonBUS.cs#L98](QuanLyQuanCaPhe/BUS/HoaDonBUS.cs#L98)
- Nhận xét thẳng: codebase sẽ khó maintain nếu contract lỗi không thống nhất.

## Giả Định Và Câu Hỏi Mở

1. Giả định hệ thống đang hướng tới triển khai thực tế nội bộ, không chỉ demo môn học.
2. Nếu chỉ phục vụ demo ngắn hạn, một số lỗi vận hành có thể chấp nhận tạm; nếu đi production thì không.
3. Chưa thấy test project tự động trong repo GD7 (không có file test). Nếu có test ở repo khác, cần bổ sung link.

## 1) Đánh Giá Tổng Thể

- **Mức độ**: **Khá** (nhưng chưa đạt chuẩn production).
- **Nhận xét ngắn gọn kiểu giảng viên**: Làm được nhiều phần khó (3 lớp, EF migration, transaction ở vài luồng trọng yếu), nhưng vẫn còn lỗi kiến trúc và bảo mật không thể bỏ qua, đặc biệt là mật khẩu mặc định và permission check có side-effect ghi DB.

## 2) Phân Tích Chi Tiết

### ✅ Những gì đã làm tốt

1. Có tách lớp BUS/DAL/Forms tương đối rõ, dễ đọc hơn các bản GD trước.
2. DbContext có ràng buộc dữ liệu tương đối tốt:
	 - Check constraint trạng thái ở [QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L37](QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L37), [QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L89](QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L89), [QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L150](QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L150), [QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L225](QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L225).
	 - Unique open-invoice index ở [QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L166](QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L166).
	 - Cột tiền dùng decimal(18,2) ở nhiều điểm như [QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L99](QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L99).
3. Có logging và global exception handling khá bài bản ở [QuanLyQuanCaPhe/Program.cs#L19](QuanLyQuanCaPhe/Program.cs#L19), [QuanLyQuanCaPhe/Program.cs#L79](QuanLyQuanCaPhe/Program.cs#L79), [QuanLyQuanCaPhe/Program.cs#L95](QuanLyQuanCaPhe/Program.cs#L95).
4. Có transaction serializable cho các luồng quan trọng ở [QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L32](QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L32), [QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L77](QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L77).
5. Build hiện tại sạch: `dotnet build -p:UseAppHost=false` thành công.

### ⚠️ Những gì còn thiếu

1. Chưa có test tự động (unit/integration) cho nghiệp vụ lõi: phân quyền, gọi món, thanh toán, nhập kho, import CSV.
2. Chưa có module thống kê thật, còn placeholder ở nhiều form.
3. Chưa có cơ chế chuẩn để hiển thị user đăng nhập hiện tại; các hàm đang để trống như [QuanLyQuanCaPhe/Forms/frmBanHang.cs#L204](QuanLyQuanCaPhe/Forms/frmBanHang.cs#L204), [QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L134](QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L134).
4. Thiếu metadata audit theo người thao tác ở bảng kho/xuất kho (hiện entity chỉ có nguyên liệu, số lượng, thời gian, lý do).

### ❌ Những điểm sai / thiết kế chưa chuẩn

1. Mật khẩu mặc định `123456` ở nhiều đường dữ liệu.
2. Permission check tự sửa DB trong read-path.
3. DAL tự sinh nhân viên/khách mặc định khi tạo hóa đơn, làm sai bản chất truy vết người thao tác.
4. Quyền khách hàng bị dính vào feature Menu, không tách domain.
5. Luồng chuyển/gộp bàn không transaction hóa rõ ràng.
6. CSV parser tự viết không đủ robust cho dữ liệu thật.
7. API BUS không đồng nhất style xử lý lỗi.

### 🚀 Những gì nên cải thiện ngay

1. Xóa toàn bộ fallback mật khẩu `123456`, bắt buộc password policy (độ dài + complexity) hoặc random secret và reset bắt buộc lần đầu.
2. Tách `PermissionBootstrapService` chạy lúc startup/migration; `CheckPermission` chỉ được đọc.
3. Gắn `NhanVienID` theo session khi tạo hóa đơn/phiếu kho; cấm auto-create nhân viên mặc định trong DAL.
4. Chuẩn hóa transaction cho mọi luồng cập nhật nhiều bảng (đặc biệt chuyển/gộp bàn).
5. Thay CSV parser tự viết bằng thư viện chuẩn (ví dụ CsvHelper) + thêm validate cột bắt buộc.
6. Bổ sung test project cho 5 luồng rủi ro cao: permission matrix, import CSV, gọi món trừ kho, thu tiền, chuyển/gộp bàn.

## 3) Kiến Trúc

### Có nên dùng 3-layer không?

- **Có**. Với WinForms + nghiệp vụ CRUD nhiều, 3-layer là phù hợp và dễ dạy/dễ maintain.
- Nhưng hiện tại mới là “3-layer cơ học”, chưa đạt “3-layer chuẩn production”.

### Code hiện tại có vấn đề gì?

1. Ranh giới trách nhiệm chưa sạch: DAL vẫn chứa quyết định nghiệp vụ (auto-create nhân viên/khách, tự seed quyền trong check).
2. Permission logic bị lặp và trộn: vừa gate theo feature/action, vừa gate cứng theo role trong form.
3. Có API BUS legacy throw exception thô, làm contract không đồng nhất với phần còn lại.

### Cách refactor cụ thể

1. Tạo `Application/UseCase` layer mỏng cho các flow lớn (`GoiMonUseCase`, `ThuTienUseCase`, `NhapKhoUseCase`).
2. DAL chỉ còn query/command persistence thuần, không auto-sinh dữ liệu hệ thống.
3. Tạo `PermissionPolicyService` tập trung; form gọi 1 chỗ duy nhất, tránh hard-code `IsStaff` rải rác.
4. Chuẩn hóa response BUS theo `Result<T>` hoặc `OperationResult` thay vì trộn tuple + exception.
5. Tách import thành service riêng + validator riêng + report lỗi theo dòng.

## 4) Database

### Thiết kế đã chuẩn chưa?

- **Khá ổn ở mức nền tảng**: có FK, index, check-constraint, decimal cho tiền, unique open invoice.
- **Chưa chuẩn production đầy đủ** ở mặt audit và consistency nghiệp vụ.

### Có thiếu bảng / quan hệ không?

1. Thiếu quan hệ theo người thao tác trong kho:
	 - [QuanLyQuanCaPhe/Data/dtaPhieuNhapKho.cs](QuanLyQuanCaPhe/Data/dtaPhieuNhapKho.cs)
	 - [QuanLyQuanCaPhe/Data/dtaPhieuXuatKho.cs](QuanLyQuanCaPhe/Data/dtaPhieuXuatKho.cs)
	 -> nên thêm `NhanVienID` (hoặc `UserId`) để truy vết.
2. Thiếu ràng buộc unique ở DB cho dữ liệu thường xuyên cần unique theo nghiệp vụ (ví dụ số điện thoại khách nếu business yêu cầu).
3. Thiếu chiến lược soft-delete nhất quán trên các bảng master (hiện user soft delete, nhưng món/khách/bàn/nguyên liệu chủ yếu hard delete).

## 5) Phân Quyền

### Admin / Manager / Staff đã hợp lý chưa?

- **Ý tưởng template quyền là đúng hướng**.
- Nhưng implementation còn lỗi kiến trúc:
	- Admin bypass toàn bộ matrix ở [QuanLyQuanCaPhe/Services/Auth/Permission.cs#L78](QuanLyQuanCaPhe/Services/Auth/Permission.cs#L78).
	- UI vẫn chặn theo role cứng ở [QuanLyQuanCaPhe/Forms/frmBanHang.cs#L212](QuanLyQuanCaPhe/Forms/frmBanHang.cs#L212), [QuanLyQuanCaPhe/Forms/frmBanHang.cs#L753](QuanLyQuanCaPhe/Forms/frmBanHang.cs#L753).

### Logic có lỗi gì không?

1. `CheckPermission` gây side-effect ghi DB làm sai kỳ vọng hàm authorization.
2. Feature phân quyền chưa “domain-driven” (Khách hàng đang dùng chung feature Menu).
3. Một số form không gate quyền ngay từ lúc vào màn hình, UX thành “vào được nhưng thao tác không chạy”.

## 6) UI/UX

### Đánh giá trải nghiệm người dùng

- Điểm cộng:
	1. Form bán hàng có cải thiện bố cục, card món và filter động khá tốt.
	2. Các luồng chính có thông báo rõ cho người dùng.

- Điểm trừ:
	1. Quá phụ thuộc `MessageBox` blocking ở hầu hết thao tác, gây mỏi thao tác và gián đoạn nhịp làm việc.
	2. Nhiều entry trên menu điều hướng còn placeholder “đang phát triển”.
	3. Thông tin user đăng nhập chưa hiển thị dù đã có hook hàm.

### Cách cải thiện để giống phần mềm thực tế

1. Chuyển dần lỗi/validation sang inline validation (ErrorProvider + status bar), giảm popup blocking.
2. Hoàn thiện tối thiểu module thống kê hoặc tạm ẩn hẳn menu chưa xong trong bản release.
3. Hiển thị rõ user hiện tại + role + chi nhánh/ca làm trên topbar.
4. Thêm loading state và disable control theo tiến trình xử lý cho thao tác nặng.

## 7) Kết Luận

- **Điểm đề xuất (thang 10)**: **6.8/10**.
- **Vì sao**:
	1. Điểm cộng lớn ở nỗ lực kiến trúc 3 lớp, migration, ràng buộc dữ liệu, logging.
	2. Nhưng trừ điểm nặng vì lỗi bảo mật mật khẩu mặc định, sai chuẩn permission check (read mà ghi DB), và một số thiết kế nghiệp vụ chưa đạt chuẩn vận hành thật.
	3. Nếu fix triệt để nhóm lỗi CRITICAL/HIGH trong mục findings, đồ án có thể lên mức **7.8 - 8.2/10**.

---

## Tóm Tắt Nghiêm Khắc

Đây là đồ án có nền tảng tốt nhưng còn tư duy “chạy được là được” ở vài điểm trọng yếu. Nếu mục tiêu chỉ là demo môn học thì có thể qua. Nếu mục tiêu là code có thể bàn giao đi làm thật, bắt buộc phải xử lý ngay nhóm lỗi bảo mật và kiến trúc permission trước tiên.
