using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class MonDAL
{
    public List<LoaiMonDTO> GetLoaiMon()
    {
        using var context = new CaPheDbContext();
        return context.LoaiMon
            .AsNoTracking()
            .OrderBy(x => x.TenLoai)
            .Select(x => new LoaiMonDTO
            {
                ID = x.ID,
                TenLoai = x.TenLoai
            })
            .ToList();
    }

    public List<MonDTO> GetDanhSachMon(string? tuKhoa)
    {
        using var context = new CaPheDbContext();
        var query = context.Mon
            .AsNoTracking()
            .Include(x => x.LoaiMon)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            query = query.Where(x =>
                x.ID.ToString().Contains(tuKhoa)
                || x.TenMon.Contains(tuKhoa)
                || x.LoaiMon.TenLoai.Contains(tuKhoa)
                || (x.MoTa ?? string.Empty).Contains(tuKhoa));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new MonDTO
            {
                ID = x.ID,
                TenMon = x.TenMon,
                LoaiMonID = x.LoaiMonID,
                TenLoaiMon = x.LoaiMon.TenLoai,
                DonGia = x.DonGia,
                MoTa = x.MoTa ?? string.Empty,
                HinhAnh = x.HinhAnh
            })
            .ToList();
    }

    public int GetNextMonId()
    {
        using var context = new CaPheDbContext();
        return (context.Mon.Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public MonDTO? GetMonById(int id)
    {
        using var context = new CaPheDbContext();
        return context.Mon
            .AsNoTracking()
            .Include(x => x.LoaiMon)
            .Where(x => x.ID == id)
            .Select(x => new MonDTO
            {
                ID = x.ID,
                TenMon = x.TenMon,
                LoaiMonID = x.LoaiMonID,
                TenLoaiMon = x.LoaiMon.TenLoai,
                DonGia = x.DonGia,
                MoTa = x.MoTa ?? string.Empty,
                HinhAnh = x.HinhAnh
            })
            .FirstOrDefault();
    }

    public MonDTO ThemMon(MonDTO monDTO)
    {
        using var context = new CaPheDbContext();
        var mon = new dtaMon
        {
            TenMon = monDTO.TenMon,
            LoaiMonID = monDTO.LoaiMonID,
            DonGia = monDTO.DonGia,
            MoTa = string.IsNullOrWhiteSpace(monDTO.MoTa) ? null : monDTO.MoTa,
            HinhAnh = string.IsNullOrWhiteSpace(monDTO.HinhAnh) ? null : monDTO.HinhAnh
        };

        context.Mon.Add(mon);
        context.SaveChanges();

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

        mon.TenMon = monDTO.TenMon;
        mon.LoaiMonID = monDTO.LoaiMonID;
        mon.DonGia = monDTO.DonGia;
        mon.MoTa = string.IsNullOrWhiteSpace(monDTO.MoTa) ? null : monDTO.MoTa;
        mon.HinhAnh = string.IsNullOrWhiteSpace(monDTO.HinhAnh) ? null : monDTO.HinhAnh;

        context.SaveChanges();
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
        var mon = context.Mon.FirstOrDefault(x => x.ID == monId);
        if (mon == null)
        {
            return false;
        }

        context.Mon.Remove(mon);
        context.SaveChanges();
        return true;
    }

    public MonImportResultDTO NhapDanhSachMon(IEnumerable<MonDTO> dsMonNhap)
    {
        using var context = new CaPheDbContext();
        var result = new MonImportResultDTO();
        var dsLoaiHopLe = context.LoaiMon.AsNoTracking().Select(x => x.ID).ToHashSet();

        foreach (var monNhap in dsMonNhap)
        {
            if (string.IsNullOrWhiteSpace(monNhap.TenMon) || monNhap.DonGia < 0 || !dsLoaiHopLe.Contains(monNhap.LoaiMonID))
            {
                result.SoBoQua++;
                continue;
            }

            var mon = context.Mon.FirstOrDefault(x => x.TenMon == monNhap.TenMon && x.LoaiMonID == monNhap.LoaiMonID);
            if (mon == null)
            {
                context.Mon.Add(new dtaMon
                {
                    TenMon = monNhap.TenMon,
                    LoaiMonID = monNhap.LoaiMonID,
                    DonGia = monNhap.DonGia,
                    MoTa = string.IsNullOrWhiteSpace(monNhap.MoTa) ? null : monNhap.MoTa,
                    HinhAnh = string.IsNullOrWhiteSpace(monNhap.HinhAnh) ? null : monNhap.HinhAnh
                });
                result.SoThemMoi++;
            }
            else
            {
                mon.DonGia = monNhap.DonGia;
                mon.MoTa = string.IsNullOrWhiteSpace(monNhap.MoTa) ? null : monNhap.MoTa;
                mon.HinhAnh = string.IsNullOrWhiteSpace(monNhap.HinhAnh) ? null : monNhap.HinhAnh;
                result.SoCapNhat++;
            }
        }

        context.SaveChanges();
        return result;
    }
}
