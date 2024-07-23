using NetCore7API.Domain.DTOs.Interfaces;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.Post
{
    public class CreatePostRequestDto
    {
        [Required]
        [MaxLength(Models.Constants.DataAnnotations.MaxLength.Text)]
        public string Text { get; set; } = string.Empty;

        [MaxLength(Models.Constants.DataAnnotations.MaxLength.Text)]
        public string? ImageURL { get; set; }
    }
}