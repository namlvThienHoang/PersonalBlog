using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalBlog.API.Models.Domain;

namespace PersonalBlog.API.Data.Configurations
{
    public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.UrlHandle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Author)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.ShortDescription)
                .HasMaxLength(500);

            builder.Property(x => x.PublishedDate)
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            // Relationships
            builder.HasOne(x => x.Category)
                .WithMany(x => x.BlogPosts)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for performance
            builder.HasIndex(x => x.UrlHandle)
                .IsUnique();

            builder.HasIndex(x => x.PublishedDate);
            builder.HasIndex(x => x.IsVisible);
        }
    }
}
