using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.Models.Domain;
using PersonalBlog.API.Repositories.Interface;

namespace PersonalBlog.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(Guid id, Category category)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingCategory == null)
                return null;

            existingCategory.Name = category.Name;
            existingCategory.UrlHandle = category.UrlHandle;

            await _context.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return false;

            // Check if category has blog posts
            var hasBlogPosts = await _context.BlogPosts
                .AnyAsync(x => x.CategoryId == id);

            if (hasBlogPosts)
                throw new InvalidOperationException("Cannot delete category that contains blog posts.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByUrlHandleAsync(string urlHandle, Guid? excludeId = null)
        {
            var query = _context.Categories.Where(x => x.UrlHandle == urlHandle);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
