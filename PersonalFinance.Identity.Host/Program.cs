using PersonalFinance.Identity.Api;
using PersonalFinance.Identity.Application.Services;
using PersonalFinance.Identity.Infrastructure.Configurations;
using PersonalFinance.Identity.Host.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PersonalFinance.Identity.Application.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Threading.RateLimiting;
using PersonalFinance.Identity.Api.Services;
using PersonalFinance.Identity.Application.Interfaces;
using System.Text;
using PersonalFinance.Identity.Application.Interfaces;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMq");
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtExpirationMinutes = builder.Configuration.GetValue<int>("Jwt:ExpirationMinutes");

var emailSettings = new EmailSettings
{
    Host = builder.Configuration["Email:Host"] ?? string.Empty,
    Port = builder.Configuration.GetValue<int>("Email:Port"),
    User = builder.Configuration["Email:User"] ?? string.Empty,
    Password = builder.Configuration["Email:Password"] ?? string.Empty,
    FromName = builder.Configuration["Email:FromName"] ?? string.Empty,
    FromEmail = builder.Configuration["Email:FromEmail"] ?? string.Empty,
    ResetPasswordUrl = builder.Configuration["Email:ResetPasswordUrl"] ?? string.Empty,
    ConfirmEmailUrl = builder.Configuration["Email:ConfirmEmailUrl"] ?? string.Empty
};

if (string.IsNullOrWhiteSpace(emailSettings.Host) || string.IsNullOrWhiteSpace(emailSettings.User) || string.IsNullOrWhiteSpace(emailSettings.Password))
{
    throw new InvalidOperationException("As configurações de e-mail não foram configuradas.");
}

if (string.IsNullOrWhiteSpace(jwtSecretKey))
{
    throw new InvalidOperationException("A configuração 'Jwt:SecretKey' não foi configurada.");
}

if (string.IsNullOrWhiteSpace(jwtIssuer))
{
    throw new InvalidOperationException("A configuração 'Jwt:Issuer' não foi configurada.");
}

if (string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException("A configuração 'Jwt:Audience' não foi configurada.");
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("A connection string 'DefaultConnection' não foi configurada.");
}

if (string.IsNullOrWhiteSpace(rabbitMqConnectionString))
{
    throw new InvalidOperationException("A connection string 'RabbitMq' não foi configurada.");
}

builder.Services.AddApplication(jwtSecretKey, jwtIssuer, jwtAudience, jwtExpirationMinutes, emailSettings);
builder.Services.AddInfrastructure(connectionString, rabbitMqConnectionString);

builder.Services.AddControllers().AddApplicationPart(typeof(ApiAssemblyReference).Assembly);

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var userIdClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tokenVersionClaim = context.Principal?.FindFirst("token_version")?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId) || !int.TryParse(tokenVersionClaim, out var tokenVersion))
            {
                context.Fail("Token inválido.");
                return;
            }

            var tokenValidationService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidationService>();
            var isValid = await tokenValidationService.ValidateAsync(userId, tokenVersion, context.HttpContext.RequestAborted);

            if (!isValid)
            {
                context.Fail("Token inválido ou revogado.");
            }
        }
    };
});

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }));

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

var bootstrapAdminEmail = builder.Configuration["BootstrapAdmin:Email"];

if (!string.IsNullOrWhiteSpace(bootstrapAdminEmail))
{
    using var scope = app.Services.CreateScope();

    var adminBootstrapService = scope.ServiceProvider.GetRequiredService<IAdminBootstrapService>();

    await adminBootstrapService.EnsureAdminAsync(bootstrapAdminEmail);
}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();