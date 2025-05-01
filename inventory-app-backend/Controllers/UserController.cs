using inventory_app_backend.DTO.User;
using inventory_app_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                var users = await _userService.AllUsers();
                return Ok(users);
            }
            catch
            {
                _logger.LogError("An error occurred while fetching all users");
                return BadRequest(new { message = "Error al obtener usuarios" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDTO user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest(new { message = "Usuario no válido" });
                }
                var result = await _userService.AddUser(user);
                if (result > 0)
                {
                    return CreatedAtAction(nameof(GetAllUsers), null, new { message = "Usuario creado satisfactoriamente" });
                }
                return BadRequest(new { message = "Error al agregar usuario" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a user");
                return BadRequest(new { message = "Error al agregar usuario" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO user)
        {
            try
            {
                if (id != user.IdUser)
                {
                    return BadRequest(new { message = "No se encontró al usuario" });
                }
                var result = await _userService.UpdateUser(user);
                if (result > 0)
                {
                    return Ok(new { message = "Usuario actualizado satisfactoriamente" });
                }
                return BadRequest(new { message = "No se encontró al usuario" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user");
                return BadRequest(new { message = "Error al actualizar usuario" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "Usuario no válido" });
                }
                var result = await _userService.DeleteUser(id);
                if (result > 0)
                {
                    return Ok(new { message = "Usuario desactivado satisfactoriamente" });
                }
                return NotFound(new { message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user");
                return BadRequest(new { message = "Error al eliminar usuario" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID {id}", id);
                var user = await _userService.GetUser(id);
                if (user != null)
                {
                    _logger.LogInformation("User retrieved successfully");
                    return Ok(user);
                }
                _logger.LogWarning("User not found");
                return NotFound("User not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving a user");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
