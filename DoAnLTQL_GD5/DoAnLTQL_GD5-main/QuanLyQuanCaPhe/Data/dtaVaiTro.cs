using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace QuanLyQuanCaPhe.Data
{
    public class dtaVaiTro
    {
        public int ID { get; set; }
        public string TenVaiTro { get; set; }
        public string? MoTa { get; set; }

        public virtual ObservableCollectionListSource<dtaUser> Users { get; } = new();
    }
}
