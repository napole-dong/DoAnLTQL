using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public interface IBanRepository
{
    BanThongKeDTO GetThongKe();
    List<BanDTO> GetDanhSachBan(string? khuVuc, string? trangThai, string? tuKhoa);
    List<BanDTO> GetSoDoBan();
    int LayHoacTaoBanMangDi();
    bool TenBanDaTonTai(string tenBan);
    void ThemBan(string tenBan);
    BanDTO? GetBanById(int banId);
    BanActionResultDTO DonBan(int banId);
    bool BanDaPhatSinhHoaDon(int banId);
    bool XoaBan(int banId);
    List<BanDTO> GetDanhSachBanDich(int banNguonId);
    BanActionResultDTO ChuyenHoacGopBan(BanChuyenGopRequestDTO request);
}

public interface IBanHangRepository
{
    BanHangPhieuDTO GetPhieuTheoBan(int banId);
    BanActionResultDTO GoiMon(int banId, IEnumerable<BanHangThemMonDTO> dsMonThem);
    BanActionResultDTO ThanhToan(int banId);
}

public interface IAuditLogRepository
{
    List<AuditLogDTO> LayDanhSachAuditLog(AuditLogFilterDTO boLoc);
    List<string> LayDanhSachHanhDong();
    List<string> LayDanhSachBangDuLieu();
}

public interface ICongThucRepository
{
    List<CongThucMonDTO> GetDanhSachCongThuc(string? tuKhoa, int? monId);
    (bool ThanhCong, string ThongBao) ThemCongThuc(int monId, int nguyenLieuId, decimal soLuong);
    (bool ThanhCong, string ThongBao) CapNhatCongThuc(int monId, int nguyenLieuId, decimal soLuong);
    (bool ThanhCong, string ThongBao) XoaCongThuc(int monId, int nguyenLieuId);
}

public interface IDangNhapRepository
{
    ThongTinDangNhapDTO? XacThucDangNhap(string tenDangNhap, string matKhau);
    bool TonTaiTenDangNhap(string tenDangNhap);
    bool LaTaiKhoanHoatDong(string tenDangNhap);
}

public interface IHoaDonRepository
{
    List<HoaDonDTO> GetDanhSachHoaDon(HoaDonFilterDTO boLoc);
    HoaDonDTO? GetHoaDonTheoId(int hoaDonId);
    int GetNextHoaDonId();
    List<HoaDonBanKhachItemDTO> GetDanhSachBanKhach();
    List<HoaDonMonItemDTO> GetDanhSachMonDangKinhDoanh();
    (bool ThanhCong, string ThongBao, int HoaDonId) ThemHoaDon(HoaDonSaveRequestDTO request);
    BanActionResultDTO CapNhatHoaDon(HoaDonSaveRequestDTO request);
    BanActionResultDTO ThemMonVaoHoaDon(int hoaDonId, int monId, short soLuong, byte[]? rowVersion = null);
    BanActionResultDTO CapNhatSoLuongMonTrongHoaDon(int hoaDonId, int monId, short soLuong, byte[]? rowVersion = null);
    BanActionResultDTO HuyHoaDon(int hoaDonId, byte[]? rowVersion = null);
    BanActionResultDTO HuyHoaDon(int hoaDonId, string reason, string user, byte[]? rowVersion = null);
    BanActionResultDTO XacNhanThuTien(int hoaDonId, byte[]? rowVersion = null);
    BanActionResultDTO CapNhatKhachHangChoHoaDonMo(int hoaDonId, int? khachHangId);
}

public interface IKhachHangRepository
{
    List<KhachHangDTO> GetDanhSachKhach(string? tuKhoa);
    int GetNextKhachHangId();
    bool DienThoaiDaTonTai(string dienThoai, int? boQuaId = null);
    KhachHangDTO ThemKhach(KhachHangDTO khachDTO);
    bool CapNhatKhach(KhachHangDTO khachDTO);
    bool XoaKhach(int khachId);
    bool RestoreKhach(int khachId);
    bool HardDeleteKhach(int khachId);
}

