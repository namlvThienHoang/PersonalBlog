using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.Models.DTOs.Request
{
    public class UpdateCategoryRequestDto
    {
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "URL handle is required")]
        [MaxLength(100, ErrorMessage = "URL handle cannot exceed 100 characters")]
        public string UrlHandle { get; set; } = string.Empty;
    }
}
