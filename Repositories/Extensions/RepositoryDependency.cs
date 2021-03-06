using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
<<<<<<< HEAD
using Repositories.Interfaces;
using Repositories.Repository;
using System;
using System.Collections.Generic;
=======
using Repositories.Interface;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
using System.Text;

namespace Repositories.Extensions
{
<<<<<<< HEAD
    public static class RepositoryDependency
    {
        public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DataContext>(x => x.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAuthRepository, AuthRepository>();
            return services;
=======
    public class RepositoryDependency
    {
        public static void AllDependency(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DataContext>(x => x.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMerchantProfileRepository, MerchantProfileRepository>();
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
        }
    }
}
