using AuthPoC.Core.Entities;

namespace AuthPoC.Core.Interfaces;

public interface ICommentRepository
{
    Task<Comment?> GetCommentByIdAsync(int commentId);
    Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId);
    Task AddCommentAsync(Comment comment);
    Task EditCommentAsync(Comment comment);
    Task DeleteCommentAsync(int commentId);
}