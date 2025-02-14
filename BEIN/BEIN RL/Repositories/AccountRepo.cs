using BEIN_DL.Data;
using BEIN_DL.Models;
using BEIN_DL.Infrastructure;
using BEIN_RL.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Shared_Library.GlobalUtilities;

namespace BEIN_RL.Repositories
{
    public class AccountRepo(BeinDbContext context, SignInManager<IdentityUser> signInManager, JwtUtility jwtUtility) : IAccount
    {
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> RegisterUserAsync(RegistrationModel model)
        {
            User? user = null;
            IdentityUser? newUser = null;
            string? errorMessage = null;
            try
            {
                user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user is not null || await signInManager.UserManager.FindByEmailAsync(model.Email) is not null )
                    throw new($"A user with the specified email address '{model.Email}' already exists.");

                newUser = new IdentityUser
                {
                    UserName = $"{model.Name}_{model.Surname}",
                    Email = model.Email,
                };

                Claim[] claims =
                [
                    new(ClaimTypes.Name, model.Name),
                    new(ClaimTypes.Surname, model.Surname),
                    new("username", $"{model.Name} {model.Surname}"),
                    new(ClaimTypes.Email, model.Email),
                    new(ClaimTypes.Role, "User")
                ];

                if (OutcomeUtilities.IdentityOutcome(await signInManager.UserManager.CreateAsync(newUser, model.Password), out errorMessage)) throw new(errorMessage);
                if (OutcomeUtilities.IdentityOutcome(await signInManager.UserManager.AddClaimsAsync(newUser, claims), out errorMessage)) throw new(errorMessage);
                if (OutcomeUtilities.IdentityOutcome(await signInManager.UserManager.AddToRoleAsync(newUser, "User"), out errorMessage)) throw new(errorMessage);

                await context.AddAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Profession = "User",
                    YearsOfExperience = 0
                });
                await context.SaveChangesAsync();

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                if (newUser is not null)
                    await signInManager.UserManager.DeleteAsync(newUser);

                user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user is not null)
                {
                    context.Remove(user);
                    await context.SaveChangesAsync();
                }

                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> SignInAsync(SignInModel model)
        {
            try
            {
                var user = await signInManager.UserManager.FindByEmailAsync(model.Email);
                if (user is null) throw new($"Could not find a user with the email address '{model.Email}'.");

                var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded) throw new("User log in failed.");

                var userData = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (userData is null) throw new($"Could not find user with the email address '{user.Email}'.");

                var claims = await signInManager.UserManager.GetClaimsAsync(user);

                _returnDictionary["Success"] = true;
                _returnDictionary["Token"] = jwtUtility.GenerateToken(userData);
                _returnDictionary["UserClaims"] = claims.Select(c => new ClaimDto
                {
                    Type = c.Type,
                    Value = c.Value
                });
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }
    }
}
