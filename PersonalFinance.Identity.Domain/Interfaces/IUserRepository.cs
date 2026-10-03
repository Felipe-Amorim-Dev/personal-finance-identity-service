using PersonalFinance.Identity.Domain.Entities;
using PersonalFinance.Identity.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AdicionarAsync(User user, CancellationToken cancellationToken = default);
        Task AtualizarAsync(User user, CancellationToken cancellationToken = default);
        Task RemoverAsync(User user, CancellationToken cancellationToken = default);
        Task<bool> ExisteRoleAsync(UserRole role, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<User> Items, int TotalCount)> ObterPaginadoAsync(int page, int pageSize, string? search, UserRole? role, bool? isActive, bool? emailConfirmed, CancellationToken cancellationToken = default);
    }
}
