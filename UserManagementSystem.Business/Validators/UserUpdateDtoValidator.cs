using FluentValidation;
using UserManagementSystem.Business.Dtos.User;

namespace UserManagementSystem.Business.Validators;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("User name is required.")
            .Length(2, 50)
            .WithMessage("User name must be between 2 and 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");
        
        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage("Invalid phone number format.");
    }
    
}