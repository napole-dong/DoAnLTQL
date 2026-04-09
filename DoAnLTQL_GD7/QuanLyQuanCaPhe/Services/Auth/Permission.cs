namespace QuanLyQuanCaPhe.Services.Auth;

public sealed class Permission
{
    private readonly Session _session;
    private readonly Func<string, string, bool> _permissionEvaluator;

    public Permission(Session session, Func<string, string, bool> permissionEvaluator)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _permissionEvaluator = permissionEvaluator ?? throw new ArgumentNullException(nameof(permissionEvaluator));
    }

    public bool IsAdmin()
    {
        return _session.Role == RoleEnum.Admin;
    }

    public bool IsManager()
    {
        return _session.Role == RoleEnum.Manager;
    }

    public bool IsStaff()
    {
        return _session.Role == RoleEnum.Staff;
    }

    public bool CanManageUser()
    {
        return IsAdmin()
               && HasPermission(ActionType.Read, Feature.User)
               && HasPermission(ActionType.Create, Feature.User)
               && HasPermission(ActionType.Update, Feature.User)
               && HasPermission(ActionType.Delete, Feature.User);
    }

    public bool CanManageMenu()
    {
        return HasPermission(ActionType.Read, Feature.Menu)
               && HasPermission(ActionType.Create, Feature.Menu)
               && HasPermission(ActionType.Update, Feature.Menu)
               && HasPermission(ActionType.Delete, Feature.Menu);
    }

    public bool CanManageInventory()
    {
        return HasPermission(ActionType.Read, Feature.Inventory)
               && HasPermission(ActionType.Create, Feature.Inventory)
               && HasPermission(ActionType.Update, Feature.Inventory);
    }

    public bool CanViewReport()
    {
        return HasPermission(ActionType.Read, Feature.Report);
    }

    public bool CanSell()
    {
        return HasPermission(ActionType.Read, Feature.Sell)
               && HasPermission(ActionType.Create, Feature.Sell);
    }

    public bool HasPermission(ActionType action, Feature feature)
    {
        var featureKey = PermissionFeatures.ToFeatureKey(feature);
        var actionKey = PermissionActions.ToActionKey(action);
        return HasPermission(featureKey, actionKey);
    }

    public bool HasPermission(string feature, string action)
    {
        if (!PermissionFeatures.IsKnownFeature(feature) || !PermissionActions.IsKnownAction(action))
        {
            return false;
        }

        if (IsAdmin())
        {
            return true;
        }

        var featureKey = PermissionFeatures.NormalizeFeature(feature);
        var actionKey = PermissionActions.NormalizeAction(action);
        return _permissionEvaluator(featureKey, actionKey);
    }

    public void EnsurePermission(ActionType action, Feature feature, string? message = null)
    {
        if (HasPermission(action, feature))
        {
            return;
        }

        throw new UnauthorizedAccessException(message ?? "Khong co quyen");
    }

    public void EnsurePermission(string feature, string action, string? message = null)
    {
        if (HasPermission(feature, action))
        {
            return;
        }

        throw new UnauthorizedAccessException(message ?? "Khong co quyen");
    }

    public static bool IsAdminSession(Session? session)
    {
        return session?.Role == RoleEnum.Admin;
    }

    public static bool IsManagerSession(Session? session)
    {
        return session?.Role == RoleEnum.Manager;
    }

    public static bool IsStaffSession(Session? session)
    {
        return session?.Role == RoleEnum.Staff;
    }
}