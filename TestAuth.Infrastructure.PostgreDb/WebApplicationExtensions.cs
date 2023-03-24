using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.PostgreDb.Options;
using TestAuth.Infrastructure.PostgreDb.Contexts;
using TestAuth.Infrastructure.PostgreDb.Seeds;

namespace TestAuth.Infrastructure.PostgreDb
{
    public static class WebApplicationExtensions
    {
        public static void UsePostgreDbInfrastructure(this WebApplication application)
        {
            using (var serviceScope = application.Services.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AuthDbContext>();
                context?.Database.Migrate();

                var unitOfWork = serviceScope.ServiceProvider.GetService<IUnitOfWork>();
                var options = serviceScope.ServiceProvider.GetService<IOptions<PostgreDbOptions>>();

                Seed.AddData(unitOfWork, options?.Value);
            }
        }
    }
}