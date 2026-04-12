# DoAnLTQL_GD9 - QuanLyQuanCaPhe

Cap nhat lan cuoi: 2026-04-13

## 1) Tong quan
GD9 tap trung vao hardening he thong QuanLyQuanCaPhe theo huong on dinh van hanh, giam loi runtime, va de test/deploy:
- WinForms + EF Core + SQL Server, mau 3 lop BUS/DAL/Data.
- Chuan hoa transaction retry-safe cho cac luong ghi phuc tap.
- Tang cuong soft-delete va toan ven du lieu khi xoa/doi trang thai.
- Chuan hoa giao dien sidebar theo role tren cac form chinh.
- Bo sung test BUS va integration test cho mot so nghiep vu quan trong.

## 2) Cong nghe chinh
- .NET 10.0 (WinForms)
- Entity Framework Core 10 (SqlServer)
- Serilog (structured logging)
- BCrypt.Net-Next (hash mat khau)
- ClosedXML + QuestPDF (xuat Excel/PDF)
- xUnit + FluentAssertions + Moq + AutoFixture (test)

## 3) Cau truc thu muc chinh
- `QuanLyQuanCaPhe/Data`: Entity, `CaPheDbContext`, mapping, query filter
- `QuanLyQuanCaPhe/DAL`: Truy cap du lieu, transaction runner, repository
- `QuanLyQuanCaPhe/BUS`: Nghiep vu
- `QuanLyQuanCaPhe/DTO`: Data transfer objects
- `QuanLyQuanCaPhe/Forms`: WinForms UI
- `QuanLyQuanCaPhe/Presenters`: Tach phu thuoc form
- `QuanLyQuanCaPhe/Services`: Auth, Navigation, UI helper, Diagnostics, Config
- `QuanLyQuanCaPhe/Migrations`: Lich su migration EF
- `QuanLyQuanCaPhe/QuanLyQuanCaPhe.Tests`: Unit + integration tests
- `QuanLyQuanCaPhe/docs`: Tai lieu ky thuat

## 4) Diem moi va hardening trong GD9

### 4.1) Transaction retry-safe
- Da su dung helper chung `DAL/ExecutionStrategyTransactionRunner.cs` cho transaction + retry strategy.
- Pattern nay giup tranh xung dot khi `EnableRetryOnFailure` dang bat trong EF Core.

### 4.2) Soft-delete va toan ven du lieu
- `CaPheDbContext` ap dung query filter cho cac entity implement `ISoftDelete`.
- Intercept `SaveChanges/SaveChangesAsync` de chuyen thao tac delete thanh soft-delete (set `IsDeleted`, `DeletedAt`, `DeletedBy`).
- Migration `20260412180556_SoftDeleteLoaiMonAndStopSellingDelete` bo sung hardening cho luong xoa loai mon.
- `LoaiMonDAL.XoaLoai` da xu ly theo huong:
  1. Chuyen cac mon active trong loai sang ngung ban (`TrangThai=0`, `DonGia=0`, `TrangThaiTextLegacy='Ngung ban'`).
  2. Soft-delete `LoaiMon` de tranh vo FK.

### 4.3) UI role consistency (sidebar)
- Da thong nhat menu sidebar runtime qua `Services/UI/SidebarUiHelper.cs` tren 9 form menu chinh.
- Co tai lieu manual check role/sidebar tai `QuanLyQuanCaPhe/docs/gd9-sidebar-role-manual-checklist-2026-04-12.md`.

### 4.4) Startup va bootstrap
- Startup trong `Program.cs` giu vong lap dang nhap -> vao `frmBanHang` -> dang xuat quay lai dang nhap.
- Khi khoi dong:
  - Dam bao tai khoan mac dinh ton tai (`admin`, `manager`, `staff`).
  - Dong bo permission template mac dinh.

### 4.5) Kiem thu
Test project da co cac nhom chinh:
- BUS tests: `BanHangFlowIntegrationTests`, `HoaDonBUSTests`, `OrderServiceTests`, `PermissionBUSTests`, `MonBUSTests`, ...
- DAL integration tests: transaction tests + `LoaiMonDalDeleteIntegrationTests`.

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

Mac dinh local trong `App.config` dang tro toi:
- `Server=localhost\SQLEXPRESS;Database=QuanLyQuanCaPhe;...`

### 5.3) Build
Chay trong thu muc `DoAnLTQL_GD9/QuanLyQuanCaPhe`:

```powershell
dotnet restore
dotnet build -p:EnableLocalDevCodeSigning=false
```

### 5.4) Chay ung dung

```powershell
dotnet run --project .\QuanLyQuanCaPhe.csproj
```

### 5.5) Chay test

```powershell
dotnet test .\QuanLyQuanCaPhe.Tests\QuanLyQuanCaPhe.Tests.csproj -p:EnableLocalDevCodeSigning=false
```

## 6) Migration va deploy
- Tao migration moi:

```powershell
dotnet ef migrations add <TenMigration>
```

- Apply migration:

```powershell
dotnet ef database update
```

- Quy trinh staging -> production tham khao:
  - `QuanLyQuanCaPhe/docs/migration-strategy-staging-prod.md`

## 7) Tai khoan bootstrap (dev)
- admin / 123
- manager / 123
- staff / 123

Khuyen nghi: doi mat khau ngay sau khi setup moi truong.

## 8) Tai lieu lien quan
- `QuanLyQuanCaPhe/docs/migration-strategy-staging-prod.md`
- `QuanLyQuanCaPhe/docs/gd9-sidebar-role-manual-checklist-2026-04-12.md`

## 9) Viec uu tien tiep theo
1. Tang do phu integration test cho cac use-case P0 (checkout/cancel/concurrency).
2. Chuan hoa them smoke test UI theo role matrix.
3. Tiep tuc ra soat code-behind lon de tach them qua presenters/services.
