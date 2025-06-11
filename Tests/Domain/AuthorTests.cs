using BlogCore.Domain;
using Xunit;

namespace Tests.Domain;

public class AuthorTests
{
    [Fact]
    public void CreateAuthor_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        var name = "Ellen";
        var surname = "Sano";
        var socialSecurityNumber = "12345678901";

        // Act
        var author = new Author(name, surname, socialSecurityNumber);

        // Assert
        Assert.Equal(name, author.Name);
        Assert.Equal(surname, author.Surname);
        Assert.Equal(socialSecurityNumber, author.SocialSecurityNumber);
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
    public void CreateAuthor_WithInvalidData_ShouldThrowDomainException(string name, string surname, string socialSecurityNumber)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new Author(name, surname, socialSecurityNumber));
    }

    [Theory]
    [InlineData("Ellen", "Sano", "12345678901")]
    [InlineData("Alexander", "Lystad", "98765432109")]
    public void CreateAuthor_WithValidData_ShouldNotThrowException(string name, string surname, string socialSecurityNumber)
    {
        // Act & Assert
        var exception = Record.Exception(() => new Author(name, surname, socialSecurityNumber));
        Assert.Null(exception);
    }
} 