using NetCore7API.Domain.DTOs.Comment;
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
    public class PostDto : BaseDto
    {
        public string Text { get; internal set; }

        public string? ImageURL { get; internal set; }

        public string? FullName { get; internal set; }

        public int Likes { get; internal set; }

        public int TotalComments { get; internal set; }

        public IEnumerable<CommentDto> Comments { get; internal set; } = Array.Empty<CommentDto>();
    }
}