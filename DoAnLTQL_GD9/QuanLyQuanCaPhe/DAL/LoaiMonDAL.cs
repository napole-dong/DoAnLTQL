using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class LoaiMonDAL : ILoaiMonRepository
{
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

        loai.TenLoai = tenLoai;
        loai.MoTa = string.IsNullOrWhiteSpace(moTa) ? null : moTa.Trim();
        context.SaveChanges();
        return true;
    }

    public bool ChuyenMonSangLoaiKhac(int loaiNguonId, int loaiDichId)
    {
        using var context = new CaPheDbContext();
        context.Mon
            .IgnoreQueryFilters()
            .Where(x => x.LoaiMonID == loaiNguonId)
            .ExecuteUpdate(setters => setters
                .SetProperty(x => x.LoaiMonID, loaiDichId));

        return true;
    }

    public bool LoaiDangSuDung(int id)
    {
        using var context = new CaPheDbContext();
        return context.Mon
            .IgnoreQueryFilters()
            .Any(x => x.LoaiMonID == id);
    }

    public bool XoaLoai(int id)
    {
        using var context = new CaPheDbContext();
        var loai = context.LoaiMon
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == id);
        if (loai == null || loai.IsDeleted)
        {
            return false;
        }

        context.Mon
            .IgnoreQueryFilters()
            .Where(x => x.LoaiMonID == id && !x.IsDeleted)
            .ExecuteUpdate(setters => setters
                .SetProperty(x => x.TrangThai, 0)
                .SetProperty(x => x.DonGia, 0m)
                .SetProperty(x => x.TrangThaiTextLegacy, "Ngừng bán"));

        context.LoaiMon.Remove(loai);
        try
        {
            context.SaveChanges();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
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
