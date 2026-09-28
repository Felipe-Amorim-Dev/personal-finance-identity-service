using PersonalFinance.Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Services
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private const int ExpirationHours = 24;

        public string GenerateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }

        public DateTime GetExpiration()
        {
            return DateTime.UtcNow.AddHours(ExpirationHours);
        }
    }
}
