# REVIEW GD2 - DASHBOARD + QUAN LI BAN + QUAN LI MON

## Pham vi danh gia
- Dashboard: thong ke tong quan, doanh thu, hoa don gan day.
- Quan li ban: so do ban, danh sach ban, them/xoa/chuyen/gop ban.
- Quan li mon: danh sach mon, CRUD mon, loc/tim kiem, xuat CSV.
- Data layer: EF Core + SQL Server + migration khoi tao.

## 1. DA DAT DUOC

### Chuc nang da hoan thanh
- Da co dashboard tong quan voi cac KPI co ban: doanh thu, so hoa don, ty le ban dang dung, tong mon ban trong ngay.
- Da co bieu do doanh thu 7 ngay va top mon ban chay.
- Da co danh sach hoa don gan day va timeline hoat dong gan day.
- Da co quan li ban: them ban, xoa ban (co check rang buoc), chuyen ban, gop ban.
- Da co so do ban dong bang FlowLayoutPanel (khong con hardcode vi tri).
- Da co quan li mon: them/sua/xoa mon, loc theo loai, tim kiem, xuat CSV.
- Da co migration khoi tao schema chinh cho POS co ban (Ban, Mon, LoaiMon, HoaDon, HoaDon_ChiTiet, NhanVien, KhachHang).
- Build hien tai pass tren net10.0-windows.

### Danh gia muc do hoan thien
- UI: 7/10
	- Giao dien dong bo mau sac, bo cuc ro, card thong ke va bang du lieu de su dung.
- Logic: 6.5/10
	- Nguoi dung co the thao tac duoc luong chinh, nhung con mot so logic nghiep vu chua chat cho production.
- Database: 6/10
	- Schema co ban day du, co FK, co migration.
	- Chua co rang buoc nang cao, chua co chuan bao mat tai khoan.
- Performance: 6/10
	- Du lieu nho van chay on, nhung query sync tren UI thread se de giat/treo khi data tang.

### Diem tot
- Co su dung AsNoTracking cho nhieu query doc, giam tracking overhead.
- Co check nghiep vu truoc xoa (khong xoa ban dang dung, khong xoa mon da phat sinh hoa don).
- Co tach ham nho ro rang trong form (LoadThongKe, LoadDanhSach..., RefreshView...).
- Co dong context o OnFormClosed (frmQuanLiBan, frmQuanLiMon).
- Co EscapeCsv khi xuat file, tranh vo format CSV co dau phay/nhay kep.

## 2. CON THIEU
- Chua co dang nhap/dang xuat thuc te va phan quyen thao tac.
- Chua co navigation day du giua cac man hinh (nhieu nut sidebar chua gan luong).
- Chua tach kien truc BUS/DAL/Service; Form dang truy cap DbContext truc tiep.
- Chua co transaction bao ve cho nghiep vu nhieu buoc (chuyen/gop ban).
- Chua co logging loi, audit log, thong diep loi co ma de trace.
- Chua co unit test/integration test.
- Chua co hardening du lieu production: hash mat khau, unique index, validation chat, concurrency control.

## 3. LOI DA FIX

### 3.1 So do ban hardcode vi tri
- Nguyen nhan truoc day: vi tri ban dat co dinh trong UI, mo rong so ban ton cong sua giao dien.
- Trang thai hien tai: da xu ly bang tao dong control ban theo du lieu DB va do vao FlowLayoutPanel.
- Muc do on dinh hien tai: on cho giai doan demo/noi bo.

### 3.2 Chua co nghiep vu chuyen/gop ban
- Nguyen nhan truoc day: chua co luong xu ly hoa don nguon-dich va cap nhat trang thai ban.
- Trang thai hien tai: da co popup chon ban dich va xu ly 2 nhanh Chuyen ban/Gop ban.
- Muc do on dinh hien tai: da chay duoc, nhung can harden them transaction + nghiep vu thanh toan.

### 3.3 Dashboard de vo UI khi loi du lieu
- Nguyen nhan truoc day: luong load KPI/phien hieu co the phat sinh exception khi DB co van de.
- Trang thai hien tai: da co fallback ve gia tri mac dinh de tranh vo man hinh.
- Muc do on dinh hien tai: dat yeu cau khong crash UI, nhung can them logging de debug.

