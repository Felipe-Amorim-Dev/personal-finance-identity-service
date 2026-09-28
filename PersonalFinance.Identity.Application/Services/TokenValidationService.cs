using PersonalFinance.Identity.Application.Interfaces;
using PersonalFinance.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Services
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly IUserRepository _userRepository;

        public TokenValidationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateAsync(Guid userId, int tokenVersion, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.ObterPorIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return false;
            }

            if (!user.IsActive)
            {
                return false;
            }

            if (user.TokenVersion != tokenVersion)
            {
                return false;
            }

            return true;
        }
    }
}
