using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Interfaces
{
    public interface IDomainEvent
    {
        string EventType { get; }
        DateTime OccurredAt { get; }
    }
}
