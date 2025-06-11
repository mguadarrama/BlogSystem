using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlogCore.Ports.Primary;
using BlogCore.Ports.Secondary;
using BlogCore.UseCases;
using Microsoft.Extensions.Configuration;
using BlogSystem.Infrastructure;
using BlogSystem.Infrastructure.Data;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        // Add Entity Framework
        builder.Services.AddDbContext<BlogSystemDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Register repositories
        builder.Services.AddScoped<IPostRepository, EfPostRepository>();
        builder.Services.AddScoped<IAuthorRepository, EfAuthorRepository>();

        // Register application services
        builder.Services.AddScoped<IPostUseCases, PostUseCases>();
        builder.Services.AddScoped<IAuthorUseCases, AuthorUseCases>();

        // Register DbSeeder
        builder.Services.AddScoped<DbSeeder>();

        // Add controllers
        builder.Services.AddControllers();

        // Add Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Ensure database is created and seeded
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BlogSystemDbContext>();
            context.Database.EnsureCreated();
            
            // Seed the database
            var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
            seeder.SeedAsync().Wait();
        }

        // Use controller routing
        app.MapControllers();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.Run();
    }
}