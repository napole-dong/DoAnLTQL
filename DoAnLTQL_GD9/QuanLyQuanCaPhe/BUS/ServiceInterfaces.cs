using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public interface IBanService
{
    BanThongKeDTO LayThongKe();
    List<BanDTO> LayDanhSachBan(string? khuVuc, string? trangThai, string? tuKhoa);
    List<BanDTO> LaySoDoBan();
    int LayHoacTaoBanMangDi();
    BanActionResultDTO ThemBan(string tenBan);
    BanDTO? LayBanTheoId(int banId);
    BanActionResultDTO XoaBan(int banId);
    BanActionResultDTO DonBan(int banId);
    List<BanDTO> LayDanhSachBanDich(int banNguonId);
    BanActionResultDTO ChuyenHoacGopBan(BanChuyenGopRequestDTO request);
}

public interface IBanHangService
{
    BanHangPhieuDTO LayPhieuTheoBan(int banId);
    BanHangTrangThaiPhieuDTO LayTrangThaiPhieuTheoBan(int banId);
    BanActionResultDTO ThemMonVaoGioTam(int banId, MonDTO mon);
    BanActionResultDTO XoaMonKhoiBan(int banId, int monId, decimal donGia, short soLuong);
    bool CoMonChoGoiTrongGioTam(int banId);
    BanActionResultDTO LuuMonChoGoi(int banId);
    BanActionResultDTO LuuMonChoGoiVoiKhachHang(int banId, int? khachHangId);
    BanActionResultDTO ThanhToanHoaDon(int banId);
    BanActionResultDTO ThanhToanHoaDonVoiKhachHang(int banId, int? khachHangId);
    List<MonDTO> LocMonPhuHopBanHang(IEnumerable<MonDTO> dsMon, string? boLocLoaiMon);
    BanActionResultDTO GoiMon(int banId, IEnumerable<BanHangThemMonDTO> dsMonThem);
    BanActionResultDTO ThanhToan(int banId);
}

public interface IAuditLogService
{
    List<AuditLogDTO> LayDanhSachAuditLog(AuditLogFilterDTO boLoc);
    List<string> LayDanhSachHanhDong();
    List<string> LayDanhSachBangDuLieu();
    AuditLogSummaryDTO TinhTongQuan(IEnumerable<AuditLogDTO> danhSachLog);
}

public interface ICongThucService
{
    List<CongThucMonDTO> LayDanhSachCongThuc(string? tuKhoa, int? monId);
    List<MonDTO> LayDanhSachMonChoCongThuc();
    List<NguyenLieuDTO> LayDanhSachNguyenLieuChoCongThuc();
    (bool ThanhCong, string ThongBao) ThemCongThuc(int monId, int nguyenLieuId, decimal soLuong);
    (bool ThanhCong, string ThongBao) CapNhatCongThuc(int monId, int nguyenLieuId, decimal soLuong);
    (bool ThanhCong, string ThongBao) XoaCongThuc(int monId, int nguyenLieuId);
}

public interface IDangNhapService
{
    (bool ThanhCong, string ThongBao, string TruongLoi, ThongTinDangNhapDTO? ThongTinDangNhap) DangNhap(string tenDangNhap, string matKhau);
}

