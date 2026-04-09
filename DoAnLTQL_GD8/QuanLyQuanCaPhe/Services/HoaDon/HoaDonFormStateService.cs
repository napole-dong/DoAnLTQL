using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.HoaDon
{
    public enum HoaDonManHinhState
    {
        Xem,
        ThemMoi,
        ChinhSua
    }

    public class HoaDonFormControlState
    {
        public bool ChoPhepSuaThongTinChung { get; set; }
        public bool ChoPhepLocMaster { get; set; }
        public bool ChoPhepGridMaster { get; set; }

        public bool ChoPhepThemMoi { get; set; }
        public bool ChoPhepSua { get; set; }
        public bool ChoPhepHuy { get; set; }
        public bool ChoPhepLuu { get; set; }
        public bool ChoPhepBoQua { get; set; }

        public bool ChoPhepThemMon { get; set; }
        public bool ChoPhepThuTien { get; set; }
        public bool ChoPhepIn { get; set; }
    }

    public class HoaDonFormStateService
    {
        public HoaDonFormControlState TaoTrangThai(HoaDonManHinhState manHinhState, HoaDonDTO? hoaDon)
        {
            var dangSua = manHinhState != HoaDonManHinhState.Xem;
            var laHoaDonMo = hoaDon?.TrangThai == 0;
            var coTongTien = hoaDon?.TongTien > 0;
            var coHoaDon = hoaDon != null;

            return new HoaDonFormControlState
            {
                ChoPhepSuaThongTinChung = dangSua,
                ChoPhepLocMaster = !dangSua,
                ChoPhepGridMaster = !dangSua,

                ChoPhepThemMoi = !dangSua,
                ChoPhepSua = !dangSua && laHoaDonMo,
                ChoPhepHuy = !dangSua && laHoaDonMo,
                ChoPhepLuu = dangSua,
                ChoPhepBoQua = dangSua,

                ChoPhepThemMon = !dangSua && laHoaDonMo,
                ChoPhepThuTien = !dangSua && laHoaDonMo && coTongTien,
                ChoPhepIn = !dangSua && coHoaDon
            };
        }
    }
}
