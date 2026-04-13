using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Audit;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.DependencyInjection;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.DAL;

public class NguyenLieuDAL : INguyenLieuRepository
{
    private readonly IActivityLogWriter _activityLogWriter;

    public NguyenLieuDAL(IActivityLogWriter? activityLogWriter = null)
    {
        _activityLogWriter = AppServiceProvider.Resolve(activityLogWriter, () => new ActivityLogService());
    }

    public List<NguyenLieuDTO> GetDanhSachNguyenLieu(string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var nguyenLieuRows = QueryDanhSachNguyenLieuRows(context, tuKhoa);
        return MapNguyenLieuDtos(nguyenLieuRows);
    }

    public List<NguyenLieuDTO> GetDanhSachNguyenLieuSapHet()
    {
        using var context = new CaPheDbContext();

        var nguyenLieuRows = QueryDanhSachNguyenLieuSapHetRows(context);

        return MapNguyenLieuDtos(nguyenLieuRows);
    }

    public int GetNextNguyenLieuId()
    {
        using var context = new CaPheDbContext();
        return (context.Set<dtaNguyenLieu>().Max(x => (int?)x.ID) ?? 0) + 1;
    }

    public NguyenLieuDTO ThemNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        using var context = new CaPheDbContext();

        var nguyenLieu = new dtaNguyenLieu
        {
            TenNguyenLieu = nguyenLieuDTO.TenNguyenLieu,
            DonViTinh = nguyenLieuDTO.DonViTinh,
            SoLuongTon = nguyenLieuDTO.SoLuongTon,
            MucCanhBao = nguyenLieuDTO.MucCanhBao,
            GiaNhapGanNhat = nguyenLieuDTO.GiaNhapGanNhat,
            TrangThai = nguyenLieuDTO.TrangThai,
            TrangThaiTextLegacy = nguyenLieuDTO.TrangThaiHienThi
        };