public interface IHoaDonService
{
    List<HoaDonDTO> LayDanhSachHoaDon(HoaDonFilterDTO boLoc);
    Task<List<HoaDonDTO>> LayDanhSachHoaDonAsync(HoaDonFilterDTO boLoc, CancellationToken cancellationToken = default);
    HoaDonDTO? LayHoaDonTheoId(int hoaDonId);
    int LayMaHoaDonTiepTheo();
    List<HoaDonBanKhachItemDTO> LayDanhSachBanKhach();
    List<HoaDonMonItemDTO> LayDanhSachMonDangKinhDoanh();
    (BanActionResultDTO Result, int HoaDonId) ThemHoaDon(HoaDonSaveRequestDTO request);
    (BanActionResultDTO Result, int HoaDonId) CreateInvoice(HoaDonSaveRequestDTO request);
    BanActionResultDTO CapNhatHoaDon(HoaDonSaveRequestDTO request);
    BanActionResultDTO ThemMonVaoHoaDon(int hoaDonId, int monId, short soLuong, byte[]? rowVersion = null);
    BanActionResultDTO CapNhatSoLuongMonTrongHoaDon(int hoaDonId, int monId, short soLuong, byte[]? rowVersion = null);
    BanActionResultDTO HuyHoaDon(int hoaDonId, byte[]? rowVersion = null);
    BanActionResultDTO HuyHoaDon(int hoaDonId, string reason, string user, byte[]? rowVersion = null);
    BanActionResultDTO XacNhanThuTien(int hoaDonId, decimal tienKhachDua, byte[]? rowVersion = null);
    decimal TinhTienThoi(decimal tongTien, decimal tienKhachDua);
}

public interface IKhachHangService
{
    List<KhachHangDTO> LayDanhSachKhach(string? textTimKhach);
    List<KhachHangDTO> LayDanhSachKhachChoBanHang(string? textTimKhach);
    int LayMaKhachTiepTheo();
    (bool ThanhCong, string ThongBao, KhachHangDTO? KhachMoi) ThemKhach(KhachHangDTO khachDTO);
    (bool ThanhCong, string ThongBao, KhachHangDTO? KhachMoi) ThemKhachNhanhChoBanHang(KhachHangDTO khachDTO);
    (bool ThanhCong, string ThongBao) CapNhatKhach(KhachHangDTO khachDTO);
    (bool ThanhCong, string ThongBao) XoaKhach(int khachId);
    (bool ThanhCong, string ThongBao) KhoiPhucKhach(int khachId);
    (bool ThanhCong, string ThongBao) HardDeleteKhach(int khachId);
}

public interface ILoaiMonService
{
    List<LoaiMonDTO> LayDanhSachLoai(string? textSearch, string? textTimLoai);
    int LayMaLoaiTiepTheo();
    (bool ThanhCong, string ThongBao, LoaiMonDTO? LoaiMoi) ThemLoai(string tenLoai, string? moTa);
    (bool ThanhCong, string ThongBao) CapNhatLoai(int id, string tenLoai, string? moTa);
    (bool ThanhCong, string ThongBao) XoaLoai(int id);
    (bool ThanhCong, string ThongBao) ChuyenMonSangLoaiKhac(int loaiNguonId, int loaiDichId);
}

public interface IMonService
{
    List<LoaiMonDTO> LayDanhSachLoaiMon();
    List<MonDTO> LayDanhSachMon(string? textSearch, string? textTimMon);
    int LayMaMonTiepTheo();
    (bool ThanhCong, string ThongBao, MonDTO? MonMoi) ThemMon(MonDTO monDTO);
    (bool ThanhCong, string ThongBao) CapNhatMon(MonDTO monDTO);
    (bool ThanhCong, string ThongBao) XoaMon(int monId);
    (bool ThanhCong, string ThongBao) DeleteMenu(int monId);
    (bool ThanhCong, string ThongBao) KhoiPhucMon(int monId);
    (bool ThanhCong, string ThongBao) HardDeleteMon(int monId);
}

public interface INguyenLieuService
{
    List<NguyenLieuDTO> LayDanhSachNguyenLieu(string? tuKhoa);
    List<NguyenLieuDTO> LayDanhSachNguyenLieuSapHet();
    int LayMaNguyenLieuTiepTheo();
    (bool ThanhCong, string ThongBao, NguyenLieuDTO? NguyenLieuMoi) ThemNguyenLieu(NguyenLieuDTO nguyenLieuDTO);
    (bool ThanhCong, string ThongBao) CapNhatNguyenLieu(NguyenLieuDTO nguyenLieuDTO);
    (bool ThanhCong, string ThongBao) XoaNguyenLieu(int maNguyenLieu);
    (bool ThanhCong, string ThongBao) NhapKhoNhieuNguyenLieu(IEnumerable<NhapKhoChiTietDTO> dsChiTiet, string? ghiChu);
    (bool ThanhCong, string ThongBao) NhapKho(int maNguyenLieu, decimal soLuongNhap, decimal giaNhap, string? ghiChu);
}

