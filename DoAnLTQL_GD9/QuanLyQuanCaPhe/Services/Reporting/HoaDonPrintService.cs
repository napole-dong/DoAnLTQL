using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyQuanCaPhe.Data;
using QuanLyQuanCaPhe.DTO;
using QuanLyQuanCaPhe.Services.Diagnostics;

namespace QuanLyQuanCaPhe.Services.Reporting
{
    // Service responsible for fetching invoice snapshot and mapping to DTO
    public class HoaDonPrintService : IHoaDonReportingService
    {
        public async Task<HoaDonPrintDto?> GetHoaDonPrintDtoAsync(int hoaDonId)
        {
            try
            {
                await using var db = new CaPheDbContext();
                var q = await db.HoaDon
                    .AsNoTracking()
                    .Include(h => h.HoaDon_ChiTiet)
                        .ThenInclude(ct => ct.Mon)
                    .Include(h => h.NhanVien)
                    .Include(h => h.Ban)
                    .Include(h => h.KhachHang)
                    .FirstOrDefaultAsync(h => h.ID == hoaDonId);

                if (q == null) return null;

                var dto = new HoaDonPrintDto
                {
                    InvoiceNo = $"HD{q.ID:D5}",
                    InvoiceDate = q.NgayLap,
                    CashierName = q.NhanVien?.HoVaTen ?? string.Empty,
                    TableName = q.Ban?.TenBan ?? string.Empty,
                    StoreName = "CA PHE NAPOLE",
                    StoreAddress = "188 Le Loi, Quan 1, TP.HCM",
                    StorePhone = "0123456789"
                };

                // map items
                var idx = 1;
                foreach (var ct in q.HoaDon_ChiTiet ?? Array.Empty<dtHoaDon_ChiTiet>())
                {
                    var price = (decimal)ct.DonGiaBan;
                    var qty = (decimal)ct.SoLuongBan;
                    var thanh = price * qty;
                    dto.Items.Add(new HoaDonPrintLineItem
                    {
                        STT = idx++,
                        TenSanPham = ct.Mon?.TenMon ?? string.Empty,
                        SoLuong = ct.SoLuongBan,
                        DonGia = price,
                        ThanhTien = thanh
                    });
                }

                dto.Subtotal = dto.Items.Sum(i => i.ThanhTien);
                dto.Discount = 0m; // business rule
                dto.Vat = 0m;
                dto.GrandTotal = dto.Subtotal - dto.Discount + dto.Vat;

                return dto;
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, $"GetHoaDonPrintDtoAsync failed for id={hoaDonId}", nameof(HoaDonPrintService));
                return null;
            }
        }

        public static DataSet ToDataSet(HoaDonPrintDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var ds = new DataSet("QLBHDataSet");

            var hd = new DataTable("HoaDon");
            hd.Columns.Add("InvoiceNo", typeof(string));
            hd.Columns.Add("InvoiceDate", typeof(DateTime));
            hd.Columns.Add("CashierName", typeof(string));
            hd.Columns.Add("TableName", typeof(string));
            hd.Columns.Add("StoreName", typeof(string));
            hd.Columns.Add("StoreAddress", typeof(string));
            hd.Columns.Add("StorePhone", typeof(string));
            hd.Columns.Add("Subtotal", typeof(decimal));
            hd.Columns.Add("Discount", typeof(decimal));
            hd.Columns.Add("Vat", typeof(decimal));
            hd.Columns.Add("GrandTotal", typeof(decimal));

            var r = hd.NewRow();
            r["InvoiceNo"] = dto.InvoiceNo;
            r["InvoiceDate"] = dto.InvoiceDate;
            r["CashierName"] = dto.CashierName;
            r["TableName"] = dto.TableName;
            r["StoreName"] = dto.StoreName;
            r["StoreAddress"] = dto.StoreAddress;
            r["StorePhone"] = dto.StorePhone;
            r["Subtotal"] = dto.Subtotal;
            r["Discount"] = dto.Discount;
            r["Vat"] = dto.Vat;
            r["GrandTotal"] = dto.GrandTotal;
            hd.Rows.Add(r);

            var dt = new DataTable("HoaDon_ChiTiet");
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("HoaDonID", typeof(int));
            dt.Columns.Add("TenSanPham", typeof(string));
            dt.Columns.Add("SoLuong", typeof(int));
            dt.Columns.Add("DonGia", typeof(decimal));
            dt.Columns.Add("ThanhTien", typeof(decimal));

            var id = 1;
            foreach (var it in dto.Items)
            {
                var rr = dt.NewRow();
                rr["ID"] = id++;
                rr["HoaDonID"] = 1; // not used but keep schema
                rr["TenSanPham"] = it.TenSanPham;
                rr["SoLuong"] = it.SoLuong;
                rr["DonGia"] = it.DonGia;
                rr["ThanhTien"] = it.ThanhTien;
                dt.Rows.Add(rr);
            }

            ds.Tables.Add(hd);
            ds.Tables.Add(dt);
            return ds;
        }
    }
}
