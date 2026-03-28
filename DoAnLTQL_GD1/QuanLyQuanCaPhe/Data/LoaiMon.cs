using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QuanLyQuanCaPhe.Data
{
    public class LoaiMon
    {
        public int ID { get; set; }
        public string TenLoai { get; set; }

        public virtual ObservableCollectionListSource<Mon> Mon { get; } = new();
    }
}