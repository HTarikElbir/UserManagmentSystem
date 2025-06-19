using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UserManagementSystem.Business.Dtos.Auth;
using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Business.Dtos.Role;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Business.MappingProfiles;
using UserManagementSystem.Business.Services;
using UserManagementSystem.Business.Services.Validation;
using UserManagementSystem.Business.Validators;

namespace UserManagementSystem.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // Add FluentValidation for DTOs
        services.AddScoped<IValidator<UserAddDto>, UserAddDtoValidator>();
        services.AddScoped<IValidator<UserUpdateDto>, UserUpdateDtoValidator>();
        services.AddScoped<IValidator<RoleAddDto>, RoleAddDtoValidator>();
        services.AddScoped<IValidator<RoleUpdateDto>, RoleUpdateDtoValidator>();
        services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
        services.AddScoped<IValidator<DepartmentAddDto>, DepartmentAddDtoValidator>();
        services.AddScoped<IValidator<DepartmentUpdateDto>, DepartmentUpdateDtoValidator>();
        
        // Add Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITokenCacheService, TokenCacheService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IReportService, ReportService>();

        // Add Validation Services
        services.AddScoped<IUserValidationService, UserValidationService>();
        services.AddScoped<IRoleValidationService, RoleValidationService>();
        services.AddScoped<IAuthValidationService, AuthValidationService>();
        services.AddScoped<IDepartmentValidationService, DepartmentValidationService>();

        // Add Password Hasher Service
        services.AddScoped<IPasswordHasher, PasswordHasherService>();

        // Add AutoMapper Profiles
        services.AddAutoMapper(typeof(UserProfile).Assembly);
        services.AddAutoMapper(typeof(RoleProfile).Assembly);
        services.AddAutoMapper(typeof(UserRoleProfile).Assembly);
        services.AddAutoMapper(typeof(DepartmentProfile).Assembly);
        
        return services;
    }
}