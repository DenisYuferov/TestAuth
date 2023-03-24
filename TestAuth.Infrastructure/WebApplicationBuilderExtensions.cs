using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
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

            AddRedis(builder);

            AddProviders(builder);
        }

        private static void AddRedis(WebApplicationBuilder builder)
        {
            var section = builder.Configuration.GetSection(nameof(RedisCacheOptions));
            builder.Services.Configure<RedisCacheOptions>(section);

            var options = section.Get<RedisCacheOptions>();

            builder.Services.AddStackExchangeRedisCache(options => options.Configuration = options.Configuration);
        }

        private static void AddProviders(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRedisCacheProvider, RedisCacheProvider>();
            builder.Services.AddScoped<IIdentityUserProvider, IdentityUserProvider>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
        }
    }
}