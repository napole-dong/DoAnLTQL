using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace QuanLyQuanCaPhe.Services.Diagnostics;

public static class AppExceptionMapper
{
    public static AppErrorDescriptor Map(Exception? exception, string? fallbackUserMessage = null)
    {
        if (exception is DbUpdateException dbUpdateEx)
        {
            if (IsDuplicateKey(dbUpdateEx))
            {
                return new AppErrorDescriptor(
                    AppErrorCode.DbDuplicateKey,
                    fallbackUserMessage ?? "Du lieu bi trung voi ban ghi da ton tai.");
            }

            return new AppErrorDescriptor(
                AppErrorCode.DbUpdateFailed,
                fallbackUserMessage ?? "Khong the cap nhat du lieu vao he thong.");
        }

        if (exception is TimeoutException)
        {
            return new AppErrorDescriptor(
                AppErrorCode.Timeout,
                fallbackUserMessage ?? "Yeu cau bi qua thoi gian cho. Vui long thu lai.");
        }

        if (exception is UnauthorizedAccessException)
        {
            return new AppErrorDescriptor(
                AppErrorCode.Unauthorized,
                fallbackUserMessage ?? "Ban khong co quyen thuc hien thao tac nay.");
        }

        if (exception is InvalidOperationException)
        {
            return new AppErrorDescriptor(
                AppErrorCode.InvalidOperation,
                fallbackUserMessage ?? "Trang thai du lieu khong hop le de thuc hien thao tac.");
        }

        return new AppErrorDescriptor(
            AppErrorCode.Unknown,
            fallbackUserMessage ?? "Da xay ra loi he thong ngoai du kien.");
    }

    private static bool IsDuplicateKey(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sqlException)
        {
            return sqlException.Number == 2601 || sqlException.Number == 2627;
        }

        return false;
    }
}

public sealed record AppErrorDescriptor(string Code, string UserMessage);
