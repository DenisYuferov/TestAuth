using Microsoft.Extensions.DependencyInjection.Extensions;

using TestAuth.Domain;
using TestAuth.Infrastructure;
using TestAuth.Infrastructure.Loggers;

namespace TestAuth.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, JsonLoggerProvider>());

            builder.Services.AddTestAuthInfrastructure(builder.Configuration);
            builder.Services.AddTestAuthDomain();

            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwagger();

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            app.UseTestAuthInfrastructure();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapControllers();

            app.UseHealthChecks("/health");

            app.Run();
        }
    }
}