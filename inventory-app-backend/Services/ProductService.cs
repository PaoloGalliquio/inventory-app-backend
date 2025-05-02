using inventory_app_backend.DTO;
using inventory_app_backend.Models;
using Microsoft.EntityFrameworkCore;
using inventory_app_backend.Constants;

namespace inventory_app_backend.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProducts();
        Task<Product> AddProduct(CreateProductDTO Product);
        Task<int> UpdateProduct(CreateProductDTO Product);
        Task<int> DeleteProduct(int id);
        Task<ProductDTO> GetProduct(int id);
        Task<List<ProductDTO>> GetProductsWithLowStock();
    }

    public class ProductService : IProductService
    {
        private readonly InventoryContext _context;

        public ProductService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProduct(CreateProductDTO product)
        {
            try
            {
                var newProduct = new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    IdCategory = product.IdCategory,
                    IdStatus = 1
                };
                if(product.Quantity < 5)
                {
                    NotifyAdmins(newProduct);
                }
                _context.Set<Product>().Add(newProduct);
                await _context.SaveChangesAsync();
                return newProduct;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product", ex);
            }
        }

        public async Task<List<ProductDTO>> GetAllProducts()
        {
            try
            {
                return await _context.Products
                    .Where(o => o.IdStatus == (int)Status.Active)
                    .Include(p => p.IdCategoryNavigation)
                    .Select(p => new ProductDTO
                    {
                        IdProduct = p.IdProduct,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        Category = new CategoryDTO
                        {
                            IdCategory = p.IdCategoryNavigation.IdCategory,
                            Name = p.IdCategoryNavigation.Name
                        }
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving products", ex);
            }
        }

        public async Task<int> DeleteProduct(int id)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    throw new Exception("Product not found");
                }
                existingProduct.IdStatus = (int)Status.Inactive;
                _context.Products.Update(existingProduct);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the product", ex);
            }
        }

        public async Task<int> UpdateProduct(CreateProductDTO Product)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(Product.IdProduct);
                if (existingProduct == null)
                {
                    throw new Exception("Product not found");
                }
                existingProduct.Name = Product.Name;
                existingProduct.Description = Product.Description;
                existingProduct.Price = Product.Price;
                existingProduct.Quantity = Product.Quantity;
                existingProduct.IdCategory = Product.IdCategory;
                if(Product.Quantity < 5)
                {
                    NotifyAdmins(existingProduct);
                }
                _context.Products.Update(existingProduct);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the product", ex);
            }
        }

        public async Task<ProductDTO> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.IdCategoryNavigation)
                    .Where(p => p.IdProduct == id)
                    .Select(p => new ProductDTO
                    {
                        IdProduct = p.IdProduct,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        IdCategory = p.IdCategoryNavigation.IdCategory,
                        Category = new CategoryDTO
                        {
                            IdCategory = p.IdCategoryNavigation.IdCategory,
                            Name = p.IdCategoryNavigation.Name
                        }
                    })
                    .FirstOrDefaultAsync();
                if (product == null)
                {
                    throw new Exception("Product not found");
                }
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the product", ex);
            }
        }

        public async Task<List<ProductDTO>> GetProductsWithLowStock()
        {
            try
            {
                var products = await _context.Products
                    .Where(o => o.IdStatus == (int)Status.Active && o.Quantity < 5)
                    .Include(p => p.IdCategoryNavigation)
                    .Select(p => new ProductDTO
                    {
                        IdProduct = p.IdProduct,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        Category = new CategoryDTO
                        {
                            IdCategory = p.IdCategoryNavigation.IdCategory,
                            Name = p.IdCategoryNavigation.Name
                        }
                    })
                    .ToListAsync();
                if (products == null || !products.Any())
                {
                    throw new Exception("No products found with low stock");
                }
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving products with low stock", ex);
            }
        }

        private async void NotifyAdmins(Product product)
        {

            var administrators = await _context.Users
                    .Where(o => o.IdStatus == (int)Status.Active && o.IdUserRole == (int)Roles.Admin)
                    .ToListAsync();
            var notifications = new List<Notification>();
            foreach (var admin in administrators)
            {
                var newNotification = new Notification
                {
                    Title = "Alerta de nuevo producto con existencias bajas",
                    Description = $"El producto {product.Name} se encuentra con existencias bajas ({product.Quantity} en almacén).",
                    IdAddresse = admin.IdUser,
                    IdStatus = (int)Status.Pending
                };
                notifications.Add(newNotification);
            }
            _context.Set<Notification>().AddRange(notifications);
        }
    }
}
