namespace QuanLyQuanCaPhe.Reporting
{
    public static class ReportConstants
    {
        // RDLC DataSet name used in .rdlc files (must match DataSet Name exactly)
        public const string DatasetName = "dsLineItems";

        // Common Report parameter names (kept minimal; add more as needed)
        public const string ParamStoreName = "StoreName";
        public const string ParamStoreAddress = "StoreAddress";
        public const string ParamStorePhone = "StorePhone";
        public const string ParamInvoiceNo = "InvoiceNo";
        public const string ParamInvoiceDate = "InvoiceDate";
        public const string ParamTableName = "TableName";
        public const string ParamCustomerName = "CustomerName";
        public const string ParamCashierName = "CashierName";
        public const string ParamWatermark = "Watermark";
    }
}
