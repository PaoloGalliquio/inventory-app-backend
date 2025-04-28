using inventory_app_backend.Models;
using inventory_app_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Fetching all products");
                var products = _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                _logger.LogInformation("Adding a new product");
                if (product == null)
                {
                    return BadRequest("Product cannot be null");
                }
                _logger.LogInformation("Adding a new product");
                var result = _productService.AddProduct(product);
                if (result.IsCompletedSuccessfully)
                {
                    return CreatedAtAction(nameof(Get), new { id = product.IdProduct }, product);
                }
                return BadRequest("Failed to add product");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a product");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
