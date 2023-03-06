using Microsoft.AspNetCore.Identity;

namespace TestAuth.Domain.Abstraction.UnitOfWorks
{
    public interface IUnitOfWork
    {
        UserManager<IdentityUser> UserManager { get; }
        SignInManager<IdentityUser> SignInManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
    }
}
