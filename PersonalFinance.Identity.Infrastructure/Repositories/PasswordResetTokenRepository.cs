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
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly IdentityDbContext _context;

        public PasswordResetTokenRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordResetToken?> ObterPorHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        {
            return await _context.PasswordResetTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
        }

        public async Task AdicionarAsync(PasswordResetToken passwordResetToken, CancellationToken cancellationToken = default)
        {
            await _context.PasswordResetTokens.AddAsync(passwordResetToken, cancellationToken);
        }

        public async Task RevogarTodosPorUsuarioAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _context.PasswordResetTokens
                .Where(x => x.UserId == userId && x.UsedAt == null && x.RevokedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
            {
                token.Revoke();
            }
        }
    }
}
