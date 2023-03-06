using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TestAuth.Infrastructure.DbContexts
{
    // Add-Migration Initial -StartupProject TestAuth.WebApi -Project TestAuth.Infrastructure
    public class AuthDbContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public AuthDbContext(
            DbContextOptions<AuthDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
