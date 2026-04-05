# DoAnLTQL - Giai doan 6 (GD6)

## 1. Tong quan

GD6 tap trung hoan thien cac nghiep vu cot loi cho quan ca phe theo huong tach lop ro rang va de bao tri:

- Kien truc `Forms - BUS - DAL - DTO - Data - Services` duoc ap dung dong bo.
- Hoan thien luong ban hang tai quan va quan ly hoa don.
- Mo rong dang nhap/tai khoan, bo sung xu ly mat khau an toan.
- Tiep tuc bo sung cac service nghiep vu de giam logic trong UI.

## 2. Cong nghe su dung

- .NET 10 (`net10.0-windows`)
- WinForms
- Entity Framework Core 10.0.5
- SQL Server / SQL Server Express
- `BCrypt.Net-Next` (hash va kiem tra mat khau)

## 3. Cau truc thu muc chinh

```text
DoAnLTQL_GD6/
	README.md
	QuanLyQuanCaPhe/
		Program.cs
		App.config
		QuanLyQuanCaPhe.csproj
		Forms/
		BUS/
		DAL/
		DTO/
		Data/
		Services/
			Auth/
			HoaDon/
			Mon/
		Migrations/
```

## 4. Chuc nang da trien khai

### 4.1 Dang nhap, tai khoan, phan quyen

- Co man hinh dang nhap (`frmDangNhap`) va luong vao he thong.
- Xac thuc qua `DangNhapBUS`/`DangNhapDAL`, co kiem tra tai khoan hoat dong.
- Mat khau duoc xu ly qua `Services/Auth/MatKhauService` su dung bcrypt.
- Ho tro nang cap mem mat khau cu sang hash moi khi dang nhap.

### 4.2 Ban hang tai quan

- Hien thi so do ban va trang thai ban.
- Chon ban, them mon vao gio tam, tong hop phieu theo ban.
- Thanh toan hoa don theo ban.
- Ho tro chuyen ban va gop ban, co kiem tra dieu kien ban nguon/ban dich.

### 4.3 Quan ly hoa don

- Danh sach hoa don co bo loc theo tu khoa, khoang ngay, trang thai.
- Them moi, cap nhat, huy hoa don voi cac rang buoc nghiep vu.
- Them mon vao hoa don cho thanh toan.
- Xac nhan thu tien, tinh tien thoi.
- Xem truoc noi dung in hoa don.

### 4.4 Quan ly ban

- Thong ke tong ban, ban trong, ban dang phuc vu, ban dat truoc.
- Them/xoa ban co rang buoc (khong xoa ban dang su dung hoac da phat sinh hoa don).
- Loc danh sach ban theo khu vuc, trang thai, tu khoa.

### 4.5 Quan ly mon va loai mon

- CRUD mon va loai mon.
- Bo sung trang thai mon (`Dang kinh doanh`/`Ngung ban`) va mo ta loai mon.
- Nhap/xuat CSV cho mon va loai mon.
- Ho tro chuyen mon sang loai khac truoc khi xoa loai mon dang duoc su dung.

### 4.6 Quan ly khach hang, nhan vien, kho

- Khach hang: CRUD + import/export CSV + validate du lieu.
- Nhan vien: CRUD + import/export CSV + tai khoan + quyen han.
- Kho nguyen lieu: CRUD, theo doi ton kho/trang thai, xuat CSV.


## 5. Muc ton dong

- Chuc nang thong ke tong hop tren mot so man hinh dang trong qua trinh phat trien.
- Phan quyen hien da co du lieu vai tro, nhung quy tac chan/chia quyen chi tiet theo tung man hinh can tiep tuc hoan thien neu yeu cau do an mo rong.

van chua fix:
- k clear danh sach khi da them 1 don vi tren cac form
- chua xoa du lieu mau
- chuyen trang giua cac form, dang nhap, phan quyen
