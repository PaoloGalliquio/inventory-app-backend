using inventory_app_backend.Mapppers;
using inventory_app_backend.Services;
using inventory_app_backend.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace inventory_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userService;

        public LoginController(ILogger<LoginController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserViewModel viewModel)
        {
            try
            {
                _logger.LogInformation($"Login method called by {viewModel?.Email}");
                if (viewModel == null)
                {
                    return BadRequest("User cannot be null");
                }
                var user = await _userService.GetUserByEmail(viewModel.Email);
                if (user == null)
                {
                    _logger.LogWarning("Invalid email or password");
                    return Unauthorized("Invalid email or password");
                }
                if (!_userService.VerifyPassword(viewModel.Password, user.Password))
                {
                    _logger.LogWarning("Invalid email or password");
                    return Unauthorized("Invalid email or password");
                }
                var userViewModel = UserMapper.MapTopUserViewModel(user);
                userViewModel.Token = _userService.GenerateToken(user.Email);
                _logger.LogInformation("User logged in successfully");

                return Ok(userViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
