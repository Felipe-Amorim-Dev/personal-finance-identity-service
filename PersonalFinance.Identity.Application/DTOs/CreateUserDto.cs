using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.DTOs
{
    public class CreateUserDto
    {
        public string Nome { get; set; } = null!;
        public string Sobrenome { get; set; } = null!;
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public EnderecoDto Endereco { get; set; } = null!;
    }
}
