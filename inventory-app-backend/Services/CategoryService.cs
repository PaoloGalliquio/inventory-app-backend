using inventory_app_backend.Constants;
using inventory_app_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory_app_backend.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> AllCategories();
        Task<int> AddCategory(Category Category);
        Task<int> UpdateCategory(Category Category);
        Task<int> DeleteCategory(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly InventoryContext _context;
        private readonly DbSet<Category> _dbSet;

        public CategoryService(InventoryContext context)
        {
            _context = context;
            _dbSet = context.Set<Category>();
        }

        public async Task<int> AddCategory(Category Category)
        {
            await _context.Categories.AddAsync(Category);
            int afectedRow = await _context.SaveChangesAsync();
            return afectedRow;
        }

        public async Task<List<Category>> AllCategories()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int> DeleteCategory(int id)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                return 0;
            }
            _context.Categories.Remove(Category);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCategory(Category Category)
        {
            _context.Categories.Update(Category);
            return await _context.SaveChangesAsync();
        }
    }
}
