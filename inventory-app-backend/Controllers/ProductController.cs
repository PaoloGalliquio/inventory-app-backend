using ClosedXML.Excel;
using inventory_app_backend.DTO;
using inventory_app_backend.Models;
using inventory_app_backend.Services;
using inventory_app_backend.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace inventory_app_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductValidator _validator;

        public ProductController(IProductService productService, ILogger<ProductController> logger, IProductValidator validator)
        {
            _productService = productService;
            _logger = logger;
            _validator = validator;
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
                var validatorResult = _validator.RunValidatorForCreate(product);
                Console.WriteLine($"Validator has errors: {validatorResult.HasErrors()}");
                if (validatorResult.HasErrors()) return BadRequest(validatorResult);

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
                    return Ok(new { message = "Producto creado satisfactoriamente"});
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
                var validatorResult = _validator.RunValidatorForUpdate(product);
                if (validatorResult.HasErrors()) return BadRequest(validatorResult);

                _logger.LogInformation("Updating product with ID {id}", id);
                if (product == null || id != product.IdProduct)
                {
                    return BadRequest("Product ID mismatch");
                }
                var result = await _productService.UpdateProduct(product);
                if (result > 0)
                {
                    _logger.LogInformation("Product updated successfully");
                    return Ok(new { message = "Producto actualizado satisfactoriamente" });
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

        [HttpGet("GetProductsLowStockReportExcel")]
        public async Task<IActionResult> GetProductsLowStockReportExcel()
        {
            try
            {
                _logger.LogInformation("Generating low stock report");
                var products = await _productService.GetProductsWithLowStock();
                if (products == null || !products.Any())
                {
                    _logger.LogWarning("No products found with low stock");
                    return NotFound(new { message = "No se encontraron productos con existencias bajas" });
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Productos bajo stock");

                    worksheet.Cell(1, 1).Value = "Nombre";
                    worksheet.Cell(1, 2).Value = "Descripción";
                    worksheet.Cell(1, 3).Value = "Precio";
                    worksheet.Cell(1, 4).Value = "Cantidad";
                    worksheet.Cell(1, 5).Value = "Categoría";

                    for (int i = 0; i < products.Count; i++)
                    {
                        var product = products[i];
                        worksheet.Cell(i + 2, 1).Value = product.Name;
                        worksheet.Cell(i + 2, 2).Value = product.Description;
                        worksheet.Cell(i + 2, 3).Value = product.Price;
                        worksheet.Cell(i + 2, 4).Value = product.Quantity;
                        worksheet.Cell(i + 2, 5).Value = product.Category?.Name;
                    }
                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        _logger.LogInformation("Low stock report generated successfully");
                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ProductosBajoStock.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the low stock report");
                return BadRequest(new { message = "Error al obtener reporte de existencias bajas" });
            }
        }

        [HttpGet("GetProductsLowStockReportPdf")]
        public async Task<IActionResult> GetProductsLowStockReportPdf()
        {
            try
            {
                _logger.LogInformation("Generating low stock PDF report");
                var products = await _productService.GetProductsWithLowStock();
                if (products == null || !products.Any())
                {
                    _logger.LogWarning("No products found with low stock");
                    return NotFound(new { message = "No se encontraron productos con existencias bajas" });
                }

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(50);
                        page.Header().Text("Reporte de Productos con Stock Bajo").FontSize(20).Bold();
                        page.Content().PaddingVertical(10).Column(col =>
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Nombre");
                                    header.Cell().Text("Descripción");
                                    header.Cell().Text("Precio");
                                    header.Cell().Text("Cantidad");
                                    header.Cell().Text("Categoría");
                                });

                                foreach (var product in products)
                                {
                                    table.Cell().Text(product.Name);
                                    table.Cell().Text(product.Description);
                                    table.Cell().Text(product.Price.ToString("C"));
                                    table.Cell().Text(product.Quantity.ToString());
                                    table.Cell().Text(product.Category?.Name ?? "N/A");
                                }
                            });
                        });
                        page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
                    });
                });

                using (var stream = new MemoryStream())
                {
                    document.GeneratePdf(stream);
                    _logger.LogInformation("Low stock PDF report generated successfully");
                    return File(
                        stream.ToArray(),
                        "application/pdf",
                        "ProductosBajoStock.pdf");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the low stock PDF report");
                return BadRequest(new { message = "Error al obtener reporte PDF de existencias bajas" });
            }
        }
    }
}
