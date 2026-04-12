using Microsoft.Extensions.DependencyInjection;
using QuanLyQuanCaPhe.BUS;
using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.Forms;
using QuanLyQuanCaPhe.Presenters;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.HoaDon;
using QuanLyQuanCaPhe.Services.Mon;

namespace QuanLyQuanCaPhe.Services.DependencyInjection;

public static class ServiceRegistration
{
    public static ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        RegisterRepositories(services);
        RegisterBusinessServices(services);
        RegisterFormServices(services);
        RegisterForms(services);

        return services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = false,
            ValidateOnBuild = false
        });
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddTransient<IBanRepository, BanDAL>();
        services.AddTransient<IBanHangRepository, BanHangDAL>();
        services.AddTransient<IAuditLogRepository, AuditLogDAL>();
        services.AddTransient<ICongThucRepository, CongThucDAL>();
        services.AddTransient<IDangNhapRepository, DangNhapDAL>();
        services.AddTransient<IHoaDonRepository, HoaDonDAL>();
        services.AddTransient<IKhachHangRepository, KhachHangDAL>();
        services.AddTransient<ILoaiMonRepository, LoaiMonDAL>();
        services.AddTransient<IMonRepository, MonDAL>();
        services.AddTransient<INguyenLieuRepository, NguyenLieuDAL>();
        services.AddTransient<INhanVienRepository, NhanVienDAL>();
        services.AddTransient<IPermissionRepository, PermissionDAL>();
        services.AddTransient<ITaiKhoanRepository, TaiKhoanDAL>();
        services.AddTransient<ITaiKhoanMacDinhRepository, TaiKhoanMacDinhDAL>();
        services.AddTransient<IThongKeRepository, ThongKeDAL>();
    }

    private static void RegisterBusinessServices(IServiceCollection services)
    {
        services.AddTransient<IActivityLogWriter, ActivityLogService>();
        services.AddTransient<IPermissionService, PermissionBUS>();
        services.AddTransient<IDangNhapService, DangNhapBUS>();
        services.AddTransient<IBanService, BanBUS>();
        services.AddTransient<IBanHangService, BanHangBUS>();
        services.AddTransient<IAuditLogService, AuditLogBUS>();
        services.AddTransient<ICongThucService, CongThucBUS>();
        services.AddTransient<IHoaDonService, HoaDonBUS>();
        services.AddTransient<IKhachHangService, KhachHangBUS>();
        services.AddTransient<ILoaiMonService, LoaiMonBUS>();
        services.AddTransient<IMonService, MonBUS>();
        services.AddTransient<INguyenLieuService, NguyenLieuBUS>();
        services.AddTransient<INhanVienService, NhanVienBUS>();
        services.AddTransient<ITaiKhoanService, TaiKhoanBUS>();
        services.AddTransient<IThongKeService, ThongKeBUS>();
        services.AddTransient<IOrderService, OrderService>();
    }

    private static void RegisterFormServices(IServiceCollection services)
    {
        services.AddTransient<HoaDonPreviewService>();
        services.AddTransient<HoaDonTienService>();
        services.AddTransient<HoaDonFormStateService>();
        services.AddTransient<MonInputValidator>();
        services.AddTransient<BanHangPresenter>();
        services.AddTransient<CongThucPresenter>();
        services.AddTransient<NhanVienPresenter>();
        services.AddTransient(provider => new HoaDonPresenter("Dữ liệu đã bị thay đổi bởi nhân viên khác. Vui lòng tải lại!"));
    }

    private static void RegisterForms(IServiceCollection services)
    {
        services.AddTransient<frmDangNhap>();
        services.AddTransient<frmBanHang>();
        services.AddTransient<frmCongThuc>();
        services.AddTransient<frmHoaDon>();
        services.AddTransient<frmKhachHang>();
        services.AddTransient<frmNhanVien>();
        services.AddTransient<frmQuanLiBan>();
        services.AddTransient<frmQuanLiKho>();
        services.AddTransient<frmQuanLiMon>();
        services.AddTransient<frmThongKe>();
        services.AddTransient<frmAuditLog>();
    }
}
