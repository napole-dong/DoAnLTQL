# DoAnLTQL_GD9 - QuanLyQuanCaPhe

Cap nhat lan cuoi: 2026-04-14

## 1) Tong quan
GD9 la giai doan hardening cua he thong QuanLyQuanCaPhe theo huong on dinh van hanh, giam loi runtime va de deploy:
- WinForms + EF Core + SQL Server, mau 3 lop BUS/DAL/Data.
- Chuan hoa transaction retry-safe cho cac luong ghi du lieu.
- Tang cuong soft-delete va toan ven du lieu khi xoa/doi trang thai.
- Chuan hoa luong phan quyen va dieu huong UI qua presenter/service.

## 2) Cong nghe chinh
- .NET 10.0 (WinForms)
- Entity Framework Core 10 (SqlServer)
- Serilog (structured logging)
- BCrypt.Net-Next (hash mat khau)
- ClosedXML + QuestPDF (xuat Excel/PDF)
- Microsoft.ReportViewer.WinForms (in hoa don/bao cao)

## 3) Cau truc repo hien tai

### 3.1) Thu muc goc GD9
- `README.md`
- `QuanLyQuanCaPhe/`

### 3.2) Thu muc chinh trong `QuanLyQuanCaPhe`
- `App.config`, `Program.cs`, `QuanLyQuanCaPhe.csproj`, `QuanLyQuanCaPhe.slnx`
- `BUS/`: Business logic
- `DAL/`: Data access, transaction handling
- `Data/`: Entity + DbContext + mapping
- `DTO/`: Data transfer objects
- `Forms/`: WinForms UI
- `Presenters/`: Presenters tach logic khoi form
- `Services/`: Auth, Permission, Navigation, Diagnostics, Security, UI...
- `Reporting/`, `Reports/`: Thanh phan va template bao cao/in
- `Migrations/`: EF Core migrations
- `scripts/`: script ho tro local
- `.vs/`, `bin/`, `obj/`, `artifacts/`: output/build cache

### 3.3) Luu y ve thanh phan da thay doi
- Thu muc test `QuanLyQuanCaPhe.Tests` khong con trong source tree hien tai cua GD9.
- Thu muc `docs` khong con trong source tree hien tai cua GD9.

## 4) Hardening dang co trong code

### 4.1) Transaction retry-safe
- Da su dung helper chung `DAL/ExecutionStrategyTransactionRunner.cs` cho transaction + retry strategy.
- Pattern nay tranh xung dot khi `EnableRetryOnFailure` dang bat trong EF Core.

### 4.2) Soft-delete va toan ven du lieu
- `CaPheDbContext` ap dung query filter cho cac entity implement `ISoftDelete`.
- Intercept `SaveChanges/SaveChangesAsync` de chuyen thao tac delete thanh soft-delete.
- Migration `20260412180556_SoftDeleteLoaiMonAndStopSellingDelete` hardening luong xoa loai mon.

### 4.3) Startup va bootstrap
- Startup trong `Program.cs` giu vong lap dang nhap -> vao `frmBanHang` -> dang xuat quay lai dang nhap.
- Khi khoi dong, he thong dam bao tai khoan mac dinh ton tai va dong bo permission template.

## 5) Huong dan chay nhanh

### 5.1) Dieu kien
- Windows
- .NET SDK 10
- SQL Server (khuyen nghi local `SQLEXPRESS` cho dev)

### 5.2) Cau hinh ket noi DB
Co the cau hinh theo 3 lop uu tien (cao -> thap):
1. Environment variable (`CAPHE_CONNECTION_STRING__<ENV>`, `CAPHE_CONNECTION_STRING_<ENV>`, `CAPHE_CONNECTION_STRING`)
2. Secret file env (`CAPHE_CONNECTION_STRING_FILE__<ENV>`, `CAPHE_CONNECTION_STRING_FILE_<ENV>`, `CAPHE_CONNECTION_STRING_FILE`)
3. `App.config` (`CaPheConnection.<ENV>` roi toi `CaPheConnection`)

Thu tu xac dinh environment:
1. `CAPHE_ENVIRONMENT`
2. `DOTNET_ENVIRONMENT`
3. `ASPNETCORE_ENVIRONMENT`
4. `App.config` key `CaPheEnvironment`

### 5.3) Cau hinh password bootstrap tai khoan mac dinh
- Chi bat buoc o lan khoi tao dau tien khi database chua co tai khoan admin.
- Dat bien cho user hien tai (PowerShell):

