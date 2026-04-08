using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class KhachHangDAL
{
    public List<KhachHangDTO> GetDanhSachKhach(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var query = context.KhachHang
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            query = query.Where(x =>
                x.ID.ToString().Contains(tuKhoa)
                || x.HoVaTen.Contains(tuKhoa)
                || (x.DienThoai ?? string.Empty).Contains(tuKhoa)
                || (x.DiaChi ?? string.Empty).Contains(tuKhoa));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new KhachHangDTO
            {
                ID = x.ID,
                HoVaTen = x.HoVaTen,
                DienThoai = x.DienThoai,
                DiaChi = x.DiaChi
            })
            .ToList();
    }

    public int GetNextKhachHangId()
    {
        using var context = new CaPheDbContext();
        return (context.KhachHang.Max(x => (int?)x.ID) ?? 0) + 1;
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

        khach.HoVaTen = khachDTO.HoVaTen;
        khach.DienThoai = string.IsNullOrWhiteSpace(khachDTO.DienThoai) ? null : khachDTO.DienThoai;
        khach.DiaChi = string.IsNullOrWhiteSpace(khachDTO.DiaChi) ? null : khachDTO.DiaChi;
        context.SaveChanges();

        return true;
    }

    public bool KhachDaPhatSinhHoaDon(int khachId)
    {
        using var context = new CaPheDbContext();
        return context.HoaDon.Any(x => x.KhachHangID == khachId);
    }

    public bool XoaKhach(int khachId)
    {
        using var context = new CaPheDbContext();
        var khach = context.KhachHang.FirstOrDefault(x => x.ID == khachId);
        if (khach == null)
        {
            return false;
        }

        context.KhachHang.Remove(khach);
        context.SaveChanges();
        return true;
    }

    public (int SoThemMoi, int SoCapNhat, int SoBoQua) NhapDanhSachKhach(IEnumerable<KhachHangDTO> dsKhachNhap)
    {
        using var context = new CaPheDbContext();
        var soThemMoi = 0;
        var soCapNhat = 0;
        var soBoQua = 0;

        foreach (var khachNhap in dsKhachNhap)
        {
            if (string.IsNullOrWhiteSpace(khachNhap.HoVaTen))
            {
                soBoQua++;
                continue;
            }

            dtaKhachHang? khach = null;
            if (!string.IsNullOrWhiteSpace(khachNhap.DienThoai))
            {
                khach = context.KhachHang.FirstOrDefault(x => x.DienThoai == khachNhap.DienThoai);
            }

            if (khach == null)
            {
                context.KhachHang.Add(new dtaKhachHang
                {
                    HoVaTen = khachNhap.HoVaTen,
                    DienThoai = string.IsNullOrWhiteSpace(khachNhap.DienThoai) ? null : khachNhap.DienThoai,
                    DiaChi = string.IsNullOrWhiteSpace(khachNhap.DiaChi) ? null : khachNhap.DiaChi
                });
                soThemMoi++;
            }
            else
            {
                khach.HoVaTen = khachNhap.HoVaTen;
                khach.DiaChi = string.IsNullOrWhiteSpace(khachNhap.DiaChi) ? null : khachNhap.DiaChi;
                soCapNhat++;
            }
        }

        context.SaveChanges();
        return (soThemMoi, soCapNhat, soBoQua);
    }
}
