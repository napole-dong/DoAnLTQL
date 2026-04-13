# DoAnLTQL - Giai doan 6 (GD6)

## Bao cao Review Senior Developer (.NET WinForms + POS)

- Ngay review: 14/04/2026
- Pham vi: DoAnLTQL_GD6/QuanLyQuanCaPhe
- Cach xac thuc: doc code theo luong Forms-BUS-DAL-Data-Services + build thuc te (`dotnet build`)
- Ket qua build: Thanh cong, 0 error, 5 warning nullability (CS8618)

## 1. ✅ DA DAT DUOC

### 1.1 Chuc nang da hoan thanh

- Dang nhap va quan ly tai khoan:
	- Co man hinh dang nhap, validate input, thong bao loi theo truong.
	- Xac thuc theo user active + role.
	- Mat khau da dung bcrypt va co co che nang cap mem du lieu plain text cu.
- Ban hang tai quan:
	- Co so do ban, chon ban, them mon vao gio tam theo ban.
	- Co goi mon, luu mon cho goi, thanh toan, chuyen ban, gop ban.
- Hoa don:
	- Loc theo ngay/trang thai/tu khoa.
	- Tao moi, sua hoa don mo, them mon, huy hoa don, thu tien.
	- Co preview noi dung in hoa don.
- Quan ly danh muc va du lieu:
	- Ban, Mon, LoaiMon, KhachHang, NhanVien, NguyenLieu deu co CRUD.
	- Co import/export CSV cho KhachHang, NhanVien, Mon, LoaiMon, Kho.

### 1.2 Muc do hoan thien

- UI: 7.0/10
	- Form duoc chia bo cuc ro rang, co card/list, thao tac duoc.
	- Nhieu man hinh chua dong nhat dieu huong/phan quyen o muc shell app.
- Logic: 7.5/10
	- Luong core POS (ban, hoa don, thanh toan, chuyen/gop ban) da co.
	- Validation nghiep vu da kha day du trong BUS/DAL.
- Database: 7.0/10
	- Migration duoc bo sung theo tien trinh, role da chuan hoa FK.
	- Mot so migration/model chua dong bo day du voi chuc nang kho nang cao.
- Performance: 6.5/10
	- Chay on voi quy mo nho.
	- Polling 500ms tren nhieu form + truy van dong bo tren UI thread se som cham khi du lieu lon.

### 1.3 Diem tot

- Tachd lop ro: Forms/BUS/DAL/DTO/Services de doc va bao tri tot.
- Validation nghiep vu kha chac tay: check trung, check rang buoc xoa, check trang thai hoa don/mon.
- Bao mat dang nhap tot hon ro ret so voi giai doan truoc (bcrypt + migration role).
- UX co cai tien: thong bao ro, luong thao tac khong qua roi.

## 2. ⚠️ CON THIEU

- Chua ap dung phan quyen thuc thi xuyen suot theo user dang nhap o cap Program/Main shell.
- Sidebar menu hien dien tren nhieu form nhung chua duoc wiring dieu huong day du.
- Chuc nang thong ke va mot so menu van dang dang placeholder/hoac chua mo luong that o toan he thong.
- Chua co unit test/integration test regression cho cac luong quan trong (dang nhap, thanh toan, huy hoa don, import CSV).
- Chua ap dung async/await cho DB call tren form, de block UI khi I/O cham.
- Chua co transaction ro rang cho cac thao tac nhieu buoc ghi DB.

## 3. 🐞 LOI DA FIX

### 3.1 Cac loi da gap truoc day

- Bao mat mat khau plain text/default tu giai doan truoc.
- Luong ban hang trong form chinh tung o muc chua hoan chinh.
- Role/tai khoan truoc day chua chuan hoa theo bang role rieng.

### 3.2 Nguyen nhan ngan gon

- Du lieu tai khoan legacy luu theo cach cu, chua co strategy hash/chuyen doi.
- Kien truc nghiep vu da tach lop nhung luong POS core chua du logic.
- Thiet ke role ban dau dua tren string field, kho mo rong va kho enforce.

### 3.3 Tinh trang hien tai

- Da on hon nhieu o mat nghiep vu cot loi va dang nhap.
- Co the demo end-to-end noi bo.
- Chua dat muc on dinh production vi van con loi he thong ton dong (muc 5).

## 4. 🔧 CACH FIX THANH CONG

- Bao mat tai khoan:
	- Dung `MatKhauService` de hash/verify bcrypt.
	- Co fallback verify du lieu plain text cu + auto rehash khi login thanh cong.
	- Giai phap nay dung vi giam rui ro lo mat khau va khong vo du lieu legacy.
- Chuan hoa role:
	- Tach bang `VaiTro`, map `User.VaiTroID` qua FK.
	- Migration co buoc map du lieu role cu sang role moi.
	- Giai phap nay dung vi de enforce, de query va de mo rong permission matrix.
- On dinh nghiep vu hoa don-ban:
	- Dong bo trang thai ban theo hoa don mo khi thu tien/huy hoa don.
	- Chuyen/gop ban co rang buoc ro rang cho ban nguon-ban dich.
	- Giai phap nay dung vi tranh trang thai ban sai va tranh luong thao tac trai nghiep vu.

## 5. ❗ LOI TON DONG

