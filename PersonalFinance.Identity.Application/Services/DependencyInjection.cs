using Microsoft.Extensions.DependencyInjection;
using PersonalFinance.Identity.Application.DTOs;
using PersonalFinance.Identity.Application.Interfaces;
using PersonalFinance.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, string jwtSecretKey, string jwtIssuer, string jwtAudience, int jwtExpirationMinutes, EmailSettings emailSettings)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenValidationService, TokenValidationService>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IRefreshTokenService, RefreshTokenService>();
            services.AddSingleton<IEmailConfirmationService, EmailConfirmationService>();
            services.AddSingleton<IPasswordResetService, PasswordResetService>();
            services.AddSingleton<ITokenService>(new TokenService(jwtSecretKey, jwtIssuer, jwtAudience, jwtExpirationMinutes));
            services.AddSingleton(emailSettings);
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAdminBootstrapService, AdminBootstrapService>();

            return services;
        }
    }
}
