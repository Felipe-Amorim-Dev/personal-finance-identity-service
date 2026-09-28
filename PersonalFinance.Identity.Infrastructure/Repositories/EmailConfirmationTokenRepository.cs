using Microsoft.EntityFrameworkCore;
using PersonalFinance.Identity.Domain.Entities;
using PersonalFinance.Identity.Domain.Interfaces;
using PersonalFinance.Identity.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.Repositories
{
    public class EmailConfirmationTokenRepository : IEmailConfirmationTokenRepository
    {
        private readonly IdentityDbContext _context;

        public EmailConfirmationTokenRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<EmailConfirmationToken?> ObterPorHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        {
            return await _context.EmailConfirmationTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
        }

        public async Task AdicionarAsync(EmailConfirmationToken emailConfirmationToken, CancellationToken cancellationToken = default)
        {
            await _context.EmailConfirmationTokens.AddAsync(emailConfirmationToken, cancellationToken);
        }

        public async Task RevogarTodosPorUsuarioAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _context.EmailConfirmationTokens
                .Where(x => x.UserId == userId && x.ConfirmedAt == null && x.RevokedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
            {
                token.Revoke();
            }
        }
    }
}
