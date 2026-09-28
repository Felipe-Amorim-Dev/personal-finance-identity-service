using PersonalFinance.Identity.Domain.Entities;
using PersonalFinance.Identity.Domain.Interfaces;
using PersonalFinance.Identity.Infrastructure.Configurations;
using PersonalFinance.Identity.Infrastructure.Outbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDbContext _context;

        public UnitOfWork(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            var entities = _context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents.Count > 0)
                .Select(x => x.Entity)
                .ToList();

            AddOutboxMessages(entities);

            var result = await _context.SaveChangesAsync(cancellationToken);

            foreach (var entity in entities)
            {
                entity.ClearDomainEvents();
            }

            return result;
        }

        private void AddOutboxMessages(List<Entity> entities)
        {
            var domainEvents = entities.SelectMany(x => x.DomainEvents).ToList();

            if (domainEvents.Count == 0)
            {
                return;
            }

            var outboxMessages = domainEvents.Select(domainEvent => new OutboxMessage(
                domainEvent.EventType,
                JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                domainEvent.OccurredAt
            )).ToList();

            _context.OutboxMessages.AddRange(outboxMessages);
        }
    }
}
