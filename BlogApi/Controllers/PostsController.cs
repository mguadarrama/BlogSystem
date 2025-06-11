using BlogCore.Domain;
using BlogCore.Ports.Primary;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostUseCases _postUseCases;

    public PostsController(IPostUseCases postUseCases)
    {
        _postUseCases = postUseCases;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        try
        {
            var post = await _postUseCases.CreatePostAsync(
                request.Title,
                request.Description,
                request.Content,
                request.AuthorId
            );
            
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id, [FromQuery] bool includeAuthor = true)
    {
        var post = await _postUseCases.GetPostAsync(id, includeAuthor);
        return post is not null ? Ok(post) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> ListPosts()
    {
        var posts = await _postUseCases.ListPostsAsync();
        return Ok(posts);
    }

    [HttpPut("{id}/content")]
    public async Task<IActionResult> UpdateContent(int id, [FromBody] UpdateContentRequest request)
    {
        try
        {
            await _postUseCases.UpdatePostContentAsync(id, request.Content);
            return NoContent();
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record CreatePostRequest(string Title, string Description, string Content, int AuthorId);
public record UpdateContentRequest(string Content); 