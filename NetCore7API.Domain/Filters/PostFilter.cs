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
        public Guid? Owner { get; set; }

        public override IQueryable<Post> Apply(IQueryable<Post> query, bool ignoreSkipTake = false)
        {
            if (Owner.HasValue)
                query = query.Where(x => x.CreatedBy == Owner);

            return base.Apply(query, ignoreSkipTake);
        }
    }
}