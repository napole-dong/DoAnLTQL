using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.DAL;

public class PermissionDAL
{
    public bool CheckPermission(int roleId, string feature, string action)
    {
        if (roleId <= 0 || string.IsNullOrWhiteSpace(feature) || string.IsNullOrWhiteSpace(action))
        {
            return false;
        }

        using var context = new CaPheDbContext();
        var permission = QueryPermissionByRoleAndFeature(context, roleId, feature.Trim());

        if (permission == null)
        {
            return false;
        }

        return CoActionDuocPhep(permission, action.Trim());
    }

    public List<string> LayDanhSachVaiTroCoTheGan(int currentRoleId)
    {
        if (currentRoleId <= 0)
        {
            return new List<string>();
        }

        using var context = new CaPheDbContext();

        var dsVaiTro = QueryDanhSachVaiTroRows(context);
        var roleIds = dsVaiTro
            .Select(x => x.ID)
            .Append(currentRoleId)
            .Distinct()
            .ToList();
        var dsQuyen = QueryDanhSachQuyenTheoRoleIds(context, roleIds);

        return dsVaiTro
            .Where(vaiTro => CoTheGanVaiTroNoiBo(currentRoleId, vaiTro.ID, dsQuyen))
            .Select(x => x.TenVaiTro)
            .ToList();
    }

    public bool CoTheGanVaiTro(int currentRoleId, string targetRoleName)
    {
        if (currentRoleId <= 0 || string.IsNullOrWhiteSpace(targetRoleName))
        {
            return false;
        }

        using var context = new CaPheDbContext();

        var targetRole = QueryVaiTroTheoTen(context, targetRoleName.Trim());

        if (targetRole == null)
        {
            return false;
        }

        var dsQuyen = QueryDanhSachQuyenTheoRoleIds(context, new[] { currentRoleId, targetRole.ID });

        return CoTheGanVaiTroNoiBo(currentRoleId, targetRole.ID, dsQuyen);
    }

    private static PermissionReadModel? QueryPermissionByRoleAndFeature(CaPheDbContext context, int roleId, string feature)
    {
        return context.Permission
            .AsNoTracking()
            .Where(x => x.VaiTroID == roleId && x.Feature == feature)
            .Select(x => new PermissionReadModel
            {
                VaiTroID = x.VaiTroID,
                Feature = x.Feature,
                CanView = x.CanView,
                CanCreate = x.CanCreate,
                CanUpdate = x.CanUpdate,
                CanDelete = x.CanDelete
            })
            .FirstOrDefault();
    }

    private static List<VaiTroReadModel> QueryDanhSachVaiTroRows(CaPheDbContext context)
    {
        return context.VaiTro
            .AsNoTracking()
            .OrderBy(x => x.TenVaiTro)
            .Select(x => new VaiTroReadModel
            {
                ID = x.ID,
                TenVaiTro = x.TenVaiTro
            })
            .ToList();
    }

    private static VaiTroReadModel? QueryVaiTroTheoTen(CaPheDbContext context, string tenVaiTro)
    {
        return context.VaiTro
            .AsNoTracking()
            .Where(x => x.TenVaiTro == tenVaiTro)
            .Select(x => new VaiTroReadModel
            {
                ID = x.ID,
                TenVaiTro = x.TenVaiTro
            })
            .FirstOrDefault();
    }

    private static List<PermissionReadModel> QueryDanhSachQuyenTheoRoleIds(CaPheDbContext context, IEnumerable<int> roleIds)
    {
        var roleIdSet = roleIds.Distinct().ToList();
        if (roleIdSet.Count == 0)
        {
            return new List<PermissionReadModel>();
        }

        return context.Permission
            .AsNoTracking()
            .Where(x => roleIdSet.Contains(x.VaiTroID))
            .Select(x => new PermissionReadModel
            {
                VaiTroID = x.VaiTroID,
                Feature = x.Feature,
                CanView = x.CanView,
                CanCreate = x.CanCreate,
                CanUpdate = x.CanUpdate,
                CanDelete = x.CanDelete
            })
            .ToList();
    }

    private static bool CoActionDuocPhep(PermissionReadModel permission, string action)
    {
        return action switch
        {
            "View" => permission.CanView,
            "Create" => permission.CanCreate,
            "Update" => permission.CanUpdate,
            "Delete" => permission.CanDelete,
            _ => false
        };
    }

    private static bool CoTheGanVaiTroNoiBo(int currentRoleId, int targetRoleId, IEnumerable<PermissionReadModel> dsQuyen)
    {
        var quyenNguoiGan = dsQuyen
            .Where(x => x.VaiTroID == currentRoleId)
            .ToDictionary(x => x.Feature, x => x, StringComparer.OrdinalIgnoreCase);

        var quyenVaiTroDich = dsQuyen
            .Where(x => x.VaiTroID == targetRoleId)
            .ToList();

        foreach (var quyenDich in quyenVaiTroDich)
        {
            if (!quyenNguoiGan.TryGetValue(quyenDich.Feature, out var quyenNguoiDangDangNhap))
            {
                if (quyenDich.CanView || quyenDich.CanCreate || quyenDich.CanUpdate || quyenDich.CanDelete)
                {
                    return false;
                }

                continue;
            }

            if (CoQuyenMoRongHon(quyenNguoiDangDangNhap, quyenDich))
            {
                return false;
            }
        }

        return true;
    }

    private static bool CoQuyenMoRongHon(PermissionReadModel quyenNguoiDangDangNhap, PermissionReadModel quyenDich)
    {
        return (quyenDich.CanView && !quyenNguoiDangDangNhap.CanView)
               || (quyenDich.CanCreate && !quyenNguoiDangDangNhap.CanCreate)
               || (quyenDich.CanUpdate && !quyenNguoiDangDangNhap.CanUpdate)
               || (quyenDich.CanDelete && !quyenNguoiDangDangNhap.CanDelete);
    }

    private sealed class PermissionReadModel
    {
        public int VaiTroID { get; init; }
        public string Feature { get; init; } = string.Empty;
        public bool CanView { get; init; }
        public bool CanCreate { get; init; }
        public bool CanUpdate { get; init; }
        public bool CanDelete { get; init; }
    }

    private sealed class VaiTroReadModel
    {
        public int ID { get; init; }
        public string TenVaiTro { get; init; } = string.Empty;
    }
}
