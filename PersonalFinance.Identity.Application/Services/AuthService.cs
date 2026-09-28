using PersonalFinance.Identity.Application.DTOs;
using PersonalFinance.Identity.Application.Interfaces;
using PersonalFinance.Identity.Domain.Entities;
using PersonalFinance.Identity.Domain.Exceptions;
using PersonalFinance.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IPasswordResetService _passwordResetService;
        private readonly IEmailService _emailService;
        private readonly IEmailConfirmationTokenRepository _emailConfirmationTokenRepository;
        private readonly IEmailConfirmationService _emailConfirmationService;

        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IPasswordHasher passwordHasher, ITokenService tokenService, IRefreshTokenService refreshTokenService, IUnitOfWork unitOfWork, IPasswordResetTokenRepository passwordResetTokenRepository, IPasswordResetService passwordResetService, IEmailService emailService, IEmailConfirmationTokenRepository emailConfirmationTokenRepository, IEmailConfirmationService emailConfirmationService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _unitOfWork = unitOfWork;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _passwordResetService = passwordResetService;
            _emailService = emailService;
            _emailConfirmationTokenRepository = emailConfirmationTokenRepository;
            _emailConfirmationService = emailConfirmationService;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.ObterPorEmailAsync(email, cancellationToken);

            if (user is null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            var passwordValid = _passwordHasher.Verify(dto.Password, user.PasswordHash);

            if (!passwordValid)
            {
                return null;
            }

            if (!user.EmailConfirmed)
            {
                throw new DomainException("O e-mail ainda não foi confirmado.");
            }

            var token = _tokenService.GenerateToken(user);

            var refreshToken = _refreshTokenService.GenerateToken();
            var refreshTokenHash = _refreshTokenService.HashToken(refreshToken);
            var refreshTokenExpiresAt = _refreshTokenService.GetExpiration();

            var refreshTokenEntity = new RefreshToken(user.Id, refreshTokenHash, refreshTokenExpiresAt);

            await _refreshTokenRepository.AdicionarAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Nome = user.Nome,
                Email = user.Email,
                Token = token,
                ExpiresAt = _tokenService.GetExpiration(),
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt
            };
        }

        public async Task<LoginResponseDto?> RefreshAsync(RefreshTokenDto dto, CancellationToken cancellationToken = default)
        {
            var tokenHash = _refreshTokenService.HashToken(dto.RefreshToken);

            var currentRefreshToken = await _refreshTokenRepository.ObterPorHashAsync(tokenHash, cancellationToken);

            if (currentRefreshToken is null)
            {
                return null;
            }

            if (!currentRefreshToken.IsActive)
            {
                return null;
            }

            var user = await _userRepository.ObterPorIdAsync(currentRefreshToken.UserId, cancellationToken);

            if (user is null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            currentRefreshToken.Revoke();

            var newRefreshToken = _refreshTokenService.GenerateToken();
            var newRefreshTokenHash = _refreshTokenService.HashToken(newRefreshToken);
            var newRefreshTokenExpiresAt = _refreshTokenService.GetExpiration();

            var refreshTokenEntity = new RefreshToken(user.Id, newRefreshTokenHash, newRefreshTokenExpiresAt);

            await _refreshTokenRepository.AdicionarAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var token = _tokenService.GenerateToken(user);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Nome = user.Nome,
                Email = user.Email,
                Token = token,
                ExpiresAt = _tokenService.GetExpiration(),
                RefreshToken = newRefreshToken,
                RefreshTokenExpiresAt = newRefreshTokenExpiresAt
            };
        }

        public async Task LogoutAsync(RefreshTokenDto dto, CancellationToken cancellationToken = default)
        {
            var tokenHash = _refreshTokenService.HashToken(dto.RefreshToken);

            var refreshToken = await _refreshTokenRepository.ObterPorHashAsync(tokenHash, cancellationToken);

            if (refreshToken is null)
            {
                return;
            }

            refreshToken.Revoke();

            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> LogoutAllAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.ObterPorIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return false;
            }

            user.InvalidateTokens();

            await _refreshTokenRepository.RevogarTodosPorUsuarioAsync(userId, cancellationToken);
            await _userRepository.AtualizarAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto, CancellationToken cancellationToken = default)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.ObterPorEmailAsync(email, cancellationToken);

            if (user is null)
            {
                return;
            }

            await _passwordResetTokenRepository.RevogarTodosPorUsuarioAsync(user.Id, cancellationToken);

            var token = _passwordResetService.GenerateToken();
            var tokenHash = _passwordResetService.HashToken(token);
            var expiresAt = _passwordResetService.GetExpiration();

            var passwordResetToken = new PasswordResetToken(user.Id, tokenHash, expiresAt);

            await _passwordResetTokenRepository.AdicionarAsync(passwordResetToken, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            await _emailService.SendPasswordResetAsync(user.Email, token, cancellationToken);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken cancellationToken = default)
        {
            var tokenHash = _passwordResetService.HashToken(dto.Token);

            var passwordResetToken = await _passwordResetTokenRepository.ObterPorHashAsync(tokenHash, cancellationToken);

            if (passwordResetToken is null)
            {
                return false;
            }

            if (!passwordResetToken.IsActive)
            {
                return false;
            }

            var user = await _userRepository.ObterPorIdAsync(passwordResetToken.UserId, cancellationToken);

            if (user is null)
            {
                return false;
            }

            if (!user.IsActive)
            {
                return false;
            }

            var passwordHash = _passwordHasher.Hash(dto.NewPassword);

            user.ChangePassword(passwordHash);
            passwordResetToken.MarkAsUsed();

            await _refreshTokenRepository.RevogarTodosPorUsuarioAsync(user.Id, cancellationToken);
            await _userRepository.AtualizarAsync(user, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<bool> ConfirmEmailAsync(ConfirmEmailDto dto, CancellationToken cancellationToken = default)
        {
            var tokenHash = _emailConfirmationService.HashToken(dto.Token);

            var confirmationToken = await _emailConfirmationTokenRepository.ObterPorHashAsync(tokenHash, cancellationToken);

            if (confirmationToken is null)
            {
                return false;
            }

            if (!confirmationToken.IsActive)
            {
                return false;
            }

            var user = await _userRepository.ObterPorIdAsync(confirmationToken.UserId, cancellationToken);

            if (user is null)
            {
                return false;
            }

            if (!user.IsActive)
            {
                return false;
            }

            user.ConfirmEmail();
            confirmationToken.Confirm();

            await _emailConfirmationTokenRepository.RevogarTodosPorUsuarioAsync(user.Id, cancellationToken);
            await _userRepository.AtualizarAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task ResendEmailConfirmationAsync(ResendEmailConfirmationDto dto, CancellationToken cancellationToken = default)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.ObterPorEmailAsync(email, cancellationToken);

            if (user is null)
            {
                return;
            }

            if (!user.IsActive)
            {
                return;
            }

            if (user.EmailConfirmed)
            {
                return;
            }

            await _emailConfirmationTokenRepository.RevogarTodosPorUsuarioAsync(user.Id, cancellationToken);

            var token = _emailConfirmationService.GenerateToken();
            var tokenHash = _emailConfirmationService.HashToken(token);
            var expiresAt = _emailConfirmationService.GetExpiration();

            var confirmationToken = new EmailConfirmationToken(user.Id, tokenHash, expiresAt);

            await _emailConfirmationTokenRepository.AdicionarAsync(confirmationToken, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            await _emailService.SendEmailConfirmationAsync(user.Email, token, cancellationToken);
        }
    }
}
