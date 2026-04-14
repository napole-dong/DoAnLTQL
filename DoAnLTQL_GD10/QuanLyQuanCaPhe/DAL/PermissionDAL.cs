using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.Services.Auth;

namespace QuanLyQuanCaPhe.DAL;

public class PermissionDAL : IPermissionRepository
{
    private static readonly IReadOnlyDictionary<string, PermissionTemplate> AdminPermissionTemplate = new Dictionary<string, PermissionTemplate>(StringComparer.OrdinalIgnoreCase)
    {
        [PermissionFeatures.NhanVien] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.BanHang] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.Menu] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.KhachHang] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.NguyenLieu] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.HoaDon] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.ThongKe] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.TaiKhoan] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true)
    };

    private static readonly IReadOnlyDictionary<string, PermissionTemplate> ManagerPermissionTemplate = new Dictionary<string, PermissionTemplate>(StringComparer.OrdinalIgnoreCase)
    {
        [PermissionFeatures.NhanVien] = new PermissionTemplate(canView: true, canCreate: false, canUpdate: false, canDelete: false),
        [PermissionFeatures.BanHang] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: false),
        [PermissionFeatures.Menu] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.KhachHang] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.NguyenLieu] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: true),
        [PermissionFeatures.HoaDon] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: false),
        [PermissionFeatures.ThongKe] = new PermissionTemplate(canView: true, canCreate: false, canUpdate: false, canDelete: false),
        [PermissionFeatures.TaiKhoan] = new PermissionTemplate(canView: false, canCreate: false, canUpdate: false, canDelete: false)
    };

    private static readonly IReadOnlyDictionary<string, PermissionTemplate> StaffPermissionTemplate = new Dictionary<string, PermissionTemplate>(StringComparer.OrdinalIgnoreCase)
    {
        [PermissionFeatures.NhanVien] = new PermissionTemplate(canView: false, canCreate: false, canUpdate: false, canDelete: false),
        [PermissionFeatures.BanHang] = new PermissionTemplate(canView: true, canCreate: true, canUpdate: true, canDelete: false),
        [PermissionFeatures.Menu] = new PermissionTemplate(canView: true, canCreate: false, canUpdate: false, canDelete: false),
        [PermissionFeatures.KhachHang] = new PermissionTemplate(canView: false, canCreate: false, canUpdate: false, canDelete: false),
        [PermissionFeatures.NguyenLieu] = new PermissionTemplate(canView: false, canCreate: false, canUpdate: false, canDelete: false),
        [PermissionFeatures.HoaDon] = new PermissionTemplate(canView: true, canCreate: false, canUpdate: false, canDelete: false),
        [PermissionFeatures.ThongKe] = new PermissionTemplate(canView: false, canCreate: false, canUpdate: false, canDelete: false),
        [PermissionFeatures.TaiKhoan] = new PermissionTemplate(canView: false, canCreate: false, canUpdate: false, canDelete: false)
    };

    public bool CheckPermission(int roleId, string feature, string action)
    {
        var featureKey = PermissionFeatures.NormalizeFeature(feature);
        var actionKey = PermissionActions.NormalizeAction(action);

        if (roleId <= 0 || !PermissionFeatures.IsKnownFeature(featureKey) || !PermissionActions.IsKnownAction(actionKey))
        {
            return false;
        }

        using var context = new CaPheDbContext();
        var permission = QueryPermissionByRoleAndFeature(context, roleId, featureKey);

        if (permission == null)
        {
            return false;
        }

        return CoActionDuocPhep(permission, actionKey);
    }

    public bool DongBoDuLieuQuyenMacDinh()
    {
        using var context = new CaPheDbContext();
        return DamBaoDuLieuQuyenMacDinh(context);
    }

    public List<string> LayDanhSachTenVaiTro()
    {
        using var context = new CaPheDbContext();
        return QueryDanhSachVaiTroRows(context)
            .Select(x => x.TenVaiTro)
            .ToList();
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
            .ToArray();
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
        var targetRole = QueryVaiTroTheoTen(context, targetRoleName);

        if (targetRole == null)
        {
            return false;
        }

        var dsQuyen = QueryDanhSachQuyenTheoRoleIds(context, new[] { currentRoleId, targetRole.ID });

        return CoTheGanVaiTroNoiBo(currentRoleId, targetRole.ID, dsQuyen);
    }

    private static PermissionReadModel? QueryPermissionByRoleAndFeature(CaPheDbContext context, int roleId, string feature)
    {
        var featureKey = NormalizeKey(feature);
        if (featureKey.Length == 0)
        {
            return null;
        }

        return context.Permission
            .AsNoTracking()
            .Where(x => x.VaiTroID == roleId)
            .Select(x => new PermissionReadModel
            {
                VaiTroID = x.VaiTroID,
                Feature = x.Feature,
                CanView = x.CanView,
                CanCreate = x.CanCreate,
                CanUpdate = x.CanUpdate,
                CanDelete = x.CanDelete
            })
            .AsEnumerable()
            .FirstOrDefault(x => NormalizeKey(x.Feature) == featureKey);
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
        var roleKey = NormalizeKey(tenVaiTro);
        if (roleKey.Length == 0)
        {
            return null;
        }

        return context.VaiTro
            .AsNoTracking()
            .Select(x => new VaiTroReadModel
            {
                ID = x.ID,
                TenVaiTro = x.TenVaiTro
            })
            .AsEnumerable()
            .FirstOrDefault(x => NormalizeKey(x.TenVaiTro) == roleKey);
    }

    private static List<PermissionReadModel> QueryDanhSachQuyenTheoRoleIds(CaPheDbContext context, IEnumerable<int> roleIds)
    {
        var roleIdArray = roleIds.Distinct().ToArray();
        if (roleIdArray.Length == 0)
        {
            return new List<PermissionReadModel>();
        }

        return context.Permission
            .AsNoTracking()
            .Where(x => roleIdArray.Contains(x.VaiTroID))
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
        return NormalizeKey(PermissionActions.NormalizeAction(action)) switch
        {
            "VIEW" => permission.CanView,
            "CREATE" => permission.CanCreate,
            "UPDATE" => permission.CanUpdate,
            "DELETE" => permission.CanDelete,
            _ => false
        };
    }

    private static bool CoTheGanVaiTroNoiBo(int currentRoleId, int targetRoleId, IEnumerable<PermissionReadModel> dsQuyen)
    {
        var quyenNguoiGan = dsQuyen
            .Where(x => x.VaiTroID == currentRoleId)
            .GroupBy(x => NormalizeKey(x.Feature), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        var quyenVaiTroDich = dsQuyen
            .Where(x => x.VaiTroID == targetRoleId)
            .ToList();

        foreach (var quyenDich in quyenVaiTroDich)
        {
            var featureKey = NormalizeKey(quyenDich.Feature);
            if (!quyenNguoiGan.TryGetValue(featureKey, out var quyenNguoiDangDangNhap))
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

    private static bool DamBaoDuLieuQuyenMacDinh(CaPheDbContext context)
    {
        var dsVaiTro = QueryDanhSachVaiTroRows(context);
        if (dsVaiTro.Count == 0)
        {
            return false;
        }

        var roleIds = dsVaiTro.Select(x => x.ID).ToArray();
        var dsQuyenHienCo = context.Permission
            .Where(x => roleIds.Contains(x.VaiTroID))
            .ToList();

        var quyenTheoRoleFeature = dsQuyenHienCo
            .GroupBy(x => ComposeRoleFeatureKey(x.VaiTroID, x.Feature), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        var daThayDoi = false;

        foreach (var vaiTro in dsVaiTro)
        {
            var mauQuyen = LayMauQuyenTheoVaiTro(vaiTro.TenVaiTro);
            if (mauQuyen.Count == 0)
            {
                continue;
            }

            foreach (var (feature, template) in mauQuyen)
            {
                var key = ComposeRoleFeatureKey(vaiTro.ID, feature);
                if (quyenTheoRoleFeature.TryGetValue(key, out var quyenHienCo))
                {
                    if (quyenHienCo.CanView == template.CanView
                        && quyenHienCo.CanCreate == template.CanCreate
                        && quyenHienCo.CanUpdate == template.CanUpdate
                        && quyenHienCo.CanDelete == template.CanDelete)
                    {
                        continue;
                    }

                    quyenHienCo.CanView = template.CanView;
                    quyenHienCo.CanCreate = template.CanCreate;
                    quyenHienCo.CanUpdate = template.CanUpdate;
                    quyenHienCo.CanDelete = template.CanDelete;
                    daThayDoi = true;
                    continue;
                }

                var quyenMoi = new dtaPermission
                {
                    VaiTroID = vaiTro.ID,
                    Feature = feature,
                    CanView = template.CanView,
                    CanCreate = template.CanCreate,
                    CanUpdate = template.CanUpdate,
                    CanDelete = template.CanDelete
                };

                context.Permission.Add(quyenMoi);
                quyenTheoRoleFeature[key] = quyenMoi;
                daThayDoi = true;
            }
        }

        if (!daThayDoi)
        {
            return false;
        }

        context.SaveChanges();
        return true;
    }

    private static IReadOnlyDictionary<string, PermissionTemplate> LayMauQuyenTheoVaiTro(string tenVaiTro)
    {
        if (!RoleMapper.TryParse(tenVaiTro, out var role))
        {
            return new Dictionary<string, PermissionTemplate>();
        }

        return role switch
        {
            Role.Admin => AdminPermissionTemplate,
            Role.Manager => ManagerPermissionTemplate,
            Role.Staff => StaffPermissionTemplate,
            _ => new Dictionary<string, PermissionTemplate>()
        };
    }

    private static string NormalizeKey(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        return input.Trim().ToUpperInvariant();
    }

    private static string ComposeRoleFeatureKey(int roleId, string? feature)
    {
        return $"{roleId}:{NormalizeKey(feature)}";
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

    private sealed class PermissionTemplate
    {
        public PermissionTemplate(bool canView, bool canCreate, bool canUpdate, bool canDelete)
        {
            CanView = canView;
            CanCreate = canCreate;
            CanUpdate = canUpdate;
            CanDelete = canDelete;
        }

        public bool CanView { get; }
        public bool CanCreate { get; }
        public bool CanUpdate { get; }
        public bool CanDelete { get; }
    }
}
