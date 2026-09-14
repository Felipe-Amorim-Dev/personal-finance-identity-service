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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var email = dto.Email.Trim().ToLowerInvariant();
            var emailExiste = await _userRepository.ExisteEmailAsync(dto.Email, cancellationToken);

            if (emailExiste)
            {
                throw new DomainException("Já existe um usuário cadastrado com este e-mail.");
            }                

            var passwordHash = _passwordHasher.Hash(dto.Password);

            var endereco = new Endereco(dto.Endereco.Logradouro, dto.Endereco.Numero, dto.Endereco.Complemento, dto.Endereco.Bairro, dto.Endereco.Cidade, dto.Endereco.Estado, dto.Endereco.Cep, dto.Endereco.Pais);

            var user = new User(dto.Nome, dto.Sobrenome, dto.DataNascimento, dto.Email, passwordHash, endereco);

            await _userRepository.AdicionarAsync(user, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

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

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Nome = user.Nome,
                Sobrenome = user.Sobrenome,
                DataNascimento = user.DataNascimento,
                Email = user.Email,
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
    }
}

