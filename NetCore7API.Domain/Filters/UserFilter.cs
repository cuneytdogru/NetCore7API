using NetCore7API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Filters
{
    public class UserFilter : BaseEntityFilter<User>
    {
        public override IQueryable<User> Apply(IQueryable<User> query, bool ignoreSkipTake = false)
        {
            return base.Apply(query, ignoreSkipTake);
        }
    }
}