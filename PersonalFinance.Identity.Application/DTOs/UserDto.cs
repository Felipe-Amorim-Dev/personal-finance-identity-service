using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Sobrenome { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public EnderecoDto Endereco { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
