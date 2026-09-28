using PersonalFinance.Identity.Domain.Enums;
using PersonalFinance.Identity.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Entities
{
    public class User : Entity
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; } = null!;
        public string Sobrenome { get; private set; } = null!;
        public DateTime DataNascimento { get; private set; }
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; } 
        public DateTime? UpdatedAt { get; private set; }
        public Endereco Endereco { get; private set; } = null!;        
        public bool EmailConfirmed { get; private set; }
        public UserRole Role { get; private set; }
        public int TokenVersion { get; private set; }        

        private User()
        {
        }

        public User(string nome, string sobrenome, DateTime dataNascimento, string email, string passwordHash, Endereco endereco)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Sobrenome = sobrenome;
            DataNascimento = dataNascimento;
            Email = email;
            PasswordHash = passwordHash;
            Endereco = endereco;
            IsActive = true;            
            EmailConfirmed = false;
            Role = UserRole.User;
            TokenVersion = 1;
            CreatedAt = DateTime.UtcNow;            

            AddDomainEvent(new UserCreatedDomainEvent(Id, Nome, Email));
        }

        public void UpdateProfile(string nome, string sobrenome, DateTime dataNascimento, string logradouro, string numero, string? complemento, string bairro, string cidade, string estado, string cep, string pais)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            DataNascimento = dataNascimento;

            Endereco.Update(logradouro, numero, complemento, bairro, cidade, estado, cep, pais);

            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserUpdatedDomainEvent(Id, Nome, Sobrenome, DataNascimento, Email, Endereco.Logradouro, Endereco.Numero, Endereco.Complemento, Endereco.Bairro, Endereco.Cidade, Endereco.Estado, Endereco.Cep, Endereco.Pais));
        }

        public void ChangePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
            TokenVersion++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            if (!IsActive)
            {
                return;
            }

            IsActive = false;
            TokenVersion++;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserDeactivatedDomainEvent(Id, Email));
        }

        public void InvalidateTokens()
        {
            TokenVersion++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ConfirmEmail()
        {
            if (EmailConfirmed)
            {
                return;
            }

            EmailConfirmed = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeRole(UserRole role)
        {
            if (Role == role)
            {
                return;
            }

            Role = role;
            TokenVersion++;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
