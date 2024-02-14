using technicalevaluation.Enum;
using technicalevaluation.Models;

namespace technicalevaluation.Repos.Interfaces
{
    public interface IUserRepo
    {
        Task<List<UserInfo>> FindAllUsers();
        Task<UserInfo> FindById(int id);
        Task<List<UserInfo>> FindByStatus(StatusUser status);
        Task<UserInfo> FindByUsername(string username);
        Task<UserInfo> Add(UserInfo user);
        Task<bool> UpdateUserCredentials(int userId, string newPassword, StatusUser newStatus);
    }
}
