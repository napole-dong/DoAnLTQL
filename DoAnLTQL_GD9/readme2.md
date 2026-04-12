# Audit Toan Dien - GD9 (POS Quan Ly Quan Ca Phe)

Audit nay cho thay he thong dang co nhieu diem rui ro production-grade, trong do co loi co the gay sai lech tai chinh, khoa ban sai trang thai, mat dau audit va ha muc bao mat nghiem trong.

## MUST FIX (nguy hiem, sua truoc khi demo hoi dong)

### 1. [CRITICAL][5] Tai khoan mac dinh bi reset mat khau 123 moi lan khoi dong

**Evidence:**
- [Program.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Program.cs#L90)
- [TaiKhoanMacDinhDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/TaiKhoanMacDinhDAL.cs#L73)
- [TaiKhoanMacDinhDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/TaiKhoanMacDinhDAL.cs#L75)

**Impact:**
Bat ky ai biet `123` co the chiem quyen sau moi lan restart app. Day la lo hong bao mat cap P0.

**Fix C# truc tiep:**

```csharp
// Program.cs
var bootstrapPassword = Environment.GetEnvironmentVariable("CAPHE_BOOTSTRAP_PASSWORD");
if (string.IsNullOrWhiteSpace(bootstrapPassword))
{
	throw new InvalidOperationException("Missing CAPHE_BOOTSTRAP_PASSWORD.");
}
var ketQuaKhoiTaoTaiKhoan = taiKhoanMacDinhDAL.DamBaoTaiKhoanMacDinh(bootstrapPassword);

// TaiKhoanMacDinhDAL.cs (khong auto reset mat khau account da ton tai)
// Chi set password khi tao moi, hoac khi co co rotate tuong minh.
var allowRotate = false; // doc tu config bao mat
if (allowRotate && (!MatKhauService.KiemTraMatKhau(matKhauMacDinh, user.MatKhau) || MatKhauService.CanNangCapHash(user.MatKhau)))
{
	user.MatKhau = MatKhauService.BamMatKhau(matKhauMacDinh);
	daCapNhat = true;
}
```

### 2. [CRITICAL][1][4] Luong goi mon bi tach 2 transaction, co the tao hoa don rong roi that bai them mon

**Evidence:**
- [BanHangDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L63)
- [BanHangDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L65)
- [BanHangDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/BanHangDAL.cs#L97)
- [HoaDonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L182)
- [HoaDonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L183)

**Impact:**
Neu tao hoa don thanh cong nhung AddItems fail (het kho/concurrency), ban van chuyen trang thai co khach, hoa don mo rong ton tai. Day la trang thai nua thanh cong nua that bai.

**Fix C# truc tiep (atomic theo ban):**

```csharp
// Y tuong: gom "dam bao hoa don + them mon" vao cung transaction duy nhat.
// BanHangDAL.GoiMon chi goi 1 API atomic.
var result = _orderService.AddItemsByTableAtomic(
	banId: banId,
	dsMonThem: dsMonThem,
	khachHangId: null);

// Trong AddItemsByTableAtomic:
// 1) lock ban + tim hoa don draft
// 2) neu chua co thi tao moi
// 3) them mon + tru kho
// 4) SaveChanges + Commit mot lan
```

### 3. [CRITICAL][1][2] Xoa loai mon co the partial-commit do ExecuteUpdate nam ngoai transaction bao trum

**Evidence:**
- [LoaiMonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/LoaiMonDAL.cs#L104)
- [LoaiMonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/LoaiMonDAL.cs#L109)
- [LoaiMonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/LoaiMonDAL.cs#L112)
- [LoaiMonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/LoaiMonDAL.cs#L115)

**Impact:**
Mon da bi chuyen trang thai ngung ban nhung xoa loai fail, du lieu lech nghiep vu va kho rollback.

**Fix C# truc tiep:**

```csharp
public bool XoaLoai(int id)
{
	return ExecutionStrategyTransactionRunner.Execute(
		context =>
		{
			var loai = context.LoaiMon.IgnoreQueryFilters().FirstOrDefault(x => x.ID == id);
			if (loai == null || loai.IsDeleted) return false;

			context.Mon
				.IgnoreQueryFilters()
				.Where(x => x.LoaiMonID == id && !x.IsDeleted)
				.ExecuteUpdate(setters => setters
					.SetProperty(x => x.TrangThai, 0)
					.SetProperty(x => x.DonGia, 0m)
					.SetProperty(x => x.TrangThaiTextLegacy, "Ngung ban"));

			context.LoaiMon.Remove(loai);
			context.SaveChanges();
			return true;
		},
		shouldCommit: ok => ok,
		isolationLevel: System.Data.IsolationLevel.Serializable);
}
```

### 4. [CRITICAL][5] Audit log khong dang tin cay: ghi sau commit va nuot loi

**Evidence:**
- [OrderService.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/BUS/OrderService.cs#L678)
- [ActivityLogService.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Services/Audit/ActivityLogService.cs#L33)
- [ActivityLogService.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Services/Audit/ActivityLogService.cs#L85)

**Impact:**
Nghiep vu tai chinh thanh cong nhung co the mat audit hoan toan, khong truy vet duoc.

**Fix C# truc tiep:**

```csharp
// Trong transaction chinh, them AuditLog cung context truoc SaveChanges:
foreach (var log in pendingAuditLogsInTransaction)
{
	context.AuditLog.Add(new dtaAuditLog
	{
		Action = log.Action,
		EntityName = log.EntityName,
		EntityId = log.EntityId,
		OldValue = Serialize(log.OldValue),
		NewValue = Serialize(log.NewValue),
		PerformedBy = log.PerformedBy ?? "system",
		CreatedAt = DateTime.Now
	});
}
await context.SaveChangesAsync();

// ActivityLogService.Log khong duoc catch rong:
catch (Exception ex)
{
	AppLogger.Error(ex, "Audit logging failed.", nameof(ActivityLogService));
	throw;
}
```

### 5. [HIGH][1][4] Lo hong concurrency khi cap nhat khach hang cho hoa don: khong dung RowVersion

**Evidence:**
- [HoaDonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L373)
- [HoaDonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L396)
- [HoaDonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L453)
- Doi chieu cho co gan RowVersion o [HoaDonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L258)

**Impact:**
Co cua so race khien invoice da doi trang thai van bi update thong tin khach.

**Fix C# truc tiep:**

```csharp
public BanActionResultDTO CapNhatKhachHangChoHoaDonMo(int hoaDonId, int? khachHangId, byte[]? rowVersion)
{
	if (rowVersion == null || rowVersion.Length == 0)
		return new BanActionResultDTO { ThanhCong = false, ThongBao = "Thieu RowVersion." };

	using var context = new CaPheDbContext();
	var hoaDon = context.HoaDon.FirstOrDefault(x => x.ID == hoaDonId);
	if (hoaDon == null) return Fail("Khong tim thay hoa don.");

	context.Entry(hoaDon).Property(x => x.RowVersion).OriginalValue = rowVersion;

	if (hoaDon.TrangThai != (int)HoaDonTrangThai.Draft)
		return Fail("Chi cap nhat khach cho hoa don Draft.");

	// cap nhat...
	context.SaveChanges();
	return Success();
}
```

### 6. [HIGH][5] Bypass phan quyen nghiep vu do form goi thang OrderService, dua vao an/hien nut UI

**Evidence:**
- [frmHoaDon.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L18)
- [frmHoaDon.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L61)
- [frmHoaDon.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L1054)
- [frmHoaDon.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L1368)
- UI-gate tai [frmHoaDon.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L1464)
- Service-level permission co o [HoaDonBUS.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/BUS/HoaDonBUS.cs#L242)

**Impact:**
Tang nghiep vu bi bypass o mot so luong thao tac hoa don.

**Fix C# truc tiep:**

```csharp
// frmHoaDon.cs: thay _orderService bang _hoaDonBUS
ketQua = await Task.Run(() => _hoaDonBUS.ThemMonVaoHoaDon(
	hoaDon.ID, monId, soLuong, _hoaDonDangChonRowVersion));

ketQua = await Task.Run(() => _hoaDonBUS.XacNhanThuTien(
	hoaDonId, tienKhachDua, _hoaDonDangChonRowVersion));

// Va bo sung guard trong OrderService (defense in depth) neu van de public.
```

### 7. [HIGH][3] Do UI khi du lieu lon: truy van sync tren UI thread + khong phan trang

**Evidence:**
- [frmHoaDon.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L301)
- [frmHoaDon.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmHoaDon.cs#L311)
- [frmBanHang.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmBanHang.cs#L773)
- [frmBanHang.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmBanHang.cs#L934)
- [frmThongKe.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmThongKe.cs#L528)
- [frmThongKe.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmThongKe.cs#L578)
- [HoaDonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L527)
- [BanDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/BanDAL.cs#L85)
- [MonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/MonDAL.cs#L344)
- [ThongKeDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/ThongKeDAL.cs#L23)

**Impact:**
Du lieu tang se khoa UI, lag nhap lieu, timeout thao tac.

**Fix C# truc tiep:**

```csharp
// DTO filter
public int PageNumber { get; set; } = 1;
public int PageSize { get; set; } = 50;

// DAL
return query
	.OrderByDescending(x => x.NgayLap)
	.ThenByDescending(x => x.ID)
	.Skip((boLoc.PageNumber - 1) * boLoc.PageSize)
	.Take(boLoc.PageSize)
	.Select(...)
	.ToList();

// Form
var dsHoaDon = await Task.Run(() => _hoaDonBUS.LayDanhSachHoaDon(boLoc));
```

### 8. [HIGH][2][4] Rang buoc DB cho du lieu nghiep vu con thieu, dang dua vao check o BUS nen race-condition van lot

**Evidence:**
- Check o BUS [KhachHangBUS.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/BUS/KhachHangBUS.cs#L67)
- [LoaiMonBUS.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/BUS/LoaiMonBUS.cs#L54)
- [BanBUS.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/BUS/BanBUS.cs#L71)
- Unique DB chi thay ro cho Users tai [CaPheDbContext.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L467)
- [CaPheDbContext.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Data/CaPheDbContext.cs#L468)

**Impact:**
Trung du lieu quan trong (ten ban, ten loai, so dien thoai) khi concurrent insert; tiem an sai KPI, sai lookup, loi nghiep vu day chuyen.

**Fix SQL truc tiep:**

```sql
-- Chuan hoa uniqueness theo ban ghi active
CREATE UNIQUE INDEX UX_Ban_TenBan ON Ban(TenBan);

CREATE UNIQUE INDEX UX_LoaiMon_TenLoai_Active
ON LoaiMon(TenLoai)
WHERE IsDeleted = 0;

CREATE UNIQUE INDEX UX_KhachHang_DienThoai_Active
ON KhachHang(DienThoai)
WHERE IsDeleted = 0 AND DienThoai IS NOT NULL;

-- Chan gia tri tien/so luong am va outlier
ALTER TABLE Mon ADD CONSTRAINT CK_Mon_DonGia_Range
CHECK (DonGia >= 0 AND DonGia <= 1000000000);

ALTER TABLE HoaDon ADD CONSTRAINT CK_HoaDon_TongTien_Range
CHECK (TongTien >= 0 AND TongTien <= 1000000000000);

ALTER TABLE NguyenLieu ADD CONSTRAINT CK_NguyenLieu_SoLuongTon_NonNegative
CHECK (SoLuongTon >= 0);

ALTER TABLE NguyenLieu ADD CONSTRAINT CK_NguyenLieu_GiaNhapGanNhat_NonNegative
CHECK (GiaNhapGanNhat >= 0);
```

## SHOULD FIX (nen cai thien som)

### 1. [1] Isolation dang qua nang o nhieu luong ghi, de gay nghen duoi tai cao

**Evidence:**
- [OrderService.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/BUS/OrderService.cs#L631)
- [BanDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/BanDAL.cs#L483)
- [HoaDonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L186)

**Khuyen nghi:**
Bat `READ_COMMITTED_SNAPSHOT` o SQL Server, ha isolation ve `ReadCommitted` cho luong khong can `Serializable` tuyet doi, giu `RowVersion` cho optimistic concurrency.

```sql
ALTER DATABASE QuanLyQuanCaPhe SET READ_COMMITTED_SNAPSHOT ON WITH ROLLBACK IMMEDIATE;
```

### 2. [1] Chua co retry chien luoc cho DbUpdateConcurrencyException, hien chi tra loi

**Evidence:**
- [OrderService.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/BUS/OrderService.cs#L668)
- [OrderService.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/BUS/OrderService.cs#L683)
- [HoaDonDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/HoaDonDAL.cs#L314)
- [BanDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/BanDAL.cs#L488)

**Khuyen nghi:**
Retry gioi han 2-3 lan cho luong idempotent, voi reload `RowVersion` moi moi lan.

### 3. [5] Co catch nuot loi lam mat dau root-cause

**Evidence:**
- [frmQuanLiBan.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmQuanLiBan.cs#L126)
- [frmDangNhap.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmDangNhap.cs#L169)
- [frmDangNhap.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Forms/frmDangNhap.cs#L196)

**Khuyen nghi:**
Log vao AppLogger voi correlation id, khong im lang.

### 4. [2] Soft delete ap dung chua dong deu, van con hard delete truc tiep o mot so domain

**Evidence:**
- [BanDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/BanDAL.cs#L296)
- [NguyenLieuDAL.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/DAL/NguyenLieuDAL.cs#L87)
- Entity co soft delete chi gom [dtaLoaiMon.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Data/dtaLoaiMon.cs#L3)
- [dtaKhachHang.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Data/dtaKhachHang.cs#L3)
- [dtaMon.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Data/dtaMon.cs#L3)
- [dtaNhanVien.cs](DoAnLTQL/DoAnLTQL_GD9/QuanLyQuanCaPhe/Data/dtaNhanVien.cs#L3)

**Khuyen nghi:**
Thong nhat chinh sach xoa, them audit cho moi hard delete co chu dich quan tri.

## Danh gia theo yeu cau 1-5

1. **Transactions, concurrency, isolation:** da co transaction va RowVersion cho HoaDon, nhung con lo hong atomicity o goi mon, thieu retry concurrency thuc dung, isolation qua nang.
2. **DB architecture, integrity:** co tien bo check/index quan trong, nhung thieu unique/check o nhieu cot nghiep vu va bien tien.
3. **EF performance, WinForms:** thieu paging + nhieu truy van sync ngay UI thread.
4. **Business logic/state machine:** lifecycle hoa don co ban tot, nhung cap nhat khach chua khoa bang RowVersion va snapshot ten mon chua bat bien.
5. **Exception/logging/security:** co global exception handler, nhung van co catch nuot loi va van de mat khau mac dinh cuc nguy hiem.

## Destructive Testing (8 test pha hoai uu tien cao)

1. Race Checkout kep tu 2 may cho cung hoa don voi cung RowVersion.
   - Ky vong an toan: chi 1 lenh thanh cong, lenh con lai tra conflict, khong am kho, khong duplicate audit pay.

2. Tao hoa don moi roi ngat ket noi ngay truoc AddItems.
   - Ky vong an toan: khong de lai ban trang thai co khach voi hoa don rong.

3. Dong thoi AddItem va ChuyenHoacGopBan tren cung ban.
   - Ky vong an toan: mot giao dich thang ro rang, giao dich con lai rollback sach, khong mat chi tiet hoa don.

4. Dong thoi Checkout va CapNhatKhachHangChoHoaDonMo.
   - Ky vong an toan: thao tac update khach phai fail do RowVersion conflict.

5. Goi XoaLoai khi so mon lon, dong thoi user khac sua mon thuoc loai do.
   - Ky vong an toan: khong partial update; hoac tat ca thanh cong, hoac tat ca rollback.

6. Doi mat khau admin thu cong roi restart app.
   - Ky vong an toan: khong bi reset ve 123.

7. Lam loi bang AuditLog (vi du deny insert) roi thuc hien thanh toan.
   - Ky vong an toan tuy chinh sach: hoac rollback toan bo nghiep vu, hoac commit nghiep vu nhung ghi outbox/fallback bat buoc; tuyet doi khong im lang mat log.

8. Seed 100k hoa don, mo man hinh hoa don/thong ke va thao tac loc lien tuc.
   - Ky vong an toan: UI khong freeze, co paging, thoi gian phan hoi chap nhan duoc.

## Action Items chot

1. **MUST FIX ngay:** bo reset password 123, atomic hoa luong goi mon, sua partial-commit XoaLoai, dong lo hong audit, them RowVersion cho cap nhat khach, bo bypass quyen qua OrderService, them paging/async, bo sung DB constraints.
2. **SHOULD FIX truoc release chinh thuc:** toi uu isolation strategy, retry concurrency co kiem soat, chuan hoa exception logging, dong nhat chinh sach soft delete.
3. Neu can, buoc tiep theo la tao patch truc tiep cho tung file theo thu tu uu tien P0 den P2 va kem migration SQL an toan de chay tren du lieu hien tai.
