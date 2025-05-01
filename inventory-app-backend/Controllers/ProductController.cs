using inventory_app_backend.DTO;
using inventory_app_backend.Models;
using inventory_app_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Fetching all products");
                var products = await _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductDTO product)
        {
            try
            {
                _logger.LogInformation("Adding a new product");
                if (product == null)
                {
                    return BadRequest("Product cannot be null");
                }
                _logger.LogInformation("Adding a new product");
                var result = await _productService.AddProduct(product);
                if (result != null)
                {
                    _logger.LogInformation("Product added successfully");
                    return CreatedAtAction(nameof(Get), new { id = product.IdProduct }, new { product, message = "Producto creado satisfactoriamente"});
                }
                _logger.LogWarning("Failed to add product");
                return BadRequest("Failed to add product");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a product");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CreateProductDTO product)
        {
            try
            {
                _logger.LogInformation("Updating product with ID {id}", id);
                if (product == null || id != product.IdProduct)
                {
                    return BadRequest("Product ID mismatch");
                }
                var result = await _productService.UpdateProduct(product);
                if (result > 0)
                {
                    _logger.LogInformation("Product updated successfully");
                    return Ok(new { product, message = "Producto actualizado satisfactoriamente" });
                }
                _logger.LogWarning("Failed to update product");
                return NotFound("Product not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a product");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting product with ID {id}", id);
                var result = await _productService.DeleteProduct(id);
                if (result > 0)
                {
                    _logger.LogInformation("Product deleted successfully");
                    return Ok(new { message = "Producto eliminado satisfactoriamente" });
                }
                _logger.LogWarning("Failed to delete product");
                return NotFound("Product not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a product");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                _logger.LogInformation("Fetching product with ID {id}", id);
                var product = await _productService.GetProduct(id);
                if (product != null)
                {
                    _logger.LogInformation("Product retrieved successfully");
                    return Ok(product);
                }
                _logger.LogWarning("Product not found");
                return NotFound("Product not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving a product");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
