using System;
using System.Collections.Generic;

namespace QuanLyQuanCaPhe.DTO
{
    public class HoaDonPrintDto
    {
        // Store info
        public string StoreName { get; set; } = string.Empty;
        public string StoreAddress { get; set; } = string.Empty;
        public string StorePhone { get; set; } = string.Empty;

        // Invoice info
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public string CashierName { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        // Financials
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal CashReceived { get; set; }

        // Line items
        public List<HoaDonPrintLineItem> Items { get; set; } = new();
    }

    public class HoaDonPrintLineItem
    {
        public int STT { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