        context.Set<dtaNguyenLieu>().Add(nguyenLieu);
        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.CreateIngredient,
            entity: "NguyenLieu",
            entityId: nguyenLieu.ID.ToString(),
            description: $"Đã thêm nguyên liệu {nguyenLieu.TenNguyenLieu}.",
            oldValue: null,
            newValue: TaoNguyenLieuSnapshot(nguyenLieu),
            performedBy: nguoiDung?.TenDangNhap);

        nguyenLieuDTO.MaNguyenLieu = nguyenLieu.ID;
        return nguyenLieuDTO;
    }

    public bool CapNhatNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        using var context = new CaPheDbContext();

        var nguyenLieu = context.Set<dtaNguyenLieu>().FirstOrDefault(x => x.ID == nguyenLieuDTO.MaNguyenLieu);
        if (nguyenLieu == null)
        {
            return false;
        }

        var oldSnapshot = TaoNguyenLieuSnapshot(nguyenLieu);
        var tenCu = nguyenLieu.TenNguyenLieu;
        var donViTinhCu = nguyenLieu.DonViTinh;
        var soLuongTonCu = nguyenLieu.SoLuongTon;
        var mucCanhBaoCu = nguyenLieu.MucCanhBao;
        var giaNhapGanNhatCu = nguyenLieu.GiaNhapGanNhat;
        var trangThaiCu = nguyenLieu.TrangThai;

        nguyenLieu.TenNguyenLieu = nguyenLieuDTO.TenNguyenLieu;
        nguyenLieu.DonViTinh = nguyenLieuDTO.DonViTinh;
        nguyenLieu.SoLuongTon = nguyenLieuDTO.SoLuongTon;
        nguyenLieu.MucCanhBao = nguyenLieuDTO.MucCanhBao;
        nguyenLieu.GiaNhapGanNhat = nguyenLieuDTO.GiaNhapGanNhat;
        nguyenLieu.TrangThai = nguyenLieuDTO.TrangThai;
        nguyenLieu.TrangThaiTextLegacy = nguyenLieuDTO.TrangThaiHienThi;

        context.SaveChanges();

        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        _activityLogWriter.Log(
            userId: nguoiDung?.UserId,
            action: AuditActions.UpdateIngredient,
            entity: "NguyenLieu",
            entityId: nguyenLieu.ID.ToString(),
            description: TaoMoTaCapNhatNguyenLieu(
                nguyenLieu.ID,
                tenCu,
                nguyenLieu.TenNguyenLieu,
                donViTinhCu,
                nguyenLieu.DonViTinh,
                soLuongTonCu,
                nguyenLieu.SoLuongTon,
                mucCanhBaoCu,
                nguyenLieu.MucCanhBao,
                giaNhapGanNhatCu,
                nguyenLieu.GiaNhapGanNhat,
                trangThaiCu,
                nguyenLieu.TrangThai),
            oldValue: oldSnapshot,
            newValue: TaoNguyenLieuSnapshot(nguyenLieu),
            performedBy: nguoiDung?.TenDangNhap);

        return true;
    }

    public OperationResult XoaNguyenLieu(int maNguyenLieu)
    {
        using var context = new CaPheDbContext();

        var nguyenLieu = context.Set<dtaNguyenLieu>().FirstOrDefault(x => x.ID == maNguyenLieu);
        if (nguyenLieu == null)
        {
            return OperationResult.Failure("Không tìm thấy nguyên liệu để xóa.");
        }

        var oldSnapshot = TaoNguyenLieuSnapshot(nguyenLieu);
        var nguoiDung = NguoiDungHienTaiService.LayNguoiDungDangNhap();

        if (CoDuLieuPhatSinhKhoHoacCongThuc(context, maNguyenLieu))
        {
            ChuyenSangTrangThaiNgungDung(nguyenLieu);
            context.SaveChanges();

            _activityLogWriter.Log(
                userId: nguoiDung?.UserId,
                action: AuditActions.DeleteIngredient,
                entity: "NguyenLieu",
                entityId: nguyenLieu.ID.ToString(),
                description: $"Không thể xóa cứng nguyên liệu {nguyenLieu.TenNguyenLieu}; hệ thống đã chuyển sang trạng thái ngừng dùng.",
                oldValue: oldSnapshot,
                newValue: TaoNguyenLieuSnapshot(nguyenLieu),
                performedBy: nguoiDung?.TenDangNhap);

            return OperationResult.Success("Nguyên liệu đã phát sinh dữ liệu kho/công thức. Hệ thống chuyển sang trạng thái ngừng dùng.");
        }

        context.Set<dtaNguyenLieu>().Remove(nguyenLieu);

        try
        {
            context.SaveChanges();

            _activityLogWriter.Log(
                userId: nguoiDung?.UserId,
                action: AuditActions.DeleteIngredient,
                entity: "NguyenLieu",
                entityId: maNguyenLieu.ToString(),
                description: $"Đã xóa nguyên liệu {nguyenLieu.TenNguyenLieu}.",
                oldValue: oldSnapshot,
                newValue: new
                {
                    DeletedPermanently = true,
                    NguyenLieuId = maNguyenLieu
                },
                performedBy: nguoiDung?.TenDangNhap);

            return OperationResult.Success("Xóa nguyên liệu thành công.");
        }
        catch (DbUpdateException ex) when (LaLoiRangBuocKhoaNgoai(ex))
        {
            AppLogger.Warning(
                $"Delete ingredient blocked by FK. NguyenLieuID={maNguyenLieu}. Fallback to inactive status.",
                nameof(NguyenLieuDAL));

            using var fallbackContext = new CaPheDbContext();
            var nguyenLieuCanCapNhat = fallbackContext.Set<dtaNguyenLieu>().FirstOrDefault(x => x.ID == maNguyenLieu);
            if (nguyenLieuCanCapNhat == null)
            {
                return OperationResult.Failure("Không tìm thấy nguyên liệu để xóa.");
            }

            ChuyenSangTrangThaiNgungDung(nguyenLieuCanCapNhat);
            fallbackContext.SaveChanges();

            _activityLogWriter.Log(
                userId: nguoiDung?.UserId,
                action: AuditActions.DeleteIngredient,
                entity: "NguyenLieu",
                entityId: nguyenLieuCanCapNhat.ID.ToString(),
                description: $"Xóa nguyên liệu {nguyenLieuCanCapNhat.TenNguyenLieu} bị chặn bởi ràng buộc dữ liệu; hệ thống đã chuyển sang ngừng dùng.",
                oldValue: oldSnapshot,
                newValue: TaoNguyenLieuSnapshot(nguyenLieuCanCapNhat),
                performedBy: nguoiDung?.TenDangNhap);

            return OperationResult.Success("Nguyên liệu đã phát sinh dữ liệu kho/công thức. Hệ thống chuyển sang trạng thái ngừng dùng.");
        }
    }

    public (bool ThanhCong, string ThongBao) NhapKho(int maNguyenLieu, decimal soLuongNhap, decimal giaNhap, string? ghiChu)
    {
        return NhapKhoNhieuNguyenLieu(
            new[]
            {
                new NhapKhoChiTietDTO
                {
                    NguyenLieuID = maNguyenLieu,
                    SoLuong = soLuongNhap,
                    DonGiaNhap = giaNhap
                }
            },
            ghiChu);
    }

    public (bool ThanhCong, string ThongBao) NhapKhoNhieuNguyenLieu(IEnumerable<NhapKhoChiTietDTO> dsChiTiet, string? ghiChu)
    {
        using var correlationScope = CorrelationContext.BeginScope();

        if (dsChiTiet == null)
        {
            return (false, "Danh sách chi tiết nhập kho không hợp lệ.");
        }

        var dsChiTietDaChuanHoa = dsChiTiet
            .Where(x => x != null)
            .Select(x => new NhapKhoChiTietDTO
            {
                NguyenLieuID = x.NguyenLieuID,
                SoLuong = decimal.Round(x.SoLuong, 3, MidpointRounding.AwayFromZero),
                DonGiaNhap = decimal.Round(x.DonGiaNhap, 2, MidpointRounding.AwayFromZero)
            })
            .ToList();

        if (dsChiTietDaChuanHoa.Count == 0)
        {
            return (false, "Phiếu nhập kho phải có ít nhất 1 dòng chi tiết.");
        }

        if (dsChiTietDaChuanHoa.Any(x => x.NguyenLieuID <= 0 || x.SoLuong <= 0 || x.DonGiaNhap < 0))
        {
            return (false, "Có dòng chi tiết nhập kho không hợp lệ.");
        }

        var coNguyenLieuTrung = dsChiTietDaChuanHoa
            .GroupBy(x => x.NguyenLieuID)
            .Any(g => g.Count() > 1);
        if (coNguyenLieuTrung)
        {
            return (false, "Một nguyên liệu chỉ được xuất hiện 1 lần trong cùng phiếu nhập.");
        }

        var nguoiDungDangNhap = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        if (nguoiDungDangNhap == null || nguoiDungDangNhap.NhanVienId <= 0)
        {
            return (false, "Không xác định được nhân viên đang thao tác. Vui lòng đăng nhập lại.");
        }

        var nhanVienId = nguoiDungDangNhap.NhanVienId;
        var dsNguyenLieuId = dsChiTietDaChuanHoa
            .Select(x => x.NguyenLieuID)
            .Distinct()
            .ToArray();

        AppLogger.Info($"Start NhapKhoNhieuNguyenLieu. SoDong={dsChiTietDaChuanHoa.Count}.", nameof(NguyenLieuDAL));
        AppLogger.Audit(
            "Inventory.Import.Start",
            "Bat dau nhap kho.",
            new
            {
                SoDong = dsChiTietDaChuanHoa.Count,
                NguyenLieuIds = dsNguyenLieuId,
                NhanVienId = nhanVienId,
                TongSoLuong = dsChiTietDaChuanHoa.Sum(x => x.SoLuong)
            },
            nameof(NguyenLieuDAL));

        try
        {
            return ExecutionStrategyTransactionRunner.ExecuteAsync(async context =>
            {
                var nhanVienTonTai = await context.NhanVien
                    .AsNoTracking()
                    .AnyAsync(x => x.ID == nhanVienId)
                    .ConfigureAwait(false);
                if (!nhanVienTonTai)
                {
                    AppLogger.Audit(
                        "Inventory.Import.Rejected",
                        "Nhan vien thao tac khong hop le.",
                        new { NhanVienId = nhanVienId },
                        nameof(NguyenLieuDAL));
                    return (false, "Tài khoản đăng nhập không liên kết nhân viên hợp lệ. Vui lòng liên hệ quản trị viên.");
                }

                var dsNguyenLieu = await context.NguyenLieu
                    .Where(x => dsNguyenLieuId.Contains(x.ID))
                    .ToDictionaryAsync(x => x.ID)
                    .ConfigureAwait(false);

                if (dsNguyenLieu.Count != dsNguyenLieuId.Length)
                {
                    var dsThieu = dsNguyenLieuId
                        .Where(x => !dsNguyenLieu.ContainsKey(x))
                        .ToArray();

                    AppLogger.Audit(
                        "Inventory.Import.Rejected",
                        "Khong tim thay nguyen lieu de nhap kho.",
                        new { NguyenLieuIdsKhongTonTai = dsThieu },
                        nameof(NguyenLieuDAL));
                    return (false, "Không tìm thấy nguyên liệu để nhập kho.");
                }

                var tonKhoTruoc = dsNguyenLieu.ToDictionary(x => x.Key, x => x.Value.SoLuongTon);

                var phieuNhap = new dtaPhieuNhapKho
                {
                    NgayNhap = DateTime.Now,
                    NhanVienID = nhanVienId,
                    GhiChu = string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu.Trim()
                };

                context.PhieuNhapKho.Add(phieuNhap);

                foreach (var chiTiet in dsChiTietDaChuanHoa)
                {
                    var nguyenLieu = dsNguyenLieu[chiTiet.NguyenLieuID];

                    context.ChiTietPhieuNhap.Add(new dtaChiTietPhieuNhap
                    {
                        PhieuNhap = phieuNhap,
                        NguyenLieuID = chiTiet.NguyenLieuID,
                        SoLuong = chiTiet.SoLuong,
                        DonGiaNhap = chiTiet.DonGiaNhap,
                        ThanhTien = decimal.Round(
                            chiTiet.SoLuong * chiTiet.DonGiaNhap,
                            2,
                            MidpointRounding.AwayFromZero)
                    });

                    nguyenLieu.SoLuongTon += chiTiet.SoLuong;
                    nguyenLieu.GiaNhapGanNhat = chiTiet.DonGiaNhap;
                    nguyenLieu.TrangThai = TinhTrangThaiNguyenLieu(nguyenLieu.SoLuongTon, nguyenLieu.MucCanhBao, nguyenLieu.TrangThai);
                    nguyenLieu.TrangThaiTextLegacy = ChuyenTrangThaiNguyenLieuTextLegacy(nguyenLieu.TrangThai, nguyenLieu.SoLuongTon);
                }

                await context.SaveChangesAsync().ConfigureAwait(false);

                AppLogger.Audit(
                    "Inventory.Import.Success",
                    "Nhap kho thanh cong.",
                    new
                    {
                        SoDong = dsChiTietDaChuanHoa.Count,
                        NhanVienId = nhanVienId,
                        PhieuNhapId = phieuNhap.ID,
                        ChiTiet = dsChiTietDaChuanHoa.Select(x => new
                        {
                            x.NguyenLieuID,
                            x.SoLuong,
                            x.DonGiaNhap,
                            SoLuongTonTruoc = tonKhoTruoc[x.NguyenLieuID],
                            SoLuongTonSau = dsNguyenLieu[x.NguyenLieuID].SoLuongTon
                        }).ToList()
                    },
                    nameof(NguyenLieuDAL));

                return (true, "Nhập kho thành công.");
            }, result => result.Item1).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            var mappedError = AppExceptionMapper.Map(ex);
            AppLogger.Error(
                ex,
                "Unexpected failure in NhapKhoNhieuNguyenLieu.",
                nameof(NguyenLieuDAL),
                mappedError.Code);
            AppLogger.Audit(
                "Inventory.Import.Failed",
                "Nhap kho that bai do loi he thong.",
                new
                {
                    SoDong = dsChiTietDaChuanHoa.Count,
                    NguyenLieuIds = dsNguyenLieuId,
                    NhanVienId = nhanVienId
                },
                nameof(NguyenLieuDAL));

            var thongBao = AppExceptionHandler.CreateUserMessage(
                "Không thể nhập kho do xảy ra lỗi trong quá trình lưu dữ liệu kho.",
                ex);
            return (false, thongBao);
        }
    }

    private static object TaoNguyenLieuSnapshot(dtaNguyenLieu nguyenLieu)
    {
        return new
        {
            nguyenLieu.ID,
            nguyenLieu.TenNguyenLieu,
            nguyenLieu.DonViTinh,
            nguyenLieu.SoLuongTon,
            nguyenLieu.MucCanhBao,
            nguyenLieu.GiaNhapGanNhat,
            nguyenLieu.TrangThai,
            nguyenLieu.TrangThaiTextLegacy
        };
    }

    private static string TaoMoTaCapNhatNguyenLieu(
        int id,
        string tenCu,
        string tenMoi,
        string donViTinhCu,
        string donViTinhMoi,
        decimal soLuongTonCu,
        decimal soLuongTonMoi,
        decimal mucCanhBaoCu,
        decimal mucCanhBaoMoi,
        decimal giaNhapCu,
        decimal giaNhapMoi,
        int trangThaiCu,
        int trangThaiMoi)
    {
        var moTa = $"Đã cập nhật nguyên liệu {tenMoi} (ID: {id}).";

        if (!string.Equals(tenCu, tenMoi, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Tên: {tenCu} -> {tenMoi}.";
        }

        if (!string.Equals(donViTinhCu, donViTinhMoi, StringComparison.OrdinalIgnoreCase))
        {
            moTa += $" Đơn vị tính: {donViTinhCu} -> {donViTinhMoi}.";
        }

        if (soLuongTonCu != soLuongTonMoi)
        {
            moTa += $" Tồn kho: {soLuongTonCu:N3} -> {soLuongTonMoi:N3}.";
        }

        if (mucCanhBaoCu != mucCanhBaoMoi)
        {
            moTa += $" Mức cảnh báo: {mucCanhBaoCu:N3} -> {mucCanhBaoMoi:N3}.";
        }

        if (giaNhapCu != giaNhapMoi)
        {
            moTa += $" Giá nhập gần nhất: {giaNhapCu:N0} -> {giaNhapMoi:N0}.";
        }

        if (trangThaiCu != trangThaiMoi)
        {
            moTa += $" Trạng thái: {ChuyenTrangThaiNguyenLieuTextLegacy(trangThaiCu, soLuongTonCu)} -> {ChuyenTrangThaiNguyenLieuTextLegacy(trangThaiMoi, soLuongTonMoi)}.";
        }

        return moTa;
    }

    private static List<NguyenLieuReadModel> QueryDanhSachNguyenLieuSapHetRows(CaPheDbContext context)
    {
        return context.Set<dtaNguyenLieu>()
            .AsNoTracking()
            .Where(x => x.SoLuongTon < x.MucCanhBao)
            .OrderBy(x => x.SoLuongTon)
            .ThenBy(x => x.TenNguyenLieu)
            .Select(x => new NguyenLieuReadModel
            {
                MaNguyenLieu = x.ID,
                TenNguyenLieu = x.TenNguyenLieu,
                DonViTinh = x.DonViTinh,
                SoLuongTon = x.SoLuongTon,
                MucCanhBao = x.MucCanhBao,
                GiaNhapGanNhat = x.GiaNhapGanNhat,
                TrangThai = x.TrangThai
            })
            .ToList();
    }

    private static List<NguyenLieuReadModel> QueryDanhSachNguyenLieuRows(CaPheDbContext context, string? tuKhoa)
    {
        var query = context.Set<dtaNguyenLieu>()
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            var keywordPattern = $"%{keyword}%";
            var hasKeywordId = int.TryParse(keyword, out var keywordId);
            var hasKeywordTrangThai = int.TryParse(keyword, out var keywordTrangThai);

            var matchNgungDung = "Ngừng dùng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchSapHet = "Sắp hết".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchHetHang = "Hết hàng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
            var matchDangSuDung = "Đang sử dụng".Contains(keyword, StringComparison.OrdinalIgnoreCase);

            query = query.Where(x =>
                (hasKeywordId && x.ID == keywordId)
                || EF.Functions.Like(x.TenNguyenLieu, keywordPattern)
                || EF.Functions.Like(x.DonViTinh, keywordPattern)
                || (hasKeywordTrangThai && x.TrangThai == keywordTrangThai)
                || (matchNgungDung && x.TrangThai == 0)
                || (matchSapHet && x.TrangThai == 2 && x.SoLuongTon > 0)
                || (matchHetHang && x.TrangThai != 0 && x.SoLuongTon <= 0)
                || (matchDangSuDung && x.TrangThai != 0 && x.TrangThai != 2 && x.SoLuongTon > 0));
        }

        return query
            .OrderBy(x => x.ID)
            .Select(x => new NguyenLieuReadModel
            {
                MaNguyenLieu = x.ID,
                TenNguyenLieu = x.TenNguyenLieu,
                DonViTinh = x.DonViTinh,
                SoLuongTon = x.SoLuongTon,
                MucCanhBao = x.MucCanhBao,
                GiaNhapGanNhat = x.GiaNhapGanNhat,
                TrangThai = x.TrangThai
            })
            .ToList();
    }

    private static List<NguyenLieuDTO> MapNguyenLieuDtos(IEnumerable<NguyenLieuReadModel> nguyenLieuRows)
    {
        return nguyenLieuRows
            .Select(MapNguyenLieuDto)
            .ToList();
    }

    private static NguyenLieuDTO MapNguyenLieuDto(NguyenLieuReadModel nguyenLieuRow)
    {
        return new NguyenLieuDTO
        {
            MaNguyenLieu = nguyenLieuRow.MaNguyenLieu,
            TenNguyenLieu = nguyenLieuRow.TenNguyenLieu,
            DonViTinh = nguyenLieuRow.DonViTinh,
            SoLuongTon = nguyenLieuRow.SoLuongTon,
            MucCanhBao = nguyenLieuRow.MucCanhBao,
            GiaNhapGanNhat = nguyenLieuRow.GiaNhapGanNhat,
            TrangThai = nguyenLieuRow.TrangThai
        };
    }

    private static int TinhTrangThaiNguyenLieu(decimal soLuongTon, decimal mucCanhBao, int trangThaiHienTai)
    {
        if (trangThaiHienTai == 0)
        {
            return 0;
        }

        if (soLuongTon <= 0)
        {
            return 2;
        }

        if (soLuongTon <= mucCanhBao)
        {
            return 2;
        }

        return 1;
    }

    private static string ChuyenTrangThaiNguyenLieuTextLegacy(int trangThai, decimal soLuongTon)
    {
        return trangThai switch
        {
            0 => "Ngừng dùng",
            2 => soLuongTon <= 0 ? "Hết hàng" : "Sắp hết",
            _ => soLuongTon <= 0 ? "Hết hàng" : "Đang sử dụng"
        };
    }

    private static bool CoDuLieuPhatSinhKhoHoacCongThuc(CaPheDbContext context, int maNguyenLieu)
    {
        return context.ChiTietPhieuNhap.Any(x => x.NguyenLieuID == maNguyenLieu)
            || context.ChiTietPhieuXuat.Any(x => x.NguyenLieuID == maNguyenLieu)
            || context.CongThucMon.Any(x => x.NguyenLieuID == maNguyenLieu);
    }

    private static void ChuyenSangTrangThaiNgungDung(dtaNguyenLieu nguyenLieu)
    {
        nguyenLieu.TrangThai = 0;
        nguyenLieu.TrangThaiTextLegacy = "Ngừng dùng";
    }

    private static bool LaLoiRangBuocKhoaNgoai(DbUpdateException exception)
    {
        var message = string.Concat(exception.Message, " ", exception.InnerException?.Message);
        return message.Contains("REFERENCE constraint", StringComparison.OrdinalIgnoreCase)
            || message.Contains("FOREIGN KEY constraint", StringComparison.OrdinalIgnoreCase)
            || message.Contains("FOREIGN KEY constraint failed", StringComparison.OrdinalIgnoreCase)
            || message.Contains("FK_ChiTietPhieuXuat_NguyenLieu_NguyenLieuID", StringComparison.OrdinalIgnoreCase)
            || message.Contains("FK_ChiTietPhieuNhap_NguyenLieu_NguyenLieuID", StringComparison.OrdinalIgnoreCase)
            || message.Contains("FK_PhieuXuatKho_NguyenLieu_NguyenLieuID", StringComparison.OrdinalIgnoreCase)
            || message.Contains("FK_CongThucMon_NguyenLieu_NguyenLieuID", StringComparison.OrdinalIgnoreCase)
            || message.Contains("FK_PhieuNhapKho_NguyenLieu_NguyenLieuID", StringComparison.OrdinalIgnoreCase);
    }

    private sealed class NguyenLieuReadModel
    {
        public int MaNguyenLieu { get; init; }
        public string TenNguyenLieu { get; init; } = string.Empty;
        public string DonViTinh { get; init; } = string.Empty;
        public decimal SoLuongTon { get; init; }
        public decimal MucCanhBao { get; init; }
        public decimal GiaNhapGanNhat { get; init; }
        public int TrangThai { get; init; }
    }
}
