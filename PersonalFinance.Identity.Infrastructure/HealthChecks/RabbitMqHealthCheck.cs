using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.HealthChecks
{
    public class RabbitMqHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public RabbitMqHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    Uri = new Uri(_connectionString)
                };

                await using var connection = await factory.CreateConnectionAsync(cancellationToken);

                if (!connection.IsOpen)
                {
                    return HealthCheckResult.Unhealthy("Não foi possível estabelecer conexão com o RabbitMQ.");
                }

                return HealthCheckResult.Healthy("RabbitMQ disponível.");
            }
            catch (Exception exception)
            {
                return HealthCheckResult.Unhealthy("Falha ao verificar o RabbitMQ.", exception);
            }
        }
    }
}
