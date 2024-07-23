using NetCore7API.Domain.DTOs.Interfaces;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.User
{
    public class RegisterUserRequestDto
    {
        [Required]
        [MaxLength(Models.Constants.DataAnnotations.MaxLength.Username)]
        [MinLength(Models.Constants.DataAnnotations.MinLength.Username)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(Models.Constants.DataAnnotations.MaxLength.Default)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(Models.Constants.DataAnnotations.MaxLength.Password)]
        [MinLength(Models.Constants.DataAnnotations.MinLength.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(Models.Constants.DataAnnotations.MaxLength.Default)]
        public string FullName { get; set; } = string.Empty;
    }
}