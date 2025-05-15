using UserManagementSystem.Business.Dtos.Auth;

namespace UserManagementSystem.Business.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
    Task<string> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}