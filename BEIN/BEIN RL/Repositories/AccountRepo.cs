using BEIN_DL.Data;
using BEIN_DL.Models;
using BEIN_RL.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BEIN_RL.Repositories
{
    public class AccountRepo(BeinDbContext context, SignInManager<IdentityUser> signInManager) : IAccount
    {
        private Dictionary<string, object> _returnDictionary = [];

        public async Task<Dictionary<string, object>> RegisterUserAsync(RegistrationModel model)
        {
            User? user = null;
            IdentityUser? newUser = null;
            IdentityResult? result = null;
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

                result = await signInManager.UserManager.CreateAsync(newUser, model.Password);
                if (!result.Succeeded)
                {
                    string error = "";
                    result.Errors.ToList().ForEach(e => error = $"{e.Code}: {e.Description}");
                    throw new(error);
                }

                result = await signInManager.UserManager.AddToRoleAsync(newUser, "User");
                if (!result.Succeeded)
                {
                    string error = "";
                    result.Errors.ToList().ForEach(e => error = $"{e.Code}: {e.Description}");
                    throw new(error);
                }

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

                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (!result.Succeeded) throw new("User log in failed.");

                _returnDictionary["Success"] = true;
                return _returnDictionary;
            }
            catch (Exception ex)
            {
                _returnDictionary["Success"] = false;
                _returnDictionary["ErrorMessage"] = ex.Message + "\nInner Exception: " + ex.InnerException;
                return _returnDictionary;
            }
        }

        public async Task<Dictionary<string, object>> SignOutAsync()
        {
            try
            {
                await signInManager.SignOutAsync();

                _returnDictionary["Success"] = true;
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
