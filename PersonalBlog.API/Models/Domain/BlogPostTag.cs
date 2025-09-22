namespace PersonalBlog.API.Models.Domain
{
    /// <summary>
    /// Junction table for Many-to-Many relationship between BlogPost and Tag
    /// </summary>
    public class BlogPostTag
    {
        public Guid BlogPostId { get; set; }
        public Guid TagId { get; set; }

        // Navigation Properties
        public BlogPost BlogPost { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
