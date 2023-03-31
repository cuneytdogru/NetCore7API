using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(Domain.Services.IPostService), typeof(PostService));
            services.AddScoped(typeof(Domain.Services.ICommentService), typeof(CommentService));

            services.AddAutoMapper(new[]
            {
                typeof(Domain.Mappings.MappingProfiles)
            });

            return services;
        }
    }
}