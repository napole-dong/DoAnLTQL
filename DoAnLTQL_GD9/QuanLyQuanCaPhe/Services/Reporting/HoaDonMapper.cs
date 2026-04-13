using System;
using System.Data;
using System.Globalization;
using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.Services.Reporting
{
    public static class HoaDonMapper
    {
        public static DataSet ToDataSet(dtaHoadon hoaDon)
        {
            if (hoaDon == null)
                throw new ArgumentNullException(nameof(hoaDon));

            var ds = new DataSet(ReportService.DataSetName);

            // Header table
            var hdTable = new DataTable("HoaDon");
            hdTable.Columns.Add("ID", typeof(int));
            hdTable.Columns.Add("NgayLap", typeof(DateTime));
            hdTable.Columns.Add("TenKhachHang", typeof(string));
            hdTable.Columns.Add("MaHoaDon", typeof(string));
            hdTable.Columns.Add("TongTien", typeof(decimal));
            hdTable.Columns.Add("NhanVien", typeof(string));
            hdTable.Columns.Add("Ban", typeof(string));

            var row = hdTable.NewRow();
            row["ID"] = hoaDon.ID;
            row["NgayLap"] = hoaDon.NgayLap;
            row["TenKhachHang"] = string.IsNullOrWhiteSpace(hoaDon.CustomerName) ? hoaDon.KhachHang?.HoVaTen ?? "Khách lẻ" : hoaDon.CustomerName!;
            row["MaHoaDon"] = hoaDon.ID.ToString();
            row["TongTien"] = hoaDon.TongTien;
            row["NhanVien"] = hoaDon.NhanVien?.HoVaTen ?? string.Empty;
            row["Ban"] = hoaDon.Ban?.TenBan ?? string.Empty;
            hdTable.Rows.Add(row);

            // Detail table
            var dtTable = new DataTable("HoaDon_ChiTiet");
            dtTable.Columns.Add("ID", typeof(int));
            dtTable.Columns.Add("HoaDonID", typeof(int));
            dtTable.Columns.Add("TenSanPham", typeof(string));
            dtTable.Columns.Add("SoLuong", typeof(int));
            dtTable.Columns.Add("DonGia", typeof(decimal));
            dtTable.Columns.Add("ThanhTien", typeof(decimal));

            foreach (var ct in hoaDon.HoaDon_ChiTiet ?? Array.Empty<dtHoaDon_ChiTiet>())
            {
                var r = dtTable.NewRow();
                r["ID"] = ct.ID;
                r["HoaDonID"] = ct.HoaDonID;
                r["TenSanPham"] = ct.Mon?.TenMon ?? string.Empty;
                r["SoLuong"] = ct.SoLuongBan;
                r["DonGia"] = ct.DonGiaBan;
                r["ThanhTien"] = ct.ThanhTien;
                dtTable.Rows.Add(r);
            }

            ds.Tables.Add(hdTable);
            ds.Tables.Add(dtTable);
            return ds;
        }
    }
}
