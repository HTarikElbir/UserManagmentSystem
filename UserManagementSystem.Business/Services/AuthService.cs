using UserManagementSystem.Business.Dtos.Auth;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class AuthService: IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;
    private readonly IAuthValidationService _authValidationService;
    private readonly ITokenCacheService _tokenCacheService;

    public AuthService(IAuthRepository authRepository, 
        ITokenService tokenService, 
        IAuthValidationService authValidationService,
        ITokenCacheService tokenCacheService)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
        _authValidationService = authValidationService;
        _tokenCacheService = tokenCacheService;
    }

    // This method handles user login by validating the provided credentials and generating a token.
    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        await _authValidationService.ValidateCredentialsAsync(loginDto.UserName, loginDto.Password);
        
        var user = await _authRepository.GetUserByUsernameAsync(loginDto.UserName);

        await _authValidationService.ValidateExistingUserAsync(loginDto.UserName);
        
        var token = _tokenService.CreateToken(user!);
        
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Token generation failed");
        }
        
        await _tokenCacheService.SetTokenAsync($"user:{user!.UserId}:login_token", token, TimeSpan.FromMinutes(60));
        
        return token;
    }
    
    // This method handles user password reset by generating a token 
    public async Task<string> RequestResetPasswordAsync(RequestResetPasswordDto resetPasswordDto)
    {
        var user = await _authRepository.GetUserByEmailAsync(resetPasswordDto.Email);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        var token = _tokenService.CreateResetPasswordToken(resetPasswordDto.Email);
        
        await _tokenCacheService.SetTokenAsync($"user:{user!.UserId}:reset_token", token, TimeSpan.FromMinutes(15));

        return token;
    }
    
    // This method handles user password reset by validating the provided token and updating the password.  -
    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _authRepository.GetUserByEmailAsync(resetPasswordDto.Email);
        if (user == null)
            throw new Exception("User not found");
        
        if (string.IsNullOrEmpty(resetPasswordDto.Token))
            throw new Exception("Invalid token");
        
        var result = await _tokenService.ValidateToken(resetPasswordDto.Token, user.UserId);
        if (!result)
            throw new Exception("Something went wrong. Please try again.");
        
        // add token check logic with a cache system (Redis)
        await _authRepository.ResetPasswordAsync(user.UserId, resetPasswordDto.NewPassword);
        
        return true;
    }
}