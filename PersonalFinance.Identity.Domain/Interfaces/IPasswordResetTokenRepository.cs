using PersonalFinance.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task<PasswordResetToken?> ObterPorHashAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task AdicionarAsync(PasswordResetToken passwordResetToken, CancellationToken cancellationToken = default);
        Task RevogarTodosPorUsuarioAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
