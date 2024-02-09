using Microsoft.EntityFrameworkCore;
using technicalevaluation.Data;
using technicalevaluation.Models;
using technicalevaluation.Repos.Interfaces;

namespace technicalevaluation.Repos
{
    public class CollaboratorRepo : ICollaboratorRepo
    {
        private readonly UsersContext _dbContext;
        public CollaboratorRepo(UsersContext usersContext)
        {
            _dbContext = usersContext;
        }
        public async Task<CollaboratorInfo> FindById(int id)
        {
            return await _dbContext.Collaborators
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<CollaboratorInfo>> FindAllCollaborators()
        {
            return await _dbContext.Collaborators
                .Include(c => c.Unit)
                .Include(c => c.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CollaboratorInfo> Add(CollaboratorInfo collaborator)
        {
            if (collaborator.UserId.HasValue)
            {
                UserInfo user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == collaborator.UserId);

                if (user == null)
                {
                    throw new InvalidOperationException("Usuário não encontrado.");
                }
                collaborator.User = user;
            }

            await _dbContext.Collaborators.AddAsync(collaborator);
            await _dbContext.SaveChangesAsync();

            return collaborator;
        }
        public async Task<bool> UpdateCollaboratorInfo(int id, string newName, int? newUnitId)
        {
            CollaboratorInfo collaborator = await _dbContext.Collaborators.FirstOrDefaultAsync(x => x.Id == id);

            if (collaborator != null)
            {
                collaborator.Name = newName;
                collaborator.UnitId = newUnitId;

                _dbContext.Collaborators.Update(collaborator);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteCollaborator(int collaboratorId)
        {
            CollaboratorInfo collaborator = await _dbContext.Collaborators.FirstOrDefaultAsync(x => x.Id == collaboratorId);

            if (collaborator != null)
            {
                _dbContext.Collaborators.Remove(collaborator);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public void Attach<T>(T entity) where T : class
        {
            _dbContext.Attach(entity);
        }


        //public async Task<CollaboratorInfo> FindByNameAndIds(string name, int unitId, int userId)
        //{
        //    return await _dbContext.Collaborators
        //        .FirstOrDefaultAsync(c => c.Name == name && c.UnitId == unitId && c.UserId == userId);
        //}
    }
}

