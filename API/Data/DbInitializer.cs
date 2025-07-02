using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeIdentityAsync()
        {
            // Create roles if not exist
            if (!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Create default SuperAdmin and Admin
            if (!await _userManager.Users.AnyAsync())
            {
                var superAdminUser = new ApplicationUser
                {
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "1234567890",
                    UserAddress = new UserAddress
                    {
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
                    UserAddress = new UserAddress
                    {
                        City = "Admin",
                        Country = "Admin",
                        Street = "Admin",
                    }
                };

                await _userManager.CreateAsync(superAdminUser, "P@ssW0rd123");
                await _userManager.CreateAsync(adminUser, "P@ssW0rd123");

                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");

                // Add 50 normal users
                for (int i = 1; i <= 50; i++)
                {
                    var user = new ApplicationUser
                    {
                        DisplayName = $"User{i}",
                        Email = $"user{i}@mail.com",
                        UserName = $"user{i}",
                        PhoneNumber = $"0123456789{i % 10}",
                        UserAddress = new UserAddress
                        {
                            City = "City" + i,
                            Country = "Country" + i,
                            Street = "Street" + i,
                        }
                    };

                    var result = await _userManager.CreateAsync(user, "P@ssW0rd123");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                }
            }
        }
    }
}
