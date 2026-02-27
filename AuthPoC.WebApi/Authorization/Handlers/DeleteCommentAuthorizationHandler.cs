using System.Security.Claims;
using AuthPoC.Core.Entities;
using AuthPoC.WebApi.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace AuthPoC.WebApi.Authorization.Handlers;

public class DeleteCommentAuthorizationHandler : AuthorizationHandler<DeleteCommentRequirement, Comment>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        DeleteCommentRequirement requirement, 
        Comment resource)
    {
        var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = context.User.FindFirstValue(ClaimTypes.Role);

        if (role == "Editor")
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        
        if (role == "Subscriber" && userId == resource.UserId)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        
        if (role == "Writer" && userId == resource.Article.AuthorId)
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}