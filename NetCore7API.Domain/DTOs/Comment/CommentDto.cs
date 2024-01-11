using NetCore7API.Domain.DTOs.Interfaces;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.Comment
{
    public class CommentDto : BaseDto
    {
        public string Text { get; set; }

        public Guid PostId { get; set; }

        public Guid UserId { get; set; }

        public PublicUserDto User { get; set; }

        public PublicUserDto ResponseToUser { get; set; }
    }
}