namespace QuanLyQuanCaPhe.DTO;

public class HoaDonSaveRequestDTO
{
    public int ID { get; set; }
    public int BanID { get; set; }
    public int? KhachHangID { get; set; }
    public DateTime NgayLap { get; set; }
    public int TrangThai { get; set; }
    public byte[]? RowVersion { get; set; }
}
