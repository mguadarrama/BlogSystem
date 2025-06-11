using BlogCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure.Data;

public class DbSeeder
{
    private readonly BlogSystemDbContext _context;

    public DbSeeder(BlogSystemDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!await _context.Authors.AnyAsync())
        {
            var authors = new List<Author>
            {
                new Author("Ellen", "Sano", "12345678901"),
                new Author("Alexander", "Lystad", "98765432109")
            };

            await _context.Authors.AddRangeAsync(authors);
            await _context.SaveChangesAsync();
        }
    }
} 