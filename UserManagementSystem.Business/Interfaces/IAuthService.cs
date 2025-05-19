using UserManagementSystem.Business.Dtos.Auth;

namespace UserManagementSystem.Business.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
    Task<bool> LogoutAsync(LogoutDto logoutDto);
    Task<string> RequestResetPasswordAsync(RequestResetPasswordDto resetPasswordDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}