using AuthPoC.Core.Entities;
using AuthPoC.Core.Interfaces;
using AuthPoC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthPoC.Infrastructure.Repositories;

public class ArticleRepository(AppDbContext db) : IArticleRepository
{
    public async Task<IEnumerable<Article>> GetArticlesAsync()
        => await db.Articles
                   .Include(a => a.Author)
                   .OrderByDescending(a => a.CreatedAt)
                   .ToListAsync();

    public Task<Article?> GetArticleByIdAsync(int id)
        => db.Articles
             .Include(a => a.Author)
             .Include(a => a.Comments).ThenInclude(c => c.User)
             .FirstOrDefaultAsync(a => a.ArticleId == id);

    public async Task AddArticleAsync(Article article)
    {
        db.Articles.Add(article);
        await db.SaveChangesAsync();
    }

    public async Task EditArticleAsync(Article article)
    {
        db.Articles.Update(article);
        await db.SaveChangesAsync();
    }

    public async Task DeleteArticleAsync(int id)
    {
        var article = await db.Articles.FindAsync(id);
        if (article is not null)
        {
            db.Articles.Remove(article);
            await db.SaveChangesAsync();
        }
    }
}
