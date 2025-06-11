using BlogCore.Domain;
using BlogCore.Ports.Primary;
using BlogCore.Ports.Secondary;

namespace BlogCore.UseCases;

public class PostUseCases : IPostUseCases
{
    private readonly IPostRepository _postRepository;
    private readonly IAuthorRepository _authorRepository;

    public PostUseCases(IPostRepository postRepository, IAuthorRepository authorRepository)
    {
        _postRepository = postRepository;
        _authorRepository = authorRepository;
    }

    public async Task<Post> CreatePostAsync(string title, string description, string content, int authorId)
    {
        var author = await _authorRepository.GetByIdAsync(authorId) 
            ?? throw new DomainException($"Author with ID {authorId} not found");
        var post = new Post(title, description, content, author);
        await _postRepository.AddAsync(post);
        return post;
    }

    public async Task<Post?> GetPostAsync(int id, bool includeAuthor = true)
    {
        return await _postRepository.GetByIdAsync(id, includeAuthor);
    }

    public async Task<IEnumerable<Post>> ListPostsAsync()
    {
        return await _postRepository.ListAsync(true);
    }

    public async Task UpdatePostContentAsync(int id, string newContent)
    {
        var post = await _postRepository.GetByIdAsync(id) 
            ?? throw new DomainException($"Post with ID {id} not found");
        post.UpdateContent(newContent);
        await _postRepository.UpdateAsync(post);
    }

    public async Task UpdatePostTitleAsync(int id, string newTitle)
    {
        var post = await _postRepository.GetByIdAsync(id) 
            ?? throw new DomainException($"Post with ID {id} not found");
        post.UpdateTitle(newTitle);
        await _postRepository.UpdateAsync(post);
    }
} 