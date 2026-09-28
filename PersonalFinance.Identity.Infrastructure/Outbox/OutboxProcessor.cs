using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PersonalFinance.Identity.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.Outbox
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxProcessor> _logger;

        public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessMessagesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task ProcessMessagesAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<IOutboxPublisher>();
            var now = DateTime.UtcNow;

            var messages = await context.OutboxMessages
                .Where(x => x.ProcessedAt == null && x.FailedAt == null && (x.NextRetryAt == null || x.NextRetryAt <= now))
                .OrderBy(x => x.OccurredAt)
                .Take(20)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                try
                {
                    await publisher.PublishAsync(message.Type, message.Content, cancellationToken);
                    message.MarkAsProcessed();
                }
                catch (Exception ex)
                {
                    message.MarkAsFailed(ex.Message);
                    _logger.LogError(ex, "Erro ao processar mensagem do Outbox {OutboxMessageId}.", message.Id);
                }
            }

            if (messages.Count > 0)
            {
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
