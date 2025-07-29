using Microsoft.AspNetCore.Identity;
using Orpheus.Data.Models;

namespace Orpheus.Areas.Identity.Data
{
    public static class IdentitySeedData
    {
        private const string AdminEmail = "admin@orpheus.com";
        private const string AdminPassword = "Admin123!";

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<OrpheusAppUser>>();

            string[] roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var admin = await userManager.FindByEmailAsync(AdminEmail);
            if (admin == null)
            {
                var newAdmin = new OrpheusAppUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
                else
                {
                    throw new Exception("Admin creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}