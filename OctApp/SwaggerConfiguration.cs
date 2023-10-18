using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace OctApp
{
    public static class SwaggerConfiguration
    {
         public static void ConfigureSwagger(this IServiceCollection services)
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authentication for minimal Api"
            };

            var securityRequirements = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            };

            var contactInfo = new OpenApiContact()
            {
                Name = "Abayomi Joe",
                Email = "aby@gm.com",
                Url = new Uri("https://kaol.ka")
            };

            var licence = new OpenApiLicense()
            {
                Name = "Free License",
            };

            var info = new OpenApiInfo()
            {
                Version = "V1",
                Title = "Booking Api",
                Description = "Restful Api for Booking",
                Contact = contactInfo,
                License = licence
            };

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info);
                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(securityRequirements);
            });
        }
    }
    }
