using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.DAL;

public class NhanVienDAL
{
    public List<NhanVienDTO> GetDanhSachNhanVien(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var nhanVienRows = QueryDanhSachNhanVienRows(context, tuKhoa);
        return MapNhanVienDtos(nhanVienRows);
    }

    public int GetNextNhanVienId()
    {
        using var context = new CaPheDbContext();
        return (context.NhanVien.Max(x => (int?)x.ID) ?? 0) + 1;
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

    public bool TenDangNhapDaTonTai(string tenDangNhap, int? boQuaNhanVienId = null)
    {
        if (string.IsNullOrWhiteSpace(tenDangNhap))
        {
            return false;
        }

        using var context = new CaPheDbContext();
        return context.User.Any(x =>
            x.TenDangNhap == tenDangNhap
            && (!boQuaNhanVienId.HasValue || x.NhanVienID != boQuaNhanVienId.Value));
    }

    public NhanVienDTO ThemNhanVien(NhanVienDTO nhanVienDTO)
    {
        using var correlationScope = CorrelationContext.BeginScope();
        using var context = new CaPheDbContext();

        AppLogger.Info($"Start ThemNhanVien. TenDangNhap={nhanVienDTO.TenDangNhap}.", nameof(NhanVienDAL));

        var strategy = context.Database.CreateExecutionStrategy();

        try
        {
            return strategy.Execute(() =>
            {
                using var transaction = context.Database.BeginTransaction();

                var nhanVien = new dtaNhanVien
                {
                    HoVaTen = nhanVienDTO.HoVaTen,
                    DienThoai = string.IsNullOrWhiteSpace(nhanVienDTO.DienThoai) ? null : nhanVienDTO.DienThoai,
                    DiaChi = string.IsNullOrWhiteSpace(nhanVienDTO.DiaChi) ? null : nhanVienDTO.DiaChi
                };

                context.NhanVien.Add(nhanVien);
                context.SaveChanges();

                var vaiTroId = LayVaiTroId(context, nhanVienDTO.QuyenHan);
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

                context.SaveChanges();
                transaction.Commit();

                nhanVienDTO.ID = nhanVien.ID;
                return nhanVienDTO;
            });
        }
        catch (Exception ex)
        {
            AppLogger.Error(ex, $"Unexpected failure in ThemNhanVien. TenDangNhap={nhanVienDTO.TenDangNhap}.", nameof(NhanVienDAL));
            throw;
        }
    }

    public bool CapNhatNhanVien(NhanVienDTO nhanVienDTO)
    {
        using var context = new CaPheDbContext();

        var nhanVien = context.NhanVien
            .Include(x => x.User)
            .FirstOrDefault(x => x.ID == nhanVienDTO.ID);

        if (nhanVien == null)
        {
            return false;
        }

        nhanVien.HoVaTen = nhanVienDTO.HoVaTen;
        nhanVien.DienThoai = string.IsNullOrWhiteSpace(nhanVienDTO.DienThoai) ? null : nhanVienDTO.DienThoai;
        nhanVien.DiaChi = string.IsNullOrWhiteSpace(nhanVienDTO.DiaChi) ? null : nhanVienDTO.DiaChi;

        var vaiTroId = LayVaiTroId(context, nhanVienDTO.QuyenHan);
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

        context.SaveChanges();
        return true;
    }

    public bool NhanVienDaPhatSinhHoaDon(int nhanVienId)
    {
        using var context = new CaPheDbContext();
        return context.HoaDon.Any(x => x.NhanVienID == nhanVienId);
    }

    public bool XoaNhanVien(int nhanVienId)
    {
        using var context = new CaPheDbContext();

        var nhanVien = context.NhanVien.FirstOrDefault(x => x.ID == nhanVienId);
        if (nhanVien == null)
        {
            return false;
        }

        context.NhanVien.Remove(nhanVien);
        context.SaveChanges();
        return true;
    }

    public (int SoThemMoi, int SoCapNhat, int SoBoQua) NhapDanhSachNhanVien(
        IEnumerable<NhanVienDTO> dsNhap,
        bool choPhepThemMoi,
        bool choPhepCapNhat)
    {
        using var correlationScope = CorrelationContext.BeginScope();
        using var context = new CaPheDbContext();

        AppLogger.Info("Start NhapDanhSachNhanVien.", nameof(NhanVienDAL));

        var strategy = context.Database.CreateExecutionStrategy();

        try
        {
            return strategy.Execute(() =>
            {
                using var transaction = context.Database.BeginTransaction();

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

                    var user = context.User
                        .Include(x => x.NhanVien)
                        .FirstOrDefault(x => x.TenDangNhap == nhanVienNhap.TenDangNhap);

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

                    var vaiTroId = LayVaiTroId(context, nhanVienNhap.QuyenHan);
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
                        context.SaveChanges();

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

                context.SaveChanges();
                transaction.Commit();

                return (soThemMoi, soCapNhat, soBoQua);
            });
        }
        catch (Exception ex)
        {
            AppLogger.Error(ex, "Unexpected failure in NhapDanhSachNhanVien.", nameof(NhanVienDAL));
            throw;
        }
    }

    private static int LayVaiTroId(CaPheDbContext context, string tenVaiTro)
    {
        var tenVaiTroChuan = string.IsNullOrWhiteSpace(tenVaiTro) ? string.Empty : tenVaiTro.Trim();
        if (tenVaiTroChuan.Length == 0)
        {
            return 0;
        }

        return context.VaiTro
            .AsNoTracking()
            .Where(x => x.TenVaiTro == tenVaiTroChuan)
            .Select(x => x.ID)
            .FirstOrDefault();
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

    private static List<NhanVienReadModel> QueryDanhSachNhanVienRows(CaPheDbContext context, string? tuKhoa)
    {
        var query = context.NhanVien
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

        return query
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
                    : "Nhân viên"
            })
            .ToList();
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
            QuyenHan = nhanVienRow.QuyenHan
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
    }
}
