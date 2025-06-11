using BlogCore.Domain;
using BlogCore.Ports.Primary;
using BlogCore.Ports.Secondary;

namespace BlogCore.UseCases;

public class AuthorUseCases : IAuthorUseCases
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorUseCases(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<Author> CreateAuthorAsync(string name, string surname, string socialSecurityNumber)
    {
        var author = new Author(name, surname, socialSecurityNumber);
        await _authorRepository.AddAsync(author);
        return author;
    }

    public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
    {
        return await _authorRepository.GetAllAsync();
    }
} 