using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace BlogCore.Domain;

public class Post
{
    public Post() { } // For EF Core and testing

    public Post(string title, string description, string content, Author author)
    {
        ValidateTitle(title);
        ValidateContent(content);
        
        Title = title;
        Description = description;
        Content = content;
        Author = author;
        AuthorId = author.Id;
        CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; internal set; }
    public int AuthorId { get; internal set; }
    public string Title { get; internal set; }
    public string Description { get; internal set; }
    public string Content { get; internal set; }
    public DateTime CreatedAt { get; internal set; }
    public Author? Author { get; internal set; }

    private void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty");
        if (title.Length > 200)
            throw new DomainException("Title cannot be longer than 200 characters");
    }

    private void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Content cannot be empty");
    }

    public void UpdateContent(string newContent)
    {
        ValidateContent(newContent);
        Content = newContent;
    }

    public void UpdateTitle(string newTitle)
    {
        ValidateTitle(newTitle);
        Title = newTitle;
    }
} 