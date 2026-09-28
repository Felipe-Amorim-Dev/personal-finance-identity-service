using Microsoft.AspNetCore.Http;
using PersonalFinance.Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public Guid? UserId
        {
            get
            {
                var value = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (Guid.TryParse(value, out var userId))
                {
                    return userId;
                }

                return null;
            }
        }

        public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? Role => User?.FindFirst(ClaimTypes.Role)?.Value;
    }
}
