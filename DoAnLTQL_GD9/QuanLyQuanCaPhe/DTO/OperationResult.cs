namespace QuanLyQuanCaPhe.DTO;

public sealed class OperationResult
{
    public bool ThanhCong { get; init; }
    public string MaThongBao { get; init; } = string.Empty;
    public string ThongBao { get; init; } = string.Empty;

    public static OperationResult Success(string thongBao, string maThongBao = "")
    {
        return new OperationResult
        {
            ThanhCong = true,
            ThongBao = thongBao,
            MaThongBao = maThongBao
        };
    }

    public static OperationResult Failure(string thongBao, string maThongBao = "")
    {
        return new OperationResult
        {
            ThanhCong = false,
            ThongBao = thongBao,
            MaThongBao = maThongBao
        };
    }
}