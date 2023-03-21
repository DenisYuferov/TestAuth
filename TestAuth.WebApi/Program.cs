using SharedCore.Infrastructure.Extensions;

using TestAuth.Infrastructure;

namespace TestAuth.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddSharedInfrastructure(AppDomain.CurrentDomain.Load("TestAuth.Domain"));
            builder.AddTestAuthInfrastructure();

            var app = builder.Build();

            app.UseTestAuthInfrastructure();
            app.UseSharedInfrastructure();

            app.Run();
        }
    }
}