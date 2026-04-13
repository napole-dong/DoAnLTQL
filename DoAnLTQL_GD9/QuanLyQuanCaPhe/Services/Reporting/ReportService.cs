using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace QuanLyQuanCaPhe.Services.Reporting
{
    public class ReportService
    {
        // Must match RDLC dataset names: QLBHDataSet_HoaDon and QLBHDataSet_HoaDon_ChiTiet
        public const string DataSetName = "QLBHDataSet";
        public const string ReportFileRelative = "Reports\\rptInHoaDon.rdlc";

        public static string GetDataSourceName(string tableName) => $"{DataSetName}_{tableName}";

        public void ShowReportPreview(Form owner, DataSet ds, string? rdlcFullPath = null, Dictionary<string, object>? parameters = null)
        {
            using var f = new Forms.frmInHoaDon();
            var path = rdlcFullPath ?? Path.Combine(Application.StartupPath, ReportFileRelative);
            f.LoadReport(ds, path, parameters);
            f.ShowDialog(owner);
        }

        // Render DataSet to PDF bytes using LocalReport
        public byte[] RenderPdf(DataSet ds, string? rdlcFullPath = null, Dictionary<string, object>? parameters = null)
        {
            if (ds == null) throw new ArgumentNullException(nameof(ds));

            var path = rdlcFullPath ?? Path.Combine(Application.StartupPath, ReportFileRelative);
            if (!File.Exists(path)) throw new FileNotFoundException("RDLC file not found", path);

            using var localReport = new LocalReport { ReportPath = path };

            // add datasources
            localReport.DataSources.Clear();
            foreach (DataTable t in ds.Tables)
            {
                localReport.DataSources.Add(new ReportDataSource(GetDataSourceName(t.TableName), t));
            }

            if (parameters != null && parameters.Count > 0)
            {
                var prms = new List<ReportParameter>();
                foreach (var kv in parameters)
                    prms.Add(new ReportParameter(kv.Key, kv.Value?.ToString() ?? string.Empty));
                localReport.SetParameters(prms);
            }

            var mimeType = string.Empty;
            var encoding = string.Empty;
            var fileNameExt = string.Empty;
            var streams = new string[0];
            var warnings = new Warning[0];

            var bytes = localReport.Render(
                "PDF",
                null,
                out mimeType,
                out encoding,
                out fileNameExt,
                out streams,
                out warnings);

            return bytes;
        }
    }
}
