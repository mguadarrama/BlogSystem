using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using BlogSystem.Infrastructure;
using BlogCore.Ports.Secondary;
using BlogCore.Ports.Primary;
using BlogCore.UseCases;
using Microsoft.AspNetCore.Hosting;

namespace Tests.Integration;

public class TestFixture : IDisposable
{
    public WebApplicationFactory<Program> WebAppFactory { get; }
    public BlogSystemDbContext DbContext { get; }
    public IAuthorRepository AuthorRepository { get; }
    public IAuthorUseCases AuthorUseCases { get; }
    private IDbContextTransaction _transaction;

    public TestFixture()
    {
        WebAppFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseStartup<TestStartup>();
            });

        var scope = WebAppFactory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<BlogSystemDbContext>();
        AuthorRepository = scope.ServiceProvider.GetRequiredService<IAuthorRepository>();
        AuthorUseCases = scope.ServiceProvider.GetRequiredService<IAuthorUseCases>();

        // Ensure database is created
        DbContext.Database.EnsureCreated();

        // Start a transaction
        _transaction = DbContext.Database.BeginTransaction();
    }

    public void Dispose()
    {
        // Rollback the transaction
        _transaction?.Rollback();
        _transaction?.Dispose();

        // Clean up the database
        DbContext.Database.EnsureDeleted();
        WebAppFactory.Dispose();
    }
} 