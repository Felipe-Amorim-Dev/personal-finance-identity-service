using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.DTOs
{
    public class ChangeUserRoleDto
    {
        [Required(ErrorMessage = "A role é obrigatória.")]
        public string Role { get; set; } = null!;
    }
}
