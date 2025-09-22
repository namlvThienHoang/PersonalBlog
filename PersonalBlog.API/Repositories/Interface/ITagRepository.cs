using PersonalBlog.API.Models.Domain;

namespace PersonalBlog.API.Repositories.Interface
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(Guid id);
        Task<IEnumerable<Tag>> GetByIdsAsync(List<Guid> tagIds);
    }
}
