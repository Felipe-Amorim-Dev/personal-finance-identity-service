using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Entities
{
    public class EmailConfirmationToken
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ConfirmedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        public bool IsActive => ConfirmedAt is null && RevokedAt is null && ExpiresAt > DateTime.UtcNow;

        private EmailConfirmationToken()
        {
        }

        public EmailConfirmationToken(Guid userId, string tokenHash, DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public void Confirm()
        {
            if (ConfirmedAt is not null)
            {
                return;
            }

            ConfirmedAt = DateTime.UtcNow;
        }

        public void Revoke()
        {
            if (RevokedAt is not null)
            {
                return;
            }

            RevokedAt = DateTime.UtcNow;
        }
    }
}
