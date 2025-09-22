using PersonalBlog.API.Models.Domain;
using PersonalBlog.API.Models.DTOs.Request;
using PersonalBlog.API.Models.DTOs.Response;

namespace PersonalBlog.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<PagedResultDto<BlogPost>> GetAllAsync(BlogPostFilterDto filter);
        Task<BlogPost?> GetByIdAsync(Guid id);
        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);
        Task<BlogPost> CreateAsync(BlogPost blogPost, List<Guid> tagIds);
        Task<BlogPost?> UpdateAsync(Guid id, BlogPost blogPost, List<Guid> tagIds);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsByUrlHandleAsync(string urlHandle, Guid? excludeId = null);
    }
}
