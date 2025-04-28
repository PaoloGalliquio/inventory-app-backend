using inventory_app_backend.Models;
using inventory_app_backend.Services;
using inventory_app_backend.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventory_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.AllUsers();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserViewModel user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }
            var result = await _userService.AddUser(user);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetAllUsers), new { id = user.IdUser }, user);
            }
            return BadRequest("Failed to add user");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.IdUser)
            {
                return BadRequest("User ID mismatch");
            }
            var result = await _userService.UpdateUser(user);
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound("User not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (result > 0)
            {
                return NoContent();
            }
            return NotFound("User not found");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.AllUsers();
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }
    }
}
