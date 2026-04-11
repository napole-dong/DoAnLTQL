using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.DAL;

public class NguyenLieuDAL
{
    public List<NguyenLieuDTO> GetDanhSachNguyenLieu(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var nguyenLieuRows = QueryDanhSachNguyenLieuRows(context, tuKhoa);
        return MapNguyenLieuDtos(nguyenLieuRows);
    }

    public List<NguyenLieuDTO> GetDanhSachNguyenLieuSapHet()
    {
        using var context = new CaPheDbContext();

        var nguyenLieuRows = QueryDanhSachNguyenLieuSapHetRows(context);

        return MapNguyenLieuDtos(nguyenLieuRows);
    }

    public int GetNextNguyenLieuId()
    {
        using var context = new CaPheDbContext();
        return (context.Set<dtaNguyenLieu>().Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public NguyenLieuDTO ThemNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        using var context = new CaPheDbContext();

        var nguyenLieu = new dtaNguyenLieu
        {
            TenNguyenLieu = nguyenLieuDTO.TenNguyenLieu,
            DonViTinh = nguyenLieuDTO.DonViTinh,
            SoLuongTon = nguyenLieuDTO.SoLuongTon,
            MucCanhBao = nguyenLieuDTO.MucCanhBao,
            GiaNhapGanNhat = nguyenLieuDTO.GiaNhapGanNhat,
            TrangThai = nguyenLieuDTO.TrangThai,
            TrangThaiTextLegacy = nguyenLieuDTO.TrangThaiHienThi
        };

        context.Set<dtaNguyenLieu>().Add(nguyenLieu);
        context.SaveChanges();

        nguyenLieuDTO.MaNguyenLieu = nguyenLieu.ID;
        return nguyenLieuDTO;
    }

    public bool CapNhatNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        using var context = new CaPheDbContext();

        var nguyenLieu = context.Set<dtaNguyenLieu>().FirstOrDefault(x => x.ID == nguyenLieuDTO.MaNguyenLieu);
        if (nguyenLieu == null)
        {
            return false;
        }

        nguyenLieu.TenNguyenLieu = nguyenLieuDTO.TenNguyenLieu;
        nguyenLieu.DonViTinh = nguyenLieuDTO.DonViTinh;
        nguyenLieu.SoLuongTon = nguyenLieuDTO.SoLuongTon;
        nguyenLieu.MucCanhBao = nguyenLieuDTO.MucCanhBao;
        nguyenLieu.GiaNhapGanNhat = nguyenLieuDTO.GiaNhapGanNhat;
        nguyenLieu.TrangThai = nguyenLieuDTO.TrangThai;
        nguyenLieu.TrangThaiTextLegacy = nguyenLieuDTO.TrangThaiHienThi;

        context.SaveChanges();
        return true;
    }

    public bool XoaNguyenLieu(int maNguyenLieu)
    {
        using var context = new CaPheDbContext();

        var nguyenLieu = context.Set<dtaNguyenLieu>().FirstOrDefault(x => x.ID == maNguyenLieu);
        if (nguyenLieu == null)
        {
            return false;
        }

        context.Set<dtaNguyenLieu>().Remove(nguyenLieu);
        context.SaveChanges();
        return true;
    }

