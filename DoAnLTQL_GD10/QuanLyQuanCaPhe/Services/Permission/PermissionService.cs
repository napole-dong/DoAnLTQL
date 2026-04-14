using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyQuanCaPhe.Services.Permission
{
    /// <summary>
    /// Centralized role-form permission service.
    /// - Maps (FormName, Role) -> FormPermission
    /// - Supports cache
    /// - Supports replacing/reloading mapping (ready for DB-backed source later)
    /// </summary>
    public class PermissionService
    {
        public static PermissionService Shared { get; } = new();

        private readonly ConcurrentDictionary<string, FormPermission> _cache = new();
        private readonly Func<Dictionary<string, Dictionary<UserRole, PermissionAction[]>>> _mappingProvider;
        private Dictionary<string, Dictionary<UserRole, PermissionAction[]>> _actionMapping;

        public PermissionService(
            Dictionary<string, Dictionary<UserRole, PermissionAction[]>>? overrides = null,
            Func<Dictionary<string, Dictionary<UserRole, PermissionAction[]>>>? mappingProvider = null,
            bool useCache = true)
        {
            UseCache = useCache;
            _mappingProvider = mappingProvider ?? BuildDefaultMapping;
            _actionMapping = NormalizeMapping(overrides ?? _mappingProvider());
        }

        public bool UseCache { get; }

        private static Dictionary<string, Dictionary<UserRole, PermissionAction[]>> BuildDefaultMapping()
        {
            // Keys are form class names.
            var m = new Dictionary<string, Dictionary<UserRole, PermissionAction[]>>(StringComparer.OrdinalIgnoreCase)
            {
                ["frmDangNhap"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.Full },
                    [UserRole.Staff] = new[] { PermissionAction.Full }
                },

                ["frmBanHang"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.Full },
                    [UserRole.Staff] = new[] { PermissionAction.Full }
                },

                ["frmHoaDon"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.Full },
                    [UserRole.Staff] = new[] { PermissionAction.Full }
                },

                ["frmKhachHang"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.Full },
                    [UserRole.Staff] = new[] { PermissionAction.Add, PermissionAction.Edit }
                },

                ["frmNhanVien"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.View },
                    [UserRole.Staff] = new[] { PermissionAction.None }
                },

                ["frmQuanLiBan"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.Full },
                    [UserRole.Staff] = new[] { PermissionAction.LimitedAction }
                },

                ["frmQuanLiKho"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.Full },
                    [UserRole.Staff] = new[] { PermissionAction.None }
                },

                ["frmQuanLiMon"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.View },
                    [UserRole.Staff] = new[] { PermissionAction.None }
                },

                ["frmCongThuc"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.None },
                    [UserRole.Staff] = new[] { PermissionAction.None }
                },

                ["frmThongKe"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.Full },
                    [UserRole.Staff] = new[] { PermissionAction.None }
                },

                ["frmAuditLog"] = new()
                {
                    [UserRole.Admin] = new[] { PermissionAction.Full },
                    [UserRole.Manager] = new[] { PermissionAction.None },
                    [UserRole.Staff] = new[] { PermissionAction.None }
                }
            };

            return m;
        }

        public bool CanAccessForm(string formName, UserRole role)
        {
            return GetPermission(formName, role).CanView;
        }

        /// <summary>
        /// Returns a FormPermission (booleans) for the given form name and role.
        /// </summary>
        public FormPermission GetPermission(string formName, UserRole role)
        {
            var normalizedFormName = NormalizeFormName(formName);
            if (normalizedFormName.Length == 0)
            {
                return FormPermission.Deny(string.Empty, role);
            }

            var cacheKey = BuildCacheKey(normalizedFormName, role);
            if (UseCache && _cache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var actions = GetPermissionActions(normalizedFormName, role);
            var perm = MapActionsToFormPermission(normalizedFormName, role, actions);

            if (UseCache)
            {
                _cache.TryAdd(cacheKey, perm);
            }

            return perm;
        }

        /// <summary>
        /// Returns the raw action set for callers that need to handle special cases (ViewOwn, LimitedEdit, etc.)
        /// </summary>
        public PermissionAction[] GetPermissionActions(string formName, UserRole role)
        {
            var normalizedFormName = NormalizeFormName(formName);
            if (normalizedFormName.Length == 0)
            {
                return new[] { PermissionAction.None };
            }

            if (!_actionMapping.TryGetValue(normalizedFormName, out var roleMap))
            {
                return new[] { PermissionAction.None };
            }

            if (!roleMap.TryGetValue(role, out var actions) || actions == null || actions.Length == 0)
            {
                return new[] { PermissionAction.None };
            }

            return actions.ToArray();
        }

        public bool HasAction(string formName, UserRole role, PermissionAction action)
        {
            return GetPermission(formName, role).HasAction(action);
        }

        public void ReloadMapping()
        {
            _actionMapping = NormalizeMapping(_mappingProvider());
            ClearCache();
        }

        public void ReplaceMapping(Dictionary<string, Dictionary<UserRole, PermissionAction[]>> newMapping)
        {
            _actionMapping = NormalizeMapping(newMapping);
            ClearCache();
        }

        private static FormPermission MapActionsToFormPermission(string formName, UserRole role, PermissionAction[] actions)
        {
            var normalizedActions = NormalizeActions(actions);
            var set = normalizedActions.ToHashSet();

            if (set.Contains(PermissionAction.Full))
            {
                return new FormPermission
                {
                    FormName = formName,
                    Role = role,
                    Actions = new[]
                    {
                        PermissionAction.Full,
                        PermissionAction.View,
                        PermissionAction.Add,
                        PermissionAction.Edit,
                        PermissionAction.Delete
                    },
                    CanView = true,
                    CanAdd = true,
                    CanEdit = true,
                    CanDelete = true,
                    IsFullAccess = true
                };
            }

            var canView = set.Contains(PermissionAction.View)
                          || set.Contains(PermissionAction.Add)
                          || set.Contains(PermissionAction.Edit)
                          || set.Contains(PermissionAction.Delete)
                          || set.Contains(PermissionAction.ViewOwn)
                          || set.Contains(PermissionAction.LimitedEdit)
                          || set.Contains(PermissionAction.LimitedAction);

            return new FormPermission
            {
                FormName = formName,
                Role = role,
                Actions = normalizedActions,
                CanView = canView,
                CanAdd = set.Contains(PermissionAction.Add),
                CanEdit = set.Contains(PermissionAction.Edit) || set.Contains(PermissionAction.LimitedEdit),
                CanDelete = set.Contains(PermissionAction.Delete),
                IsFullAccess = false
            };
        }

        private static Dictionary<string, Dictionary<UserRole, PermissionAction[]>> NormalizeMapping(
            Dictionary<string, Dictionary<UserRole, PermissionAction[]>> source)
        {
            var normalized = new Dictionary<string, Dictionary<UserRole, PermissionAction[]>>(StringComparer.OrdinalIgnoreCase);

            foreach (var formEntry in source)
            {
                var normalizedFormName = NormalizeFormName(formEntry.Key);
                if (normalizedFormName.Length == 0 || formEntry.Value == null)
                {
                    continue;
                }

                var roleMap = new Dictionary<UserRole, PermissionAction[]>();
                foreach (var roleEntry in formEntry.Value)
                {
                    roleMap[roleEntry.Key] = NormalizeActions(roleEntry.Value);
                }

                normalized[normalizedFormName] = roleMap;
            }

            return normalized;
        }

        private static PermissionAction[] NormalizeActions(PermissionAction[]? actions)
        {
            if (actions == null || actions.Length == 0)
            {
                return new[] { PermissionAction.None };
            }

            var normalized = actions.Distinct().ToList();
            if (normalized.Count > 1)
            {
                normalized.Remove(PermissionAction.None);
            }

            return normalized.Count == 0
                ? new[] { PermissionAction.None }
                : normalized.ToArray();
        }

        private static string NormalizeFormName(string? formName)
        {
            if (string.IsNullOrWhiteSpace(formName))
            {
                return string.Empty;
            }

            var normalized = formName.Trim();
            var lastDot = normalized.LastIndexOf('.');
            if (lastDot >= 0 && lastDot < normalized.Length - 1)
            {
                normalized = normalized[(lastDot + 1)..];
            }

            return normalized;
        }

        private static string BuildCacheKey(string formName, UserRole role)
        {
            return formName.ToUpperInvariant() + ":" + role;
        }

        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}
