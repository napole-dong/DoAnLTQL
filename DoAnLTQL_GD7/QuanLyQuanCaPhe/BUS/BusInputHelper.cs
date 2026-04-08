namespace QuanLyQuanCaPhe.BUS;

internal static class BusInputHelper
{
    public static string NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }

    public static string? NormalizeNullableText(string? value)
    {
        var normalized = NormalizeText(value);
        return normalized.Length == 0 ? null : normalized;
    }

    public static string? MergeKeywords(params string?[] values)
    {
        var tokens = values
            .Select(NormalizeText)
            .Where(x => x.Length > 0)
            .ToList();

        if (tokens.Count == 0)
        {
            return null;
        }

        return string.Join(" ", tokens);
    }

    public static bool IsValidPhoneNumber(string phone)
    {
        var normalized = NormalizeText(phone);
        return normalized.Length is >= 9 and <= 11 && normalized.All(char.IsDigit);
    }

    public static List<string> SplitCsvLine(string line)
    {
        var result = new List<string>();
        var current = new System.Text.StringBuilder();
        var inQuotes = false;

        foreach (var c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (c == ',' && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
                continue;
            }

            current.Append(c);
        }

        result.Add(current.ToString());
        return result;
    }
}
