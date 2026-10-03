using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinance.Identity.Domain.Interfaces;
using PersonalFinance.Identity.Infrastructure.HealthChecks;
using PersonalFinance.Identity.Infrastructure.Outbox;
using PersonalFinance.Identity.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, string rabbitMqConnectionString)
        {
            services.AddDbContext<IdentityDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            services.AddScoped<IEmailConfirmationTokenRepository, EmailConfirmationTokenRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IOutboxPublisher>(new RabbitMqOutboxPublisher(rabbitMqConnectionString));
            services.AddHostedService<OutboxProcessor>();            

            return services;
        }
    }
}
