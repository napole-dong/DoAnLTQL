using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.SoftDelete;

namespace QuanLyQuanCaPhe.DAL;

public class KhachHangDAL : IKhachHangRepository
{
    private readonly ISoftDeleteService _softDeleteService = new SoftDeleteService();
    private readonly IActivityLogWriter _activityLogWriter;

    public KhachHangDAL(IActivityLogWriter? activityLogWriter = null)
    {
        _activityLogWriter = AppServiceProvider.Resolve(activityLogWriter, () => new ActivityLogService());
    }

    public List<KhachHangDTO> GetDanhSachKhach(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var khachRows = QueryDanhSachKhachRows(context, tuKhoa);
        return MapKhachHangDtos(khachRows);
    }

    public int GetNextKhachHangId()
    {
        using var context = new CaPheDbContext();
        return (context.KhachHang
            .IgnoreQueryFilters()
            .Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public bool DienThoaiDaTonTai(string dienThoai, int? boQuaId = null)
    {
        if (string.IsNullOrWhiteSpace(dienThoai))
        {
            return false;
        }

        using var context = new CaPheDbContext();
        return context.KhachHang.Any(x =>
            x.DienThoai == dienThoai
            && (!boQuaId.HasValue || x.ID != boQuaId.Value));
    }

    public KhachHangDTO ThemKhach(KhachHangDTO khachDTO)
    {
        using var context = new CaPheDbContext();
        var khach = new dtaKhachHang
        {
            HoVaTen = khachDTO.HoVaTen,
            DienThoai = string.IsNullOrWhiteSpace(khachDTO.DienThoai) ? null : khachDTO.DienThoai,
            DiaChi = string.IsNullOrWhiteSpace(khachDTO.DiaChi) ? null : khachDTO.DiaChi
        };

        context.KhachHang.Add(khach);
        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.CreateCustomer,
            entity: "KhachHang",
            entityId: khach.ID.ToString(),
            description: $"Đã thêm khách hàng {khach.HoVaTen}.",
            oldValue: null,
            newValue: TaoKhachHangSnapshot(khach),
            performedBy: nguoiDung?.TenDangNhap);

        khachDTO.ID = khach.ID;
        return khachDTO;
    }

    public bool CapNhatKhach(KhachHangDTO khachDTO)
    {
        using var context = new CaPheDbContext();
        var khach = context.KhachHang.FirstOrDefault(x => x.ID == khachDTO.ID);
        if (khach == null)
        {
            return false;
        }

        var oldSnapshot = TaoKhachHangSnapshot(khach);
        var hoVaTenCu = khach.HoVaTen;
        var dienThoaiCu = khach.DienThoai;
        var diaChiCu = khach.DiaChi;

        khach.HoVaTen = khachDTO.HoVaTen;
        khach.DienThoai = string.IsNullOrWhiteSpace(khachDTO.DienThoai) ? null : khachDTO.DienThoai;
        khach.DiaChi = string.IsNullOrWhiteSpace(khachDTO.DiaChi) ? null : khachDTO.DiaChi;
        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.UpdateCustomer,
            entity: "KhachHang",
            entityId: khach.ID.ToString(),
            description: TaoMoTaCapNhatKhachHang(
                khach.ID,
                hoVaTenCu,
                khach.HoVaTen,
                dienThoaiCu,
                khach.DienThoai,
                diaChiCu,
                khach.DiaChi),
            oldValue: oldSnapshot,
            newValue: TaoKhachHangSnapshot(khach),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool XoaKhach(int khachId)
    {
        using var context = new CaPheDbContext();
        var khach = context.KhachHang
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == khachId);
        if (khach == null)
        {
            return false;
        }

        var oldSnapshot = TaoKhachHangSnapshot(khach);
        var daXoa = _softDeleteService.SoftDelete(context, khach);
        if (!daXoa)
        {
            return false;
        }

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.DeleteCustomer,
            entity: "KhachHang",
            entityId: khach.ID.ToString(),
            description: $"Đã ngừng hoạt động khách hàng {khach.HoVaTen}.",
            oldValue: oldSnapshot,
            newValue: TaoKhachHangSnapshot(khach),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool RestoreKhach(int khachId)
    {
        using var context = new CaPheDbContext();
        var khach = context.KhachHang
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == khachId);
        if (khach == null || !khach.IsDeleted)
        {
            return false;
        }

        var oldSnapshot = TaoKhachHangSnapshot(khach);
        _softDeleteService.Restore(context, khach);

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.RestoreCustomer,
            entity: "KhachHang",
            entityId: khach.ID.ToString(),
            description: $"Đã khôi phục khách hàng {khach.HoVaTen}.",
            oldValue: oldSnapshot,
            newValue: TaoKhachHangSnapshot(khach),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool HardDeleteKhach(int khachId)
    {
        using var context = new CaPheDbContext();
        var khach = context.KhachHang
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == khachId);
        if (khach == null)
        {
            return false;
        }

        var oldSnapshot = TaoKhachHangSnapshot(khach);
        _softDeleteService.HardDelete(context, khach);

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.HardDeleteCustomer,
            entity: "KhachHang",
            entityId: khachId.ToString(),
            description: $"Đã xóa vĩnh viễn khách hàng {khach.HoVaTen}.",
            oldValue: oldSnapshot,
            newValue: new
            {
                DeletedPermanently = true,
                KhachHangId = khachId
            },
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    private static object TaoKhachHangSnapshot(dtaKhachHang khach)
    {
        return new
        {
            khach.ID,
            khach.HoVaTen,
            khach.DienThoai,
            khach.DiaChi,
            khach.IsDeleted,
            khach.DeletedAt,
            khach.DeletedBy
        };
    }

    private static string TaoMoTaCapNhatKhachHang(
        int khachId,
        string hoVaTenCu,
        string hoVaTenMoi,
        string? dienThoaiCu,
        string? dienThoaiMoi,
        string? diaChiCu,
        string? diaChiMoi)
    {
        var moTa = $"Đã cập nhật khách hàng {hoVaTenMoi} (ID: {khachId}).";

        if (!string.Equals(hoVaTenCu, hoVaTenMoi, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Họ tên: {hoVaTenCu} -> {hoVaTenMoi}.";
        }

        if (!string.Equals(dienThoaiCu ?? string.Empty, dienThoaiMoi ?? string.Empty, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Điện thoại: {(dienThoaiCu ?? "-")} -> {(dienThoaiMoi ?? "-")}.";
        }

        if (!string.Equals(diaChiCu ?? string.Empty, diaChiMoi ?? string.Empty, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Địa chỉ: {(diaChiCu ?? "-")} -> {(diaChiMoi ?? "-")}.";
        }

        return moTa;
    }

    private static List<KhachHangReadModel> QueryDanhSachKhachRows(CaPheDbContext context, string? tuKhoa)
    {
        var query = context.KhachHang
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            var keywordPattern = $"%{keyword}%";
            var hasKeywordId = int.TryParse(keyword, out var keywordId);

            query = query.Where(x =>
                (hasKeywordId && x.ID == keywordId)
                || EF.Functions.Like(x.HoVaTen, keywordPattern)
                || EF.Functions.Like(x.DienThoai ?? string.Empty, keywordPattern)
                || EF.Functions.Like(x.DiaChi ?? string.Empty, keywordPattern));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new KhachHangReadModel
            {
                ID = x.ID,
                HoVaTen = x.HoVaTen,
                DienThoai = x.DienThoai,
                DiaChi = x.DiaChi,
                IsDeleted = x.IsDeleted,
                DeletedAt = x.DeletedAt,
                DeletedBy = x.DeletedBy
            })
            .ToList();
    }

    private static List<KhachHangDTO> MapKhachHangDtos(IEnumerable<KhachHangReadModel> khachRows)
    {
        return khachRows
            .Select(MapKhachHangDto)
            .ToList();
    }

    private static KhachHangDTO MapKhachHangDto(KhachHangReadModel khachRow)
    {
        return new KhachHangDTO
        {
            ID = khachRow.ID,
            HoVaTen = khachRow.HoVaTen,
            DienThoai = khachRow.DienThoai,
            DiaChi = khachRow.DiaChi,
            IsDeleted = khachRow.IsDeleted,
            DeletedAt = khachRow.DeletedAt,
            DeletedBy = khachRow.DeletedBy
        };
    }

    private sealed class KhachHangReadModel
    {
        public int ID { get; init; }
        public string HoVaTen { get; init; } = string.Empty;
        public string? DienThoai { get; init; }
        public string? DiaChi { get; init; }
        public bool IsDeleted { get; init; }
        public DateTime? DeletedAt { get; init; }
        public string? DeletedBy { get; init; }
    }
}
