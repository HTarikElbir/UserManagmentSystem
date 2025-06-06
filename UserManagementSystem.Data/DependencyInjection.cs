using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Interfaces;
using UserManagementSystem.Data.Repositories;

namespace UserManagementSystem.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
    {
        // Add DbContext with SQLite configuration
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlite(connectionString, b => b.MigrationsAssembly("UserManagementSystem.API"))); //SQLite Configuration

        // Add Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        return services;
    }
}