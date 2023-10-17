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
        public Guid PostId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid? CommentId { get; private set; }
    }
}