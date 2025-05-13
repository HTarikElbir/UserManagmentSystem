using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}