```powershell
[Environment]::SetEnvironmentVariable("CAPHE_BOOTSTRAP_PASSWORD", "123", "User")
```

### 5.4) Build va chay
Chay trong thu muc `DoAnLTQL_GD9/QuanLyQuanCaPhe`:

```powershell
dotnet restore
dotnet build -p:EnableLocalDevCodeSigning=false
dotnet run --project .\QuanLyQuanCaPhe.csproj
```

### 5.5) Migration
```powershell
dotnet ef migrations add <TenMigration>
dotnet ef database update
```

## 6) Tai khoan bootstrap (dev)
- admin / <gia tri CAPHE_BOOTSTRAP_PASSWORD luc bootstrap>
- manager / <gia tri CAPHE_BOOTSTRAP_PASSWORD luc bootstrap>
- staff / <gia tri CAPHE_BOOTSTRAP_PASSWORD luc bootstrap>

Khuyen nghi: doi mat khau ngay sau khi setup moi truong.

## 7) Viec uu tien tiep theo
1. Khoi phuc/bo sung lai bo test tu dong cho GD9 theo muc tieu release.
2. Tang do phu test cho cac use-case P0 (checkout/cancel/concurrency).
3. Tiep tuc tach code-behind lon qua presenters/services.

## 8) Danh gia module/chuc nang theo goc nhin Senior Reviewer (.NET WinForms + POS)

Ngay review: 2026-04-14
Pham vi: GD9 (toan bo module van hanh chinh BanHang/HoaDon/Kho/PhanQuyen/Navigation/Logging).

### Findings uu tien (theo muc do)
1. **Medium - Chua dong nhat 2 nguon quyen (feature vs form-role)**
	 - Permission theo feature cho Staff dang khoa KhachHang (`DAL/PermissionDAL.cs`, staff `KhachHang` canView=false).
	 - Permission theo form-role lai cho Staff Add/Edit tren `frmKhachHang` (`Services/Permission/PermissionService.cs`).
	 - Sidebar va guard runtime dang check ca 2 lop (`Services/UI/SidebarUiHelper.cs`, `Forms/frmKhachHang.cs`) nen hanh vi thuc te de bi lech voi matrix form-role.
	 - Tac dong: kho debug quyen, de phat sinh bug nghiep vu khi thay doi role policy.

2. **Medium - Con sync-over-async trong luong nghiep vu data**
	 - Van con `.GetResult()` tren mot so path (`DAL/HoaDonDAL.cs`, `DAL/NguyenLieuDAL.cs`, `BUS/OrderService.cs`).
	 - Tac dong: co nguy co block thread/tre latency khi tai cao, kho scale va kho profile bottleneck.

3. **Low - UX thong tin user dang nhap chua hoan tat**
	 - Method `HienThiNguoiDungDangNhap()` trong `Forms/frmBanHang.cs` dang de trong.
	 - Tac dong: giam do ro ngu canh van hanh (ca lam viec, user hien tai).

4. **Low - Tai lieu chua dong bo 100% voi codebase**
	 - README truoc review dat tieu de GD10 trong khi source tree la GD9.
	 - Tac dong: nham lan khi ban giao/release note.

---

### 1. DA DAT DUOC

- Chuc nang da hoan thanh:
	- Dang nhap + vong lap dang xuat/dang nhap lai on dinh.
	- Ban hang tai quan/mang di, goi mon, doi mon, xoa mon, thu tien, huy hoa don.
	- Quan ly ban, quan ly mon, cong thuc, kho, khach hang, nhan vien, thong ke, audit log.
	- Bootstrap role/account mac dinh + dong bo quyen mac dinh khi startup.
	- In hoa don (co luong thermal print) va xuat bao cao.

- Muc do hoan thien:
	- UI: **8/10** (sidebar/permission/navigation da duoc chuan hoa, luong thao tac chinh mach lac).
	- Logic: **8.5/10** (state machine hoa don, transaction-safe order flow, rollback ton kho ro rang).
	- Database: **8.5/10** (constraint, index, soft-delete, rowversion concurrency, migration hardening).
	- Performance: **7.5/10** (da co retry strategy + query AsNoTracking cho nhieu read path, nhung con sync-over-async).

