using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Auth;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.DAL;

public class BanDAL
{
    public BanThongKeDTO GetThongKe()
    {
        using var context = new CaPheDbContext();

        var tongBan = context.Ban
            .AsNoTracking()
            .Count();

        var banDangPhucVu = context.Ban
            .AsNoTracking()
            .Count(b => b.TrangThai == 1);

        var banTrong = context.Ban
            .AsNoTracking()
            .Count(b => b.TrangThai == 0);

        var banDatTruoc = context.HoaDon
            .AsNoTracking()
            .Count(hd => hd.TrangThai == 0
                && hd.GhiChuHoaDon != null
                && EF.Functions.Like(hd.GhiChuHoaDon, "%đặt trước%"));

        return new BanThongKeDTO
        {
            TongBan = tongBan,
            BanDangPhucVu = banDangPhucVu,
            BanTrong = banTrong,
            BanDatTruoc = banDatTruoc
        };
    }

    public List<BanDTO> GetDanhSachBan(string? khuVuc, string? trangThai, string? tuKhoa)
    {
        using var context = new CaPheDbContext();

        var query = context.Ban
            .AsNoTracking()
            .AsQueryable();

        query = ApplyKhuVucFilter(query, khuVuc);
        query = ApplyTrangThaiFilter(query, trangThai);
        query = ApplyTuKhoaFilter(query, tuKhoa);

        var banRows = query
            .OrderBy(b => b.ID)
            .Select(b => new BanReadModel
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai
            })
            .ToList();

