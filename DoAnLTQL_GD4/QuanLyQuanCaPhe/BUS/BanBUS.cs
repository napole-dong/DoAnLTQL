using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class BanBUS
{
    private readonly BanDAL _banDAL = new();

    public BanThongKeDTO LayThongKe()
    {
        return _banDAL.GetThongKe();
    }

    public List<BanDTO> LayDanhSachBan(string? khuVuc, string? trangThai, string? tuKhoa)
    {
        return _banDAL.GetDanhSachBan(khuVuc, trangThai, tuKhoa?.Trim());
    }

    public List<BanDTO> LaySoDoBan()
    {
        return _banDAL.GetSoDoBan();
    }

    public BanActionResultDTO ThemBan(string tenBan)
    {
        tenBan = tenBan.Trim();
        if (string.IsNullOrWhiteSpace(tenBan))
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Tên bàn không được để trống." };
        }

        if (_banDAL.TenBanDaTonTai(tenBan))
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Tên bàn đã tồn tại." };
        }

        _banDAL.ThemBan(tenBan);
        return new BanActionResultDTO { ThanhCong = true, ThongBao = "Thêm bàn thành công." };
    }

    public BanDTO? LayBanTheoId(int banId)
    {
        return _banDAL.GetBanById(banId);
    }

    public BanActionResultDTO XoaBan(int banId)
    {
        var ban = _banDAL.GetBanById(banId);
        if (ban == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy bàn cần xóa." };
        }

        if (ban.TrangThai != 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Bàn đang được sử dụng, không thể xóa." };
        }

        if (_banDAL.BanDaPhatSinhHoaDon(banId))
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Bàn đã phát sinh hóa đơn, không thể xóa." };
        }

        var daXoa = _banDAL.XoaBan(banId);
        return daXoa
            ? new BanActionResultDTO { ThanhCong = true, ThongBao = "Xóa bàn thành công." }
            : new BanActionResultDTO { ThanhCong = false, ThongBao = "Không thể xóa bàn." };
    }

    public List<BanDTO> LayDanhSachBanDich(int banNguonId)
    {
        return _banDAL.GetDanhSachBanDich(banNguonId);
    }

    public BanActionResultDTO ChuyenHoacGopBan(BanChuyenGopRequestDTO request)
    {
        if (request.BanNguonId <= 0 || request.BanDichId <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Thông tin bàn không hợp lệ." };
        }

        if (request.BanNguonId == request.BanDichId)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Bàn nguồn và bàn đích không được trùng nhau." };
        }

        return _banDAL.ChuyenHoacGopBan(request);
    }
}
