using technicalevaluation.Models;

namespace technicalevaluation.Repos.Interfaces
{
    public interface ICollaboratorRepo
    {
        Task<List<CollaboratorInfo>> FindAllCollaborators();
        Task<CollaboratorInfo> FindById(int id);
        Task<CollaboratorInfo> Add(CollaboratorInfo collaborator);
        Task<bool> UpdateCollaboratorInfo(int id, string newName, int? newUnitId);
        Task<bool> DeleteCollaborator(int collaboratorId);
        void Attach<T>(T entity) where T : class;
    }
}
