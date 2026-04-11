# DoAnLTQL_GD8 - QuanLyQuanCaPhe

## 1) Tong quan
Day la giai doan GD8 cua do an Quan Ly Quan Ca Phe, su dung mo hinh WinForms + EF Core + SQL Server.

## 2) Cong nghe chinh
- .NET 10.0 (WinForms)
- Entity Framework Core 10
- SQL Server
- Serilog (ghi log)
- BCrypt (bam mat khau)
- ClosedXML + QuestPDF (xuat du lieu)

## 3) Cau truc thu muc chinh
- `QuanLyQuanCaPhe/Data`: Entity + DbContext
- `QuanLyQuanCaPhe/DAL`: Truy cap du lieu
- `QuanLyQuanCaPhe/BUS`: Nghiep vu
- `QuanLyQuanCaPhe/Forms`: Giao dien WinForms
- `QuanLyQuanCaPhe/Presenters`: Tach dependency cho Forms
- `QuanLyQuanCaPhe/Services`: Auth, Navigation, Diagnostics, Export
- `QuanLyQuanCaPhe/Migrations`: Lich su migration EF
- `QuanLyQuanCaPhe/docs`: Tai lieu ky thuat

## 4) Bao cao tien do GD8 (cap nhat 2026-04-11)

### 4.1) Da hoan thanh
1. Bao mat va quyen
- Da bo fallback mat khau mac dinh 123456 trong luong tao/sua/import user.
- Da bo side-effect `SaveChanges` trong read-path check permission.
- Da bo Admin bypass, UI gate theo feature/action matrix.
- Da tach feature `KhachHang` rieng trong ma tran quyen.

2. Navigation va lifecycle
- Da bo toan bo placeholder "dang duoc phat trien" de mo form that.
- Da co shell host (`MainForm`) de embed child form, tranh mo lap.
- Da fix loop dang xuat -> quay lai dang nhap thay vi tat app.
- Da fix loi circular owner (`A circular control reference...`) khi dieu huong qua lai.

3. Nghiep vu don hang, ton kho, hoa don
- Da chuan hoa qua `OrderService` (Add/Update/Remove/Checkout/Cancel).
- Da dong bo tru kho/hoan kho cho cac thao tac don hang.
- Da them `RowVersion`, `TongTien`, `ThanhTien`, `AuditLog` de harden invoice.
- Da chot nghiep vu chuyen/gop ban va bo sung mode `Mang di` + thao tac `Don ban`.

4. Du lieu va delete policy
- Da co baseline soft delete (`ISoftDelete`, query filter, intercept SaveChanges).
- Da bo sung restore + hard-delete guard cho cac module chinh.
- Da dung snapshot ten khach tren hoa don (`CustomerName`) de tranh mat du lieu lich su.

5. Hieu nang UI va do on dinh
- Da fix freeze UI o cac thao tac nang (thu tien, huy hoa don, luu hoa don, gop/chuyen ban).
- Da chuyen luong NhanVien sang async end-to-end (Form -> BUS -> DAL).
- Da harden DataGridView selection tranh xoa nham do `CurrentRow`.

6. Thong ke, xuat du lieu, test infra
- `frmThongKe` da nap du lieu theo bo loc ngay/tuan/thang.
- Da them `DataExportService` cho CSV/Excel/PDF va ap dung cho nhieu form.
- Da co huong build/test trong moi truong App Control (ky so local output).

### 4.2) Chua hoan thanh
1. Chua phu test tu dong day du cho cac use-case P0:
- Goi mon / them mon vao hoa don / checkout / huy hoa don.
- Permission matrix theo role-feature-action.
- Concurrency conflict (`RowVersion`) tren thao tac dong thoi.

2. Chua dong bo 100% transaction strategy toan DAL:
- Van can ra soat toan bo diem dung `BeginTransaction(...)` de bao dam nam trong `CreateExecutionStrategy().Execute(...)` khi context co retry.

3. Dashboard ThongKe moi dat muc co ban:
- Can bo sung KPI nang cao, chart tong hop, va bo loc linh hoat hon cho muc bao cao van hanh.

4. Refactor presenter/dependency chua phu het:
- Da co `Presenters`, nhung van can tiep tuc tach code-behind o mot so form lon.

### 4.3) Sai sot can chinh sua
1. Tai khoan bootstrap mac dinh (`admin/manager/staff`) hien van dung mat khau khoi tao de test:
- Can bat buoc doi mat khau lan dang nhap dau, hoac gate theo environment.

2. Build/test phu thuoc App Control policy tren may local:
- Neu quen ky so output se fail testhost/load assembly.
- Can chuan hoa script va huong dan run cho team.

3. Test provider dang nghieng ve SQL Server:
- Model co nhieu kieu cot dac thu SQL Server, chua co test strategy xuyen provider (SQLite/InMemory).

4. Mot so luong UI van co nguy co duplicate logic:
- Can tiep tuc gom helper chung cho validation, binding, loading state.

### 4.4) Nhung da fix tot (on dinh va co hieu qua ro)
1. Fix lech ton kho giua luong `BanHang` va `HoaDon`.
2. Fix dang xuat dong app bang vong lap dang nhap lai trong startup.
3. Fix circular form ownership khi dieu huong qua lai nhieu man hinh.
4. Fix freeze UI o cac nut thao tac nang tren HoaDon/QuanLiBan/NhanVien.
5. Fix xoa nham do DataGridView selection khong dong bo.
6. Fix side-effect permission read-path va bo Admin bypass khong can thiet.

