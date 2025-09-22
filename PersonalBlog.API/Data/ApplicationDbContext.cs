using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data.Configurations;
using PersonalBlog.API.Models.Domain;

namespace PersonalBlog.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostTag> BlogPostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new BlogPostConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new BlogPostTagConfiguration());

            // Seed initial data
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            var categories = new List<Category>
            {
                new() { Id =  Guid.NewGuid(), Name = "Technology", UrlHandle = "technology" },
                new() { Id =  Guid.NewGuid(), Name = "Programming", UrlHandle = "programming" },
                new() { Id = Guid.NewGuid(), Name = "Web Development", UrlHandle = "web-development" }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Seed Tags
            var tags = new List<Tag>
            {
                new() { Id =  Guid.NewGuid(), Name = "C#", UrlHandle = "csharp" },
                new() { Id =  Guid.NewGuid(), Name = ".NET", UrlHandle = "dotnet" },
                new() { Id =  Guid.NewGuid(), Name = "Entity Framework", UrlHandle = "entity-framework" },
                new() { Id =  Guid.NewGuid(), Name = "API", UrlHandle = "api" },
                new() { Id =  Guid.NewGuid(), Name = "React", UrlHandle = "react" }
            };

            modelBuilder.Entity<Tag>().HasData(tags);
        }
    }
}
