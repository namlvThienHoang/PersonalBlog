using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.API.Models.DTOs.Response;
using PersonalBlog.API.Repositories.Interface;

namespace PersonalBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagsController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all tags
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagResponseDto>>> GetAllTags()
        {
            var tags = await _tagRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<TagResponseDto>>(tags);
            return Ok(response);
        }

        /// <summary>
        /// Get a tag by ID
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TagResponseDto>> GetTagById(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound($"Tag with ID {id} not found.");
            }

            var response = _mapper.Map<TagResponseDto>(tag);
            return Ok(response);
        }
    }
}
