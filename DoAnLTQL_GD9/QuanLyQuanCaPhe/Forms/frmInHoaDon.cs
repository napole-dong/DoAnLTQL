using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using QuanLyQuanCaPhe.Services.Reporting;

namespace QuanLyQuanCaPhe.Forms
{
    public partial class frmInHoaDon : Form
    {
        public frmInHoaDon()
        {
            InitializeComponent();
        }

        public void LoadReport(DataSet ds, string rdlcFullPath, Dictionary<string, object>? parameters = null)
        {
            try
            {
                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;
                reportViewer1.LocalReport.DataSources.Clear();

                if (!File.Exists(rdlcFullPath))
                {
                    MessageBox.Show($"Report file not found: {rdlcFullPath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                reportViewer1.LocalReport.ReportPath = rdlcFullPath;

                // Bind each DataTable; RDLC must reference dataset names like QLBHDataSet_HoaDon
                foreach (DataTable t in ds.Tables)
                {
                    var dsName = ReportService.GetDataSourceName(t.TableName);
                    var rds = new ReportDataSource(dsName, t);
                    reportViewer1.LocalReport.DataSources.Add(rds);
                }

                if (parameters != null)
                {
                    try
                    {
                        var defined = reportViewer1.LocalReport.GetParameters();
                        var valid = parameters
                            .Where(p => defined.Any(d => string.Equals(d.Name, p.Key, StringComparison.OrdinalIgnoreCase)))
                            .Select(p => new ReportParameter(p.Key, p.Value?.ToString() ?? string.Empty))
                            .ToArray();

                        if (valid.Length > 0)
                        {
                            reportViewer1.LocalReport.SetParameters(valid);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Warning: failed to set some report parameters: {ex.Message}", "Báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
