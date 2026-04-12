using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class HoaDonBUS : IHoaDonService
{
    private readonly IHoaDonRepository _hoaDonDAL;
    private readonly IOrderService _orderService;
    private readonly IPermissionService _permissionBUS;

    public HoaDonBUS(
        IHoaDonRepository? hoaDonDAL = null,
        IOrderService? orderService = null,
        IPermissionService? permissionBUS = null)
    {
        _hoaDonDAL = hoaDonDAL ?? new HoaDonDAL();
        _orderService = orderService ?? new OrderService();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public List<HoaDonDTO> LayDanhSachHoaDon(HoaDonFilterDTO boLoc)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View))
        {
            return new List<HoaDonDTO>();
        }

        var boLocDaChuanHoa = ChuanHoaBoLoc(boLoc);
        var dsHoaDon = _hoaDonDAL.GetDanhSachHoaDon(boLocDaChuanHoa);

        GanTrangThaiHoaDon(dsHoaDon);

        return dsHoaDon;
    }

    public HoaDonDTO? LayHoaDonTheoId(int hoaDonId)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View))
        {
            return null;
        }

        if (hoaDonId <= 0)
        {
            return null;
        }

        var hoaDon = _hoaDonDAL.GetHoaDonTheoId(hoaDonId);
        if (hoaDon == null)
        {
            return null;
        }

        GanTrangThaiHoaDon(hoaDon);
        return hoaDon;
    }

    public int LayMaHoaDonTiepTheo()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Create))
        {
            return 0;
        }

        return _hoaDonDAL.GetNextHoaDonId();
    }

    public List<HoaDonBanKhachItemDTO> LayDanhSachBanKhach()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View))
        {
            return new List<HoaDonBanKhachItemDTO>();
        }

        return _hoaDonDAL.GetDanhSachBanKhach();
    }

    public List<HoaDonMonItemDTO> LayDanhSachMonDangKinhDoanh()
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.View))
        {
            return new List<HoaDonMonItemDTO>();
        }

        return _hoaDonDAL.GetDanhSachMonDangKinhDoanh();
    }

    public (BanActionResultDTO Result, int HoaDonId) ThemHoaDon(HoaDonSaveRequestDTO request)
    {
        if (!_permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Create))
        {
            return (BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền tạo hóa đơn."), 0);
        }

        if (request.BanID <= 0)
        {
            return (BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn trước khi tạo hóa đơn."), 0);
        }

        request.NgayLap = request.NgayLap == default ? DateTime.Now : request.NgayLap;
        request.TrangThai = (int)HoaDonTrangThai.ChuaThanhToan;

        var ketQua = _hoaDonDAL.ThemHoaDon(request);
        return (BusMessageCatalog.CreateActionResult(ketQua.ThanhCong, ketQua.ThongBao), ketQua.HoaDonId);
    }

    public (BanActionResultDTO Result, int HoaDonId) CreateInvoice(HoaDonSaveRequestDTO request)
    {
        if (!_permissionBUS.IsAdmin() && !_permissionBUS.IsManager() && !_permissionBUS.IsStaff())
        {
            return (BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền tạo hóa đơn."), 0);
        }

        if (!_permissionBUS.CheckPermission(ActionType.Create, Feature.Invoice))
        {
            return (BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền tạo hóa đơn."), 0);
        }

        return ThemHoaDon(request);
    }

    public BanActionResultDTO CapNhatHoaDon(HoaDonSaveRequestDTO request)
    {
        if (!CoTheChinhSuaHoaDon())
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền cập nhật hóa đơn.");
        }

        if (request.ID <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Không tìm thấy hóa đơn để cập nhật.");
        }

        if (request.BanID <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn bàn hợp lệ.");
        }

        if (request.RowVersion == null || request.RowVersion.Length == 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Dữ liệu đã bị thay đổi bởi nhân viên khác. Vui lòng tải lại!");
        }

        request.NgayLap = request.NgayLap == default ? DateTime.Now : request.NgayLap;
        return BusMessageCatalog.NormalizeActionResult(_hoaDonDAL.CapNhatHoaDon(request));
    }

    public BanActionResultDTO ThemMonVaoHoaDon(int hoaDonId, int monId, short soLuong, byte[]? rowVersion = null)
    {
        if (!CoTheChinhSuaHoaDon())
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền chỉnh sửa món trong hóa đơn.");
        }

        if (hoaDonId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn trước khi thêm món.");
        }

        if (monId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn món hợp lệ.");
        }

        if (soLuong <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Số lượng món phải lớn hơn 0.");
        }

        return BusMessageCatalog.NormalizeActionResult(_orderService.AddItemToOrder(hoaDonId, monId, soLuong, rowVersion));
    }

    public BanActionResultDTO CapNhatSoLuongMonTrongHoaDon(int hoaDonId, int monId, short soLuong, byte[]? rowVersion = null)
    {
        if (!CoTheChinhSuaHoaDon())
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền chỉnh sửa món trong hóa đơn.");
        }

        if (hoaDonId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn trước khi thêm món.");
        }

        if (monId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn món hợp lệ.");
        }

        if (soLuong <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Số lượng món phải lớn hơn 0.");
        }

        return BusMessageCatalog.NormalizeActionResult(_orderService.UpdateItemQuantity(hoaDonId, monId, soLuong, rowVersion));
    }

    public BanActionResultDTO HuyHoaDon(int hoaDonId, byte[]? rowVersion = null)
    {
        if (!_permissionBUS.CanDeleteInvoice())
        {
            return BusMessageCatalog.CreateActionResult(false, "Chỉ Admin mới được hủy hoặc xóa hóa đơn.");
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Delete))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền hủy hóa đơn.");
        }

        if (hoaDonId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn cần hủy.");
        }

        return BusMessageCatalog.NormalizeActionResult(_orderService.CancelOrder(hoaDonId, rowVersion));
    }

    public BanActionResultDTO HuyHoaDon(int hoaDonId, string reason, string user, byte[]? rowVersion = null)
    {
        if (!_permissionBUS.CanDeleteInvoice())
        {
            return BusMessageCatalog.CreateActionResult(false, "Chỉ Admin mới được hủy hoặc xóa hóa đơn.");
        }

        if (!_permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Delete))
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền hủy hóa đơn.");
        }

        if (hoaDonId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn cần hủy.");
        }

        return BusMessageCatalog.NormalizeActionResult(_hoaDonDAL.HuyHoaDon(hoaDonId, reason, user, rowVersion));
    }

    public BanActionResultDTO XacNhanThuTien(int hoaDonId, decimal tienKhachDua, byte[]? rowVersion = null)
    {
        var coQuyenThuTien = _permissionBUS.CheckPermission(PermissionFeatures.HoaDon, PermissionActions.Update)
            || _permissionBUS.CheckPermission(PermissionFeatures.BanHang, PermissionActions.Update);

        if (!coQuyenThuTien)
        {
            return BusMessageCatalog.CreateActionResult(false, "Bạn không có quyền xác nhận thu tiền.");
        }

        if (hoaDonId <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Vui lòng chọn hóa đơn trước khi thu tiền.");
        }

        var hoaDon = LayHoaDonTheoId(hoaDonId);
        if (hoaDon == null)
        {
            return BusMessageCatalog.CreateActionResult(false, "Không tìm thấy hóa đơn cần thu tiền.");
        }

        if (hoaDon.TrangThai != 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Hóa đơn này không còn ở trạng thái chờ thanh toán.");
        }

        if (hoaDon.TongTien <= 0)
        {
            return BusMessageCatalog.CreateActionResult(false, "Hóa đơn chưa có món, không thể xác nhận thu tiền.");
        }

        if (tienKhachDua < hoaDon.TongTien)
        {
            return BusMessageCatalog.CreateActionResult(false, "Tiền khách đưa chưa đủ để thanh toán hóa đơn.");
        }

        return BusMessageCatalog.NormalizeActionResult(_orderService.Checkout(hoaDonId, rowVersion));
    }

    public decimal TinhTienThoi(decimal tongTien, decimal tienKhachDua)
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
            "Draft" or "Chưa thanh toán" => (int)HoaDonTrangThai.Draft,
            "Paid" or "Đã thanh toán" => (int)HoaDonTrangThai.Paid,
            "Closed" or "Đã hoàn tất" => (int)HoaDonTrangThai.Closed,
            "Cancelled" or "Đã hủy" => (int)HoaDonTrangThai.Cancelled,
            _ => null
        };
    }

    public static string DinhDangMaHoaDon(int hoaDonId)
    {
        return $"HD{hoaDonId:D5}";
    }

    public static string ChuyenTrangThaiHoaDon(int trangThai)
    {
        return (HoaDonTrangThai)trangThai switch
        {
            HoaDonTrangThai.Paid => "Paid",
            HoaDonTrangThai.Closed => "Closed",
            HoaDonTrangThai.Cancelled => "Cancelled",
            _ => "Draft"
        };
    }

    private static bool CoTheChinhSuaHoaDon()
    {
        return NguoiDungHienTaiService.LayNguoiDungDangNhap() != null;
    }

    private static HoaDonFilterDTO ChuanHoaBoLoc(HoaDonFilterDTO boLoc)
    {
        var ketQua = new HoaDonFilterDTO
        {
            TuKhoa = BusInputHelper.NormalizeNullableText(boLoc.TuKhoa),
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

    private static void GanTrangThaiHoaDon(IEnumerable<HoaDonDTO> dsHoaDon)
    {
        foreach (var hoaDon in dsHoaDon)
        {
            GanTrangThaiHoaDon(hoaDon);
        }
    }

    private static void GanTrangThaiHoaDon(HoaDonDTO hoaDon)
    {
        hoaDon.TrangThaiText = ChuyenTrangThaiHoaDon(hoaDon.TrangThai);
    }
}
