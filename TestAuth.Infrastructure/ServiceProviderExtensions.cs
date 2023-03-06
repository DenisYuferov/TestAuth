using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.Options;
using TestAuth.Infrastructure.DbContexts;
using TestAuth.Infrastructure.Seeds;

namespace TestAuth.Infrastructure
{
    public static class ServiceProviderExtensions
    {
        public static void UseTestAuthInfrastructure(this IServiceProvider serviceProvider)
        {           
            using (var serviceScope = serviceProvider.CreateScope())
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