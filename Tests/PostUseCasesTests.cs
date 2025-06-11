using BlogCore.Domain;
using BlogCore.Ports.Primary;
using BlogCore.Ports.Secondary;
using BlogCore.UseCases;
using Moq;
using Xunit;

namespace Tests;

public class PostUseCasesTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IAuthorRepository> _authorRepositoryMock;
    private readonly PostUseCases _postUseCases;
    private readonly Author _testAuthor;

    public PostUseCasesTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _authorRepositoryMock = new Mock<IAuthorRepository>();
        _postUseCases = new PostUseCases(_postRepositoryMock.Object, _authorRepositoryMock.Object);
        _testAuthor = new Author("Ellen", "Sano", "12345678901");
    }

    [Fact]
    public async Task CreatePostAsync_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        var title = "Test Post";
        var description = "Test Description";
        var content = "Test Content";
        var authorId = 1;

        _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(authorId))
            .ReturnsAsync(_testAuthor);

        _postRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Post>()))
            .Returns(Task.CompletedTask);

        // Act
        var post = await _postUseCases.CreatePostAsync(title, description, content, authorId);

        // Assert
        Assert.NotNull(post);
        Assert.Equal(title, post.Title);
        Assert.Equal(description, post.Description);
        Assert.Equal(content, post.Content);
        Assert.Equal(_testAuthor, post.Author);

        _authorRepositoryMock.Verify(repo => repo.GetByIdAsync(authorId), Times.Once);
        _postRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public async Task CreatePostAsync_WithNonExistentAuthor_ShouldThrowDomainException()
    {
        // Arrange
        var title = "Test Post";
        var description = "Test Description";
        var content = "Test Content";
        var authorId = 999;

        _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(authorId))
            .ReturnsAsync((Author?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            _postUseCases.CreatePostAsync(title, description, content, authorId));
        Assert.Equal($"Author with ID {authorId} not found", exception.Message);

        _authorRepositoryMock.Verify(repo => repo.GetByIdAsync(authorId), Times.Once);
        _postRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public async Task GetPostAsync_WithExistingPost_ShouldReturnPost()
    {
        // Arrange
        var postId = 1;
        var post = new Post("Test Post", "Test Description", "Test Content", _testAuthor);
        
        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()))
            .ReturnsAsync(post);

        // Act
        var result = await _postUseCases.GetPostAsync(postId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(post, result);
        _postRepositoryMock.Verify(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task GetPostAsync_WithNonExistentPost_ShouldReturnNull()
    {
        // Arrange
        var postId = 999;
        
        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()))
            .ReturnsAsync((Post?)null);

        // Act
        var result = await _postUseCases.GetPostAsync(postId);

        // Assert
        Assert.Null(result);
        _postRepositoryMock.Verify(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task ListPostsAsync_ShouldReturnAllPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post("Post 1", "Description 1", "Content 1", _testAuthor),
            new Post("Post 2", "Description 2", "Content 2", _testAuthor)
        };

        _postRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<bool>()))
            .ReturnsAsync(posts);

        // Act
        var result = await _postUseCases.ListPostsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(posts, result);
        _postRepositoryMock.Verify(repo => repo.ListAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePostContentAsync_WithExistingPost_ShouldUpdateContent()
    {
        // Arrange
        var postId = 1;
        var post = new Post("Test Post", "Test Description", "Original Content", _testAuthor);
        var newContent = "Updated Content";

        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()))
            .ReturnsAsync(post);

        _postRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Post>()))
            .Returns(Task.CompletedTask);

        // Act
        await _postUseCases.UpdatePostContentAsync(postId, newContent);

        // Assert
        Assert.Equal(newContent, post.Content);
        _postRepositoryMock.Verify(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()), Times.Once);
        _postRepositoryMock.Verify(repo => repo.UpdateAsync(post), Times.Once);
    }

    [Fact]
    public async Task UpdatePostContentAsync_WithNonExistentPost_ShouldThrowDomainException()
    {
        // Arrange
        var postId = 999;
        var newContent = "Updated Content";

        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()))
            .ReturnsAsync((Post?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            _postUseCases.UpdatePostContentAsync(postId, newContent));
        Assert.Equal($"Post with ID {postId} not found", exception.Message);

        _postRepositoryMock.Verify(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()), Times.Once);
        _postRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public async Task UpdatePostTitleAsync_WithExistingPost_ShouldUpdateTitle()
    {
        // Arrange
        var postId = 1;
        var post = new Post("Original Title", "Test Description", "Test Content", _testAuthor);
        var newTitle = "Updated Title";

        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()))
            .ReturnsAsync(post);

        _postRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Post>()))
            .Returns(Task.CompletedTask);

        // Act
        await _postUseCases.UpdatePostTitleAsync(postId, newTitle);

        // Assert
        Assert.Equal(newTitle, post.Title);
        _postRepositoryMock.Verify(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()), Times.Once);
        _postRepositoryMock.Verify(repo => repo.UpdateAsync(post), Times.Once);
    }

    [Fact]
    public async Task UpdatePostTitleAsync_WithNonExistentPost_ShouldThrowDomainException()
    {
        // Arrange
        var postId = 999;
        var newTitle = "Updated Title";

        _postRepositoryMock.Setup(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()))
            .ReturnsAsync((Post?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            _postUseCases.UpdatePostTitleAsync(postId, newTitle));
        Assert.Equal($"Post with ID {postId} not found", exception.Message);

        _postRepositoryMock.Verify(repo => repo.GetByIdAsync(postId, It.IsAny<bool>()), Times.Once);
        _postRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Post>()), Times.Never);
    }
} 