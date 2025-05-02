using inventory_app_backend.Constants;
using inventory_app_backend.DTO.User;
using inventory_app_backend.Services;
using inventory_app_backend.Validators;
using Microsoft.AspNetCore.Mvc;

namespace inventory_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly ILoginValidator _validator;

        public HomeController(ILogger<HomeController> logger, IUserService userService, ILoginValidator validator)
        {
            _logger = logger;
            _userService = userService;
            _validator = validator;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var validatorResult = _validator.RunValidatorForLogin(loginDTO);
                if (validatorResult.HasErrors()) return BadRequest(validatorResult);

                _logger.LogInformation($"Login method called by {loginDTO?.Email}");
                if (loginDTO == null)
                {
                    return BadRequest("User cannot be null");
                }
                var user = await _userService.GetUserByEmail(loginDTO.Email);
                if (user == null)
                {
                    _logger.LogWarning("Invalid email or password");
                    return Unauthorized("Invalid email or password");
                }
                if (!_userService.VerifyPassword(loginDTO.Password, user.Password))
                {
                    _logger.LogWarning("Invalid email or password");
                    return Unauthorized("Invalid email or password");
                }
                var loginResultDTO = new LoginResultDTO
                {
                    IdUser = user.IdUser,
                    Name = user.Name,
                    Email = user.Email,
                    IdUserRole = user.IdUserRole,
                    UserRoleName = Enum.GetName(typeof(Roles), user.IdUserRole) ?? "-",
                    Token = _userService.GenerateToken(user.Email)
                };
                _logger.LogInformation("User logged in successfully");

                return Ok(loginResultDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
