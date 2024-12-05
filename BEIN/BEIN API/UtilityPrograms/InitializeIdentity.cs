using BEIN_DL.Data;
using BEIN_DL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BEIN_API.UtilityPrograms
{
    internal static class InitializeIdentity
    {
        internal static async Task InitializeIdentityAsync(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "System Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            if (await userManager.FindByEmailAsync("Lukhanyo@gmail.com") == null)
            {
                var admin = new IdentityUser
                {
                    UserName = "Lukhanyo_Mayekiso",
                    Email = "Lukhanyo@gmail.com",
                    PhoneNumber = "0739002497"
                };

                
                var result = await userManager.CreateAsync(admin, "Admin101!");
                if (!result.Succeeded)
                {
                    List<string> errors = [];
                    result.Errors.ToList().ForEach(error => errors.Add($"{error.Code}: {error.Description}"));
                    Console.WriteLine("\n\nAdmin account creation failed.\nErrors: " + string.Join("\n", errors) + "\n\n");
                }
                
                result = await userManager.AddToRoleAsync(admin, "System Admin");
                if (!result.Succeeded) Console.WriteLine("Failed to add user to the admin role." + string.Join("\n", result.Errors));

                var context = scope.ServiceProvider.GetRequiredService<BeinDbContext>();
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "Lukhanyo@gmail.com");
                if (user is null)
                {
                    await context.AddAsync(new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Lukhanyo",
                        Surname = "Mayekiso",
                        Email = "Lukhanyo@gmail.com",
                        PhoneNumber = "0739002497",
                        Profession = "Admin",
                        YearsOfExperience = 0
                    });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
