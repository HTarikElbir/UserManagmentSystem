using UserManagementSystem.Business.Dtos.Auth;

namespace UserManagementSystem.Business.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
    Task<string> RequestResetPasswordAsync(RequestResetPasswordDto resetPasswordDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}