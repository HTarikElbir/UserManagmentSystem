using FluentValidation;
using UserManagementSystem.Business.Dtos.Role;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class UserValidationService : IUserValidationService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<UserUpdateDto> _updateValidator;
    private readonly IValidator<UserAddDto> _addValidator;
    
    public UserValidationService(IUserRepository userRepository,
        IValidator<UserUpdateDto> updateValidator, 
        IValidator<UserAddDto> addValidator)
    {
        _userRepository = userRepository;
        _updateValidator = updateValidator;
        _addValidator = addValidator;
    }
    
    public async Task ValidateUserExistAsync(int userId)
    {
        var user = await _userRepository.GetByIdUserAsync(userId);
        
        if (user == null)
        {
            throw new Exception($"User with ID {userId} does not exist.");
        }
    }

    public async Task ValidateUpdateRequestAsync(int userId, UserUpdateDto userUpdateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(userUpdateDto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        await ValidateUserExistAsync(userId);
    }

    public async Task ValidateAddRequestAsync(UserAddDto userAddDto)
    {
        var validationResult = await _addValidator.ValidateAsync(userAddDto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var existingUser = await _userRepository.GetByEmailAsync(userAddDto.Email);
        if (existingUser != null)
        {
            throw new Exception($"User with email {userAddDto.Email} already exists.");
        }
    }
}