using FakeItEasy;
using inventory_app_backend.Controllers;
using inventory_app_backend.DTO;
using inventory_app_backend.Services;
using inventory_app_backend.Validators;
using inventory_app_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inventory_app_backend.DTO.User;

namespace inventory_app_backend.test.Controller
{
    public class UserControllerTest
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IUserValidator _validator;
        private UserController _controller;

        public UserControllerTest()
        {
            _userService = A.Fake<IUserService>();
            _logger = A.Fake<ILogger<UserController>>();
            _validator = A.Fake<IUserValidator>();
            _controller = new UserController(_userService, _logger, _validator);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult()
        {
            // Arrange
            var fakeUsers = new List<GetUserDTO>
            {
                new GetUserDTO { IdUser = 1, Name = "John Doe", Email = "john.doe@example.com", IdRole = 1, RoleName = "Admin" },
                new GetUserDTO { IdUser = 2, Name = "Jane Smith", Email = "jane.smith@example.com", IdRole = 2, RoleName = "User" }
            };

            A.CallTo(() => _userService.AllUsers()).Returns(Task.FromResult(fakeUsers));

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsType<List<GetUserDTO>>(okResult.Value);
            Assert.Equal(fakeUsers.Count, returnedUsers.Count);
            Assert.Equal(fakeUsers, returnedUsers);
        }

        [Fact]
        public async Task AddUser_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newUser = new UserDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                IdRole = 1
            };

            var createdUserId = 1;
            A.CallTo(() => _userService.AddUser(newUser)).Returns(Task.FromResult(createdUserId));

            // Act
            var result = await _controller.AddUser(newUser);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetAllUsers", createdAtResult.ActionName);
            var value = createdAtResult.Value;
            var messageProperty = value.GetType().GetProperty("message");
            var messageValue = (string)messageProperty.GetValue(value);
            Assert.Equal("Usuario creado satisfactoriamente", messageValue);
        }

        [Fact]
        public async Task UpdateUser_ReturnsOkResult()
        {
            // Arrange
            var updatedUser = new UpdateUserDTO
            {
                IdUser = 1,
                Name = "John Updated",
                Email = "john.updated@example.com",
                Password = "newpassword123",
                IdRole = 2
            };

            A.CallTo(() => _userService.UpdateUser(updatedUser)).Returns(Task.FromResult(1));

            // Act
            var result = await _controller.UpdateUser(updatedUser.IdUser, updatedUser);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;
            var messageProperty = value.GetType().GetProperty("message");
            var messageValue = (string)messageProperty.GetValue(value);
            Assert.Equal("Usuario actualizado satisfactoriamente", messageValue);
        }
    }
}
