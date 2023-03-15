using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.Options;
using TestAuth.Infrastructure.DbContexts;
using TestAuth.Infrastructure.Seeds;

using Tion.Map.AspNetCore.Exceptions;

namespace TestAuth.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseTestAuthInfrastructure(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<ExceptionMiddleware>();

            UseDatabaseIfrastructure(appBuilder);
        }

        private static void UseDatabaseIfrastructure(IApplicationBuilder appBuilder)
        {
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AuthDbContext>();
                context?.Database.Migrate();

                var unitOfWork = serviceScope.ServiceProvider.GetService<IUnitOfWork>();
                var options = serviceScope.ServiceProvider.GetService<IOptions<DatabaseOptions>>();

                Seed.AddData(unitOfWork, options?.Value);
            }
        }
    }
}