using PersonalFinance.Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
        Task<LoginResponseDto?> RefreshAsync(RefreshTokenDto dto, CancellationToken cancellationToken = default);
        Task LogoutAsync(RefreshTokenDto dto, CancellationToken cancellationToken = default);
        Task<bool> LogoutAllAsync(Guid userId, CancellationToken cancellationToken = default);
        Task ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken cancellationToken = default);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken cancellationToken = default);
        Task<bool> ConfirmEmailAsync(ConfirmEmailDto dto, CancellationToken cancellationToken = default);
        Task ResendEmailConfirmationAsync(ResendEmailConfirmationDto dto, CancellationToken cancellationToken = default);
    }
}