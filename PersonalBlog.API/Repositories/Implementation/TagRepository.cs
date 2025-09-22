using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.Models.Domain;
using PersonalBlog.API.Repositories.Interface;

namespace PersonalBlog.API.Repositories.Implementation
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(Guid id)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Tag>> GetByIdsAsync(List<Guid> tagIds)
        {
            return await _context.Tags
                .Where(x => tagIds.Contains(x.Id))
                .ToListAsync();
        }
    }
}
