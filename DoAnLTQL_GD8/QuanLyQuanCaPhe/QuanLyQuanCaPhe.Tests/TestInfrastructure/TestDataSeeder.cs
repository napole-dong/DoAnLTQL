using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.Tests.TestInfrastructure;

public static class TestDataSeeder
{
    public sealed record SeededUser(int UserId, int NhanVienId, int RoleId, string Username, string RoleName);

    public static int EnsureRole(CaPheDbContext context, RoleEnum role)
    {
        var roleName = RoleMapper.ToRoleName(role);
        var existing = context.VaiTro.FirstOrDefault(x => x.TenVaiTro == roleName);
        if (existing != null)
        {
            return existing.ID;
        }

        var created = new dtaVaiTro
        {
            TenVaiTro = roleName,
            MoTa = $"Role {roleName}"
        };

        context.VaiTro.Add(created);
        context.SaveChanges();
        return created.ID;
    }

    public static void GrantPermission(
        CaPheDbContext context,
        int roleId,
        string feature,
        bool canView,
        bool canCreate,
        bool canUpdate,
        bool canDelete)
    {
        var row = context.Permission.FirstOrDefault(x => x.VaiTroID == roleId && x.Feature == feature);
        if (row == null)
        {
            row = new dtaPermission
            {
                VaiTroID = roleId,
                Feature = feature
            };

            context.Permission.Add(row);
        }

        row.CanView = canView;
        row.CanCreate = canCreate;
        row.CanUpdate = canUpdate;
        row.CanDelete = canDelete;
        context.SaveChanges();
    }

    public static void GrantFullPermissions(CaPheDbContext context, int roleId)
    {
        foreach (var feature in PermissionFeatures.TatCa)
        {
            GrantPermission(context, roleId, feature, canView: true, canCreate: true, canUpdate: true, canDelete: true);
        }
    }

    public static SeededUser CreateUser(
        CaPheDbContext context,
        string username,
        string password,
        RoleEnum role,
        bool isActive = true)
    {
        var roleId = EnsureRole(context, role);

        var employee = new dtaNhanVien
        {
            HoVaTen = $"Nhan vien {username}",
            DienThoai = "0900000000",
            DiaChi = "Dia chi test"
        };

        context.NhanVien.Add(employee);
        context.SaveChanges();

        var user = new dtaUser
        {
            NhanVienID = employee.ID,
            VaiTroID = roleId,
            TenDangNhap = username,
            MatKhau = MatKhauService.BamMatKhau(password),
            HoatDong = isActive
        };

        context.User.Add(user);
        context.SaveChanges();

        return new SeededUser(user.ID, employee.ID, roleId, username, RoleMapper.ToRoleName(role));
    }

    public static void SetCurrentUser(SeededUser user)
    {
        NguoiDungHienTaiService.DatNguoiDungDangNhap(new ThongTinDangNhapDTO
        {
            UserId = user.UserId,
            NhanVienId = user.NhanVienId,
            RoleId = user.RoleId,
            TenDangNhap = user.Username,
            HoVaTen = user.Username,
            QuyenHan = user.RoleName
        });
    }

    public static dtaBan CreateBan(CaPheDbContext context, string tenBan = "Ban 01", int trangThai = 0)
    {
        var ban = new dtaBan
        {
            TenBan = tenBan,
            TrangThai = trangThai
        };

        context.Ban.Add(ban);
        context.SaveChanges();
        return ban;
    }

    public static dtaLoaiMon CreateLoaiMon(CaPheDbContext context, string tenLoai = "Cafe")
    {
        var loai = new dtaLoaiMon
        {
            TenLoai = tenLoai,
            MoTa = "Loai mon test"
        };

        context.LoaiMon.Add(loai);
        context.SaveChanges();
        return loai;
    }

    public static dtaMon CreateMon(
        CaPheDbContext context,
        int loaiMonId,
        string tenMon = "Latte",
        decimal donGia = 20000m,
        int trangThai = 1)
    {
        var mon = new dtaMon
        {
            LoaiMonID = loaiMonId,
            TenMon = tenMon,
            DonGia = donGia,
            TrangThai = trangThai,
            TrangThaiTextLegacy = "Dang kinh doanh"
        };

        context.Mon.Add(mon);
        context.SaveChanges();
        return mon;
    }

    public static dtaNguyenLieu CreateNguyenLieu(
        CaPheDbContext context,
        string ten = "Sua",
        decimal soLuongTon = 100m,
        decimal mucCanhBao = 10m,
        decimal giaNhapGanNhat = 5000m,
        int trangThai = 1)
    {
        var nguyenLieu = new dtaNguyenLieu
        {
            TenNguyenLieu = ten,
            DonViTinh = "ml",
            SoLuongTon = soLuongTon,
            MucCanhBao = mucCanhBao,
            GiaNhapGanNhat = giaNhapGanNhat,
            TrangThai = trangThai,
            TrangThaiTextLegacy = "Dang su dung"
        };

        context.NguyenLieu.Add(nguyenLieu);
        context.SaveChanges();
        return nguyenLieu;
    }

    public static dtaCongThucMon CreateCongThuc(
        CaPheDbContext context,
        int monId,
        int nguyenLieuId,
        decimal soLuong)
    {
        var recipe = new dtaCongThucMon
        {
            MonID = monId,
            NguyenLieuID = nguyenLieuId,
            SoLuong = soLuong
        };

        context.CongThucMon.Add(recipe);
        context.SaveChanges();
        return recipe;
    }

    public static dtaHoadon CreateHoaDon(
        CaPheDbContext context,
        int banId,
        int nhanVienId,
        int trangThai = 0,
        decimal tongTien = 0m,
        int? khachHangId = null)
    {
        var invoice = new dtaHoadon
        {
            BanID = banId,
            NhanVienID = nhanVienId,
            KhachHangID = khachHangId,
            CustomerName = "Khach le",
            NgayLap = DateTime.Now,
            TrangThai = trangThai,
            TongTien = tongTien,
            GhiChuHoaDon = string.Empty
        };

        context.HoaDon.Add(invoice);
        context.SaveChanges();
        return invoice;
    }

    public static dtHoaDon_ChiTiet CreateHoaDonChiTiet(
        CaPheDbContext context,
        int hoaDonId,
        int monId,
        short soLuong,
        decimal donGia,
        string? ghiChu = null)
    {
        var line = new dtHoaDon_ChiTiet
        {
            HoaDonID = hoaDonId,
            MonID = monId,
            SoLuongBan = soLuong,
            DonGiaBan = donGia,
            ThanhTien = decimal.Round(soLuong * donGia, 2, MidpointRounding.AwayFromZero),
            GhiChu = ghiChu
        };

        context.HoaDon_ChiTiet.Add(line);
        context.SaveChanges();
        return line;
    }
}
