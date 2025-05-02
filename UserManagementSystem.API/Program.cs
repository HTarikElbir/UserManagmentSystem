using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.MappingProfiles;
using UserManagementSystem.Business.Services;
using UserManagementSystem.Business.Validators;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Interfaces;
using UserManagementSystem.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add controllers and FluentValidation
builder.Services.AddControllers();

// Add DbContext with SQLite configuration
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlite(connectionString, b => b.MigrationsAssembly("UserManagementSystem.API"))); //SQLite Configuration

// Add FluentValidation for DTOs
builder.Services.AddScoped<IValidator<UserAddDto>, UserAddDtoValidator>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Add MappingProfile 
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.AddAutoMapper(typeof(RoleProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
