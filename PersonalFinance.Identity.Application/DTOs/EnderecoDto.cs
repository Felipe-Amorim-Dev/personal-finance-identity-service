using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.DTOs
{
    public class EnderecoDto
    {
        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        [StringLength(200)]
        public string Logradouro { get; set; } = null!;

        [Required(ErrorMessage = "O número é obrigatório.")]
        [StringLength(20)]
        public string Numero { get; set; } = null!;

        [StringLength(150)]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [StringLength(100)]
        public string Bairro { get; set; } = null!;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [StringLength(100)]
        public string Cidade { get; set; } = null!;

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(100)]
        public string Estado { get; set; } = null!;

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(20)]
        public string Cep { get; set; } = null!;

        [Required(ErrorMessage = "O país é obrigatório.")]
        [StringLength(100)]
        public string Pais { get; set; } = null!;
    }
}
