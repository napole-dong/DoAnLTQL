using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.DAL;

public class BanDAL
{
    public const string TenBanMangDi = "Mang đi";
    private const string InvoiceConcurrencyMessage = "Dữ liệu đã bị thay đổi bởi nhân viên khác. Vui lòng tải lại!";

    public static bool LaBanMangDi(string? tenBan)
    {
        return string.Equals(tenBan?.Trim(), TenBanMangDi, StringComparison.OrdinalIgnoreCase);
    }

    public BanThongKeDTO GetThongKe()
    {
        using var context = new CaPheDbContext();

        var banRows = context.Ban
            .AsNoTracking()
            .Where(b => b.TenBan != TenBanMangDi)
            .Select(b => new BanReadModel
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai,
                TrangThaiHoaDonDangHoatDong = context.HoaDon
                    .Where(i => i.BanID == b.ID
                        && i.TrangThai != (int)HoaDonTrangThai.Closed
                        && i.TrangThai != (int)HoaDonTrangThai.Cancelled)
                    .OrderByDescending(i => i.NgayLap)
                    .ThenByDescending(i => i.ID)
                    .Select(i => (int?)i.TrangThai)
                    .FirstOrDefault()
            })
            .ToList();

        var tongBan = banRows.Count;
        var banTrong = banRows.Count(b => XacDinhTrangThaiHienThiBan(b) == 0);
        var banDangPhucVu = banRows.Count(b => XacDinhTrangThaiHienThiBan(b) == 1);
        var banChoDon = banRows.Count(b => XacDinhTrangThaiHienThiBan(b) == 2);

