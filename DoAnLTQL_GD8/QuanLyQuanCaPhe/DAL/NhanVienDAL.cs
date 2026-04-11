using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Diagnostics;
using QuanLyQuanCaPhe.Services.SoftDelete;
using System.Globalization;
using System.Text.Json;

namespace QuanLyQuanCaPhe.DAL;

public class NhanVienDAL
{
    private readonly ISoftDeleteService _softDeleteService = new SoftDeleteService();

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

    public async Task<List<NhanVienDTO>> GetDanhSachNhanVienAsync(string? tuKhoa, bool includeDeleted = false)
    {
        await using var context = new CaPheDbContext();

        var nhanVienRows = await QueryDanhSachNhanVienRowsAsync(context, tuKhoa, includeDeleted).ConfigureAwait(false);
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
        using var strategyContext = new CaPheDbContext();

        AppLogger.Info($"Start ThemNhanVien. TenDangNhap={nhanVienDTO.TenDangNhap}.", nameof(NhanVienDAL));

        var strategy = strategyContext.Database.CreateExecutionStrategy();

        try
        {
            return await strategy.ExecuteAsync(async () =>
            {
                await using var context = new CaPheDbContext();
                await using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

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
                await transaction.CommitAsync().ConfigureAwait(false);

                nhanVienDTO.ID = nhanVien.ID;
                return nhanVienDTO;
            }).ConfigureAwait(false);
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
        using var strategyContext = new CaPheDbContext();
        var strategy = strategyContext.Database.CreateExecutionStrategy();

        try
        {
            return strategy.Execute(() =>
            {
                using var context = new CaPheDbContext();
                using var transaction = context.Database.BeginTransaction();

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

                    GhiNhanVienAuditLog(context, "SoftDeleteNhanVien", nhanVien.ID, oldSnapshot, new
                    {
                        Mode = "SoftDelete",
                        IsDeleted = true,
                        AccountStatus = user == null ? "NoAccount" : "Disabled"
                    });

                    _softDeleteService.SoftDelete(context, nhanVien);
                    transaction.Commit();
                    return new DeleteNhanVienResult(DeleteNhanVienOutcome.SuccessSoftDelete);
                }

                GhiNhanVienAuditLog(context, "HardDeleteNhanVien", nhanVien.ID, oldSnapshot, new
                {
                    Mode = "HardDelete",
                    CascadeDeleteUser = true
                });

                if (!nhanVien.IsDeleted)
                {
                    nhanVien.IsDeleted = true;
                }

                context.Entry(nhanVien).State = EntityState.Deleted;
                context.SaveChanges();

                transaction.Commit();
                return new DeleteNhanVienResult(DeleteNhanVienOutcome.SuccessHardDelete);
            });
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

        GhiNhanVienAuditLog(context, "RestoreNhanVien", nhanVien.ID, oldSnapshot, new
        {
            Mode = "Restore",
            IsDeleted = false,
            AccountStatus = nhanVien.User == null ? "NoAccount" : "Enabled"
        });

        _softDeleteService.Restore(context, nhanVien);
        return true;
    }

    public bool HardDeleteNhanVien(int nhanVienId)
    {
        var result = DeleteNhanVien(nhanVienId, softDelete: false);
        return result.Outcome == DeleteNhanVienOutcome.SuccessHardDelete;
    }

    public async Task<(int SoThemMoi, int SoCapNhat, int SoBoQua)> NhapDanhSachNhanVienAsync(
        IEnumerable<NhanVienDTO> dsNhap,
        bool choPhepThemMoi,
        bool choPhepCapNhat)
    {
        using var correlationScope = CorrelationContext.BeginScope();
        using var strategyContext = new CaPheDbContext();

        AppLogger.Info("Start NhapDanhSachNhanVien.", nameof(NhanVienDAL));

        var strategy = strategyContext.Database.CreateExecutionStrategy();

        try
        {
            return await strategy.ExecuteAsync(async () =>
            {
                await using var context = new CaPheDbContext();
                await using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

                var soThemMoi = 0;
                var soCapNhat = 0;
                var soBoQua = 0;

                foreach (var nhanVienNhap in dsNhap)
                {
                    if (string.IsNullOrWhiteSpace(nhanVienNhap.HoVaTen)
                        || string.IsNullOrWhiteSpace(nhanVienNhap.TenDangNhap)
                        || string.IsNullOrWhiteSpace(nhanVienNhap.QuyenHan))
                    {
                        soBoQua++;
                        continue;
                    }

                    var user = await context.User
                        .Include(x => x.NhanVien)
                        .FirstOrDefaultAsync(x => x.TenDangNhap == nhanVienNhap.TenDangNhap)
                        .ConfigureAwait(false);

                    if (user == null && !choPhepThemMoi)
                    {
                        soBoQua++;
                        continue;
                    }

                    if (user != null && !choPhepCapNhat)
                    {
                        soBoQua++;
                        continue;
                    }

                    var vaiTroId = await LayVaiTroIdAsync(context, nhanVienNhap.QuyenHan).ConfigureAwait(false);
                    if (vaiTroId <= 0)
                    {
                        soBoQua++;
                        continue;
                    }

                    if (user == null)
                    {
                        var matKhauKhoiTao = LayMatKhauTuyChon(nhanVienNhap.MatKhau);
                        if (matKhauKhoiTao == null)
                        {
                            soBoQua++;
                            continue;
                        }

                        var nhanVienMoi = new dtaNhanVien
                        {
                            HoVaTen = nhanVienNhap.HoVaTen,
                            DienThoai = string.IsNullOrWhiteSpace(nhanVienNhap.DienThoai) ? null : nhanVienNhap.DienThoai,
                            DiaChi = string.IsNullOrWhiteSpace(nhanVienNhap.DiaChi) ? null : nhanVienNhap.DiaChi
                        };

                        context.NhanVien.Add(nhanVienMoi);
                        await context.SaveChangesAsync().ConfigureAwait(false);

                        context.User.Add(new dtaUser
                        {
                            NhanVienID = nhanVienMoi.ID,
                            TenDangNhap = nhanVienNhap.TenDangNhap,
                            MatKhau = MatKhauService.BamMatKhauNeuCan(matKhauKhoiTao),
                            VaiTroID = vaiTroId,
                            HoatDong = true
                        });

                        soThemMoi++;
                    }
                    else
                    {
                        user.NhanVien.HoVaTen = nhanVienNhap.HoVaTen;
                        user.NhanVien.DienThoai = string.IsNullOrWhiteSpace(nhanVienNhap.DienThoai) ? null : nhanVienNhap.DienThoai;
                        user.NhanVien.DiaChi = string.IsNullOrWhiteSpace(nhanVienNhap.DiaChi) ? null : nhanVienNhap.DiaChi;
                        user.VaiTroID = vaiTroId;

                        var matKhauMoi = LayMatKhauTuyChon(nhanVienNhap.MatKhau);
                        if (matKhauMoi != null)
                        {
                            user.MatKhau = MatKhauService.BamMatKhauNeuCan(matKhauMoi);
                        }

                        soCapNhat++;
                    }
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);

                return (soThemMoi, soCapNhat, soBoQua);
            }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            AppLogger.Error(ex, "Unexpected failure in NhapDanhSachNhanVien.", nameof(NhanVienDAL));
            throw;
        }
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

    private static void GhiNhanVienAuditLog(
        CaPheDbContext context,
        string action,
        int nhanVienId,
        object oldValue,
        object? newValue)
    {
        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        var performedBy = string.IsNullOrWhiteSpace(nguoiDung?.TenDangNhap)
            ? "system"
            : nguoiDung!.TenDangNhap;

        context.AuditLog.Add(new dtaAuditLog
        {
            Action = action,
            EntityName = "NhanVien",
            EntityId = nhanVienId.ToString(CultureInfo.InvariantCulture),
            OldValue = JsonSerializer.Serialize(oldValue),
            NewValue = newValue == null ? null : JsonSerializer.Serialize(newValue),
            PerformedBy = performedBy,
            CreatedAt = DateTime.Now
        });
    }

    private static async Task<List<NhanVienReadModel>> QueryDanhSachNhanVienRowsAsync(CaPheDbContext context, string? tuKhoa, bool includeDeleted)
    {
        var queryBase = includeDeleted
            ? context.NhanVien.IgnoreQueryFilters()
            : context.NhanVien;

        var query = queryBase
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
