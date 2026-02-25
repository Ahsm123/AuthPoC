using AuthPoC.Core.Entities;
using AuthPoC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthPoC.WebApi.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync())
            return;

        var users = new List<User>
        {
            new() { Username = "subscriber_user", PasswordHash = BCrypt.Net.BCrypt.HashPassword("subscriber123"), Role = "Subscriber" },
            new() { Username = "editor_user",     PasswordHash = BCrypt.Net.BCrypt.HashPassword("editor123"),     Role = "Editor" },
            new() { Username = "writer_user",     PasswordHash = BCrypt.Net.BCrypt.HashPassword("writer123"),     Role = "Writer" },
        };

        db.Users.AddRange(users);
        await db.SaveChangesAsync();

        var writer = users.First(u => u.Role == "Writer");
        var article = new Article
        {
            Title = "Breaking News: Authorization is Important",
            Content = "Fine-grained authorization is essential for real-world applications...",
            AuthorId = writer.UserId,
            CreatedAt = DateTime.UtcNow,
        };

        db.Articles.Add(article);
        await db.SaveChangesAsync();

        var subscriber = users.First(u => u.Role == "Subscriber");
        db.Comments.Add(new Comment
        {
            Content = "Great article!",
            ArticleId = article.ArticleId,
            UserId = subscriber.UserId,
            CreatedAt = DateTime.UtcNow,
        });

        await db.SaveChangesAsync();
    }
}
