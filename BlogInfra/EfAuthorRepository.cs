using BlogCore.Domain;
using BlogCore.Ports.Secondary;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure;

public class EfAuthorRepository : IAuthorRepository
{
    private readonly BlogSystemDbContext _context;

    public EfAuthorRepository(BlogSystemDbContext context)
    {
        _context = context;
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task AddAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var author = await GetByIdAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
} 