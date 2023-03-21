using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.PostgreDb.Options;
using TestAuth.Infrastructure.PostgreDb.Contexts;
using TestAuth.Infrastructure.PostgreDb.UnitOfWorks;

namespace TestAuth.Infrastructure.PostgreDb
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddPostgreDbInfrastructure(this WebApplicationBuilder builder)
        {
            AddDatabaseInfrastructure(builder.Services, builder.Configuration);
        }

        private static void AddDatabaseInfrastructure(IServiceCollection services, IConfiguration configuration)
        {
            var databaseSection = configuration.GetSection(DatabaseOptions.Database);
            services.Configure<DatabaseOptions>(databaseSection);

            var databaseOptions = databaseSection.Get<DatabaseOptions>();
            services.AddDbContext<AuthDbContext>(opt => opt.UseNpgsql(databaseOptions?.Connection));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>();
        }
    }
}