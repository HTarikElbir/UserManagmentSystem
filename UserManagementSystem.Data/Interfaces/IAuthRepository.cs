using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Data.Interfaces;

public interface IAuthRepository
{
    Task<User?> Authenticate(string username, string password);
}