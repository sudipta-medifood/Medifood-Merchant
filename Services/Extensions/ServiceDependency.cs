<<<<<<< HEAD
﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using Services.Interfaces;
using Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Services.Extensions
{
    public static class ServiceDependency
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
=======
﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Services.Interface;
using Services.Service;
using Utilities;

namespace Services.Extensions
{
    public class ServiceDependency
    {
        public static void ALLDependency(IServiceCollection services, IConfiguration configuration)
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
        {
            services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();
<<<<<<< HEAD
            return services;
=======
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("TokenSettings:TokenSecretKey").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
        }
    }
}
