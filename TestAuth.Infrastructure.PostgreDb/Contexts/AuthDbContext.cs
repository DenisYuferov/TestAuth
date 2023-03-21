using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestAuth.Infrastructure.PostgreDb.Contexts
{
    // Add-Migration Initial -StartupProject TestAuth.WebApi -Project TestAuth.Infrastructure.PostgreDb
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {

        }
    }
}