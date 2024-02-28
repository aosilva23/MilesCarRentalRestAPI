using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace milescarrental.API.Configuration
{
    internal static class SwaggerExtensions
    {
        internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API milescarrentalAPI",
                    Version = "v1",
                    Description = "milescarrentalAPI. ",
                });

                //var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                //var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                //options.IncludeXmlComments(commentsFile);
            });

            return services;
        }

        internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "milescarrental API V1");
            });

            return app;
        }
    }
}