using FakeItEasy;
using inventory_app_backend.Controllers;
using inventory_app_backend.DTO;
using inventory_app_backend.Models;
using inventory_app_backend.Services;
using inventory_app_backend.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace inventory_app_backend.test.Controller
{
    public class ProductControllerTest
    {

        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductValidator _validator;
        private ProductController _controller;

        public ProductControllerTest()
        {
            _productService = A.Fake<IProductService>();
            _logger = A.Fake<ILogger<ProductController>>();
            _validator = A.Fake<IProductValidator>();
            _controller = new ProductController(_productService, _logger, _validator);
        }

        [Fact]
        public async Task Get_ReturnsOkResult()
        {
            // Arrange
            var controller = new ProductController(_productService, _logger, _validator);
            var products = new List<ProductDTO>
            {
                new ProductDTO { IdProduct = 1, Name = "Product1", Description = "Description1", Price = 10.0m, Quantity = 5, IdCategory = 1 },
                new ProductDTO { IdProduct = 2, Name = "Product2", Description = "Description2", Price = 20.0m, Quantity = 10, IdCategory = 2 }
            };
            A.CallTo(() => _productService.GetAllProducts()).Returns(products);
            // Act
            var result = await controller.Get();
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult()
        {
            // Arrange
            var controller = new ProductController(_productService, _logger, _validator);
            var product = new ProductDTO { IdProduct = 1, Name = "Product1", Description = "Description1", Price = 10.0m, Quantity = 5, IdCategory = 1 };
            A.CallTo(() => _productService.GetProduct(1)).Returns(product);
            // Act
            var result = await controller.Get(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult()
        {
            // Arrange
            var controller = new ProductController(_productService, _logger, _validator);
            A.CallTo(() => _productService.DeleteProduct(1)).Returns(1);
            // Act
            var result = await controller.Delete(1);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsOkResult()
        {
            // Arrange
            var newProduct = new CreateProductDTO
            {
                IdProduct = 1,
                Name = "Product1",
                Description = "Description1",
                Price = 10.0m,
                Quantity = 5,
                IdCategory = 1
            };

            var createdProductId = 1;
            A.CallTo(() => _productService.AddProduct(newProduct)).Returns(Task.FromResult(new Product
            {
                IdProduct = createdProductId,
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                Quantity = newProduct.Quantity,
                IdCategory = newProduct.IdCategory
            }));

            // Act
            var result = await _controller.Post(newProduct);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;
            var messageProperty = value.GetType().GetProperty("message");
            var messageValue = (string)messageProperty.GetValue(value);
            Assert.Equal("Producto creado satisfactoriamente", messageValue);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOkResult()
        {
            // Arrange
            var updatedProduct = new CreateProductDTO
            {
                IdProduct = 1,
                Name = "Product1",
                Description = "Description1",
                Price = 10.0m,
                Quantity = 5,
                IdCategory = 1
            };

            A.CallTo(() => _productService.UpdateProduct(updatedProduct)).Returns(Task.FromResult(1));

            // Act
            var result = await _controller.Put(updatedProduct.IdProduct, updatedProduct);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;
            var messageProperty = value.GetType().GetProperty("message");
            var messageValue = (string)messageProperty.GetValue(value);
            Assert.Equal("Producto actualizado satisfactoriamente", messageValue);
        }
    }
}
