using System.Security.Claims;
using AuthPoC.Core.Entities;
using AuthPoC.Core.Interfaces;
using AuthPoC.WebApi.Dtos.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthPoC.WebApi.Controllers;

[ApiController]
[Route("api/articles/{articleId:int}/comments")]
public class CommentsController(ICommentRepository comments) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetByArticle(int articleId)
    {
        var result = await comments.GetCommentsByArticleIdAsync(articleId);
        var dtos = result.Select(c => new CommentResponse(
            c.CommentId, c.Content, c.User.Username, c.ArticleId, c.CreatedAt));
        return Ok(dtos);
    }

    [Authorize(Policy = "SubscriberPolicy")]
    [HttpPost]
    public async Task<IActionResult> Create(int articleId, [FromBody] CreateCommentRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var comment = new Comment
        {
            Content = request.Content,
            ArticleId = articleId,
            UserId = int.Parse(userId!),
        };

        await comments.AddCommentAsync(comment);
        return Created($"/api/articles/{articleId}/comments/{comment.CommentId}",
            new CommentResponse(comment.CommentId, comment.Content, "self", articleId, comment.CreatedAt));
    }

    [Authorize(Policy = "SubscriberPolicy")]
    [HttpPut("{commentId:int}")]
    public async Task<IActionResult> Update(int articleId, int commentId, [FromBody] UpdateCommentRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole  = User.FindFirstValue(ClaimTypes.Role);
        
        var comment = await comments.GetCommentByIdAsync(commentId);
        if (comment is null || comment.ArticleId != articleId) return NotFound();

        if(userRole == "Writer") return Forbid();
        if(userRole != "Editor" && userId != comment.UserId) return Forbid();
        
        comment.Content = request.Content;
        await comments.EditCommentAsync(comment);
        
        return NoContent();
    }

    [Authorize(Policy = "SubscriberPolicy")]
    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> Delete(int articleId, int commentId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole  = User.FindFirstValue(ClaimTypes.Role);
        
        var comment = await comments.GetCommentByIdAsync(commentId);
        if (comment is null || comment.ArticleId != articleId) return NotFound();
        
        var authorId = comment.Article.AuthorId;
        
        if(userId != comment.UserId && authorId != userId && userRole != "Editor") return Forbid();

        await comments.DeleteCommentAsync(commentId);
        return NoContent();
    }
}
