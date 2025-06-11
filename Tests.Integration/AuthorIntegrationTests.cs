using BlogCore.Domain;
using Xunit;

namespace Tests.Integration;

public class AuthorIntegrationTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;

    public AuthorIntegrationTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateAuthor_ShouldPersistToDatabase()
    {
        // Arrange
        var name = "Ellen";
        var surname = "Sano";
        var socialSecurityNumber = "12345678901";

        // Act
        var author = await _fixture.AuthorUseCases.CreateAuthorAsync(name, surname, socialSecurityNumber);

        // Assert
        var savedAuthor = await _fixture.AuthorRepository.GetByIdAsync(author.Id);
        Assert.NotNull(savedAuthor);
        Assert.Equal(name, savedAuthor.Name);
        Assert.Equal(surname, savedAuthor.Surname);
        Assert.Equal(socialSecurityNumber, savedAuthor.SocialSecurityNumber);
    }

    [Fact]
    public async Task GetAllAuthors_ShouldReturnAllAuthors()
    {
        // Arrange
        var author1 = await _fixture.AuthorUseCases.CreateAuthorAsync("Ellen", "Sano", "12345678901");
        var author2 = await _fixture.AuthorUseCases.CreateAuthorAsync("Alexander", "Lystad", "98765432109");

        // Act
        var authors = await _fixture.AuthorUseCases.GetAllAuthorsAsync();

        // Assert
        Assert.Contains(authors, a => a.Id == author1.Id);
        Assert.Contains(authors, a => a.Id == author2.Id);
    }

    [Fact]
    public async Task CreateAuthor_WithInvalidData_ShouldThrowDomainException()
    {
        // Arrange
        var name = "Ellen";
        var surname = "Sano";
        var invalidSocialSecurityNumber = "123"; // Too short

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() =>
            _fixture.AuthorUseCases.CreateAuthorAsync(name, surname, invalidSocialSecurityNumber));
    }

    [Fact]
    public async Task CreateAuthor_WithDuplicateSocialSecurityNumber_ShouldSucceed()
    {
        // Arrange
        var socialSecurityNumber = "12345678901";
        await _fixture.AuthorUseCases.CreateAuthorAsync("Ellen", "Sano", socialSecurityNumber);

        // Act & Assert
        // Note: This test assumes that duplicate SSNs are allowed.
        // If your business rules change to prevent duplicates, this test should be updated.
        var author = await _fixture.AuthorUseCases.CreateAuthorAsync("Alexander", "Lystad", socialSecurityNumber);
        Assert.NotNull(author);
        Assert.Equal(socialSecurityNumber, author.SocialSecurityNumber);
    }
} 