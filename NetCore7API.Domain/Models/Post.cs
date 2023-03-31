using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore7API.Domain.Models
{
    public class Post : BaseEntity
    {
        public string Text { get; set; }
        public string? ImageURL { get; set; } = null;
        public string FullName { get; set; }

        public int Likes { get; set; }

        [NotMapped]
        public int TotalComments { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Post(
            string text,
            string? imageURL,
            string? fullName = Constants.AnonymousFullName)
        {
            this.Text = text;
            this.ImageURL = imageURL;
            this.FullName = string.IsNullOrWhiteSpace(fullName) ? Constants.AnonymousFullName : fullName;
            this.Likes = 0;
        }

        public void Update(UpdatePostDto dto)
        {
            this.Text = dto.Text;
            this.ImageURL = dto.ImageURL;
        }

        public void Like(LikePostDto dto)
        {
            if (dto.IsLiked)
                this.Likes++;
            else if (this.Likes > 0)
                this.Likes--;
        }
    }
}