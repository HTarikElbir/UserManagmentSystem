using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.Auth;
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
        
        // Add Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITokenCacheService, TokenCacheService>();

        // Add Validation Services
        services.AddScoped<IUserValidationService, UserValidationService>();
        services.AddScoped<IRoleValidationService, RoleValidationService>();
        services.AddScoped<IAuthValidationService, AuthValidationService>();

        // Add Password Hasher Service
        services.AddScoped<IPasswordHasher, PasswordHasherService>();

        // Add AutoMapper Profiles
        services.AddAutoMapper(typeof(UserProfile).Assembly);
        services.AddAutoMapper(typeof(RoleProfile).Assembly);
        
        return services;
    }
}