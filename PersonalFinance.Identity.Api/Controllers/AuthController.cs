using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Identity.Application.DTOs;
using PersonalFinance.Identity.Application.Interfaces;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(IAuthService authService, ICurrentUserService currentUserService)
        {
            _authService = authService;
            _currentUserService = currentUserService;
        }

        [EnableRateLimiting("auth")]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(dto, cancellationToken);

            if (result is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    message = "E-mail ou senha inválidos."
                });
            }

            return Ok(result);
        }

        [EnableRateLimiting("auth")]
        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponseDto>> Refresh(RefreshTokenDto dto, CancellationToken cancellationToken)
        {
            var result = await _authService.RefreshAsync(dto, cancellationToken);

            if (result is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    message = "Refresh token inválido ou expirado."
                });
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenDto dto, CancellationToken cancellationToken)
        {
            await _authService.LogoutAsync(dto, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                return Unauthorized();
            }

            var result = await _authService.LogoutAllAsync(userId.Value, cancellationToken);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [EnableRateLimiting("auth")]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto, CancellationToken cancellationToken)
        {
            await _authService.ForgotPasswordAsync(dto, cancellationToken);

            return Ok(new
            {
                message = "Se o e-mail informado estiver cadastrado, você receberá as instruções para redefinir sua senha."
            });
        }

        [EnableRateLimiting("auth")]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto, CancellationToken cancellationToken)
        {
            var result = await _authService.ResetPasswordAsync(dto, cancellationToken);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "O token de recuperação é inválido ou expirou."
                });
            }

            return Ok(new
            {
                message = "Senha redefinida com sucesso."
            });
        }

        [EnableRateLimiting("auth")]
        [HttpPost("confirmar-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto dto, CancellationToken cancellationToken)
        {
            var result = await _authService.ConfirmEmailAsync(dto, cancellationToken);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "O token de confirmação é inválido ou expirou."
                });
            }

            return Ok(new
            {
                message = "E-mail confirmado com sucesso."
            });
        }

        [EnableRateLimiting("auth")]
        [HttpPost("resend-email-confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationDto dto, CancellationToken cancellationToken)
        {
            await _authService.ResendEmailConfirmationAsync(dto, cancellationToken);

            return Ok(new
            {
                message = "Se houver uma conta pendente de confirmação para este e-mail, uma nova mensagem será enviada."
            });
        }
    }
}