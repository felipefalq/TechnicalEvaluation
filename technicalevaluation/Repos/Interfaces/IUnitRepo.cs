using technicalevaluation.Models;

namespace technicalevaluation.Repos.Interfaces
{
    public interface IUnitRepo
    {
        Task<List<UnitInfo>> FindAllUnits();
        Task<UnitInfo> FindByUnitId(int id);
        Task<UnitInfo> Add(UnitInfo unit);
        Task<bool> Delete(int id);
        Task<bool> DeactivateUnit(int unitId);

        //Task<List<UnitWithCollaborators>> FindAllUnitsWithCollaborators();

    }
}
