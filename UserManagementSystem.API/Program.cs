using UserManagementSystem.Business;
using UserManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add controllers and FluentValidation
builder.Services.AddControllers();

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add layer services
builder.Services.AddDataServices(connectionString);
builder.Services.AddBusinessServices();

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
