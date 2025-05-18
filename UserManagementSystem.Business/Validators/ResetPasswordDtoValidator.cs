using FluentValidation;
using UserManagementSystem.Business.Dtos.Auth;

namespace UserManagementSystem.Business.Validators;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
          
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches(@"[A-Za-z]").WithMessage("Password must contain at least one letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit")
            .Matches(@"[?\W_]").WithMessage("Password must contain at least one special character");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email address");
    }
}