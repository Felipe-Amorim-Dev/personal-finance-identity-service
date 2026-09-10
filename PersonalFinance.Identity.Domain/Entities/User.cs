using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Sobrenome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Endereco Endereco { get; private set; }

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
            CreatedAt = DateTime.UtcNow;
        }
    }
}
