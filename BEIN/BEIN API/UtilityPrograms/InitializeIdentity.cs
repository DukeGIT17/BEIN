using BEIN_DL.Data;
using BEIN_DL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Console;
using Shared_Library.GlobalUtilities;

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
            string? errorMessage = null;

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

                    Claim[] claims =
                    [
                        new(ClaimTypes.Name, admin.Name),
                        new(ClaimTypes.Surname, admin.Surname),
                        new("username", $"{admin.Name} {admin.Surname}"),
                        new(ClaimTypes.Email, admin.Email),
                        new(ClaimTypes.MobilePhone, admin.PhoneNumber!),
                        new(ClaimTypes.Role, "Admin")
                    ];

                    if (!StaticUtilites.IdentityOutcome(await userManager.CreateAsync(sysAdmin, "Admin101!"), out errorMessage)) WriteLine(errorMessage);
                    if (!StaticUtilites.IdentityOutcome(await userManager.AddClaimsAsync(sysAdmin, claims), out errorMessage)) WriteLine(errorMessage);
                    if (!StaticUtilites.IdentityOutcome(await userManager.AddToRoleAsync(sysAdmin, "System Admin"), out errorMessage)) WriteLine(errorMessage);

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
