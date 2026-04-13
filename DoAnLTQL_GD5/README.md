# DoAnLTQL_GD5 - Bao Cao Review Senior (.NET WinForms POS)

Pham vi review:
- Solution: `DoAnLTQL_GD5/QuanLyQuanCaPhe`
- Huong review: Senior Developer / Reviewer cho WinForms POS
- Ngay review: 2026-04-14

## 1. ✅ DA DAT DUOC

### Chuc nang da hoan thanh
- Quan ly mon va loai mon:
	- CRUD mon, CRUD loai mon.
	- Tim kiem theo tu khoa.
	- Nhap/Xuat CSV.
	- Validate du lieu dau vao.
	- Co xu ly nghiep vu khi xoa loai dang duoc su dung (cho phep chuyen mon sang loai khac truoc khi xoa).
- Quan ly ban:
	- Dashboard thong ke ban.
	- So do ban dong theo trang thai.
	- Them/xoa ban co rang buoc.
	- Chuyen ban/gop ban dua tren hoa don dang mo.
- Quan ly khach hang:
	- CRUD + tim kiem + nhap/xuat CSV.
	- Validate SDT, chong trung SDT.
	- Chan xoa neu da phat sinh hoa don.
- Quan ly nhan vien:
	- CRUD + tim kiem + nhap/xuat CSV.
	- Quan ly user/vai tro qua bang `User` va `VaiTro`.
	- Chan xoa neu da phat sinh hoa don.
- Quan ly kho nguyen lieu:
	- CRUD + tim kiem + xuat CSV.
	- Tu dong tinh trang thai ton kho theo muc canh bao.

### Danh gia muc do hoan thien
- UI: 7/10 (form quan tri da day du, bo cuc on; mot so man hinh cot loi con dang shell).
- Logic: 7/10 (validation va rule nghiep vu co day du cho CRUD quan tri).
- Database: 6.5/10 (model EF va migration chinh tuong doi tot, da tach quyen/role).
- Performance: 5.5/10 (nhieu form auto-refresh 500ms, truy van lap lai lien tuc).

### Diem tot
- Tach lop `Forms/BUS/DAL/DTO/Data` ro rang, de bao tri.
- Nhieu truy van dung `AsNoTracking()` cho read-only.
- Co business guard quan trong: khong xoa du lieu da lien quan hoa don.
- Co migration chuan hoa auth tu string role sang bang role + FK.
- UX thao tac quan tri kha hop ly: confirm khi xoa, thong bao ket qua ro rang.

## 2. ⚠️ CON THIEU

- Chua co nghiep vu cot loi POS:
	- `frmBanHang` chua co code xu ly.
	- `frmHoaDon` chua co code xu ly.
- Chua co luong dang nhap + phan quyen thuc thi tren UI (co bang user/role nhung chua gate chuc nang theo role).
- Nut `Thong ke` va `Hoa don` trong `frmNhanVien` moi la placeholder thong bao.
- Chuc nang `Nhap kho` hien tai chi reload du lieu, chua co import/chung tu nhap kho.
- Chua co test tu dong (unit test/integration test) cho BUS/DAL.
- Chua co logging/exception handling tap trung.
- Chua co profile cau hinh theo moi truong (dev/staging/prod).

## 3. 🐞 LOI DA FIX

Du lieu cho thay da co nhung nhom loi da duoc xu ly trong cac ban truoc:

- Da chuan hoa tai khoan va vai tro:
	- Tach `VaiTro` thanh bang rieng.
	- Chuyen du lieu role cu sang `VaiTroID` qua migration SQL.
	- Them unique index cho `User.TenDangNhap` va `User.NhanVienID`.
- Da loai bo thong tin dang nhap khoi bang `NhanVien` (tranh lap du lieu auth).
- Da bo sung cot `Mon.TrangThai` va `LoaiMon.MoTa` de phuc vu nghiep vu hien thi/quan ly.
- Da bo sung guard cho cac thao tac xoa du lieu lien quan hoa don (Mon/Loai/Ban/Khach/NhanVien).
- Da bo sung luong chuyen/gop ban va merge chi tiet hoa don.

Tinh trang hien tai: cac nhom loi tren da o muc on dinh cho pham vi CRUD quan tri.

## 4. 🔧 CACH FIX THANH CONG

