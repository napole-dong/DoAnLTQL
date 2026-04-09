using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

internal static class BusMessageCatalog
{
    private const string PaymentSuccessTemplate = "Thanh toán hóa đơn HD{MA_HD} thành công.";
    private const string ReceiveMoneySuccessTemplate = "Đã xác nhận thu tiền cho hóa đơn HD{MA_HD}.";

    private static readonly Dictionary<string, string> _messageByCode = new(StringComparer.OrdinalIgnoreCase);
    private static readonly Dictionary<string, string> _codeByMessage = new(StringComparer.Ordinal);
    private static readonly List<(Regex Pattern, string Code)> _dynamicPatterns = new();

    static BusMessageCatalog()
    {
        RegisterMessages(
            "Bạn không có quyền thêm bàn.",
            "Tên bàn không được để trống.",
            "Tên bàn đã tồn tại.",
            "Thêm bàn thành công.",
            "Bạn không có quyền xóa bàn.",
            "Không tìm thấy bàn cần xóa.",
            "Bàn đang được sử dụng, không thể xóa.",
            "Bàn đã phát sinh hóa đơn, không thể xóa.",
            "Xóa bàn thành công.",
            "Không thể xóa bàn.",
            "Bạn không có quyền chuyển hoặc gộp bàn.",
            "Thông tin bàn không hợp lệ.",
            "Bàn nguồn và bàn đích không được trùng nhau.",
            "Không tìm thấy bàn nguồn.",
            "Bàn nguồn chưa có hóa đơn đang phục vụ để chuyển/gộp.",
            "Không tìm thấy bàn đích.",
            "Chỉ có thể chuyển sang bàn trống.",
            "Chuyển bàn thành công.",
            "Bàn đích chưa có hóa đơn đang phục vụ để gộp.",
            "Gộp bàn thành công.",
            "Bạn không có quyền thêm món.",
            "Vui lòng chọn bàn trước khi thêm món.",
            "Món không hợp lệ.",
            "Đã thêm món vào giỏ tạm.",
            "Đã cập nhật số lượng món trong giỏ tạm.",
            "Bạn không có quyền lưu bàn.",
            "Bạn không có quyền thanh toán.",
            "Vui lòng chọn bàn trước khi thanh toán.",
            "Bạn không có quyền gọi món.",
            "Vui lòng chọn bàn trước khi gọi món.",
            "Chưa có món hợp lệ để gọi.",
            "Vui lòng chọn bàn hợp lệ.",
            "Không có món mới để lưu bàn.",
            "Không tìm thấy bàn để gọi món.",
            "Danh sách món gọi không hợp lệ.",
            "Có món không tồn tại trong hệ thống.",
            "Có món đang ngừng bán, vui lòng tải lại danh sách món.",
            "Món chưa có công thức chế biến, không thể gọi món.",
            "Công thức món tham chiếu nguyên liệu không tồn tại.",
            "Nguyên liệu không đủ tồn kho để gọi món.",
            "Không thể gọi món do xảy ra lỗi trong quá trình cập nhật tồn kho.",
            "Gọi món thành công.",
            "Không tìm thấy bàn cần thanh toán.",
            "Bàn này chưa có hóa đơn mở.",
            "Hóa đơn chưa có món, không thể thanh toán.",
            "Tên đăng nhập không được để trống.",
            "Mật khẩu không được để trống.",
            "Đăng nhập thành công.",
            "Tên đăng nhập không tồn tại.",
            "Tài khoản đang bị khóa. Vui lòng liên hệ quản trị viên.",
            "Mật khẩu không chính xác.",
            "Bạn không có quyền tạo hóa đơn.",
            "Vui lòng chọn bàn trước khi tạo hóa đơn.",
            "Bàn đang có hóa đơn chưa thanh toán.",
            "Tạo hóa đơn mới thành công.",
            "Bạn không có quyền cập nhật hóa đơn.",
            "Không tìm thấy hóa đơn để cập nhật.",
            "Vui lòng chọn bàn hợp lệ.",
            "Chỉ được sửa hóa đơn chưa thanh toán.",
            "Không tìm thấy bàn đã chọn.",
            "Bàn đã có hóa đơn mở khác.",
            "Cập nhật hóa đơn thành công.",
            "Bạn không có quyền chỉnh sửa món trong hóa đơn.",
            "Vui lòng chọn hóa đơn trước khi thêm món.",
            "Vui lòng chọn món hợp lệ.",
            "Số lượng món phải lớn hơn 0.",
            "Không tìm thấy hóa đơn để thêm món.",
            "Chỉ thêm món cho hóa đơn chưa thanh toán.",
            "Không tìm thấy món đã chọn.",
            "Món đang ngừng bán, vui lòng chọn món khác.",
            "Thêm món vào hóa đơn thành công.",
            "Bạn không có quyền hủy hóa đơn.",
            "Vui lòng chọn hóa đơn cần hủy.",
            "Không tìm thấy hóa đơn cần hủy.",
            "Hóa đơn đã thanh toán, không thể hủy.",
            "Hóa đơn đã ở trạng thái hủy.",
            "Hủy hóa đơn thành công.",
            "Bạn không có quyền xác nhận thu tiền.",
            "Vui lòng chọn hóa đơn trước khi thu tiền.",
            "Không tìm thấy hóa đơn cần thu tiền.",
            "Hóa đơn này không còn ở trạng thái chờ thanh toán.",
            "Hóa đơn chưa có món, không thể xác nhận thu tiền.",
            "Hóa đơn không ở trạng thái chờ thanh toán.",
            "Hóa đơn chưa có món, không thể thu tiền.",
            "Tiền khách đưa chưa đủ để thanh toán hóa đơn.",
            "Bạn không có quyền thêm khách hàng.",
            "Số điện thoại đã tồn tại.",
            "Thêm khách hàng thành công.",
            "Bạn không có quyền cập nhật khách hàng.",
            "Vui lòng chọn khách hàng cần cập nhật.",
            "Cập nhật khách hàng thành công.",
            "Không tìm thấy khách hàng để cập nhật.",
            "Bạn không có quyền xóa khách hàng.",
            "Vui lòng chọn khách hàng cần xóa.",
            "Khách hàng đã phát sinh hóa đơn, không thể xóa.",
            "Xóa khách hàng thành công.",
            "Không tìm thấy khách hàng để xóa.",
            "Họ và tên không được để trống.",
            "Số điện thoại không hợp lệ.",
            "Bạn không có quyền thêm loại món.",
            "Tên loại món không được để trống.",
            "Tên loại món đã tồn tại.",
            "Thêm loại món thành công.",
            "Bạn không có quyền cập nhật loại món.",
            "Vui lòng chọn loại món cần cập nhật.",
            "Cập nhật loại món thành công.",
            "Không tìm thấy loại món để cập nhật.",
            "Bạn không có quyền xóa loại món.",
            "Vui lòng chọn loại món cần xóa.",
            "Loại món đang được sử dụng, không thể xóa.",
            "Xóa loại món thành công.",
            "Không tìm thấy loại món để xóa.",
            "Bạn không có quyền chuyển món giữa các loại.",
            "Loại món chuyển không hợp lệ.",
            "Loại món đích phải khác loại món nguồn.",
            "Loại món nguồn hoặc đích không tồn tại.",
            "Đã chuyển món sang loại mới.",
            "Không thể chuyển món sang loại mới.",
            "Bạn không có quyền thêm món.",
            "Thêm món thành công.",
            "Bạn không có quyền cập nhật món.",
            "Vui lòng chọn món cần cập nhật.",
            "Cập nhật món thành công.",
            "Không tìm thấy món để cập nhật.",
            "Bạn không có quyền xóa món.",
            "Vui lòng chọn món cần xóa.",
            "Món đã phát sinh hóa đơn, không thể xóa.",
            "Xóa món thành công.",
            "Không tìm thấy món để xóa.",
            "Tên món không được để trống.",
            "Vui lòng chọn loại món.",
            "Vui lòng chọn trạng thái món.",
            "Đơn giá không hợp lệ.",
            "Món đang kinh doanh phải có đơn giá lớn hơn 0.",
            "Bạn không có quyền thêm nguyên liệu.",
            "Thêm nguyên liệu thành công.",
            "Bạn không có quyền cập nhật nguyên liệu.",
            "Vui lòng chọn nguyên liệu cần cập nhật.",
            "Cập nhật nguyên liệu thành công.",
            "Không tìm thấy nguyên liệu để cập nhật.",
            "Bạn không có quyền xóa nguyên liệu.",
            "Vui lòng chọn nguyên liệu cần xóa.",
            "Xóa nguyên liệu thành công.",
            "Không tìm thấy nguyên liệu để xóa.",
            "Tên nguyên liệu không được để trống.",
            "Vui lòng chọn đơn vị tính.",
            "Số lượng tồn không hợp lệ.",
            "Mức cảnh báo không hợp lệ.",
            "Giá nhập gần nhất không hợp lệ.",
            "Vui lòng chọn trạng thái.",
            "Bạn không có quyền thêm nhân viên.",
            "Bạn không có quyền gán vai trò này.",
            "Tên đăng nhập đã tồn tại.",
            "Thêm nhân viên thành công.",
            "Bạn không có quyền cập nhật nhân viên.",
            "Vui lòng chọn nhân viên cần cập nhật.",
            "Cập nhật nhân viên thành công.",
            "Không tìm thấy nhân viên để cập nhật.",
            "Bạn không có quyền xóa nhân viên.",
            "Vui lòng chọn nhân viên cần xóa.",
            "Nhân viên đã phát sinh hóa đơn, không thể xóa.",
            "Xóa nhân viên thành công.",
            "Không tìm thấy nhân viên để xóa.",
            "Mật khẩu không được để trống.",
            "Vui lòng chọn quyền hạn.",
            PaymentSuccessTemplate,
            ReceiveMoneySuccessTemplate);

        var paymentCode = _codeByMessage[PaymentSuccessTemplate];
        var receiveMoneyCode = _codeByMessage[ReceiveMoneySuccessTemplate];

        _dynamicPatterns.Add((new Regex(@"^Thanh toán hóa đơn HD\d{5} thành công\.$", RegexOptions.Compiled), paymentCode));
        _dynamicPatterns.Add((new Regex(@"^Đã xác nhận thu tiền cho hóa đơn HD\d{5}\.$", RegexOptions.Compiled), receiveMoneyCode));
    }

