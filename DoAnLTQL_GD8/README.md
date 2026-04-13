# DoAnLTQL_GD8 - Senior Developer Review (.NET WinForms POS)

Ngay review: 2026-04-14  
Pham vi: Toan bo GD8 (WinForms + BUS/DAL + EF Core + test project).

## 1. DA DAT DUOC

### Chuc nang da hoan thanh
- Dang nhap + vong lap dang xuat quay lai man hinh dang nhap da hoat dong on dinh.
	- Bang chung: Program.cs:34, Program.cs:54.
- Dieu huong form theo shell `frmBanHang` + host child form da co va dung duoc.
	- Bang chung: Forms/frmBanHang.cs:557, Forms/frmBanHang.cs:589, Services/Navigation/FormNavigationService.cs:38.
- Nghiep vu don hang da duoc centralize vao `OrderService` (add/remove/update/replace/checkout/cancel).
	- Bang chung: BUS/OrderService.cs:116, BUS/OrderService.cs:259, BUS/OrderService.cs:511.
- Invoice hardening da co `RowVersion` va xu ly concurrency conflict.
	- Bang chung: BUS/OrderService.cs:611, BUS/OrderService.cs:627, DAL/HoaDonDAL.cs:235.
- Transaction strategy tai cac luong quan trong da dua vao `CreateExecutionStrategy` + transaction.
	- Bang chung: DAL/BanDAL.cs:333, DAL/BanDAL.cs:368, DAL/HoaDonDAL.cs:91, DAL/HoaDonDAL.cs:101, BUS/OrderService.cs:585.
- Soft delete baseline da duoc trien khai muc he thong (query filter + intercept delete).
	- Bang chung: Data/CaPheDbContext.cs:524, Data/CaPheDbContext.cs:603, Data/CaPheDbContext.cs:617.
- Module thong ke + xuat du lieu CSV/Excel/PDF da co.
	- Bang chung: Forms/frmThongKe.cs:19, Forms/frmThongKe.cs:376, Services/Export/DataExportService.cs:14.

### Muc do hoan thien
- UI: Kha day du cho van hanh noi bo (ban hang, hoa don, nhan vien, thong ke).
- Logic: Khung nghiep vu chinh da co, dac biet la order lifecycle.
- Database: Mapping/constraint/transaction/coherence tot hon nhieu so voi cac giai doan truoc.
- Performance: Da giam freeze o mot so thao tac nang bang async + Task.Run, nhung chua dong bo toan bo.

### Diem tot
- Phan lop BUS/DAL ro rang, code nghiep vu co cau truc.
- Co global exception logging + audit log theo nghiep vu.
- Test automation da co va dang chay pass.

## 2. CON THIEU

- Chua dat production-ready ve security bootstrap account (chi tiet o muc 5).
- Password policy hien tai qua nhe, chua dat best practice production.
- Con nhieu luong UI goi DB sync trong UI thread (vi du thong ke), can async end-to-end.
	- Bang chung: Forms/frmThongKe.cs:214, Forms/frmThongKe.cs:266.
- Test chua cover day du cac edge case quan trong:
	- Remove item/replace item va tinh tong tien sau khi xoa dong.
	- Bootstrap account behavior sau khi doi mat khau.
	- Concurrency race thuc su (2 context song song), hien moi test stale token.

## 3. LOI DA FIX

### Da tung gap
- Logout dong app thay vi quay lai login.
- Side-effect trong read permission.
- Lech ton kho giua cac luong order.
- Circular dieu huong form.
- Freeze UI o cac thao tac nang.

### Nguyen nhan chinh
- Lifecycle form va startup chua duoc chot theo 1 pattern.
- Transaction va nghiep vu order bi phan tan nhieu noi.
- Thao tac nang chay tren UI thread.

### Tinh trang hien tai
- Cac nhom loi tren da on dinh hon ro ret trong GD8.
- Build va test local hien tai dang xanh:
	- `dotnet build -p:EnableLocalDevCodeSigning=false` -> PASS.
	- `dotnet test .\\QuanLyQuanCaPhe.Tests\\QuanLyQuanCaPhe.Tests.csproj -p:EnableLocalDevCodeSigning=false` -> PASS (35/35).

## 4. CACH FIX THANH CONG

