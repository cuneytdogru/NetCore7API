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
        public bool ShowHidden { get; set; } = false;

        public override IQueryable<Comment> Apply(IQueryable<Comment> query, bool ignoreSkipTake = false)
        {
            if (!ShowHidden)
                query = query.Where(x => x.Hidden == false);

            return base.Apply(query, ignoreSkipTake);
        }
    }
}