using QuanLyQuanCaPhe.Data;

namespace QuanLyQuanCaPhe.DAL;

public class TaiKhoanDAL
{
    public bool DeleteUser(int userId)
    {
        if (userId <= 0)
        {
            return false;
        }

        using var context = new CaPheDbContext();
        var user = context.User.FirstOrDefault(x => x.ID == userId);
        if (user == null)
        {
            return false;
        }

        // Soft delete to keep audit history and avoid breaking FK links.
        user.HoatDong = false;
        context.SaveChanges();
        return true;
    }
}
