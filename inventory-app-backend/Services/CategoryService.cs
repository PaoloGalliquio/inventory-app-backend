using inventory_app_backend.Constants;
using inventory_app_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace inventory_app_backend.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> AddCategory(Category Category);
        Task<int> UpdateCategory(Category Category);
        Task<int> DeleteCategory(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly InventoryContext _context;

        public CategoryService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving categories", ex);
            }
        }

        public async Task<Category> AddCategory(Category category)
        {
            try
            {
                _context.Set<Category>().Add(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the category", ex);
            }
        }

        public async Task<int> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }
                _context.Categories.Remove(category);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the category", ex);
            }
        }

        public async Task<int> UpdateCategory(Category Category)
        {
            try
            {
                var existingCategory = await _context.Categories.FindAsync(Category.IdCategory);
                if (existingCategory == null)
                {
                    throw new Exception("Category not found");
                }
                existingCategory.Name = Category.Name;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the category", ex);
            }
        }
    }
}
