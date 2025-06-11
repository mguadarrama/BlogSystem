using BlogCore.Domain;
using Xunit;

namespace Tests;

public class PostTests
{
    private readonly Author _testAuthor;

    public PostTests()
    {
        _testAuthor = new Author("Ellen", "Sano", "12345678901");
    }

    [Fact]
    public void CreatePost_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        var title = "Test Post";
        var description = "Test Description";
        var content = "Test Content";

        // Act
        var post = new Post(title, description, content, _testAuthor);

        // Assert
        Assert.Equal(title, post.Title);
        Assert.Equal(description, post.Description);
        Assert.Equal(content, post.Content);
        Assert.Equal(_testAuthor, post.Author);
        Assert.Equal(_testAuthor.Id, post.AuthorId);
        Assert.NotEqual(default, post.CreatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void CreatePost_WithInvalidTitle_ShouldThrowDomainException(string invalidTitle)
    {
        // Arrange
        var description = "Test Description";
        var content = "Test Content";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new Post(invalidTitle, description, content, _testAuthor));
        Assert.Equal("Title cannot be empty", exception.Message);
    }

    [Fact]
    public void CreatePost_WithTitleTooLong_ShouldThrowDomainException()
    {
        // Arrange
        var tooLongTitle = new string('a', 201); // 201 characters
        var description = "Test Description";
        var content = "Test Content";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new Post(tooLongTitle, description, content, _testAuthor));
        Assert.Equal("Title cannot be longer than 200 characters", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void CreatePost_WithInvalidContent_ShouldThrowDomainException(string invalidContent)
    {
        // Arrange
        var title = "Test Post";
        var description = "Test Description";

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => 
            new Post(title, description, invalidContent, _testAuthor));
        Assert.Equal("Content cannot be empty", exception.Message);
    }

    [Fact]
    public void UpdateContent_WithValidContent_ShouldUpdateSuccessfully()
    {
        // Arrange
        var post = new Post("Test Post", "Test Description", "Original Content", _testAuthor);
        var newContent = "Updated Content";

        // Act
        post.UpdateContent(newContent);

        // Assert
        Assert.Equal(newContent, post.Content);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateContent_WithInvalidContent_ShouldThrowDomainException(string invalidContent)
    {
        // Arrange
        var post = new Post("Test Post", "Test Description", "Original Content", _testAuthor);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => post.UpdateContent(invalidContent));
        Assert.Equal("Content cannot be empty", exception.Message);
    }

    [Fact]
    public void UpdateTitle_WithValidTitle_ShouldUpdateSuccessfully()
    {
        // Arrange
        var post = new Post("Original Title", "Test Description", "Test Content", _testAuthor);
        var newTitle = "Updated Title";

        // Act
        post.UpdateTitle(newTitle);

        // Assert
        Assert.Equal(newTitle, post.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateTitle_WithInvalidTitle_ShouldThrowDomainException(string invalidTitle)
    {
        // Arrange
        var post = new Post("Original Title", "Test Description", "Test Content", _testAuthor);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => post.UpdateTitle(invalidTitle));
        Assert.Equal("Title cannot be empty", exception.Message);
    }

    [Fact]
    public void UpdateTitle_WithTitleTooLong_ShouldThrowDomainException()
    {
        // Arrange
        var post = new Post("Original Title", "Test Description", "Test Content", _testAuthor);
        var tooLongTitle = new string('a', 201); // 201 characters

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => post.UpdateTitle(tooLongTitle));
        Assert.Equal("Title cannot be longer than 200 characters", exception.Message);
    }
} 