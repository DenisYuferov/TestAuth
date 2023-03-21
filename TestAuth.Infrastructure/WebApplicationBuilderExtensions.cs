using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using SharedCore.Domain.Abstraction.Providers;
using SharedCore.Infrastructure.Providers;

using TestAuth.Infrastructure.PostgreDb;

namespace TestAuth.Infrastructure
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddTestAuthInfrastructure(this WebApplicationBuilder builder)
        {
            builder.AddPostgreDbInfrastructure();

            AddProviders(builder);
        }

        private static void AddProviders(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IJwtProvider, JwtProvider>();
        }
    }
}