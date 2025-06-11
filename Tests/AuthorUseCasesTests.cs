using BlogCore.Domain;
using BlogCore.Ports.Secondary;
using BlogCore.UseCases;
using Moq;
using Xunit;

namespace Tests;

public class AuthorUseCasesTests
{
    private readonly Mock<IAuthorRepository> _mockAuthorRepository;
    private readonly AuthorUseCases _authorUseCases;

    public AuthorUseCasesTests()
    {
        _mockAuthorRepository = new Mock<IAuthorRepository>();
        _authorUseCases = new AuthorUseCases(_mockAuthorRepository.Object);
    }

    [Fact]
    public async Task CreateAuthorAsync_WithValidData_ShouldCreateAndReturnAuthor()
    {
        // Arrange
        var name = "Ellen";
        var surname = "Sano";
        var socialSecurityNumber = "12345678901";
        var author = new Author(name, surname, socialSecurityNumber);

        _mockAuthorRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Author>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _authorUseCases.CreateAuthorAsync(name, surname, socialSecurityNumber);

        // Assert
        Assert.Equal(name, result.Name);
        Assert.Equal(surname, result.Surname);
        Assert.Equal(socialSecurityNumber, result.SocialSecurityNumber);
        _mockAuthorRepository.Verify(repo => repo.AddAsync(It.IsAny<Author>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAuthorsAsync_ShouldReturnAllAuthors()
    {
        // Arrange
        var expectedAuthors = new List<Author>
        {
            new Author("Ellen", "Sano", "12345678901"),
            new Author("Alexander", "Lystad", "98765432109")
        };

        _mockAuthorRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedAuthors);

        // Act
        var result = await _authorUseCases.GetAllAuthorsAsync();

        // Assert
        Assert.Equal(expectedAuthors, result);
        _mockAuthorRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Theory]
    [InlineData("", "Sano", "12345678901")]
    [InlineData(null, "Sano", "12345678901")]
    [InlineData(" ", "Sano", "12345678901")]
    [InlineData("Ellen", "", "12345678901")]
    [InlineData("Ellen", null, "12345678901")]
    [InlineData("Ellen", " ", "12345678901")]
    [InlineData("Ellen", "Sano", "")]
    [InlineData("Ellen", "Sano", null)]
    [InlineData("Ellen", "Sano", " ")]
    [InlineData("Ellen", "Sano", "123456789")] // Too short
    [InlineData("Ellen", "Sano", "123456789012")] // Too long
    [InlineData("Ellen", "Sano", "1234567890a")] // Contains non-digit
    public async Task CreateAuthorAsync_WithInvalidData_ShouldThrowDomainException(string name, string surname, string socialSecurityNumber)
    {
        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => 
            _authorUseCases.CreateAuthorAsync(name, surname, socialSecurityNumber));
    }
} 