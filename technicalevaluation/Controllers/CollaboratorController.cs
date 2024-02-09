using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using technicalevaluation.Data;
using technicalevaluation.Models;
using technicalevaluation.Repos;
using technicalevaluation.Repos.Interfaces;

namespace technicalevaluation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorRepo _collaboratorRepo;
        private readonly IUnitRepo _unitRepo;
        private readonly IUserRepo _userRepo;
        private readonly ILogger _logger;
        private readonly UsersContext _dbContext;
        public CollaboratorController(ICollaboratorRepo collaboratorRepo, IUnitRepo unitRepo, IUserRepo userRepo, ILogger<CollaboratorController> logger, UsersContext dbContext)
        {
            _collaboratorRepo = collaboratorRepo;
            _unitRepo = unitRepo;
            _userRepo = userRepo;
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<CollaboratorInfo>>> FindAllCollaborators()
        {
            List<CollaboratorInfo> collaborators = await _collaboratorRepo.FindAllCollaborators();
            return Ok(collaborators);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CollaboratorInfo>> FindById(int id)
        {
            CollaboratorInfo collaborator = await _collaboratorRepo.FindById(id);
            return Ok(collaborator);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CollaboratorInfo collaboratorInfo)
        {
            try
            {
                CollaboratorInfo existingCollaborator = await _collaboratorRepo.FindById(collaboratorInfo.Id);
                if (existingCollaborator != null)
                {
                    return Conflict(new { Message = "Colaborador já registrado." });
                }

                UnitInfo unit = await _unitRepo.FindByUnitId(collaboratorInfo.UnitId.GetValueOrDefault());
                if (unit == null)
                {
                    return NotFound("Unidade não encontrada.");
                }

                if (unit.Status == Enum.StatusUnit.Inativa)
                {
                    return BadRequest("Não é possível registrar um colaborador em uma unidade inativa.");
                }
                UserInfo user = await _userRepo.FindById(collaboratorInfo.UserId.GetValueOrDefault());
                if (user == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                collaboratorInfo.Unit = unit;
                collaboratorInfo.User = user;

                _dbContext.Attach(collaboratorInfo.Unit);
                _dbContext.Attach(collaboratorInfo.User);

                await _collaboratorRepo.Add(collaboratorInfo);

                return Ok("Colaborador registrado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar colaborador");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CollaboratorInfo>> UpdateCollaborator(int id, [FromBody] CollaboratorInfo collaboratorInfo)
        {
            try
            {
                CollaboratorInfo existingCollaborator = await _collaboratorRepo.FindById(id);
                if (existingCollaborator == null)
                {
                    return NotFound(new { Message = "Colaborador não encontrado." });
                }

                UnitInfo unit = await _unitRepo.FindByUnitId(collaboratorInfo.UnitId.GetValueOrDefault());
                if (unit == null)
                {
                    return NotFound("Unidade não encontrada.");
                }

                bool result = await _collaboratorRepo.UpdateCollaboratorInfo(id, collaboratorInfo.Name, collaboratorInfo.UnitId);

                if (result)
                {
                    return Ok(new { Message = "Colaborador atualizado com sucesso." });
                }
                return NotFound(new { Message = "Colaborador não encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erro ao atualizar colaborador: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollaborator(int id)
        {
            try
            {
                CollaboratorInfo existingCollaborator = await _collaboratorRepo.FindById(id);
                if (existingCollaborator == null)
                {
                    return NotFound(new { Message = "Colaborador não encontrado." });
                }

                bool result = await _collaboratorRepo.DeleteCollaborator(id);

                if (result)
                {
                    return Ok(new { Message = "Colaborador removido com sucesso." });
                }
                return NotFound(new { Message = "Colaborador não encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erro ao remover colaborador: {ex.Message}" });
            }
        }
    }
}