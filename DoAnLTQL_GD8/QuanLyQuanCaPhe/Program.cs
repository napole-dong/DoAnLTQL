using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var logDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "QuanLyQuanCaPhe",
                "logs");

            AppLogger.Initialize(logDirectory);
            ConfigureGlobalExceptionHandling();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try
            {
                using var startupScope = CorrelationContext.BeginScope();
                AppLogger.Info("Application startup.", nameof(Program));
                KhoiTaoDuLieuQuyenMacDinh();

                while (true)
                {
                    DangXuatDieuHuongService.DatLaiYeuCauDangNhapLai();

                    using var frmDangNhap = new Forms.frmDangNhap();
                    if (frmDangNhap.ShowDialog() != DialogResult.OK)
                    {
                        AppLogger.Info("Login canceled by user.", nameof(Program));
                        break;
                    }

                    if (frmDangNhap.ThongTinDangNhap == null)
                    {
                        AppLogger.Warning("Login returned OK but login information is null.", nameof(Program));
                        break;
                    }

                    NguoiDungHienTaiService.DatNguoiDungDangNhap(frmDangNhap.ThongTinDangNhap);
                    AppLogger.Info("Login succeeded.", nameof(Program));

                    Application.Run(new Forms.frmBanHang());
                    NguoiDungHienTaiService.XoaNguoiDungDangNhap();

                    if (!DangXuatDieuHuongService.DaYeuCauDangNhapLai)
                    {
                        break;
                    }

                    AppLogger.Info("User logged out. Returning to login screen.", nameof(Program));
                }
            }
            catch (Exception ex)
            {
                AppExceptionHandler.Handle(ex, "Program.Main", showDialog: true);
            }
            finally
            {
                NguoiDungHienTaiService.XoaNguoiDungDangNhap();
                CorrelationContext.Clear();
                AppLogger.Info("Application shutdown.", nameof(Program));
                AppLogger.Shutdown();
            }
        }

        private static void KhoiTaoDuLieuQuyenMacDinh()
        {
            var permissionBUS = new PermissionBUS();
            var daDongBo = permissionBUS.DongBoDuLieuQuyenMacDinh();

            AppLogger.Info(
                daDongBo
                    ? "Permission defaults synchronized at startup."
                    : "Permission defaults already up to date at startup.",
                nameof(Program));
        }

        private static void ConfigureGlobalExceptionHandling()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.ThreadException += (_, args) =>
            {
                using var scope = CorrelationContext.BeginScope();
                AppExceptionHandler.Handle(args.Exception, "Application.ThreadException", showDialog: true);
            };

            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                using var scope = CorrelationContext.BeginScope();

                var exception = args.ExceptionObject as Exception
                    ?? new Exception(args.ExceptionObject?.ToString() ?? "Unhandled non-exception object.");

                AppExceptionHandler.Handle(exception, "AppDomain.CurrentDomain.UnhandledException", showDialog: true);
            };

            TaskScheduler.UnobservedTaskException += (_, args) =>
            {
                using var scope = CorrelationContext.BeginScope();
                AppExceptionHandler.Handle(args.Exception, "TaskScheduler.UnobservedTaskException", showDialog: false);
                args.SetObserved();
            };
        }
    }
}