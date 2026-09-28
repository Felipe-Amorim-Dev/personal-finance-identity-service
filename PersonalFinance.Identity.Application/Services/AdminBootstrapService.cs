using PersonalFinance.Identity.Application.Interfaces;
using PersonalFinance.Identity.Domain.Enums;
using PersonalFinance.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Services
{
    public class AdminBootstrapService : IAdminBootstrapService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminBootstrapService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task EnsureAdminAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            var adminExists = await _userRepository.ExisteRoleAsync(UserRole.Admin, cancellationToken);

            if (adminExists)
            {
                return;
            }

            var normalizedEmail = email.Trim().ToLowerInvariant();

            var user = await _userRepository.ObterPorEmailAsync(normalizedEmail, cancellationToken);

            if (user is null)
            {
                return;
            }

            user.ChangeRole(UserRole.Admin);

            await _userRepository.AtualizarAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
