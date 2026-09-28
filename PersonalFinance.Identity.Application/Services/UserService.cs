using PersonalFinance.Identity.Application.DTOs;
using PersonalFinance.Identity.Application.Interfaces;
using PersonalFinance.Identity.Domain.Entities;
using PersonalFinance.Identity.Domain.Enums;
using PersonalFinance.Identity.Domain.Exceptions;
using PersonalFinance.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IEmailConfirmationTokenRepository _emailConfirmationTokenRepository;
        private readonly IEmailConfirmationService _emailConfirmationService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, IEmailConfirmationTokenRepository emailConfirmationTokenRepository, IEmailConfirmationService emailConfirmationService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _emailConfirmationTokenRepository = emailConfirmationTokenRepository;
            _emailConfirmationService = emailConfirmationService;
            _emailService = emailService;
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            if (await _userRepository.ExisteEmailAsync(email, cancellationToken))
            {
                throw new DomainException("Já existe um usuário cadastrado com este e-mail.");
            }

            var passwordHash = _passwordHasher.Hash(dto.Password);

            var endereco = new Endereco(
                dto.Endereco.Logradouro,
                dto.Endereco.Numero,
                dto.Endereco.Complemento,
                dto.Endereco.Bairro,
                dto.Endereco.Cidade,
                dto.Endereco.Estado,
                dto.Endereco.Cep,
                dto.Endereco.Pais
            );

            var user = new User(dto.Nome, dto.Sobrenome, dto.DataNascimento, email, passwordHash, endereco);

            await _userRepository.AdicionarAsync(user, cancellationToken);

            var confirmationToken = _emailConfirmationService.GenerateToken();
            var confirmationTokenHash = _emailConfirmationService.HashToken(confirmationToken);
            var confirmationTokenExpiresAt = _emailConfirmationService.GetExpiration();

            var emailConfirmationToken = new EmailConfirmationToken(user.Id, confirmationTokenHash, confirmationTokenExpiresAt);

            await _emailConfirmationTokenRepository.AdicionarAsync(emailConfirmationToken, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            await _emailService.SendEmailConfirmationAsync(user.Email, confirmationToken, cancellationToken);

            return MapToDto(user);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.ObterPorIdAsync(id, cancellationToken);

            if (user is null)
            {
                return null;
            }                

            return MapToDto(user);
        }

        public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.ObterPorEmailAsync(email, cancellationToken);

            if (user is null)
            {
                return null;
            }                

            return MapToDto(user);
        }

        public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.ObterPorIdAsync(id, cancellationToken);

            if (user is null)
            {
                return null;
            }

            user.UpdateProfile(dto.Nome, dto.Sobrenome, dto.DataNascimento, dto.Endereco.Logradouro, dto.Endereco.Numero, dto.Endereco.Complemento, dto.Endereco.Bairro, dto.Endereco.Cidade, dto.Endereco.Estado, dto.Endereco.Cep, dto.Endereco.Pais);

            await _userRepository.AtualizarAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return MapToDto(user);
        }

        public async Task<bool> ChangePasswordAsync(Guid id, ChangePasswordDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.ObterPorIdAsync(id, cancellationToken);

            if (user is null)
            {
                return false;
            }

            var currentPasswordValid = _passwordHasher.Verify(dto.CurrentPassword, user.PasswordHash);

            if (!currentPasswordValid)
            {
                throw new DomainException("A senha atual informada é inválida.");
            }

            var newPasswordHash = _passwordHasher.Hash(dto.NewPassword);

            user.ChangePassword(newPasswordHash);

            await _refreshTokenRepository.RevogarTodosPorUsuarioAsync(user.Id, cancellationToken);
            await _userRepository.AtualizarAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.ObterPorIdAsync(id, cancellationToken);

            if (user is null)
            {
                return false;
            }

            user.Deactivate();

            await _refreshTokenRepository.RevogarTodosPorUsuarioAsync(user.Id, cancellationToken);
            await _userRepository.AtualizarAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<bool> ChangeRoleAsync(Guid userId, ChangeUserRoleDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.ObterPorIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return false;
            }

            if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            {
                throw new DomainException("A role informada é inválida.");
            }

            if (!Enum.IsDefined(role))
            {
                throw new DomainException("A role informada é inválida.");
            }

            user.ChangeRole(role);

            await _userRepository.AtualizarAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        #region Metodos Privados
        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Nome = user.Nome,
                Sobrenome = user.Sobrenome,
                DataNascimento = user.DataNascimento,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,

                Endereco = new EnderecoDto
                {
                    Logradouro = user.Endereco.Logradouro,
                    Numero = user.Endereco.Numero,
                    Complemento = user.Endereco.Complemento,
                    Bairro = user.Endereco.Bairro,
                    Cidade = user.Endereco.Cidade,
                    Estado = user.Endereco.Estado,
                    Cep = user.Endereco.Cep,
                    Pais = user.Endereco.Pais
                }
            };
        }
        #endregion
    }
}

