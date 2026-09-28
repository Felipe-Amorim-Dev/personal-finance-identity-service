using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Interfaces
{
    public interface IAdminBootstrapService
    {
        Task EnsureAdminAsync(string email, CancellationToken cancellationToken = default);
    }
}
