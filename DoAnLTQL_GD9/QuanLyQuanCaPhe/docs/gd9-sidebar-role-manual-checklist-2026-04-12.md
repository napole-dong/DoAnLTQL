# GD9 Sidebar Role Manual Checklist (2026-04-12)

## Scope
- Forms: frmBanHang, frmQuanLiBan, frmQuanLiMon, frmCongThuc, frmHoaDon, frmQuanLiKho, frmKhachHang, frmNhanVien, frmThongKe
- Sidebar menu order (unified):
  - Ban hang
  - Quan ly ban
  - Quan ly mon
  - Cong thuc
  - Quan ly kho
  - Hoa don
  - Khach hang
  - Nhan vien
  - Thong ke

## Default test accounts
- admin / 123
- manager / 123
- staff / 123

## Expected visibility by role
- Admin:
  - Visible: Ban hang, Quan ly ban, Quan ly mon, Cong thuc, Quan ly kho, Hoa don, Khach hang, Nhan vien, Thong ke
- Manager:
  - Visible: Ban hang, Quan ly ban, Quan ly mon, Cong thuc, Quan ly kho, Hoa don, Khach hang, Thong ke
  - Hidden: Nhan vien
- Staff:
  - Visible: Ban hang, Quan ly ban, Quan ly mon, Cong thuc, Hoa don
  - Hidden: Quan ly kho, Khach hang, Nhan vien, Thong ke

## Quick runtime steps
1. Start app and login as one role.
2. On each opened screen, verify:
   - Sidebar keeps same order as scope list.
   - Correct buttons are visible/hidden by role.
   - Active button highlight matches current form.
3. Navigate through all visible menu entries and repeat check.
4. Logout, switch role, repeat.

## Per-role x per-form check grid
- Mark PASS only when all 3 conditions are true on that form:
  - Order is correct
  - Visible/hidden set is correct
  - Active highlight is correct

### Admin
- [ ] frmBanHang
- [ ] frmQuanLiBan
- [ ] frmQuanLiMon
- [ ] frmCongThuc
- [ ] frmHoaDon
- [ ] frmQuanLiKho
- [ ] frmKhachHang
- [ ] frmNhanVien
- [ ] frmThongKe

### Manager
- [ ] frmBanHang
- [ ] frmQuanLiBan
- [ ] frmQuanLiMon
- [ ] frmCongThuc
- [ ] frmHoaDon
- [ ] frmQuanLiKho
- [ ] frmKhachHang
- [ ] frmNhanVien (expected to be hidden from sidebar)
- [ ] frmThongKe

### Staff
- [ ] frmBanHang
- [ ] frmQuanLiBan
- [ ] frmQuanLiMon
- [ ] frmCongThuc
- [ ] frmHoaDon
- [ ] frmQuanLiKho (expected to be hidden from sidebar)
- [ ] frmKhachHang (expected to be hidden from sidebar)
- [ ] frmNhanVien (expected to be hidden from sidebar)
- [ ] frmThongKe (expected to be hidden from sidebar)

## Static verification already completed in this session
- Sidebar runtime unification hook found in 9/9 forms.
- Runtime click binding for both dynamic buttons found in 9/9 forms.
- Both dynamic buttons passed into sidebar visibility helper in 9/9 forms.
- Build status: PASS (`dotnet build QuanLyQuanCaPhe.csproj`).