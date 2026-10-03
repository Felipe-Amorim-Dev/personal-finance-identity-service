using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Application.DTOs
{
    public class UserFilterDto
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;

        public string? Search { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
        public bool? EmailConfirmed { get; set; }
    }
}
