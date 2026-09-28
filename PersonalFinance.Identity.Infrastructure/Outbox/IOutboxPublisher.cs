using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.Outbox
{
    public interface IOutboxPublisher
    {
        Task PublishAsync(string type, string content, CancellationToken cancellationToken = default);
    }
}