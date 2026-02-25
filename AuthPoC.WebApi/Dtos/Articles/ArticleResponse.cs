namespace AuthPoC.WebApi.Dtos.Articles;

public record ArticleResponse(
    int ArticleId,
    string Title,
    string Content,
    string AuthorUsername,
    DateTime CreatedAt
);
