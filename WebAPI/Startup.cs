using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.IO;
using System.Linq;
using System.Reflection;
=======
using System.Linq;
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
<<<<<<< HEAD
using Microsoft.OpenApi.Models;
using Repositories.Extensions;
using Services.Extensions;
=======
using Repositories;
using Repositories.Extensions;
using Repositories.Interface;
using Repositories.Repository;
using Services.Extensions;
using Utilities;
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
<<<<<<< HEAD
            services.AddSwagger();
            services.AddRepositoryDependencies(Configuration);
            services.AddServiceDependencies(Configuration);
=======
            RepositoryDependency.AllDependency(services, Configuration);
            ServiceDependency.ALLDependency(services, Configuration);
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

<<<<<<< HEAD
            app.UseCustomSwagger();

=======
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
            app.UseHttpsRedirection();

            app.UseRouting();

<<<<<<< HEAD
=======
            app.UseAuthentication();

>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
