using PersonalFinance.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        DateTime GetExpiration();
    }
}
