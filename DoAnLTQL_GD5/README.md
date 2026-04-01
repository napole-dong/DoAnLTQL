# DoAnLTQL_GD5

## 1. Tong quan

DoAnLTQL_GD5 la giai doan hoan thien ung dung WinForms quan ly quan ca phe theo huong tach lop ro rang:

- `Forms`: giao dien va xu ly su kien UI.
- `BUS`: nghiep vu.
- `DAL`: truy xuat du lieu.
- `DTO`: doi tuong truyen du lieu.
- `Data`: entity + `DbContext` cho Entity Framework Core.
- `Services`: cac service chuyen biet (hien tai co service CSV/validate cho Mon).

## 2. Cong nghe su dung

- .NET 10 (`net10.0-windows`)
- WinForms
- Entity Framework Core 10.0.5
- SQL Server / SQL Server Express

## 3. Cau truc du an chinh

```text
DoAnLTQL_GD5/
  README.md
  QuanLyQuanCaPhe/
	 Program.cs
	 App.config
	 QuanLyQuanCaPhe.csproj
	 BUS/
	 DAL/
	 DTO/
	 Data/
	 Forms/
	 Migrations/
	 Services/
```

## 4. Tinh nang da trien khai

### 4.1 Quan ly mon va loai mon

- CRUD mon.
- CRUD loai mon.
- Tim kiem theo tu khoa.
- Nhap/Xuat CSV cho mon va loai mon.
- Validate du lieu dau vao (ten mon, loai, don gia, trang thai).
- Rang buoc nghiep vu:
  - Khong xoa mon da phat sinh hoa don.
  - Khong xoa loai mon dang su dung.
  - Ho tro chuyen mon sang loai khac roi xoa loai cu.

### 4.2 Quan ly ban

- Hien thi thong ke ban (tong ban, dang phuc vu, ban trong, dat truoc).
- Hien thi so do ban dong.
- Them ban, xoa ban (co rang buoc).
- Loc/tim kiem theo khu vuc, trang thai, tu khoa.
- Ho tro chuyen ban / gop ban dua tren hoa don dang mo.

### 4.3 Quan ly khach hang

- CRUD khach hang.
- Tim kiem nhanh.
- Nhap/Xuat CSV.
- Validate ho ten, so dien thoai.
- Kiem tra trung so dien thoai.
- Khong xoa khach da phat sinh hoa don.

### 4.4 Quan ly nhan vien va tai khoan

- CRUD nhan vien.
- Tim kiem nhanh.
- Nhap/Xuat CSV.
- Quan ly thong tin dang nhap + quyen han qua cap `User`/`VaiTro`.
- Kiem tra trung ten dang nhap.
- Khong xoa nhan vien da phat sinh hoa don.

### 4.5 Quan ly kho nguyen lieu

- CRUD nguyen lieu.
- Tim kiem theo ma/ten/don vi/trang thai.
- Tu dong tinh trang thai theo ton kho va muc canh bao (`Het hang`, `Sap het`, `Dang su dung`, `Ngung dung`).
- Xuat CSV danh sach kho.

## 5. Phan chua hoan thien

- `frmBanHang` hien moi la khung form, chua co code-behind xu ly nghiep vu ban hang.
- `frmHoaDon` chua co nghiep vu xu ly hoa don.
- Trong `frmNhanVien`, nut `Thong ke` va `Hoa don` hien tai moi hien thong bao "dang duoc phat trien".
- Chuc nang `Nhap kho` trong `frmQuanLiKho` hien tai chi tai lai du lieu, chua co luong nhap CSV/nhap phieu kho.



1. Hoan thien nghiep vu ban hang tai `frmBanHang` (chon ban, goi mon, tam tinh, thanh toan).
2. Hoan thien man hinh `frmHoaDon` va bo loc lich su hoa don.
3. Them migration chinh thuc cho bang `NguyenLieu` de dam bao tao DB moi on dinh.
4. Bo sung dang nhap/phan quyen theo `User`/`VaiTro` o luong vao ung dung.
5. Bo sung test cho BUS/DAL cua cac nghiep vu quan trong.
