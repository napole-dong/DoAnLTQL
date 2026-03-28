using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.Mon
{
    public class MonValidationResult
    {
        public bool HopLe { get; private set; }
        public string? ThongBao { get; private set; }
        public MonInputField TruongLoi { get; private set; }
        public MonDTO? Mon { get; private set; }

        private MonValidationResult()
        {
        }

        public static MonValidationResult ThanhCong(MonDTO mon)
        {
            return new MonValidationResult
            {
                HopLe = true,
                Mon = mon,
                TruongLoi = MonInputField.None
            };
        }

        public static MonValidationResult ThatBai(string thongBao, MonInputField truongLoi)
        {
            return new MonValidationResult
            {
                HopLe = false,
                ThongBao = thongBao,
                TruongLoi = truongLoi
            };
        }
    }
}
