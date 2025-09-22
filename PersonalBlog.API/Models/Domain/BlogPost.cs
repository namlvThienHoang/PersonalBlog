using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.Models.Domain
{
    public class BlogPost
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? FeaturedImageUrl { get; set; }

        [Required]
        [MaxLength(200)]
        public string UrlHandle { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        public DateTime PublishedDate { get; set; }

        public bool IsVisible { get; set; } = true;

        // Foreign Key
        public Guid CategoryId { get; set; }

        // Navigation Properties
        public Category Category { get; set; } = null!;
        public ICollection<BlogPostTag> BlogPostTags { get; set; } = new List<BlogPostTag>();
    }
}
