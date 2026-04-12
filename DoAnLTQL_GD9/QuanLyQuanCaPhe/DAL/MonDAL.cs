using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.SoftDelete;

namespace QuanLyQuanCaPhe.DAL;

public class MonDAL : IMonRepository
{
    private readonly ISoftDeleteService _softDeleteService = new SoftDeleteService();
    private readonly IActivityLogWriter _activityLogWriter;

    public MonDAL(IActivityLogWriter? activityLogWriter = null)
    {
        _activityLogWriter = AppServiceProvider.Resolve(activityLogWriter, () => new ActivityLogService());
    }

    public List<LoaiMonDTO> GetLoaiMon()
    {
        using var context = new CaPheDbContext();
        return context.LoaiMon
            .AsNoTracking()
            .OrderBy(x => x.TenLoai)
            .Select(x => new LoaiMonDTO
            {
                ID = x.ID,
                TenLoai = x.TenLoai,
                MoTa = x.MoTa ?? string.Empty
            })
            .ToList();
    }

    public List<MonDTO> GetDanhSachMon(string? tuKhoa, int? loaiMonId = null)
    {
        using var context = new CaPheDbContext();

        var monRows = QueryDanhSachMonRows(context, tuKhoa, loaiMonId);
        return MapMonDtos(monRows);
    }

