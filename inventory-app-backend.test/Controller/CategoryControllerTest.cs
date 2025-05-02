using FakeItEasy;
using inventory_app_backend.Controllers;
using inventory_app_backend.DTO;
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
    public class CategoryControllerTest
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private CategoryController _controller;

        public CategoryControllerTest()
        {
            _categoryService = A.Fake<ICategoryService>();
            _logger = A.Fake<ILogger<CategoryController>>();
            _controller = new CategoryController(_categoryService, _logger);
        }

        [Fact]
        public async Task Get_ReturnsOkResult()
        {
            // Arrange
            var fakeCategories = new List<Category>
            {
                new Category { IdCategory = 1, Name = "Category 1" },
                new Category { IdCategory = 2, Name = "Category 2" }
            };

            A.CallTo(() => _categoryService.GetAllCategories())
                .Returns(Task.FromResult(fakeCategories));

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsType<List<Category>>(okResult.Value);
            Assert.Equal(fakeCategories.Count, returnedCategories.Count);
            Assert.Equal(fakeCategories, returnedCategories);
        }

    }
}
