using FluentValidation;
using UserManagementSystem.Business.Dtos.Auth;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class AuthValidationService : IAuthValidationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<ResetPasswordDto> _resetPasswordValidator;
    
    public AuthValidationService(IUserRepository userRepository, IPasswordHasher passwordHasher, IValidator<ResetPasswordDto> resetPasswordValidator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _resetPasswordValidator = resetPasswordValidator;
    }


    public async  Task ValidateExistingUserAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        
        if (user == null)
            throw new Exception("Invalid username");
    }

    public async Task ValidateExistingEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        
        if (user == null)
            throw new Exception("User with this email does not exist.");
    }

    public async Task ValidateResetPasswordDtoAsync(ResetPasswordDto resetPasswordDto)
    {
        var result = await _resetPasswordValidator.ValidateAsync(resetPasswordDto);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
        
        if( resetPasswordDto.Token == null )
            throw new Exception("Invalid token");
        
        await ValidateExistingEmailAsync(resetPasswordDto.Email);
    }

    public async Task ValidateCredentialsAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null || !_passwordHasher.VerifyPassword(password, user.Password))
        {
            throw new Exception("Invalid username or password");
        }
    }
}