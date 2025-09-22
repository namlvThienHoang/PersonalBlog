namespace PersonalBlog.API.Models.DTOs.Request
{
    public class BlogPostFilterDto
    {
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public List<Guid>? TagIds { get; set; }
        public bool? IsVisible { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "PublishedDate"; // PublishedDate, Title
        public bool IsDescending { get; set; } = true;

        public void ValidatePagination()
        {
            if (PageNumber < 1) PageNumber = 1;
            if (PageSize < 1) PageSize = 10;
            if (PageSize > 100) PageSize = 100; // Limit max page size
        }
    }
}
