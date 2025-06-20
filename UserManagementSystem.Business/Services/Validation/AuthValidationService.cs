using FluentValidation;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<AuthValidationService> _logger;
    
    public AuthValidationService(IUserRepository userRepository, IPasswordHasher passwordHasher, IValidator<ResetPasswordDto> resetPasswordValidator, ILogger<AuthValidationService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _resetPasswordValidator = resetPasswordValidator;
        _logger = logger;
    }


    public async  Task ValidateExistingUserAsync(string username)
    {
        try
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            
            if (user == null)
            {
                _logger.LogWarning("Invalid username validation failed: {Username}", username);
                throw new Exception("Invalid username");
            }
        }
        catch (Exception ex) when (ex.Message != "Invalid username")
        {
            _logger.LogError(ex, "Failed to validate existing user: {Username}", username);
            throw;
        }
    }

    public async Task ValidateExistingEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            
            if (user == null)
            {
                _logger.LogWarning("Invalid email validation failed: {Email}", email);
                throw new Exception("User with this email does not exist.");
            }
        }
        catch (Exception ex) when (ex.Message != "User with this email does not exist.")
        {
            _logger.LogError(ex, "Failed to validate existing email: {Email}", email);
            throw;
        }
    }

    public async Task ValidateResetPasswordDtoAsync(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var result = await _resetPasswordValidator.ValidateAsync(resetPasswordDto);
            if (!result.IsValid)
            {
                _logger.LogWarning("Reset password validation failed - Email: {Email}, Errors: {Errors}", 
                    resetPasswordDto.Email, string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(result.Errors);
            }
            
            if (resetPasswordDto.Token == null)
            {
                _logger.LogWarning("Invalid reset password token - Email: {Email}", resetPasswordDto.Email);
                throw new Exception("Invalid token");
            }
            
            await ValidateExistingEmailAsync(resetPasswordDto.Email);
        }
        catch (Exception ex) when (ex is not ValidationException && ex.Message != "Invalid token")
        {
            _logger.LogError(ex, "Failed to validate reset password DTO - Email: {Email}", resetPasswordDto.Email);
            throw;
        }
    }

    public async Task ValidateCredentialsAsync(string username, string password)
    {
        try
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null || !_passwordHasher.VerifyPassword(password, user.Password))
            {
                _logger.LogWarning("Invalid login attempt - Username: {Username}", username);
                throw new Exception("Invalid username or password");
            }
        }
        catch (Exception ex) when (ex.Message != "Invalid username or password")
        {
            _logger.LogError(ex, "Failed to validate credentials for user: {Username}", username);
            throw;
        }
    }
}