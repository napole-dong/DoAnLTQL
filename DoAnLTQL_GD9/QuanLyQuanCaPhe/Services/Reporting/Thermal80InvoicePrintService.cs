using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.Reporting
{
    public sealed class Thermal80InvoicePrintService
    {
        private const int PaperWidth80Mm = 315;
        private const int PaperHeight = 2000;
        private const float MaxContentWidth = 299f;
        private const float ItemNameColumnRatio = 0.48f;
        private const float ItemQtyColumnRatio = 0.10f;
        private const float ItemUnitPriceColumnRatio = 0.18f;
        private static readonly CultureInfo MoneyCulture = CultureInfo.InvariantCulture;

        public void PrintWithDialog(IWin32Window owner, HoaDonPrintDto dto, string? printerName = null)
        {
            ArgumentNullException.ThrowIfNull(dto);

            using var document = CreateDocument(dto, printerName);
            using var printDialog = new PrintDialog
            {
                AllowCurrentPage = false,
                AllowPrintToFile = false,
                AllowSelection = false,
                UseEXDialog = true,
                Document = document
            };

            if (printDialog.ShowDialog(owner) == DialogResult.OK)
            {
                document.Print();
            }
        }

        public PrintDocument CreateDocument(HoaDonPrintDto dto, string? printerName = null)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var document = new PrintDocument
            {
                PrintController = new StandardPrintController(),
                OriginAtMargins = true
            };

            document.DefaultPageSettings.PaperSize = new PaperSize("Thermal80", PaperWidth80Mm, PaperHeight);
            document.DefaultPageSettings.Margins = new Margins(8, 8, 8, 8);
            document.DefaultPageSettings.Landscape = false;

            if (!string.IsNullOrWhiteSpace(printerName))
            {
                document.PrinterSettings.PrinterName = printerName;
            }

            document.PrintPage += (_, e) =>
            {
                DrawInvoicePage(e, dto);
            };

            return document;
        }

        private static void DrawInvoicePage(PrintPageEventArgs e, HoaDonPrintDto dto)
        {
            var g = e.Graphics ?? throw new InvalidOperationException("Print graphics context is null.");

            using var left = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            using var center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };
            using var right = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };

            using var fontStoreTitle = new Font("Arial", 14f, FontStyle.Bold);
            using var fontHeader = new Font("Arial", 10f, FontStyle.Regular);
            using var fontInvoiceTitle = new Font("Arial", 10f, FontStyle.Bold);
            using var fontNormal = new Font("Arial", 9f, FontStyle.Regular);
            using var fontBold = new Font("Arial", 9f, FontStyle.Bold);
            using var brush = new SolidBrush(Color.Black);
            using var separatorPen = new Pen(Color.Black, 1f);

            float yPos = 0f;
            float startTop = e.MarginBounds.Top;
            float startLeft = e.MarginBounds.Left;
            float width = Math.Min(MaxContentWidth, e.MarginBounds.Width);
            float x = startLeft + (e.MarginBounds.Width - width) / 2f;

            void DrawTextLine(string text, Font font, StringFormat format, float extraBottom = 2f)
            {
                var size = g.MeasureString(text, font, new SizeF(width, 1000f), format);
                var height = (float)Math.Ceiling(size.Height);
                g.DrawString(text, font, brush, new RectangleF(x, startTop + yPos, width, height), format);
                yPos += height + extraBottom;
            }

            void DrawSeparator()
            {
                var lineY = startTop + yPos + 1f;
                g.DrawLine(separatorPen, x, lineY, x + width, lineY);
                yPos += 6f;
            }

            void DrawAmountRow(string label, decimal amount, Font font)
            {
                var labelWidth = width * 0.62f;
                var amountWidth = width - labelWidth;
                var height = font.GetHeight(g) + 2f;

                g.DrawString(label, font, brush, new RectangleF(x, startTop + yPos, labelWidth, height), right);
                g.DrawString(FormatMoney(amount), font, brush, new RectangleF(x + labelWidth, startTop + yPos, amountWidth, height), right);
                yPos += height;
            }

            var storeName = string.IsNullOrWhiteSpace(dto.StoreName) ? "CÀ PHÊ PRO" : dto.StoreName;
            var storeAddress = string.IsNullOrWhiteSpace(dto.StoreAddress) ? "Long Xuyên, An Giang" : dto.StoreAddress;
            var storePhone = string.IsNullOrWhiteSpace(dto.StorePhone) ? "0971742428" : dto.StorePhone;

            DrawTextLine(storeName, fontStoreTitle, center, 1f);
            DrawTextLine($"Địa chỉ: {storeAddress}", fontHeader, center, 1f);
            DrawTextLine($"SĐT: {storePhone}", fontHeader, center, 2f);

            DrawSeparator();

            DrawTextLine("HÓA ĐƠN THANH TOÁN", fontInvoiceTitle, center, 3f);

            DrawTextLine($"Mã HĐ: {dto.InvoiceNo}", fontNormal, left, 1f);
            DrawTextLine($"Ngày giờ: {dto.InvoiceDate:dd/MM/yyyy HH:mm}", fontNormal, left, 1f);
            DrawTextLine($"Thu ngân: {dto.CashierName}", fontNormal, left, 1f);
            DrawTextLine($"Bàn: {dto.TableName}", fontNormal, left, 1f);

            if (TryBuildCustomerLine(dto, out var customerLine))
            {
                DrawTextLine(customerLine, fontNormal, left, 1f);
            }

            DrawSeparator();

            var nameColWidth = width * ItemNameColumnRatio;
            var qtyColWidth = width * ItemQtyColumnRatio;
            var unitColWidth = width * ItemUnitPriceColumnRatio;
            var totalColWidth = width - nameColWidth - qtyColWidth - unitColWidth;

            var nameColX = x;
            var qtyColX = nameColX + nameColWidth;
            var unitColX = qtyColX + qtyColWidth;
            var totalColX = unitColX + unitColWidth;

            var headerHeight = fontBold.GetHeight(g) + 2f;
            g.DrawString("TÊN MÓN", fontBold, brush, new RectangleF(nameColX, startTop + yPos, nameColWidth, headerHeight), left);
            g.DrawString("SL", fontBold, brush, new RectangleF(qtyColX, startTop + yPos, qtyColWidth, headerHeight), right);
            g.DrawString("ĐƠN GIÁ", fontBold, brush, new RectangleF(unitColX, startTop + yPos, unitColWidth, headerHeight), right);
            g.DrawString("THÀNH TIỀN", fontBold, brush, new RectangleF(totalColX, startTop + yPos, totalColWidth, headerHeight), right);
            yPos += headerHeight;

            DrawSeparator();

            foreach (var item in dto.Items)
            {
                var productName = string.IsNullOrWhiteSpace(item.TenSanPham) ? "(Không tên món)" : item.TenSanPham;
                var nameHeight = MeasureWrappedHeight(g, productName, fontNormal, nameColWidth);
                var singleLineHeight = fontNormal.GetHeight(g) + 1f;
                var rowHeight = Math.Max(singleLineHeight, nameHeight);

                g.DrawString(productName, fontNormal, brush, new RectangleF(nameColX, startTop + yPos, nameColWidth, rowHeight), left);
                g.DrawString(item.SoLuong.ToString(MoneyCulture), fontNormal, brush, new RectangleF(qtyColX, startTop + yPos, qtyColWidth, singleLineHeight), right);
                g.DrawString(FormatMoney(item.DonGia), fontNormal, brush, new RectangleF(unitColX, startTop + yPos, unitColWidth, singleLineHeight), right);
                g.DrawString(FormatMoney(item.ThanhTien), fontNormal, brush, new RectangleF(totalColX, startTop + yPos, totalColWidth, singleLineHeight), right);

                yPos += rowHeight + 2f;
            }

            DrawSeparator();

            DrawAmountRow("Tổng tiền hàng:", dto.Subtotal, fontNormal);
            DrawAmountRow("Giảm giá:", dto.Discount, fontNormal);

            DrawSeparator();

            var cashReceived = dto.CashReceived > 0 ? dto.CashReceived : dto.GrandTotal;
            var changeAmount = cashReceived - dto.GrandTotal;
            if (changeAmount < 0)
            {
                changeAmount = 0;
            }

            DrawAmountRow("TỔNG CỘNG:", dto.GrandTotal, fontBold);
            DrawAmountRow("Tiền khách đưa:", cashReceived, fontNormal);
            DrawAmountRow("Tiền thừa:", changeAmount, fontNormal);

            DrawSeparator();

            DrawTextLine("Cảm ơn quý khách và hẹn gặp lại!", fontNormal, center, 1f);
            DrawTextLine("Wifi: CaPhePro | Pass: 12345678", fontNormal, center, 1f);

            e.HasMorePages = false;
        }

        private static bool TryBuildCustomerLine(HoaDonPrintDto dto, out string line)
        {
            line = string.Empty;

            var customerName = dto.CustomerName?.Trim() ?? string.Empty;
            var customerPhone = dto.CustomerPhone?.Trim() ?? string.Empty;
            var hasName = !string.IsNullOrWhiteSpace(customerName);
            var hasPhone = !string.IsNullOrWhiteSpace(customerPhone);

            if (!hasName && !hasPhone)
            {
                return false;
            }

            if (hasName && hasPhone)
            {
                line = $"Khách hàng: {customerName} ({customerPhone})";
            }
            else if (hasName)
            {
                line = $"Khách hàng: {customerName}";
            }
            else
            {
                line = $"Khách hàng: ({customerPhone})";
            }

            return true;
        }

        private static float MeasureWrappedHeight(Graphics g, string text, Font font, float width)
        {
            var size = g.MeasureString(text, font, new SizeF(width, 1000f));
            return (float)Math.Ceiling(size.Height);
        }

        private static string FormatMoney(decimal amount)
        {
            return string.Format(MoneyCulture, "{0:N0}", amount);
        }
    }
}
