using PersonalFinance.Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Events
{
    public class UserUpdatedDomainEvent : IDomainEvent
    {
        public string EventType => "identity.user.updated.v1";
        public Guid UserId { get; }
        public string Nome { get; }
        public string Sobrenome { get; }
        public DateTime DataNascimento { get; }
        public string Email { get; }
        public string Logradouro { get; }
        public string Numero { get; }
        public string? Complemento { get; }
        public string Bairro { get; }
        public string Cidade { get; }
        public string Estado { get; }
        public string Cep { get; }
        public string Pais { get; }
        public DateTime OccurredAt { get; }

        public UserUpdatedDomainEvent(Guid userId, string nome, string sobrenome, DateTime dataNascimento, string email, string logradouro, string numero, string? complemento, string bairro, string cidade, string estado, string cep, string pais)
        {
            UserId = userId;
            Nome = nome;
            Sobrenome = sobrenome;
            DataNascimento = dataNascimento;
            Email = email;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
            Pais = pais;
            OccurredAt = DateTime.UtcNow;
        }
    }
}
