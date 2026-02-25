using AuthPoC.Core.Entities;
using AuthPoC.Core.Interfaces;
using AuthPoC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthPoC.Infrastructure.Repositories;

public class CommentRepository(AppDbContext db) : ICommentRepository
{
    public Task<Comment?> GetCommentByIdAsync(int commentId)
        => db.Comments
             .Include(c => c.User)
             .Include(c => c.Article)
             .FirstOrDefaultAsync(c => c.CommentId == commentId);

    public async Task<IEnumerable<Comment>> GetCommentsByArticleIdAsync(int articleId)
        => await db.Comments
                   .Include(c => c.User)
                   .Where(c => c.ArticleId == articleId)
                   .OrderBy(c => c.CreatedAt)
                   .ToListAsync();

    public async Task AddCommentAsync(Comment comment)
    {
        db.Comments.Add(comment);
        await db.SaveChangesAsync();
    }

    public async Task EditCommentAsync(Comment comment)
    {
        db.Comments.Update(comment);
        await db.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        var comment = await db.Comments.FindAsync(commentId);
        if (comment is not null)
        {
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
        }
    }
}
