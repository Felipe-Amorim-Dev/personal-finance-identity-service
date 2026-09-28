using PersonalFinance.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Events
{
    public class UserCreatedDomainEvent : IDomainEvent
    {
        public string EventType => "identity.user.created.v1";
        public Guid UserId { get; }
        public string Nome { get; }
        public string Email { get; }
        public DateTime OccurredAt { get; }

        public UserCreatedDomainEvent(Guid userId, string nome, string email)
        {
            UserId = userId;
            Nome = nome;
            Email = email;
            OccurredAt = DateTime.UtcNow;
        }
    }
}
