using BlogCore.Domain;

namespace BlogCore.Ports.Primary;

public interface IAuthorUseCases
{
    Task<Author> CreateAuthorAsync(string name, string surname, string socialSecurityNumber);
    Task<IEnumerable<Author>> GetAllAuthorsAsync();
} 