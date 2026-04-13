using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.DAL;

public class LoaiMonDAL : ILoaiMonRepository
{
    private readonly IActivityLogWriter _activityLogWriter;

    public LoaiMonDAL(IActivityLogWriter? activityLogWriter = null)
    {
        _activityLogWriter = AppServiceProvider.Resolve(activityLogWriter, () => new ActivityLogService());
    }

    public bool LoaiTonTai(int id)
    {
        using var context = new CaPheDbContext();
        return context.LoaiMon.Any(x => x.ID == id);
    }

    public List<LoaiMonDTO> GetDanhSachLoai(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var loaiRows = QueryDanhSachLoaiRows(context, tuKhoa);
        return MapLoaiMonDtos(loaiRows);
    }

    public int GetNextLoaiMonId()
    {
        using var context = new CaPheDbContext();
        return (context.LoaiMon.Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public bool TenLoaiDaTonTai(string tenLoai, int? boQuaId = null)
    {
        using var context = new CaPheDbContext();
        return context.LoaiMon.Any(x => x.TenLoai == tenLoai && (!boQuaId.HasValue || x.ID != boQuaId.Value));
    }

    public LoaiMonDTO ThemLoai(string tenLoai, string? moTa)
    {
        using var context = new CaPheDbContext();
        var loai = new dtaLoaiMon
        {
            TenLoai = tenLoai,
            MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa.Trim()
        };
        context.LoaiMon.Add(loai);
        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.CreateCategory,
            entity: "LoaiMon",
            entityId: loai.ID.ToString(),
            description: $"Đã thêm loại món {loai.TenLoai}.",
            oldValue: null,
            newValue: TaoLoaiMonSnapshot(loai, soMonThuocLoai: 0),
            performedBy: nguoiDung?.TenDangNhap);

        return new LoaiMonDTO
        {
            ID = loai.ID,
            TenLoai = loai.TenLoai,
            SoMon = 0,
            MoTa = loai.MoTa ?? string.Empty
        };
    }

    public bool CapNhatLoai(int id, string tenLoai, string? moTa)
    {
        using var context = new CaPheDbContext();
        var loai = context.LoaiMon.FirstOrDefault(x => x.ID == id);
        if (loai == null)
        {
            return false;
        }

        var soMonThuocLoai = DemSoMonHoatDong(context, id);
        var oldSnapshot = TaoLoaiMonSnapshot(loai, soMonThuocLoai);
        var tenLoaiCu = loai.TenLoai;
        var moTaCu = loai.MoTa;

        loai.TenLoai = tenLoai;
        loai.MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa.Trim();
        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.UpdateCategory,
            entity: "LoaiMon",
            entityId: loai.ID.ToString(),
            description: TaoMoTaCapNhatLoaiMon(loai.ID, tenLoaiCu, loai.TenLoai, moTaCu, loai.MoTa),
            oldValue: oldSnapshot,
            newValue: TaoLoaiMonSnapshot(loai, soMonThuocLoai),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool ChuyenMonSangLoaiKhac(int loaiNguonId, int loaiDichId)
    {
        using var context = new CaPheDbContext();

        var loaiNguon = context.LoaiMon
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == loaiNguonId);
        var loaiDich = context.LoaiMon
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == loaiDichId);
        if (loaiNguon == null || loaiDich == null)
        {
            return false;
        }

        var soMonDaChuyen = context.Mon
            .IgnoreQueryFilters()
            .Where(x => x.LoaiMonID == loaiNguonId)
            .ExecuteUpdate(setters => setters
                .SetProperty(x => x.LoaiMonID, loaiDichId));

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.TransferCategoryItems,
            entity: "LoaiMon",
            entityId: $"{loaiNguonId}->{loaiDichId}",
            description: $"Đã chuyển {soMonDaChuyen} món từ loại {loaiNguon.TenLoai} sang {loaiDich.TenLoai}.",
            oldValue: new
            {
                LoaiNguonId = loaiNguonId,
                TenLoaiNguon = loaiNguon.TenLoai,
                LoaiDichId = loaiDichId,
                TenLoaiDich = loaiDich.TenLoai
            },
            newValue: new
            {
                SoMonDaChuyen = soMonDaChuyen,
                LoaiNguonId = loaiNguonId,
                LoaiDichId = loaiDichId
            },
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool LoaiDangSuDung(int id)
    {
        using var context = new CaPheDbContext();
        return context.Mon
            .IgnoreQueryFilters()
            .Any(x => x.LoaiMonID == id);
    }

    public OperationResult XoaLoai(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID loai mon phai lon hon 0.");
        }

        try
        {
            object? oldSnapshotForLog = null;
            object? newSnapshotForLog = null;
            string? descriptionForLog = null;

            var ketQua = ExecutionStrategyTransactionRunner.Execute(
                context =>
                {
                    var loai = context.LoaiMon
                        .IgnoreQueryFilters()
                        .FirstOrDefault(x => x.ID == id);
                    if (loai == null)
                    {
                        return OperationResult.Failure("Không tìm thấy loại món để xóa.");
                    }

                    if (loai.IsDeleted)
                    {
                        return OperationResult.Failure("Loại món đã được xóa trước đó.");
                    }

                    var soMonHoatDongTruocKhiXoa = context.Mon
                        .IgnoreQueryFilters()
                        .Count(x => x.LoaiMonID == id && !x.IsDeleted);
                    var coMonThuocLoai = soMonHoatDongTruocKhiXoa > 0;

                    oldSnapshotForLog = TaoLoaiMonSnapshot(loai, soMonHoatDongTruocKhiXoa);

                    var soMonDaNgungBan = context.Mon
                        .IgnoreQueryFilters()
                        .Where(x => x.LoaiMonID == id && !x.IsDeleted)
                        .ExecuteUpdate(setters => setters
                            .SetProperty(x => x.TrangThai, 0)
                            .SetProperty(x => x.DonGia, 0m)
                            .SetProperty(x => x.TrangThaiTextLegacy, "Ngừng bán"));

                    var tenLoai = loai.TenLoai;

                    loai.IsDeleted = true;
                    loai.DeletedAt = DateTime.Now;
                    loai.DeletedBy = NguoiDungHienTaiService.LayNguoiDungDangNhap()?.TenDangNhap ?? "system";

                    context.SaveChanges();

                    newSnapshotForLog = TaoLoaiMonSnapshot(loai, soMonDaNgungBan);
                    descriptionForLog = coMonThuocLoai
                        ? $"Đã ngừng bán {soMonDaNgungBan} món thuộc loại {tenLoai} và xóa loại món."
                        : $"Đã xóa loại món {tenLoai}.";

                    return coMonThuocLoai
                        ? OperationResult.Success("Đã ngừng bán toàn bộ món thuộc loại và xóa loại món thành công.")
                        : OperationResult.Success("Xóa loại món thành công.");
                },
                shouldCommit: result => result.ThanhCong,
                isolationLevel: System.Data.IsolationLevel.Serializable);

            if (ketQua.ThanhCong)
            {
                var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
                _activityLogWriter.Log(
                    userId: nguoiDung?.UserId,
                    action: AuditActions.DeleteCategory,
                    entity: "LoaiMon",
                    entityId: id.ToString(),
                    description: descriptionForLog ?? $"Đã xóa loại món ID {id}.",
                    oldValue: oldSnapshotForLog,
                    newValue: newSnapshotForLog,
                    performedBy: nguoiDung?.TenDangNhap);
            }

            return ketQua;
        }
        catch (Exception ex)
        {
            AppLogger.Error(ex, $"XoaLoai failed. LoaiID={id}.", nameof(LoaiMonDAL));
            throw;
        }
    }