    public static BanActionResultDTO CreateActionResult(bool thanhCong, string? thongBao)
    {
        var normalizedMessage = NormalizeMessage(thongBao);
        return new BanActionResultDTO
        {
            ThanhCong = thanhCong,
            MaThongBao = GetCodeByMessage(normalizedMessage),
            ThongBao = normalizedMessage
        };
    }

    public static BanActionResultDTO NormalizeActionResult(BanActionResultDTO result)
    {
        return CreateActionResult(result.ThanhCong, result.ThongBao);
    }

    public static string GetCodeByMessage(string? thongBao)
    {
        var normalized = NormalizeRaw(thongBao);
        if (normalized.Length == 0)
        {
            return "BUS_INFO_EMPTY";
        }

        if (_codeByMessage.TryGetValue(normalized, out var code))
        {
            return code;
        }

        foreach (var (pattern, dynamicCode) in _dynamicPatterns)
        {
            if (pattern.IsMatch(normalized))
            {
                return dynamicCode;
            }
        }

        return "BUS_UNMAPPED_MESSAGE";
    }

    public static string GetMessageByCode(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return string.Empty;
        }

        return _messageByCode.TryGetValue(code, out var message)
            ? message
            : string.Empty;
    }

    public static IReadOnlyDictionary<string, string> GetCatalog()
    {
        return _messageByCode;
    }

    private static string NormalizeMessage(string? thongBao)
    {
        var normalized = NormalizeRaw(thongBao);
        if (normalized.Length == 0)
        {
            return string.Empty;
        }

        if (_codeByMessage.TryGetValue(normalized, out var code)
            && _messageByCode.TryGetValue(code, out var exactMessage))
        {
            return exactMessage;
        }

        return normalized;
    }

    private static string NormalizeRaw(string? thongBao)
    {
        return string.IsNullOrWhiteSpace(thongBao) ? string.Empty : thongBao.Trim();
    }

    private static void RegisterMessages(params string[] messages)
    {
        foreach (var message in messages)
        {
            var normalized = NormalizeRaw(message);
            if (normalized.Length == 0 || _codeByMessage.ContainsKey(normalized))
            {
                continue;
            }

            var code = BuildCode(normalized);
            var uniqueCode = code;
            var suffix = 2;
            while (_messageByCode.ContainsKey(uniqueCode))
            {
                uniqueCode = $"{code}_{suffix}";
                suffix++;
            }

            _codeByMessage[normalized] = uniqueCode;
            _messageByCode[uniqueCode] = normalized;
        }
    }

    private static string BuildCode(string message)
    {
        var noAccent = RemoveDiacritics(message).ToUpperInvariant();
        var sb = new StringBuilder();
        var previousUnderscore = false;

        foreach (var c in noAccent)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(c);
                previousUnderscore = false;
                continue;
            }

            if (!previousUnderscore)
            {
                sb.Append('_');
                previousUnderscore = true;
            }
        }

        var slug = sb.ToString().Trim('_');
        if (slug.Length == 0)
        {
            slug = "EMPTY";
        }

        return $"BUS_{slug}";
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .Replace('Đ', 'D')
            .Replace('đ', 'd');
    }
}
