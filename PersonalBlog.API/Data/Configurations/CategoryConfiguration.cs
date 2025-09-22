using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalBlog.API.Models.Domain;

namespace PersonalBlog.API.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.UrlHandle)
                .IsRequired()
                .HasMaxLength(100);

            // Index for performance
            builder.HasIndex(x => x.UrlHandle)
                .IsUnique();
        }
    }
}
