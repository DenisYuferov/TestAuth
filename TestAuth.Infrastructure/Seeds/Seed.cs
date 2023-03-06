using Microsoft.AspNetCore.Identity;
using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.Options;
using TestAuth.Domain.Model.Seeds;

namespace TestAuth.Infrastructure.Seeds
{
    public static class Seed
    {
        public static void AddData(IUnitOfWork? unitOfWork, DatabaseOptions? options)
        {
            foreach (var seedUser in options?.SeedUsers!)
            {
                CreateUser(unitOfWork, seedUser);
            }
        }

        private static void CreateUser(IUnitOfWork? unitOfWork, SeedUser seedUser)
        {
            var identityUser = unitOfWork?.UserManager.FindByEmailAsync(seedUser.Email!).Result;
            if (identityUser == null)
            {
                CreateRolesIfNotExist(unitOfWork,seedUser.Roles!);

                identityUser = new IdentityUser { Email = seedUser.Email, UserName = seedUser.Name };

                var result = unitOfWork?.UserManager.CreateAsync(identityUser, seedUser.Password!).Result;
                CheckIdentityResult(result, "User", seedUser.Name!);

                result =  unitOfWork?.UserManager.AddToRolesAsync(identityUser, seedUser.Roles!).Result;
                CheckIdentityResult(result, "Roles", string.Join(", ",seedUser.Roles!));
            }
        }

        private static void CreateRolesIfNotExist(IUnitOfWork? unitOfWork, List<string> roles)
        {
            foreach (var role in roles)
            {
                var identityRole = unitOfWork?.RoleManager.FindByNameAsync(role).Result;
                if (identityRole == null)
                {
                    identityRole = new IdentityRole { Name = role };

                    var result = unitOfWork?.RoleManager.CreateAsync(identityRole).Result;
                    CheckIdentityResult(result, "Role", role);
                }
            }
        }

        private static void CheckIdentityResult(IdentityResult? result, string identity, string identityName)
        {
            if (result?.Succeeded != true)
            {
                var errors = result?.Errors.Select(e => $"{Environment.NewLine}{e.Code} {e.Description}");
                var errorsMessage = string.Join(string.Empty, errors!);

                throw new Exception($"Error for {identity} {identityName}: {errorsMessage}");
            }
        }
    }
}
