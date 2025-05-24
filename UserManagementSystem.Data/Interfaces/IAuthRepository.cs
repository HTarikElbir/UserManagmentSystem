using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Data.Interfaces;

public interface IAuthRepository
{
    Task<bool> ResetPasswordAsync(int userId, string password);
}