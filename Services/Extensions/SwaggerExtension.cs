using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services.Extensions
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            //Register the swagger generator, defining 1 or more swagger documents
            services.AddSwaggerGen(swagergen =>
            {
                swagergen.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Medifood Rider",
                    Version = "v1",
                    Description = "API to perform Rider operations",
                    TermsOfService = new Uri("http://medifoodex.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Medifood",
                        Email = "medifoodx@gmail.com",
                        Url = new Uri("http://medifoodex.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Medifood Rider API License",
                        Url = new Uri("http://medifoodex.com")
                    }
                });
                // Set the comments path for the Swagger JSON and UI
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "WebAPI.xml");
                swagergen.IncludeXmlComments(xmlPath);
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated swagger as JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etx.),
            // specifying the swagger JSON endpoints
            app.UseSwaggerUI(swaggerui =>
            {
                swaggerui.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
