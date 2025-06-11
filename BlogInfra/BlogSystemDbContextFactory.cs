using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlogSystem.Infrastructure;

public class BlogSystemDbContextFactory : IDesignTimeDbContextFactory<BlogSystemDbContext>
{
    public BlogSystemDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlogSystemDbContext>();
        // Use a local connection string for design-time operations
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BlogDb;Trusted_Connection=True;");
        return new BlogSystemDbContext(optionsBuilder.Options);
    }
} 