using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlogSystem.Infrastructure;
using BlogCore.Ports.Secondary;
using BlogCore.Ports.Primary;
using BlogCore.UseCases;

namespace Tests.Integration;

public class TestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<BlogSystemDbContext>(options =>
        {
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BlogDb_Test;Trusted_Connection=True;");
        });

        services.AddScoped<IAuthorRepository, EfAuthorRepository>();
        services.AddScoped<IAuthorUseCases, AuthorUseCases>();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
} 