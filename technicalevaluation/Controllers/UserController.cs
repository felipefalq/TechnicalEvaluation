using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using technicalevaluation.Enum;
using technicalevaluation.Models;
using technicalevaluation.Repos.Interfaces;

namespace technicalevaluation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly ILogger _logger;
        public UserController(IUserRepo userRepo, ILogger<UserController> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<List<UserInfo>>> FindAllUsers()
        {
            List<UserInfo> users = await _userRepo.FindAllUsers();
            return Ok(users);
        }
        [HttpGet("{status}")]
        public async Task<ActionResult<List<UserInfo>>> FindByStatus(StatusUser status)
        {
            List <UserInfo> users = await _userRepo.FindByStatus(status);
            return Ok(users);
        }
        [HttpPost]
        public async Task<ActionResult<UserInfo>> Register([FromBody] UserInfo userInfo)
        {
            try
            {
                UserInfo existingUser = await _userRepo.FindByUsername(userInfo.Username);
                if (existingUser != null)
                {
                    return Conflict(new { Message = "Nome de usuário já cadastrado." });
                }

                UserInfo user = await _userRepo.Add(userInfo);

                return Ok(new { Message = "Usuário registrado com sucesso.", User = user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar usuário");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfo>> UpdateUser(int id, [FromBody] UserInfo userInfo)
        {
            try
            {
                UserInfo existingUser = await _userRepo.FindById(id);
                if (existingUser == null)
                {
                    return NotFound(new { Message = "Usuário não encontrado." });
                }
                UserInfo existingUserWithNewUsername = await _userRepo.FindByUsername(userInfo.Username);
                if (existingUserWithNewUsername != null && existingUserWithNewUsername.Id != id)
                {
                    return Conflict(new { Message = "Já existe um usuário com esse novo nome de usuário." });
                }

                bool result = await _userRepo.UpdateUserCredentials(id, userInfo.Password, userInfo.Status);

                if (result)
                {
                    UserInfo updatedUser = await _userRepo.FindById(id);
                    return Ok(updatedUser);
                }
                else
                {
                    return NotFound(new { Message = "Usuário não encontrado." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Erro ao atualizar usuário: {ex.Message}" });
            }
        }
    }
}