- Fix bang migration + data migration script:
	- Tao bang `VaiTro`, backfill du lieu role cu, sau do rang buoc FK.
	- Cach nay dung vi tranh mat du lieu khi chuan hoa schema, dong thoi dam bao toan ven quan he.
- Fix bang business rule tai BUS:
	- Validate input truoc DAL.
	- Chan thao tac xoa neu da phat sinh giao dich.
	- Cach nay hieu qua vi loi duoc chan tu tang nghiep vu thay vi cho no vo toi DB exception.
- Fix UX thao tac nhay cam:
	- Confirm truoc khi xoa.
	- Thong bao ket qua thanh cong/that bai ro rang.
	- Cach nay giam thao tac nham va de van hanh thuc te.
- Fix quan tri danh muc:
	- Cho phep chuyen mon sang loai khac roi xoa loai cu.
	- Cach nay giai quyet conflict du lieu thay vi ep nguoi dung thao tac tay trong DB.

## 5. ❗ LOI TON DONG

- High - Entry point app dang `Application.Run(new Forms.frmBanHang())` trong khi `frmBanHang` chua co nghiep vu.
	- Anh huong: vao app nhung khong ban hang duoc, luong POS cot loi bi block.
- High - Mat khau dang luu plain text, co mat khau mac dinh `123456`.
	- Anh huong: rui ro bao mat nghiem trong, khong dat chuan production.
- High - Chua co co che dang nhap/xac thuc/authorize thuc thi theo role.
	- Anh huong: role trong DB gan nhu chua phat huy gia tri bao mat.
- High - Chua co migration chinh thuc tao bang `NguyenLieu` (model co, migration lich su chua thay lenh tao bang).
	- Anh huong: khoi tao DB moi co nguy co thieu bang, gay loi runtime.
- Medium - Auto-refresh 500ms tren nhieu form (`frmQuanLiBan`, `frmNhanVien`, `frmKhachHang`, `frmQuanLiKho`).
	- Anh huong: DB bi query lien tuc, de ton tai nguyen va giam do muot khi scale.
- Medium - Parser CSV tu viet con don gian, chua bao phu day du case quote phuc tap theo chuan RFC.
	- Anh huong: co the doc sai du lieu o file CSV phuc tap.
- Medium - Mot so xu ly query/filter chua toi uu (vi du co cho materialize roi moi filter).
	- Anh huong: tang tai RAM/CPU khi du lieu lon.
- Low - Build con warning nullable (`CS8618`) o mot so entity.
	- Anh huong: khong vo build nhung giam do chat che null-safety.

## 6. 🚀 DE XUAT CAI TIEN

### Refactor code
- Them abstraction `IRepository`/`IUnitOfWork` cho operation phuc tap (nhat la gop/chuyen ban, import).
- Dong bo mau `Result<T>` thay vi tuple roi rac de de logging/telemetry.
- Tach CSV parser sang thu vien chuan (vd. CsvHelper) de giam bug parser.

### Toi uu performance
- Giam tan suat auto-refresh (2-5 giay) hoac chuyen sang event-driven refresh.
- Toi uu query va filter xuong DB (tranh `ToList()` som roi filter memory).
- Co che debounce cho tim kiem text.

### Cai thien UI/UX
- Hoan thien full workflow ban hang: chon ban -> goi mon -> tam tinh -> thanh toan -> in hoa don.
- Hoan thien man hinh hoa don (lich su, loc theo ngay/ban/nhan vien, xem chi tiet).
- Bo sung role-based UI gate (an/hien/chong thao tac theo quyen).

### Nang cap kien truc
- Bo sung module auth:
	- Hash password (BCrypt/Argon2), khong luu plain text.
	- Chinh sach reset/password policy.
- Them migration cho `NguyenLieu` va script seed/upgrade cho moi truong moi.
- Bo sung test:
	- Unit test cho BUS validation/rules.
	- Integration test cho DAL voi DB test.

## 7. 📊 DANH GIA TONG QUAN

- Diem tong quan: **6.8/10**
- Muc do san sang deploy: **Beta noi bo**
	- Chua dat production vi thieu workflow POS cot loi + thieu auth/security + migration chua tron ven.
- Nhan xet reviewer:
	- Nen tang ky thuat cua module quan tri la tot va de tiep tuc mo rong.
	- De dat production, can uu tien 3 cum viec: (1) hoan thien BanHang/HoaDon, (2) khoa bao mat user/password + auth flow, (3) hoan tat migration va test.
