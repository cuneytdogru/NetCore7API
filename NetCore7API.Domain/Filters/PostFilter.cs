using NetCore7API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Filters
{
    public class PostFilter : BaseEntityFilter<Post>
    {
        public Guid? UserId { get; set; }

        public string? UserName { get; set; }

        public string? LikedByUserName { get; set; }

        public override IQueryable<Post> Apply(IQueryable<Post> query, bool ignoreSkipTake = false)
        {
            if (UserId.HasValue)
                query = query.Where(x => x.UserId == UserId);
            else if (!string.IsNullOrWhiteSpace(UserName))
            {
                query = query.Where(x => x.User.UserName == UserName);
            }

            if (!string.IsNullOrWhiteSpace(LikedByUserName))
            {
                query = query.Where(x => x.Likes.Any(x => x.User.UserName == LikedByUserName));
            }

            return base.Apply(query, ignoreSkipTake);
        }
    }
}