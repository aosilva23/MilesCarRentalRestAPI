using System;
using System.Linq;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using milescarrental.API.Configuration;
using milescarrental.Application.Configuration.Validation;
using milescarrental.API.SeedWork;
using milescarrental.Domain.SeedWork;
using milescarrental.Infrastructure;
using milescarrental.Infrastructure.Database;
using milescarrental.Infrastructure.SeedWork;
using milescarrental.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;
using milescarrental.API.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

[assembly: UserSecretsId("54e8eb06-aaa1-4fff-9f05-3ced1cb623c2")]
namespace milescarrental.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private const string OrdersConnectionString = "ConnectionString";
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IWebHostEnvironment env)
        {
            this._configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddJsonFile($"hosting.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // agregar las excepciones de CORs:
            services.AddCors();            

            AppConfiguration appConfig = this._configuration.GetSection("AppConfigurarion").Get<AppConfiguration>();
            var children = this._configuration.GetSection("Caching").GetChildren();
            var cachingConfiguration = children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));
            //oid integration (KeyCloak)
            IdentityModelEventSource.ShowPII = true;

            services.AddSingleton(appConfig);
            #region Api Versioning
            // Add API Versioning to the Project
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
            #endregion
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            services.AddControllers();
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidationFilter());
            })
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });
            services.AddMemoryCache();
            services.AddSwaggerDocumentation();
            services.AddDbContext<OrdersContext>(options =>
                {
                    options
                        .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>()
                        
                        .UseSqlServer(this._configuration[OrdersConnectionString]);
                });
            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
            });

            #region Probar cargue token JWT por Swagger, el token se genera desde api/v1/permisosacceso/Autenticacion 

            services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
                        "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
                        "Ingresa la palabra 'Bearer' seguida de un [espacio] y despues su token en el campo de abajo \r\n\r\n" +
                        "Ejemplo: \"Bearer tkdknkdllskd\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "Bearer"
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                            {
                                                Type = ReferenceType.SecurityScheme,
                                                Id = "Bearer"
                                            },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });
                });

            #endregion

            //services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());            

            #region Agregar Configuracion Autenciación por Token con JWT

            var key = appConfig.ApiKey;
                byte[] KeyBytes = Encoding.ASCII.GetBytes(key);

                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(KeyBytes),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
                services.AddAuthorization();

            #endregion

            return ApplicationStartup.Initialize(
                services, 
                this._configuration[OrdersConnectionString],
                cachingConfiguration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //AppDomain ad = AppDomain.CreateDomain("MyDomain");
            //ad.Load("MyAssembly");

            string urlPermisosCors = this._configuration["UrlCOR"];
            app.UseCors("_myAllowSpecificOrigins");
            /*app.UseCors(options =>
            {                
                options.WithOrigins(urlPermisosCors);
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });*/
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseProblemDetails();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            #region Habilita Autorizzacion JWT:
                app.UseAuthentication(); 
                app.UseAuthorization();
            #endregion

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwaggerDocumentation();
        }

    }
}
