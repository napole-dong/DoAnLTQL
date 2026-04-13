# REVIEW GD3 - QUAN LY QUAN CA PHE (WINFORMS + POS)

Phan vi danh gia:
- Dashboard
- Quan ly ban
- Quan ly mon
- Loai mon
- Data model + EF Core migration

Nguon tham chieu code chinh:
- QuanLyQuanCaPhe/Forms/frmDashboard.cs
- QuanLyQuanCaPhe/Forms/frmQuanLiBan.cs
- QuanLyQuanCaPhe/Forms/frmQuanLiMon.cs
- QuanLyQuanCaPhe/Forms/frmLoaiMon.cs
- QuanLyQuanCaPhe/Data/*.cs
- QuanLyQuanCaPhe/Migrations/20260325033700_KhoiTaoCSDL.cs

Tinh trang build:
- Build pass: net10.0-windows
- Co 8 warning (chu yeu nullable warning + 1 canh bao dereference co the null)

## 1) DA DAT DUOC

Chuc nang da hoan thanh:
- Dashboard da co du lieu tong quan: doanh thu, so hoa don, ti le ban dang su dung, so mon ban.
- Dashboard da co bieu do doanh thu 7 ngay, top mon ban chay, danh sach hoa don gan day, activity gan day.
- Co co che chuyen trang trong dashboard bang panel host (embedded form), khong mo tung form roi rac.
- Quan ly mon da co CRUD co ban: them, cap nhat, xoa (co chan xoa neu da phat sinh hoa don).
- Quan ly mon da co import/export CSV.
- Quan ly mon da co preview hinh tu file local hoac URL.
- Quan ly ban da co: them ban, xoa ban (co dieu kien), loc theo khu vuc/trang thai, tim kiem, so do ban dong.
- Chuyen/Gop ban da co logic nghiep vu co ban va cap nhat lai trang thai ban/hoa don.
- Da co migration khoi tao DB va quan he FK giua cac bang chinh.

Danh gia muc do hoan thien:
- UI: 7.5/10 (theme dong bo, bo cuc ro; mot so man hinh con placeholder)
- Logic: 6.5/10 (Quan ly mon/ban kha day du, Loai mon chua co logic)
- Database: 6.0/10 (co schema + FK, chua co constraint/unique index/bao mat du lieu)
- Performance: 6.5/10 (co AsNoTracking o nhieu query, chua co toi uu cho production lon)

Diem tot:
- Luong UI dashboard host page ro rang, de mo rong module.
- Su dung AsNoTracking cho truy van chi doc.
- Co cac guard nghiep vu quan trong: chan xoa mon da phat sinh hoa don, chan xoa ban dang su dung.
- Logic gop ban co xu ly merge dong chi tiet trung nhau (MonID + DonGia + GhiChu).

## 2) CON THIEU

Chuc nang chua co/chua day du:
- Loai mon chua co logic: cac button handler dang de trong.
- Cac menu tren sidebar dashboard nhu Hoa don/Nhan vien/Thong ke moi la UI, chua thay handler mo man hinh.
- Nut Dang xuat chua co click handler thuc hien dang xuat.

Best practice chua ap dung day du:
- Chua co logging cho exception; co catch rong (nuot loi) o Dashboard va Quan ly ban.
- Chua co transaction/concurrency policy ro rang cho thao tac nghiep vu phuc tap (chuyen/gop ban trong moi truong da user).
- Chua co test tu dong (unit/integration).
- Chua co phan tach ro BUS/DAL/Service o GD3 (logic dang nam tren Form code-behind).

Can cai thien de production-ready:
- Bo sung auth + phan quyen toi thieu.
- Chuyen mat khau sang hash (bcrypt/pbkdf2), khong luu plain text.
- Chuan hoa DB constraint, unique index, check constraint cho status/price.
- Bo sung audit log va thong diep loi co ma loi.

## 3) LOI DA FIX

Theo dau vet code hien tai va README cu:
- Loi chuyen trang form roi rac da duoc xu ly bang panel host trong dashboard.
- Logic Quan ly mon (CRUD + import/export + preview anh) da co va hoat dong o muc chuc nang co ban.
- Chuyen/Gop ban da duoc trien khai (README cu ghi la chua fix, nhung code hien tai da co ham xu ly).

Nguyen nhan ngan gon:
- Truoc day man hinh mo roi rac, kho dong bo UI state va dieu huong.
- Chuc nang mon/ban truoc do thieu handler xu ly nghiep vu.

Tinh trang hien tai:
- O muc local/dev la on dinh co ban (build pass).
- Chua the xem la on dinh production vi con warning, module Loai mon chua hoan tat va chua co test.

## 4) CACH FIX THANH CONG

Huong xu ly da dung:
- Navigation: dung mot dashboard host (_pageHost) de nhung form con (TopLevel=false, Dock=Fill), dong form cu truoc khi mo form moi.
- Quan ly mon: xac thuc input truoc khi ghi DB, chan xoa mon da duoc su dung, cap nhat DataGrid theo datasource typed.
- Chuyen/Gop ban: lay hoa don nguon/dich, merge chi tiet trung, xoa chi tiet nguon, cap nhat trang thai hoa don va trang thai ban.

Vi sao hieu qua:
- Giam phan manh dieu huong va tao 1 diem vao nhat quan cho UI.
- Co dieu kien nghiep vu truoc khi ghi DB nen han che loi du lieu co ban.
- Luong gop ban giai quyet duoc bai toan trung dong mon thay vi tao duplicate vo han.

## 5) LOI TON DONG

1) HIGH - Loai mon chua co logic xu ly
- Anh huong: man hinh co UI nhung khong thao tac duoc; nghiep vu danh muc mon chua hoan thanh.

2) HIGH - Mat khau dang duoc luu plain text trong model
- Anh huong: rui ro bao mat nghiem trong, khong dat chuan production.

3) MEDIUM - Catch rong nuot loi
- Anh huong: khi loi DB/runtime, nguoi dung chi thay so 0 hoac trang thai fallback, kho truy vet va kho debug.

4) MEDIUM - Domain trang thai ban khong nhat quan
- Anh huong: model ghi chu 0/1, nhung UI/logic dung them status=2 (Dat truoc); de gay sai lech du lieu/nghiep vu.

5) MEDIUM - Warning nullability + canh bao dereference co the null
- Anh huong: nguy co phat sinh NullReference trong runtime o mot so edge-case.

6) MEDIUM - Mot so menu sidebar chua co action that su
- Anh huong: UX gay nham lan vi menu co hien thi nhung khong mo duoc module.

7) LOW - Connection string dang hard-code local SQLEXPRESS
- Anh huong: kho deploy moi truong khac, kho quan ly secret/config.

8) LOW - CSV parser tu viet chua robust cho mo hinh quote phuc tap
- Anh huong: import co the sai du lieu voi file CSV phuc tap (escaped quote nhieu truong hop).

## 6) DE XUAT CAI TIEN

Refactor code:
- Tach logic nghiep vu tu Form sang service/BUS.
- Dung Presenter hoac MVP pattern cho WinForms de de test/de bao tri.

Toi uu performance:
- Chuyen cac thao tac DB dai sang async de tranh block UI.
- Toi uu truy van tim kiem va bo sung index cho cot tim kiem thuong dung.

Cai thien UI/UX:
- Hoan tat Loai mon theo cung chuan voi Quan ly mon.
- Disable tam thoi cac nut thao tac trong khi dang xu ly thao tac DB.
- Bo sung thong bao loi than thien + ma loi de support.

Nang cap kien truc:
- Bo sung Auth + Role-based permission.
- Ma hoa mat khau + chinh sach password.
- Them logging tap trung, audit log, va test tu dong cho flow quan trong (chuyen/gop ban, CRUD mon, import CSV).

## 7) DANH GIA TONG QUAN

Cham diem:
- 6.8/10

Muc do san sang deploy:
- Dev: San sang
- Beta noi bo: Gan dat (can hoan tat Loai mon + fix warning/chuan hoa loi)
- Production: Chua san sang

Nhan xet reviewer:
- Day la ban GD3 co nen tang UI kha tot va da co duong day nghiep vu cho Dashboard/Quan ly mon/Quan ly ban.
- Tuy nhien, de di production can xu ly nghiem tuc nhom van de: module Loai mon dang placeholder, bao mat mat khau, xu ly exception/logging, va bo sung test.
