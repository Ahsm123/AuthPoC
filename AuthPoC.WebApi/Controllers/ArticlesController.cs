using System.Security.Claims;
using AuthPoC.Core.Entities;
using AuthPoC.Core.Interfaces;
using AuthPoC.WebApi.Dtos.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthPoC.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController(IArticleRepository articles) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await articles.GetArticlesAsync();
        var dtos = result.Select(a => new ArticleResponse(
            a.ArticleId, a.Title, a.Content, a.Author.Username, a.CreatedAt));
        return Ok(dtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var article = await articles.GetArticleByIdAsync(id);
        if (article is null) return NotFound();

        return Ok(new ArticleResponse(
            article.ArticleId, article.Title, article.Content,
            article.Author.Username, article.CreatedAt));
    }

    [Authorize(Policy = "WriterPolicy")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleRequest request)
    {
        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var article = new Article
        {
            Title = request.Title,
            Content = request.Content,
            AuthorId = int.Parse(authorId!),
        };

        await articles.AddArticleAsync(article);
        return CreatedAtAction(nameof(GetById), new { id = article.ArticleId },
            new ArticleResponse(article.ArticleId, article.Title, article.Content, "self", article.CreatedAt));
    }

    [Authorize(Policy = "EditorPolicy")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArticleRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        var article = await articles.GetArticleByIdAsync(id);
        if (article is null) return NotFound();

        if (userRole != "Editor" && userId != article.AuthorId) return Forbid(); 

        article.Title = request.Title;
        article.Content = request.Content;

        await articles.EditArticleAsync(article);
        return NoContent();
    }

    [Authorize(Policy = "EditorPolicy")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        
        var article = await articles.GetArticleByIdAsync(id);
        if (article is null) return NotFound();
        
        if (userRole != "Editor" && userId != article.AuthorId) return Forbid();

        await articles.DeleteArticleAsync(id);
        return NoContent();
    }
}
