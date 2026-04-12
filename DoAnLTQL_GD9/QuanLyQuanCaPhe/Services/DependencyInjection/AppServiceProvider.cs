using Microsoft.Extensions.DependencyInjection;

namespace QuanLyQuanCaPhe.Services.DependencyInjection;

public static class AppServiceProvider
{
    private static IServiceProvider? _serviceProvider;

    public static bool IsInitialized => _serviceProvider != null;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public static T GetRequiredService<T>() where T : notnull
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("Service provider has not been initialized.");
        }

        return _serviceProvider.GetRequiredService<T>();
    }

    public static T? TryGetService<T>() where T : class
    {
        return _serviceProvider?.GetService<T>();
    }

    public static T Resolve<T>(T? service, Func<T> fallbackFactory) where T : class
    {
        if (service != null)
        {
            return service;
        }

        var resolved = TryGetService<T>();
        if (resolved != null)
        {
            return resolved;
        }

        return fallbackFactory();
    }
}
