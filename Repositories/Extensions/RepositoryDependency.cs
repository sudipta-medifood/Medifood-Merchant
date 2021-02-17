using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Interface;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Repositories.Extensions
{
    public class RepositoryDependency
    {
        public static void AllDependency(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DataContext>(x => x.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMerchantProfileRepository, MerchantProfileRepository>();
        }
    }
}
