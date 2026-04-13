namespace QuanLyQuanCaPhe.DTO;

public enum HoaDonTrangThai
{
    Open = 0,
    Paid = 1,
    Voided = 3,

    Draft = Open,
    ChuaThanhToan = Open,
    DaThanhToan = Paid,

    Closed = 2,
    DaDong = Closed,

    Cancelled = Voided,
    DaHuy = Voided
}

public static class HoaDonStateMachine
{
    public static bool IsOpen(int trangThai)
    {
        return trangThai == (int)HoaDonTrangThai.Open;
    }

    public static bool IsPaid(int trangThai)
    {
        return trangThai == (int)HoaDonTrangThai.Paid;
    }

    public static bool IsVoided(int trangThai)
    {
        return trangThai == (int)HoaDonTrangThai.Voided
            || trangThai == (int)HoaDonTrangThai.Closed;
    }

    public static bool CanTransition(int fromStatus, int toStatus)
    {
        if (IsOpen(fromStatus))
        {
            return IsPaid(toStatus) || IsVoided(toStatus);
        }

        return false;
    }

    public static string ToDisplayText(int trangThai)
    {
        if (IsOpen(trangThai))
        {
            return "Open";
        }

        if (IsPaid(trangThai))
        {
            return "Paid";
        }

        if (IsVoided(trangThai))
        {
            return "Voided";
        }

        return "Unknown";
    }
}
