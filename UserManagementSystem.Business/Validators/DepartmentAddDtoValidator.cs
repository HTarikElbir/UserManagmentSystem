using FluentValidation;
using UserManagementSystem.Business.Dtos.Department;

namespace UserManagementSystem.Business.Validators;

public class DepartmentAddDtoValidator : AbstractValidator<DepartmentAddDto>
{
    public DepartmentAddDtoValidator()
    {
        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Department name cannot be empty")
            .Length(2, 50).WithMessage("Department name must be between 2 and 50 characters")
            .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Department name must contain only letters, numbers and spaces");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty")
            .MaximumLength(100).WithMessage("Description cannot exceed 100 characters");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive status must be specified");
    }
}