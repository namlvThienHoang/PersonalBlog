using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string UrlHandle { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
