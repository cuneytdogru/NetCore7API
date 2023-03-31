using NetCore7API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Filters
{
    public class CommentFilter : BaseEntityFilter<Comment>
    {
        public Guid? PostId { get; set; }

        public override IQueryable<Comment> Apply(IQueryable<Comment> query, bool ignoreSkipTake = false)
        {
            if (PostId.HasValue)
                query = query.Where(x => x.PostId == PostId);

            return base.Apply(query, ignoreSkipTake);
        }
    }
}