## 4. CACH FIX THANH CONG

### 4.1 So do ban dong
- Cach lam: doc danh sach ban tu DB, tao Button runtime cho tung ban, map mau theo TrangThai, add vao FlowLayoutPanel.
- Giai phap dung vi: bo vi tri hardcode, scale duoc khi tang so ban, de bao tri.

### 4.2 Chuyen/Gop ban
- Cach lam: lay hoa don mo cua ban nguon, cho chon ban dich, sau do:
	- Chuyen ban: doi BanID cua hoa don nguon sang ban dich + cap nhat TrangThai 2 ban.
	- Gop ban: merge tung dong HoaDon_ChiTiet theo MonID/DonGia/GhiChu vao hoa don dich, dong hoa don nguon.
- Giai phap hieu qua o muc giai doan hien tai vi da giai quyet duoc nghiep vu can ban va dong bo du lieu ban.

### 4.3 Fallback dashboard
- Cach lam: bao toan bo load KPI trong try/catch, neu loi thi reset ve gia tri an toan (0, danh sach rong).
- Giai phap dung vi: uu tien trai nghiem nguoi dung, tranh crash man hinh tong quan.

## 5. LOI TON DONG

| Loi | Muc do | Anh huong |
|---|---|---|
| Bind su kien click bi trung (Designer + code-behind) o QuanLiMon va QuanLiBan | High | 1 lan click co the goi handler 2 lan; voi nut ghi DB (them/sua/xoa) co nguy co ghi lap/hanh vi khong mong muon |
| Nghiep vu gop/chuyen ban chua dung transaction DB | High | Neu loi giua chung co the de du lieu nua duong (inconsistent state) |
| Mat khau nhan vien dang o dang plain text (MatKhau string) | High | Rui ro bao mat nghiem trong, khong dat production |
| Trang thai hoa don nguon khi gop dang set = 1 (da thanh toan) du khong thu tien | Medium | Lech nghia nghiep vu va so lieu thong ke hoa don |
| Dashboard khong dispose DbContext field sau khi dong form | Medium | Co the gay ro ri tai nguyen neu mo/dong nhieu lan |
| Nhieu thao tac query sync tren UI thread | Medium | De giat/treo UI khi du lieu lon hoac SQL cham |
| Build con 9 warning nullable | Low-Medium | Chua clean code, co nguy co NullReference ve sau |

## 6. DE XUAT CAI TIEN

### Refactor code
- Uu tien tach kien truc 3 lop (UI -> BUS -> DAL), khong de Form truy cap EF truc tiep.
- Tao service chung cho thao tac bang/mon/hoa don de giam duplicate logic.
- Loai bo bind event trung: chi giu 1 noi bind (uu tien Designer hoac constructor, khong ca hai).

### Toi uu performance
- Chuyen cac truy van lon sang async (ToListAsync, CountAsync) + await tren UI hop ly.
- Giam so lan round-trip SQL trong dashboard bang gom query/thong ke theo batch.
- Them index cho cac cot tim kiem/loc thuong xuyen.

### Cai thien UI/UX
- Hoan thien luong dieu huong giua cac form (Dashboard <-> Ban <-> Mon).
- Gan hanh vi that cho Dang xuat, User menu, va cac nut sidebar con lai.
- Bo sung thong bao loi than thien kem ma loi de de support.

### Nang cap kien truc
- Them transaction cho chuyen/gop ban.
- Chuyen mat khau sang bcrypt/argon2 + bo sung role/permission.
- Them optimistic concurrency (rowversion) cho hoa don chi tiet khi cap nhat song song.
- Bo sung bo test toi thieu cho nghiep vu quan trong (gop/chuyen ban, xoa mon, xoa ban).

## 7. DANH GIA TONG QUAN
- Diem tong quan: 6.8/10.
- Muc do san sang deploy: Beta noi bo (khong production).
- Nhan xet reviewer:
	- Ban GD2 da co tien trien tot, da vuot muc prototype UI va da co xu ly nghiep vu co ban cho Ban va Mon.
	- Diem nghen lon nhat hien nay la do tin cay thao tac ghi du lieu (bind event trung, chua transaction) va bao mat tai khoan.
	- Neu xu ly cac muc High truoc, ban nay co the len duoc ban UAT on dinh.
