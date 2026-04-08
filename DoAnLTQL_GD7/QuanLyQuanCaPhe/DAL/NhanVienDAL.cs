using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

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
        using var context = new CaPheDbContext();

        var nhanVien = new dtaNhanVien
        {
            HoVaTen = nhanVienDTO.HoVaTen,
            DienThoai = string.IsNullOrWhiteSpace(nhanVienDTO.DienThoai) ? null : nhanVienDTO.DienThoai,
            DiaChi = string.IsNullOrWhiteSpace(nhanVienDTO.DiaChi) ? null : nhanVienDTO.DiaChi
        };

        context.NhanVien.Add(nhanVien);
        context.SaveChanges();

        var vaiTroId = GetOrCreateVaiTroId(context, nhanVienDTO.QuyenHan);
        context.User.Add(new dtaUser
        {
            NhanVienID = nhanVien.ID,
            TenDangNhap = nhanVienDTO.TenDangNhap,
            MatKhau = MatKhauService.BamMatKhauNeuCan(nhanVienDTO.MatKhau ?? "123456"),
            VaiTroID = vaiTroId,
            HoatDong = true
        });

        context.SaveChanges();

        nhanVienDTO.ID = nhanVien.ID;
        return nhanVienDTO;
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

        var vaiTroId = GetOrCreateVaiTroId(context, nhanVienDTO.QuyenHan);

        if (nhanVien.User == null)
        {
            context.User.Add(new dtaUser
            {
                NhanVienID = nhanVien.ID,
                TenDangNhap = nhanVienDTO.TenDangNhap,
                MatKhau = MatKhauService.BamMatKhauNeuCan(
                    string.IsNullOrWhiteSpace(nhanVienDTO.MatKhau) ? "123456" : nhanVienDTO.MatKhau),
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

    public (int SoThemMoi, int SoCapNhat, int SoBoQua) NhapDanhSachNhanVien(IEnumerable<NhanVienDTO> dsNhap)
    {
        using var context = new CaPheDbContext();

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

            var vaiTroId = GetOrCreateVaiTroId(context, nhanVienNhap.QuyenHan);

            if (user == null)
            {
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
                    MatKhau = MatKhauService.BamMatKhauNeuCan(
                        string.IsNullOrWhiteSpace(nhanVienNhap.MatKhau) ? "123456" : nhanVienNhap.MatKhau),
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

                if (!string.IsNullOrWhiteSpace(nhanVienNhap.MatKhau))
                {
                    user.MatKhau = MatKhauService.BamMatKhauNeuCan(nhanVienNhap.MatKhau);
                }

                soCapNhat++;
            }
        }

        context.SaveChanges();
        return (soThemMoi, soCapNhat, soBoQua);
    }

    private static int GetOrCreateVaiTroId(CaPheDbContext context, string tenVaiTro)
    {
        var tenVaiTroChuan = string.IsNullOrWhiteSpace(tenVaiTro) ? "Staff" : tenVaiTro.Trim();

        var vaiTro = context.VaiTro.FirstOrDefault(x => x.TenVaiTro == tenVaiTroChuan);
        if (vaiTro != null)
        {
            return vaiTro.ID;
        }

        vaiTro = new dtaVaiTro
        {
            TenVaiTro = tenVaiTroChuan,
            MoTa = null
        };

        context.VaiTro.Add(vaiTro);
        context.SaveChanges();
        return vaiTro.ID;
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
