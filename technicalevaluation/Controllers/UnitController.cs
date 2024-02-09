using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using technicalevaluation.Enum;
using technicalevaluation.Models;
using technicalevaluation.Repos.Interfaces;

namespace technicalevaluation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitRepo _unitRepo;
        private readonly ILogger<UnitController> _logger;

        public UnitController(IUnitRepo unitRepo, ILogger<UnitController> logger)
        {
            _unitRepo = unitRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<UnitInfo>>> FindAllUnits()
        {
            List<UnitInfo> unit = await _unitRepo.FindAllUnits();
            return Ok(unit);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitInfo>> FindById(int id)
        {
            UnitInfo unit = await _unitRepo.FindByUnitId(id);
            return Ok(unit);
        }
        [HttpPost]
        public async Task<ActionResult<UnitInfo>> Register([FromBody] UnitInfo unitInfo)
        {
            try
            {
                UnitInfo existingUnit = await _unitRepo.FindByUnitId(unitInfo.UnitId);
                if (existingUnit != null)
                {
                    return Conflict(new { Message = "Unidade já registrada." });
                }

                UnitInfo unit = await _unitRepo.Add(unitInfo);

                return Ok(new { Message = "Unidade registrada com sucesso.", Unit = unit });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar unidade");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpPut("{id}/deactivate")]
        public async Task<ActionResult<UnitInfo>> DeactivateUnit(int id)
        {
            try
            {
                UnitInfo existingUnit = await _unitRepo.FindByUnitId(id);
                if (existingUnit == null)
                {
                    return NotFound(new { Message = "Unidade não encontrada." });
                }

                if (existingUnit.Status == StatusUnit.Inativa)
                {
                    return Conflict(new { Message = "Unidade já está inativa." });
                }

                bool result = await _unitRepo.DeactivateUnit(id);

                if (result)
                {
                    return Ok(new { Message = "Unidade inativada com sucesso." });
                }
                return NotFound(new { Message = "Unidade não encontrada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erro ao inativar unidade: {ex.Message}" });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            try
            {
                UnitInfo existingUnit = await _unitRepo.FindByUnitId(id);
                if (existingUnit == null)
                {
                    return NotFound(new { Message = "Unidade não encontrada." });
                }

                bool result = await _unitRepo.Delete(id);

                if (result)
                {
                    return Ok(new { Message = "Unidade removida com sucesso." });
                }
                return NotFound(new { Message = "Unidade não encontrada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erro ao remover unidade: {ex.Message}" });
            }
        }
    }
}
