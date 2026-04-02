using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class HoaDonBUS
{
    private readonly HoaDonDAL _hoaDonDAL = new();

    public List<HoaDonDTO> LayDanhSachHoaDon(HoaDonFilterDTO boLoc)
    {
        var boLocDaChuanHoa = ChuanHoaBoLoc(boLoc);
        var dsHoaDon = _hoaDonDAL.GetDanhSachHoaDon(boLocDaChuanHoa);

        foreach (var hoaDon in dsHoaDon)
        {
            hoaDon.TrangThaiText = ChuyenTrangThaiHoaDon(hoaDon.TrangThai);
        }

        return dsHoaDon;
    }

    public HoaDonDTO? LayHoaDonTheoId(int hoaDonId)
    {
        if (hoaDonId <= 0)
        {
            return null;
        }

        var hoaDon = _hoaDonDAL.GetHoaDonTheoId(hoaDonId);
        if (hoaDon == null)
        {
            return null;
        }

        hoaDon.TrangThaiText = ChuyenTrangThaiHoaDon(hoaDon.TrangThai);
        return hoaDon;
    }

    public int LayMaHoaDonTiepTheo()
    {
        return _hoaDonDAL.GetNextHoaDonId();
    }

    public List<HoaDonBanKhachItemDTO> LayDanhSachBanKhach()
    {
        return _hoaDonDAL.GetDanhSachBanKhach();
    }

    public List<HoaDonMonItemDTO> LayDanhSachMonDangKinhDoanh()
    {
        return _hoaDonDAL.GetDanhSachMonDangKinhDoanh();
    }

    public (BanActionResultDTO Result, int HoaDonId) ThemHoaDon(HoaDonSaveRequestDTO request)
    {
        if (request.BanID <= 0)
        {
            return (new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Vui lòng chọn bàn trước khi tạo hóa đơn."
            }, 0);
        }

        request.NgayLap = request.NgayLap == default ? DateTime.Now : request.NgayLap;
        request.TrangThai = 0;

        var ketQua = _hoaDonDAL.ThemHoaDon(request);
        return (new BanActionResultDTO
        {
            ThanhCong = ketQua.ThanhCong,
            ThongBao = ketQua.ThongBao
        }, ketQua.HoaDonId);
    }

    public BanActionResultDTO CapNhatHoaDon(HoaDonSaveRequestDTO request)
    {
        if (request.ID <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy hóa đơn để cập nhật." };
        }

        if (request.BanID <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Vui lòng chọn bàn hợp lệ." };
        }

        request.NgayLap = request.NgayLap == default ? DateTime.Now : request.NgayLap;
        return _hoaDonDAL.CapNhatHoaDon(request);
    }

    public BanActionResultDTO ThemMonVaoHoaDon(int hoaDonId, int monId, short soLuong)
    {
        if (hoaDonId <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Vui lòng chọn hóa đơn trước khi thêm món." };
        }

        if (monId <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Vui lòng chọn món hợp lệ." };
        }

        if (soLuong <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Số lượng món phải lớn hơn 0." };
        }

        return _hoaDonDAL.ThemMonVaoHoaDon(hoaDonId, monId, soLuong);
    }

    public BanActionResultDTO HuyHoaDon(int hoaDonId)
    {
        if (hoaDonId <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Vui lòng chọn hóa đơn cần hủy." };
        }

        return _hoaDonDAL.HuyHoaDon(hoaDonId);
    }

    public BanActionResultDTO XacNhanThuTien(int hoaDonId, int tienKhachDua)
    {
        if (hoaDonId <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Vui lòng chọn hóa đơn trước khi thu tiền." };
        }

        var hoaDon = LayHoaDonTheoId(hoaDonId);
        if (hoaDon == null)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Không tìm thấy hóa đơn cần thu tiền." };
        }

        if (hoaDon.TrangThai != 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Hóa đơn này không còn ở trạng thái chờ thanh toán." };
        }

        if (hoaDon.TongTien <= 0)
        {
            return new BanActionResultDTO { ThanhCong = false, ThongBao = "Hóa đơn chưa có món, không thể xác nhận thu tiền." };
        }

        if (tienKhachDua < hoaDon.TongTien)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Tiền khách đưa chưa đủ để thanh toán hóa đơn."
            };
        }

        return _hoaDonDAL.XacNhanThuTien(hoaDonId);
    }

    public int TinhTienThoi(int tongTien, int tienKhachDua)
    {
        if (tienKhachDua <= tongTien)
        {
            return 0;
        }

        return tienKhachDua - tongTien;
    }

    public static int? ChuyenTextSangTrangThaiLoc(string? trangThaiText)
    {
        if (string.IsNullOrWhiteSpace(trangThaiText) || trangThaiText == "Tất cả")
        {
            return null;
        }

        return trangThaiText switch
        {
            "Chưa thanh toán" => 0,
            "Đã thanh toán" => 1,
            "Đã hủy" => 2,
            _ => null
        };
    }

    public static string DinhDangMaHoaDon(int hoaDonId)
    {
        return $"HD{hoaDonId:D5}";
    }

    public static string ChuyenTrangThaiHoaDon(int trangThai)
    {
        return trangThai switch
        {
            1 => "Đã thanh toán",
            2 => "Đã hủy",
            _ => "Chưa thanh toán"
        };
    }

    private static HoaDonFilterDTO ChuanHoaBoLoc(HoaDonFilterDTO boLoc)
    {
        var ketQua = new HoaDonFilterDTO
        {
            TuKhoa = boLoc.TuKhoa?.Trim(),
            TuNgay = boLoc.TuNgay == default ? DateTime.Today.AddDays(-30) : boLoc.TuNgay,
            DenNgay = boLoc.DenNgay == default ? DateTime.Today : boLoc.DenNgay,
            TrangThai = boLoc.TrangThai
        };

        if (ketQua.TuNgay > ketQua.DenNgay)
        {
            (ketQua.TuNgay, ketQua.DenNgay) = (ketQua.DenNgay, ketQua.TuNgay);
        }

        return ketQua;
    }
}
