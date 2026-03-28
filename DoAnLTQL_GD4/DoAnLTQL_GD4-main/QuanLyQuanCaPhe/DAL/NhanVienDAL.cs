using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.DAL;

public class NhanVienDAL
{
    public List<NhanVienDTO> GetDanhSachNhanVien(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var query = context.NhanVien
            .AsNoTracking()
            .Select(x => new NhanVienDTO
            {
                ID = x.ID,
                HoVaTen = x.HoVaTen,
                DienThoai = x.DienThoai,
                DiaChi = x.DiaChi,
                TenDangNhap = x.User != null ? x.User.TenDangNhap : string.Empty,
                QuyenHan = x.User != null && x.User.VaiTro != null
                    ? x.User.VaiTro.TenVaiTro
                    : "Nhân viên"
            });

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            query = query.Where(x =>
                x.ID.ToString().Contains(tuKhoa)
                || x.HoVaTen.Contains(tuKhoa)
                || (x.DienThoai ?? string.Empty).Contains(tuKhoa)
                || (x.DiaChi ?? string.Empty).Contains(tuKhoa)
                || x.TenDangNhap.Contains(tuKhoa)
                || x.QuyenHan.Contains(tuKhoa));
        }

        return query
            .OrderBy(x => x.ID)
            .ToList();
    }

    public int GetNextNhanVienId()
    {
        using var context = new CaPheDbContext();
        return (context.NhanVien.Max(x => (int?)x.ID) ?? 0) + 1;
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
            MatKhau = nhanVienDTO.MatKhau ?? "123456",
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
                MatKhau = string.IsNullOrWhiteSpace(nhanVienDTO.MatKhau) ? "123456" : nhanVienDTO.MatKhau,
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
                nhanVien.User.MatKhau = nhanVienDTO.MatKhau;
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
                    MatKhau = string.IsNullOrWhiteSpace(nhanVienNhap.MatKhau) ? "123456" : nhanVienNhap.MatKhau,
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
                    user.MatKhau = nhanVienNhap.MatKhau;
                }

                soCapNhat++;
            }
        }

        context.SaveChanges();
        return (soThemMoi, soCapNhat, soBoQua);
    }

    private static int GetOrCreateVaiTroId(CaPheDbContext context, string tenVaiTro)
    {
        var tenVaiTroChuan = string.IsNullOrWhiteSpace(tenVaiTro) ? "Nhân viên" : tenVaiTro.Trim();

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
}
