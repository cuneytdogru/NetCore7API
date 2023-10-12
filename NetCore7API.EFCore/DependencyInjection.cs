using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore7API.Domain.Repositories;
using NetCore7API.EFCore.Context;
using NetCore7API.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.EFCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BlogContext>(options => options.UseSqlServer(configuration.GetConnectionString("BlogContext")));

            services.AddScoped(typeof(Domain.Repositories.IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, BlogUnitOfWork>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();

            return services;
        }
    }
}