using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Domain.Entities
{
    public class Endereco
    {
        public Guid Id { get; private set; }
        public string Logradouro { get; private set; } = null!;
        public string Numero { get; private set; } = null!;
        public string? Complemento { get; private set; }
        public string Bairro { get; private set; } = null!;
        public string Cidade { get; private set; } = null!;
        public string Estado { get; private set; } = null!;
        public string Cep { get; private set; } = null!;
        public string Pais { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Endereco()
        {
        }

        public Endereco(string logradouro, string numero, string? complemento, string bairro, string cidade, string estado, string cep, string pais)
        {
            Id = Guid.NewGuid();
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
            Pais = pais;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
