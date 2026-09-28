using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public string Type { get; private set; } = null!;
        public string Content { get; private set; } = null!;
        public DateTime OccurredAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string? Error { get; private set; }
        public int RetryCount { get; private set; }
        public DateTime? NextRetryAt { get; private set; }
        public DateTime? FailedAt { get; private set; }

        private OutboxMessage()
        {
        }

        public OutboxMessage(string type, string content, DateTime occurredAt)
        {
            Id = Guid.NewGuid();
            Type = type;
            Content = content;
            OccurredAt = occurredAt;
            RetryCount = 0;
        }

        public void MarkAsProcessed()
        {
            ProcessedAt = DateTime.UtcNow;
            Error = null;
            NextRetryAt = null;
        }

        public void MarkAsFailed(string error)
        {
            const int maxRetryCount = 5;

            RetryCount++;
            Error = error;

            if (RetryCount >= maxRetryCount)
            {
                FailedAt = DateTime.UtcNow;
                NextRetryAt = null;
                return;
            }

            var delay = Math.Min(Math.Pow(2, RetryCount), 300);
            NextRetryAt = DateTime.UtcNow.AddSeconds(delay);
        }
    }
}