    private static int DemSoMonHoatDong(CaPheDbContext context, int loaiId)
    {
        return context.Mon
            .IgnoreQueryFilters()
            .Count(x => x.LoaiMonID == loaiId && !x.IsDeleted);
    }

    private static object TaoLoaiMonSnapshot(dtaLoaiMon loai, int soMonThuocLoai)
    {
        return new
        {
            loai.ID,
            loai.TenLoai,
            loai.MoTa,
            SoMonThuocLoai = soMonThuocLoai,
            loai.IsDeleted,
            loai.DeletedAt,
            loai.DeletedBy
        };
    }

    private static string TaoMoTaCapNhatLoaiMon(int id, string tenLoaiCu, string tenLoaiMoi, string? moTaCu, string? moTaMoi)
    {
        var moTa = $"Đã cập nhật loại món {tenLoaiMoi} (ID: {id}).";

        if (!string.Equals(tenLoaiCu, tenLoaiMoi, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Tên loại: {tenLoaiCu} -> {tenLoaiMoi}.";
        }

        if (!string.Equals(moTaCu ?? string.Empty, moTaMoi ?? string.Empty, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Mô tả: {(string.IsNullOrWhiteSpace(moTaCu) ? "-" : moTaCu)} -> {(string.IsNullOrWhiteSpace(moTaMoi) ? "-" : moTaMoi)}.";
        }

        return moTa;
    }

    private static List<LoaiMonReadModel> QueryDanhSachLoaiRows(CaPheDbContext context, string? tuKhoa)
    {
        var query = context.LoaiMon
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            var keywordPattern = $"%{keyword}%";
            var hasKeywordId = int.TryParse(keyword, out var keywordId);

            query = query.Where(x =>
                (hasKeywordId && x.ID == keywordId)
                || EF.Functions.Like(x.TenLoai, keywordPattern)
                || EF.Functions.Like(x.MoTa ?? string.Empty, keywordPattern));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new LoaiMonReadModel
            {
                ID = x.ID,
                TenLoai = x.TenLoai,
                SoMon = x.Mon.Count,
                MoTa = x.MoTa ?? string.Empty
            })
            .ToList();
    }

    private static List<LoaiMonDTO> MapLoaiMonDtos(IEnumerable<LoaiMonReadModel> loaiRows)
    {
        return loaiRows
            .Select(MapLoaiMonDto)
            .ToList();
    }

    private static LoaiMonDTO MapLoaiMonDto(LoaiMonReadModel loaiRow)
    {
        return new LoaiMonDTO
        {
            ID = loaiRow.ID,
            TenLoai = loaiRow.TenLoai,
            SoMon = loaiRow.SoMon,
            MoTa = loaiRow.MoTa
        };
    }

    private sealed class LoaiMonReadModel
    {
        public int ID { get; init; }
        public string TenLoai { get; init; } = string.Empty;
        public int SoMon { get; init; }
        public string MoTa { get; init; } = string.Empty;
    }
}
