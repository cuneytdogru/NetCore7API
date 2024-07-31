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

        public ICollection<Like> Likes { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }

        private Comment()
        {
            Likes = new HashSet<Like>();
        }

        public Comment(
            Post post,
            User user,
            string text,
            bool hidden = false)
        {
            this.Post = post;
            this.User = user;

            this.PostId = post.Id;
            this.UserId = user.Id;

            this.Text = text;
            this.Hidden = hidden;

            Likes = new HashSet<Like>();
        }

        public void Update(UpdateCommentRequestDto dto)
        {
            this.Text = dto.Text;
        }

        public void Hide(HideCommentRequestDto dto)
        {
            this.Hidden = dto.IsHidden;
        }
    }
}