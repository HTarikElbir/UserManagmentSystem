using QuestPDF.Infrastructure;
using Serilog;
using UserManagementSystem.API.Extensions;
using UserManagementSystem.API.Middlewares;
using UserManagementSystem.Business;
using UserManagementSystem.Business.Settings;
using UserManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

//var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfiguration();
builder.Services.AddRedisCache(builder.Configuration);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.SQLite(
        sqliteDbPath: "UserManagement.db",
        tableName: "Logs",
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information));

QuestPDF.Settings.License = LicenseType.Community;

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add layer services
builder.Services.AddDataServices(connectionString!);
builder.Services.AddBusinessServices();

var app = builder.Build();

app.UseGlobalExceptionMiddleware();
app.UseMiddleware<LoggingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();          
app.UseAuthorization();  

app.MapControllers();
app.Run();
