using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.Options;
using TestAuth.Infrastructure.DbContexts;
using TestAuth.Infrastructure.UnitOfWorks;

namespace TestAuth.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTestAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDatabaseInfrastructure(services, configuration);

            AddIdentityServerInfrastructure(services);

            AddAuthentication(services, configuration);

            services.AddAuthorization();
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                                  "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                                  "Example: \"Bearer 1safsfsdfdfd\""
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference {  Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                        new string[] {}
                    }
                });
            });
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection(JwtOptions.Jwt);
            services.Configure<JwtOptions>(jwtSection);

            var jwtOptions = jwtSection.Get<JwtOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = jwtOptions?.Audience,
                        ValidIssuer = jwtOptions?.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.SecurityKey!))
                    };
                });
        }

        private static void AddDatabaseInfrastructure(IServiceCollection services, IConfiguration configuration)
        {
            var databaseSection = configuration.GetSection(DatabaseOptions.Database);
            services.Configure<DatabaseOptions>(databaseSection);

            var databaseOptions = databaseSection.Get<DatabaseOptions>();
            services.AddDbContext<AuthDbContext>(opt => opt.UseNpgsql(databaseOptions?.Connection));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddIdentityServerInfrastructure(IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<IdentityUser, AuthDbContext>();
        }
    }
}