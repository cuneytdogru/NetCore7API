using NetCore7API.Domain.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.DTOs.Like
{
    public class LikeDto : BaseDto
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public Guid? CommentId { get; set; }
    }
}