public interface INhanVienService
{
    Task<List<NhanVienDTO>> LayDanhSachNhanVienAsync(string? tuKhoa);
    int LayMaNhanVienTiepTheo();
    List<string> LayDanhSachVaiTroCoTheGan();
    Task<(bool ThanhCong, string ThongBao, NhanVienDTO? NhanVienMoi)> ThemNhanVienAsync(NhanVienDTO nhanVienDTO);
    Task<(bool ThanhCong, string ThongBao)> CapNhatNhanVienAsync(NhanVienDTO nhanVienDTO);
    (bool ThanhCong, string ThongBao) XoaNhanVien(int nhanVienId, bool softDelete = true);
    (bool ThanhCong, string ThongBao) KhoiPhucNhanVien(int nhanVienId);
    (bool ThanhCong, string ThongBao) HardDeleteNhanVien(int nhanVienId);
}

public interface IOrderService
{
    OperationResult AddItemsByTableAtomic(int banId, IEnumerable<BanHangThemMonDTO> dsMonThem, int? khachHangId = null);
    BanActionResultDTO AddItemToOrder(int orderId, int productId, short quantity, byte[]? expectedRowVersion = null);
    BanActionResultDTO AddItemsToOrder(int orderId, IEnumerable<BanHangThemMonDTO> dsMonThem, string successMessage = "Gọi món thành công.", byte[]? expectedRowVersion = null);
    BanActionResultDTO RemoveItemFromOrder(int orderId, int productId, short quantity, byte[]? expectedRowVersion = null);
    BanActionResultDTO UpdateItemQuantity(int orderId, int productId, short quantity, byte[]? expectedRowVersion = null);
    BanActionResultDTO ReplaceItemInOrder(int orderId, int currentProductId, int replacementProductId, short quantity, byte[]? expectedRowVersion = null);
    BanActionResultDTO CancelOrder(int orderId, byte[]? expectedRowVersion = null);
    BanActionResultDTO VoidInvoice(int orderId, string reason, string user, byte[]? expectedRowVersion = null);
    BanActionResultDTO CancelInvoice(int orderId, string reason, string user, byte[]? expectedRowVersion = null);
    BanActionResultDTO Checkout(int orderId, byte[]? expectedRowVersion = null);
}

public interface IPermissionService
{
    bool CheckPermission(ActionType action, Feature feature);
    bool DongBoDuLieuQuyenMacDinh();
    bool HasPermission(ActionType action, Feature feature);
    bool CheckPermission(string feature, string action);
    void EnsurePermission(ActionType action, Feature feature, string? message = null);
    void EnsurePermission(string feature, string action, string? message = null);
    bool IsAdmin();
    bool IsManager();
    bool IsStaff();
    UserSession? LayUserSession();
    bool CanManageEmployees();
    bool CanEditMenuPrice();
    bool CanDeleteInvoice();
    bool CanTransferTable();
    bool CanMergeTable();
    bool CanManageUser();
    bool CanManageMenu();
    bool CanManageInventory();
    bool CanViewReport();
    bool CanSell();
    List<string> LayDanhSachVaiTroCoTheGan();
    bool CoTheGanVaiTro(string tenVaiTro);
}

public interface ITaiKhoanService
{
    (bool ThanhCong, string ThongBao) DeleteUser(int userId);
}

public interface IThongKeService
{
    List<ThongKeHoaDonDTO> LayDanhSachHoaDonDaThanhToan(DateTime tuNgay, DateTime denNgay, string? tuKhoa);
    List<ThongKeTopMonDTO> LayTopMonBanChay(DateTime tuNgay, DateTime denNgay, string? tuKhoa, int soLuongTop = 10);
    int LaySoHoaDonHuy(DateTime tuNgay, DateTime denNgay, string? tuKhoa);
}