    public int GetNextMonId()
    {
        using var context = new CaPheDbContext();
        return (context.Mon
            .IgnoreQueryFilters()
            .Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public bool TenMonDaTonTai(string tenMon, int? boQuaId = null)
    {
        var tenMonChuan = tenMon.Trim().ToLower();

        using var context = new CaPheDbContext();
        return context.Mon.Any(x =>
            x.TenMon.Trim().ToLower() == tenMonChuan
            && (!boQuaId.HasValue || x.ID != boQuaId.Value));
    }

    public MonDTO? GetMonById(int id)
    {
        using var context = new CaPheDbContext();

        var monRow = QueryMonByIdRow(context, id);
        return monRow == null ? null : MapMonDto(monRow);
    }

    public MonDTO ThemMon(MonDTO monDTO)
    {
        using var context = new CaPheDbContext();
        var mon = new dtaMon
        {
            TenMon = monDTO.TenMon,
            LoaiMonID = monDTO.LoaiMonID,
            DonGia = monDTO.DonGia,
            TrangThai = monDTO.TrangThai,
            TrangThaiTextLegacy = monDTO.TrangThaiHienThi,
            MoTa = string.IsNullOrWhiteSpace(monDTO.MoTa) ? null : monDTO.MoTa,
            HinhAnh = string.IsNullOrWhiteSpace(monDTO.HinhAnh) ? null : monDTO.HinhAnh
        };

        context.Mon.Add(mon);
        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            nguoiDung?.UserId,
            AuditActions.CreateProduct,
            "Product",
            mon.ID.ToString(),
            $"Đã thêm món {mon.TenMon} với giá {mon.DonGia:N0}.",
            oldValue: null,
            newValue: TaoMonSnapshot(mon),
            performedBy: nguoiDung?.TenDangNhap);

        monDTO.ID = mon.ID;
        return monDTO;
    }

    public bool CapNhatMon(MonDTO monDTO)
    {
        using var context = new CaPheDbContext();
        var mon = context.Mon.FirstOrDefault(x => x.ID == monDTO.ID);
        if (mon == null)
        {
            return false;
        }

        var oldSnapshot = TaoMonSnapshot(mon);
        var tenMonCu = mon.TenMon;
        var donGiaCu = mon.DonGia;
        var trangThaiCu = mon.TrangThai;

        mon.TenMon = monDTO.TenMon;
        mon.LoaiMonID = monDTO.LoaiMonID;
        mon.DonGia = monDTO.DonGia;
        mon.TrangThai = monDTO.TrangThai;
        mon.TrangThaiTextLegacy = monDTO.TrangThaiHienThi;
        mon.MoTa = string.IsNullOrWhiteSpace(monDTO.MoTa) ? null : monDTO.MoTa;
        mon.HinhAnh = string.IsNullOrWhiteSpace(monDTO.HinhAnh) ? null : monDTO.HinhAnh;

        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            nguoiDung?.UserId,
            AuditActions.UpdateProduct,
            "Product",
            mon.ID.ToString(),
            TaoMoTaCapNhatMon(mon.ID, tenMonCu, mon.TenMon, donGiaCu, mon.DonGia, trangThaiCu, mon.TrangThai),
            oldValue: oldSnapshot,
            newValue: TaoMonSnapshot(mon),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool MonDaPhatSinhHoaDon(int monId)
    {
        using var context = new CaPheDbContext();
        return context.HoaDon_ChiTiet.Any(x => x.MonID == monId);
    }

    public bool XoaMon(int monId)
    {
        using var context = new CaPheDbContext();
        var mon = context.Mon
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == monId);
        if (mon == null)
        {
            return false;
        }

        var oldSnapshot = TaoMonSnapshot(mon);
        var daXoa = _softDeleteService.SoftDelete(context, mon);
        if (!daXoa)
        {
            return false;
        }

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            nguoiDung?.UserId,
            AuditActions.DeleteProduct,
            "Product",
            mon.ID.ToString(),
            $"Đã ngừng bán món {mon.TenMon}.",
            oldValue: oldSnapshot,
            newValue: TaoMonSnapshot(mon),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool RestoreMon(int monId)
    {
        using var context = new CaPheDbContext();
        var mon = context.Mon
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == monId);
        if (mon == null || !mon.IsDeleted)
        {
            return false;
        }

        var oldSnapshot = TaoMonSnapshot(mon);
        _softDeleteService.Restore(context, mon);

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            nguoiDung?.UserId,
            AuditActions.UpdateProduct,
            "Product",
            mon.ID.ToString(),
            $"Đã khôi phục món {mon.TenMon} về trạng thái hoạt động.",
            oldValue: oldSnapshot,
            newValue: TaoMonSnapshot(mon),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool HardDeleteMon(int monId)
    {
        using var context = new CaPheDbContext();
        var mon = context.Mon
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == monId);
        if (mon == null)
        {
            return false;
        }

        var oldSnapshot = TaoMonSnapshot(mon);
        _softDeleteService.HardDelete(context, mon);

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            nguoiDung?.UserId,
            AuditActions.DeleteProduct,
            "Product",
            monId.ToString(),
            $"Đã xóa vĩnh viễn món {mon.TenMon}.",
            oldValue: oldSnapshot,
            newValue: new
            {
                DeletedPermanently = true,
                MonId = monId
            },
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    private static object TaoMonSnapshot(dtaMon mon)
    {
        return new
        {
            mon.ID,
            mon.TenMon,
            mon.LoaiMonID,
            mon.DonGia,
            mon.TrangThai,
            mon.MoTa,
            mon.HinhAnh,
            mon.IsDeleted,
            mon.DeletedAt,
            mon.DeletedBy
        };
    }

    private static string TaoMoTaCapNhatMon(
        int monId,
        string tenCu,
        string tenMoi,
        decimal giaCu,
        decimal giaMoi,
        int trangThaiCu,
        int trangThaiMoi)
    {
        var noiDung = $"Đã cập nhật món {tenMoi} (ID: {monId}).";

        if (!string.Equals(tenCu, tenMoi, StringComparison.OrdinalIgnoreCase))
        {
            noiDung += $" Tên: {tenCu} -> {tenMoi}.";
        }

        if (giaCu != giaMoi)
        {
            noiDung += $" Giá: {giaCu:N0} -> {giaMoi:N0}.";
        }

        if (trangThaiCu != trangThaiMoi)
        {
            noiDung += $" Trạng thái: {ChuyenTrangThaiMon(trangThaiCu)} -> {ChuyenTrangThaiMon(trangThaiMoi)}.";
        }

        return noiDung;
    }

    private static string ChuyenTrangThaiMon(int trangThai)
    {
        return trangThai switch
        {
            0 => "Ngừng bán",
            1 => "Đang kinh doanh",
            2 => "Tạm ngừng",
            _ => "Không xác định"
        };
    }

    private static List<MonReadModel> QueryDanhSachMonRows(CaPheDbContext context, string? tuKhoa, int? loaiMonId)
    {
        var query = context.Mon
            .AsNoTracking()
            .AsQueryable();

        if (loaiMonId.HasValue)
        {
            query = query.Where(x => x.LoaiMonID == loaiMonId.Value);
        }

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            var keywordPattern = $"%{keyword}%";
            var hasKeywordId = int.TryParse(keyword, out var keywordId);

            var matchDangKinhDoanh = "Đang kinh doanh".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchNgungBan = "Ngừng bán".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchTamNgung = "Tạm ngừng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchKhongXacDinh = "Không xác định".Contains(keyword, StringComparison.OrdinalIgnoreCase);

            query = query.Where(x =>
                (hasKeywordId && x.ID == keywordId)
                || EF.Functions.Like(x.TenMon, keywordPattern)
                || EF.Functions.Like(x.LoaiMon.TenLoai, keywordPattern)
                || EF.Functions.Like(x.MoTa ?? string.Empty, keywordPattern)
                || (matchDangKinhDoanh && x.TrangThai == 1)
                || (matchNgungBan && x.TrangThai == 0)
                || (matchTamNgung && x.TrangThai == 2)
                || (matchKhongXacDinh && x.TrangThai != 0 && x.TrangThai != 1 && x.TrangThai != 2));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new MonReadModel
            {
                ID = x.ID,
                TenMon = x.TenMon,
                LoaiMonID = x.LoaiMonID,
                TenLoaiMon = x.LoaiMon.TenLoai,
                DonGia = x.DonGia,
                TrangThai = x.TrangThai,
                MoTa = x.MoTa ?? string.Empty,
                HinhAnh = x.HinhAnh,
                IsDeleted = x.IsDeleted,
                DeletedAt = x.DeletedAt,
                DeletedBy = x.DeletedBy
            })
            .ToList();
    }

    private static MonReadModel? QueryMonByIdRow(CaPheDbContext context, int id)
    {
        return context.Mon
            .AsNoTracking()
            .Where(x => x.ID == id)
            .Select(x => new MonReadModel
            {
                ID = x.ID,
                TenMon = x.TenMon,
                LoaiMonID = x.LoaiMonID,
                TenLoaiMon = x.LoaiMon.TenLoai,
                DonGia = x.DonGia,
                TrangThai = x.TrangThai,
                MoTa = x.MoTa ?? string.Empty,
                HinhAnh = x.HinhAnh,
                IsDeleted = x.IsDeleted,
                DeletedAt = x.DeletedAt,
                DeletedBy = x.DeletedBy
            })
            .FirstOrDefault();
    }

    private static List<MonDTO> MapMonDtos(IEnumerable<MonReadModel> monRows)
    {
        return monRows
            .Select(MapMonDto)
            .ToList();
    }

    private static MonDTO MapMonDto(MonReadModel monRow)
    {
        return new MonDTO
        {
            ID = monRow.ID,
            TenMon = monRow.TenMon,
            LoaiMonID = monRow.LoaiMonID,
            TenLoaiMon = monRow.TenLoaiMon,
            DonGia = monRow.DonGia,
            TrangThai = monRow.TrangThai,
            MoTa = monRow.MoTa,
            HinhAnh = monRow.HinhAnh,
            IsDeleted = monRow.IsDeleted,
            DeletedAt = monRow.DeletedAt,
            DeletedBy = monRow.DeletedBy
        };
    }

    private sealed class MonReadModel
    {
        public int ID { get; init; }
        public string TenMon { get; init; } = string.Empty;
        public int LoaiMonID { get; init; }
        public string TenLoaiMon { get; init; } = string.Empty;
        public decimal DonGia { get; init; }
        public int TrangThai { get; init; }
        public string MoTa { get; init; } = string.Empty;
        public string? HinhAnh { get; init; }
        public bool IsDeleted { get; init; }
        public DateTime? DeletedAt { get; init; }
        public string? DeletedBy { get; init; }
    }
}