    public (bool ThanhCong, string ThongBao) NhapKho(int maNguyenLieu, decimal soLuongNhap, decimal giaNhap, string? ghiChu)
    {
        using var correlationScope = CorrelationContext.BeginScope();

        AppLogger.Info($"Start NhapKho. NguyenLieuID={maNguyenLieu}, SoLuongNhap={soLuongNhap}.", nameof(NguyenLieuDAL));
        AppLogger.Audit(
            "Inventory.Import.Start",
            "Bat dau nhap kho.",
            new
            {
                NguyenLieuId = maNguyenLieu,
                SoLuongNhap = soLuongNhap,
                GiaNhap = giaNhap
            },
            nameof(NguyenLieuDAL));

        using var strategyContext = new CaPheDbContext();
        var strategy = strategyContext.Database.CreateExecutionStrategy();

        try
        {
            return strategy.ExecuteAsync(async () =>
            {
                await using var context = new CaPheDbContext();
                await using var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);

                try
                {
                    var nguyenLieu = await context.NguyenLieu
                        .FirstOrDefaultAsync(x => x.ID == maNguyenLieu)
                        .ConfigureAwait(false);
                    if (nguyenLieu == null)
                    {
                        await transaction.RollbackAsync().ConfigureAwait(false);
                        AppLogger.Audit(
                            "Inventory.Import.Rejected",
                            "Khong tim thay nguyen lieu de nhap kho.",
                            new { NguyenLieuId = maNguyenLieu },
                            nameof(NguyenLieuDAL));
                        return (false, "Không tìm thấy nguyên liệu để nhập kho.");
                    }

                    var soLuongTonTruoc = nguyenLieu.SoLuongTon;

                    var phieuNhap = new dtaPhieuNhapKho
                    {
                        NguyenLieuID = maNguyenLieu,
                        SoLuongNhap = soLuongNhap,
                        GiaNhap = giaNhap,
                        NgayNhap = DateTime.Now,
                        GhiChu = string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu.Trim()
                    };

                    context.PhieuNhapKho.Add(phieuNhap);

                    nguyenLieu.SoLuongTon += soLuongNhap;
                    nguyenLieu.GiaNhapGanNhat = giaNhap;
                    nguyenLieu.TrangThai = TinhTrangThaiNguyenLieu(nguyenLieu.SoLuongTon, nguyenLieu.MucCanhBao, nguyenLieu.TrangThai);
                    nguyenLieu.TrangThaiTextLegacy = ChuyenTrangThaiNguyenLieuTextLegacy(nguyenLieu.TrangThai, nguyenLieu.SoLuongTon);

                    await context.SaveChangesAsync().ConfigureAwait(false);
                    await transaction.CommitAsync().ConfigureAwait(false);

                    AppLogger.Audit(
                        "Inventory.Import.Success",
                        "Nhap kho thanh cong.",
                        new
                        {
                            NguyenLieuId = maNguyenLieu,
                            SoLuongNhap = soLuongNhap,
                            SoLuongTonTruoc = soLuongTonTruoc,
                            SoLuongTonSau = nguyenLieu.SoLuongTon,
                            GiaNhap = giaNhap
                        },
                        nameof(NguyenLieuDAL));

                    return (true, "Nhập kho thành công.");
                }
                catch
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            var mappedError = AppExceptionMapper.Map(ex);
            AppLogger.Error(
                ex,
                $"Unexpected failure in NhapKho. NguyenLieuID={maNguyenLieu}.",
                nameof(NguyenLieuDAL),
                mappedError.Code);
            AppLogger.Audit(
                "Inventory.Import.Failed",
                "Nhap kho that bai do loi he thong.",
                new
                {
                    NguyenLieuId = maNguyenLieu,
                    SoLuongNhap = soLuongNhap,
                    GiaNhap = giaNhap
                },
                nameof(NguyenLieuDAL));

            var thongBao = AppExceptionHandler.CreateUserMessage(
                "Không thể nhập kho do xảy ra lỗi trong quá trình lưu dữ liệu kho.",
                ex);
            return (false, thongBao);
        }
    }

    private static List<NguyenLieuReadModel> QueryDanhSachNguyenLieuSapHetRows(CaPheDbContext context)
    {
        return context.Set<dtaNguyenLieu>()
            .AsNoTracking()
            .Where(x => x.SoLuongTon < x.MucCanhBao)
            .OrderBy(x => x.SoLuongTon)
            .ThenBy(x => x.TenNguyenLieu)
            .Select(x => new NguyenLieuReadModel
            {
                MaNguyenLieu = x.ID,
                TenNguyenLieu = x.TenNguyenLieu,
                DonViTinh = x.DonViTinh,
                SoLuongTon = x.SoLuongTon,
                MucCanhBao = x.MucCanhBao,
                GiaNhapGanNhat = x.GiaNhapGanNhat,
                TrangThai = x.TrangThai
            })
            .ToList();
    }

    private static List<NguyenLieuReadModel> QueryDanhSachNguyenLieuRows(CaPheDbContext context, string? tuKhoa)
    {
        var query = context.Set<dtaNguyenLieu>()
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            var keywordPattern = $"%{keyword}%";
            var hasKeywordId = int.TryParse(keyword, out var keywordId);
            var hasKeywordTrangThai = int.TryParse(keyword, out var keywordTrangThai);

            var matchNgungDung = "Ngừng dùng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchSapHet = "Sắp hết".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchHetHang = "Hết hàng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchDangSuDung = "Đang sử dụng".Contains(keyword, StringComparison.OrdinalIgnoreCase);

            query = query.Where(x =>
                (hasKeywordId && x.ID == keywordId)
                || EF.Functions.Like(x.TenNguyenLieu, keywordPattern)
                || EF.Functions.Like(x.DonViTinh, keywordPattern)
                || (hasKeywordTrangThai && x.TrangThai == keywordTrangThai)
                || (matchNgungDung && x.TrangThai == 0)
                || (matchSapHet && x.TrangThai == 2 && x.SoLuongTon > 0)
                || (matchHetHang && x.TrangThai != 0 && x.SoLuongTon <= 0)
                || (matchDangSuDung && x.TrangThai != 0 && x.TrangThai != 2 && x.SoLuongTon > 0));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new NguyenLieuReadModel
            {
                MaNguyenLieu = x.ID,
                TenNguyenLieu = x.TenNguyenLieu,
                DonViTinh = x.DonViTinh,
                SoLuongTon = x.SoLuongTon,
                MucCanhBao = x.MucCanhBao,
                GiaNhapGanNhat = x.GiaNhapGanNhat,
                TrangThai = x.TrangThai
            })
            .ToList();
    }