- Diem tot:
	- Co chia lop BUS/DAL/Data + Services ro rang, de theo doi va mo rong.
	- Co app logger + correlation + app exception mapping phuc vu production support.
	- Permission duoc gate o nhieu lop (UI + service), giam bypass truc tiep.
	- Luong inventory khi thao tac hoa don da duoc centralize vao OrderService.

### 2. CON THIEU

- Chua co bo test tu dong trong source tree GD9 (khong thay project test), nen do tin cay regression chua dat production-grade.
- Chua dong nhat hoan toan chinh sach quyen giua feature-permission va form-permission.
- Con mot so sync wrapper tren async path trong nghiep vu quan trong.
- Thong tin user context tren UI chinh (ban hang) chua hien thi ro.

### 3. LOI DA FIX

- Loi transaction voi EF retry strategy va user-initiated transaction da duoc xu ly bang helper transaction runner dung chung.
- Loi bootstrap tai khoan mac dinh (yeu cau mat khau ngay ca khi da co admin) da duoc harden dung dieu kien theo first-time bootstrap.
- Loi FK khi xoa LoaiMon co lien quan Mon da duoc xu ly bang stop-selling + soft-delete + query include soft-deleted khi can.
- Loi object disposed/circular navigation trong luong mo form da duoc harden bang guard + flow host child form an toan.

Tinh trang hien tai: cac loi tren da o trang thai **on dinh**, build pass va khong thay compile/runtime blocker obvious o muc source review.

### 4. CACH FIX THANH CONG

- Giai phap transaction:
	- Dong nhat vao `DAL/ExecutionStrategyTransactionRunner.cs`.
	- Commit condition qua `shouldCommit`, isolation level duoc khai bao ro.
	- Hieu qua vi dam bao tuong thich EF execution strategy + rollback nhat quan.

- Giai phap data integrity:
	- Them check constraint + unique index + rowversion + soft-delete query filter toan cuc (`Data/CaPheDbContext.cs`).
	- Hieu qua vi chan du lieu sai ngay tai DB layer, tranh phu thuoc hoan toan vao UI/BUS.

- Giai phap order/inventory:
	- Don diem nghiep vu vao `BUS/OrderService.cs` cho Add/Remove/Replace/Checkout/Cancel.
	- Hieu qua vi tranh lech ton kho giua nhieu flow, giam duplicate logic.

- Giai phap startup/security:
	- Bootstrap account + role + permission sync o startup (`Program.cs`, `DAL/TaiKhoanMacDinhDAL.cs`).
	- Hieu qua vi giam loi cai dat moi truong moi va giam state drift quyen.

### 5. LOI TON DONG

1. Permission matrix chua dong nhat (feature vs form-role)
	 - Muc do: **Medium**
	 - Anh huong: role Staff co the gap hanh vi khong nhat quan giua mong doi va thuc te truy cap module KhachHang.

2. Sync-over-async tren mot so luong nghiep vu
	 - Muc do: **Medium**
	 - Anh huong: co the tang do tre UI va giam kha nang chiu tai khi so thao tac tang cao.

3. UX context user chua day du tren form chinh
	 - Muc do: **Low**
	 - Anh huong: giam tinh minh bach van hanh (ai dang thao tac tai quay).

### 6. DE XUAT CAI TIEN

- Refactor:
	- Hop nhat 1 nguon su that cho permission matrix (uu tien DB/template + map form tu matrix do).
	- Giam tiep code-behind lon trong Forms, tiep tuc day logic qua Presenter/Service.

- Performance:
	- Chuyen het path nghiep vu chinh sang async end-to-end, loai bo `.GetResult()`.
	- Can nhac queue-based async logger thay vi fire-and-forget Task.Run moi lan ghi audit.

- UI/UX:
	- Hoan tat `HienThiNguoiDungDangNhap()` + hien role/ca lam viec ro rang.
	- Bo sung chi bao trang thai processing dong nhat tren tat ca thao tac lau.

- Kien truc:
	- Them test project cho P0 flow (checkout/cancel/chuyen-gop ban/concurrency).
	- Chuan hoa release checklist: migration stamp/check, bootstrap env var, smoke test role-permission.

### 7. DANH GIA TONG QUAN

- Diem tong quan: **8.2/10**
- Muc do san sang deploy: **Beta (near-production)**
- Nhan xet reviewer:
	- Codebase GD9 da dat muc hardening tot cho giai doan chay thu nghiem nghiem tuc.
	- De len production on dinh, can uu tien 3 viec: test tu dong P0, dong nhat permission matrix, va xoa sync-over-async o core flow.
