namespace QuanLyQuanCaPhe.Services.Diagnostics;

public static class AppErrorCode
{
    public const string Unknown = "SYS-500";
    public const string Timeout = "SYS-408";
    public const string InvalidOperation = "APP-409";
    public const string Unauthorized = "SEC-403";
    public const string DbDuplicateKey = "DB-409";
    public const string DbUpdateFailed = "DB-500";
}
