using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Data.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
}