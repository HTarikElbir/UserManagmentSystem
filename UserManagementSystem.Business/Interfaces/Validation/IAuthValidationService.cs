namespace UserManagementSystem.Business.Interfaces.Validation;

public interface IAuthValidationService
{
    Task ValidateCredentialsAsync(string username, string password);
    Task ValidateExistingUserAsync(string username);
}