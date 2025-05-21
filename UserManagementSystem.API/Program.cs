using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserManagementSystem.API.Extensions;
using UserManagementSystem.Business;
using UserManagementSystem.Business.Settings;
using UserManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User Management System API",
        Version = "v1",
        Description = "User Management System API Documentation"
    });

    // JWT Authentication için security tanımı
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Add controllers and FluentValidation
builder.Services.AddControllers();

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure JWT settings
builder.Services.AddOptions<JwtSettings>()
    .BindConfiguration(nameof(JwtSettings));

// Authentication 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var secretKey = builder.Configuration["JwtSettings:SecretKey"];
    var issuer = builder.Configuration["JwtSettings:Issuer"];
    var audience = builder.Configuration["JwtSettings:Audience"];

    if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
    {
        throw new InvalidOperationException("JWT settings are not properly configured in appsettings.Development.json");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        },
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var response = new
            {
                Message = "Unauthorized: You are not authorized to access this resource.",
                StatusCode = 401
            };
            await context.Response.WriteAsJsonAsync(response);
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            var response = new
            {
                Message = "Forbidden: You don't have permission to access this resource.",
                StatusCode = 403
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//builder.Services.AddAuthorization();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "UserManagementSystem";
});

// Add layer services
builder.Services.AddDataServices(connectionString!);
builder.Services.AddBusinessServices();

var app = builder.Build();

app.UseGlobalExceptionMiddleware();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Add middlewares
app.UseHttpsRedirection();

app.UseAuthentication();          
app.UseAuthorization();  

app.MapControllers();
app.Run();
