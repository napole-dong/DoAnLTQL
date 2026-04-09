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
                TenLoai = x.TenLoai,
                MoTa = x.MoTa ?? string.Empty
            })
            .ToList();
    }

    public List<MonDTO> GetDanhSachMon(string? tuKhoa, int? loaiMonId = null)
    {
        using var context = new CaPheDbContext();

        var monRows = QueryDanhSachMonRows(context, tuKhoa, loaiMonId);
        return MapMonDtos(monRows);
    }

    public int GetNextMonId()
    {
        using var context = new CaPheDbContext();
        return (context.Mon.Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public MonDTO? GetMonById(int id)
    {
        using var context = new CaPheDbContext();

        var monRow = QueryMonByIdRow(context, id);
        return monRow == null ? null : MapMonDto(monRow);
    }

    public MonDTO ThemMon(MonDTO monDTO)
    {
        using var context = new CaPheDbContext();
        var mon = new dtaMon
        {
            TenMon = monDTO.TenMon,
            LoaiMonID = monDTO.LoaiMonID,
            DonGia = monDTO.DonGia,
            TrangThai = monDTO.TrangThai,
            TrangThaiTextLegacy = monDTO.TrangThaiHienThi,
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
        mon.TrangThai = monDTO.TrangThai;
        mon.TrangThaiTextLegacy = monDTO.TrangThaiHienThi;
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

    public MonImportResultDTO NhapDanhSachMon(IEnumerable<MonDTO> dsMonNhap, bool choPhepThemMoi, bool choPhepCapNhat)
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
                if (!choPhepThemMoi)
                {
                    result.SoBoQua++;
                    continue;
                }

                context.Mon.Add(new dtaMon
                {
                    TenMon = monNhap.TenMon,
                    LoaiMonID = monNhap.LoaiMonID,
                    DonGia = monNhap.DonGia,
                    TrangThai = monNhap.TrangThai,
                    TrangThaiTextLegacy = monNhap.TrangThaiHienThi,
                    MoTa = string.IsNullOrWhiteSpace(monNhap.MoTa) ? null : monNhap.MoTa,
                    HinhAnh = string.IsNullOrWhiteSpace(monNhap.HinhAnh) ? null : monNhap.HinhAnh
                });
                result.SoThemMoi++;
            }
            else
            {
                if (!choPhepCapNhat)
                {
                    result.SoBoQua++;
                    continue;
                }

                mon.DonGia = monNhap.DonGia;
                mon.TrangThai = monNhap.TrangThai;
                mon.TrangThaiTextLegacy = monNhap.TrangThaiHienThi;
                mon.MoTa = string.IsNullOrWhiteSpace(monNhap.MoTa) ? null : monNhap.MoTa;
                mon.HinhAnh = string.IsNullOrWhiteSpace(monNhap.HinhAnh) ? null : monNhap.HinhAnh;
                result.SoCapNhat++;
            }
        }

        context.SaveChanges();
        return result;
    }

    private static List<MonReadModel> QueryDanhSachMonRows(CaPheDbContext context, string? tuKhoa, int? loaiMonId)
    {
        var query = context.Mon
            .AsNoTracking()
            .AsQueryable();

        if (loaiMonId.HasValue)
        {
            query = query.Where(x => x.LoaiMonID == loaiMonId.Value);
        }

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            var keywordPattern = $"%{keyword}%";
            var hasKeywordId = int.TryParse(keyword, out var keywordId);

            var matchDangKinhDoanh = "Đang kinh doanh".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchNgungBan = "Ngừng bán".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchTamNgung = "Tạm ngừng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchKhongXacDinh = "Không xác định".Contains(keyword, StringComparison.OrdinalIgnoreCase);

            query = query.Where(x =>
                (hasKeywordId && x.ID == keywordId)
                || EF.Functions.Like(x.TenMon, keywordPattern)
                || EF.Functions.Like(x.LoaiMon.TenLoai, keywordPattern)
                || EF.Functions.Like(x.MoTa ?? string.Empty, keywordPattern)
                || (matchDangKinhDoanh && x.TrangThai == 1)
                || (matchNgungBan && x.TrangThai == 0)
                || (matchTamNgung && x.TrangThai == 2)
                || (matchKhongXacDinh && x.TrangThai != 0 && x.TrangThai != 1 && x.TrangThai != 2));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new MonReadModel
            {
                ID = x.ID,
                TenMon = x.TenMon,
                LoaiMonID = x.LoaiMonID,
                TenLoaiMon = x.LoaiMon.TenLoai,
                DonGia = x.DonGia,
                TrangThai = x.TrangThai,
                MoTa = x.MoTa ?? string.Empty,
                HinhAnh = x.HinhAnh
            })
            .ToList();
    }

    private static MonReadModel? QueryMonByIdRow(CaPheDbContext context, int id)
    {
        return context.Mon
            .AsNoTracking()
            .Where(x => x.ID == id)
            .Select(x => new MonReadModel
            {
                ID = x.ID,
                TenMon = x.TenMon,
                LoaiMonID = x.LoaiMonID,
                TenLoaiMon = x.LoaiMon.TenLoai,
                DonGia = x.DonGia,
                TrangThai = x.TrangThai,
                MoTa = x.MoTa ?? string.Empty,
                HinhAnh = x.HinhAnh
            })
            .FirstOrDefault();
    }

    private static List<MonDTO> MapMonDtos(IEnumerable<MonReadModel> monRows)
    {
        return monRows
            .Select(MapMonDto)
            .ToList();
    }

    private static MonDTO MapMonDto(MonReadModel monRow)
    {
        return new MonDTO
        {
            ID = monRow.ID,
            TenMon = monRow.TenMon,
            LoaiMonID = monRow.LoaiMonID,
            TenLoaiMon = monRow.TenLoaiMon,
            DonGia = monRow.DonGia,
            TrangThai = monRow.TrangThai,
            MoTa = monRow.MoTa,
            HinhAnh = monRow.HinhAnh
        };
    }

    private sealed class MonReadModel
    {
        public int ID { get; init; }
        public string TenMon { get; init; } = string.Empty;
        public int LoaiMonID { get; init; }
        public string TenLoaiMon { get; init; } = string.Empty;
        public decimal DonGia { get; init; }
        public int TrangThai { get; init; }
        public string MoTa { get; init; } = string.Empty;
        public string? HinhAnh { get; init; }
    }
}
