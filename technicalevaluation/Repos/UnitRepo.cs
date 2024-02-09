using Microsoft.EntityFrameworkCore;
using technicalevaluation.Data;
using technicalevaluation.Enum;
using technicalevaluation.Models;
using technicalevaluation.Repos.Interfaces;

namespace technicalevaluation.Repos
{
    public class UnitRepo : IUnitRepo
    {
        private readonly UsersContext _dbContext;
        public UnitRepo(UsersContext usersContext)
        {
            _dbContext = usersContext;
        }

        public async Task<UnitInfo> FindByUnitId(int id)
        {
            return await _dbContext.Unit.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<UnitInfo>> FindAllUnits()
        {
            return await _dbContext.Unit.ToListAsync();
        }
        public async Task<UnitInfo> Add(UnitInfo unit)
        {
            await _dbContext.Unit.AddAsync(unit);
            await _dbContext.SaveChangesAsync();

            return unit;
        }
        public async Task<bool> Delete(int id)
        {
            UnitInfo unitById = await FindByUnitId(id);

            if (unitById == null)
            {
                throw new Exception($"Unidade para o Id:{id} não foi encontrado no banco de dados.");
            }

            _dbContext.Unit.Remove(unitById);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeactivateUnit(int unitId)
        {
            UnitInfo unit = await _dbContext.Unit.FirstOrDefaultAsync(x => x.Id == unitId);

            if (unit != null)
            {
                unit.Status = StatusUnit.Inativa;

                _dbContext.Unit.Update(unit);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
