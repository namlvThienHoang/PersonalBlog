using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.API.Models.DTOs.Request;
using PersonalBlog.API.Models.DTOs.Response;
using PersonalBlog.API.Repositories.Interface;

namespace PersonalBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public BlogPostsController(
            IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            IMapper mapper)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all blog posts with filtering, searching, and pagination
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<BlogPostResponseDto>>> GetAllBlogPosts([FromQuery] BlogPostFilterDto filter)
        {
            var pagedResult = await _blogPostRepository.GetAllAsync(filter);

            var response = new PagedResultDto<BlogPostResponseDto>
            {
                Data = _mapper.Map<List<BlogPostResponseDto>>(pagedResult.Data),
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };

            return Ok(response);
        }

        /// <summary>
        /// Get a blog post by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BlogPostResponseDto>> GetBlogPostById(Guid id)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);
            if (blogPost == null)
            {
                return NotFound($"Blog post with ID {id} not found.");
            }

            var response = _mapper.Map<BlogPostResponseDto>(blogPost);
            return Ok(response);
        }

        /// <summary>
        /// Get a blog post by URL handle
        /// </summary>
        [HttpGet("url-handle/{urlHandle}")]
        public async Task<ActionResult<BlogPostResponseDto>> GetBlogPostByUrlHandle(string urlHandle)
        {
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
            if (blogPost == null)
            {
                return NotFound($"Blog post with URL handle '{urlHandle}' not found.");
            }

            var response = _mapper.Map<BlogPostResponseDto>(blogPost);
            return Ok(response);
        }

        /// <summary>
        /// Create a new blog post
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BlogPostResponseDto>> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if URL handle already exists
            if (await _blogPostRepository.ExistsByUrlHandleAsync(request.UrlHandle))
            {
                return BadRequest($"URL handle '{request.UrlHandle}' already exists.");
            }

            // Validate category exists
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return BadRequest($"Category with ID {request.CategoryId} not found.");
            }

            // Validate tags exist
            if (request.TagIds.Any())
            {
                var existingTags = await _tagRepository.GetByIdsAsync(request.TagIds);
                var existingTagIds = existingTags.Select(t => t.Id).ToList();
                var invalidTagIds = request.TagIds.Except(existingTagIds).ToList();

                if (invalidTagIds.Any())
                {
                    return BadRequest($"Tags with IDs [{string.Join(", ", invalidTagIds)}] not found.");
                }
            }

            var blogPost = _mapper.Map<Models.Domain.BlogPost>(request);
            var createdBlogPost = await _blogPostRepository.CreateAsync(blogPost, request.TagIds);

            var response = _mapper.Map<BlogPostResponseDto>(createdBlogPost);
            return CreatedAtAction(nameof(GetBlogPostById), new { id = createdBlogPost.Id }, response);
        }

        /// <summary>
        /// Update an existing blog post
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<BlogPostResponseDto>> UpdateBlogPost(Guid id, [FromBody] UpdateBlogPostRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if URL handle already exists (excluding current post)
            if (await _blogPostRepository.ExistsByUrlHandleAsync(request.UrlHandle, id))
            {
                return BadRequest($"URL handle '{request.UrlHandle}' already exists.");
            }

            // Validate category exists
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return BadRequest($"Category with ID {request.CategoryId} not found.");
            }

            // Validate tags exist
            if (request.TagIds.Any())
            {
                var existingTags = await _tagRepository.GetByIdsAsync(request.TagIds);
                var existingTagIds = existingTags.Select(t => t.Id).ToList();
                var invalidTagIds = request.TagIds.Except(existingTagIds).ToList();

                if (invalidTagIds.Any())
                {
                    return BadRequest($"Tags with IDs [{string.Join(", ", invalidTagIds)}] not found.");
                }
            }

            var blogPost = _mapper.Map<Models.Domain.BlogPost>(request);
            var updatedBlogPost = await _blogPostRepository.UpdateAsync(id, blogPost, request.TagIds);

            if (updatedBlogPost == null)
            {
                return NotFound($"Blog post with ID {id} not found.");
            }

            var response = _mapper.Map<BlogPostResponseDto>(updatedBlogPost);
            return Ok(response);
        }

        /// <summary>
        /// Delete a blog post
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteBlogPost(Guid id)
        {
            var deleted = await _blogPostRepository.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound($"Blog post with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
