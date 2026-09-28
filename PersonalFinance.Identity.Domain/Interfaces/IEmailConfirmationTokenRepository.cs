using PersonalFinance.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Interfaces
{
    public interface IEmailConfirmationTokenRepository
    {
        Task<EmailConfirmationToken?> ObterPorHashAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task AdicionarAsync(EmailConfirmationToken emailConfirmationToken, CancellationToken cancellationToken = default);
        Task RevogarTodosPorUsuarioAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