### 5.1 Danh sach bug con lai

1. HIGH - Chua enforce session dang nhap + role o startup flow
	 - Trieu chung: Program dang nhap xong mo thang `frmBanHang`, khong truyen context user/role de gate toan he thong.
	 - Anh huong: user nao login duoc deu co nguy co thao tac vuot quyen tren cac module khac.

2. HIGH - Sidebar dieu huong chua dong bo tren nhieu form
	 - Trieu chung: co button menu trong Designer nhung khong duoc bind Click o code-behind (chi `frmNhanVien` co bind mot phan).
	 - Anh huong: UX dut quang, khong di chuyen module on dinh, de phat sinh flow sai.

3. MEDIUM - Polling 500ms tren nhieu form quan tri
	 - Trieu chung: `Timer Interval=500` + truy van lai lien tuc.
	 - Anh huong: tang tai DB/CPU, lag UI khi du lieu lon hoac may client yeu.

4. MEDIUM - Luong kho nang cao chua hoan tat
	 - Trieu chung: co migration `PhieuNhapKho` nhung context/chuc nang UI nghiep vu nhap kho chi tiet chua theo kip; nut `Nhap kho` hien tai chi reload du lieu.
	 - Anh huong: kho khong co nhat ky nhap xuat day du de truy vet.

5. MEDIUM - Chua transaction hoa cac thao tac nhieu buoc
	 - Trieu chung: nhieu ham DAL goi `SaveChanges` nhieu lan trong 1 nghiep vu ma khong co transaction bao quanh.
	 - Anh huong: nguy co du lieu dang do neu loi giua chung.

6. LOW - Warning nullability va dau vet code tam
	 - Trieu chung: build canh bao CS8618 tren mot so entity; con comment thua (`//hello word`).
	 - Anh huong: giam chat luong code, giam do tin cay static analysis.

## 6. 🚀 DE XUAT CAI TIEN

### 6.1 Refactor code

- Tao `CurrentUserContext` va truyen xuyen suot tu login -> shell -> forms/BUS.
- Chuan hoa dieu huong bang mot `NavigationService` dung chung cho toan bo form chinh.
- Tach CSV parser chung, tranh lap lai `SplitCsvLine` o nhieu BUS.
- Bo sung `Result` object co ma loi/thong diep thong nhat de log va trace.

### 6.2 Toi uu performance

- Giam polling (500ms -> 2-5s hoac refresh theo su kien).
- Chuyen cac thao tac DB nang sang async (khong block UI).
- Uu tien filter/query tren SQL thay vi `ToList` som roi loc o memory.

### 6.3 Cai thien UI/UX

- Dong nhat hanh vi menu tren tat ca form (navigate, active state, logout).
- Bo sung loading state/disabling nut trong thao tac lau.
- Nang cap in hoa don tu preview MessageBox len print workflow thuc te (print/thermal).

### 6.4 Nang cap kien truc

- Ap dung transaction cho cac use-case nhieu buoc ghi DB (ban hang, hoa don, nhap kho).
- Bo sung audit log toi thieu cho thao tac nhay cam (huy hoa don, sua gia, xoa du lieu).
- Bo sung test regression cho 5 luong trong yeu: DangNhap, GoiMon, ThanhToan, HuyHoaDon, Import CSV.

## 7. 📊 DANH GIA TONG QUAN

- Diem tong quan: **7.1/10**
- Muc do san sang deploy: **Beta noi bo**
- Nhan xet reviewer:
	- GD6 da dat duoc muc "co the van hanh demo nghiep vu cot loi" va co cai tien ro o bao mat dang nhap.
	- De len production that su, can uu tien chot 3 cum: enforce phan quyen xuyen suot, dong bo dieu huong/menu, va hardening transaction + performance.

## Phu luc chung cu ky thuat (tham khao nhanh)

- Dang nhap + startup:
	- `Program.cs` (login xong run thang `frmBanHang`)
	- `Forms/frmDangNhap.cs` (`ThongTinDangNhap` da co nhung chua duoc su dung xuyen suot)
- Bao mat mat khau:
	- `Services/Auth/MatKhauService.cs`
	- `DAL/DangNhapDAL.cs`
- Chuan hoa role:
	- `Migrations/20260327083029_ChuanHoaTaiKhoanVaVaiTro.cs`
- POS core:
	- `BUS/BanHangBUS.cs`, `DAL/BanHangDAL.cs`
	- `BUS/HoaDonBUS.cs`, `DAL/HoaDonDAL.cs`
	- `DAL/BanDAL.cs`
- Van de ton dong tieu bieu:
	- Sidebar khong bind dong bo: `Forms/frmBanHang.Designer.cs`, `Forms/frmBanHang.cs`
	- Polling 500ms: `Forms/frmQuanLiBan.cs`, `Forms/frmKhachHang.cs`, `Forms/frmNhanVien.cs`, `Forms/frmQuanLiKho.cs`
	- Kho nang cao chua chot: `Migrations/20260401165029_ThemBangPhieuNhapKho.cs`, `Data/CaPheDbContext.cs`, `Forms/frmQuanLiKho.cs`
	- Build warning nullability: `Data/dtaBan.cs`, `Data/dtaNhanVien.cs`, `Data/dtaUser.cs`, `Data/dtaVaiTro.cs`
