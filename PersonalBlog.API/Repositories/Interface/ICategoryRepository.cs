using PersonalBlog.API.Models.Domain;

namespace PersonalBlog.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category> CreateAsync(Category category);
        Task<Category?> UpdateAsync(Guid id, Category category);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsByUrlHandleAsync(string urlHandle, Guid? excludeId = null);
    }
}
