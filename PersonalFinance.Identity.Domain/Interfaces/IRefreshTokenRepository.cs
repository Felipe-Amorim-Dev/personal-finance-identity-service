using PersonalFinance.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> ObterPorHashAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task AdicionarAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task RevogarTodosPorUsuarioAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
