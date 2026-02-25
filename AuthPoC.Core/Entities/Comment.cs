namespace AuthPoC.Core.Entities;

public class Comment
{
    public int CommentId { get; set; }
    public required string Content { get; set; }
    public int ArticleId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Article Article { get; set; } = null!;
}