namespace QuanLyQuanCaPhe.Services.Auth;

public enum ActionType
{
    Create,
    Read,
    Update,
    Delete
}

public static class PermissionActions
{
    public const string View = "View";
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";

    // Action aliases for fine-grained business workflows.
    public const string NhapKho = "NhapKho";
    public const string PhanQuyen = "PhanQuyen";

    private static readonly HashSet<string> CrudActions = new(StringComparer.OrdinalIgnoreCase)
    {
        View,
        Create,
        Update,
        Delete
    };

    public static string ToActionKey(ActionType action)
    {
        return action switch
        {
            ActionType.Create => Create,
            ActionType.Read => View,
            ActionType.Update => Update,
            ActionType.Delete => Delete,
            _ => string.Empty
        };
    }

    public static bool TryParseActionType(string? action, out ActionType actionType)
    {
        var actionKey = NormalizeAction(action);
        var isKnown = CrudActions.Contains(actionKey);
        actionType = actionKey.ToUpperInvariant() switch
        {
            "CREATE" => ActionType.Create,
            "VIEW" => ActionType.Read,
            "UPDATE" => ActionType.Update,
            "DELETE" => ActionType.Delete,
            _ => ActionType.Read
        };

        return isKnown;
    }

    public static string NormalizeAction(string? action)
    {
        if (string.IsNullOrWhiteSpace(action))
        {
            return string.Empty;
        }

        var actionKey = action.Trim();
        if (actionKey.Equals(NhapKho, StringComparison.OrdinalIgnoreCase)
            || actionKey.Equals(PhanQuyen, StringComparison.OrdinalIgnoreCase))
        {
            return Update;
        }

        return actionKey;
    }

    public static bool IsKnownAction(string? action)
    {
        var actionKey = NormalizeAction(action);
        if (actionKey.Length == 0)
        {
            return false;
        }

        return CrudActions.Contains(actionKey);
    }
}
