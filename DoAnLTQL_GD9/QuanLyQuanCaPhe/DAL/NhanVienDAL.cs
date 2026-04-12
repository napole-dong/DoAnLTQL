using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Diagnostics;
using QuanLyQuanCaPhe.Services.SoftDelete;

namespace QuanLyQuanCaPhe.DAL;

public class NhanVienDAL : INhanVienRepository
{
    private readonly ISoftDeleteService _softDeleteService = new SoftDeleteService();
    private readonly IActivityLogWriter _activityLogWriter;

    public NhanVienDAL(IActivityLogWriter? activityLogWriter = null)
    {
        _activityLogWriter = AppServiceProvider.Resolve(activityLogWriter, () => new ActivityLogService());
    }

    public enum DeleteNhanVienOutcome
    {
        SuccessHardDelete,
        SuccessSoftDelete,
        ForbiddenSelfDelete,
        ForbiddenAdminAccount,
        NotFound,
        AlreadyDeleted,
        HasInvoices,
        InvalidInput
    }

    public sealed record DeleteNhanVienResult(DeleteNhanVienOutcome Outcome)
    {
        public bool ThanhCong =>
            Outcome == DeleteNhanVienOutcome.SuccessHardDelete
            || Outcome == DeleteNhanVienOutcome.SuccessSoftDelete;
    }

    public async Task<List<NhanVienDTO>> GetDanhSachNhanVienAsync(string? tuKhoa)
    {
        await using var context = new CaPheDbContext();

        var nhanVienRows = await QueryDanhSachNhanVienRowsAsync(context, tuKhoa).ConfigureAwait(false);
        return MapNhanVienDtos(nhanVienRows);
    }

