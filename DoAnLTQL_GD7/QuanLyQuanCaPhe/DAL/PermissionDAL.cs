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
        var permission = context.Permission
            .AsNoTracking()
            .FirstOrDefault(x => x.VaiTroID == roleId && x.Feature == feature.Trim());

        if (permission == null)
        {
            return false;
        }

        return action.Trim() switch
        {
            "View" => permission.CanView,
            "Create" => permission.CanCreate,
            "Update" => permission.CanUpdate,
            "Delete" => permission.CanDelete,
            _ => false
        };
    }

    public List<string> LayDanhSachVaiTroCoTheGan(int currentRoleId)
    {
        if (currentRoleId <= 0)
        {
            return new List<string>();
        }

        using var context = new CaPheDbContext();

        var dsVaiTro = context.VaiTro
            .AsNoTracking()
            .OrderBy(x => x.TenVaiTro)
            .ToList();

        var dsQuyen = context.Permission
            .AsNoTracking()
            .ToList();

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

        var targetRole = context.VaiTro
            .AsNoTracking()
            .FirstOrDefault(x => x.TenVaiTro == targetRoleName.Trim());

        if (targetRole == null)
        {
            return false;
        }

        var dsQuyen = context.Permission
            .AsNoTracking()
            .ToList();

        return CoTheGanVaiTroNoiBo(currentRoleId, targetRole.ID, dsQuyen);
    }

    private static bool CoTheGanVaiTroNoiBo(int currentRoleId, int targetRoleId, IEnumerable<dtaPermission> dsQuyen)
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

    private static bool CoQuyenMoRongHon(dtaPermission quyenNguoiDangDangNhap, dtaPermission quyenDich)
    {
        return (quyenDich.CanView && !quyenNguoiDangDangNhap.CanView)
               || (quyenDich.CanCreate && !quyenNguoiDangDangNhap.CanCreate)
               || (quyenDich.CanUpdate && !quyenNguoiDangDangNhap.CanUpdate)
               || (quyenDich.CanDelete && !quyenNguoiDangDangNhap.CanDelete);
    }
}
