using NetCore7API.Domain.DTOs.Interfaces;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public string Text { get; set; }
    }
}