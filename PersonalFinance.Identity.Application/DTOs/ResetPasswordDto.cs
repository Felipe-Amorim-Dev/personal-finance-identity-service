using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.DTOs
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "O token é obrigatório.")]
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "A senha deve possuir no mínimo 8 caracteres.")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
        [Compare(nameof(NewPassword), ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
