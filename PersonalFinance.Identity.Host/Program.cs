using PersonalFinance.Identity.Api;
using PersonalFinance.Identity.Application.Services;
using PersonalFinance.Identity.Infrastructure.Configurations;
using PersonalFinance.Identity.Host.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("A connection string 'DefaultConnection' não foi configurada.");
}

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);

builder.Services.AddControllers().AddApplicationPart(typeof(ApiAssemblyReference).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();