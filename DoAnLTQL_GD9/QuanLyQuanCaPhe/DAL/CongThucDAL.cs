using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;

namespace QuanLyQuanCaPhe.DAL;

public class CongThucDAL : ICongThucRepository
{
    private readonly IActivityLogWriter _activityLogWriter;

    public CongThucDAL(IActivityLogWriter? activityLogWriter = null)
    {
        _activityLogWriter = AppServiceProvider.Resolve(activityLogWriter, () => new ActivityLogService());
    }

    public List<CongThucMonDTO> GetDanhSachCongThuc(string? tuKhoa, int? monId)
    {
        using var context = new CaPheDbContext();

        var query = context.CongThucMon
            .AsNoTracking()
            .AsQueryable();

        if (monId.HasValue && monId.Value > 0)
        {
            query = query.Where(x => x.MonID == monId.Value);
        }

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            var keywordPattern = $"%{keyword}%";
            var hasKeywordId = int.TryParse(keyword, out var keywordId);

            query = query.Where(x =>
                (hasKeywordId && (x.MonID == keywordId || x.NguyenLieuID == keywordId))
                || EF.Functions.Like(x.Mon.TenMon, keywordPattern)
                || EF.Functions.Like(x.NguyenLieu.TenNguyenLieu, keywordPattern)
                || EF.Functions.Like(x.NguyenLieu.DonViTinh, keywordPattern));
        }

        return query
            .OrderBy(x => x.MonID)
            .ThenBy(x => x.NguyenLieuID)
            .Select(x => new CongThucMonDTO
            {
                MonID = x.MonID,
                TenMon = x.Mon.TenMon,
                NguyenLieuID = x.NguyenLieuID,
                TenNguyenLieu = x.NguyenLieu.TenNguyenLieu,
                SoLuong = x.SoLuong,
                DonViTinh = x.NguyenLieu.DonViTinh,
                SoLuongTon = x.NguyenLieu.SoLuongTon
            })
            .ToList();
    }

    public (bool ThanhCong, string ThongBao) ThemCongThuc(int monId, int nguyenLieuId, decimal soLuong)
    {
        using var context = new CaPheDbContext();

        var mon = context.Mon
            .AsNoTracking()
            .FirstOrDefault(x => x.ID == monId && !x.IsDeleted && x.TrangThai != 0);
        if (mon == null)
        {
            return (false, "Món không tồn tại hoặc đã ngừng bán.");
        }

        var nguyenLieu = context.NguyenLieu
            .AsNoTracking()
            .FirstOrDefault(x => x.ID == nguyenLieuId && x.TrangThai != 0);
        if (nguyenLieu == null)
        {
            return (false, "Nguyên liệu không tồn tại hoặc đã ngừng dùng.");
        }

        var daTonTai = context.CongThucMon.Any(x => x.MonID == monId && x.NguyenLieuID == nguyenLieuId);
        if (daTonTai)
        {
            return (false, "Công thức của món với nguyên liệu này đã tồn tại.");
        }

        context.CongThucMon.Add(new dtaCongThucMon
        {
            MonID = monId,
            NguyenLieuID = nguyenLieuId,
            SoLuong = soLuong
        });

        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.CreateRecipe,
            entity: "CongThucMon",
            entityId: TaoCongThucEntityId(monId, nguyenLieuId),
            description: $"Đã thêm công thức cho món {mon.TenMon}: {nguyenLieu.TenNguyenLieu} = {soLuong:N3} {nguyenLieu.DonViTinh}.",
            oldValue: null,
            newValue: TaoCongThucSnapshot(monId, mon.TenMon, nguyenLieuId, nguyenLieu.TenNguyenLieu, soLuong, nguyenLieu.DonViTinh),
            performedBy: nguoiDung?.TenDangNhap);

        return (true, "Thêm nguyên liệu vào công thức thành công.");
    }

    public (bool ThanhCong, string ThongBao) CapNhatCongThuc(int monId, int nguyenLieuId, decimal soLuong)
    {
        using var context = new CaPheDbContext();

        var congThuc = context.CongThucMon
            .Include(x => x.Mon)
            .Include(x => x.NguyenLieu)
            .FirstOrDefault(x => x.MonID == monId && x.NguyenLieuID == nguyenLieuId);
        if (congThuc == null)
        {
            return (false, "Không tìm thấy công thức để cập nhật.");
        }

        var oldSnapshot = TaoCongThucSnapshot(
            congThuc.MonID,
            congThuc.Mon.TenMon,
            congThuc.NguyenLieuID,
            congThuc.NguyenLieu.TenNguyenLieu,
            congThuc.SoLuong,
            congThuc.NguyenLieu.DonViTinh);
        var soLuongCu = congThuc.SoLuong;

        congThuc.SoLuong = soLuong;
        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.UpdateRecipe,
            entity: "CongThucMon",
            entityId: TaoCongThucEntityId(monId, nguyenLieuId),
            description: $"Đã cập nhật công thức món {congThuc.Mon.TenMon}: {congThuc.NguyenLieu.TenNguyenLieu} {soLuongCu:N3} -> {soLuong:N3} {congThuc.NguyenLieu.DonViTinh}.",
            oldValue: oldSnapshot,
            newValue: TaoCongThucSnapshot(
                congThuc.MonID,
                congThuc.Mon.TenMon,
                congThuc.NguyenLieuID,
                congThuc.NguyenLieu.TenNguyenLieu,
                congThuc.SoLuong,
                congThuc.NguyenLieu.DonViTinh),
            performedBy: nguoiDung?.TenDangNhap);

        return (true, "Cập nhật công thức thành công.");
    }

    public (bool ThanhCong, string ThongBao) XoaCongThuc(int monId, int nguyenLieuId)
    {
        using var context = new CaPheDbContext();

        var congThuc = context.CongThucMon
            .Include(x => x.Mon)
            .Include(x => x.NguyenLieu)
            .FirstOrDefault(x => x.MonID == monId && x.NguyenLieuID == nguyenLieuId);
        if (congThuc == null)
        {
            return (false, "Không tìm thấy công thức để xóa.");
        }

        var oldSnapshot = TaoCongThucSnapshot(
            congThuc.MonID,
            congThuc.Mon.TenMon,
            congThuc.NguyenLieuID,
            congThuc.NguyenLieu.TenNguyenLieu,
            congThuc.SoLuong,
            congThuc.NguyenLieu.DonViTinh);

        context.CongThucMon.Remove(congThuc);
        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.DeleteRecipe,
            entity: "CongThucMon",
            entityId: TaoCongThucEntityId(monId, nguyenLieuId),
            description: $"Đã xóa nguyên liệu {congThuc.NguyenLieu.TenNguyenLieu} khỏi công thức món {congThuc.Mon.TenMon}.",
            oldValue: oldSnapshot,
            newValue: new
            {
                Deleted = true,
                congThuc.MonID,
                congThuc.NguyenLieuID
            },
            performedBy: nguoiDung?.TenDangNhap);

        return (true, "Đã xóa nguyên liệu khỏi công thức.");
    }

    private static string TaoCongThucEntityId(int monId, int nguyenLieuId)
    {
        return $"{monId}:{nguyenLieuId}";
    }

    private static object TaoCongThucSnapshot(
        int monId,
        string tenMon,
        int nguyenLieuId,
        string tenNguyenLieu,
        decimal soLuong,
        string donViTinh)
    {
        return new
        {
            MonId = monId,
            TenMon = tenMon,
            NguyenLieuId = nguyenLieuId,
            TenNguyenLieu = tenNguyenLieu,
            SoLuong = soLuong,
            DonViTinh = donViTinh
        };
    }
}
