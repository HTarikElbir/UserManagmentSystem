using UserManagementSystem.Business.Dtos.Auth;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class AuthService: IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IAuthRepository authRepository, ITokenService tokenService)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _authRepository.GetUserByUsernameAsync(loginDto.UserName);
        
        if (user == null || user.Password != loginDto.Password)
        {
            throw new Exception("Invalid username or password");
        }

        var token = _tokenService.CreateToken(user);
        
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Token generation failed");
        }
        
        return token;
    }
}