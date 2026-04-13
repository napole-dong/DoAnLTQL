# DoAnLTQL_GD10 - QuanLyQuanCaPhe

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
