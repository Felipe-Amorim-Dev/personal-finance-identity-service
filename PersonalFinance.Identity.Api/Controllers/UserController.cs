using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Identity.Application.DTOs;
using PersonalFinance.Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto, CancellationToken cancellationToken)
        {
            var user = await _userService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
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

        [HttpGet("by-email")]
        public async Task<ActionResult<UserDto>> GetByEmail([FromQuery] string email, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
