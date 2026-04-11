using System.Text;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuanLyQuanCaPhe.Services.Export;

public class DataExportService
{
    private static readonly object QuestPdfLock = new();
    private static bool _questPdfConfigured;

    public ExportResult XuatBangDuLieu(
        IWin32Window owner,
        string tieuDeHopThoai,
        string tenFileMacDinh,
        string tieuDeTaiLieu,
        IReadOnlyList<string> tieuDeCot,
        IReadOnlyList<IReadOnlyList<string>> duLieu)
    {
        if (tieuDeCot == null || tieuDeCot.Count == 0)
        {
            return ExportResult.TaoThatBai("Khong co cau hinh cot de xuat du lieu.");
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "Excel Workbook (*.xlsx)|*.xlsx|PDF Document (*.pdf)|*.pdf|CSV UTF-8 (*.csv)|*.csv",
            FileName = tenFileMacDinh,
            Title = tieuDeHopThoai,
            AddExtension = true,
            DefaultExt = "xlsx"
        };

        if (dialog.ShowDialog(owner) != DialogResult.OK)
        {
            return ExportResult.TaoDaHuy();
        }

        try
        {
            var duongDan = dialog.FileName;
            var phanMoRong = Path.GetExtension(duongDan).ToLowerInvariant();

            switch (phanMoRong)
            {
                case ".xlsx":
                    XuatExcel(duongDan, tieuDeCot, duLieu);
                    break;
                case ".pdf":
                    XuatPdf(duongDan, tieuDeTaiLieu, tieuDeCot, duLieu);
                    break;
                default:
                    XuatCsv(duongDan, tieuDeCot, duLieu);
                    break;
            }

            return ExportResult.TaoThanhCong($"Xuat du lieu thanh cong: {duongDan}");
        }
        catch (Exception ex)
        {
            return ExportResult.TaoThatBai($"Khong the xuat du lieu. Chi tiet: {ex.Message}");
        }
    }

    private static void XuatCsv(
        string filePath,
        IReadOnlyList<string> tieuDeCot,
        IReadOnlyList<IReadOnlyList<string>> duLieu)
    {
        var lines = new List<string>
        {
            string.Join(",", tieuDeCot.Select(EscapeCsv))
        };

        lines.AddRange(duLieu.Select(dong => string.Join(",", ChuanHoaDong(dong, tieuDeCot.Count).Select(EscapeCsv))));

        File.WriteAllLines(filePath, lines, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
    }

    private static void XuatExcel(
        string filePath,
        IReadOnlyList<string> tieuDeCot,
        IReadOnlyList<IReadOnlyList<string>> duLieu)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("DuLieu");

        for (var i = 0; i < tieuDeCot.Count; i++)
        {
            worksheet.Cell(1, i + 1).Value = tieuDeCot[i];
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#F4EDE5");
        }

        for (var rowIndex = 0; rowIndex < duLieu.Count; rowIndex++)
        {
            var dong = ChuanHoaDong(duLieu[rowIndex], tieuDeCot.Count);
            for (var colIndex = 0; colIndex < tieuDeCot.Count; colIndex++)
            {
                worksheet.Cell(rowIndex + 2, colIndex + 1).Value = dong[colIndex];
            }
        }

        worksheet.Columns().AdjustToContents();
        workbook.SaveAs(filePath);
    }

    private static void XuatPdf(
        string filePath,
        string tieuDeTaiLieu,
        IReadOnlyList<string> tieuDeCot,
        IReadOnlyList<IReadOnlyList<string>> duLieu)
    {
        DamBaoQuestPdfDaDuocCauHinh();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                page.Header().Column(column =>
                {
                    column.Item().Text(tieuDeTaiLieu).SemiBold().FontSize(15);
                    column.Item().Text($"Ngay xuat: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(9).FontColor(Colors.Grey.Darken1);
                });

                page.Content().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        for (var i = 0; i < tieuDeCot.Count; i++)
                        {
                            columns.RelativeColumn();
                        }
                    });

                    table.Header(header =>
                    {
                        foreach (var cot in tieuDeCot)
                        {
                            header.Cell().Element(StyleHeaderCell).Text(cot);
                        }
                    });

                    if (duLieu.Count == 0)
                    {
                        table.Cell()
                            .ColumnSpan((uint)tieuDeCot.Count)
                            .Element(StyleDataCell)
                            .Text("Khong co du lieu");
                    }
                    else
                    {
                        foreach (var dong in duLieu)
                        {
                            var dongDaChuanHoa = ChuanHoaDong(dong, tieuDeCot.Count);
                            foreach (var giaTri in dongDaChuanHoa)
                            {
                                table.Cell().Element(StyleDataCell).Text(giaTri);
                            }
                        }
                    }
                });
            });
        }).GeneratePdf(filePath);
    }

    private static IContainer StyleHeaderCell(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(Colors.Grey.Lighten3)
            .PaddingVertical(4)
            .PaddingHorizontal(6)
            .DefaultTextStyle(x => x.SemiBold());
    }

    private static IContainer StyleDataCell(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(3)
            .PaddingHorizontal(6);
    }

    private static void DamBaoQuestPdfDaDuocCauHinh()
    {
        if (_questPdfConfigured)
        {
            return;
        }

        lock (QuestPdfLock)
        {
            if (_questPdfConfigured)
            {
                return;
            }

            QuestPDF.Settings.License = LicenseType.Community;
            _questPdfConfigured = true;
        }
    }

    private static IReadOnlyList<string> ChuanHoaDong(IReadOnlyList<string> dong, int soLuongCot)
    {
        var ketQua = new string[soLuongCot];

        for (var i = 0; i < soLuongCot; i++)
        {
            ketQua[i] = i < dong.Count ? dong[i] : string.Empty;
        }

        return ketQua;
    }

    private static string EscapeCsv(string value)
    {
        if (!value.Contains(',') && !value.Contains('"') && !value.Contains('\n') && !value.Contains('\r'))
        {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"")}\"";
    }

    public sealed class ExportResult
    {
        public bool ThanhCong { get; init; }
        public bool DaHuy { get; init; }
        public string ThongBao { get; init; } = string.Empty;

        public static ExportResult TaoThanhCong(string thongBao)
        {
            return new ExportResult { ThanhCong = true, DaHuy = false, ThongBao = thongBao };
        }

        public static ExportResult TaoThatBai(string thongBao)
        {
            return new ExportResult { ThanhCong = false, DaHuy = false, ThongBao = thongBao };
        }

        public static ExportResult TaoDaHuy()
        {
            return new ExportResult { ThanhCong = false, DaHuy = true, ThongBao = string.Empty };
        }
    }
}
