using Microsoft.EntityFrameworkCore;
using technicalevaluation.Data;
using technicalevaluation.Enum;
using technicalevaluation.Models;
using technicalevaluation.Repos.Interfaces;

namespace technicalevaluation.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly UsersContext _dbContext;
        public UserRepo(UsersContext usersContext)
        {
            _dbContext = usersContext;
        }
        public async Task<List<UserInfo>> FindByStatus(StatusUser status)
        {
            return await _dbContext.Users.Where(x => x.Status == status).ToListAsync();
        }
        public async Task<UserInfo> FindByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
        }
        public async Task<UserInfo> FindById(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<UserInfo>> FindAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }
        public async Task<UserInfo> Add(UserInfo user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
        public async Task<bool> UpdateUserCredentials(int id, string newPassword, StatusUser newStatus)
        {
            UserInfo user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                user.Password = newPassword;
                user.Status = newStatus;

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
