namespace PersonalBlog.API.Models.DTOs.Response
{
    public class BlogPostResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public bool IsVisible { get; set; }

        // Related data
        public CategoryResponseDto Category { get; set; } = null!;
        public List<TagResponseDto> Tags { get; set; } = new List<TagResponseDto>();
    }
}
