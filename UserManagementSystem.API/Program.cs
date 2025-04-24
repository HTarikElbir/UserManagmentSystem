using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Services;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Interfaces;
using UserManagementSystem.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add controllers
builder.Services.AddControllers();

// Add DbContext with SQLite configuration
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlite(connectionString, b => b.MigrationsAssembly("UserManagementSystem.API"))); //SQLite Configuration

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

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
