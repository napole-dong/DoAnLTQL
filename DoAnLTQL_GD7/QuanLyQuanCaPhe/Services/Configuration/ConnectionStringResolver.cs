using System.Configuration;
using System.Text;

namespace QuanLyQuanCaPhe.Services.Configuration;

public static class ConnectionStringResolver
{
    private const string AppConfigConnectionName = "CaPheConnection";
    private const string EnvironmentOverrideKey = "CAPHE_ENVIRONMENT";
    private const string EnvironmentAppSettingKey = "CaPheEnvironment";
    private const string ConnectionStringEnvKey = "CAPHE_CONNECTION_STRING";
    private const string ConnectionStringFileEnvKey = "CAPHE_CONNECTION_STRING_FILE";

    public static string Resolve()
    {
        var environmentName = ResolveEnvironmentName();

        if (TryGetEnvironmentSetting(ConnectionStringEnvKey, environmentName, includeGeneric: true, out var connectionString))
        {
            return connectionString;
        }

        if (TryGetEnvironmentSetting(ConnectionStringFileEnvKey, environmentName, includeGeneric: true, out var secretFilePath))
        {
            return ReadConnectionStringFromSecretFile(secretFilePath);
        }

        if (TryGetFromAppConfig(environmentName, out connectionString))
        {
            return connectionString;
        }

        if (TryGetFromAppConfig(null, out connectionString))
        {
            return connectionString;
        }

        throw new InvalidOperationException(
            "Khong tim thay chuoi ket noi. Hay cau hinh CAPHE_CONNECTION_STRING/CAPHE_CONNECTION_STRING_FILE hoac them CaPheConnection trong App.config.");
    }

    private static string? ResolveEnvironmentName()
    {
        var environmentName = Environment.GetEnvironmentVariable(EnvironmentOverrideKey);
        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            return environmentName.Trim();
        }

        environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            return environmentName.Trim();
        }

        environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            return environmentName.Trim();
        }

        environmentName = ConfigurationManager.AppSettings[EnvironmentAppSettingKey];
        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            return environmentName.Trim();
        }

        return null;
    }

    private static bool TryGetEnvironmentSetting(string baseKey, string? environmentName, bool includeGeneric, out string value)
    {
        foreach (var key in BuildEnvironmentKeys(baseKey, environmentName, includeGeneric))
        {
            var environmentValue = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrWhiteSpace(environmentValue))
            {
                value = environmentValue.Trim();
                return true;
            }
        }

        value = string.Empty;
        return false;
    }

    private static IEnumerable<string> BuildEnvironmentKeys(string baseKey, string? environmentName, bool includeGeneric)
    {
        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            var token = NormalizeToken(environmentName);
            if (!string.IsNullOrWhiteSpace(token))
            {
                yield return $"{baseKey}__{token}";
                yield return $"{baseKey}_{token}";
            }
        }

        if (includeGeneric)
        {
            yield return baseKey;
        }
    }

    private static bool TryGetFromAppConfig(string? environmentName, out string connectionString)
    {
        foreach (var name in BuildConnectionStringNames(environmentName))
        {
            var fromConnectionStrings = ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
            if (!string.IsNullOrWhiteSpace(fromConnectionStrings))
            {
                connectionString = fromConnectionStrings.Trim();
                return true;
            }

            var fromAppSettings = ConfigurationManager.AppSettings[name];
            if (!string.IsNullOrWhiteSpace(fromAppSettings))
            {
                connectionString = fromAppSettings.Trim();
                return true;
            }
        }

        connectionString = string.Empty;
        return false;
    }

    private static IEnumerable<string> BuildConnectionStringNames(string? environmentName)
    {
        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            var token = NormalizeToken(environmentName);
            yield return $"{AppConfigConnectionName}.{environmentName.Trim()}";

            if (!string.IsNullOrWhiteSpace(token) && !string.Equals(token, environmentName.Trim(), StringComparison.Ordinal))
            {
                yield return $"{AppConfigConnectionName}.{token}";
                yield return $"{AppConfigConnectionName}_{token}";
            }
        }

        yield return AppConfigConnectionName;
    }

    private static string ReadConnectionStringFromSecretFile(string secretFilePath)
    {
        if (!File.Exists(secretFilePath))
        {
            throw new InvalidOperationException($"Khong tim thay file secret chuoi ket noi: {secretFilePath}");
        }

        var content = File.ReadAllText(secretFilePath, Encoding.UTF8).Trim();
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException($"File secret chuoi ket noi rong: {secretFilePath}");
        }

        return content;
    }

    private static string NormalizeToken(string source)
    {
        var text = source.Trim().ToUpperInvariant();
        if (text.Length == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder(text.Length);
        foreach (var ch in text)
        {
            builder.Append(char.IsLetterOrDigit(ch) ? ch : '_');
        }

        return builder.ToString();
    }
}
