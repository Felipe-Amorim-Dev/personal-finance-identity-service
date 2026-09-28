using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.DTOs
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "O refresh token é obrigatório.")]
        public string RefreshToken { get; set; } = null!;
    }
}
