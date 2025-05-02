using FakeItEasy;
using inventory_app_backend.Constants;
using inventory_app_backend.Controllers;
using inventory_app_backend.DTO.User;
using inventory_app_backend.Models;
using inventory_app_backend.Services;
using inventory_app_backend.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory_app_backend.test.Controller
{
    public class HomeControllerTest
    {
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginValidator _validator;
        private HomeController _controller;

        public HomeControllerTest()
        {
            _userService = A.Fake<IUserService>();
            _logger = A.Fake<ILogger<HomeController>>();
            _validator = A.Fake<ILoginValidator>();
            _controller = new HomeController(_logger, _userService, _validator);
        }

        [Fact]
        public async Task Login_ReturnsOkResult()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Email = "test@example.com",
                Password = "validPassword"
            };

            var user = new User
            {
                IdUser = 1,
                Name = "Test User",
                Email = "test@example.com",
                IdUserRole = (int)Roles.User,
                Password = "hashedPassword"
            };

            // Mock validator to return no errors
            var validatorResult = new ValidatorResult();
            A.CallTo(() => _validator.RunValidatorForLogin(loginDto)).Returns(validatorResult);

            // Mock user service
            A.CallTo(() => _userService.GetUserByEmail(loginDto.Email)).Returns(Task.FromResult(user));
            A.CallTo(() => _userService.VerifyPassword(loginDto.Password, user.Password)).Returns(true);
            A.CallTo(() => _userService.GenerateToken(user.Email)).Returns("fakeToken");

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var loginResult = Assert.IsType<LoginResultDTO>(okResult.Value);

            Assert.Equal(user.IdUser, loginResult.IdUser);
            Assert.Equal(user.Name, loginResult.Name);
            Assert.Equal(user.Email, loginResult.Email);
            Assert.Equal(user.IdUserRole, loginResult.IdUserRole);
            Assert.Equal(Enum.GetName(typeof(Roles), user.IdUserRole), loginResult.UserRoleName);
            Assert.Equal("fakeToken", loginResult.Token);
        }
    }
}