        return new BanThongKeDTO
        {
            TongBan = tongBan,
            BanDangPhucVu = banDangPhucVu,
            BanTrong = banTrong,
            BanDatTruoc = banChoDon
        };
    }

    public List<BanDTO> GetDanhSachBan(string? khuVuc, string? trangThai, string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var query = context.Ban
            .AsNoTracking()
            .Where(b => b.TenBan != TenBanMangDi)
            .AsQueryable();

        query = ApplyKhuVucFilter(query, khuVuc);
        query = ApplyTrangThaiFilter(query, context, trangThai);
        query = ApplyTuKhoaFilter(query, context, tuKhoa);

        var banRows = query
            .OrderBy(b => b.ID)
            .Select(b => new BanReadModel
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai,
                TrangThaiHoaDonDangHoatDong = context.HoaDon
                    .Where(i => i.BanID == b.ID
                        && i.TrangThai != (int)HoaDonTrangThai.Closed
                        && i.TrangThai != (int)HoaDonTrangThai.Cancelled)
                    .OrderByDescending(i => i.NgayLap)
                    .ThenByDescending(i => i.ID)
                    .Select(i => (int?)i.TrangThai)
                    .FirstOrDefault()
            })
            .ToList();

        return MapBanDtos(banRows);
    }

    public List<BanDTO> GetSoDoBan()
    {
        using var context = new CaPheDbContext();

        var banRows = context.Ban
            .AsNoTracking()
            .Where(b => b.TenBan != TenBanMangDi)
            .OrderBy(b => b.ID)
            .Select(b => new BanReadModel
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai,
                TrangThaiHoaDonDangHoatDong = context.HoaDon
                    .Where(i => i.BanID == b.ID
                        && i.TrangThai != (int)HoaDonTrangThai.Closed
                        && i.TrangThai != (int)HoaDonTrangThai.Cancelled)
                    .OrderByDescending(i => i.NgayLap)
                    .ThenByDescending(i => i.ID)
                    .Select(i => (int?)i.TrangThai)
                    .FirstOrDefault()
            })
            .ToList();

        return MapBanDtos(banRows);
    }

    public int LayHoacTaoBanMangDi()
    {
        using var context = new CaPheDbContext();

        var banMangDi = context.Ban
            .FirstOrDefault(x => x.TenBan == TenBanMangDi);

        if (banMangDi == null)
        {
            banMangDi = new dtaBan
            {
                TenBan = TenBanMangDi,
                TrangThai = 0
            };

            context.Ban.Add(banMangDi);
            context.SaveChanges();
            return banMangDi.ID;
        }

        if (banMangDi.TrangThai != 0)
        {
            var hoaDonDangHoatDong = context.HoaDon
                .Where(x => x.BanID == banMangDi.ID
                    && x.TrangThai != (int)HoaDonTrangThai.Closed
                    && x.TrangThai != (int)HoaDonTrangThai.Cancelled)
                .OrderByDescending(x => x.NgayLap)
                .ThenByDescending(x => x.ID)
                .FirstOrDefault();

            if (hoaDonDangHoatDong == null)
            {
                banMangDi.TrangThai = 0;
                context.SaveChanges();
            }
            else if (hoaDonDangHoatDong.TrangThai == (int)HoaDonTrangThai.Paid)
            {
                hoaDonDangHoatDong.TrangThai = (int)HoaDonTrangThai.Closed;
                banMangDi.TrangThai = 0;
                context.SaveChanges();
            }
        }

        return banMangDi.ID;
    }

    public bool TenBanDaTonTai(string tenBan)
    {
        using var context = new CaPheDbContext();
        return context.Ban.Any(x => x.TenBan == tenBan);
    }

    public void ThemBan(string tenBan)
    {
        using var context = new CaPheDbContext();
        context.Ban.Add(new dtaBan
        {
            TenBan = tenBan,
            TrangThai = 0
        });
        context.SaveChanges();
    }

    public BanDTO? GetBanById(int banId)
    {
        using var context = new CaPheDbContext();

        var banRow = context.Ban
            .AsNoTracking()
            .Where(x => x.ID == banId)
            .Select(x => new BanReadModel
            {
                ID = x.ID,
                TenBan = x.TenBan,
                TrangThai = x.TrangThai,
                TrangThaiHoaDonDangHoatDong = context.HoaDon
                    .Where(i => i.BanID == x.ID
                        && i.TrangThai != (int)HoaDonTrangThai.Closed
                        && i.TrangThai != (int)HoaDonTrangThai.Cancelled)
                    .OrderByDescending(i => i.NgayLap)
                    .ThenByDescending(i => i.ID)
                    .Select(i => (int?)i.TrangThai)
                    .FirstOrDefault()
            })
            .FirstOrDefault();

        return banRow == null ? null : MapBanDto(banRow);
    }

    public BanActionResultDTO DonBan(int banId)
    {
        if (banId <= 0)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Bàn cần dọn không hợp lệ."
            };
        }

        using var context = new CaPheDbContext();
        var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
        if (ban == null)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Không tìm thấy bàn cần dọn."
            };
        }

        var hoaDonDangHoatDong = context.HoaDon
            .Where(x => x.BanID == banId
                && x.TrangThai != (int)HoaDonTrangThai.Closed
                && x.TrangThai != (int)HoaDonTrangThai.Cancelled)
            .OrderByDescending(x => x.NgayLap)
            .ThenByDescending(x => x.ID)
            .FirstOrDefault();

        if (ban.TrangThai == 0 && hoaDonDangHoatDong == null)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Bàn đã trống"
            };
        }

        if (hoaDonDangHoatDong?.TrangThai == (int)HoaDonTrangThai.Draft)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = "Bàn chưa thanh toán! Hãy thanh toán hoặc hủy trước."
            };
        }

        if (hoaDonDangHoatDong?.TrangThai == (int)HoaDonTrangThai.Paid)
        {
            hoaDonDangHoatDong.TrangThai = (int)HoaDonTrangThai.Closed;
        }

        ban.TrangThai = 0;

        try
        {
            context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = InvoiceConcurrencyMessage
            };
        }

        return new BanActionResultDTO
        {
            ThanhCong = true,
            ThongBao = "Dọn bàn thành công."
        };
    }

    public bool BanDaPhatSinhHoaDon(int banId)
    {
        using var context = new CaPheDbContext();
        return context.HoaDon.Any(x => x.BanID == banId);
    }

    public bool XoaBan(int banId)
    {
        using var context = new CaPheDbContext();
        var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
        if (ban == null)
        {
            return false;
        }

        context.Ban.Remove(ban);
        context.SaveChanges();
        return true;
    }

    public List<BanDTO> GetDanhSachBanDich(int banNguonId)
    {
        using var context = new CaPheDbContext();

        var banRows = context.Ban
            .AsNoTracking()
            .Where(b => b.ID != banNguonId)
            .Where(b => b.TenBan != TenBanMangDi)
            .OrderBy(b => b.TenBan)
            .Select(b => new BanReadModel
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai,
                TrangThaiHoaDonDangHoatDong = context.HoaDon
                    .Where(i => i.BanID == b.ID
                        && i.TrangThai != (int)HoaDonTrangThai.Closed
                        && i.TrangThai != (int)HoaDonTrangThai.Cancelled)
                    .OrderByDescending(i => i.NgayLap)
                    .ThenByDescending(i => i.ID)
                    .Select(i => (int?)i.TrangThai)
                    .FirstOrDefault()
            })
            .ToList();

        return MapBanDtos(banRows);
    }

    public BanActionResultDTO ChuyenHoacGopBan(BanChuyenGopRequestDTO request)
    {
        using var correlationScope = CorrelationContext.BeginScope();
        using var strategyContext = new CaPheDbContext();
        var executionStrategy = strategyContext.Database.CreateExecutionStrategy();

        var nguoiDungDangNhap = NguoiDungHienTaiService.LayNguoiDungDangNhap();
        var userId = nguoiDungDangNhap?.UserId ?? 0;
        var role = nguoiDungDangNhap?.QuyenHan ?? RoleMapper.ToRoleName(Role.Staff);
        var thaoTac = request.LaChuyenBan ? "Table.Transfer" : "Table.Merge";

        BanActionResultDTO TuChoi(string thongBao)
        {
            AppLogger.Audit(
                $"{thaoTac}.Rejected",
                thongBao,
                new
                {
                    UserId = userId,
                    Role = role,
                    Timestamp = DateTimeOffset.UtcNow,
                    BanNguonId = request.BanNguonId,
                    BanDichId = request.BanDichId
                },
                nameof(BanDAL));

            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }

        try
        {
            return executionStrategy.ExecuteAsync(async () =>
            {
                await using var context = new CaPheDbContext();
                await using var transaction = await context.Database
                    .BeginTransactionAsync(System.Data.IsolationLevel.Serializable)
                    .ConfigureAwait(false);

                try
                {
                    var banNguon = await context.Ban
                        .FirstOrDefaultAsync(b => b.ID == request.BanNguonId)
                        .ConfigureAwait(false);
                    if (banNguon == null)
                    {
                        await transaction.RollbackAsync().ConfigureAwait(false);
                        return TuChoi("Không tìm thấy bàn nguồn.");
                    }

                    if (LaBanMangDi(banNguon.TenBan))
                    {
                        await transaction.RollbackAsync().ConfigureAwait(false);
                        return TuChoi("Không áp dụng chuyển hoặc gộp cho đơn mang đi.");
                    }

                    var hoaDonNguon = await context.HoaDon
                        .Include(h => h.HoaDon_ChiTiet)
                        .FirstOrDefaultAsync(h => h.BanID == banNguon.ID && h.TrangThai == 0)
                        .ConfigureAwait(false);
                    if (hoaDonNguon == null)
                    {
                        await transaction.RollbackAsync().ConfigureAwait(false);
                        return TuChoi("Bàn nguồn phải có hóa đơn đang mở để chuyển hoặc gộp.");
                    }

                    var banDich = await context.Ban
                        .FirstOrDefaultAsync(b => b.ID == request.BanDichId)
                        .ConfigureAwait(false);
                    if (banDich == null)
                    {
                        await transaction.RollbackAsync().ConfigureAwait(false);
                        return TuChoi("Không tìm thấy bàn đích.");
                    }

                    if (LaBanMangDi(banDich.TenBan))
                    {
                        await transaction.RollbackAsync().ConfigureAwait(false);
                        return TuChoi("Không thể chuyển hoặc gộp vào bàn mang đi.");
                    }

                    if (request.LaChuyenBan)
                    {
                        var banDichCoHoaDonMo = await context.HoaDon
                            .AsNoTracking()
                            .AnyAsync(h => h.BanID == banDich.ID && h.TrangThai == 0)
                            .ConfigureAwait(false);

                        if (banDich.TrangThai != 0 || banDichCoHoaDonMo)
                        {
                            await transaction.RollbackAsync().ConfigureAwait(false);
                            return TuChoi("Chỉ có thể chuyển sang bàn đích đang trống.");
                        }

                        hoaDonNguon.BanID = banDich.ID;
                        banNguon.TrangThai = 0;
                        banDich.TrangThai = 1;

                        await context.SaveChangesAsync().ConfigureAwait(false);
                        await transaction.CommitAsync().ConfigureAwait(false);

                        AppLogger.Audit(
                            "Table.Transfer.Success",
                            "Chuyển bàn thành công.",
                            new
                            {
                                UserId = userId,
                                Role = role,
                                Timestamp = DateTimeOffset.UtcNow,
                                BanNguonId = request.BanNguonId,
                                BanDichId = request.BanDichId,
                                HoaDonId = hoaDonNguon.ID
                            },
                            nameof(BanDAL));

                        return new BanActionResultDTO
                        {
                            ThanhCong = true,
                            ThongBao = "Chuyển bàn thành công."
                        };
                    }

                    var hoaDonDich = await context.HoaDon
                        .Include(h => h.HoaDon_ChiTiet)
                        .FirstOrDefaultAsync(h => h.BanID == banDich.ID && h.TrangThai == 0)
                        .ConfigureAwait(false);

                    if (hoaDonDich == null)
                    {
                        await transaction.RollbackAsync().ConfigureAwait(false);
                        return TuChoi("Gộp bàn yêu cầu cả bàn nguồn và bàn đích đều đang có hóa đơn chưa thanh toán.");
                    }

                    HopNhatChiTietHoaDon(context, hoaDonNguon.HoaDon_ChiTiet, hoaDonDich.ID);

                    context.HoaDon_ChiTiet.RemoveRange(hoaDonNguon.HoaDon_ChiTiet);
                    context.HoaDon.Remove(hoaDonNguon);

                    banNguon.TrangThai = 0;
                    banDich.TrangThai = 1;

                    hoaDonDich.GhiChuHoaDon = ThemGhiChuGopBan(hoaDonDich.GhiChuHoaDon, request.BanNguonId, request.BanDichId);
                    TinhLaiTongTienHoaDon(hoaDonDich);

                    await context.SaveChangesAsync().ConfigureAwait(false);
                    await transaction.CommitAsync().ConfigureAwait(false);

                    AppLogger.Audit(
                        "Table.Merge.Success",
                        "Gộp bàn thành công.",
                        new
                        {
                            UserId = userId,
                            Role = role,
                            Timestamp = DateTimeOffset.UtcNow,
                            BanNguonId = request.BanNguonId,
                            BanDichId = request.BanDichId,
                            HoaDonDichId = hoaDonDich.ID
                        },
                        nameof(BanDAL));

                    return new BanActionResultDTO
                    {
                        ThanhCong = true,
                        ThongBao = "Gộp bàn thành công."
                    };
                }
                catch
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = InvoiceConcurrencyMessage
            };
        }
        catch (Exception ex)
        {

            var mappedError = AppExceptionMapper.Map(ex);
            AppLogger.Error(
                ex,
                $"Unexpected failure in ChuyenHoacGopBan. BanNguonID={request.BanNguonId}, BanDichID={request.BanDichId}, LaChuyenBan={request.LaChuyenBan}.",
                nameof(BanDAL),
                mappedError.Code);
            AppLogger.Audit(
                $"{thaoTac}.Failed",
                "Thao tac chuyen/gop ban that bai do loi he thong.",
                new
                {
                    UserId = userId,
                    Role = role,
                    Timestamp = DateTimeOffset.UtcNow,
                    BanNguonId = request.BanNguonId,
                    BanDichId = request.BanDichId
                },
                nameof(BanDAL));

            var thongBao = AppExceptionHandler.CreateUserMessage(
                "Không thể thực hiện chuyển/gộp bàn do lỗi hệ thống.",
                ex);

            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }
    }

    private static void HopNhatChiTietHoaDon(
        CaPheDbContext context,
        IEnumerable<dtHoaDon_ChiTiet> chiTietNguon,
        int hoaDonDichId)
    {
        var chiTietDich = context.HoaDon_ChiTiet
            .Where(x => x.HoaDonID == hoaDonDichId)
            .ToList();

        foreach (var ctNguon in chiTietNguon)
        {
            var ctDich = chiTietDich.FirstOrDefault(x =>
                x.MonID == ctNguon.MonID
                && x.DonGiaBan == ctNguon.DonGiaBan
                && x.GhiChu == ctNguon.GhiChu);

            if (ctDich == null)
            {
                var chiTietMoi = new dtHoaDon_ChiTiet
                {
                    HoaDonID = hoaDonDichId,
                    MonID = ctNguon.MonID,
                    SoLuongBan = ctNguon.SoLuongBan,
                    DonGiaBan = ctNguon.DonGiaBan,
                    ThanhTien = LamTronTien(ctNguon.SoLuongBan * ctNguon.DonGiaBan),
                    GhiChu = ctNguon.GhiChu
                };

                chiTietDich.Add(chiTietMoi);
                context.HoaDon_ChiTiet.Add(chiTietMoi);
                continue;
            }

            var tongSoLuong = ctDich.SoLuongBan + ctNguon.SoLuongBan;
            ctDich.SoLuongBan = (short)Math.Clamp(tongSoLuong, 1, short.MaxValue);
            ctDich.ThanhTien = LamTronTien(ctDich.SoLuongBan * ctDich.DonGiaBan);
        }
    }

    private static void TinhLaiTongTienHoaDon(dtaHoadon hoaDon)
    {
        hoaDon.TongTien = LamTronTien(hoaDon.HoaDon_ChiTiet
            .Where(x => x.SoLuongBan > 0)
            .Sum(x => x.ThanhTien));
    }

    private static decimal LamTronTien(decimal giaTri)
    {
        return decimal.Round(giaTri, 2, MidpointRounding.AwayFromZero);
    }

    private static string ThemGhiChuGopBan(string? ghiChuHienTai, int banNguonId, int banDichId)
    {
        var ghiChuThem = $"[Gop ban {banNguonId:D2}->{banDichId:D2} {DateTime.Now:dd/MM/yyyy HH:mm}]";
        if (string.IsNullOrWhiteSpace(ghiChuHienTai))
        {
            return ghiChuThem;
        }

        return $"{ghiChuHienTai} {ghiChuThem}".Trim();
    }

    private static IQueryable<dtaBan> ApplyKhuVucFilter(IQueryable<dtaBan> query, string? khuVuc)
    {
        if (string.IsNullOrWhiteSpace(khuVuc) || khuVuc == "Tất cả khu vực")
        {
            return query;
        }

        return khuVuc switch
        {
            "Khu trong nhà" => query.Where(b =>
                b.TenBan.StartsWith("A") || b.TenBan.StartsWith("a")
                || b.TenBan.StartsWith("B") || b.TenBan.StartsWith("b")),
            "Khu sân vườn" => query.Where(b => b.TenBan.StartsWith("C") || b.TenBan.StartsWith("c")),
            "Khu phòng riêng" => query.Where(b => b.TenBan.StartsWith("D") || b.TenBan.StartsWith("d")),
            "Khu khác" => query.Where(b =>
                !(b.TenBan.StartsWith("A") || b.TenBan.StartsWith("a")
                  || b.TenBan.StartsWith("B") || b.TenBan.StartsWith("b")
                  || b.TenBan.StartsWith("C") || b.TenBan.StartsWith("c")
                  || b.TenBan.StartsWith("D") || b.TenBan.StartsWith("d"))),
            _ => query
        };
    }

    private static IQueryable<dtaBan> ApplyTrangThaiFilter(IQueryable<dtaBan> query, CaPheDbContext context, string? trangThai)
    {
        if (string.IsNullOrWhiteSpace(trangThai) || trangThai == "Tất cả trạng thái")
        {
            return query;
        }

        return trangThai switch
        {
            "Trống" => query.Where(b => b.TrangThai == 0
                && !context.HoaDon.Any(i => i.BanID == b.ID
                    && i.TrangThai != (int)HoaDonTrangThai.Closed
                    && i.TrangThai != (int)HoaDonTrangThai.Cancelled)),
            "Đang phục vụ" or "Có khách" => query.Where(b =>
                context.HoaDon.Any(i => i.BanID == b.ID && i.TrangThai == (int)HoaDonTrangThai.Draft)
                || (b.TrangThai == 1
                    && !context.HoaDon.Any(i => i.BanID == b.ID && i.TrangThai == (int)HoaDonTrangThai.Paid))),
            "Chờ dọn / Đã thanh toán" => query.Where(b =>
                b.TrangThai == 2
                || context.HoaDon.Any(i => i.BanID == b.ID && i.TrangThai == (int)HoaDonTrangThai.Paid)),
            "Đặt trước" => query.Where(b => b.TrangThai == 2),
            _ => query
        };
    }

    private static IQueryable<dtaBan> ApplyTuKhoaFilter(IQueryable<dtaBan> query, CaPheDbContext context, string? tuKhoa)
    {
        if (string.IsNullOrWhiteSpace(tuKhoa))
        {
            return query;
        }

        var keyword = tuKhoa.Trim();
        var keywordPattern = $"%{keyword}%";
        var hasKeywordId = int.TryParse(keyword, out var keywordId);

        var matchSanSang = "Sẵn sàng".Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || "Trống".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchDangPhucVu = "Đang phục vụ".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchChoDon = "Chờ dọn".Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || "Đã thanh toán".Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || "Cho don".Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || "Đặt trước".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchTrong = keyword.Equals("trống", StringComparison.OrdinalIgnoreCase);

        var matchKhuTrongNha = "Khu trong nhà".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchKhuSanVuon = "Khu sân vườn".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchKhuPhongRieng = "Khu phòng riêng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchKhuKhac = "Khu khác".Contains(keyword, StringComparison.OrdinalIgnoreCase);

        return query.Where(b =>
            (hasKeywordId && b.ID == keywordId)
            || EF.Functions.Like(b.TenBan, keywordPattern)
            || (matchSanSang && b.TrangThai == 0
                && !context.HoaDon.Any(i => i.BanID == b.ID
                    && i.TrangThai != (int)HoaDonTrangThai.Closed
                    && i.TrangThai != (int)HoaDonTrangThai.Cancelled))
            || (matchDangPhucVu && (context.HoaDon.Any(i => i.BanID == b.ID && i.TrangThai == (int)HoaDonTrangThai.Draft)
                || (b.TrangThai == 1 && !context.HoaDon.Any(i => i.BanID == b.ID && i.TrangThai == (int)HoaDonTrangThai.Paid))))
            || (matchChoDon && (b.TrangThai == 2 || context.HoaDon.Any(i => i.BanID == b.ID && i.TrangThai == (int)HoaDonTrangThai.Paid)))
            || (matchTrong && b.TrangThai == 0
                && !context.HoaDon.Any(i => i.BanID == b.ID
                    && i.TrangThai != (int)HoaDonTrangThai.Closed
                    && i.TrangThai != (int)HoaDonTrangThai.Cancelled))
            || (matchKhuTrongNha
                && (b.TenBan.StartsWith("A") || b.TenBan.StartsWith("a")
                    || b.TenBan.StartsWith("B") || b.TenBan.StartsWith("b")))
            || (matchKhuSanVuon && (b.TenBan.StartsWith("C") || b.TenBan.StartsWith("c")))
            || (matchKhuPhongRieng && (b.TenBan.StartsWith("D") || b.TenBan.StartsWith("d")))
            || (matchKhuKhac
                && !(b.TenBan.StartsWith("A") || b.TenBan.StartsWith("a")
                     || b.TenBan.StartsWith("B") || b.TenBan.StartsWith("b")
                     || b.TenBan.StartsWith("C") || b.TenBan.StartsWith("c")
                     || b.TenBan.StartsWith("D") || b.TenBan.StartsWith("d"))));
    }

    private static List<BanDTO> MapBanDtos(IEnumerable<BanReadModel> banRows)
    {
        return banRows
            .Select(MapBanDto)
            .ToList();
    }

    private static BanDTO MapBanDto(BanReadModel banRow)
    {
        var trangThaiHienThi = XacDinhTrangThaiHienThiBan(banRow);

        return new BanDTO
        {
            ID = banRow.ID,
            TenBan = banRow.TenBan,
            TrangThai = banRow.TrangThai,
            TrangThaiHoaDonDangHoatDong = banRow.TrangThaiHoaDonDangHoatDong,
            CoHoaDonDangHoatDong = banRow.TrangThaiHoaDonDangHoatDong.HasValue,
            KhuVuc = XacDinhKhuVuc(banRow.TenBan),
            TinhTrang = ChuyenTrangThaiBan(trangThaiHienThi)
        };
    }

    private sealed class BanReadModel
    {
        public int ID { get; init; }
        public string TenBan { get; init; } = string.Empty;
        public int TrangThai { get; init; }
        public int? TrangThaiHoaDonDangHoatDong { get; init; }
    }

    private static int XacDinhTrangThaiHienThiBan(BanReadModel ban)
    {
        if (ban.TrangThai == 0 && !ban.TrangThaiHoaDonDangHoatDong.HasValue)
        {
            return 0;
        }

        if (ban.TrangThaiHoaDonDangHoatDong == (int)HoaDonTrangThai.Paid)
        {
            return 2;
        }

        if (ban.TrangThaiHoaDonDangHoatDong == (int)HoaDonTrangThai.Draft)
        {
            return 1;
        }

        if (ban.TrangThai == 2)
        {
            return 2;
        }

        return ban.TrangThai == 0 ? 0 : 1;
    }

    private static string ChuyenTrangThaiBan(int trangThai)
    {
        return trangThai switch
        {
            0 => "Trống",
            1 => "Có khách",
            2 => "Chờ dọn / Đã thanh toán",
            _ => "Trống"
        };
    }

    private static string XacDinhKhuVuc(string tenBan)
    {
        if (string.IsNullOrWhiteSpace(tenBan))
        {
            return "-";
        }

        if (LaBanMangDi(tenBan))
        {
            return "Mang đi";
        }

        var kyTu = char.ToUpperInvariant(tenBan.Trim().FirstOrDefault(char.IsLetter));
        return kyTu switch
        {
            'A' or 'B' => "Khu trong nhà",
            'C' => "Khu sân vườn",
            'D' => "Khu phòng riêng",
            _ => "Khu khác"
        };
    }
}
