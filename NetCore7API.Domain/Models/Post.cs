using NetCore7API.Domain.DTOs.Comment;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Services;
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
            User user,
            string text,
            string? imageURL)
        {
            this.User = user;
            this.UserId = user.Id;

            this.Text = text;
            this.ImageURL = imageURL;
            this.TotalLikes = 0;
            this.TotalComments = 0;

            this.Likes = new HashSet<Like>();
            this.Comments = new HashSet<Comment>();
        }

        public void Update(UpdatePostRequestDto dto, User user)
        {
            if (this.UserId != user.Id)
                throw new UserUnauthorizedException("You are not authorized to modify this post.");

            this.Text = dto.Text;
            this.ImageURL = dto.ImageURL;
        }

        public void Delete(User user)
        {
            if (this.UserId != user.Id)
                throw new UserUnauthorizedException("You are not authorized to modify this post.");

            this.Deleted = true;
        }

        public void AddLike(User user)
        {
            if (this.Likes.Any(x => x.UserId == user.Id))
                return;

            this.Likes.Add(new Models.Like(this, user));
            this.TotalLikes++;
        }

        public void RemoveLike(User user)
        {
            var like = this.Likes.FirstOrDefault(x => x.UserId == user.Id);

            if (like is not null)
            {
                this.Likes.Remove(like);

                if (this.TotalLikes > 0)
                    this.TotalLikes--;
            }
        }

        public Comment AddComment(CreateCommentRequestDto dto, User user)
        {
            var comment = new Models.Comment(this, user, dto.Text);
            this.Comments.Add(comment);
            this.TotalComments++;

            return comment;
        }

        public Comment UpdateComment(UpdateCommentRequestDto dto, Comment comment, User user)
        {
            if (comment.UserId != user.Id)
                throw new UserUnauthorizedException("You are not authorized to modify this comment.");

            comment.Update(dto);

            return comment;
        }

        public void RemoveComment(Comment comment, User user)
        {
            if (comment.UserId != user.Id)
                throw new UserUnauthorizedException("You are not authorized to modify this comment.");

            this.Comments.Remove(comment);

            if (this.TotalComments > 0)
                this.TotalComments--;
        }
    }
}