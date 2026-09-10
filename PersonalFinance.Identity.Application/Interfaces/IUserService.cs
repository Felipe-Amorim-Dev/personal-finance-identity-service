using PersonalFinance.Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default);

        Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
