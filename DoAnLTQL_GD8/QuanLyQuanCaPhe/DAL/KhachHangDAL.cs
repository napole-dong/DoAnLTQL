using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.SoftDelete;

namespace QuanLyQuanCaPhe.DAL;

public class KhachHangDAL
{
    private readonly ISoftDeleteService _softDeleteService = new SoftDeleteService();

    public List<KhachHangDTO> GetDanhSachKhach(string? tuKhoa, bool includeDeleted = false)
    {
        using var context = new CaPheDbContext();

        var khachRows = QueryDanhSachKhachRows(context, tuKhoa, includeDeleted);
        return MapKhachHangDtos(khachRows);
    }

    public int GetNextKhachHangId()
    {
        using var context = new CaPheDbContext();
        return (context.KhachHang
            .IgnoreQueryFilters()
            .Max(x => (int?)x.ID) ?? 0) + 1;
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

    public bool XoaKhach(int khachId)
    {
        using var context = new CaPheDbContext();
        var khach = context.KhachHang
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == khachId);
        if (khach == null)
        {
            return false;
        }

        return _softDeleteService.SoftDelete(context, khach);
    }

    public bool RestoreKhach(int khachId)
    {
        using var context = new CaPheDbContext();
        var khach = context.KhachHang
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == khachId);
        if (khach == null || !khach.IsDeleted)
        {
            return false;
        }

        _softDeleteService.Restore(context, khach);
        return true;
    }

    public bool HardDeleteKhach(int khachId)
    {
        using var context = new CaPheDbContext();
        var khach = context.KhachHang
            .IgnoreQueryFilters()
            .FirstOrDefault(x => x.ID == khachId);
        if (khach == null)
        {
            return false;
        }

        _softDeleteService.HardDelete(context, khach);
        return true;
    }

    public (int SoThemMoi, int SoCapNhat, int SoBoQua) NhapDanhSachKhach(
        IEnumerable<KhachHangDTO> dsKhachNhap,
        bool choPhepThemMoi,
        bool choPhepCapNhat)
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
                if (!choPhepThemMoi)
                {
                    soBoQua++;
                    continue;
                }

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
                if (!choPhepCapNhat)
                {
                    soBoQua++;
                    continue;
                }

                khach.HoVaTen = khachNhap.HoVaTen;
                khach.DiaChi = string.IsNullOrWhiteSpace(khachNhap.DiaChi) ? null : khachNhap.DiaChi;
                soCapNhat++;
            }
        }

        context.SaveChanges();
        return (soThemMoi, soCapNhat, soBoQua);
    }

    private static List<KhachHangReadModel> QueryDanhSachKhachRows(CaPheDbContext context, string? tuKhoa, bool includeDeleted)
    {
        var queryBase = includeDeleted
            ? context.KhachHang.IgnoreQueryFilters()
            : context.KhachHang;

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
                || EF.Functions.Like(x.DiaChi ?? string.Empty, keywordPattern));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new KhachHangReadModel
            {
                ID = x.ID,
                HoVaTen = x.HoVaTen,
                DienThoai = x.DienThoai,
                DiaChi = x.DiaChi,
                IsDeleted = x.IsDeleted,
                DeletedAt = x.DeletedAt,
                DeletedBy = x.DeletedBy
            })
            .ToList();
    }

    private static List<KhachHangDTO> MapKhachHangDtos(IEnumerable<KhachHangReadModel> khachRows)
    {
        return khachRows
            .Select(MapKhachHangDto)
            .ToList();
    }

    private static KhachHangDTO MapKhachHangDto(KhachHangReadModel khachRow)
    {
        return new KhachHangDTO
        {
            ID = khachRow.ID,
            HoVaTen = khachRow.HoVaTen,
            DienThoai = khachRow.DienThoai,
            DiaChi = khachRow.DiaChi,
            IsDeleted = khachRow.IsDeleted,
            DeletedAt = khachRow.DeletedAt,
            DeletedBy = khachRow.DeletedBy
        };
    }

    private sealed class KhachHangReadModel
    {
        public int ID { get; init; }
        public string HoVaTen { get; init; } = string.Empty;
        public string? DienThoai { get; init; }
        public string? DiaChi { get; init; }
        public bool IsDeleted { get; init; }
        public DateTime? DeletedAt { get; init; }
        public string? DeletedBy { get; init; }
    }
}
