using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.Services.Permission
{
    public static class PermissionExtensions
    {
        private static readonly string[] AddPatterns = { "btnAdd", "btnThem", "Add", "Them", "Create", "Tao" };
        private static readonly string[] EditPatterns = { "btnEdit", "btnCapNhat", "Edit", "CapNhat", "Sua", "Update", "Luu" };
        private static readonly string[] DeletePatterns = { "btnDelete", "btnXoa", "Delete", "Xoa", "Remove" };

        /// <summary>
        /// Apply permission to a WinForms Form by: hiding/disabling common Add/Edit/Delete buttons and making DataGridView readonly when appropriate.
        /// This uses heuristics on control names (vb: "btnThem", "btnCapNhat", "btnXoa", etc.).
        /// For more precise control pass a custom routine from the form instead of relying solely on this helper.
        /// </summary>
        public static void ApplyPermission(this Form form, FormPermission permission)
        {
            if (form == null || permission == null)
            {
                return;
            }

            var allControls = GetAllControlsRecursive(form).ToList();

            // Buttons
            foreach (var btn in allControls.OfType<Button>())
            {
                var name = (btn.Name ?? string.Empty).ToLowerInvariant();
                if (MatchesAny(name, AddPatterns))
                {
                    btn.Visible = permission.CanAdd;
                    btn.Enabled = permission.CanAdd;
                }
                else if (MatchesAny(name, EditPatterns))
                {
                    btn.Visible = permission.CanEdit;
                    btn.Enabled = permission.CanEdit;
                }
                else if (MatchesAny(name, DeletePatterns))
                {
                    btn.Visible = permission.CanDelete;
                    btn.Enabled = permission.CanDelete;
                }
            }

            // DataGridViews: if user only has view permission (no add/edit/delete) then make grids readonly
            var onlyView = permission.IsViewOnly;
            foreach (var dgv in allControls.OfType<DataGridView>())
            {
                dgv.ReadOnly = onlyView;
            }
        }

        /// <summary>
        /// Resolve permission from PermissionService and apply to the form in one call.
        /// </summary>
        public static FormPermission ApplyPermission(this Form form, PermissionService permissionService, UserRole role, string? formName = null)
        {
            var targetFormName = string.IsNullOrWhiteSpace(formName) ? form.GetType().Name : formName;
            var permission = permissionService.GetPermission(targetFormName, role);
            form.ApplyPermission(permission);
            return permission;
        }

        /// <summary>
        /// Guard form access. When no view permission, show message and return false.
        /// Caller decides whether to close the form.
        /// </summary>
        public static bool EnsureCanView(this Form form, FormPermission permission, string? deniedMessage = null)
        {
            if (permission.CanView)
            {
                return true;
            }

            MessageBox.Show(
                deniedMessage ?? "Bạn không có quyền truy cập",
                "Từ chối truy cập",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return false;
        }

        private static bool MatchesAny(string name, IEnumerable<string> patterns)
        {
            foreach (var p in patterns)
            {
                if (string.IsNullOrWhiteSpace(p))
                {
                    continue;
                }

                if (name.Contains(p.ToLowerInvariant()))
                {
                    return true;
                }
            }

            return false;
        }

        private static IEnumerable<Control> GetAllControlsRecursive(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                yield return c;
                foreach (var nested in GetAllControlsRecursive(c))
                {
                    yield return nested;
                }
            }
        }

        /// <summary>
        /// Helper to map current logged-in user info to a UserRole.
        /// This uses the user's QuyenHan text first then falls back to RoleId heuristics.
        /// </summary>
        public static UserRole GetCurrentUserRole()
        {
            var login = NguoiDungHienTaiService.LayNguoiDungDangNhap();
            if (login == null)
            {
                return UserRole.Staff;
            }

            if (RoleMapper.TryParseRoleEnum(login.QuyenHan, out var parsedRole))
            {
                return MapToUserRole(parsedRole);
            }

            // Fallback by RoleId when role name is unavailable.
            if (login.RoleId == 1)
            {
                return UserRole.Admin;
            }

            if (login.RoleId == 2)
            {
                return UserRole.Manager;
            }

            return UserRole.Staff;
        }

        private static UserRole MapToUserRole(RoleEnum role)
        {
            return role switch
            {
                RoleEnum.Admin => UserRole.Admin,
                RoleEnum.Manager => UserRole.Manager,
                _ => UserRole.Staff
            };
        }
    }
}
