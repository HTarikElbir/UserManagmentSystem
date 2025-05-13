namespace UserManagementSystem.Business.Interfaces.Validation;

public interface IAuthValidationService
{
    Task ValidateExistingUserAsync(string username);
    void ValidatePassword(string password, string hashedPassword);
}