        return MapBanDtos(banRows);
    }

    public List<BanDTO> GetSoDoBan()
    {
        using var context = new CaPheDbContext();

        var banRows = context.Ban
            .AsNoTracking()
            .OrderBy(b => b.ID)
            .Select(b => new BanReadModel
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai
            })
            .ToList();

        return MapBanDtos(banRows);
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
                TrangThai = x.TrangThai
            })
            .FirstOrDefault();

        return banRow == null ? null : MapBanDto(banRow);
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
            .OrderBy(b => b.TenBan)
            .Select(b => new BanReadModel
            {
                ID = b.ID,
                TenBan = b.TenBan,
                TrangThai = b.TrangThai
            })
            .ToList();

        return MapBanDtos(banRows);
    }

    public BanActionResultDTO ChuyenHoacGopBan(BanChuyenGopRequestDTO request)
    {
        using var correlationScope = CorrelationContext.BeginScope();
        using var context = new CaPheDbContext();
        var executionStrategy = context.Database.CreateExecutionStrategy();

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
            return executionStrategy.Execute(() =>
            {
                using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

                try
                {
                    var banNguon = context.Ban.FirstOrDefault(b => b.ID == request.BanNguonId);
                    if (banNguon == null)
                    {
                        transaction.Rollback();
                        return TuChoi("Không tìm thấy bàn nguồn.");
                    }

                    var hoaDonNguon = context.HoaDon
                        .Include(h => h.HoaDon_ChiTiet)
                        .FirstOrDefault(h => h.BanID == banNguon.ID && h.TrangThai == 0);
                    if (hoaDonNguon == null)
                    {
                        transaction.Rollback();
                        return TuChoi("Bàn nguồn phải có hóa đơn đang mở để chuyển hoặc gộp.");
                    }

                    var banDich = context.Ban.FirstOrDefault(b => b.ID == request.BanDichId);
                    if (banDich == null)
                    {
                        transaction.Rollback();
                        return TuChoi("Không tìm thấy bàn đích.");
                    }

                    if (request.LaChuyenBan)
                    {
                        var banDichCoHoaDonMo = context.HoaDon
                            .AsNoTracking()
                            .Any(h => h.BanID == banDich.ID && h.TrangThai == 0);

                        if (banDich.TrangThai != 0 || banDichCoHoaDonMo)
                        {
                            transaction.Rollback();
                            return TuChoi("Chỉ có thể chuyển sang bàn đích đang trống.");
                        }

                        hoaDonNguon.BanID = banDich.ID;
                        banNguon.TrangThai = 0;
                        banDich.TrangThai = 1;

                        context.SaveChanges();
                        transaction.Commit();

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

                    var hoaDonDich = context.HoaDon
                        .Include(h => h.HoaDon_ChiTiet)
                        .FirstOrDefault(h => h.BanID == banDich.ID && h.TrangThai == 0);

                    if (hoaDonDich == null)
                    {
                        if (banDich.TrangThai != 0)
                        {
                            transaction.Rollback();
                            return TuChoi("Bàn đích không khả dụng để gộp (phải trống hoặc có hóa đơn mở).");
                        }

                        hoaDonDich = TaoHoaDonDichTuHoaDonNguon(context, hoaDonNguon, banDich.ID);
                    }

                    HopNhatChiTietHoaDon(context, hoaDonNguon.HoaDon_ChiTiet, hoaDonDich.ID);

                    context.HoaDon_ChiTiet.RemoveRange(hoaDonNguon.HoaDon_ChiTiet);
                    context.HoaDon.Remove(hoaDonNguon);

                    banNguon.TrangThai = 0;
                    banDich.TrangThai = 1;

                    hoaDonDich.GhiChuHoaDon = ThemGhiChuGopBan(hoaDonDich.GhiChuHoaDon, request.BanNguonId, request.BanDichId);

                    context.SaveChanges();
                    transaction.Commit();

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
                    transaction.Rollback();
                    throw;
                }
            });
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

    private static dtaHoadon TaoHoaDonDichTuHoaDonNguon(CaPheDbContext context, dtaHoadon hoaDonNguon, int banDichId)
    {
        var hoaDonDich = new dtaHoadon
        {
            NhanVienID = hoaDonNguon.NhanVienID,
            KhachHangID = hoaDonNguon.KhachHangID,
            BanID = banDichId,
            NgayLap = DateTime.Now,
            TrangThai = 0,
            GhiChuHoaDon = ThemGhiChuGopBan(hoaDonNguon.GhiChuHoaDon, hoaDonNguon.BanID, banDichId)
        };

        context.HoaDon.Add(hoaDonDich);
        context.SaveChanges();

        return hoaDonDich;
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
                    GhiChu = ctNguon.GhiChu
                };

                chiTietDich.Add(chiTietMoi);
                context.HoaDon_ChiTiet.Add(chiTietMoi);
                continue;
            }

            var tongSoLuong = ctDich.SoLuongBan + ctNguon.SoLuongBan;
            ctDich.SoLuongBan = (short)Math.Clamp(tongSoLuong, 1, short.MaxValue);
        }
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

    private static IQueryable<dtaBan> ApplyTrangThaiFilter(IQueryable<dtaBan> query, string? trangThai)
    {
        if (string.IsNullOrWhiteSpace(trangThai) || trangThai == "Tất cả trạng thái")
        {
            return query;
        }

        return trangThai switch
        {
            "Trống" => query.Where(b => b.TrangThai == 0),
            "Đang phục vụ" => query.Where(b => b.TrangThai == 1),
            "Đặt trước" => query.Where(b => b.TrangThai == 2),
            _ => query
        };
    }

    private static IQueryable<dtaBan> ApplyTuKhoaFilter(IQueryable<dtaBan> query, string? tuKhoa)
    {
        if (string.IsNullOrWhiteSpace(tuKhoa))
        {
            return query;
        }

        var keyword = tuKhoa.Trim();
        var keywordPattern = $"%{keyword}%";
        var hasKeywordId = int.TryParse(keyword, out var keywordId);

        var matchSanSang = "Sẵn sàng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchDangPhucVu = "Đang phục vụ".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchDatTruoc = "Đặt trước".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchTrong = keyword.Equals("trống", StringComparison.OrdinalIgnoreCase);

        var matchKhuTrongNha = "Khu trong nhà".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchKhuSanVuon = "Khu sân vườn".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchKhuPhongRieng = "Khu phòng riêng".Contains(keyword, StringComparison.OrdinalIgnoreCase);
        var matchKhuKhac = "Khu khác".Contains(keyword, StringComparison.OrdinalIgnoreCase);

        return query.Where(b =>
            (hasKeywordId && b.ID == keywordId)
            || EF.Functions.Like(b.TenBan, keywordPattern)
            || (matchSanSang && b.TrangThai == 0)
            || (matchDangPhucVu && b.TrangThai == 1)
            || (matchDatTruoc && b.TrangThai == 2)
            || (matchTrong && b.TrangThai == 0)
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
        return new BanDTO
        {
            ID = banRow.ID,
            TenBan = banRow.TenBan,
            TrangThai = banRow.TrangThai,
            KhuVuc = XacDinhKhuVuc(banRow.TenBan),
            TinhTrang = ChuyenTrangThaiBan(banRow.TrangThai)
        };
    }

    private sealed class BanReadModel
    {
        public int ID { get; init; }
        public string TenBan { get; init; } = string.Empty;
        public int TrangThai { get; init; }
    }

    private static string ChuyenTrangThaiBan(int trangThai)
    {
        return trangThai switch
        {
            0 => "Sẵn sàng",
            1 => "Đang phục vụ",
            2 => "Đặt trước",
            _ => "Sẵn sàng"
        };
    }

    private static string XacDinhKhuVuc(string tenBan)
    {
        if (string.IsNullOrWhiteSpace(tenBan))
        {
            return "-";
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
