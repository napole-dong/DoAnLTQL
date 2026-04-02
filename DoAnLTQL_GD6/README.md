# DoAnLTQL_GD6

## Danh sach cong viec can hoan thanh

### 1) Uu tien cao (anh huong truc tiep den nghiep vu)

- [ ] Hoan thien man hinh Hoa don.
	- Hien tai `frmHoaDon` moi co khung giao dien, chua co code-behind nghiep vu.
	- Can bo sung: loc hoa don theo thoi gian/trang thai, xem chi tiet, tong tien, thao tac in/xuat.

- [ ] Hoan thien man hinh Thong ke.
	- Hien tai chua co form/thanh phan thong ke hoan chinh.
	- Can bo sung: doanh thu theo ngay-tuan-thang, top mon ban, so hoa don, so ban phuc vu.

- [ ] Hoan thien dieu huong sang Hoa don/Thong ke trong he thong menu.
	- Hien tai o `frmNhanVien`, 2 nut `Thong ke` va `Hoa don` dang la thong bao "dang duoc phat trien".
	- Can noi dieu huong that den man hinh nghiep vu tuong ung va dong bo tren cac form chinh.

- [ ] Hoan thien nghiep vu Nhap kho.
	- Hien tai nut `Nhap kho` o `frmQuanLiKho` chi tai lai du lieu.
	- Can bo sung luong nhap kho that (nhap so luong, gia nhap, cap nhat ton, luu lich su/phieu nhap).

### 2) Uu tien trung binh (on dinh he thong va CSDL)

- [ ] Chot migration cho bang NguyenLieu.
	- Model/DbSet da co `NguyenLieu`, nhung migration khoi tao chua tao bang nay.
	- Can tao migration bo sung va kiem tra lai kich ban tao DB moi.

- [ ] Bo sung luong Dang nhap/Phan quyen thuc te.
	- Hien tai `Program.cs` dang mo truc tiep `frmBanHang`.
	- Can bo sung man hinh dang nhap, xac thuc theo `User`/`VaiTro`, va gioi han quyen truy cap theo vai tro.

### 3) Uu tien hoan thien truoc khi nop

- [ ] Bo sung test cho BUS/DAL cac nghiep vu quan trong (BanHang, HoaDon, Ban, Mon, NhanVien).
- [ ] Xu ly canh bao nullable (CS8618) tren cac entity de tang do on dinh build.
- [ ] Cap nhat tai lieu huong dan cai dat/chay du an va checklist demo nghiep vu.

## Ghi chu trang thai hien tai

- `dotnet build` tai thu muc `DoAnLTQL_GD6/QuanLyQuanCaPhe` da build thanh cong, khong co loi, con 5 canh bao nullable.
