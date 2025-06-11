using BlogCore.Domain;

namespace BlogCore.Ports.Secondary;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(int id, bool includeAuthor = true);
    Task<IEnumerable<Post>> ListAsync(bool includeAuthor = true);
    Task AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(int id);
} 