using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalBlog.API.Models.Domain;

namespace PersonalBlog.API.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.UrlHandle)
                .IsRequired()
                .HasMaxLength(50);

            // Index for performance
            builder.HasIndex(x => x.UrlHandle)
                .IsUnique();
        }
    }
}
