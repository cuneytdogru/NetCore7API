using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        [MinLength(Models.Constants.DataAnnotations.MinLength.Username)]
        [MaxLength(Models.Constants.DataAnnotations.MaxLength.Username)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MinLength(Models.Constants.DataAnnotations.MinLength.Password)]
        [MaxLength(Models.Constants.DataAnnotations.MaxLength.Password)]
        public string Password { get; set; } = string.Empty;
    }
}