using AuthPoC.Core.Entities;
using AuthPoC.Core.Interfaces;
using AuthPoC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthPoC.Infrastructure.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public Task<User?> GetUserByIdAsync(int userId)
        => db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

    public Task<User?> GetUserByUsernameAsync(string username)
        => db.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task AddUserAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }
}
