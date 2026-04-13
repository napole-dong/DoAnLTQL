using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using QuanLyQuanCaPhe.Services.Diagnostics;
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
                if (ds == null)
                {
                    MessageBox.Show(
                        "Khong co du lieu de in hoa don.",
                        "Bao cao",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                reportViewer1.Reset();
                reportViewer1.ProcessingMode = ProcessingMode.Local;
                reportViewer1.LocalReport.DataSources.Clear();

                if (!File.Exists(rdlcFullPath))
                {
                    AppLogger.Warning($"Report template not found at path: {rdlcFullPath}", nameof(frmInHoaDon));
                    MessageBox.Show(
                        "Khong tim thay mau in hoa don. Vui long kiem tra cau hinh report.",
                        "Bao cao",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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
                        AppLogger.Error(ex, "Failed to apply one or more report parameters.", nameof(frmInHoaDon));
                        MessageBox.Show(
                            "Mot so tham so in hoa don khong hop le. He thong se tiep tuc voi tham so mac dinh.",
                            "Bao cao",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "LoadReport failed.", nameof(frmInHoaDon));
                MessageBox.Show(
                    AppExceptionHandler.CreateUserMessage("Khong the tai bao cao hoa don.", ex),
                    "Bao cao",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