    private static List<NguyenLieuDTO> MapNguyenLieuDtos(IEnumerable<NguyenLieuReadModel> nguyenLieuRows)
    {
        return nguyenLieuRows
            .Select(MapNguyenLieuDto)
            .ToList();
    }

    private static NguyenLieuDTO MapNguyenLieuDto(NguyenLieuReadModel nguyenLieuRow)
    {
        return new NguyenLieuDTO
        {
            MaNguyenLieu = nguyenLieuRow.MaNguyenLieu,
            TenNguyenLieu = nguyenLieuRow.TenNguyenLieu,
            DonViTinh = nguyenLieuRow.DonViTinh,
            SoLuongTon = nguyenLieuRow.SoLuongTon,
            MucCanhBao = nguyenLieuRow.MucCanhBao,
            GiaNhapGanNhat = nguyenLieuRow.GiaNhapGanNhat,
            TrangThai = nguyenLieuRow.TrangThai
        };
    }

    private static int TinhTrangThaiNguyenLieu(decimal soLuongTon, decimal mucCanhBao, int trangThaiHienTai)
    {
        if (trangThaiHienTai == 0)
        {
            return 0;
        }

        if (soLuongTon <= 0)
        {
            return 2;
        }

        if (soLuongTon <= mucCanhBao)
        {
            return 2;
        }

        return 1;
    }

    private static string ChuyenTrangThaiNguyenLieuTextLegacy(int trangThai, decimal soLuongTon)
    {
        return trangThai switch
        {
            0 => "Ngừng dùng",
            2 => soLuongTon <= 0 ? "Hết hàng" : "Sắp hết",
            _ => soLuongTon <= 0 ? "Hết hàng" : "Đang sử dụng"
        };
    }

    private sealed class NguyenLieuReadModel
    {
        public int MaNguyenLieu { get; init; }
        public string TenNguyenLieu { get; init; } = string.Empty;
        public string DonViTinh { get; init; } = string.Empty;
        public decimal SoLuongTon { get; init; }
        public decimal MucCanhBao { get; init; }
        public decimal GiaNhapGanNhat { get; init; }
        public int TrangThai { get; init; }
    }
}
