using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore7API.Domain.Models
{
    public class Post : BaseEntity
    {
        public string Text { get; private set; } = string.Empty;
        public string? ImageURL { get; private set; } = null;
        public int TotalComments { get; private set; }
        public int TotalLikes { get; private set; }
        public Guid UserId { get; private set; } = Guid.Empty;

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public virtual User User { get; set; }

        private Post()
        {
            this.Likes = new HashSet<Like>();
            this.Comments = new HashSet<Comment>();
        }

        public Post(
            Guid userId,
            string text,
            string? imageURL)
        {
            this.UserId = userId;
            this.Text = text;
            this.ImageURL = imageURL;
            this.TotalLikes = 0;
            this.TotalComments = 0;

            this.Likes = new HashSet<Like>();
            this.Comments = new HashSet<Comment>();
        }

        public void Update(UpdatePostRequestDto dto)
        {
            this.Text = dto.Text;
            this.ImageURL = dto.ImageURL;
        }

        public void AddLike(Guid userId)
        {
            if (this.Likes.Any(x => x.UserId == userId))
                return;

            this.Likes.Add(new Models.Like(this.Id, userId));
            this.TotalLikes++;
        }

        public void RemoveLike(Guid userId)
        {
            var like = this.Likes.FirstOrDefault(x => x.UserId == userId);

            if (like is not null)
            {
                this.Likes.Remove(like);

                if (this.TotalLikes > 0)
                    this.TotalLikes--;
            }
        }

        public Comment AddComment(Guid userId, CreateCommentRequestDto dto)
        {
            var comment = new Models.Comment(this.Id, userId, dto.Text);
            this.Comments.Add(comment);
            this.TotalComments++;

            return comment;
        }

        public Comment? UpdateComment(Guid userId, Guid commentId, UpdateCommentRequestDto dto)
        {
            var comment = this.Comments
                .Where(x => x.UserId == userId)
                .FirstOrDefault(x => x.Id == commentId);

            if (comment is not null)
            {
                comment.Update(dto);
            }

            return comment;
        }

        public Comment? RemoveComment(Guid userId, Guid commentId)
        {
            var comment = this.Comments
                .Where(x => x.UserId == userId)
                .FirstOrDefault(x => x.Id == commentId);

            if (comment is not null)
            {
                this.Comments.Remove(comment);

                if (this.TotalComments > 0)
                    this.TotalComments--;
            }

            return comment;
        }
    }
}