using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.API.Models.DTOs.Request;
using PersonalBlog.API.Models.DTOs.Response;
using PersonalBlog.API.Repositories.Interface;

namespace PersonalBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            return Ok(response);
        }

        /// <summary>
        /// Get a category by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            var response = _mapper.Map<CategoryResponseDto>(category);
            return Ok(response);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if URL handle already exists
            if (await _categoryRepository.ExistsByUrlHandleAsync(request.UrlHandle))
            {
                return BadRequest($"URL handle '{request.UrlHandle}' already exists.");
            }

            var category = _mapper.Map<Models.Domain.Category>(request);
            var createdCategory = await _categoryRepository.CreateAsync(category);

            var response = _mapper.Map<CategoryResponseDto>(createdCategory);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, response);
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if URL handle already exists (excluding current category)
            if (await _categoryRepository.ExistsByUrlHandleAsync(request.UrlHandle, id))
            {
                return BadRequest($"URL handle '{request.UrlHandle}' already exists.");
            }

            var category = _mapper.Map<Models.Domain.Category>(request);
            var updatedCategory = await _categoryRepository.UpdateAsync(id, category);

            if (updatedCategory == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            var response = _mapper.Map<CategoryResponseDto>(updatedCategory);
            return Ok(response);
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var deleted = await _categoryRepository.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound($"Category with ID {id} not found.");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
