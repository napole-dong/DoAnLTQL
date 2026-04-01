using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QuanLyQuanCaPhe.Data
{
    public class dtaLoaiMon
    {
        public int ID { get; set; }
        public string TenLoai { get; set; } = string.Empty;
        public string? MoTa { get; set; }

        public virtual ObservableCollectionListSource<dtaMon> Mon { get; } = new();
    }
}