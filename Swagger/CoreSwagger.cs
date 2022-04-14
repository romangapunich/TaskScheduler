using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace TaskScheduler.Swagger

{
    public static class SwaggerExtension
    {
        public static void UseCoreSwagger(this IApplicationBuilder app,string url,string name, Action<SwaggerUIOptions> setupAction = null)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options => {
                    options.SwaggerEndpoint(url, name);
                    setupAction?.Invoke(options);
                    options.RoutePrefix = string.Empty;
                }
            );
        }

        public static void AddCoreSwagger(this IServiceCollection services,  string name, string ver, string description)
        {
            var commentsPath = Path.Combine(AppContext.BaseDirectory, $"{name}.xml");
  
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = $"{ver}",
                    Title = $"{name} API"
                });
                // Add Comments
                c.IncludeXmlComments(commentsPath);
                // Bearer token authentication
                var securityDefinition = new OpenApiSecurityScheme {
                    Name = name,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = description,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                };
                c.AddSecurityDefinition("jwt_auth", securityDefinition);

                // Make sure swagger UI requires a Bearer token specified
                var securityScheme = new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Id = "jwt_auth",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                var securityRequirements = new OpenApiSecurityRequirement {
                    { securityScheme, new string[] { } }
                };
                c.AddSecurityRequirement(securityRequirements);

            });
        }
    }
}
