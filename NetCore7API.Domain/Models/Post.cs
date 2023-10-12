using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore7API.Domain.Models
{
    public class Post : BaseEntity
    {
        public string Text { get; private set; }
        public string? ImageURL { get; private set; } = null;
        public int TotalLikes { get; private set; }
        public Guid UserId { get; private set; }

        [NotMapped]
        public int TotalComments { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public User User { get; set; }

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
        }

        public void Update(UpdatePostDto dto)
        {
            this.Text = dto.Text;
            this.ImageURL = dto.ImageURL;
        }

        internal void AddLike(Guid userId)
        {
            this.Likes.Add(new Models.Like(this.Id, userId));
            this.TotalLikes++;
        }

        internal void RemoveLike(Guid userId)
        {
            var like = this.Likes.FirstOrDefault(x => x.UserId == userId);

            if (like is not null)
            {
                this.Likes.Remove(like);

                if (this.TotalLikes > 0)
                    this.TotalLikes--;
            }
        }

        public void Like(Guid userId, LikePostDto dto)
        {
            if (dto.IsLiked)
                this.AddLike(userId);
            else
                this.RemoveLike(userId);
        }
    }
}