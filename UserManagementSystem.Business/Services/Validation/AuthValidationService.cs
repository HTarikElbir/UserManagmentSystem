using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class AuthValidationService : IAuthValidationService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public AuthValidationService(IAuthRepository authRepository, IPasswordHasher passwordHasher)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
    }


    public async  Task ValidateExistingUserAsync(string username)
    {
        var user = await _authRepository.GetUserByUsernameAsync(username);
        
        if (user == null)
            throw new Exception("Invalid username");
    }

    public void ValidatePassword(string password, string hashedPassword)
    {
        if(!_passwordHasher.VerifyPassword(password, hashedPassword))
            throw new Exception("Invalid password");
    }
}