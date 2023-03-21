using Microsoft.AspNetCore.Builder;

using TestAuth.Infrastructure.PostgreDb;

namespace TestAuth.Infrastructure
{
    public static class WebApplicationExtensions
    {
        public static void UseTestAuthInfrastructure(this WebApplication application)
        {
            application.UsePostgreDbInfrastructure();
        }
    }
}