- Dung execution strategy + transaction serializable cho luong ghi nhieu buoc.
	- Tai sao hieu qua: dam bao tinh nhat quan khi co transient fault va retry.
- Dua order mutation ve 1 service trung tam (`OrderService`).
	- Tai sao hieu qua: tranh split business rule, de audit va de test.
- Dung `RowVersion` + bat `DbUpdateConcurrencyException`.
	- Tai sao hieu qua: tranh ghi de du lieu khi thao tac dong thoi.
- Dung runtime child-host policy + navigation service.
	- Tai sao hieu qua: giam lap form, giam loop dieu huong va loi owner.
- Dung global soft-delete query filter + intercept SaveChanges.
	- Tai sao hieu qua: giu lich su, giam nguy co mat du lieu operational.

## 5. LOI TON DONG

### H1 - HIGH
- Van hardcode bootstrap password `"123"` trong startup, va reset lai password tai khoan mac dinh moi lan app chay.
	- Bang chung: Program.cs:81, DAL/TaiKhoanMacDinhDAL.cs:73, DAL/TaiKhoanMacDinhDAL.cs:75.
	- Anh huong: Bao mat rat cao, co the vo hieu hoa toan bo chinh sach doi mat khau.

### H2 - HIGH
- Nguy co sai tong tien hoa don khi xoa/doi mon do xoa entity chi tiet nhung khong bo khoi collection truoc khi tinh lai tong.
	- Bang chung: BUS/OrderService.cs:190, BUS/OrderService.cs:201, BUS/OrderService.cs:374, BUS/OrderService.cs:402.
	- Anh huong: Sai so tong tien tren hoa don, anh huong truc tiep den doanh thu va doi soat.

### M1 - MEDIUM
- Password strength validation thuc te chua enforce do manh.
	- Bang chung: Services/Auth/MatKhauService.cs:6, Services/Auth/MatKhauService.cs:39, Services/Auth/MatKhauService.cs:48.
	- Anh huong: Tang rui ro tai khoan yeu trong moi truong that.

### M2 - MEDIUM
- Test infra dat ten `SqliteTestScope` nhung dang dung SQL Server local.
	- Bang chung: QuanLyQuanCaPhe.Tests/TestInfrastructure/SqliteTestScope.cs:7, QuanLyQuanCaPhe.Tests/TestInfrastructure/SqliteTestScope.cs:19.
	- Anh huong: De gay hieu nham, kho setup CI/CD va kho reproducible tren moi may.

### M3 - MEDIUM
- `frmThongKe` van load/loc du lieu theo huong sync trong UI thread.
	- Bang chung: Forms/frmThongKe.cs:214, Forms/frmThongKe.cs:266.
	- Anh huong: Co the lag UI khi du lieu lon.

## 6. DE XUAT CAI TIEN

### Refactor code
- Tach ro policy bootstrap account: chi tao account mac dinh o lan khoi tao dau tien, khong reset mat khau moi startup.
- Chuyen cac luong DAL/BUS con lai sang async that su, han che `Task.Run` bao quanh ham sync.

### Toi uu performance
- Chuyen `frmThongKe` sang async load + cancellation token + paging.
- Bo sung index theo use-case thong ke (ngay, trang thai, nhan vien) neu du lieu lon.

### Cai thien UI/UX
- Co loading indicator theo tung panel (khong chi `UseWaitCursor`).
- Bo sung thong bao loi co ma loi nghiep vu de support van hanh.

### Nang cap kien truc
- Bo sung integration test scenario race thuc te (2 context) cho invoice/order.
- Chuan hoa test infra: doi ten `SqliteTestScope` hoac tach SQLServerTestScope ro rang.
- Tao migration checklist + release checklist cho production gate.

## 7. DANH GIA TONG QUAN

- Diem: 7.6/10.
- Muc do san sang deploy:
	- Dev: San sang.
	- Beta/UAT: San sang co dieu kien.
	- Production: Chua nen deploy cho den khi dong H1 + H2.
- Nhan xet reviewer:
	- GD8 da dat duoc nen tang ky thuat kha tot cho WinForms POS (phan lop, transaction, concurrency, soft delete, test automation).
	- Tuy nhien 2 loi HIGH hien tai anh huong truc tiep bao mat va tinh dung tien hoa don, can fix truoc khi go-live.


