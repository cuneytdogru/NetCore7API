using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NetCore7API.Domain.Models
{
    public class Like : BaseEntity
    {
        public Guid PostId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid? CommentId { get; private set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual Comment? Comment { get; set; }

        private Like()
        {
        }

        public Like(
            Post post,
            User user,
            Comment? comment = null)
        {
            this.Post = post;
            this.User = user;

            this.PostId = post.Id;
            this.UserId = user.Id;

            if (comment is not null)
            {
                this.Comment = comment;
                this.CommentId = comment.Id;
            }
        }
    }
}