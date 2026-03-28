using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class LoaiMonDAL
{
    public List<LoaiMonDTO> GetDanhSachLoai(string? tuKhoa)
    {
        using var context = new CaPheDbContext();
        var query = context.LoaiMon
            .AsNoTracking()
            .Select(x => new LoaiMonDTO
            {
                ID = x.ID,
                TenLoai = x.TenLoai,
                SoMon = x.Mon.Count
            });

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            query = query.Where(x => x.ID.ToString().Contains(tuKhoa) || x.TenLoai.Contains(tuKhoa));
        }

        return query.OrderBy(x => x.ID).ToList();
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

    public LoaiMonDTO ThemLoai(string tenLoai)
    {
        using var context = new CaPheDbContext();
        var loai = new dtaLoaiMon { TenLoai = tenLoai };
        context.LoaiMon.Add(loai);
        context.SaveChanges();

        return new LoaiMonDTO
        {
            ID = loai.ID,
            TenLoai = loai.TenLoai,
            SoMon = 0
        };
    }

    public bool CapNhatLoai(int id, string tenLoai)
    {
        using var context = new CaPheDbContext();
        var loai = context.LoaiMon.FirstOrDefault(x => x.ID == id);
        if (loai == null)
        {
            return false;
        }

        loai.TenLoai = tenLoai;
        context.SaveChanges();
        return true;
    }

    public bool LoaiDangSuDung(int id)
    {
        using var context = new CaPheDbContext();
        return context.Mon.Any(x => x.LoaiMonID == id);
    }

    public bool XoaLoai(int id)
    {
        using var context = new CaPheDbContext();
        var loai = context.LoaiMon.FirstOrDefault(x => x.ID == id);
        if (loai == null)
        {
            return false;
        }

        context.LoaiMon.Remove(loai);
        context.SaveChanges();
        return true;
    }

    public LoaiMonImportResultDTO NhapLoaiMon(IEnumerable<string> dsTenLoai)
    {
        using var context = new CaPheDbContext();
        var result = new LoaiMonImportResultDTO();

        foreach (var tenLoaiRaw in dsTenLoai)
        {
            var tenLoai = tenLoaiRaw.Trim();
            if (string.IsNullOrWhiteSpace(tenLoai))
            {
                result.SoBoQua++;
                continue;
            }

            var loaiDaCo = context.LoaiMon.FirstOrDefault(x => x.TenLoai == tenLoai);
            if (loaiDaCo != null)
            {
                result.SoCapNhat++;
                continue;
            }

            context.LoaiMon.Add(new dtaLoaiMon { TenLoai = tenLoai });
            result.SoThemMoi++;
        }

        context.SaveChanges();
        return result;
    }
}
