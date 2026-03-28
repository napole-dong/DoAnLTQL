using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.Mon
{
    public class MonCsvService
    {
        public string[] DocCsv(string filePath)
        {
            return File.ReadAllLines(filePath, Encoding.UTF8);
        }

        public void XuatCsv(string filePath, IEnumerable<MonDTO> dsMon)
        {
            var lines = new List<string>
            {
                "ID,TenMon,LoaiMonID,TenLoaiMon,DonGia,MoTa,HinhAnh"
            };

            lines.AddRange(dsMon.Select(x => string.Join(",",
                x.ID,
                EscapeCsv(x.TenMon),
                x.LoaiMonID,
                EscapeCsv(x.TenLoaiMon),
                x.DonGia,
                EscapeCsv(x.MoTa),
                EscapeCsv(x.HinhAnh ?? string.Empty))));

            File.WriteAllLines(filePath, lines, Encoding.UTF8);
        }

        private static string EscapeCsv(string value)
        {
            if (!value.Contains(',') && !value.Contains('"') && !value.Contains('\n') && !value.Contains('\r'))
            {
                return value;
            }

            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
    }
}
