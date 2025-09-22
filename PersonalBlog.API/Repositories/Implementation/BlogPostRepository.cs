using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.Models.Domain;
using PersonalBlog.API.Models.DTOs.Request;
using PersonalBlog.API.Models.DTOs.Response;
using PersonalBlog.API.Repositories.Interface;

namespace PersonalBlog.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDto<BlogPost>> GetAllAsync(BlogPostFilterDto filter)
        {
            filter.ValidatePagination();

            var query = _context.BlogPosts
                .Include(x => x.Category)
                .Include(x => x.BlogPostTags)
                .ThenInclude(x => x.Tag)
                .AsQueryable();

            // Apply filters
            if (filter.IsVisible.HasValue)
            {
                query = query.Where(x => x.IsVisible == filter.IsVisible.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.Trim().ToLower();
                query = query.Where(x =>
                    x.Title.ToLower().Contains(searchTerm) ||
                    x.Content.ToLower().Contains(searchTerm) ||
                    x.ShortDescription.ToLower().Contains(searchTerm));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == filter.CategoryId.Value);
            }

            if (filter.TagIds != null && filter.TagIds.Any())
            {
                query = query.Where(x => x.BlogPostTags.Any(bpt => filter.TagIds.Contains(bpt.TagId)));
            }

            // Apply sorting
            query = filter.SortBy?.ToLower() switch
            {
                "title" => filter.IsDescending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title),
                "publisheddate" => filter.IsDescending ? query.OrderByDescending(x => x.PublishedDate) : query.OrderBy(x => x.PublishedDate),
                _ => filter.IsDescending ? query.OrderByDescending(x => x.PublishedDate) : query.OrderBy(x => x.PublishedDate)
            };

            // Get total count for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var blogPosts = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResultDto<BlogPost>
            {
                Data = blogPosts,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await _context.BlogPosts
                .Include(x => x.Category)
                .Include(x => x.BlogPostTags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _context.BlogPosts
                .Include(x => x.Category)
                .Include(x => x.BlogPostTags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost, List<Guid> tagIds)
        {
            // Add the blog post
            await _context.BlogPosts.AddAsync(blogPost);

            // Add tags if any
            if (tagIds.Any())
            {
                var blogPostTags = tagIds.Select(tagId => new BlogPostTag
                {
                    BlogPostId = blogPost.Id,
                    TagId = tagId
                }).ToList();

                await _context.BlogPostTags.AddRangeAsync(blogPostTags);
            }

            await _context.SaveChangesAsync();

            // Return with includes
            return await GetByIdAsync(blogPost.Id) ?? blogPost;
        }

        public async Task<BlogPost?> UpdateAsync(Guid id, BlogPost blogPost, List<Guid> tagIds)
        {
            var existingBlogPost = await _context.BlogPosts
                .Include(x => x.BlogPostTags)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingBlogPost == null)
                return null;

            // Update blog post properties
            existingBlogPost.Title = blogPost.Title;
            existingBlogPost.ShortDescription = blogPost.ShortDescription;
            existingBlogPost.Content = blogPost.Content;
            existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
            existingBlogPost.UrlHandle = blogPost.UrlHandle;
            existingBlogPost.Author = blogPost.Author;
            existingBlogPost.PublishedDate = blogPost.PublishedDate;
            existingBlogPost.IsVisible = blogPost.IsVisible;
            existingBlogPost.CategoryId = blogPost.CategoryId;

            // Update tags - remove existing and add new ones
            _context.BlogPostTags.RemoveRange(existingBlogPost.BlogPostTags);

            if (tagIds.Any())
            {
                var newBlogPostTags = tagIds.Select(tagId => new BlogPostTag
                {
                    BlogPostId = id,
                    TagId = tagId
                }).ToList();

                await _context.BlogPostTags.AddRangeAsync(newBlogPostTags);
            }

            await _context.SaveChangesAsync();

            // Return with includes
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var blogPost = await _context.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (blogPost == null)
                return false;

            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByUrlHandleAsync(string urlHandle, Guid? excludeId = null)
        {
            var query = _context.BlogPosts.Where(x => x.UrlHandle == urlHandle);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
