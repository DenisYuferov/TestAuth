using Microsoft.AspNetCore.Identity;

using TestAuth.Domain.Abstraction.UnitOfWorks;

namespace TestAuth.Infrastructure.PostgreDb.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManager<IdentityUser> UserManager => _userManager;
        public SignInManager<IdentityUser> SignInManager => _signInManager;
        public RoleManager<IdentityRole> RoleManager => _roleManager;

        public UnitOfWork(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException();
        }
    }
}
