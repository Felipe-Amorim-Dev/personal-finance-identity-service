using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetAsync(string email, string token, CancellationToken cancellationToken = default);
        Task SendEmailConfirmationAsync(string email, string token, CancellationToken cancellationToken = default);
    }
}
