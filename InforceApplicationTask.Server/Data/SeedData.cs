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
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (await userManager.Users.AnyAsync() || await roleManager.Roles.AnyAsync() || await context.About.AnyAsync()) return;

                var about = new About
                {
                    Id = Guid.NewGuid(),
                    Description = "URL Shortener algorithm works in the following way:\r\n\r\nProgram checks whether this URL has its short code equivalent in the database; if it does, API returns a 400 status code.\r\n\r\nProgram generates a random combination of 10 possible characters, particularly:\r\n\r\n'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'\r\n\r\nProgram checks whether this combination exists; if it does, the program generates one more combination. It lasts until the generated combination does not appear in the database. If it doesn't, then the program adds a new entity with the requested original URL and its short code equivalent.",
                    CreatedAt = DateTime.UtcNow,
                };

                await context.About.AddAsync(about);

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
