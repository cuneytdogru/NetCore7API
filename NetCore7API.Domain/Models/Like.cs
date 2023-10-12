using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Models.Interfaces;

namespace NetCore7API.Domain.Models
{
    public class Like : BaseEntity
    {
        public Guid PostId { get; private set; }
        public Guid? CommentId { get; private set; }
        public Guid UserId { get; private set; }

        public virtual Post Post { get; set; }
        public virtual Comment? Comment { get; set; }
        public virtual User User { get; set; }

        public Like(
            Guid postId,
            Guid userId,
            Guid? commentId = null)
        {
            this.PostId = postId;
            this.CommentId = commentId;
            this.UserId = userId;
        }
    }
}