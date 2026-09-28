using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.DTOs
{
    public class ConfirmEmailDto
    {
        [Required(ErrorMessage = "O token é obrigatório.")]
        public string Token { get; set; } = null!;
    }
}
