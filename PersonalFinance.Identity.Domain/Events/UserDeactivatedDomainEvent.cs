using PersonalFinance.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Events
{
    public class UserDeactivatedDomainEvent : IDomainEvent
    {
        public string EventType => "identity.user.deactivated.v1";
        public Guid UserId { get; }
        public string Email { get; }
        public DateTime OccurredAt { get; }

        public UserDeactivatedDomainEvent(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
            OccurredAt = DateTime.UtcNow;
        }
    }
}
