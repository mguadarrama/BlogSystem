using BlogCore.Domain;

namespace BlogCore.Ports.Secondary;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(int id);
    Task<IEnumerable<Author>> GetAllAsync();
    Task AddAsync(Author author);
    Task UpdateAsync(Author author);
    Task DeleteAsync(int id);
} 