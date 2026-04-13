using System.Linq;

namespace QuanLyQuanCaPhe.Services.UI;

public static class InputValidationHelper
{
    public static bool TryGetRequiredText(string? value, out string normalizedValue)
    {
        normalizedValue = (value ?? string.Empty).Trim();
        return normalizedValue.Length > 0;
    }

    public static bool TryGetOptionalPhone(string? value, out string? normalizedPhone)
    {
        var phone = (value ?? string.Empty).Trim();
        if (phone.Length == 0)
        {
            normalizedPhone = null;
            return true;
        }

        var isValidLength = phone.Length is >= 9 and <= 11;
        if (!phone.All(char.IsDigit) || !isValidLength)
        {
            normalizedPhone = phone;
            return false;
        }

        normalizedPhone = phone;
        return true;
    }

    public static string? NormalizeOptionalText(string? value)
    {
        var normalized = (value ?? string.Empty).Trim();
        return normalized.Length == 0 ? null : normalized;
    }
}
