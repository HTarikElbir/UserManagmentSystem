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
    
    // Validates if a user exists by their ID.
    public async Task ValidateUserExistAsync(int userId)
    {
        var user = await _userRepository.GetByIdUserAsync(userId);
        
        if (user == null)
        {
            throw new Exception($"User with ID {userId} does not exist.");
        }
    }
    
    // Validates if a user exists by their Role.
    public async Task ValidateUserExistByRoleAsync(string roleName, int page = 1, int pageSize = 10)
    {
        var users = await _userRepository.GetUsersByRoleAsync(roleName, page, pageSize);
        
        if (users == null || users.Count == 0)
        {
            throw new Exception("No users found for the specified role.");
        }
    }
    
    // Validates if a user exists by their Department.
    public async Task ValidateUserExistByDepartmentAsync(string departmentName, int page = 1, int pageSize = 10)
    {
        var users = await _userRepository.GetUsersByDepartmentAsync(departmentName, page, pageSize);
        
        if (users == null || users.Count == 0)
        {
            throw new Exception("No users found for the specified department.");
        }
    }
    
    // Validates for updating a user.
    public async Task ValidateUpdateRequestAsync(int userId, UserUpdateDto userUpdateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(userUpdateDto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        await ValidateUserExistAsync(userId);
    }

    // Validates for adding a new user.
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