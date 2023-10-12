using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Models.Interfaces;

namespace NetCore7API.Domain.Models
{
    public class Comment : BaseEntity
    {
        public Guid PostId { get; private set; }
        public Guid UserId { get; private set; }
        public string Text { get; private set; }
        public bool Hidden { get; private set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }

        public Comment(
            Guid postId,
            Guid userId,
            string text,
            bool hidden = false)
        {
            this.PostId = postId;
            this.UserId = userId;
            this.Text = text;
            this.Hidden = hidden;
        }

        public void Update(UpdateCommentDto dto)
        {
            this.Text = dto.Text;
        }

        public void Hide(HideCommentDto dto)
        {
            this.Hidden = dto.IsHidden;
        }
    }
}