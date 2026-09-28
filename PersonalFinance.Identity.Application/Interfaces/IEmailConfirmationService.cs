using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Interfaces
{
    public interface IEmailConfirmationService
    {
        string GenerateToken();
        string HashToken(string token);
        DateTime GetExpiration();
    }
}
