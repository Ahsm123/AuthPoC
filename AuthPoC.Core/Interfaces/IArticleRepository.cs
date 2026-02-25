using AuthPoC.Core.Entities;

namespace AuthPoC.Core.Interfaces;

public interface IArticleRepository
{
    Task<IEnumerable<Article>> GetArticlesAsync();
    Task<Article?> GetArticleByIdAsync(int id);
    Task AddArticleAsync(Article article);
    Task EditArticleAsync(Article article);
    Task DeleteArticleAsync(int id);
}