public interface ILoaiMonRepository
{
    bool LoaiTonTai(int id);
    List<LoaiMonDTO> GetDanhSachLoai(string? tuKhoa);
    int GetNextLoaiMonId();
    bool TenLoaiDaTonTai(string tenLoai, int? boQuaId = null);
    LoaiMonDTO ThemLoai(string tenLoai, string? moTa);
    bool CapNhatLoai(int id, string tenLoai, string? moTa);
    bool ChuyenMonSangLoaiKhac(int loaiNguonId, int loaiDichId);
    bool LoaiDangSuDung(int id);
    bool XoaLoai(int id);
}

public interface IMonRepository
{
    List<LoaiMonDTO> GetLoaiMon();
    List<MonDTO> GetDanhSachMon(string? tuKhoa, int? loaiMonId = null);
    int GetNextMonId();
    bool TenMonDaTonTai(string tenMon, int? boQuaId = null);
    MonDTO? GetMonById(int id);
    MonDTO ThemMon(MonDTO monDTO);
    bool CapNhatMon(MonDTO monDTO);
    bool MonDaPhatSinhHoaDon(int monId);
    bool XoaMon(int monId);
    bool RestoreMon(int monId);
    bool HardDeleteMon(int monId);
}

public interface INguyenLieuRepository
{
    List<NguyenLieuDTO> GetDanhSachNguyenLieu(string? tuKhoa);
    List<NguyenLieuDTO> GetDanhSachNguyenLieuSapHet();
    int GetNextNguyenLieuId();
    NguyenLieuDTO ThemNguyenLieu(NguyenLieuDTO nguyenLieuDTO);
    bool CapNhatNguyenLieu(NguyenLieuDTO nguyenLieuDTO);
    bool XoaNguyenLieu(int maNguyenLieu);
    (bool ThanhCong, string ThongBao) NhapKho(int maNguyenLieu, decimal soLuongNhap, decimal giaNhap, string? ghiChu);
}

public interface INhanVienRepository
{
    Task<List<NhanVienDTO>> GetDanhSachNhanVienAsync(string? tuKhoa);
    int GetNextNhanVienId();
    List<string> LayDanhSachTenVaiTro();
    Task<bool> TenDangNhapDaTonTaiAsync(string tenDangNhap, int? boQuaNhanVienId = null);
    Task<NhanVienDTO> ThemNhanVienAsync(NhanVienDTO nhanVienDTO);
    Task<bool> CapNhatNhanVienAsync(NhanVienDTO nhanVienDTO);
    bool NhanVienDaPhatSinhHoaDon(int nhanVienId);
    NhanVienDAL.DeleteNhanVienResult DeleteNhanVien(int nhanVienId, bool softDelete);
    bool XoaNhanVien(int nhanVienId);
    bool RestoreNhanVien(int nhanVienId);
    bool HardDeleteNhanVien(int nhanVienId);
}

public interface IPermissionRepository
{
    bool CheckPermission(int roleId, string feature, string action);
    bool DongBoDuLieuQuyenMacDinh();
    List<string> LayDanhSachTenVaiTro();
    List<string> LayDanhSachVaiTroCoTheGan(int currentRoleId);
    bool CoTheGanVaiTro(int currentRoleId, string targetRoleName);
}

public interface ITaiKhoanRepository
{
    TaiKhoanDAL.DeleteUserResult DeleteUser(int userId);
}

public interface ITaiKhoanMacDinhRepository
{
    TaiKhoanMacDinhDAL.KhoiTaoTaiKhoanMacDinhResult DamBaoTaiKhoanMacDinh(string matKhauMacDinh);
}

public interface IThongKeRepository
{
    List<ThongKeHoaDonDTO> LayDanhSachHoaDonDaThanhToan(DateTime tuNgay, DateTime denNgay, string? tuKhoa);
    List<ThongKeTopMonDTO> LayTopMonBanChay(DateTime tuNgay, DateTime denNgay, string? tuKhoa, int soLuongTop);
    int LaySoHoaDonHuy(DateTime tuNgay, DateTime denNgay, string? tuKhoa);
}
