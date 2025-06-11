using BlogCore.Domain;
using BlogCore.Ports.Secondary;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure;

public class EfPostRepository : IPostRepository
{
    private readonly BlogSystemDbContext _context;

    public EfPostRepository(BlogSystemDbContext context)
    {
        _context = context;
    }

    public async Task<Post?> GetByIdAsync(int id, bool includeAuthor = true)
    {
        var query = _context.Posts.AsQueryable();
        if (includeAuthor)
            query = query.Include(p => p.Author);
            
        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Post>> ListAsync(bool includeAuthor = true)
    {
        var query = _context.Posts.AsQueryable();
        if (includeAuthor)
            query = query.Include(p => p.Author);
            
        return await query.ToListAsync();
    }

    public async Task AddAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var post = await GetByIdAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
} 