    public int GetNextNhanVienId()
    {
        using var context = new CaPheDbContext();
        return (context.NhanVien
            .IgnoreQueryFilters()
            .Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public List<string> LayDanhSachTenVaiTro()
    {
        using var context = new CaPheDbContext();
        return context.VaiTro
            .AsNoTracking()
            .OrderBy(x => x.TenVaiTro)
            .Select(x => x.TenVaiTro)
            .ToList();
    }

    public async Task<bool> TenDangNhapDaTonTaiAsync(string tenDangNhap, int? boQuaNhanVienId = null)
    {
        if (string.IsNullOrWhiteSpace(tenDangNhap))
        {
            return false;
        }

        await using var context = new CaPheDbContext();
        return await context.User.AnyAsync(x =>
            x.TenDangNhap == tenDangNhap
            && (!boQuaNhanVienId.HasValue || x.NhanVienID != boQuaNhanVienId.Value)).ConfigureAwait(false);
    }

    public async Task<NhanVienDTO> ThemNhanVienAsync(NhanVienDTO nhanVienDTO)
    {
        using var correlationScope = CorrelationContext.BeginScope();

        AppLogger.Info($"Start ThemNhanVien. TenDangNhap={nhanVienDTO.TenDangNhap}.", nameof(NhanVienDAL));

        try
        {
            var nhanVienMoi = await ExecutionStrategyTransactionRunner.ExecuteAsync(async context =>
            {
                var nhanVien = new dtaNhanVien
                {
                    HoVaTen = nhanVienDTO.HoVaTen,
                    DienThoai = string.IsNullOrWhiteSpace(nhanVienDTO.DienThoai) ? null : nhanVienDTO.DienThoai,
                    DiaChi = string.IsNullOrWhiteSpace(nhanVienDTO.DiaChi) ? null : nhanVienDTO.DiaChi
                };

                context.NhanVien.Add(nhanVien);
                await context.SaveChangesAsync().ConfigureAwait(false);

                var vaiTroId = await LayVaiTroIdAsync(context, nhanVienDTO.QuyenHan).ConfigureAwait(false);
                if (vaiTroId <= 0)
                {
                    throw new InvalidOperationException("Vai trò không hợp lệ.");
                }

                context.User.Add(new dtaUser
                {
                    NhanVienID = nhanVien.ID,
                    TenDangNhap = nhanVienDTO.TenDangNhap,
                    MatKhau = MatKhauService.BamMatKhauNeuCan(LayMatKhauBatBuoc(nhanVienDTO.MatKhau)),
                    VaiTroID = vaiTroId,
                    HoatDong = true
                });

                await context.SaveChangesAsync().ConfigureAwait(false);

                nhanVienDTO.ID = nhanVien.ID;
                return nhanVienDTO;
            }).ConfigureAwait(false);

            var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
            _activityLogWriter.Log(
                nguoiDung?.UserId,
                AuditActions.CreateUser,
                "User",
                nhanVienMoi.ID.ToString(),
                $"Đã tạo nhân viên {nhanVienMoi.HoVaTen} với tài khoản {nhanVienMoi.TenDangNhap}.",
                oldValue: null,
                newValue: new
                {
                    nhanVienMoi.ID,
                    nhanVienMoi.HoVaTen,
                    nhanVienMoi.TenDangNhap,
                    nhanVienMoi.QuyenHan,
                    nhanVienMoi.DienThoai,
                    nhanVienMoi.DiaChi
                },
                performedBy: nguoiDung?.TenDangNhap);

            return nhanVienMoi;
        }
        catch (Exception ex)
        {
            AppLogger.Error(ex, $"Unexpected failure in ThemNhanVien. TenDangNhap={nhanVienDTO.TenDangNhap}.", nameof(NhanVienDAL));
            throw;
        }
    }

    public async Task<bool> CapNhatNhanVienAsync(NhanVienDTO nhanVienDTO)
    {
        await using var context = new CaPheDbContext();

        var nhanVien = await context.NhanVien
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.ID == nhanVienDTO.ID)
            .ConfigureAwait(false);

        if (nhanVien == null)
        {
            return false;
        }

        var oldSnapshot = TaoNhanVienDeleteSnapshot(nhanVien);
        var hoTenCu = nhanVien.HoVaTen;
        var usernameCu = nhanVien.User?.TenDangNhap ?? string.Empty;
        var vaiTroCu = nhanVien.User?.VaiTro?.TenVaiTro ?? string.Empty;

        nhanVien.HoVaTen = nhanVienDTO.HoVaTen;
        nhanVien.DienThoai = string.IsNullOrWhiteSpace(nhanVienDTO.DienThoai) ? null : nhanVienDTO.DienThoai;
        nhanVien.DiaChi = string.IsNullOrWhiteSpace(nhanVienDTO.DiaChi) ? null : nhanVienDTO.DiaChi;

        var vaiTroId = await LayVaiTroIdAsync(context, nhanVienDTO.QuyenHan).ConfigureAwait(false);
        if (vaiTroId <= 0)
        {
            return false;
        }

        if (nhanVien.User == null)
        {
            var matKhauKhoiTao = LayMatKhauTuyChon(nhanVienDTO.MatKhau);
            if (matKhauKhoiTao == null)
            {
                return false;
            }

            context.User.Add(new dtaUser
            {
                NhanVienID = nhanVien.ID,
                TenDangNhap = nhanVienDTO.TenDangNhap,
                MatKhau = MatKhauService.BamMatKhauNeuCan(matKhauKhoiTao),
                VaiTroID = vaiTroId,
                HoatDong = true
            });
        }
        else
        {
            nhanVien.User.TenDangNhap = nhanVienDTO.TenDangNhap;
            nhanVien.User.VaiTroID = vaiTroId;
            nhanVien.User.HoatDong = true;

            if (!string.IsNullOrWhiteSpace(nhanVienDTO.MatKhau))
            {
                nhanVien.User.MatKhau = MatKhauService.BamMatKhauNeuCan(nhanVienDTO.MatKhau);
            }
        }

        await context.SaveChangesAsync().ConfigureAwait(false);

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        var moTa = $"Đã cập nhật nhân viên {nhanVien.HoVaTen} (ID: {nhanVien.ID}).";
        if (!string.Equals(hoTenCu, nhanVien.HoVaTen, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Họ tên: {hoTenCu} -> {nhanVien.HoVaTen}.";
        }

        if (!string.Equals(usernameCu, nhanVien.User?.TenDangNhap ?? string.Empty, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Tên đăng nhập: {usernameCu} -> {nhanVien.User?.TenDangNhap}.";
        }

        var vaiTroMoi = nhanVien.User?.VaiTro?.TenVaiTro ?? nhanVienDTO.QuyenHan;
        if (!string.Equals(vaiTroCu, vaiTroMoi, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Vai trò: {vaiTroCu} -> {vaiTroMoi}.";
        }

        _activityLogWriter.Log(
            nguoiDung?.UserId,
            AuditActions.UpdateUser,
            "User",
            nhanVien.ID.ToString(),
            moTa,
            oldValue: oldSnapshot,
            newValue: TaoNhanVienDeleteSnapshot(nhanVien),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool NhanVienDaPhatSinhHoaDon(int nhanVienId)
    {
        using var context = new CaPheDbContext();
        return context.HoaDon.Any(x => x.NhanVienID == nhanVienId);
    }

    public DeleteNhanVienResult DeleteNhanVien(int nhanVienId, bool softDelete)
    {
        if (nhanVienId <= 0)
        {
            return new DeleteNhanVienResult(DeleteNhanVienOutcome.InvalidInput);
        }

        using var correlationScope = CorrelationContext.BeginScope();

        try
        {
            object? oldSnapshotForLog = null;
            object? newSnapshotForLog = null;
            string? descriptionForLog = null;

            var result = ExecutionStrategyTransactionRunner.Execute(context =>
            {
                var nhanVien = context.NhanVien
                    .IgnoreQueryFilters()
                    .Include(x => x.User)
                    .ThenInclude(x => x!.VaiTro)
                    .FirstOrDefault(x => x.ID == nhanVienId);

                if (nhanVien == null)
                {
                    return new DeleteNhanVienResult(DeleteNhanVienOutcome.NotFound);
                }

                if (!softDelete && NhanVienCoHoaDon(context, nhanVienId))
                {
                    return new DeleteNhanVienResult(DeleteNhanVienOutcome.HasInvoices);
                }

                var user = nhanVien.User;
                var currentUserId = NguoiDungHienTaiService.LayNguoiDungDangNhap()?.UserId ?? 0;
                if (user != null && currentUserId > 0 && user.ID == currentUserId)
                {
                    return new DeleteNhanVienResult(DeleteNhanVienOutcome.ForbiddenSelfDelete);
                }

                var laTaiKhoanAdmin = IsAdminUser(user);
                var oldSnapshot = TaoNhanVienDeleteSnapshot(nhanVien);

                if (laTaiKhoanAdmin)
                {
                    return new DeleteNhanVienResult(DeleteNhanVienOutcome.ForbiddenAdminAccount);
                }

                if (softDelete)
                {
                    if (nhanVien.IsDeleted)
                    {
                        return new DeleteNhanVienResult(DeleteNhanVienOutcome.AlreadyDeleted);
                    }

                    if (user != null)
                    {
                        user.HoatDong = false;
                    }

                    _softDeleteService.SoftDelete(context, nhanVien);

                    oldSnapshotForLog = oldSnapshot;
                    newSnapshotForLog = TaoNhanVienDeleteSnapshot(nhanVien);
                    descriptionForLog = $"Đã ngừng hoạt động nhân viên {nhanVien.HoVaTen} (ID: {nhanVien.ID}) và khóa tài khoản liên kết.";

                    return new DeleteNhanVienResult(DeleteNhanVienOutcome.SuccessSoftDelete);
                }

                if (!nhanVien.IsDeleted)
                {
                    nhanVien.IsDeleted = true;
                }

                context.Entry(nhanVien).State = EntityState.Deleted;
                context.SaveChanges();

                oldSnapshotForLog = oldSnapshot;
                newSnapshotForLog = new
                {
                    Mode = "HardDelete",
                    NhanVienId = nhanVien.ID,
                    CascadeDeleteUser = true,
                    DeletedPermanently = true
                };
                descriptionForLog = $"Đã xóa vĩnh viễn nhân viên {nhanVien.HoVaTen} (ID: {nhanVien.ID}).";

                return new DeleteNhanVienResult(DeleteNhanVienOutcome.SuccessHardDelete);
            }, result => result.ThanhCong);

            if (result.ThanhCong)
            {
                var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
                _activityLogWriter.Log(
                    nguoiDung?.UserId,
                    AuditActions.DeleteUser,
                    "User",
                    nhanVienId.ToString(),
                    descriptionForLog ?? $"Đã xóa nhân viên ID {nhanVienId}.",
                    oldValue: oldSnapshotForLog,
                    newValue: newSnapshotForLog,
                    performedBy: nguoiDung?.TenDangNhap);
            }

            return result;
        }
        catch (Exception ex)
        {
            AppLogger.Error(
                ex,
                $"Unexpected failure in DeleteNhanVien. NhanVienId={nhanVienId}, SoftDelete={softDelete}.",
                nameof(NhanVienDAL));
            throw;
        }
    }

    public bool XoaNhanVien(int nhanVienId)
    {
        return DeleteNhanVien(nhanVienId, softDelete: true).ThanhCong;
    }

    public bool RestoreNhanVien(int nhanVienId)
    {
        using var context = new CaPheDbContext();

        var nhanVien = context.NhanVien
            .IgnoreQueryFilters()
            .Include(x => x.User)
            .FirstOrDefault(x => x.ID == nhanVienId);
        if (nhanVien == null || !nhanVien.IsDeleted)
        {
            return false;
        }

        var oldSnapshot = TaoNhanVienDeleteSnapshot(nhanVien);
        if (nhanVien.User != null)
        {
            nhanVien.User.HoatDong = true;
        }

        _softDeleteService.Restore(context, nhanVien);

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            nguoiDung?.UserId,
            AuditActions.UpdateUser,
            "User",
            nhanVien.ID.ToString(),
            $"Đã khôi phục nhân viên {nhanVien.HoVaTen} (ID: {nhanVien.ID}).",
            oldValue: oldSnapshot,
            newValue: TaoNhanVienDeleteSnapshot(nhanVien),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public bool HardDeleteNhanVien(int nhanVienId)
    {
        var result = DeleteNhanVien(nhanVienId, softDelete: false);
        return result.Outcome == DeleteNhanVienOutcome.SuccessHardDelete;
    }

    private static async Task<int> LayVaiTroIdAsync(CaPheDbContext context, string tenVaiTro)
    {
        var tenVaiTroChuan = string.IsNullOrWhiteSpace(tenVaiTro) ? string.Empty : tenVaiTro.Trim();
        if (tenVaiTroChuan.Length == 0)
        {
            return 0;
        }

        return await context.VaiTro
            .AsNoTracking()
            .Where(x => x.TenVaiTro == tenVaiTroChuan)
            .Select(x => x.ID)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
    }

    private static string LayMatKhauBatBuoc(string? matKhau)
    {
        var matKhauChuan = LayMatKhauTuyChon(matKhau);
        if (matKhauChuan == null)
        {
            throw new InvalidOperationException("Mat khau khong duoc de trong khi khoi tao tai khoan.");
        }

        return matKhauChuan;
    }

    private static string? LayMatKhauTuyChon(string? matKhau)
    {
        return string.IsNullOrWhiteSpace(matKhau)
            ? null
            : matKhau.Trim();
    }

    private static bool NhanVienCoHoaDon(CaPheDbContext context, int nhanVienId)
    {
        return context.HoaDon
            .AsNoTracking()
            .Any(x => x.NhanVienID == nhanVienId);
    }

    private static bool IsAdminUser(dtaUser? user)
    {
        if (user == null)
        {
            return false;
        }

        var role = RoleMapper.ParseRoleEnum(user.VaiTro?.TenVaiTro, RoleEnum.Staff);
        return role == RoleEnum.Admin;
    }

    private static object TaoNhanVienDeleteSnapshot(dtaNhanVien nhanVien)
    {
        return new
        {
            NhanVienId = nhanVien.ID,
            nhanVien.HoVaTen,
            nhanVien.IsDeleted,
            nhanVien.DeletedAt,
            nhanVien.DeletedBy,
            User = nhanVien.User == null
                ? null
                : new
                {
                    UserId = nhanVien.User.ID,
                    Username = nhanVien.User.TenDangNhap,
                    RoleId = nhanVien.User.VaiTroID,
                    RoleName = nhanVien.User.VaiTro?.TenVaiTro ?? string.Empty,
                    IsActive = nhanVien.User.HoatDong
                }
        };
    }

    private static async Task<List<NhanVienReadModel>> QueryDanhSachNhanVienRowsAsync(CaPheDbContext context, string? tuKhoa)
    {
        var query = context.NhanVien
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
                || EF.Functions.Like(x.DiaChi ?? string.Empty, keywordPattern)
                || (x.User != null && EF.Functions.Like(x.User.TenDangNhap, keywordPattern))
                || (x.User != null && x.User.VaiTro != null && EF.Functions.Like(x.User.VaiTro.TenVaiTro, keywordPattern)));
        }

        return await query
            .OrderBy(x => x.ID)
            .Select(x => new NhanVienReadModel
            {
                ID = x.ID,
                HoVaTen = x.HoVaTen,
                DienThoai = x.DienThoai,
                DiaChi = x.DiaChi,
                TenDangNhap = x.User != null ? x.User.TenDangNhap : string.Empty,
                QuyenHan = x.User != null && x.User.VaiTro != null
                    ? x.User.VaiTro.TenVaiTro
                    : "Nhân viên",
                IsDeleted = x.IsDeleted,
                DeletedAt = x.DeletedAt,
                DeletedBy = x.DeletedBy
            })
            .ToListAsync()
            .ConfigureAwait(false);
    }

    private static List<NhanVienDTO> MapNhanVienDtos(IEnumerable<NhanVienReadModel> nhanVienRows)
    {
        return nhanVienRows
            .Select(MapNhanVienDto)
            .ToList();
    }

    private static NhanVienDTO MapNhanVienDto(NhanVienReadModel nhanVienRow)
    {
        return new NhanVienDTO
        {
            ID = nhanVienRow.ID,
            HoVaTen = nhanVienRow.HoVaTen,
            DienThoai = nhanVienRow.DienThoai,
            DiaChi = nhanVienRow.DiaChi,
            TenDangNhap = nhanVienRow.TenDangNhap,
            QuyenHan = nhanVienRow.QuyenHan,
            IsDeleted = nhanVienRow.IsDeleted,
            DeletedAt = nhanVienRow.DeletedAt,
            DeletedBy = nhanVienRow.DeletedBy
        };
    }

    private sealed class NhanVienReadModel
    {
        public int ID { get; init; }
        public string HoVaTen { get; init; } = string.Empty;
        public string? DienThoai { get; init; }
        public string? DiaChi { get; init; }
        public string TenDangNhap { get; init; } = string.Empty;
        public string QuyenHan { get; init; } = string.Empty;
        public bool IsDeleted { get; init; }
        public DateTime? DeletedAt { get; init; }
        public string? DeletedBy { get; init; }
    }
}
