﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NetCore7API.Domain.Services;
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
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddAutoMapper(new[]
            {
                typeof(Domain.Mappings.MappingProfiles)
            });

            services.AddValidatorsFromAssemblyContaining<Domain.Validations.UserValidation>();

            return services;
        }
    }
}