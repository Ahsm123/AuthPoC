using AuthPoC.Core.Entities;
using AuthPoC.Core.Interfaces;
using AuthPoC.WebApi.Dtos.Auth;
using AuthPoC.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthPoC.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserRepository users, JwtTokenService jwt) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await users.GetUserByUsernameAsync(request.Username);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials." });

        var token = jwt.GenerateToken(user);
        return Ok(new LoginResponse(token, user.Username, user.Role));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existing = await users.GetUserByUsernameAsync(request.Username);
        if (existing is not null)
            return Conflict(new { message = "Username already taken." });

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role,
        };

        await users.AddUserAsync(user);
        return Created($"/api/auth/login", new { message = "User created." });
    }
}
