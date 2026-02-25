namespace AuthPoC.WebApi.Dtos.Comments;

public record CommentResponse(
    int CommentId,
    string Content,
    string AuthorUsername,
    int ArticleId,
    DateTime CreatedAt
);
