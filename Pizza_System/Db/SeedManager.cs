using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pizza_System.Model;

namespace Pizza_System.Db
{
    // create Manager user
    public static class SeedManager
    {
        public static async Task Seed (IServiceProvider services)
        {
            await SeedRoles(services);
            await SeedManagerUser(services);
        }

        // seed  roles to the database
        private static async Task SeedRoles(IServiceProvider services)
        {
            var roles = services.GetRequiredService<RoleManager<IdentityRole>>();

            
            /*IdentityResult identityResult =*/ await roles.CreateAsync(new IdentityRole(Role.Manager));
            await roles.CreateAsync(new IdentityRole(Role.Customer));
            //throw new NotImplementedException();
        }

        private static async Task SeedManagerUser(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            var _userManager = services.GetRequiredService<UserManager<User>>();
            var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            /*var managerUser = await context.Users.FirstOrDefaultAsync(user => user.Email == "Admin@hotmail.com");
            //User managerUser = new User();
            managerUser.FirstName = "Manager";
            managerUser.LastName = "Manager";
            managerUser.Email = "manager@hotmail.com";

            await _userManager.CreateAsync(managerUser, "Manager@hotmail.com");
            await _userManager.AddToRoleAsync(managerUser, Role.Manager);*/

            var managerUser = await context.Users.FirstOrDefaultAsync(user => user.Email == "manager@hotmail.com");

            if (managerUser is null)
            {
                managerUser = new User();
                managerUser.FirstName = "Manager";
                managerUser.LastName = "Manager";
                managerUser.UserName = "manager@hotmail.com";
                managerUser.Email = "manager@hotmail.com";

                IdentityResult result = await _userManager.CreateAsync(managerUser, "manager@hotmail.com");
                IdentityResult secondResult = await _userManager.AddToRoleAsync(managerUser, Role.Manager);

                /*{ UserName = "AuthenticationAdmin", Email = "your@email.com" };
                await userManager.CreateAsync(adminUser, "VerySecretPassword!1");
                await userManager.AddToRoleAsync(adminUser, Role.Admin);*/
            }
            
        }
    }
}
