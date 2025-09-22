using AutoMapper;
using PersonalBlog.API.Models.Domain;
using PersonalBlog.API.Models.DTOs.Request;
using PersonalBlog.API.Models.DTOs.Response;

namespace PersonalBlog.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // BlogPost mappings
            CreateMap<BlogPost, BlogPostResponseDto>()
                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(src => src.BlogPostTags.Select(bpt => bpt.Tag)));

            CreateMap<CreateBlogPostRequestDto, BlogPost>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.PublishedDate,
                    opt => opt.MapFrom(src => src.PublishedDate ?? DateTime.UtcNow))
                .ForMember(dest => dest.BlogPostTags, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<UpdateBlogPostRequestDto, BlogPost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PublishedDate,
                    opt => opt.MapFrom(src => src.PublishedDate ?? DateTime.UtcNow))
                .ForMember(dest => dest.BlogPostTags, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            // Category mappings
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<CreateCategoryRequestDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.BlogPosts, opt => opt.Ignore());
            CreateMap<UpdateCategoryRequestDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BlogPosts, opt => opt.Ignore());

            // Tag mappings
            CreateMap<Tag, TagResponseDto>();
        }
    }
}
