using InforceApplicationTask.Server.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InforceApplicationTask.Server.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (await userManager.Users.AnyAsync() || await roleManager.Roles.AnyAsync()) return;

                var roles = new[] { UserRoles.Admin, UserRoles.User };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var adminUser = new ApplicationUser
                {
                    UserName = "xpert",
                    Email = "testemailfortask@gmail.com",
                    Role = UserRoles.Admin,
                };

                var normalUser = new ApplicationUser
                {
                    UserName = "testuser",
                    Email = "exampletestemail@gmail.com",
                    Role = UserRoles.User
                };

                if (userManager.Users.All(u => u.UserName != adminUser.UserName))
                {
                    await userManager.CreateAsync(adminUser, "abobus123");
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                }

                if (userManager.Users.All(u => u.UserName != normalUser.UserName))
                {
                    await userManager.CreateAsync(normalUser, "cool321");
                    await userManager.AddToRoleAsync(normalUser, UserRoles.User);
                }
            }
        }
    }
}
