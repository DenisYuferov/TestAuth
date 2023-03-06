using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestAuth.Domain.Abstraction.Providers;
using TestAuth.Domain.Providers;
using TestAuth.Domain.Validators;

namespace TestAuth.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTestAuthDomain(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddValidatorsFromAssembly(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            //services.AddAutoMapper(typeof(Mapper));

            services.AddTransient<IJwtProvider, JwtProvider>();

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        }
    }
}