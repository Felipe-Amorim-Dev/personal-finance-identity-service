using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Identity.Application.DTOs;
using PersonalFinance.Identity.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PersonalFinance.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public UserController(IUserService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto, CancellationToken cancellationToken)
        {
            var user = await _userService.CreateAsync(dto, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, user);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("buscar-email")]
        public async Task<ActionResult<UserDto>> GetByEmail([FromQuery] string email, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpGet("buscar")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                return Unauthorized();
            }

            var user = await _userService.GetByIdAsync(userId.Value, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPut("atualizar")]
        public async Task<ActionResult<UserDto>> UpdateCurrentUser(UpdateUserDto dto, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                return Unauthorized();
            }

            var user = await _userService.GetByIdAsync(userId.Value, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPut("alterar-senha")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                return Unauthorized();
            }

            var user = await _userService.GetByIdAsync(userId.Value, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpPatch("desativar-usuario")]
        public async Task<IActionResult> Deactivate(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                return Unauthorized();
            }

            var user = await _userService.GetByIdAsync(userId.Value, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