### 4.5) Cach fix cac loi ton dong
1. Fix tai khoan bootstrap mac dinh:
- Them co `RequirePasswordChange` cho user bootstrap va bat buoc doi mat khau ngay sau login dau tien.
- Tach config theo environment: chi cho phep mat khau bootstrap trong `Development`, cam trong `Production`.

2. Fix phu thuoc ky so khi build/test tren App Control:
- Chuan hoa 2 script run chung cho team (`build-local.ps1`, `test-local.ps1`) de tu dong ky output truoc khi run.
- Cap nhat README phan setup bat buoc va checklist troubleshoot khi testhost fail load assembly.

3. Fix transaction strategy chua dong bo:
- Quet toan bo DAL de tim diem dung `BeginTransaction(...)`.
- Chuan hoa mau transaction: boc toan bo xu ly ghi trong `CreateExecutionStrategy().Execute(...)` hoac `ExecuteAsync(...)`.
- Bo sung integration test cho cac use-case co transaction nhieu buoc (checkout, chuyen/gop ban, nhap kho).

4. Fix thieu test tu dong P0:
- Uu tien test cho order lifecycle: add item, update quantity, remove item, checkout, cancel.
- Them test cho permission matrix (role-feature-action) va concurrency (`RowVersion`) khi 2 session cap nhat dong thoi.

5. Fix dashboard ThongKe moi o muc co ban:
- Bo sung KPI bat buoc: doanh thu ngay/thang, top mon, top khung gio, ti le huy don.
- Them preset bo loc nhanh va xuat dashboard snapshot de doi van hanh doi chieu so lieu.

6. Fix duplicate logic tren UI:
- Tao helper chung cho validation input, loading state, debounce tim kiem va xu ly DataGridView selection.
- Tach tiep code-behind sang Presenter theo uu tien form co tan suat thay doi cao (`frmHoaDon`, `frmBanHang`, `frmNhanVien`).

7. Fix test strategy lech SQL Server:
- Giu bo test chinh tren SQL Server local de bao toan mapping thuc te.
- Them mot lop smoke test provider-doc-lap cho logic BUS thuần (khong phu thuoc SQL type dac thu).

8. Fix theo thu tu uu tien de dong GD8 an toan:
- P0 tuan 1: transaction strategy + test order/concurrency.
- P1 tuan 2: bootstrap password policy + dashboard KPI + presenter cleanup.

### 4.6) Cach de fix thanh cong cac loi o 4.4
1. Loi lech ton kho giua `BanHang` va `HoaDon`:
- Bat buoc 1 diem vao nghiep vu qua `OrderService`; cam form goi truc tiep DAL order/invoice.
- Viet integration test doi chieu ton kho truoc-sau cho AddItem, Checkout, Cancel.
- Ghi audit theo `HoaDonId` va `MonId` de truy vet neu so lieu bi lech.

2. Loi dang xuat dong app thay vi quay lai dang nhap:
- Giu startup theo vong lap login, logout chi dat co dang nhap lai va dong root form.
- Khong goi `Application.Exit()` trong cac child form.
- Them smoke test 3-5 vong dang nhap/dang xuat lien tiep.

3. Loi circular ownership khi dieu huong form:
- Khong dung `Show(owner)` cho cac form menu qua lai nhau.
- Dieu huong qua 1 service duy nhat (host child form trong shell) de tranh vong tham chieu.
- Neu form da ton tai thi focus/activate thay vi tao moi.

4. Loi freeze UI o thao tac nang:
- Chuyen thao tac I/O sang async/await, trong luc chay thi disable nut + bat wait cursor.
- Cam truy van DB sync tren UI thread o cac event click/load lon.
- Them log thoi gian xu ly, canh bao khi thao tac vuot nguong (vd > 1s).

5. Loi xoa nham do DataGridView selection khong dong bo:
- Xoa dua tren `SelectedRows` + ID snapshot, khong dua vao `CurrentRow`.
- Sau khi bind lai du lieu: `ClearSelection`, dat `CurrentCell = null`, roi moi restore dong can chon.
- Bo sung helper selection dung chung va test thao tac xoa lien tiep.

6. Loi permission read-path co side-effect va Admin bypass:
- Dam bao `CheckPermission` chi doc du lieu, khong `SaveChanges`.
- Khong hard-code bypass theo role; moi role deu di qua matrix feature/action.
- Bo sung regression test cho 3 role (admin/manager/staff) tren cac action quan trong.

7. Cach xac nhan fix thanh cong (Definition of Done):
- Build PASS (`EnableLocalDevCodeSigning=false` neu can).
- Test PASS cho ca happy path + negative path + concurrency path.
- Smoke test tren UI khong treo, khong vo navigation, khong sai permission.
- Khong phat sinh loi moi trong log exception sau 1 ngay van hanh thu.

## 5) Tinh trang build
- Trang thai: PASS trong moi truong local.
- Lenh build tham chieu:

```powershell
dotnet build -p:EnableLocalDevCodeSigning=false
```

## 6) Ke hoach tiep theo de dong GD8
1. Hoan tat test regression cho order + permission + concurrency.
2. Ra soat transaction strategy cho toan bo DAL con lai.
3. Chot policy tai khoan bootstrap va doi mat khau lan dau.
4. Nang cap dashboard ThongKe (KPI/charts) theo yeu cau nghiep vu.

## 7) Ghi chu van hanh
- Moi thay doi schema phai di kem migration va tai lieu rollback.
- Khong merge len nhanh chinh neu chua qua build va smoke test tren DB that.

