using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Interfaces
{
    public interface ITokenValidationService
    {
        Task<bool> ValidateAsync(Guid userId, int tokenVersion, CancellationToken cancellationToken = default);
    }
}
