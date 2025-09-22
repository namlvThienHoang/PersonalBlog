using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.Models.DTOs.Request
{
    public class UpdateBlogPostRequestDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Short description cannot exceed 500 characters")]
        public string ShortDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;

        public string? FeaturedImageUrl { get; set; }

        [Required(ErrorMessage = "URL handle is required")]
        [MaxLength(200, ErrorMessage = "URL handle cannot exceed 200 characters")]
        public string UrlHandle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required")]
        [MaxLength(100, ErrorMessage = "Author name cannot exceed 100 characters")]
        public string Author { get; set; } = string.Empty;

        public DateTime? PublishedDate { get; set; }

        public bool IsVisible { get; set; } = true;

        [Required(ErrorMessage = "Category ID is required")]
        public Guid CategoryId { get; set; }

        public List<Guid> TagIds { get; set; } = new List<Guid>();
    }
}
