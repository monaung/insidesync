using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsideSync.Application.DTOs
{
    public class OTP
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
