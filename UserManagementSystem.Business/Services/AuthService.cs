using Microsoft.Extensions.Logging;
using UserManagementSystem.Business.Dtos.Auth;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class AuthService: IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IAuthValidationService _authValidationService;
    private readonly ITokenCacheService _tokenCacheService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthRepository authRepository, 
        ITokenService tokenService, 
        IAuthValidationService authValidationService,
        ITokenCacheService tokenCacheService,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        ILogger<AuthService> logger)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
        _authValidationService = authValidationService;
        _tokenCacheService = tokenCacheService;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _logger = logger;
    }

    // This method handles user login by validating the provided credentials and generating a token.
    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        try
        {
            _logger.LogInformation("Login attempt: {Username}", loginDto.UserName);
            
            await _authValidationService.ValidateCredentialsAsync(loginDto.UserName, loginDto.Password);
        
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.UserName);
        
            var token = _tokenService.CreateToken(user!);
        
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Token generation failed for user: {Username}", loginDto.UserName);
                throw new Exception("Token generation failed");
            }
            //await _tokenCacheService.RemoveTokenAsync($"user:{user!.UserId}:login_token");
            
            await _tokenCacheService.SetTokenAsync($"user:{user!.UserId}:login_token", token, TimeSpan.FromMinutes(60));
            
            _logger.LogInformation("Login successful: {Username}, UserId: {UserId}", loginDto.UserName, user.UserId);
            
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed: {Username}", loginDto.UserName);
            throw;
        }
    }
    
    public async Task<bool> LogoutAsync(LogoutDto logoutDto)
    {
        try
        {
            _logger.LogInformation("Logout attempt: {Email}", logoutDto.Email);
            
            await _authValidationService.ValidateExistingEmailAsync(logoutDto.Email);
        
            var user = await _userRepository.GetUserByEmailAsync(logoutDto.Email);

            if (!await _tokenService.ValidateToken(logoutDto.Token, user!.UserId))
            {
                _logger.LogWarning("Invalid token for logout: {Email}", logoutDto.Email);
                throw new Exception("Token is invalid or expired.");
            }
        
            await _tokenCacheService.AddToBlackListAsync(logoutDto.Token);
        
            await _tokenCacheService.RemoveTokenAsync(($"user:{user!.UserId}:login_token"));
            
            _logger.LogInformation("Logout successful: {Email}, UserId: {UserId}", logoutDto.Email, user.UserId);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Logout failed: {Email}", logoutDto.Email);
            throw;
        }
    }

    // This method handles user password reset by generating a token 
    public async Task<string> RequestResetPasswordAsync(RequestResetPasswordDto resetPasswordDto)
    {
        try
        {
            _logger.LogInformation("Password reset request: {Email}", resetPasswordDto.Email);
            
            await _authValidationService.ValidateExistingEmailAsync(resetPasswordDto.Email);
        
            var user = await _userRepository.GetUserByEmailAsync(resetPasswordDto.Email);
        
            var token = _tokenService.CreateResetPasswordToken(resetPasswordDto.Email);
        
            await _tokenCacheService.SetTokenAsync($"user:{user!.UserId}:reset_token", token, TimeSpan.FromMinutes(15));
            
            _logger.LogInformation("Password reset token generated: {Email}, UserId: {UserId}", resetPasswordDto.Email, user.UserId);
            
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Password reset request failed: {Email}", resetPasswordDto.Email);
            throw;
        }
    }
    
    // This method handles user password reset by validating the provided token and updating the password.  -
    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            _logger.LogInformation("Password reset attempt: {Email}", resetPasswordDto.Email);
            
            await _authValidationService.ValidateResetPasswordDtoAsync(resetPasswordDto);
        
            var user = await _userRepository.GetUserByEmailAsync(resetPasswordDto.Email);
        
            var result = await _tokenService.ValidateToken(resetPasswordDto.Token, user!.UserId);
            if (!result)
            {
                _logger.LogWarning("Invalid reset token: {Email}", resetPasswordDto.Email);
                throw new Exception("Token is invalid or expired.");
            }
         
            var hashedPassword = _passwordHasher.HashPassword(resetPasswordDto.NewPassword);
        
            await _authRepository.ResetPasswordAsync(user.UserId, hashedPassword);
        
            await _tokenCacheService.RemoveTokenAsync($"user:{user.UserId}:reset_token");
        
            _logger.LogInformation("Password reset successful: {Email}, UserId: {UserId}", resetPasswordDto.Email, user.UserId);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Password reset failed: {Email}", resetPasswordDto.Email);
            throw;
        }
    }
}