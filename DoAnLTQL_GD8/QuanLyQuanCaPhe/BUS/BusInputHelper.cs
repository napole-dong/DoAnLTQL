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
        if (TrySplitCsvLine(line, out var columns, out var parseError))
        {
            return columns;
        }

        throw new FormatException(parseError);
    }

    public static bool TrySplitCsvLine(string line, out List<string> columns, out string parseError)
    {
        columns = new List<string>();
        parseError = string.Empty;

        if (line is null)
        {
            parseError = "Dòng CSV không hợp lệ (null).";
            return false;
        }

        var current = new System.Text.StringBuilder();
        var inQuotes = false;
        var justClosedQuote = false;

        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (inQuotes)
            {
                if (c == '"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                        continue;
                    }

                    inQuotes = false;
                    justClosedQuote = true;
                    continue;
                }

                current.Append(c);
                continue;
            }

            if (justClosedQuote)
            {
                if (c == ',')
                {
                    columns.Add(current.ToString());
                    current.Clear();
                    justClosedQuote = false;
                    continue;
                }

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                parseError = $"CSV không hợp lệ tại vị trí {i + 1}: ký tự '{c}' xuất hiện sau dấu nháy đóng.";
                return false;
            }

            if (c == ',')
            {
                columns.Add(current.ToString());
                current.Clear();
                continue;
            }

            if (c == '"')
            {
                if (current.Length == 0 || current.ToString().All(char.IsWhiteSpace))
                {
                    current.Clear();
                    inQuotes = true;
                    continue;
                }

                parseError = $"CSV không hợp lệ tại vị trí {i + 1}: dấu nháy mở nằm giữa dữ liệu không được bao quote.";
                return false;
            }

            current.Append(c);
        }

        if (inQuotes)
        {
            parseError = "CSV không hợp lệ: thiếu dấu nháy đóng cho một trường dữ liệu.";
            return false;
        }

        columns.Add(current.ToString());
        return true;
    }
}
