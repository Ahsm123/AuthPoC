namespace AuthPoC.Core.Entities;

public class Article
{
    public int ArticleId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public int AuthorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Author { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = [];
}