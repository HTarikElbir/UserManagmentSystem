using FluentValidation;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.Validators;

public class UserAddDtoValidator : AbstractValidator<UserAddDto>
{
    public UserAddDtoValidator()
    {
        
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName cannot be empty")
            .Length(3, 50).WithMessage("UserName must be between 3 and 50 characters")
            .Matches("^[a-zA-Z]+$").WithMessage("User name must contain only alphanumeric characters");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches(@"[A-Za-z]").WithMessage("Password must contain at least one letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit")
            .Matches(@"[?\W_]").WithMessage("Password must contain at least one special character");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email address");
        
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone cannot be empty")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");
        
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department cannot be empty");
        
    }
}