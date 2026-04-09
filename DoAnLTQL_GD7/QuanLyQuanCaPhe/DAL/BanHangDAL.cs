using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.DAL;

public class BanHangDAL
{
    public BanHangPhieuDTO GetPhieuTheoBan(int banId)
    {
        using var context = new CaPheDbContext();

        var banRow = QueryBanRow(context, banId);
        if (banRow == null)
        {
            return new BanHangPhieuDTO();
        }

        var hoaDonMoId = QueryHoaDonMoId(context, banId);
        var chiTietRows = hoaDonMoId.HasValue
            ? QueryHoaDonChiTietRows(context, hoaDonMoId.Value)
            : new List<BanHangOrderItemReadModel>();

        return MapBanHangPhieuDto(banRow, hoaDonMoId, chiTietRows);
    }

    public BanActionResultDTO GoiMon(int banId, IEnumerable<BanHangThemMonDTO> dsMonThem)
    {
        using var correlationScope = CorrelationContext.BeginScope();
        using var context = new CaPheDbContext();
        using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

        AppLogger.Info($"Start GoiMon. BanID={banId}.", nameof(BanHangDAL));
        AppLogger.Audit(
            "Order.Place.Start",
            "Bat dau xu ly goi mon.",
            new { BanId = banId },
            nameof(BanHangDAL));

        BanActionResultDTO TuChoi(string thongBao)
        {
            AppLogger.Audit(
                "Order.Place.Rejected",
                thongBao,
                new { BanId = banId },
                nameof(BanHangDAL));

            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }

        try
        {
            var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
            if (ban == null)
            {
                transaction.Rollback();
                return TuChoi("Không tìm thấy bàn để gọi món.");
            }

            var dsMonHopLe = TongHopMonThemHopLe(dsMonThem);
            if (!dsMonHopLe.Any())
            {
                transaction.Rollback();
                return TuChoi("Danh sách món gọi không hợp lệ.");
            }

            var dsMonId = dsMonHopLe
                .Select(x => x.MonID)
                .Distinct()
                .ToArray();

            var dsMonDb = context.Mon
                .Where(x => dsMonId.Contains(x.ID))
                .ToDictionary(x => x.ID);

            if (dsMonDb.Count != dsMonId.Length)
            {
                transaction.Rollback();
                return TuChoi("Có món không tồn tại trong hệ thống.");
            }

            if (dsMonDb.Values.Any(x => x.TrangThai != 1))
            {
                transaction.Rollback();
                return TuChoi("Có món đang ngừng bán, vui lòng tải lại danh sách món.");
            }

            // B1: Tao hoa don mo neu ban chua co hoa don dang phuc vu.
            var hoaDonMo = context.HoaDon
                .FirstOrDefault(x => x.BanID == banId && x.TrangThai == 0);

            if (hoaDonMo == null)
            {
                var nhanVienId = GetOrCreateNhanVienMacDinh(context);
                var khachHangId = GetOrCreateKhachLe(context);

                hoaDonMo = new dtaHoadon
                {
                    BanID = banId,
                    NhanVienID = nhanVienId,
                    KhachHangID = khachHangId,
                    NgayLap = DateTime.Now,
                    TrangThai = 0,
                    GhiChuHoaDon = string.Empty
                };

                context.HoaDon.Add(hoaDonMo);
                context.SaveChanges();
            }

            // B2: Ghi hoa don chi tiet (them moi hoac cong don so luong).
            var chiTietHoaDon = context.HoaDon_ChiTiet
                .Where(x => x.HoaDonID == hoaDonMo.ID)
                .ToList();
            var chiTietTheoMonGia = chiTietHoaDon
                .Where(x => x.GhiChu == null)
                .GroupBy(x => (x.MonID, x.DonGiaBan))
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var monThem in dsMonHopLe)
            {
                var mon = dsMonDb[monThem.MonID];
                var chiTietKey = (monThem.MonID, mon.DonGia);

                if (!chiTietTheoMonGia.TryGetValue(chiTietKey, out var chiTiet))
                {
                    var chiTietMoi = new dtHoaDon_ChiTiet
                    {
                        HoaDonID = hoaDonMo.ID,
                        MonID = monThem.MonID,
                        SoLuongBan = monThem.SoLuong,
                        DonGiaBan = mon.DonGia,
                        GhiChu = null
                    };

                    chiTietHoaDon.Add(chiTietMoi);
                    context.HoaDon_ChiTiet.Add(chiTietMoi);
                    chiTietTheoMonGia[chiTietKey] = chiTietMoi;
                }
                else
                {
                    chiTiet.SoLuongBan = (short)Math.Clamp(chiTiet.SoLuongBan + monThem.SoLuong, 1, short.MaxValue);
                }
            }

            // B3: Lay cong thuc, tinh nhu cau nguyen lieu va kiem tra ton kho.
            var dsCongThuc = QueryCongThucMonRows(context, dsMonId);
            var loiCongThuc = KiemTraDayDuCongThuc(dsMonId, dsCongThuc);
            if (loiCongThuc != null)
            {
                transaction.Rollback();
                AppLogger.Audit(
                    "Order.Place.Rejected",
                    loiCongThuc.ThongBao,
                    new { BanId = banId },
                    nameof(BanHangDAL));
                return loiCongThuc;
            }

            var dsNhuCauNguyenLieu = TinhNhuCauNguyenLieu(dsMonHopLe, dsCongThuc);
            var dsNguyenLieuId = dsNhuCauNguyenLieu
                .Select(x => x.NguyenLieuID)
                .Distinct()
                .ToArray();

            var dsNguyenLieuDb = context.NguyenLieu
                .Where(x => dsNguyenLieuId.Contains(x.ID))
                .ToDictionary(x => x.ID);

            if (dsNguyenLieuDb.Count != dsNguyenLieuId.Length)
            {
                transaction.Rollback();
                return TuChoi("Công thức món tham chiếu nguyên liệu không tồn tại.");
            }

            var loiTonKho = KiemTraTonKho(dsNhuCauNguyenLieu, dsNguyenLieuDb);
            if (loiTonKho != null)
            {
                transaction.Rollback();
                AppLogger.Audit(
                    "Order.Place.Rejected",
                    loiTonKho.ThongBao,
                    new { BanId = banId },
                    nameof(BanHangDAL));
                return loiTonKho;
            }

            // B4: Ton kho du -> tru ton va ghi phieu xuat kho trong cung transaction.
            TruTonKhoNguyenLieu(context, hoaDonMo.ID, banId, dsNhuCauNguyenLieu, dsNguyenLieuDb);

            ban.TrangThai = 1;
            context.SaveChanges();
            transaction.Commit();

            AppLogger.Audit(
                "Order.Place.Success",
                "Goi mon thanh cong.",
                new
                {
                    BanId = banId,
                    HoaDonId = hoaDonMo.ID,
                    SoLoaiMon = dsMonHopLe.Count,
                    TongSoLuong = dsMonHopLe.Sum(x => (int)x.SoLuong)
                },
                nameof(BanHangDAL));

            return new BanActionResultDTO { ThanhCong = true, ThongBao = "Gọi món thành công." };
        }
        catch (DbUpdateException ex) when (LaLoiTrungHoaDonMo(ex))
        {
            transaction.Rollback();
            AppLogger.Warning(
                $"Open invoice conflict while GoiMon. BanID={banId}. {ex.Message}",
                nameof(BanHangDAL),
                AppErrorCode.DbDuplicateKey);
            AppLogger.Audit(
                "Order.Place.Failed",
                "Goi mon that bai do xung dot hoa don mo.",
                new { BanId = banId },
                nameof(BanHangDAL));

            var thongBao = AppExceptionHandler.CreateUserMessage(
                "Bàn đang có hóa đơn chưa thanh toán.",
                ex);

            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            var mappedError = AppExceptionMapper.Map(ex);
            AppLogger.Error(
                ex,
                $"Unexpected failure in GoiMon. BanID={banId}.",
                nameof(BanHangDAL),
                mappedError.Code);
            AppLogger.Audit(
                "Order.Place.Failed",
                "Goi mon that bai do loi he thong.",
                new { BanId = banId },
                nameof(BanHangDAL));
            var thongBao = AppExceptionHandler.CreateUserMessage(
                "Không thể gọi món do xảy ra lỗi trong quá trình cập nhật tồn kho.",
                ex);

            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }
    }

    private static List<BanHangThemMonDTO> TongHopMonThemHopLe(IEnumerable<BanHangThemMonDTO> dsMonThem)
    {
        return dsMonThem
            .Where(x => x.MonID > 0 && x.SoLuong > 0)
            .GroupBy(x => x.MonID)
            .Select(g => new BanHangThemMonDTO
            {
                MonID = g.Key,
                SoLuong = (short)Math.Clamp(g.Sum(x => (int)x.SoLuong), 1, short.MaxValue)
            })
            .ToList();
    }

    private static List<CongThucMonReadModel> QueryCongThucMonRows(
        CaPheDbContext context,
        IReadOnlyCollection<int> dsMonId)
    {
        if (dsMonId.Count == 0)
        {
            return new List<CongThucMonReadModel>();
        }

        return context.CongThucMon
            .AsNoTracking()
            .Where(x => dsMonId.Contains(x.MonID))
            .Select(x => new CongThucMonReadModel
            {
                MonID = x.MonID,
                NguyenLieuID = x.NguyenLieuID,
                SoLuong = x.SoLuong
            })
            .ToList();
    }

    private static BanActionResultDTO? KiemTraDayDuCongThuc(
        IReadOnlyCollection<int> dsMonId,
        IEnumerable<CongThucMonReadModel> dsCongThuc)
    {
        var dsMonCoCongThuc = dsCongThuc
            .Select(x => x.MonID)
            .Distinct()
            .ToHashSet();

        foreach (var monId in dsMonId)
        {
            if (!dsMonCoCongThuc.Contains(monId))
            {
                return new BanActionResultDTO
                {
                    ThanhCong = false,
                    ThongBao = "Món chưa có công thức chế biến, không thể gọi món."
                };
            }
        }

        return null;
    }

    private static List<NhuCauNguyenLieuReadModel> TinhNhuCauNguyenLieu(
        IEnumerable<BanHangThemMonDTO> dsMonThem,
        IEnumerable<CongThucMonReadModel> dsCongThuc)
    {
        var soLuongMonTheoId = dsMonThem
            .ToDictionary(x => x.MonID, x => (decimal)x.SoLuong);

        return dsCongThuc
            .GroupBy(x => x.NguyenLieuID)
            .Select(g => new NhuCauNguyenLieuReadModel
            {
                NguyenLieuID = g.Key,
                SoLuongCan = g.Sum(x => x.SoLuong * soLuongMonTheoId[x.MonID])
            })
            .Where(x => x.SoLuongCan > 0)
            .ToList();
    }

    private static BanActionResultDTO? KiemTraTonKho(
        IEnumerable<NhuCauNguyenLieuReadModel> dsNhuCauNguyenLieu,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb)
    {
        foreach (var nhuCau in dsNhuCauNguyenLieu)
        {
            if (!dsNguyenLieuDb.TryGetValue(nhuCau.NguyenLieuID, out var nguyenLieu))
            {
                return new BanActionResultDTO
                {
                    ThanhCong = false,
                    ThongBao = "Công thức món tham chiếu nguyên liệu không tồn tại."
                };
            }

            if (nguyenLieu.SoLuongTon < nhuCau.SoLuongCan)
            {
                return new BanActionResultDTO
                {
                    ThanhCong = false,
                    ThongBao = "Nguyên liệu không đủ tồn kho để gọi món."
                };
            }
        }

        return null;
    }

    private static void TruTonKhoNguyenLieu(
        CaPheDbContext context,
        int hoaDonId,
        int banId,
        IEnumerable<NhuCauNguyenLieuReadModel> dsNhuCauNguyenLieu,
        IReadOnlyDictionary<int, dtaNguyenLieu> dsNguyenLieuDb)
    {
        var thoiDiemXuat = DateTime.Now;
        var lyDoXuat = TaoLyDoXuatKhoTuGoiMon(hoaDonId, banId);
        var dsPhieuXuat = new List<dtaPhieuXuatKho>();

        foreach (var nhuCau in dsNhuCauNguyenLieu)
        {
            if (!dsNguyenLieuDb.TryGetValue(nhuCau.NguyenLieuID, out var nguyenLieu))
            {
                continue;
            }

            nguyenLieu.SoLuongTon = Math.Max(0, nguyenLieu.SoLuongTon - nhuCau.SoLuongCan);
            nguyenLieu.TrangThai = TinhTrangThaiNguyenLieu(nguyenLieu.SoLuongTon, nguyenLieu.MucCanhBao, nguyenLieu.TrangThai);
            nguyenLieu.TrangThaiTextLegacy = ChuyenTrangThaiNguyenLieuTextLegacy(nguyenLieu.TrangThai, nguyenLieu.SoLuongTon);

            dsPhieuXuat.Add(new dtaPhieuXuatKho
            {
                NguyenLieuID = nhuCau.NguyenLieuID,
                SoLuongXuat = nhuCau.SoLuongCan,
                NgayXuat = thoiDiemXuat,
                LyDo = lyDoXuat
            });
        }

        if (dsPhieuXuat.Count > 0)
        {
            context.PhieuXuatKho.AddRange(dsPhieuXuat);
        }
    }

    private static string TaoLyDoXuatKhoTuGoiMon(int hoaDonId, int banId)
    {
        return $"Xuất kho phục vụ gọi món cho hóa đơn HD{hoaDonId:D5} (bàn {banId:D2}).";
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

    public BanActionResultDTO ThanhToan(int banId)
    {
        using var correlationScope = CorrelationContext.BeginScope();
        using var context = new CaPheDbContext();

        AppLogger.Audit(
            "Payment.Collect.Start",
            "Bat dau thu tien cho ban.",
            new { BanId = banId },
            nameof(BanHangDAL));

        BanActionResultDTO TuChoi(string thongBao)
        {
            AppLogger.Audit(
                "Payment.Collect.Rejected",
                thongBao,
                new { BanId = banId },
                nameof(BanHangDAL));

            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }

        try
        {
            var ban = context.Ban.FirstOrDefault(x => x.ID == banId);
            if (ban == null)
            {
                return TuChoi("Không tìm thấy bàn cần thanh toán.");
            }

            var hoaDonMo = context.HoaDon
                .Include(x => x.HoaDon_ChiTiet)
                .FirstOrDefault(x => x.BanID == banId && x.TrangThai == 0);

            if (hoaDonMo == null)
            {
                return TuChoi("Bàn này chưa có hóa đơn mở.");
            }

            if (!hoaDonMo.HoaDon_ChiTiet.Any())
            {
                return TuChoi("Hóa đơn chưa có món, không thể thanh toán.");
            }

            var tongTien = hoaDonMo.HoaDon_ChiTiet.Sum(x => x.SoLuongBan * x.DonGiaBan);

            hoaDonMo.TrangThai = 1;
            ban.TrangThai = 0;
            context.SaveChanges();

            AppLogger.Audit(
                "Payment.Collect.Success",
                "Thu tien thanh cong.",
                new
                {
                    BanId = banId,
                    HoaDonId = hoaDonMo.ID,
                    TongTien = tongTien
                },
                nameof(BanHangDAL));

            return new BanActionResultDTO
            {
                ThanhCong = true,
                ThongBao = $"Thanh toán hóa đơn HD{hoaDonMo.ID:D5} thành công."
            };
        }
        catch (Exception ex)
        {
            var mappedError = AppExceptionMapper.Map(ex);
            AppLogger.Error(
                ex,
                $"Unexpected failure in ThanhToan. BanID={banId}.",
                nameof(BanHangDAL),
                mappedError.Code);
            AppLogger.Audit(
                "Payment.Collect.Failed",
                "Thu tien that bai do loi he thong.",
                new { BanId = banId },
                nameof(BanHangDAL));

            var thongBao = AppExceptionHandler.CreateUserMessage(
                "Không thể thu tiền do lỗi hệ thống.",
                ex);

            return new BanActionResultDTO
            {
                ThanhCong = false,
                ThongBao = thongBao
            };
        }
    }

    private static BanReadModel? QueryBanRow(CaPheDbContext context, int banId)
    {
        return context.Ban
            .AsNoTracking()
            .Where(x => x.ID == banId)
            .Select(x => new BanReadModel
            {
                ID = x.ID,
                TenBan = x.TenBan,
                TrangThaiBan = x.TrangThai
            })
            .FirstOrDefault();
    }

    private static int? QueryHoaDonMoId(CaPheDbContext context, int banId)
    {
        return context.HoaDon
            .AsNoTracking()
            .Where(x => x.BanID == banId && x.TrangThai == 0)
            .Select(x => (int?)x.ID)
            .FirstOrDefault();
    }

    private static List<BanHangOrderItemReadModel> QueryHoaDonChiTietRows(CaPheDbContext context, int hoaDonId)
    {
        return context.HoaDon_ChiTiet
            .AsNoTracking()
            .Where(x => x.HoaDonID == hoaDonId)
            .GroupBy(x => new { x.MonID, x.Mon.TenMon, x.DonGiaBan })
            .Select(g => new BanHangOrderItemReadModel
            {
                MonID = g.Key.MonID,
                TenMon = g.Key.TenMon,
                DonGia = g.Key.DonGiaBan,
                TongSoLuong = g.Sum(x => (int)x.SoLuongBan)
            })
            .OrderBy(x => x.TenMon)
            .ToList();
    }

    private static BanHangPhieuDTO MapBanHangPhieuDto(
        BanReadModel banRow,
        int? hoaDonMoId,
        IEnumerable<BanHangOrderItemReadModel> chiTietRows)
    {
        return new BanHangPhieuDTO
        {
            BanID = banRow.ID,
            TenBan = banRow.TenBan,
            TrangThaiBan = banRow.TrangThaiBan,
            HoaDonID = hoaDonMoId,
            ChiTiet = chiTietRows.Select(MapBanHangOrderItemDto).ToList()
        };
    }

    private static BanHangOrderItemDTO MapBanHangOrderItemDto(BanHangOrderItemReadModel chiTietRow)
    {
        return new BanHangOrderItemDTO
        {
            MonID = chiTietRow.MonID,
            TenMon = chiTietRow.TenMon,
            DonGia = chiTietRow.DonGia,
            SoLuong = (short)Math.Clamp(chiTietRow.TongSoLuong, 1, short.MaxValue)
        };
    }

    private sealed class BanReadModel
    {
        public int ID { get; init; }
        public string TenBan { get; init; } = string.Empty;
        public int TrangThaiBan { get; init; }
    }

    private sealed class BanHangOrderItemReadModel
    {
        public int MonID { get; init; }
        public string TenMon { get; init; } = string.Empty;
        public decimal DonGia { get; init; }
        public int TongSoLuong { get; init; }
    }

    private sealed class CongThucMonReadModel
    {
        public int MonID { get; init; }
        public int NguyenLieuID { get; init; }
        public decimal SoLuong { get; init; }
    }

    private sealed class NhuCauNguyenLieuReadModel
    {
        public int NguyenLieuID { get; init; }
        public decimal SoLuongCan { get; init; }
    }

    private static int GetOrCreateNhanVienMacDinh(CaPheDbContext context)
    {
        var nhanVien = context.NhanVien.OrderBy(x => x.ID).FirstOrDefault();
        if (nhanVien != null)
        {
            return nhanVien.ID;
        }

        nhanVien = new dtaNhanVien
        {
            HoVaTen = "Nhân viên bán hàng",
            DienThoai = null,
            DiaChi = null
        };

        context.NhanVien.Add(nhanVien);
        context.SaveChanges();

        return nhanVien.ID;
    }

    private static int GetOrCreateKhachLe(CaPheDbContext context)
    {
        var khach = context.KhachHang.FirstOrDefault(x => x.HoVaTen == "Khách lẻ");

        if (khach != null)
        {
            return khach.ID;
        }

        khach = new dtaKhachHang
        {
            HoVaTen = "Khách lẻ",
            DienThoai = null,
            DiaChi = null
        };

        context.KhachHang.Add(khach);
        context.SaveChanges();

        return khach.ID;
    }

    private static bool LaLoiTrungHoaDonMo(DbUpdateException ex)
    {
        if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx)
        {
            return (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                && sqlEx.Message.Contains("UX_HoaDon_Ban_Mo", StringComparison.OrdinalIgnoreCase);
        }

        return ex.InnerException?.Message.Contains("UX_HoaDon_Ban_Mo", StringComparison.OrdinalIgnoreCase) == true;
    }
}
