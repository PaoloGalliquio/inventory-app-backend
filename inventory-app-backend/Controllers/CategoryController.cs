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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Fetching all categories");
                var categories = await _categoryService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving categories");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category)
        {
            try
            {
                _logger.LogInformation("Adding a new category");
                if (category == null)
                {
                    return BadRequest("Category cannot be null");
                }
                _logger.LogInformation("Adding a new category");
                var result = await _categoryService.AddCategory(category);
                if (result != null)
                {
                    _logger.LogInformation("Category added successfully");
                    return CreatedAtAction(nameof(Get), new { id = category.IdCategory }, category);
                }
                _logger.LogWarning("Failed to add category");
                return BadRequest("Failed to add category");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a category");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            try
            {
                _logger.LogInformation("Updating category with ID {id}", id);
                if (category == null || id != category.IdCategory)
                {
                    return BadRequest("Category ID mismatch");
                }
                var result = await _categoryService.UpdateCategory(category);
                if (result > 0)
                {
                    _logger.LogInformation("Category updated successfully");
                    return NoContent();
                }
                _logger.LogWarning("Failed to update category");
                return NotFound("Category not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a category");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting category with ID {id}", id);
                var result = await _categoryService.DeleteCategory(id);
                if (result > 0)
                {
                    _logger.LogInformation("Category deleted successfully");
                    return NoContent();
                }
                _logger.LogWarning("Failed to delete category");
                return NotFound("Category not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a category");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
