using NetCore7API.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.EFCore.Providers
{
    public class Provider : IProvider
    {
        protected readonly Context.BlogContext Context;

        public Provider(Context.BlogContext context)
        {
            this.Context = context;
        }
    }
}