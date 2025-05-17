using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
    
    string CreateResetPasswordToken(string email);
    
    Task<bool> ValidateToken(string token, int userId);
}