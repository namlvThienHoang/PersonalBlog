using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string UrlHandle { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<BlogPostTag> BlogPostTags { get; set; } = new List<BlogPostTag>();
    }
}
