using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.Services.HoaDon
{
    public enum HoaDonManHinhState
    {
        Xem
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
        public bool ChoPhepXoaMon { get; set; }
        public bool ChoPhepThuTien { get; set; }
        public bool KhoaToanBoChiTietDoDaThanhToan { get; set; }
    }

    public class HoaDonFormStateService
    {
        public HoaDonFormControlState TaoTrangThai(HoaDonManHinhState _, HoaDonDTO? hoaDon)
        {
            var laHoaDonMo = hoaDon != null && HoaDonStateMachine.IsOpen(hoaDon.TrangThai);
            var laHoaDonDaThanhToan = hoaDon != null && HoaDonStateMachine.IsPaid(hoaDon.TrangThai);
            var coTongTien = hoaDon?.TongTien > 0;
            var coHoaDon = hoaDon != null;

            return new HoaDonFormControlState
            {
                ChoPhepSuaThongTinChung = false,
                ChoPhepLocMaster = true,
                ChoPhepGridMaster = true,

                ChoPhepThemMoi = false,
                ChoPhepSua = false,
                ChoPhepHuy = laHoaDonMo,
                ChoPhepLuu = false,
                ChoPhepBoQua = false,

                ChoPhepThemMon = laHoaDonMo,
                ChoPhepXoaMon = laHoaDonMo,
                ChoPhepThuTien = laHoaDonMo && coTongTien,
                KhoaToanBoChiTietDoDaThanhToan = laHoaDonDaThanhToan
            };
        }
    }
}
