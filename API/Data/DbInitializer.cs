
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DbInitializer
        ( UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager)
    {
        public async Task InitializeIdentityAsync()
        {
            //if ((await _identityContext.Database.GetPendingMigrationsAsync()).Any())
            //    await _identityContext.Database.MigrateAsync();

            if(! await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            if (!await _userManager.Users.AnyAsync()) 
            {
                var superAdminUser = new ApplicationUser
                {
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "1234567890",    
                    UserAddress = new UserAddress {
                        City = "SuperAdmin",
                        Country = "SuperAdmin",
                        Street = "SuperAdmin",
                    }
                };
                var adminUser = new ApplicationUser
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "1234567890",    
                    UserAddress = new UserAddress {
                        City = "Admin",
                        Country = "Admin",
                        Street = "Admin",
                    }
                };

                await _userManager.CreateAsync(superAdminUser,"P@ssW0rd123");
                await _userManager.CreateAsync(adminUser, "P@ssW0rd123");

                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

    }
}

