using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class AuditLogDAL : IAuditLogRepository
{
    public List<AuditLogDTO> LayDanhSachAuditLog(AuditLogFilterDTO boLoc)
    {
        if (boLoc == null)
        {
            return new List<AuditLogDTO>();
        }

        var (tuNgay, denNgay) = ChuanHoaKhoangNgay(boLoc.TuNgay, boLoc.DenNgay);
        var soLuongToiDa = Math.Clamp(boLoc.SoLuongToiDa, 50, 5000);

        using var context = new CaPheDbContext();

        var query = context.AuditLog
            .AsNoTracking()
            .Where(x => x.CreatedAt >= tuNgay && x.CreatedAt <= denNgay)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(boLoc.HanhDong))
        {
            var hanhDong = boLoc.HanhDong.Trim();
            query = query.Where(x => x.Action == hanhDong);
        }

        if (!string.IsNullOrWhiteSpace(boLoc.BangDuLieu))
        {
            var bangDuLieu = boLoc.BangDuLieu.Trim();
            query = query.Where(x => x.EntityName == bangDuLieu);
        }

        if (!string.IsNullOrWhiteSpace(boLoc.NguoiDung))
        {
            var nguoiDungPattern = $"%{boLoc.NguoiDung.Trim()}%";
            query = query.Where(x => EF.Functions.Like(x.PerformedBy, nguoiDungPattern));
        }

        if (!string.IsNullOrWhiteSpace(boLoc.TuKhoa))
        {
            var tuKhoa = boLoc.TuKhoa.Trim();
            var tuKhoaPattern = $"%{tuKhoa}%";
            var coId = int.TryParse(tuKhoa, out var idBanGhi);

            query = query.Where(x =>
                (coId && x.Id == idBanGhi)
                || EF.Functions.Like(x.Action, tuKhoaPattern)
                || EF.Functions.Like(x.EntityName, tuKhoaPattern)
                || EF.Functions.Like(x.EntityId, tuKhoaPattern)
                || EF.Functions.Like(x.PerformedBy, tuKhoaPattern)
                || EF.Functions.Like(x.OldValue ?? string.Empty, tuKhoaPattern)
                || EF.Functions.Like(x.NewValue ?? string.Empty, tuKhoaPattern));
        }

        return query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(soLuongToiDa)
            .Select(x => new AuditLogDTO
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                Action = x.Action,
                EntityName = x.EntityName,
                EntityId = x.EntityId,
                OldValue = x.OldValue,
                NewValue = x.NewValue,
                PerformedBy = x.PerformedBy
            })
            .ToList();
    }

    public List<string> LayDanhSachHanhDong()
    {
        using var context = new CaPheDbContext();

        return context.AuditLog
            .AsNoTracking()
            .Select(x => x.Action)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }

    public List<string> LayDanhSachBangDuLieu()
    {
        using var context = new CaPheDbContext();

        return context.AuditLog
            .AsNoTracking()
            .Select(x => x.EntityName)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }

    private static (DateTime TuNgay, DateTime DenNgay) ChuanHoaKhoangNgay(DateTime tuNgay, DateTime denNgay)
    {
        var tuNgayDate = tuNgay == default ? DateTime.Today.AddDays(-7) : tuNgay.Date;
        var denNgayDate = denNgay == default ? DateTime.Today : denNgay.Date;

        if (tuNgayDate > denNgayDate)
        {
            (tuNgayDate, denNgayDate) = (denNgayDate, tuNgayDate);
        }

        return (tuNgayDate, denNgayDate.AddDays(1).AddTicks(-1));
    }
}
