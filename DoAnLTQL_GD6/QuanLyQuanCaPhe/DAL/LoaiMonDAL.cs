using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class LoaiMonDAL
{
    public bool LoaiTonTai(int id)
    {
        using var context = new CaPheDbContext();
        return context.LoaiMon.Any(x => x.ID == id);
    }

    public List<LoaiMonDTO> GetDanhSachLoai(string? tuKhoa)
    {
        using var context = new CaPheDbContext();
        var query = context.LoaiMon
            .AsNoTracking()
            .Select(x => new LoaiMonDTO
            {
                ID = x.ID,
                TenLoai = x.TenLoai,
                SoMon = x.Mon.Count,
                MoTa = x.MoTa ?? string.Empty
            });

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            query = query.Where(x =>
                x.ID.ToString().Contains(tuKhoa)
                || x.TenLoai.Contains(tuKhoa)
                || x.MoTa.Contains(tuKhoa));
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
        var dsMon = context.Mon.Where(x => x.LoaiMonID == loaiNguonId).ToList();
        if (dsMon.Count == 0)
        {
            return true;
        }

        foreach (var mon in dsMon)
        {
            mon.LoaiMonID = loaiDichId;
        }

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

    public LoaiMonImportResultDTO NhapLoaiMon(IEnumerable<LoaiMonDTO> dsLoaiNhap)
    {
        using var context = new CaPheDbContext();
        var result = new LoaiMonImportResultDTO();

        foreach (var loaiNhap in dsLoaiNhap)
        {
            var tenLoai = loaiNhap.TenLoai.Trim();
            var moTa = string.IsNullOrWhiteSpace(loaiNhap.MoTa) ? null : loaiNhap.MoTa.Trim();
            if (string.IsNullOrWhiteSpace(tenLoai))
            {
                result.SoBoQua++;
                continue;
            }

            var loaiDaCo = context.LoaiMon.FirstOrDefault(x => x.TenLoai == tenLoai);
            if (loaiDaCo != null)
            {
                loaiDaCo.MoTa = moTa;
                result.SoCapNhat++;
                continue;
            }

            context.LoaiMon.Add(new dtaLoaiMon
            {
                TenLoai = tenLoai,
                MoTa = moTa
            });
            result.SoThemMoi++;
        }

        context.SaveChanges();
        return result;
    }
}
