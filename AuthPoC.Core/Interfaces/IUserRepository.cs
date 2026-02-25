using AuthPoC.Core.Entities;

namespace AuthPoC.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
}