using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ApiBD.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

// Aceptar conexiones desde 0.0.0.0:5041
builder.WebHost.UseUrls("http://0.0.0.0:5041");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// EF Core MySQL DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString))
    // Usar convención snake_case for columnas y tablas
    .UseSnakeCaseNamingConvention());
// Generic repositories
builder.Services.AddScoped(typeof(ApiBD.Core.Interfaces.IGenericRepository<>),
    typeof(ApiBD.Infrastructure.Repositories.GenericRepository<>));
// AutoMapper
builder.Services.AddAutoMapper(typeof(ApiBD.Application.Profiles.MappingProfile).Assembly);
// Controllers
builder.Services.AddControllers();

// Servicios CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware CORS
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
