using Microsoft.Extensions.Diagnostics.HealthChecks;
using PersonalFinance.Identity.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly IdentityDbContext _context;

        public DatabaseHealthCheck(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);

                if (!canConnect)
                {
                    return HealthCheckResult.Unhealthy("Não foi possível conectar ao banco de dados.");
                }

                return HealthCheckResult.Healthy("Banco de dados disponível.");
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Unhealthy("Falha ao verificar o banco de dados.", exception);
            }
        }
    }
}
