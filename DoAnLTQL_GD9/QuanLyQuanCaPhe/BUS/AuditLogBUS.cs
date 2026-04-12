using System.Text.Json;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.BUS;

public class AuditLogBUS : IAuditLogService
{
    private static readonly IReadOnlyDictionary<string, string> MucDoTheoActionNoiBo =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["LOGIN_SUCCESS"] = "Info",
            ["LOGIN_FAILED"] = "Warning",
            ["LOGOUT"] = "Info",

            ["CREATE_USER"] = "Info",
            ["UPDATE_USER"] = "Info",
            ["DELETE_USER"] = "Warning",
            ["CREATE_PRODUCT"] = "Info",
            ["UPDATE_PRODUCT"] = "Info",
            ["DELETE_PRODUCT"] = "Warning",
            ["CREATE_INVOICE"] = "Info",
            ["UPDATE_INVOICE"] = "Info",
            ["DELETE_INVOICE"] = "Warning",
            ["ADD_ITEM"] = "Info",
            ["REMOVE_ITEM"] = "Warning",
            ["REPLACE_ITEM"] = "Warning",
            ["PAY_INVOICE"] = "Info",

            // Legacy actions
            ["ADDITEM"] = "Info",
            ["REMOVEITEM"] = "Warning",
            ["REPLACEITEM"] = "Warning",
            ["CHECKOUT"] = "Info",
            ["CANCEL"] = "Warning",
            ["CANCEL_INVOICE"] = "Warning",
            ["SOFTDELETENHANVIEN"] = "Warning",
            ["RESTORENHANVIEN"] = "Info",
            ["HARDDELETENHANVIEN"] = "Critical",
            ["DELETENHANVIEN_TRIGGER"] = "Critical"
        };

    private readonly IAuditLogRepository _auditLogDAL;
    private readonly IPermissionService _permissionBUS;

    public AuditLogBUS(IAuditLogRepository? auditLogDAL = null, IPermissionService? permissionBUS = null)
    {
        _auditLogDAL = auditLogDAL ?? new AuditLogDAL();
        _permissionBUS = permissionBUS ?? new PermissionBUS();
    }

    public List<AuditLogDTO> LayDanhSachAuditLog(AuditLogFilterDTO boLoc)
    {
        if (!CoQuyenXemAuditLog())
        {
            return new List<AuditLogDTO>();
        }

        var boLocDaChuanHoa = ChuanHoaBoLoc(boLoc);
        var mucDoCanLoc = boLocDaChuanHoa.MucDo;

        boLocDaChuanHoa.MucDo = null;

        var danhSachLog = _auditLogDAL.LayDanhSachAuditLog(boLocDaChuanHoa);
        foreach (var log in danhSachLog)
        {
            GanThongTinHienThi(log);
        }

        if (!string.IsNullOrWhiteSpace(mucDoCanLoc))
        {
            danhSachLog = danhSachLog
                .Where(x => string.Equals(x.MucDo, mucDoCanLoc, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return danhSachLog;
    }

    public List<string> LayDanhSachHanhDong()
    {
        if (!CoQuyenXemAuditLog())
        {
            return new List<string>();
        }

        return _auditLogDAL.LayDanhSachHanhDong();
    }

    public List<string> LayDanhSachBangDuLieu()
    {
        if (!CoQuyenXemAuditLog())
        {
            return new List<string>();
        }

        return _auditLogDAL.LayDanhSachBangDuLieu();
    }

    public AuditLogSummaryDTO TinhTongQuan(IEnumerable<AuditLogDTO> danhSachLog)
    {
        var dsLog = danhSachLog?.ToList() ?? new List<AuditLogDTO>();
        var homNay = DateTime.Today;

        return new AuditLogSummaryDTO
        {
            TongBanGhi = dsLog.Count,
            SuKienQuanTrong = dsLog.Count(LaSuKienQuanTrong),
            NguoiDungHoatDong = dsLog
                .Select(x => BusInputHelper.NormalizeText(x.PerformedBy))
                .Where(x => x.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count(),
            PhatSinhHomNay = dsLog.Count(x => x.CreatedAt.Date == homNay)
        };
    }

    private bool CoQuyenXemAuditLog()
    {
        return _permissionBUS.IsAdmin();
    }

    private static AuditLogFilterDTO ChuanHoaBoLoc(AuditLogFilterDTO? boLoc)
    {
        var tuNgay = boLoc?.TuNgay ?? DateTime.Today.AddDays(-7);
        var denNgay = boLoc?.DenNgay ?? DateTime.Today;

        if (tuNgay.Date > denNgay.Date)
        {
            (tuNgay, denNgay) = (denNgay, tuNgay);
        }

        return new AuditLogFilterDTO
        {
            TuNgay = tuNgay.Date,
            DenNgay = denNgay.Date,
            MucDo = ChuanHoaGiaTriBoLoc(boLoc?.MucDo),
            HanhDong = ChuanHoaGiaTriBoLoc(boLoc?.HanhDong),
            BangDuLieu = ChuanHoaGiaTriBoLoc(boLoc?.BangDuLieu),
            NguoiDung = BusInputHelper.NormalizeNullableText(boLoc?.NguoiDung),
            TuKhoa = BusInputHelper.NormalizeNullableText(boLoc?.TuKhoa),
            SoLuongToiDa = boLoc?.SoLuongToiDa is > 0 ? boLoc.SoLuongToiDa : 1000
        };
    }

    private static string? ChuanHoaGiaTriBoLoc(string? giaTri)
    {
        var giaTriDaChuanHoa = BusInputHelper.NormalizeNullableText(giaTri);
        if (giaTriDaChuanHoa == null)
        {
            return null;
        }

        if (giaTriDaChuanHoa.Equals("Tat ca", StringComparison.OrdinalIgnoreCase)
            || giaTriDaChuanHoa.Equals("Tất cả", StringComparison.OrdinalIgnoreCase)
            || giaTriDaChuanHoa.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return giaTriDaChuanHoa;
    }

    private static void GanThongTinHienThi(AuditLogDTO log)
    {
        log.Action = BusInputHelper.NormalizeText(log.Action);
        log.EntityName = BusInputHelper.NormalizeText(log.EntityName);
        log.EntityId = BusInputHelper.NormalizeText(log.EntityId);
        log.PerformedBy = BusInputHelper.NormalizeText(log.PerformedBy);

        log.MucDo = XacDinhMucDo(log.Action);
        log.ChiTietHienThi = TaoChiTietNgan(log);
        log.DiaChiIp = TrichXuatDiaChiIp(log.NewValue, log.OldValue);
    }

    private static bool LaSuKienQuanTrong(AuditLogDTO log)
    {
        return string.Equals(log.MucDo, "Warning", StringComparison.OrdinalIgnoreCase)
            || string.Equals(log.MucDo, "Error", StringComparison.OrdinalIgnoreCase)
            || string.Equals(log.MucDo, "Critical", StringComparison.OrdinalIgnoreCase);
    }

    private static string XacDinhMucDo(string action)
    {
        var actionKey = ChuanHoaActionKey(action);

        if (MucDoTheoActionNoiBo.TryGetValue(actionKey, out var mucDoTheoAction))
        {
            return mucDoTheoAction;
        }

        if (actionKey.Contains("ERROR", StringComparison.Ordinal)
            || actionKey.Contains("FAIL", StringComparison.Ordinal)
            || actionKey.Contains("EXCEPTION", StringComparison.Ordinal))
        {
            return "Error";
        }

        if (actionKey.Contains("HARDDELETE", StringComparison.Ordinal)
            || actionKey.Contains("HARD_DELETE", StringComparison.Ordinal)
            || actionKey.Contains("FORCE_DELETE", StringComparison.Ordinal))
        {
            return "Critical";
        }

        if (actionKey.Contains("SOFTDELETE", StringComparison.Ordinal)
            || actionKey.Contains("SOFT_DELETE", StringComparison.Ordinal))
        {
            return "Warning";
        }

        if (actionKey.Contains("RESTORE", StringComparison.Ordinal))
        {
            return "Info";
        }

        if (actionKey.Contains("DELETE", StringComparison.Ordinal)
            || actionKey.Contains("CANCEL", StringComparison.Ordinal)
            || actionKey.Contains("LOCK", StringComparison.Ordinal)
            || actionKey.Contains("UNLOCK", StringComparison.Ordinal))
        {
            return "Warning";
        }

        return "Info";
    }

    private static string ChuanHoaActionKey(string action)
    {
        var actionDaChuanHoa = BusInputHelper.NormalizeText(action);
        if (actionDaChuanHoa.Length == 0)
        {
            return string.Empty;
        }

        return actionDaChuanHoa
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("-", "_", StringComparison.Ordinal)
            .ToUpperInvariant();
    }

    private static string TaoChiTietNgan(AuditLogDTO log)
    {
        var moTa = TrichXuatGiaTriJson(log.NewValue, "Description")
                   ?? TrichXuatGiaTriJson(log.NewValue, "description")
                   ?? TrichXuatGiaTriJson(log.OldValue, "Description")
                   ?? TrichXuatGiaTriJson(log.OldValue, "description");

        if (!string.IsNullOrWhiteSpace(moTa))
        {
            return CatNganChuoi(moTa, 220);
        }

        var lyDo = TrichXuatGiaTriJson(log.NewValue, "Reason")
                   ?? TrichXuatGiaTriJson(log.NewValue, "reason");

        if (!string.IsNullOrWhiteSpace(lyDo))
        {
            return CatNganChuoi($"Ly do: {lyDo}", 180);
        }

        var coGiaTriCu = !string.IsNullOrWhiteSpace(log.OldValue);
        var coGiaTriMoi = !string.IsNullOrWhiteSpace(log.NewValue);

        if (coGiaTriCu && coGiaTriMoi)
        {
            return "Da ghi nhan thay doi du lieu truoc/sau.";
        }

        if (!coGiaTriCu && coGiaTriMoi)
        {
            return "Da tao moi du lieu.";
        }

        if (coGiaTriCu && !coGiaTriMoi)
        {
            return "Da xoa hoac vo hieu hoa du lieu.";
        }

        return "-";
    }

    private static string TrichXuatDiaChiIp(params string?[] nguonDuLieu)
    {
        foreach (var duLieu in nguonDuLieu)
        {
            var ip = TrichXuatGiaTriJson(duLieu, "IpAddress")
                     ?? TrichXuatGiaTriJson(duLieu, "IPAddress")
                     ?? TrichXuatGiaTriJson(duLieu, "IP")
                     ?? TrichXuatGiaTriJson(duLieu, "ip");

            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }
        }

        return "-";
    }

    private static string? TrichXuatGiaTriJson(string? json, string tenThuocTinh)
    {
        if (string.IsNullOrWhiteSpace(json) || string.IsNullOrWhiteSpace(tenThuocTinh))
        {
            return null;
        }

        try
        {
            using var taiLieu = JsonDocument.Parse(json);
            if (!taiLieu.RootElement.TryGetProperty(tenThuocTinh, out var giaTri))
            {
                return null;
            }

            return giaTri.ValueKind switch
            {
                JsonValueKind.String => giaTri.GetString(),
                JsonValueKind.Number => giaTri.GetRawText(),
                JsonValueKind.True => bool.TrueString,
                JsonValueKind.False => bool.FalseString,
                _ => giaTri.GetRawText()
            };
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string CatNganChuoi(string giaTri, int doDaiToiDa)
    {
        if (string.IsNullOrEmpty(giaTri) || doDaiToiDa <= 0 || giaTri.Length <= doDaiToiDa)
        {
            return giaTri;
        }

        return giaTri[..doDaiToiDa] + "...";
    }
}
