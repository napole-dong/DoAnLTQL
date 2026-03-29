using QuanLyQuanCaPhe.DAL;
using QuanLyQuanCaPhe.DTO;

namespace QuanLyQuanCaPhe.BUS;

public class NguyenLieuBUS
{
    private readonly NguyenLieuDAL _nguyenLieuDAL = new();

    public List<NguyenLieuDTO> LayDanhSachNguyenLieu(string? tuKhoa)
    {
        return _nguyenLieuDAL.GetDanhSachNguyenLieu(tuKhoa?.Trim());
    }

    public int LayMaNguyenLieuTiepTheo()
    {
        return _nguyenLieuDAL.GetNextNguyenLieuId();
    }

    public (bool ThanhCong, string ThongBao, NguyenLieuDTO? NguyenLieuMoi) ThemNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        var validation = KiemTraThongTin(nguyenLieuDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi, null);
        }

        nguyenLieuDTO.TrangThai = TinhTrangThai(nguyenLieuDTO.SoLuongTon, nguyenLieuDTO.MucCanhBao, nguyenLieuDTO.TrangThai);
        var nguyenLieuMoi = _nguyenLieuDAL.ThemNguyenLieu(nguyenLieuDTO);
        return (true, "Thêm nguyên liệu thành công.", nguyenLieuMoi);
    }

    public (bool ThanhCong, string ThongBao) CapNhatNguyenLieu(NguyenLieuDTO nguyenLieuDTO)
    {
        if (nguyenLieuDTO.MaNguyenLieu <= 0)
        {
            return (false, "Vui lòng chọn nguyên liệu cần cập nhật.");
        }

        var validation = KiemTraThongTin(nguyenLieuDTO);
        if (!validation.HopLe)
        {
            return (false, validation.ThongBaoLoi);
        }

        nguyenLieuDTO.TrangThai = TinhTrangThai(nguyenLieuDTO.SoLuongTon, nguyenLieuDTO.MucCanhBao, nguyenLieuDTO.TrangThai);
        var daCapNhat = _nguyenLieuDAL.CapNhatNguyenLieu(nguyenLieuDTO);

        return daCapNhat
            ? (true, "Cập nhật nguyên liệu thành công.")
            : (false, "Không tìm thấy nguyên liệu để cập nhật.");
    }

    public (bool ThanhCong, string ThongBao) XoaNguyenLieu(int maNguyenLieu)
    {
        if (maNguyenLieu <= 0)
        {
            return (false, "Vui lòng chọn nguyên liệu cần xóa.");
        }

        var daXoa = _nguyenLieuDAL.XoaNguyenLieu(maNguyenLieu);
        return daXoa
            ? (true, "Xóa nguyên liệu thành công.")
            : (false, "Không tìm thấy nguyên liệu để xóa.");
    }

    private static (bool HopLe, string ThongBaoLoi) KiemTraThongTin(NguyenLieuDTO nguyenLieuDTO)
    {
        if (string.IsNullOrWhiteSpace(nguyenLieuDTO.TenNguyenLieu))
        {
            return (false, "Tên nguyên liệu không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(nguyenLieuDTO.DonViTinh))
        {
            return (false, "Vui lòng chọn đơn vị tính.");
        }

        if (nguyenLieuDTO.SoLuongTon < 0)
        {
            return (false, "Số lượng tồn không hợp lệ.");
        }

        if (nguyenLieuDTO.MucCanhBao < 0)
        {
            return (false, "Mức cảnh báo không hợp lệ.");
        }

        if (nguyenLieuDTO.GiaNhapGanNhat < 0)
        {
            return (false, "Giá nhập gần nhất không hợp lệ.");
        }

        if (string.IsNullOrWhiteSpace(nguyenLieuDTO.TrangThai))
        {
            return (false, "Vui lòng chọn trạng thái.");
        }

        return (true, string.Empty);
    }

    private static string TinhTrangThai(decimal soLuongTon, decimal mucCanhBao, string trangThaiHienTai)
    {
        if (trangThaiHienTai.Equals("Ngừng dùng", StringComparison.OrdinalIgnoreCase))
        {
            return "Ngừng dùng";
        }

        if (soLuongTon <= 0)
        {
            return "Hết hàng";
        }

        if (soLuongTon <= mucCanhBao)
        {
            return "Sắp hết";
        }

        return "Đang sử dụng";
    }
}
