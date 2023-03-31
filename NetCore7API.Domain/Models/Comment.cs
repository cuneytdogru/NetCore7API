using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Models.Interfaces;

namespace NetCore7API.Domain.Models
{
    public class Comment : BaseEntity
    {
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public bool Hidden { get; set; }
        public string FullName { get; set; }

        public virtual Post Post { get; set; }

        public Comment(
            Guid postId,
            string text,
            string? fullName = Constants.AnonymousFullName,
            bool hidden = false)
        {
            this.PostId = postId;
            this.Text = text;
            this.FullName = string.IsNullOrWhiteSpace(fullName) ? Constants.AnonymousFullName : fullName;
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