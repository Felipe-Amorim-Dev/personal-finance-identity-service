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
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;

        public UserRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<User?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Endereco).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<User?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Endereco).FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }

        public async Task AdicionarAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public Task AtualizarAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.Users.Update(user);

            return Task.CompletedTask;
        }

        public Task RemoverAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.Users.Remove(user);

            return Task.CompletedTask;
        }
    }
}