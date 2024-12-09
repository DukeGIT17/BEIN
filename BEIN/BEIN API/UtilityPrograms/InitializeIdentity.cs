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
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = scope.ServiceProvider.GetRequiredService<BeinDbContext>();

            var roles = new[] { "System Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            User[] admins =
            [
                new User 
                {
                    Name = "Lukhanyo",
                    Surname = "Mayekiso",
                    Email = "Lukhanyo@gmail.com",
                    PhoneNumber = "0739002497"
                },
                new User 
                {
                    Name = "Thabiso",
                    Surname = "Soaisa",
                    Email = "Thabiso@gmail.com",
                    PhoneNumber = "078958568"
                },

            ];

            foreach (var admin in admins)
            {
                if (await userManager.FindByEmailAsync(admin.Email) == null)
                {
                    var sysAdmin = new IdentityUser
                    {
                        UserName = $"{admin.Name}_{admin.Surname}",
                        Email = admin.Email,
                        PhoneNumber = admin.PhoneNumber
                    };


                    var result = await userManager.CreateAsync(sysAdmin, "Admin101!");
                    if (!result.Succeeded)
                    {
                        List<string> errors = [];
                        result.Errors.ToList().ForEach(error => errors.Add($"{error.Code}: {error.Description}"));
                        Console.WriteLine("\n\nAdmin account creation failed.\nErrors: " + string.Join("\n", errors) + "\n\n");
                    }

                    result = await userManager.AddToRoleAsync(sysAdmin, "System Admin");
                    if (!result.Succeeded) Console.WriteLine("Failed to add user to the admin role." + string.Join("\n", result.Errors));

                    var user = await context.Users.FirstOrDefaultAsync(u => u.Email == admin.Email);
                    if (user is null)
                    {
                        await context.AddAsync(new User
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = admin.Name,
                            Surname = admin.Surname,
                            Email = admin.Email,
                            PhoneNumber = admin.PhoneNumber,
                            Profession = "Admin",
                            YearsOfExperience = 0
                        });
                    }
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
