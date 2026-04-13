using System;
using System.Linq;

namespace QuanLyQuanCaPhe.Services.Permission
{
    public enum UserRole
    {
        Admin,
        Manager,
        Staff
    }

    public enum PermissionAction
    {
        View,
        Add,
        Edit,
        Delete,
        Full,
        None,
        ViewOwn,
        LimitedEdit,
        LimitedAction
    }

    public sealed class FormPermission
    {
        public string FormName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Staff;
        public PermissionAction[] Actions { get; set; } = Array.Empty<PermissionAction>();
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool IsFullAccess { get; set; }

        public bool CanViewOwn => HasAction(PermissionAction.ViewOwn);
        public bool HasLimitedEdit => HasAction(PermissionAction.LimitedEdit);
        public bool HasLimitedAction => HasAction(PermissionAction.LimitedAction);
        public bool IsViewOnly => CanView && !CanAdd && !CanEdit && !CanDelete;

        public bool HasAction(PermissionAction action)
        {
            return Actions.Any(x => x == action);
        }

        public static FormPermission Deny(string formName, UserRole role)
        {
            return new FormPermission
            {
                FormName = formName,
                Role = role,
                Actions = new[] { PermissionAction.None },
                CanView = false,
                CanAdd = false,
                CanEdit = false,
                CanDelete = false,
                IsFullAccess = false
            };
        }
    }
}
