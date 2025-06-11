using BlogCore.Domain;

namespace BlogCore.Ports.Primary;

public interface IPostUseCases
{
    Task<Post> CreatePostAsync(string title, string description, string content, int authorId);
    Task<Post?> GetPostAsync(int id, bool includeAuthor = true);
    Task<IEnumerable<Post>> ListPostsAsync();
    Task UpdatePostContentAsync(int id, string newContent);
    Task UpdatePostTitleAsync(int id, string newTitle);
} 