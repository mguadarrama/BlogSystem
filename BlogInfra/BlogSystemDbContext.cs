using BlogCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure;

public class BlogSystemDbContext : DbContext
{
    public BlogSystemDbContext(DbContextOptions<BlogSystemDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Author> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.AuthorId);

        base.OnModelCreating(modelBuilder);
    }
} 