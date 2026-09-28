using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "A senha atual é obrigatória.")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A nova senha deve possuir entre 8 e 100 caracteres.")]
        public string NewPassword { get; set; } = null!;
    }
}
