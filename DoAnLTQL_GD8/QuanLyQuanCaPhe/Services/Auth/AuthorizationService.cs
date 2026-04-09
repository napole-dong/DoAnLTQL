namespace QuanLyQuanCaPhe.Services.Auth;

public sealed class AuthorizationPolicyOptions
{
    // Set false if business requires Manager cannot edit menu prices.
    public bool AllowManagerEditMenuPrice { get; init; } = true;
}

public sealed class AuthorizationService
{
    private readonly AuthorizationPolicyOptions _policy;

    public AuthorizationService(UserSession userSession, AuthorizationPolicyOptions? policy = null)
    {
        UserSession = userSession ?? throw new ArgumentNullException(nameof(userSession));
        _policy = policy ?? new AuthorizationPolicyOptions();
    }

    public UserSession UserSession { get; }
    public Role Role => UserSession.Role;

    public bool CanManageEmployees()
    {
        return Role == Role.Admin;
    }

    public bool CanManageMenu()
    {
        return Role == Role.Admin || Role == Role.Manager;
    }

    public bool CanEditMenuPrice()
    {
        return Role switch
        {
            Role.Admin => true,
            Role.Manager => _policy.AllowManagerEditMenuPrice,
            _ => false
        };
    }

    public bool CanViewReports()
    {
        return Role == Role.Admin || Role == Role.Manager;
    }

    public bool CanDeleteInvoice()
    {
        return Role == Role.Admin;
    }

    public bool CanTransferTable()
    {
        return Role == Role.Admin || Role == Role.Manager || Role == Role.Staff;
    }

    public bool CanMergeTable()
    {
        return Role == Role.Admin || Role == Role.Manager || Role == Role.Staff;
    }

    public void EnsureCanDeleteInvoice(string? message = null)
    {
        if (CanDeleteInvoice())
        {
            return;
        }

        throw new UnauthorizedAccessException(message ?? "Chi Admin moi duoc xoa hoa don.");
    }

    public void EnsureCanTransferTable(string? message = null)
    {
        if (CanTransferTable())
        {
            return;
        }

        throw new UnauthorizedAccessException(message ?? "Khong co quyen chuyen ban.");
    }

    public void EnsureCanMergeTable(string? message = null)
    {
        if (CanMergeTable())
        {
            return;
        }

        throw new UnauthorizedAccessException(message ?? "Khong co quyen gop ban.");
    }

    public static AuthorizationService? TryCreateForCurrentUser(AuthorizationPolicyOptions? policy = null)
    {
        var session = NguoiDungHienTaiService.LayUserSession();
        return session == null ? null : new AuthorizationService(session, policy);
    }
}
