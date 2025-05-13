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

    public AuthService(IAuthRepository authRepository, 
        ITokenService tokenService, 
        IAuthValidationService authValidationService)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
        _authValidationService = authValidationService;
    }

    // This method handles user login by validating the provided credentials and generating a token.
    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _authRepository.GetUserByUsernameAsync(loginDto.UserName);
        
        await _authValidationService.ValidateExistingUserAsync(loginDto.UserName);
        
        _authValidationService.ValidatePassword(loginDto.Password, user!.Password);

        var token = _tokenService.CreateToken(user);
        
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Token generation failed");
        }
        
        return token;
    }
}