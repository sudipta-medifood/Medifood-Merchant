using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Interfaces;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Extensions
{
    public static class RepositoryDependency
    {
        public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DataContext>(x => x.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAuthRepository, AuthRepository>();
            return services;
        }
    }
}
