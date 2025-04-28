using inventory_app_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory_app_backend.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> AddProduct(Product Product);
        Task<int> UpdateProduct(Product Product);
        Task<int> DeleteProduct(int id);
    }

    public class ProductService : IProductService
    {
        private readonly InventoryContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductService(InventoryContext context)
        {
            _context = context;
            _dbSet = context.Set<Product>();
        }

        public async Task<Product> AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product", ex);
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int> DeleteProduct(int id)
        {
            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                return 0;
            }
            _context.Products.Remove(Product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateProduct(Product Product)
        {
            _context.Products.Update(Product);
            return await _context.SaveChangesAsync();
        }
    }
}
