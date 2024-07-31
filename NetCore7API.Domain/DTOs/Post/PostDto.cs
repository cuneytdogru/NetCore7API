using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Interfaces;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.Post
{
    public class PostDto : BaseDto
    {
        public string Text { get; set; }

        public string? ImageURL { get; set; }

        public bool IsLiked { get; set; }

        public int TotalLikes { get; set; }

        public int TotalComments { get; set; }

        public Guid UserId { get; set; }

        public IEnumerable<CommentDto> Comments { get; set; } = Array.Empty<CommentDto>();

        public PublicUserDto User { get; set; }
    }
}